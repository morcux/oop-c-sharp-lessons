using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/themes")]
public class ThemesController : ControllerBase
{
    private readonly ThemeService _themeService;

    public ThemesController(ThemeService themeService)
    {
        _themeService = themeService;
    }

    [HttpGet]
    public async Task<ActionResult<List<ThemeResponse>>> GetAll([FromQuery] int limit = 100, [FromQuery] int offset = 0)
    {
        var themes = await _themeService.GetAllThemesAsync(limit, offset);
        return Ok(themes);
    }
    [HttpPost]
    public async Task<ActionResult<ThemeResponse>> Create([FromBody] CreateThemeRequest request)
    {
        var theme = await _themeService.CreateThemeAsync(request);
        return CreatedAtAction(nameof(GetBySlug), new { slug = theme.Slug }, theme);
    }

    [HttpGet("slug/{slug}")]
    public async Task<ActionResult<ThemeResponse>> GetBySlug(string slug)
    {
        var theme = await _themeService.GetThemeBySlugAsync(slug);
        return theme == null ? (ActionResult<ThemeResponse>)NotFound(new { message = "Theme not found" }) : (ActionResult<ThemeResponse>)Ok(theme);
    }

}
