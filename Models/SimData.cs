
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
    public string Latitude { get; internal set; }
    public string Longitude { get; internal set; }
    public string Date { get; internal set; }
    public string UTCTime { get; internal set; }
    public string Altitude { get; internal set; }
    public string Speed { get; internal set; }
    public string Course { get; internal set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }

}