using AntecLMS.Application.Common.Interfaces;

namespace AntecLMS.Infrastructure.Services;

public class FileStorageService : IFileStorageService
{
  private readonly string _basePath;
  private const string UploadsFolder = "uploads/materials";

  public FileStorageService()
  {
    _basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", UploadsFolder);
    if (!Directory.Exists(_basePath))
      Directory.CreateDirectory(_basePath);
  }

  public async Task<string> SaveFileAsync(
    Stream fileStream,
    string fileName,
    CancellationToken ct = default
  )
  {
    var uniqueName = $"{Guid.NewGuid():N}_{fileName}";
    var fullPath = Path.Combine(_basePath, uniqueName);

    await using var stream = new FileStream(fullPath, FileMode.Create);
    await fileStream.CopyToAsync(stream, ct);

    return $"/{UploadsFolder}/{uniqueName}";
  }

  public Task DeleteFileAsync(string filePath, CancellationToken ct = default)
  {
    if (string.IsNullOrEmpty(filePath))
      return Task.CompletedTask;

    var fileName = Path.GetFileName(filePath);
    var fullPath = Path.Combine(_basePath, fileName);

    if (File.Exists(fullPath))
      File.Delete(fullPath);

    return Task.CompletedTask;
  }

  public string GetFileUrl(string relativePath) => relativePath;
}
