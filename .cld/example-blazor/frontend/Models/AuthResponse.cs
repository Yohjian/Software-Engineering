namespace Frontend.Models;

public record AuthResponse(string Token, string Email);

// ASP.NET Identity error shape
public record IdentityErrorDto(string Code, string Description);
