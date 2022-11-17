namespace sim7600collector.Models.DTOs;

public class SimLogsDto
{
    public int Id { get; set; }
    public string? Device { get; set; }
    public string? Logitem { get; set; }
    public string? Message { get; set; }

    public SimLogsDto() { }
    public SimLogsDto(SimLogs simLogs) =>
    (Id, Device, Logitem, Message) = (simLogs.Id,
                                      simLogs.Device,
                                      simLogs.Logitem,
                                      simLogs.Message);
}
