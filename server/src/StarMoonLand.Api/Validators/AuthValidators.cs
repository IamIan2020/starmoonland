using FluentValidation;
using StarMoonLand.Api.DTOs.Auth;

namespace StarMoonLand.Api.Validators;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("請輸入電子信箱")
            .EmailAddress().WithMessage("電子信箱格式不正確");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("請輸入密碼")
            .MinimumLength(6).WithMessage("密碼至少 6 個字元");
    }
}

public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(x => x.RefreshToken)
            .NotEmpty().WithMessage("Refresh Token 不得為空");
    }
}
