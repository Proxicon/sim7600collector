using System.ComponentModel.DataAnnotations;

namespace sim7600collector.Models;

public class SimLogs
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required]
    public string? Device { get; set; }
    [Required]
    public string? Logitem { get; set; }
    [Required]
    public string? Message { get; set; }
}