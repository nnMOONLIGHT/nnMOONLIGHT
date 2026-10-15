namespace StyleAI.Core.DTOs.Auth.Registrationfull;

public record AuthResponseDto(string Token, string Email, string Role, DateTime ExpiresAt);