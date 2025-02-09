using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models;

[Table("Portfolio")]
public class Portfolio
{
    public string AppUserId { get; set; } = string.Empty;

    public int StockId { get; set; }

    public AppUser AppUser { get; set; }

    public Stock Stock { get; set; }
}