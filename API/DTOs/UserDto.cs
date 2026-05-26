namespace API.DTOs;

public class UserDto
{
    // ===============================
    // Safe user data returned to Angular
    // Sensitive Identity fields and passwords are not exposed.
    // ===============================

    public int Id { get; set; }

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public string Token { get; set; } = string.Empty;

    public string[] Roles { get; set; } = [];
}