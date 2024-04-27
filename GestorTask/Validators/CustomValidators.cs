using FluentValidation;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace GestorTask.Validators;

public static class CustomValidators
{
    public static IRuleBuilderOptions<T, string> CustomNotNullEmptyEqual<T>(this IRuleBuilder<T, string> ruleBuilder, string value, string propertName)
    {
        return ruleBuilder
            .NotNull()
            .NotEmpty()
            .NotEqual(value, StringComparer.InvariantCultureIgnoreCase)
            .WithMessage($"Verifique que esta llenando correctamente el campo {propertName}");
    }

}
