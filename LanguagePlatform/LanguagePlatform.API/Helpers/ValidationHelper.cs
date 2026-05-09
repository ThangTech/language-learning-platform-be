using FluentValidation;
using LanguagePlatform.Application.DTOs.Common;

namespace LanguagePlatform.API.Helpers;

/// <summary>
/// Extension methods to simplify calling FluentValidation from controllers.
/// </summary>
public static class ValidationHelper
{
    /// <summary>
    /// Validates the given model and returns a failed ApiResponse if invalid.
    /// Returns null if validation passes.
    /// </summary>
    public static async Task<ApiResponse<T>?> ValidateAsync<TRequest, T>(
        IValidator<TRequest> validator,
        TRequest request)
    {
        var result = await validator.ValidateAsync(request);
        if (result.IsValid) return null;

        var errors = result.Errors.Select(e => e.ErrorMessage).ToList();
        return ApiResponse<T>.Fail(errors.First(), errors);
    }
}
