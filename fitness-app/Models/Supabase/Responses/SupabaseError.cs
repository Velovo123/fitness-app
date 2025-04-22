using Newtonsoft.Json;

namespace fitness_app.Models.Supabase.Responses;

public class SupabaseError
{
    public int Code { get; set; }
    
    [JsonProperty("error_code")]
    public string ErrorCode { get; set; } = string.Empty;
    public string Msg { get; set; } = string.Empty;
}