using System.Security.Claims;
using diploma_server.Account;
using diploma_server.Account.Dto;
using diploma_server.Repository;
using diploma_server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace diploma_server.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    public UserController(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto registerUserDto)
    {
        var user = new User
        {
            Username = registerUserDto.Username,
            Password = registerUserDto.Password,
            
        };
        user.Token = JwtTokenGenerator.GenerateJwtToken(user, _configuration);
        await _userRepository.AddUserAsync(user);
        return Ok(new { Token = user.Token });
    }

    [HttpPost("authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] AuthenticateUserDto authenticateUserDto)
    {
        var user = await _userRepository.GetUserByUsernameAsync(authenticateUserDto.Username);

        if (user == null || user.Password != authenticateUserDto.Password) // Обязательно хэшируйте и проверяйте хэш пароля
            return Unauthorized("Invalid username or password");

        // Генерация JWT-токена (логика создания токена должна быть реализована)
        user.Token = JwtTokenGenerator.GenerateJwtToken(user, _configuration);

        await _userRepository.UpdateUserAsync(user);
        return Ok(new { Token = user.Token });
    }

    [Authorize]
    [HttpPut("update")]
    public async Task<IActionResult> Update([FromBody] UpdateUserDto updateUserDto)
    {
        var username = _httpContextAccessor.HttpContext.User.Identity.Name;

        if (username != updateUserDto.Username)
            return Forbid();

        var user = await _userRepository.GetUserByUsernameAsync(updateUserDto.Username);
        if (user == null)
            return NotFound("User not found");

        user.Password = updateUserDto.Password; // Обязательно хэшируйте пароли в реальных приложениях!
        user.ProfilePhotoUrl = updateUserDto.ProfilePhotoUrl;

        await _userRepository.UpdateUserAsync(user);

        return Ok("User updated successfully");
    }

    [Authorize]
    [HttpDelete("delete")]
    public async Task<IActionResult> Delete()
    {
        var username = _httpContextAccessor.HttpContext.User.Identity.Name;

        await _userRepository.DeleteUserAsync(username);
        return Ok("User deleted successfully");
    }

    
}
