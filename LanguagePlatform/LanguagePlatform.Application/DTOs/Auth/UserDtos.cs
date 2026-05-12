namespace LanguagePlatform.Application.DTOs.Auth;

public class UserDto
{
    public Guid Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string DisplayName => FullName;

    public string Initials => GetInitials();

    public string? AvatarUrl { get; set; }

    public string Role { get; set; } = string.Empty;

    public string RoleLabel => GetRoleLabel();

    public string Status { get; set; } = string.Empty;

    public string StatusLabel => GetStatusLabel();

    public string StatusColor => GetStatusColor();

    public DateTime CreatedAt { get; set; }

    private string GetInitials()
    {
        var parts = FullName
            .Split(' ', StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length == 0)
        {
            return "U";
        }

        if (parts.Length == 1)
        {
            return parts[0][0].ToString().ToUpperInvariant();
        }

        return $"{parts[0][0]}{parts[^1][0]}".ToUpperInvariant();
    }

    private string GetRoleLabel()
    {
        return Role switch
        {
            "Admin" => "Quản trị viên",
            "User" => "Người học",
            _ => Role
        };
    }

    private string GetStatusLabel()
    {
        return Status switch
        {
            "Active" => "Đang hoạt động",
            "Locked" => "Đã khóa",
            _ => Status
        };
    }

    private string GetStatusColor()
    {
        return Status switch
        {
            "Active" => "bg-primary/10 text-primary",
            "Locked" => "bg-error/10 text-error",
            _ => "bg-outline text-outline"
        };
    }
}
