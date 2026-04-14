namespace Shared.Common.DTOs;

/// <summary>
/// Search parameters for advanced search endpoints.
/// </summary>
public class SearchParams : PaginationParams
{
    public string? SearchTerm { get; set; }
}
