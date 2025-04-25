using Application.Abstractions.Media;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.Media;

internal class MediaService : IMediaService
{
    private readonly ILogger<MediaService> _logger;
    private readonly string _fileSavePath = "./wwwroot/Media";
    private readonly string _baseUrl;

    public MediaService(IHttpContextAccessor httpContextAccessor, ILogger<MediaService> logger)
    {
        var request = httpContextAccessor.HttpContext?.Request;
        _baseUrl = $"{request!.Scheme}://{request.Host}/Media";
        _logger = logger;
    }

    public string GetImageBaseUrl()
    {
        return _baseUrl;
    }

    public void Remove(string folderName, string fileName)
    {
        var folderPath = Path.Combine(_fileSavePath, folderName);
        var filePath = Path.Combine(folderPath, fileName);

        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path cannot be null or empty.", nameof(filePath));

        _logger.LogInformation("Removing file: {FilePath}", filePath);

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            _logger.LogInformation("File removed: {FilePath}", filePath);
        }
        else
        {
            _logger.LogWarning("File not found: {FilePath}", filePath);
        }
    }

    public async Task<string> SaveAsync(IFormFile mediaFile, string folderName)
    {
        if (mediaFile == null)
            throw new ArgumentNullException(nameof(mediaFile), "Media file cannot be null.");

        _logger.LogInformation("Saving media file: {FileName} to folder: {FolderName}", mediaFile.FileName, folderName);

        var filePath = GetUniqueFilePath(folderName, Path.GetExtension(mediaFile.FileName));
        Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);

        try
        {
            await using var stream = new FileStream(filePath, FileMode.Create);
            await mediaFile.CopyToAsync(stream);
            _logger.LogInformation("File saved successfully: {FilePath}", filePath);
            return Path.GetFileName(filePath);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error saving file: {FileName}", mediaFile.FileName);
            throw;
        }
    }

    private string GetUniqueFilePath(string folderName, string extension)
    {
        var fileName = $"{Guid.NewGuid()}{extension}";
        return Path.Combine(_fileSavePath, folderName, fileName).Replace("\\", "/");
    }
}