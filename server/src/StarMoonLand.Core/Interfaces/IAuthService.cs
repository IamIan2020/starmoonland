namespace StarMoonLand.Core.Interfaces;

public interface IAuthService
{
    Task<(bool Success, string? AccessToken, string? RefreshToken, object? User, string? Error)> LoginAsync(string email, string password);
    Task<(bool Success, string? AccessToken, string? RefreshToken, string? Error)> RefreshTokenAsync(string refreshToken);
}
