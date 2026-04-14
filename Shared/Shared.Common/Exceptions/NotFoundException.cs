namespace Shared.Common.Exceptions;

/// <summary>
/// Exception thrown when a requested resource is not found (HTTP 404).
/// </summary>
public class NotFoundException : Exception
{
    public NotFoundException() : base("Resource not found.") { }
    public NotFoundException(string message) : base(message) { }
    public NotFoundException(string entityName, object key)
        : base($"{entityName} with key '{key}' was not found.") { }
}
