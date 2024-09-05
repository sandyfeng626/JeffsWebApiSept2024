using FluentValidation;
using Issues.Api.Vendors.Utils;
using Marten;
using Riok.Mapperly.Abstractions;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace Issues.Api.Catalog;

[Produces("application/json")]
[ApiExplorerSettings(GroupName = "Software")]
public class Api(ILookupVendors vendorLookup, TimeProvider timeProvider, IDocumentSession session) : ControllerBase
{
    // Todo: Only members of SoftwareCenter role should be able to do this.
    /// <summary>
    /// Use this to add a piece of software to the vendor
    /// </summary>
    /// <param name="vendor">the id of the vendor</param>
    /// <returns></returns>
    [HttpPost("/vendors/{vendor}/software")]
    [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(Tags = ["Software"])]

    public async Task<ActionResult<SoftwareCatalogItemResponse>> AddSoftwareToCatalogAsync(
        [FromBody] CreateSoftwareCatalogItemRequest request,
        [FromRoute] string vendor,
        [FromServices] IValidator<CreateSoftwareCatalogItemRequest> validator,
        CancellationToken token
        )
    {

        var validations = await validator.ValidateAsync(request, token);

        if (!validations.IsValid)
        {
            return BadRequest(validations.ToDictionary());
        }
        // Todo: This could be a filter.
        if (await vendorLookup.IsCurrentVendorAsync(vendor) == false)
        {
            return NotFound();
        }

        // create an entity and save it to the database
        var entity = request.MapToEntity(vendor, timeProvider.GetUtcNow());
        // save it to the database
        session.Store(entity);
        await session.SaveChangesAsync(token);



        var response = entity.MapToResponse();
        // we need to return something back again, but not the entity or the request.
        return CreatedAtRoute("catalog#getsoftwarebyid", new { vendor, software = response.Id }, response);
    }
    // Document
    [HttpGet("/vendors/{vendor}/software/{software}", Name = "catalog#getsoftwarebyid")]
    [ResponseCache(Duration = 15, Location = ResponseCacheLocation.Client)]
    [SwaggerOperation(Tags = ["Software"])]
    public async Task<ActionResult<SoftwareCatalogItemResponse>> GetSoftwareById(string software, string vendor, CancellationToken token)
    {
        var item = await session.Query<CatalogItemEntity>().SingleOrDefaultAsync(item => item.Slug == software, token);
        if (await vendorLookup.IsCurrentVendorAsync(vendor) == false)
        {
            return NotFound("Vendor does not exist");
        }
        if (item is null)
        {
            return NotFound("Vendor does not have that software");
        }
        else
        {
            return Ok(item.MapToResponse());
        }
    }

    // TODO add a GET /vendors/{vendor}/software -> return all the software for a vendor.
    // As long as that is a good vendor, this should never return a 404.
}

public interface ILookupVendors
{
    Task<bool> IsCurrentVendorAsync(string vendor);
}

public record CreateSoftwareCatalogItemRequest
{
    // at least five characters and no more than 100. Required
    public string Name { get; set; } = string.Empty;
    // at least 10 characters and now more than 1024. Required.
    public string Description { get; set; } = string.Empty;

}

public record SoftwareCatalogItemResponse
{
    // Todo: Maybe add validation attributes to show what we promise will be there.
    public string Id { get; set; } = string.Empty; // slug
    [Required]
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public DateTimeOffset Added { get; set; }

}

public class CreateSoftwareCatalogItemRequestValidator : AbstractValidator<CreateSoftwareCatalogItemRequest>
{
    public CreateSoftwareCatalogItemRequestValidator(IDocumentSession session)
    {
        RuleFor(c => c.Name).NotEmpty().MinimumLength(5).MaximumLength(100);
        RuleFor(c => c.Description).NotEmpty().MinimumLength(10).MaximumLength(1024);
        RuleFor(v => v.Name).MustAsync(async (name, cancellation) =>
        {
            var slug = name.GenerateSlug();
            var exists = await session.Query<CatalogItemEntity>().AnyAsync(v => v.Slug == slug, cancellation);
            return !exists;
        }).WithMessage("That Software Item Already Exists");
    }
}

public static class CatalogMappingExtensions
{
    public static CatalogItemEntity MapToEntity(this CreateSoftwareCatalogItemRequest request, string vendor, DateTimeOffset createdTime)
    {
        return new CatalogItemEntity
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            AddedBy = "sub of the person that added this",
            DateAdded = createdTime,
            Slug = request.Name.GenerateSlug(),
            Vendor = vendor,
        };
    }

    //public static SoftwareCatalogItemResponse MapToResponse(this CatalogItemEntity entity)
    //{
    //    return new SoftwareCatalogItemResponse
    //    {
    //        Id = entity.Slug,
    //        Name = entity.Name,
    //        Description = entity.Description,
    //        Added = entity.DateAdded
    //    };
    //}
}

[Mapper]
public static partial class CatalogMappers
{


    [MapPropertyFromSource(nameof(SoftwareCatalogItemResponse.Id), Use = nameof(ConvertIdToSlug))]
    public static partial SoftwareCatalogItemResponse MapToResponse(this CatalogItemEntity entity);

    private static string ConvertIdToSlug(CatalogItemEntity entity) => entity.Slug;
}