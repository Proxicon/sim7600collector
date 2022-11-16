
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace sim7600collector.Models;

public class SimData
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required]
    public string? Device { get; set; }
    [Required]
    public string? Location { get; set; }
    public string? Battery { get; set; }
    public int Signal { get; set; }
}