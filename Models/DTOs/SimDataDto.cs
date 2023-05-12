using Microsoft.AspNetCore.Authorization;

namespace sim7600collector.Models.DTOs;

public class SimDataDto
{
    public int Id { get; set; }
    public string? Device { get; set; }
    public string? Location { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
    public double? DecimalLatitude { get; set; }
    public double? DecimalLongitude { get; set; }
    public DateTime? Date { get; set; }
    public TimeSpan? UTCTime { get; set; }
    public double? Altitude { get; set; }
    public double? SpeedKnots { get; set; }
    public double? SpeedKmph { get; set; }
    public double? SpeedMph { get; set; }
    public double? Course { get; set; }

    public SimDataDto() { }

    public SimDataDto(SimData Sim7600DataItem) =>
    (Id, Device, Location ,Latitude, Longitude, Date, UTCTime, Altitude, SpeedKnots, SpeedKmph, SpeedMph, Course) =
    (Sim7600DataItem.Id, Sim7600DataItem.Device, Sim7600DataItem.Location, Sim7600DataItem.Latitude, Sim7600DataItem.Longitude,
     Sim7600DataItem.Date, Sim7600DataItem.UTCTime, Sim7600DataItem.Altitude, Sim7600DataItem.SpeedKnots,
     Sim7600DataItem.SpeedKmph, Sim7600DataItem.SpeedMph, Sim7600DataItem.Course);
}
