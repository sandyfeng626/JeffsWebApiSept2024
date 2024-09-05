using HtTemplate.Configuration;
using Issues.Api.Catalog;
using Marten;
using Microsoft.FeatureManagement.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace Issues.Api.Hr;

//[ApiExplorerSettings(GroupName = "Hr")] // hide from the documentation - but it is still there.
[FeatureGate(ApiFeatureManagementOptions.HrCatalogFeature)]
public class Catalog(IDocumentSession session) : ControllerBase
{

    [HttpGet("/hr/software-catalog")]
    [SwaggerOperation(Tags = ["Human Relations"])]
    public async Task<ActionResult> GetFullSoftwareCatalogAsync(CancellationToken cancellationToken)
    {
        var response = await session.Query<CatalogItemEntity>()
            .Select(item => new HrCatalogItem() { Id = item.Slug, Title = item.Name, Vendor = item.Vendor })
            .ToListAsync(cancellationToken);

        return Ok(new { catalog = response });
    }
}

public record HrCatalogItem
{
    public string Id { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Vendor { get; set; } = string.Empty;
}