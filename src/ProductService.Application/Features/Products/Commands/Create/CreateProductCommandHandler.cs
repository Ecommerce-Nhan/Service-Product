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
        var bucketName = Constants.ProductService.BucketName;
        var prefix = $"Products/Product-{input.Code}";
        var listUriImages = input.Images != null && input.Images.Any()
                            ? GenerateImagePlaceholders(input.Images, bucketName, prefix)
                            : null;
        var product = await _manager.CreateAsync(
                input.Name,
                input.Code,
                input.Note,
                input.CostPrice,
                listUriImages
            );
        await _repository.AddAsync(product);
        BackgroundJob.Enqueue(() => UploadFileToS3(input.Images, bucketName, prefix));

        return product.Id;
    }
    private List<string> GenerateImagePlaceholders(IEnumerable<IFormFile> images,
                                                   string bucketName,
                                                   string prefix)
    {
        var result = images.Select(file =>
        {
            var key = string.IsNullOrEmpty(prefix) ? file.FileName
                                                   : $"{prefix.TrimEnd('/')}/{file.FileName}";
            return $"{bucketName}/{key}";
        });
        return result.ToList();
    }
    public async Task UploadFileToS3(IEnumerable<IFormFile>? images,
                                                 string bucketName,
                                                 string prefix)
    {
        if (images != null && images.Any())
        {
            foreach (var file in images)
            {
                if (!file.ContentType.StartsWith("image/"))
                    throw new Exception("Invalid File Format. We support only Images.");

                var request = new PutObjectRequest()
                {
                    BucketName = bucketName,
                    Key = string.IsNullOrEmpty(prefix) ? file.FileName : $"{prefix?.TrimEnd('/')}/{file.FileName}",
                    InputStream = file.OpenReadStream(),
                };
                request.Metadata.Add("Content-Type", file.ContentType);
                await _s3Service.UploadToS3BucketAsync(request);
            }
        }
    }
}