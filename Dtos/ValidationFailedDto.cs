namespace RIA.API.Dtos;

public class ValidationFailedDto(string reason = "Validation failed due to the following error(s)",
    params string[] errors)
{
    public string Reason { get; set; } = reason;
    public IEnumerable<string> Errors { get; set; } = errors ?? ["Unexpected error occurred. Please refer to the logs for more details."];
}