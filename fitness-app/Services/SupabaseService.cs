using System.Net.Http.Headers;
using System.Text;
using fitness_app.Constants;
using Newtonsoft.Json;
using Supabase.Gotrue;
using Supabase.Storage;

namespace fitness_app.Services;
using Client = Supabase.Client;

public class SupabaseService : ISupabaseService
{
    private readonly Client _client;
    private readonly HttpClient _httpClient;
    private readonly string _updateUserAvatarEndPoint = EnvConstants.UpdateUserAvatarEndPoint;

    public SupabaseService(Client supabaseClient, HttpClient httpClient)
    {
        _client = supabaseClient;
        _httpClient = httpClient;
    }
    
    public async Task<string?> UploadFileToBucket(string bucketName, string filePath, string fileName)
    {
        var path = await _client.Storage
            .From(bucketName)
            .Upload(filePath, fileName);

        var url = _client.Storage.From(bucketName)
            .GetPublicUrl(fileName);
        
        return url;
    }

    public async Task<bool> UpdateUserAvatarMetadataAsync(Session session, string imageUrl)
    {
        if (session == null || session.User == null)
            throw new InvalidOperationException("Invalid session or missing user information.");
        
        var payload = new
        {
            userId = session.User.Id,
            imageUrl = imageUrl
        };
        
        string jsonPayload = JsonConvert.SerializeObject(payload);
        
        var content = new StringContent(
            jsonPayload, 
            Encoding.UTF8, 
            MediaTypesConstants.ApplicationJson);

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer", 
            session.AccessToken);
        
        HttpResponseMessage response = await _httpClient.PostAsync(_updateUserAvatarEndPoint, content);

        if (!response.IsSuccessStatusCode)
        {
            //log or do something
        }
        
        return response.IsSuccessStatusCode;

    }
}