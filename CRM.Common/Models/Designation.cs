using System.ComponentModel.DataAnnotations;

namespace CRM.Common.Models;

public class Designation
{

    [Key]
    public int Id { get; set; }
    public string? Title { get; set; }
    public List<Employee>? Employees { get; set; }
}