using Hangfire;
using MediatR;
using ProductService.Application.Services.S3;
using SharedLibrary.Helpers;

namespace ProductService.Application.Features.Products.Notifications;

public class UploadImagesToS3Handler : INotificationHandler<ProductCreatedNotification>
{
    public async Task Handle(ProductCreatedNotification notification, CancellationToken cancellationToken)
    {
        var bucketName = Constants.ProductService.BucketName;
        var prefix = $"Products/Product-{notification.model.Code}";
        notification.entity.Images = new List<string>();

        foreach (var image in notification.model.Images)
        {
            if (!image.ContentType.StartsWith("image/"))
            {
                throw new Exception("Invalid File Format. We support only Images.");
            }
            using var memoryStream = new MemoryStream();
            await image.CopyToAsync(memoryStream);
            var imageData = memoryStream.ToArray();
            var fileName = image.FileName;
            var key = string.IsNullOrEmpty(prefix) ? fileName : $"{prefix?.TrimEnd('/')}/{fileName}";
            notification.entity.Images.Add($"{bucketName}/{key}");

            BackgroundJob.Enqueue<IS3Service>(service => 
                        service.UploadToS3BucketAsync(bucketName, key, imageData, image.ContentType));
        }
    }
}