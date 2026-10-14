using Application.DTOs;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services;

public class PhotoService
{
    private readonly IPhotoRepository _photoRepository;
    private readonly IThemeRepository _themeRepository;

    public PhotoService(IPhotoRepository photoRepository, IThemeRepository themeRepository)
    {
        _photoRepository = photoRepository;
        _themeRepository = themeRepository;
    }

    public async Task<List<PhotoResponse>> GetAllPhotosAsync(int limit, int offset)
    {
        var photos = await _photoRepository.GetAllAsync(limit, offset);
        return [.. photos.Select(ToResponse)];
    }

    public async Task<PhotoResponse?> GetPhotoByIdAsync(Guid id)
    {
        var photo = await _photoRepository.GetByIdAsync(id);
        return photo == null ? null : ToResponse(photo);
    }

    public async Task<List<PhotoResponse>?> GetPhotosByThemeAsync(Guid themeId)
    {
        var theme = await _themeRepository.GetByIdAsync(themeId);
        if (theme == null)
        {
            return null;
        }

        var photos = await _photoRepository.GetByThemeAsync(themeId);
        return [.. photos.Select(ToResponse)];
    }

    public async Task<PhotoResponse?> CreatePhotoAsync(CreatePhotoRequest request)
    {
        var theme = await _themeRepository.GetByIdAsync(request.ThemeId);
        if (theme == null)
        {
            return null;
        }

        var photo = new Photo(Guid.NewGuid(), request.ThemeId, request.Url, request.SequenceNumber);

        await _photoRepository.CreateAsync(photo);

        return ToResponse(photo);
    }

    public async Task<PhotoResponse?> UpdatePhotoAsync(Guid id, UpdatePhotoRequest request)
    {
        var photo = await _photoRepository.GetByIdAsync(id);
        if (photo == null)
        {
            return null;
        }

        photo.Update(request.Url, request.SequenceNumber);

        await _photoRepository.UpdateAsync(photo);

        return ToResponse(photo);
    }

    public async Task<bool> DeletePhotoAsync(Guid id)
    {
        var photo = await _photoRepository.GetByIdAsync(id);
        if (photo == null)
        {
            return false;
        }

        await _photoRepository.DeleteAsync(id);
        return true;
    }

    private static PhotoResponse ToResponse(Photo photo)
    {
        return new(
        photo.Id,
        photo.Url,
        photo.SequenceNumber,
        photo.ThemeId
    );
    }
}
