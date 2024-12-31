using campus.AdditionalServices.Exceptions;
using campus.AdditionalServices.TokenHelpers;
using campus.DBContext;
using campus.DBContext.DTO.ReportDTO;
using campus.DBContext.Models.Enums;
using campus.DBContext.Query;
using campus.Services.IServices;
using Microsoft.EntityFrameworkCore;

namespace campus.Services;

public class ReportService : IReportService
{
    private readonly AppDBContext _db;
    private readonly TokenInteraction _tokenHelper;

    public ReportService(AppDBContext db, TokenInteraction tokenHelper)
    {
        _db = db;
        _tokenHelper = tokenHelper;
    }

    public async Task<List<ReportRecord>> GenerateReport(ReportQuery reportParams,string token)
    {
        var accountId = _tokenHelper.GetAccountIdFromToken(token);
        var account = await _db.Accounts.FirstOrDefaultAsync(acc => acc.Id == accountId);

        if (account == null) throw new UnauthorizedException("Пользователь не авторизован");
        if (!account.isAdmin) throw new ForbiddenException("У вас недостаточно прав");

        var queryCourses = _db.Teachers
            .Include(t => t.Course)
                .ThenInclude(c => c.Group)
            .Include(t => t.Course)
                .ThenInclude(c => c.Teachers)
            .Include(t => t.Account)
            .Where(t => t.IsMain)
            .AsQueryable();

        if (reportParams.semester.HasValue)
        {
            queryCourses = queryCourses.Where(t => t.Course.Semester == reportParams.semester.Value);
        }

        if (reportParams.campusGroupIds is { Count: > 0 })
        {
            queryCourses = queryCourses.Where(t => t.Course.GroupId == reportParams.campusGroupIds.First());
        }
        
        var reportRecords = await queryCourses
            .GroupBy(t => new { t.AccountId, t.Account.FullName })
            .Select(teacherGroup => new ReportRecord
            {
                fullName = teacherGroup.Key.FullName,
                id = teacherGroup.Key.AccountId,
                campusGroupReports = teacherGroup
                    .GroupBy(t => new { t.Course.Group.Id, t.Course.Group.Name })
                    .Select(groupByGroup => new CampusGroupReport
                    {
                        id = groupByGroup.Key.Id,
                        name = groupByGroup.Key.Name,
                        averagePassed = groupByGroup.Sum(c => c.Course.Students.Count(s => s.FinalResult == StudentMarks.Passed)) 
                                        / (double)groupByGroup.Sum(c => c.Course.Students.Count),
                        
                        averageFailed = groupByGroup.Sum(c => c.Course.Students.Count(s => s.FinalResult == StudentMarks.Failed)) 
                                        / (double)groupByGroup.Sum(c => c.Course.Students.Count),})
                    .ToList()
            })
            .OrderBy(r => r.fullName)
            .ToListAsync();
        
        return reportRecords;
    }
}