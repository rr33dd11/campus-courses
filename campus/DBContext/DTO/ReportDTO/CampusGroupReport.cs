namespace campus.DBContext.DTO.ReportDTO;

public class CampusGroupReport
{
    public string name { get; set; }
    public Guid id { get; set; }
    public double averagePassed { get; set; }
    public double averageFailed { get; set; }
}