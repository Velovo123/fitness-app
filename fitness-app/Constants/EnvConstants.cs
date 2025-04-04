using System;
using System.IO;
using DotNetEnv;

namespace fitness_app.Constants;

public static class EnvConstants
{
    public static readonly string EnvFilePath = Path.Combine(AppContext.BaseDirectory, ".env");
    public static readonly string SupabaseUrl;
    public static readonly string SupabaseKey;
    public static readonly string ClientId;
    public static readonly string CheckEmailExistenceEndpoint;
    public static readonly string DefaultImageUrl;
    
    static EnvConstants()
    {
        Env.Load(EnvFilePath);

        SupabaseUrl = Env.GetString("SUPABASE_URL");
        SupabaseKey = Env.GetString("SUPABASE_KEY");
        ClientId = Env.GetString("GOOGLE_CLIENT_ID");
        CheckEmailExistenceEndpoint = Env.GetString("CHECK_EMAIL_EXISTENCE_ENDPOINT");
        DefaultImageUrl = Env.GetString("DEFAULT_IMAGE_URL");
    }
}