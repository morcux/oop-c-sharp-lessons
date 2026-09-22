using Application.DTOs;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services;

public class ThemeService
{
    private readonly IThemeRepository _themeRepository;

    public ThemeService(IThemeRepository themeRepository)
    {
        _themeRepository = themeRepository;
    }

    public async Task<List<ThemeResponse>> GetAllThemesAsync(int limit, int offset)
    {
        var themes = await _themeRepository.GetAllAsync(limit, offset);

        return [.. themes.Select(t => new ThemeResponse(
            t.Id,
            t.Name,
            t.Slug,
            t.Description,
            t.Type.ToString(),
            t.Price,
            [.. t.Videos.Select(v => new VideoResponse(v.Id, v.Title, v.Url, v.SequenceNumber, v.ThemeId))]
        ))];
    }

    public async Task<ThemeResponse?> GetThemeBySlugAsync(string slug)
    {
        var theme = await _themeRepository.GetBySlugAsync(slug);
        return theme == null
            ? null
            : new ThemeResponse(
            theme.Id,
            theme.Name,
            theme.Slug,
            theme.Description,
            theme.Type.ToString(),
            theme.Price,
            [.. theme.Videos.Select(v => new VideoResponse(v.Id, v.Title, v.Url, v.SequenceNumber, v.ThemeId))]
        );
    }

    public async Task<ThemeResponse> CreateThemeAsync(CreateThemeRequest request)
    {
        var themeType = Enum.Parse<ThemeType>(request.Type, true);
        var theme = new Theme(Guid.NewGuid(), request.Name, request.Slug, request.Description, themeType, request.Price);

        await _themeRepository.CreateAsync(theme);

        return new ThemeResponse(
            theme.Id,
            theme.Name,
            theme.Slug,
            theme.Description,
            theme.Type.ToString(),
            theme.Price,
            []
        );
    }
}
