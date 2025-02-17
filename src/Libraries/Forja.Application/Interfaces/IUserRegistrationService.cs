namespace Forja.Application.Interfaces;

public interface IUserRegistrationService
{
    Task RegisterUserAsync(RegisterUserCommand request);
}