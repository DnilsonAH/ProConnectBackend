using ProConnect_Backend.Domain.Ports.IServices;
using System;
namespace ProConnect_Backend.Infrastructure.Services;

public class TimeZoneConverter : ITimeZoneConverter
{
    public DateTime GetLocalTimeInColombiaPeru()
    {
        string timeZoneId = "America/Lima"; // O "SA Pacific Standard Time" para Windows

        try
        {
            TimeZoneInfo tzi = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
            var nowUtc = DateTime.UtcNow;
            DateTime localTime = TimeZoneInfo.ConvertTimeFromUtc(nowUtc, tzi);
            return localTime;
        }
        catch (TimeZoneNotFoundException)
        {
            Console.WriteLine("Zona horaria no encontrada, devolviendo UTC.");
            return DateTime.UtcNow;
        }
    }
}