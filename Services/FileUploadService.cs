using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace MiniTrainingCenterCatalog.Mvc.Services;

public class FileUploadService : IFileUploadService
{
    private const long MaxFileSize = 2 * 1024 * 1024;

    private static readonly HashSet<string> AllowedExtensions =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".gif",
            ".webp"
        };

    private static readonly HashSet<string> AllowedContentTypes =
        new(StringComparer.OrdinalIgnoreCase)
        {
            "image/jpeg",
            "image/png",
            "image/gif",
            "image/webp"
        };

    private readonly IWebHostEnvironment _environment;

    public FileUploadService(
        IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<FileUploadResult> SaveCourseThumbnailAsync(
        IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return Fail("Please choose an image file.");
        }

        if (file.Length > MaxFileSize)
        {
            return Fail("Image must not exceed 2MB.");
        }

        var extension =
            Path.GetExtension(file.FileName);

        if (string.IsNullOrWhiteSpace(extension) ||
            !AllowedExtensions.Contains(extension))
        {
            return Fail("Only .jpg, .jpeg, .png, .gif and .webp files are allowed.");
        }

        if (!AllowedContentTypes.Contains(file.ContentType))
        {
            return Fail("The uploaded file is not a supported image type.");
        }

        var originalFileName = Path.GetFileName(file.FileName);
        if (string.IsNullOrWhiteSpace(originalFileName) ||
            originalFileName != file.FileName)
        {
            return Fail("The uploaded file name is invalid.");
        }

        var uploadRoot = Path.Combine(
            _environment.WebRootPath,
            "uploads",
            "courses");

        Directory.CreateDirectory(uploadRoot);

        var safeFileName =
            $"{Guid.NewGuid():N}{extension.ToLowerInvariant()}";

        var physicalPath =
            Path.Combine(uploadRoot, safeFileName);

        await using var stream =
            new FileStream(physicalPath, FileMode.CreateNew);

        await file.CopyToAsync(stream);

        return new FileUploadResult
        {
            Succeeded = true,
            PublicPath = $"/uploads/courses/{safeFileName}"
        };
    }

    public void DeletePublicFile(
        string? publicPath)
    {
        if (string.IsNullOrWhiteSpace(publicPath) ||
            !publicPath.StartsWith("/uploads/courses/", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var relativePath = publicPath
            .TrimStart('/')
            .Replace('/', Path.DirectorySeparatorChar);

        var physicalPath = Path.GetFullPath(
            Path.Combine(
                _environment.WebRootPath,
                relativePath));

        var uploadRoot = Path.GetFullPath(
            Path.Combine(
                _environment.WebRootPath,
                "uploads",
                "courses"));

        if (!physicalPath.StartsWith(uploadRoot, StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        if (File.Exists(physicalPath))
        {
            File.Delete(physicalPath);
        }
    }

    private static FileUploadResult Fail(
        string message)
    {
        return new FileUploadResult
        {
            Succeeded = false,
            ErrorMessage = message
        };
    }
}
