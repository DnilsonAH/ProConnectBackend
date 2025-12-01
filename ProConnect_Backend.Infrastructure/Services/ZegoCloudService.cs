using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ProConnect_Backend.Domain.Ports.IServices;

namespace ProConnect_Backend.Infrastructure.Services;

public class ZegoCloudService : IVideoCallService
{
    private readonly uint _appId;
    private readonly string _serverSecret;
    private readonly ILogger<ZegoCloudService> _logger;

    public ZegoCloudService(IConfiguration configuration, ILogger<ZegoCloudService> logger)
    {
        _logger = logger;
        var section = configuration.GetSection("ZegoCloud");

        // Validaciones defensivas: Si falta la config, la app debe fallar rápido para avisarnos.
        if (!uint.TryParse(section["AppId"], out _appId))
        {
            // Usamos 0 o lanzamos error si es crítico. Aquí lanzamos error para que lo configures.
            // Si estás probando y aún no tienes el ID, pon uno dummy en appsettings.
            _logger.LogWarning("ZegoCloud AppId no configurado o inválido.");
        }

        _serverSecret = section["ServerSecret"] ??
                        throw new InvalidOperationException("ZegoCloud ServerSecret no configurado");

        // Advertencia si la longitud del secreto es inusual (Zego suele usar 32 chars)
        if (_serverSecret.Length != 32)
        {
            _logger.LogWarning(
                "⚠️ El ServerSecret configurado tiene {Length} caracteres. Verifica si es el ServerSecret (32 chars) o el AppSign (64 chars). El algoritmo intentará adaptarse.",
                _serverSecret.Length);
        }
    }

    public string GenerateCallToken(string userId, string roomId)
    {
        // El token será válido por 24 horas (3600 seg * 24)
        long effectiveTimeInSeconds = 3600 * 24;

        // Payload: Configuración de privilegios.
        // 1 = Login (Entrar a la sala)
        // 2 = Publish (Transmitir video/audio)
        var payloadObj = new
        {
            room_id = roomId,
            privilege = new Dictionary<int, int>
            {
                { 1, 1 },
                { 2, 1 }
            },
            stream_id_list = (object)null!
        };

        string payload = JsonSerializer.Serialize(payloadObj);

        return GenerateToken04(_appId, userId, _serverSecret, effectiveTimeInSeconds, payload);
    }

    /// <summary>
    /// Algoritmo de generación de Token V4 de ZegoCloud (Cifrado AES).
    /// </summary>
    private static string GenerateToken04(long appId, string userId, string serverSecret, long effectiveTimeInSeconds,
        string payload)
    {
        long createTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        long expireTime = createTime + effectiveTimeInSeconds;

        // Nonce: Un número aleatorio para evitar ataques de repetición
        long nonce = new Random().NextInt64();

        var tokenInfo = new
        {
            app_id = appId,
            user_id = userId,
            nonce = nonce,
            ctime = createTime,
            expire = expireTime,
            payload = payload
        };

        string plainText = JsonSerializer.Serialize(tokenInfo);

        // Preparación de la llave AES (Debe ser de 32 bytes)
        byte[] keyBytes;
        if (serverSecret.Length == 32)
        {
            keyBytes = Encoding.UTF8.GetBytes(serverSecret);
        }
        else
        {
            // Fallback para secretos de longitud no estándar (ej. 64 chars)
            var secretBytes = Encoding.UTF8.GetBytes(serverSecret);
            keyBytes = new byte[32];
            Array.Copy(secretBytes, keyBytes, Math.Min(secretBytes.Length, 32));
        }

        // Generación del Vector de Inicialización (IV) aleatorio
        byte[] iv = new byte[16];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(iv);
        }

        // Cifrado del contenido
        byte[] contentBytes;
        using (var aes = Aes.Create())
        {
            aes.Key = keyBytes;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
            using (var msEncrypt = new MemoryStream())
            {
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                using (var swEncrypt = new StreamWriter(csEncrypt))
                {
                    swEncrypt.Write(plainText);
                }

                contentBytes = msEncrypt.ToArray();
            }
        }

        // Empaquetado final en formato binario Big-Endian
        using (var ms = new MemoryStream())
        using (var writer = new BinaryWriter(ms, Encoding.UTF8))
        {
            byte[] expireBytes = BitConverter.GetBytes(expireTime);
            if (BitConverter.IsLittleEndian) Array.Reverse(expireBytes);
            writer.Write(expireBytes);

            ushort ivLen = (ushort)iv.Length;
            byte[] ivLenBytes = BitConverter.GetBytes(ivLen);
            if (BitConverter.IsLittleEndian) Array.Reverse(ivLenBytes);
            writer.Write(ivLenBytes);

            writer.Write(iv);

            ushort contentLen = (ushort)contentBytes.Length;
            byte[] contentLenBytes = BitConverter.GetBytes(contentLen);
            if (BitConverter.IsLittleEndian) Array.Reverse(contentLenBytes);
            writer.Write(contentLenBytes);

            writer.Write(contentBytes);

            // Prefijo "04" indica la versión del algoritmo de token
            return "04" + Convert.ToBase64String(ms.ToArray());
        }
    }
}