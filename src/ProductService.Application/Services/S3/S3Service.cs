using Amazon.S3;
using Amazon.S3.Model;

namespace ProductService.Application.Services.S3;

public class S3Service : IS3Service
{
    private readonly IAmazonS3 _s3Client;
    public S3Service(IAmazonS3 s3Client)
    {
        _s3Client = s3Client;
    }

    public async Task<(Stream, string)> GetFileByKeyAsync(string bucketName, string key)
    {
        var s3Object = await _s3Client.GetObjectAsync(bucketName, key);
        GetObjectRequest request = new GetObjectRequest()
        {
            BucketName = bucketName,
            Key = key
        };
        return (s3Object.ResponseStream, s3Object.Headers.ContentType);
    }
    public async Task UploadToS3BucketAsync(string bucketName, string key, byte[] data, string contentType)
    {
        using var stream = new MemoryStream(data);

        var request = new PutObjectRequest
        {
            BucketName = bucketName,
            Key = key,
            InputStream = stream,
            ContentType = contentType
        };
        await _s3Client.PutObjectAsync(request);
    }
    public async Task DeleteFromS3BucketAsync(string key, string bucketName)
    {
        await _s3Client.DeleteObjectAsync(bucketName, key);
    }

    public async Task<string> GetPreSignedUrlAsync(string key, string bucketName, DateTime expiration)
    {
        string urlString = string.Empty;
        var request = new GetPreSignedUrlRequest()
        {
            BucketName = bucketName,
            Key = key,
            Expires = expiration,
        };
        urlString = await _s3Client.GetPreSignedURLAsync(request);
        
        return urlString;
    }

    public async Task<object> GetAllFilesAsync(string bucketName, string? prefix)
    {
        var request = new ListObjectsV2Request()
        {
            BucketName = bucketName,
            Prefix = prefix
        };
        var result = await _s3Client.ListObjectsV2Async(request);
        var s3Objects = result.S3Objects.Select(s =>
        {
            var urlRequest = new GetPreSignedUrlRequest()
            {
                BucketName = bucketName,
                Key = s.Key,
                Expires = DateTime.UtcNow.AddMinutes(1)
            };
            return new
            {
                Name = s.Key.ToString(),
                PresignedUrl = _s3Client.GetPreSignedURL(urlRequest),
            };
        });

        return s3Objects;
    }
}
