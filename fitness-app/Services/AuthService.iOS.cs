#if IOS
using System.Text;
using fitness_app.Constants;
using Foundation;
using Google.SignIn;
using Newtonsoft.Json;
using Supabase.Gotrue;
using Supabase.Gotrue.Exceptions;
using UIKit;

namespace fitness_app.Services;

public class AuthService_iOS : IAuthService
{
    private readonly string _supabaseUrl = EnvConstants.SupabaseUrl;
    private readonly string _clientId = EnvConstants.ClientId;
    private readonly string _checkEmailExistenceEndpoint = EnvConstants.CheckEmailExistenceEndpoint;
    
    private readonly Client _supabaseClient;
    private bool _isInitialized;

    public AuthService_iOS(Client supabaseClient)
    {
        _supabaseClient = supabaseClient;
    }
    
    
    public async Task InitializeAsync()
    {
        if (!_isInitialized)
        {
            await _supabaseClient.RetrieveSessionAsync();
            _isInitialized = true;
        }
    }
    
    public async Task<Session?> SignUpAsync(string email, string password, string fullName, string phone)
    {
        var emailExists = await CheckIfEmailExistsAsync(email);
        if (emailExists)
        {
            return null;
        }
        
        var signUpOptions = new SignUpOptions
        {
            Data = new Dictionary<string, object>
            {
                { SignUpOptionsConstants.FullName, fullName },
                { SignUpOptionsConstants.Phone, phone }
            }
        };
        
        return await _supabaseClient.SignUp(email, password, signUpOptions);
    }
    
    private async Task<bool> CheckIfEmailExistsAsync(string email)
    {
        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri(_supabaseUrl);
        var preCheckPayload = new
        {
            email
        };

        var payloadJson = JsonConvert.SerializeObject(preCheckPayload);
        var httpContent = new StringContent(payloadJson, Encoding.UTF8, MediaTypesConstants.ApplicationJson);

        var preCheckResponse = await httpClient.PostAsync(_checkEmailExistenceEndpoint, httpContent);
        var preCheckResponseJson = await preCheckResponse.Content.ReadAsStringAsync();

        var anonResponseTemplate = new { exists = false, error = "" };
        var preCheckResult = JsonConvert.DeserializeAnonymousType(preCheckResponseJson, anonResponseTemplate);

        return preCheckResult != null && preCheckResult.exists;
    }


    public async Task<Session?> SignInAsync(string email, string password)
    {
        Session? session;
        try
        {
            session = await _supabaseClient.SignIn(email, password);
        }
        catch (GotrueException ex)
        {
            if (ex.Message.Contains(AuthErrorConstants.EmailNotConfirmed, StringComparison.OrdinalIgnoreCase))
            {
                throw;
            }
            session = null;
        }
        catch (Exception)
        {
            session = null;
            // log unexpected ex
        }
        
        return session;
    }

    public async Task<bool> ResetPasswordForEmailAsync(string email)
    {
        try
        {
            var emailExists = await CheckIfEmailExistsAsync(email);
            if (!emailExists)
                return false;
            
            await _supabaseClient.ResetPasswordForEmail(email);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<Session?> VerifyUserOtpAsync(string email, string otp)
    {
        try
        {
            var session = await _supabaseClient.VerifyOTP(email,
                otp,
                Supabase.Gotrue.Constants.EmailOtpType.Email);

            return IsValidSession(session) ? session : null;
        }
        catch (GotrueException)
        {
            //user've entered invalid token
            return null;
        }
        catch (Exception)
        {
            //log 
            return null;
        }
    }
    
    private bool IsValidSession(Session? session)
    {
        return session?.AccessToken != null;
    }

    public async Task SendMagicLink(string email)
    {
        try
        {
            await _supabaseClient.SendMagicLink(email);
        }
        catch (Exception ex)
        {
            //
        }
    }

    public async Task<Session?> SignInWithGoogleAsync()
    {
        await InitializeAsync();
        
        var googleUser = await PerformGoogleSignInAsync();
        if (googleUser?.Authentication == null)
            return null;
        
        var idToken = googleUser.Authentication.IdToken;
        var accessToken = googleUser.Authentication.AccessToken;


        var session = await _supabaseClient.SignInWithIdToken(
            provider: Supabase.Gotrue.Constants.Provider.Google,
            idToken: idToken,
            accessToken: accessToken,
            nonce: null 
        );

        return session;
    }
    
    private async Task<GoogleUser?> PerformGoogleSignInAsync()
    {
        SignIn.SharedInstance.ClientId = _clientId;

        var tcs = new TaskCompletionSource<GoogleUser>();
    
        SignIn.SharedInstance.Delegate = new GoogleSignInDelegate(tcs);

        SignIn.SharedInstance.PresentingViewController = GetRootViewController() ?? throw new InvalidOperationException();
        
        if(SignIn.SharedInstance.HasPreviousSignIn)
            SignIn.SharedInstance.RestorePreviousSignIn();
        else
            SignIn.SharedInstance.SignInUser();
    
        return await tcs.Task;
    }
    
    public static UIViewController? GetRootViewController()
    {
        if (UIDevice.CurrentDevice.CheckSystemVersion(13, 0))
        {
#pragma warning disable CA1416
            var windowScene = UIApplication.SharedApplication
                .ConnectedScenes
                .OfType<UIWindowScene>()
                .FirstOrDefault();

            var rootVc = windowScene?.Windows
                .FirstOrDefault(w => w.IsKeyWindow)?
                .RootViewController;
#pragma warning restore CA1416

            return rootVc;
        }
        else
        {
            return UIApplication.SharedApplication.KeyWindow?.RootViewController!;
        }
    }
    
    public class GoogleSignInDelegate : SignInDelegate
    {
        private readonly TaskCompletionSource<GoogleUser> _tcs;

        public GoogleSignInDelegate(TaskCompletionSource<GoogleUser> tcs)
        {
            _tcs = tcs;
        }

        public override void DidSignIn(SignIn signIn, GoogleUser user, NSError? error)
        {
            if (error != null)
            {
                _tcs.TrySetException(new Exception(error.LocalizedDescription));
            }
            else
            {
                _tcs.TrySetResult(user);
            }
        }
    }
}

#endif