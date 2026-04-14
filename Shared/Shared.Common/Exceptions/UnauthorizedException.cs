namespace Shared.Common.Exceptions;

/// <summary>
/// Exception thrown when user is not authenticated (HTTP 401).
/// </summary>
public class UnauthorizedException : Exception
{
    public UnauthorizedException() : base("Unauthorized access.") { }
    public UnauthorizedException(string message) : base(message) { }
}
