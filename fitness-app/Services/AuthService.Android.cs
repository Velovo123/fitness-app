using System;
using System.Threading.Tasks;
using Supabase.Gotrue;

namespace fitness_app.Services;

public class AuthService_Android : IAuthService
{
    public Task InitializeAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Session?> SignInWithGoogleAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Session?> TryAutoSignInAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Session?> SignUpAsync(string email, string password, string fullName, string phone)
    {
        throw new NotImplementedException();
    }

    public Task<Session?> SignInAsync(string email, string password)
    {
        throw new NotImplementedException();
    }

    public Task<bool> ResetPasswordForEmailAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<Session?> VerifyUserOtpAsync(string email, string otp)
    {
        throw new NotImplementedException();
    }

    public Task SendMagicLink(string email)
    {
        throw new NotImplementedException();
    }
}