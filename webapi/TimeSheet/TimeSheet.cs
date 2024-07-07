using System.Text.Json.Serialization;

public class TimeSheet
{
    public int Id { get; set; }
    public int Employee { get; set; }
    public int Reason { get; set; }
    public DateTime StartDate { get; set; }
    public int Duration { get; set; }
    public bool Discounted { get; set; }
    public string Description { get; set; }

    [JsonConstructor]
    public TimeSheet(int id, int employee, int reason, DateTime startDate, int duration, bool discounted, string description)
    {
        Id = id;
        Employee = employee;
        Reason = reason;
        StartDate = startDate;
        Duration = duration;
        Discounted = discounted;
        Description = description;
    }
}