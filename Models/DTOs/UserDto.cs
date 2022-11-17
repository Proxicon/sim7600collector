using System.ComponentModel.DataAnnotations;

namespace sim7600collector.Models.DTOs;

public class UserDto
{
    public int Id { get; set; }
    [Required]
    public string? Username { get; set; }
    [Required]
    public string? Password { get; set; }
    public DateTime CreatedOn { get; set; }
    public string? Email { get; set; }

    public UserDto() { }
    public UserDto(User user) =>
        (Id, Username, Password, CreatedOn, Email) = (user.Id,
                                                      user.Username, 
                                                      user.Password, 
                                                      user.CreatedOn, 
                                                      user.Email);

}
