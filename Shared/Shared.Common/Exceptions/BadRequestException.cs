namespace Shared.Common.Exceptions;

/// <summary>
/// Exception thrown when request data is invalid (HTTP 400).
/// </summary>
public class BadRequestException : Exception
{
    public List<string> Errors { get; }

    public BadRequestException() : base("Bad request.")
    {
        Errors = new List<string>();
    }

    public BadRequestException(string message) : base(message)
    {
        Errors = new List<string>();
    }

    public BadRequestException(string message, List<string> errors) : base(message)
    {
        Errors = errors;
    }
}
