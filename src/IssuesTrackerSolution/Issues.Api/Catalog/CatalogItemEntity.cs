namespace Issues.Api.Catalog;

public class CatalogItemEntity
{
    public Guid Id { get; set; }

    public string Slug { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string AddedBy { get; set; } = string.Empty;
    public DateTimeOffset DateAdded { get; set; }
    public string Vendor { get; set; } = string.Empty;

}
