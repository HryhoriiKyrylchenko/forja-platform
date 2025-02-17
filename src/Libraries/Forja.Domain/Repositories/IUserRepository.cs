namespace Forja.Domain.Repositories;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<bool> ExistsByUsernameAsync(string username);
}