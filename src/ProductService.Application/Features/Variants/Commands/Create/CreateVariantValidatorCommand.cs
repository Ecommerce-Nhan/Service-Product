using FluentValidation;
using System.Text.Json;

namespace ProductService.Application.Features.Variants.Commands.Create;

public class CreateVariantValidatorCommand : AbstractValidator<CreateVariantCommand>
{
    public CreateVariantValidatorCommand()
    {
        RuleFor(x => x.Model.ProductId).NotEmpty().WithMessage("Product Id is required.");
        RuleFor(x => x.Model.SKU).NotEmpty().WithMessage("Code SKU is required.");
        RuleFor(x => x.Model.Quantity).GreaterThan(0);
        RuleFor(x => x.Model.UnitPrice).GreaterThan(0);
        RuleFor(x => x.Model.MainImage).NotEmpty();
        RuleFor(x => x.Model.Attributes)
            .NotEmpty().WithMessage("Attributes không được để trống.")
            .Must(BeAValidJson).WithMessage("Attributes phải là một JSON hợp lệ.");
    }
    private bool BeAValidJson(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return false;

        try
        {
            JsonDocument.Parse(json);
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
    }
}