namespace Application.DTOs;

public record ThemeResponse(
    Guid Id,
    string Name,
    string Slug,
    string Description,
    string Type,
    int Price,
    List<VideoResponse> Videos
);
