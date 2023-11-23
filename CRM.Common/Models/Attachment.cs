using System.ComponentModel.DataAnnotations.Schema;

namespace CRM.Common.Models;

public class Attachment
{
    public int Id { get; set; }
    public string? FilePath { get; set; }

    [ForeignKey("Employee")]
    public Guid? EmployeeId { get; set; }
    public Employee? Employee { get; set; }
}