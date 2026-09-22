using Application.DTOs;
using Domain.Entities;
using Domain.Repositories;

namespace Application.Services;

public class VideoService
{
    private readonly IVideoRepository _videoRepository;
    private readonly IThemeRepository _themeRepository;

    public VideoService(IVideoRepository videoRepository, IThemeRepository themeRepository)
    {
        _videoRepository = videoRepository;
        _themeRepository = themeRepository;
    }

    public async Task<List<VideoResponse>> GetAllVideosAsync(int limit, int offset)
    {
        var videos = await _videoRepository.GetAllAsync(limit, offset);
        return [.. videos.Select(ToResponse)];
    }

    public async Task<VideoResponse?> GetVideoByIdAsync(Guid id)
    {
        var video = await _videoRepository.GetByIdAsync(id);
        return video == null ? null : ToResponse(video);
    }

    public async Task<List<VideoResponse>?> GetVideosByThemeAsync(Guid themeId)
    {
        var theme = await _themeRepository.GetByIdAsync(themeId);
        if (theme == null)
        {
            return null;
        }

        var videos = await _videoRepository.GetByThemeAsync(themeId);
        return [.. videos.Select(ToResponse)];
    }

    public async Task<VideoResponse?> CreateVideoAsync(CreateVideoRequest request)
    {
        var theme = await _themeRepository.GetByIdAsync(request.ThemeId);
        if (theme == null)
        {
            return null;
        }

        var video = new Video(Guid.NewGuid(), request.ThemeId, request.Title, request.Url, request.SequenceNumber);

        await _videoRepository.CreateAsync(video);

        return ToResponse(video);
    }

    public async Task<VideoResponse?> UpdateVideoAsync(Guid id, UpdateVideoRequest request)
    {
        var video = await _videoRepository.GetByIdAsync(id);
        if (video == null)
        {
            return null;
        }

        video.Update(request.Title, request.Url, request.SequenceNumber);

        await _videoRepository.UpdateAsync(video);

        return ToResponse(video);
    }

    public async Task<bool> DeleteVideoAsync(Guid id)
    {
        var video = await _videoRepository.GetByIdAsync(id);
        if (video == null)
        {
            return false;
        }

        await _videoRepository.DeleteAsync(id);
        return true;
    }

    private static VideoResponse ToResponse(Video video)
    {
        return new(
        video.Id,
        video.Title,
        video.Url,
        video.SequenceNumber,
        video.ThemeId
    );
    }
}
