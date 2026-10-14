using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public record CreatePaymentRequest(
    Guid ThemeId,
    [RegularExpression(@"^\+?[1-9]\d{1,14}$")] string Phone,
    string RedirectPath
);
