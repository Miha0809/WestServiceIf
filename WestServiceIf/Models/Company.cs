using System.ComponentModel.DataAnnotations;

namespace WestServiceIf.Models;

public class Company
{
    [Key]
    public int Id { get; set; }
    
    [StringLength(int.MaxValue, MinimumLength = 1)]
    public string Name { get; set; }
    
    [StringLength(int.MaxValue)]
    public string? Description { get; set; }
}