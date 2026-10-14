
namespace Application.DTOs;

public record PhotoResponse(
    Guid Id,
    string Url,
    int SequenceNumber,
    Guid ThemeId
);
