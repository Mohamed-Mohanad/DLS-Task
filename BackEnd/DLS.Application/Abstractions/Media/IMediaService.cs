using Microsoft.AspNetCore.Http;

namespace Application.Abstractions.Media;

public interface IMediaService
{
    void Remove(string folderName, string fileName);
    Task<string> SaveAsync(IFormFile mediaFile, string folderName);
    string GetImageBaseUrl();

}
