using System.ComponentModel.DataAnnotations;

namespace WestServiceIf.Models;

public class Product
{
    [Key]
    public int Id { get; set; }
    
    [StringLength(ushort.MaxValue, MinimumLength = 1)]
    public required string Title { get; set; }

    [StringLength(int.MaxValue, MinimumLength = 1)]
    public required string Description { get; set; }
}