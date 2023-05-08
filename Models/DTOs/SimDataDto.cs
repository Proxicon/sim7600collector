using Microsoft.AspNetCore.Authorization;

namespace sim7600collector.Models.DTOs;

public class SimDataDto
{
    public int Id { get; set; }
    public string? Device { get; set; }
    public string? Location { get; set; }
    public string? Latitude { get; set; }
    public string? Longitude { get; set; }
    public string? Date { get; set; }
    public string? UTCTime { get; set; }
    public string? Altitude { get; set; }
    public string? Speed { get; set; }
    public string? Course { get; set; }
    public string? Battery { get; set; }
    public int Signal { get; set; }

    public SimDataDto() { }

    public SimDataDto(SimData Sim7600DataItem) =>
    (Id, Device, Location ,Latitude, Longitude, Date, UTCTime, Altitude, Speed, Course, Battery, Signal) =
    (Sim7600DataItem.Id, Sim7600DataItem.Device, Sim7600DataItem.Location, Sim7600DataItem.Latitude, Sim7600DataItem.Longitude,
     Sim7600DataItem.Date, Sim7600DataItem.UTCTime, Sim7600DataItem.Altitude, Sim7600DataItem.Speed,
     Sim7600DataItem.Course, Sim7600DataItem.Battery, Sim7600DataItem.Signal);
}
