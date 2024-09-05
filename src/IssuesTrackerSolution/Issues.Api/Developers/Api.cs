namespace Issues.Api.Developers;

public class Api : ControllerBase

{
    [HttpGet("/developers")]
    public ActionResult GetDevelopers()
    {
        return Ok("They are rad");
    }
}
