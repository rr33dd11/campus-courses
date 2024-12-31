using campus.DBContext.DTO.ReportDTO;
using campus.DBContext.Query;

namespace campus.Services.IServices;

public interface IReportService
{
    public Task<List<ReportRecord>> GenerateReport(ReportQuery reportParams, string token);
}