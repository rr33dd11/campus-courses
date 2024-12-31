namespace campus.DBContext.Models.Enums;

using System.Text.Json.Serialization;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CourseStatuses
{
    Created,
    OpenForAssigning,
    Started,
    Finished
}