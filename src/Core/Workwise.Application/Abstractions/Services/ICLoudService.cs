using Microsoft.AspNetCore.Http;

namespace Workwise.Application.Abstractions.Services
{
    public interface ICLoudService
    {
        Task<string> FileCreateAsync(IFormFile file);
        Task<bool> FileDeleteAsync(string filename);
    }
}
