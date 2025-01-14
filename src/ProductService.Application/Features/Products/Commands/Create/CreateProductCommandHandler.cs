using MediatR;
using ProductService.Domain.Products;
using Microsoft.AspNetCore.Http;
using Hangfire;
using ProductService.Application.Services.S3;
using Amazon.S3.Model;
using SharedLibrary.Helpers;

namespace ProductService.Application.Features.Products.Commands.Create;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _repository;
    private readonly ProductManager _manager;
    private readonly IS3Service _s3Service;
    public CreateProductCommandHandler(IProductRepository repository,
                                       ProductManager manager,
                                       IS3Service s3Service)
    {
        _repository = repository;
        _manager = manager;
        _s3Service = s3Service;
    }
    public async Task<Guid> Handle(CreateProductCommand command,
                                   CancellationToken cancellationToken)
    {
        var input = command.Model;
        var product = await _manager.CreateAsync(
                input.Name,
                input.Code,
                input.Note,
                input.CostPrice
            );
        await _repository.AddAsync(product);
        if (input.Images != null)
        {
            await HandleTempFiles(input.Images, input.Code);
        }

        return product.Id;
    }
    private async Task HandleTempFiles(IEnumerable<IFormFile> inputImages, string code)
    {
        var bucketName = Constants.ProductService.BucketName;
        var prefix = $"Products/Product-{code}";
        foreach (var image in inputImages)
        {
            if (!image.ContentType.StartsWith("image/"))
            {
                throw new Exception("Invalid File Format. We support only Images.");
            }
            using var memoryStream = new MemoryStream();
            await image.CopyToAsync(memoryStream);
            var imageData = memoryStream.ToArray();
            //
            var fileName = image.FileName;
            var key = string.IsNullOrEmpty(prefix) ? fileName : $"{prefix?.TrimEnd('/')}/{fileName}";
            var request = new PutObjectRequest()
            {
                BucketName = bucketName,
                Key = key,
                InputStream = image.OpenReadStream(),
            };
            request.Metadata.Add("Content-Type", image.ContentType);
            BackgroundJob.Enqueue<IS3Service>(service => service.UploadToS3BucketAsync(bucketName, key, imageData, image.ContentType));
        }

        await Task.CompletedTask;
    }
}