namespace AntecLMS.Application.Common.Interfaces;

public interface IFileStorageService
{
  Task<string> SaveFileAsync(Stream fileStream, string fileName, CancellationToken ct = default);
  Task DeleteFileAsync(string filePath, CancellationToken ct = default);
  string GetFileUrl(string relativePath);
}
