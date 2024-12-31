namespace campus.DBContext.Models.Enums;

using System.Text.Json.Serialization;
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Semesters
{
    Autumn,
    Spring
}