using Microsoft.AspNetCore.Http;

namespace ProductService.Application.Services.S3;

public interface IS3Service
{
    Task<object> GetAllFilesAsync(string bucketName, string? prefix);
    Task<(Stream, string)> GetFileByKeyAsync(string bucketName, string key);
    Task<string> UploadToS3BucketAsync(IFormFile file, string bucketName, string? prefix);
    Task DeleteFromS3BucketAsync(string key, string bucketName);
    Task<string> GetPreSignedUrlAsync(string key, string bucketName, DateTime expiration);
}