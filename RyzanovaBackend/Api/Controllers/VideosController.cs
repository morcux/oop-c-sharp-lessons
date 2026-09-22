using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/videos")]
public class VideosController : ControllerBase
{
    private readonly VideoService _videoService;

    public VideosController(VideoService videoService)
    {
        _videoService = videoService;
    }

    [HttpGet]
    public async Task<ActionResult<List<VideoResponse>>> GetAll([FromQuery] int limit = 100, [FromQuery] int offset = 0)
    {
        var videos = await _videoService.GetAllVideosAsync(limit, offset);
        return Ok(videos);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<VideoResponse>> GetById(Guid id)
    {
        var video = await _videoService.GetVideoByIdAsync(id);
        return video == null
            ? NotFound(new { message = "Video not found" })
            : Ok(video);
    }

    [HttpGet("theme/{themeId:guid}")]
    public async Task<ActionResult<List<VideoResponse>>> GetByTheme(Guid themeId)
    {
        var videos = await _videoService.GetVideosByThemeAsync(themeId);
        return videos == null
            ? NotFound(new { message = "Theme not found" })
            : Ok(videos);
    }

    [HttpPost]
    public async Task<ActionResult<VideoResponse>> Create([FromBody] CreateVideoRequest request)
    {
        var video = await _videoService.CreateVideoAsync(request);
        return video == null
            ? BadRequest(new { message = "Theme not found" })
            : CreatedAtAction(nameof(GetById), new { id = video.Id }, video);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<VideoResponse>> Update(Guid id, [FromBody] UpdateVideoRequest request)
    {
        var video = await _videoService.UpdateVideoAsync(id, request);
        return video == null
            ? NotFound(new { message = "Video not found" })
            : Ok(video);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _videoService.DeleteVideoAsync(id);
        return deleted
            ? NoContent()
            : NotFound(new { message = "Video not found" });
    }
}
