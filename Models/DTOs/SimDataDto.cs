namespace sim7600collector.Models.DTOs;

public class SimDataDto
{
    public int Id { get; set; }
    public string? Device { get; set; }
    public string? Location { get; set; }
    public string? Battery { get; set; }
    public int Signal { get; set; }

    public SimDataDto() { }

    public SimDataDto(SimData Sim7600DataItem) =>
    (Id, Device, Location, Battery, Signal) = (Sim7600DataItem.Id,
                                               Sim7600DataItem.Device,
                                               Sim7600DataItem.Location,
                                               Sim7600DataItem.Battery,
                                               Sim7600DataItem.Signal);
}
