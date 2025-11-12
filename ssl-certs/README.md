# 🔐 Certificados SSL para MySQL

Esta carpeta contiene los certificados SSL necesarios para conectarse a Google Cloud SQL MySQL con autenticación de certificados de cliente.

## 📋 Archivos requeridos (opcionales)

Si deseas usar autenticación SSL con certificados de cliente, coloca los siguientes archivos en esta carpeta:

- `client-cert.pem` - Certificado del cliente
- `client-key.pem` - Llave privada del cliente
- `server-ca.pem` - Certificado de autoridad del servidor

## ⚙️ Comportamiento del sistema

### ✅ Si los certificados están presentes:
La aplicación se conectará usando **SSL con certificados de cliente** (autenticación mutua TLS) Esto indica de que estas en modo de desarrollo y no estas usando una base de datos de Produccion.

```
SslMode=Required;SslCa=server-ca.pem;SslCert=client-cert.pem;SslKey=client-key.pem
```

### ⚠️ Si los certificados NO están presentes:
La aplicación se conectará usando **SSL sin certificados de cliente** (solo cifrado de la conexión) Por ende, se sobreentiende de que la conexion se esta realizando desde un entorno de produccion con una base de datos con solo ssl requerido.

```
SslMode=Required
```

## 🔒 Seguridad

**IMPORTANTE:** Los archivos de certificados están excluidos del control de versiones (`.gitignore`).

**NUNCA** subas los certificados SSL al repositorio. Cada desarrollador debe obtener sus propios certificados desde:
- Google Cloud Console
- Administrador de base de datos
- Sistema de gestión de secretos (Azure Key Vault, AWS Secrets Manager, etc.)

## 📝 Notas

- La carpeta se crea automáticamente si no existe
- Los certificados son opcionales
- El sistema detecta automáticamente su presencia
- No es necesario reiniciar la aplicación al agregar/quitar certificados (se detectan al inicio)
