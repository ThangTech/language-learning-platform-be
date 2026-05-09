namespace LanguagePlatform.Application.DTOs.Common;

// Dùng khi API trả về có kèm dữ liệu (ví dụ: trả về thông tin user sau khi đăng nhập)
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public List<string>? Errors { get; set; }

    public static ApiResponse<T> Ok(T data, string message = "Thành công")
    {
        return new ApiResponse<T>
        {
            Success = true,
            Message = message,
            Data = data
        };
    }

    public static ApiResponse<T> Fail(string message, List<string>? errors = null)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Message = message,
            Errors = errors
        };
    }
}

// Dùng khi API chỉ trả về thông báo, không có dữ liệu kèm theo
// Ví dụ: thông báo lỗi validation, thông báo xóa thành công
public class ApiErrorResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public List<string>? Errors { get; set; }

    public static ApiErrorResponse Fail(string message, List<string>? errors = null)
    {
        return new ApiErrorResponse
        {
            Success = false,
            Message = message,
            Errors = errors
        };
    }
}

// Dùng khi API trả về danh sách có phân trang (ví dụ: danh sách từ vựng trang 1, 2, 3...)
public class PagedResult<T>
{
    public List<T> Items { get; set; } = new List<T>();
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }

    // Tổng số trang = làm tròn lên (tổng bản ghi / số bản ghi mỗi trang)
    public int TotalPages
    {
        get
        {
            return (int)Math.Ceiling(TotalCount / (double)PageSize);
        }
    }

    public bool HasPreviousPage
    {
        get
        {
            return Page > 1;
        }
    }

    public bool HasNextPage
    {
        get
        {
            return Page < TotalPages;
        }
    }
}
