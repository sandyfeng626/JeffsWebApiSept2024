using System.ComponentModel.DataAnnotations;


namespace Issues.Api.Vendors.Models;

public record VendorInformationResponse // "Write Model" (stuff I'm sending to client)
{

    [Required]
    public string Id { get; set; } = string.Empty;
    [Required]
    public string Name { get; set; } = string.Empty;
};
