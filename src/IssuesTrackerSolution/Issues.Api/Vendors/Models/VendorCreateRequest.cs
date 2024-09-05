using FluentValidation;
using Issues.Api.Vendors.Entities;
using Issues.Api.Vendors.Utils;
using Marten;


namespace Issues.Api.Vendors.Models;

public record VendorCreateRequest
{
    public string Name { get; set; } = string.Empty;


    public class VendorCreateRequestValidator : AbstractValidator<VendorCreateRequest>
    {
        public VendorCreateRequestValidator(IDocumentSession session)
        {

            RuleFor(v => v.Name).NotEmpty().MinimumLength(3).MaximumLength(100);
            RuleFor(v => v.Name).MustAsync(async (name, cancellation) =>
            {
                var slug = name.GenerateSlug();
                var exists = await session.Query<VendorItemEntity>().AnyAsync(v => v.Slug == slug, cancellation);
                return !exists;
            }).WithMessage("That Vendor Already Exists");
        }
    }
}
