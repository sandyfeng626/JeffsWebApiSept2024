namespace Issues.Api.Vendors.Utils;

public static class SlugGenerator
{

    public static string GenerateSlug(this string value)
    {
        var options = new Slugify.SlugHelperConfiguration()
        {
            ForceLowerCase = true,
        };
        var slugger = new Slugify.SlugHelper(options);

        var slug = slugger.GenerateSlug(value);

        return slug;
    }
}
