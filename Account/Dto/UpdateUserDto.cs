namespace diploma_server.Account.Dto;

public class UpdateUserDto
{
    public string Username { get; set; }
    public string Password { get; set; }
    public string? ProfilePhotoUrl { get; set; }
}