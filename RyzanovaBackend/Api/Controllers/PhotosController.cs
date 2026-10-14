using Application.DTOs;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/v1/photos")]
public class PhotosController : ControllerBase
{
    private readonly PhotoService _photoService;

    public PhotosController(PhotoService photoService)
    {
        _photoService = photoService;
    }

    [HttpGet]
    public async Task<ActionResult<List<PhotoResponse>>> GetAll([FromQuery] int limit = 100, [FromQuery] int offset = 0)
    {
        var photos = await _photoService.GetAllPhotosAsync(limit, offset);
        return Ok(photos);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PhotoResponse>> GetById(Guid id)
    {
        var photo = await _photoService.GetPhotoByIdAsync(id);
        return photo == null
            ? NotFound(new { message = "Photo not found" })
            : Ok(photo);
    }

    [HttpGet("theme/{themeId:guid}")]
    public async Task<ActionResult<List<PhotoResponse>>> GetByTheme(Guid themeId)
    {
        var photos = await _photoService.GetPhotosByThemeAsync(themeId);
        return photos == null
            ? NotFound(new { message = "Theme not found" })
            : Ok(photos);
    }

    [HttpPost]
    public async Task<ActionResult<PhotoResponse>> Create([FromBody] CreatePhotoRequest request)
    {
        var photo = await _photoService.CreatePhotoAsync(request);
        return photo == null
            ? BadRequest(new { message = "Theme not found" })
            : CreatedAtAction(nameof(GetById), new { id = photo.Id }, photo);
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<PhotoResponse>> Update(Guid id, [FromBody] UpdatePhotoRequest request)
    {
        var photo = await _photoService.UpdatePhotoAsync(id, request);
        return photo == null
            ? NotFound(new { message = "Photo not found" })
            : Ok(photo);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleted = await _photoService.DeletePhotoAsync(id);
        return deleted
            ? NoContent()
            : NotFound(new { message = "Photo not found" });
    }
}
