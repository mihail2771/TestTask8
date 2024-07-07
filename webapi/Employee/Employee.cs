using System.Text.Json.Serialization;

public class Employee
{
    public int Id { get; set; }
    public string LastName { get; set; }
    public string FirstName { get; set; }

    [JsonConstructor]
    public Employee(int id, string lastName, string firstName)
    {
        Id = id;
        LastName = lastName;
        FirstName = firstName;
    }
}