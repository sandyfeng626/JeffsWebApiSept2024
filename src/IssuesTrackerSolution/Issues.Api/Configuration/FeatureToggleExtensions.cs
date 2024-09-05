using Microsoft.FeatureManagement;

namespace HtTemplate.Configuration;
// For more details, see https://timdeschryver.dev/blog/feature-flags-in-net-from-simple-to-more-advanced
public static class FeatureToggleExtensions
{
    public static async Task<bool> IsDevFeatureEnabledAsync(this WebApplication app, string feature)
    {
        return await app.Services.GetRequiredService<IFeatureManager>().IsEnabledAsync(feature);
    }
}


