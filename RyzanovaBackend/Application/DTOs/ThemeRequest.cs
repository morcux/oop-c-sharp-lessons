namespace Application.DTOs;

public record CreateThemeRequest(
    string Name,
    string Slug,
    string Description,
    string Type,
    int Price
);

