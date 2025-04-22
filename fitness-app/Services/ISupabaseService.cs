using Supabase.Gotrue;

namespace fitness_app.Services;

public interface ISupabaseService
{
    Task<string?> UploadFileToBucket(string bucketName, string filePath, string FileName);

    Task<bool> UpdateUserAvatarMetadataAsync(Session session, string imageUrl);
}