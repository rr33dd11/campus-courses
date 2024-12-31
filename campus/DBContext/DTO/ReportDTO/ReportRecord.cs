namespace campus.DBContext.DTO.ReportDTO;

public class ReportRecord
{
    public string fullName { get; set; }
    public Guid id { get; set; }
    public List<CampusGroupReport> campusGroupReports { get; set; }
}