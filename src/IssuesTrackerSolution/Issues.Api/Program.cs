using FluentValidation;
using HtTemplate.Configuration;
using Issues.Api.Catalog;
using Issues.Api.Issues;
using Issues.Api.Vendors.Entities;
using Issues.Api.Vendors.Services;
using Marten;
using Wolverine.Marten;
using static Issues.Api.Vendors.Models.VendorCreateRequest;

var builder = WebApplication.CreateBuilder(args); // the built in reasonable defaults, according to the ASP.NET MVC Core team.

builder.AddCustomFeatureManagement();

builder.Services.AddCustomServices();
builder.Services.AddCustomOasGeneration();

builder.Services.AddControllers();

builder.Services.AddScoped<VendorData>();
builder.Services.AddScoped<ILookupVendors, VendorData>(); // pretty particular to our Catalog feature.


var connectionString = builder.Configuration.GetConnectionString("issues") ?? throw new Exception("No Connection String in Environment");
builder.Services.AddMarten(options =>
{
    options.Connection(connectionString);
    options.Schema.For<VendorItemEntity>().UniqueIndex(Marten.Schema.UniqueIndexType.Computed, v => v.Slug);
}).UseLightweightSessions().IntegrateWithWolverine();

builder.Services.AddValidatorsFromAssemblyContaining<VendorCreateRequestValidator>(); // you only need to do this once.

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("IsSoftwareCenterAdmin", policy =>
    {
        policy.RequireRole("SoftwareCenter");
        policy.RequireRole("SoftwareCenterAdmin");
    });

var app = builder.Build(); // after this line, you can't add or change services any more.
// everything after this line is about setting up the HTTP "Middleware" - the stuff that is going to process incoming requests/responses.
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) // Environment Variable ASPNETCORE_ENVIRONMENT
{
    app.UseSwagger(); // the thing that generates our swagger.json (the formal description of our API).
    app.UseSwaggerUI(); // this adds SwaggerUI - which is an HTML5/JavaScript app you get to at /swagger
}

app.UseAuthentication(); // AuthZ - If we know who you are, you still might not be able to do this thing.
app.UseAuthorization(); // AuthN - verifying the identity of the person

app.MapControllers(); // Uses reflection in .NET to find all the controllers, look at their attributes and create the route table.

if (await app.IsDevFeatureEnabledAsync(ApiFeatureManagementOptions.IssuesFeature))
{
    app.MapIssuesApi();
}

app.Run(); // it starts running here.