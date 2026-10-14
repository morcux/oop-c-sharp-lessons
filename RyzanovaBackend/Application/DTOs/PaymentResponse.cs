namespace Application.DTOs;

public record PaymentLinkResponse(string PaymentUrl);

public record PaymentResponse(
    Guid Id,
    Guid ThemeId,
    string Status,
    DateTime CreatedAt
);