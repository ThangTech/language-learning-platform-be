namespace Shared.Common.Exceptions;

/// <summary>
/// Exception thrown when user doesn't have permission (HTTP 403).
/// </summary>
public class ForbiddenException : Exception
{
    public ForbiddenException() : base("You do not have permission to access this resource.") { }
    public ForbiddenException(string message) : base(message) { }
}
