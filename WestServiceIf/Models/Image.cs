using System.ComponentModel.DataAnnotations.Schema;

namespace WestServiceIf.Models;

public class Image
{
    public int Id { get; set; }
    public string FileName { get; set; }
    public byte[] ImageData { get; set; }

    // Зовнішній ключ
    public int ProductId { get; set; }
    
    [ForeignKey("ProductId")]
    public virtual Product Product { get; set; }
}