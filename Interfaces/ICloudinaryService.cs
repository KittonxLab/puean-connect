namespace PuanConnect.Interfaces;

public interface ICloudinaryService
{
  Task<string?> UploadImageAsync(IFormFile file);
  Task<string?> DeleteImageAsync(string publicUrl);
}
