using FluentValidation;
using tkNews.Application.Models.Account;

namespace tkNews.Application.Validators.Account;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email adresi boş olamaz.")
            .EmailAddress().WithMessage("Geçerli bir email adresi giriniz.")
            .MaximumLength(100).WithMessage("Email adresi 100 karakterden uzun olamaz.");
            
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Şifre boş olamaz.")
            .MinimumLength(8).WithMessage("Şifre en az 8 karakter olmalıdır.")
            .MaximumLength(50).WithMessage("Şifre 50 karakterden uzun olamaz.")
            .Matches("[A-Z]").WithMessage("Şifre en az bir büyük harf içermelidir.")
            .Matches("[a-z]").WithMessage("Şifre en az bir küçük harf içermelidir.")
            .Matches("[0-9]").WithMessage("Şifre en az bir rakam içermelidir.")
            .Matches("[^a-zA-Z0-9]").WithMessage("Şifre en az bir özel karakter içermelidir.");
            
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Ad boş olamaz.")
            .MaximumLength(50).WithMessage("Ad 50 karakterden uzun olamaz.")
            .Matches("^[a-zA-ZğüşıöçĞÜŞİÖÇ ]+$").WithMessage("Ad sadece harf içerebilir.");
            
        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Soyad boş olamaz.")
            .MaximumLength(50).WithMessage("Soyad 50 karakterden uzun olamaz.")
            .Matches("^[a-zA-ZğüşıöçĞÜŞİÖÇ ]+$").WithMessage("Soyad sadece harf içerebilir.");
    }
} 