namespace HtTemplate.Configuration;

public class ApiFeatureManagementOptions
{
    public const string FeatureManagement = "FeatureManagement";
    public bool HrCatalog { get; set; }
    public const string HrCatalogFeature = "HrCatalog";
    public bool Issues { get; set; }
    public const string IssuesFeature = "Issues";
}