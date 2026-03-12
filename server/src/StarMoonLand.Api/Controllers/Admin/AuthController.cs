using Microsoft.AspNetCore.Mvc;
using StarMoonLand.Api.DTOs;
using StarMoonLand.Api.DTOs.Auth;
using StarMoonLand.Core.Interfaces;

namespace StarMoonLand.Api.Controllers.Admin;

[ApiController]
[Route("api/admin/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var result = await _authService.LoginAsync(request.Email, request.Password);

        if (!result.Success)
            return Unauthorized(ApiResponse<object>.Fail(result.Error!));

        var userObj = result.User!;
        var type = userObj.GetType();
        var response = new LoginResponse
        {
            AccessToken = result.AccessToken!,
            RefreshToken = result.RefreshToken!,
            User = new UserInfo
            {
                Id = type.GetProperty("id")?.GetValue(userObj)?.ToString() ?? "",
                Email = type.GetProperty("email")?.GetValue(userObj)?.ToString() ?? "",
                DisplayName = type.GetProperty("displayName")?.GetValue(userObj)?.ToString() ?? "",
            }
        };

        return Ok(ApiResponse<LoginResponse>.Ok(response));
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var result = await _authService.RefreshTokenAsync(request.RefreshToken);

        if (!result.Success)
            return Unauthorized(ApiResponse<object>.Fail(result.Error!));

        var response = new
        {
            accessToken = result.AccessToken,
            refreshToken = result.RefreshToken,
        };

        return Ok(ApiResponse<object>.Ok(response));
    }
}
