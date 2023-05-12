
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
    public string? Latitude { get; internal set; }
    public string? Longitude { get; internal set; }
    public Double DecimalLatitude { get; internal set; }
    public Double DecimalLongitude { get; internal set; }
    public Double Altitude { get; internal set; }
    public Double Course { get; internal set; }
    public Double SpeedKnots { get; internal set; }
    public Double SpeedKmph { get; internal set; }
    public Double SpeedMph { get; internal set; }
    public DateTime Date { get; internal set; }
    public TimeSpan UTCTime { get; internal set; }
    public DateTime DateTime { get; internal set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public DateTime CreatedAt { get; set; }

}