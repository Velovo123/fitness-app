using Supabase.Gotrue;

namespace fitness_app.Services;

public interface IAuthService
{
    Task InitializeAsync();

    Task<Session?> SignInWithGoogleAsync();

    Task<Session?> SignUpAsync(
        string email,
        string password,
        string fullName,
        string phone);

    Task<Session?> SignInAsync(string email, string password);
    
    Task<bool> ResetPasswordForEmailAsync(string email);

    Task<Session?> VerifyUserOtpAsync(string email, string otp);

    Task SendMagicLink(string email);
}