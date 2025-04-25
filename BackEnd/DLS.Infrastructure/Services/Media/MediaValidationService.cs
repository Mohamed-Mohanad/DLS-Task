using Application.Abstractions.Media;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services.Media;

internal class MediaValidationService : IMediaValidationService
{
    private readonly string[] _allowedImageExtensions = new string[] {
            ".jpg",
            ".jpeg",
            ".png",
            ".gif",
            ".bmp",
            ".tiff"
        };
    private readonly long _maxFileSize = 20971520;

    public MediaValidationService()
    {
    }

    public bool IsValidImageExtension(IFormFile file)
    {
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        return _allowedImageExtensions.Contains(extension);
    }

    public bool IsValidSize(IFormFile file)
    {
        return file.Length <= _maxFileSize;
    }
}

