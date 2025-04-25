using Microsoft.AspNetCore.Http;

namespace Application.Abstractions.Media;

public interface IMediaValidationService
{
    bool IsValidImageExtension(IFormFile file);
    bool IsValidSize(IFormFile file);

}
