using Amazon.S3.Model;

namespace ProductService.Application.Services.S3;

public interface IS3Service
{
    Task<object> GetAllFilesAsync(string bucketName, string? prefix);
    Task<(Stream, string)> GetFileByKeyAsync(string bucketName, string key);
    Task<string> UploadToS3BucketAsync(string bucketName, string key, byte[] data, string contentType);
    Task DeleteFromS3BucketAsync(string key, string bucketName);
    Task<string> GetPreSignedUrlAsync(string key, string bucketName, DateTime expiration);
}