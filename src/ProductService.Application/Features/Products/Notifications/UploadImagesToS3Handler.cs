using Hangfire;
using MediatR;
using ProductService.Application.Services.S3;
using SharedLibrary.Helpers;
using System.Buffers;

namespace ProductService.Application.Features.Products.Notifications;

public class UploadImagesToS3Handler : INotificationHandler<ProductCreatedNotification>
{
    public async Task Handle(ProductCreatedNotification notification, CancellationToken cancellationToken)
    {
        var bucketName = Constants.ProductService.BucketName;
        var prefix = $"Products/Product-{notification.model.Code}";
        var bufferPool = ArrayPool<byte>.Shared;
        var uploadTasks = new List<Task>();

        notification.entity.Images = new List<string>();

        foreach (var image in notification.model.Images)
        {
            if (!image.ContentType.StartsWith("image/"))
            {
                throw new Exception("Invalid File Format. We support only Images.");
            }
            var fileName = image.FileName;
            var key = string.IsNullOrEmpty(prefix) ? fileName : $"{prefix?.TrimEnd('/')}/{fileName}";
            notification.entity.Images.Add($"{bucketName}/{key}");

            var buffer = bufferPool.Rent((int)image.Length);
            using (var memoryStream = new MemoryStream(buffer))
            {
                await image.CopyToAsync(memoryStream, cancellationToken);
                var imageData = memoryStream.ToArray();

                uploadTasks.Add(Task.Run(() => BackgroundJob.Enqueue<IS3Service>(service =>
                        service.UploadToS3BucketAsync(bucketName, key, imageData, image.ContentType))));
            }
            bufferPool.Return(buffer);
        }
        await Task.WhenAll(uploadTasks);
    }
}