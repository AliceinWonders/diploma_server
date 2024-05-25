using diploma_server.Account;

namespace diploma_server.Repository;

public interface IUserRepository
{
    Task<User> GetUserByUsernameAsync(string username);
    Task AddUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(string username);
}