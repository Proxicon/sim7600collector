using System.ComponentModel.DataAnnotations;

namespace sim7600collector.Models;

public class User
{
    public int Id { get; set; }
    [Required]
    public string? Username { get; set; }
    [Required]
    public string? Password { get; set; }
    public DateTime CreatedOn { get; set; }
    public string? Email { get; set; }
}