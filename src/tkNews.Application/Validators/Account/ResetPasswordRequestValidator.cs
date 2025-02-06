using FluentValidation;
using tkNews.Application.Models.Account;

namespace tkNews.Application.Validators.Account;

public class ResetPasswordRequestValidator : AbstractValidator<ResetPasswordRequest>
{
    public ResetPasswordRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email adresi boş olamaz.")
            .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.");
            
        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("Token boş olamaz.");
            
        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("Yeni şifre boş olamaz.")
            .MinimumLength(8).WithMessage("Yeni şifre en az 8 karakter olmalıdır.")
            .MaximumLength(50).WithMessage("Yeni şifre 50 karakterden uzun olamaz.")
            .Matches("[A-Z]").WithMessage("Yeni şifre en az bir büyük harf içermelidir.")
            .Matches("[a-z]").WithMessage("Yeni şifre en az bir küçük harf içermelidir.")
            .Matches("[0-9]").WithMessage("Yeni şifre en az bir rakam içermelidir.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Yeni şifre en az bir özel karakter içermelidir.");
    }
}