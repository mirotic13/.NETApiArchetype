using Domain.Entities.Template;
using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace ApiArchetype.Validators;

[ExcludeFromCodeCoverage]
public class TemplateClassValidator : AbstractValidator<TemplateClass>
{
    public TemplateClassValidator()
    {
        RuleFor(x => x.Name).NotEmpty().Length(2, 50);
        RuleFor(x => x.CreatedAt).LessThanOrEqualTo(DateTime.Now);
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.Status).IsInEnum();
    }
}
