using Microsoft.AspNetCore.Http;

namespace MiniTrainingCenterCatalog.Mvc.Services;

public interface IFileUploadService
{
    Task<FileUploadResult> SaveCourseThumbnailAsync(
        IFormFile file);

    void DeletePublicFile(
        string? publicPath);
}

public class FileUploadResult
{
    public bool Succeeded { get; set; }

    public string? PublicPath { get; set; }

    public string? ErrorMessage { get; set; }
}
