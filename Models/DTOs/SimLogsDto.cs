namespace sim7600collector.Models.DTOs;

public class SimLogsDto
{
    public int Id { get; set; }
    public string? Device { get; set; }
    public string? Logitem { get; set; }
    public string? Message { get; set; }

    public SimLogsDto() { }
    public SimLogsDto(SimLogs Sim7600Logs) =>
    (Id, Device, Logitem, Message) = (Sim7600Logs.Id,
                                      Sim7600Logs.Device,
                                      Sim7600Logs.Logitem,
                                      Sim7600Logs.Message);
}
