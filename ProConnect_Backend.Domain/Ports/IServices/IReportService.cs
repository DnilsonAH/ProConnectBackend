namespace ProConnect_Backend.Domain.Ports.IServices;

public interface IReportService
{
    /// <summary>
    /// Genera un reporte Excel (XLSX) con los profesionales y sus datos (categoría, profesión, especializaciones, contacto)
    /// y retorna el contenido como arreglo de bytes listo para enviar como archivo.
    /// </summary>
    Task<byte[]> GenerateProfessionalsReportAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Genera un reporte Excel (XLSX) con las sesiones y datos asociados (profesional, cliente, categoría del profesional, fechas, estado)
    /// </summary>
    Task<byte[]> GenerateSessionsReportAsync(CancellationToken cancellationToken = default);
}
