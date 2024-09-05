namespace Issues.Api.Vendors.Entities;

public class VendorItemEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string AddedBy { get; set; } = string.Empty; // the SoftwareCenter Admin that added this.
    public DateTimeOffset Added { get; set; }
}
