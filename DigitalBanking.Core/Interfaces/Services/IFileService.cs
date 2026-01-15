using Microsoft.AspNetCore.Http;

namespace DigitalBanking.Core.Interfaces.Services;

public interface IFileService
{
    Task<string> SaveAvatarAsync(IFormFile file);
    void DeleteFile(string? filePath);
}
