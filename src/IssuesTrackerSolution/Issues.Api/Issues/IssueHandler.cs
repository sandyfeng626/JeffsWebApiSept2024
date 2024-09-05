namespace Issues.Api.Issues;

public static class IssueHandler
{
    public static void Handle(AssignNewIssue cmd, ILogger logger)
    {
        Thread.Sleep(1000); // this could be async... but just simulating that delay
        logger.LogInformation("Got an issue with the id of {id}", cmd.Id);
    }
}
