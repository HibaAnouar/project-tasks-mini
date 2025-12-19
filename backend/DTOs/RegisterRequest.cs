namespace backend.DTOs;

using System.ComponentModel.DataAnnotations;

public record RegisterRequest(
    [Required, EmailAddress] string Email,
    [Required, MinLength(6)] string Password
);
