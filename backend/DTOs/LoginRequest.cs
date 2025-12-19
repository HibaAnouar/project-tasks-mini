namespace backend.DTOs;

using System.ComponentModel.DataAnnotations;

public record LoginRequest(
    [Required, EmailAddress] string Email,
    [Required] string Password
);
