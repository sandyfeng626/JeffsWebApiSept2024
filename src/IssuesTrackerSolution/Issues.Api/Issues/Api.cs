using Marten;
using Microsoft.AspNetCore.Http.HttpResults;
using Riok.Mapperly.Abstractions;
using Wolverine;

namespace Issues.Api.Issues;

public static class Api
{
    public static IEndpointRouteBuilder MapIssuesApi(this IEndpointRouteBuilder builder)
    {
        builder.MapPost("/vendors/{vendor}/software/{software}/issues", AddNewIssueAsync);

        return builder;
    }

    public static async Task<Results<Ok<CreateIssueResponseItem>, BadRequest>> AddNewIssueAsync(
        [FromRoute] string vendor,
        [FromRoute] string software,
        [FromBody] CreateIssueRequest request,
        TimeProvider timeProvider,
        IDocumentSession session,
        IMessageBus bus)
    {

        // call another API or two 
        // call another api and send this, and get the tech works on word.
        //await Task.Delay(1000); // this is us having to call that other API...
        var response = new CreateIssueResponseItem
        {
            Id = Guid.NewGuid(),
            Vendor = vendor,
            Software = software,
            LoggedBy = "the user",
            Created = timeProvider.GetUtcNow(),
            Status = IssueStatus.PendingTechAssignment
        };

        var entity = response.MapToEntity();

        session.Store(entity);
        await session.SaveChangesAsync();
        await bus.PublishAsync(new AssignNewIssue(entity.Id));

        return TypedResults.Ok(response);
    }
}

// What is the resource (URL)
// POST /vendors/{vendor}/software/{software}/issues
// POST /issues
// what is the request model
// description of what is wrong.
// what is the entity (what are saving in the database or whatever)
// What data?
// - issue id (guid) // generated
// - their employee id (sub) // JWT
// - what software // URI
// - description of the issue // body
// - When was it created // generate
// - status // generate
//   - PendingTechAssignment

// what are you sending back again.
// Here's your issue id, here's what you told us, and here is the status.

public record CreateIssueRequest
{
    public string Description { get; set; } = string.Empty;

}

public record CreateIssueResponseItem
{
    public Guid Id { get; set; }
    public string LoggedBy { get; set; } = string.Empty; // who filed the issue
    public string Software { get; set; } = string.Empty;
    public string Vendor { get; set; } = string.Empty;
    public DateTimeOffset Created { get; set; }
    public IssueStatus Status { get; set; }
}

public class IssueEntity
{
    public Guid Id { get; set; }
    public string LoggedBy { get; set; } = string.Empty; // who filed the issue
    public string Software { get; set; } = string.Empty;
    public string Vendor { get; set; } = string.Empty;
    public DateTimeOffset Created { get; set; }
    public IssueStatus Status { get; set; }
}


public enum IssueStatus { PendingTechAssignment }

[Mapper]
public static partial class IssueMappers
{
    public static partial IssueEntity MapToEntity(this CreateIssueResponseItem entity);
}

public record AssignNewIssue(Guid Id);