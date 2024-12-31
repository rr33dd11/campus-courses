using campus.DBContext.Models.Enums;

namespace campus.DBContext.Query;

public class ReportQuery
{
    public Semesters? semester { get; set; }
    public List<Guid>? campusGroupIds { get; set; }
}