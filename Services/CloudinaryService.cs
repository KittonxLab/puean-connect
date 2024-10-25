using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using PuanConnect.Interfaces;

namespace PuanConnect.Services;

public class CloudinaryService : ICloudinaryService
{
  private readonly Cloudinary _cloudinary;

  public CloudinaryService(IConfiguration configuration)
  {
    Account account = new Account(
        configuration["Cloudinary:CloudName"],
        configuration["Cloudinary:ApiKey"],
        configuration["Cloudinary:ApiSecret"]
    );

    _cloudinary = new Cloudinary(account);
  }

  public async Task<string?> UploadImageAsync(IFormFile file)
  {
    if (file == null || file.Length == 0)
    {
      return null;
    }

    using (var stream = file.OpenReadStream())
    {
      var uploadParams = new ImageUploadParams
      {
        File = new FileDescription(file.FileName, stream),
      };

      var uploadResult = await _cloudinary.UploadAsync(uploadParams);

      return uploadResult.SecureUrl.AbsoluteUri;
    }
  }

  public async Task<string?> DeleteImageAsync(string publicUrl)
  {
    var uri = new Uri(publicUrl);
    var publicId = Path.GetFileNameWithoutExtension(uri.Segments.Last());

    var deleteParams = new DeletionParams(publicId);

    var result = await _cloudinary.DestroyAsync(deleteParams);

    return result.Result == "ok" ? publicId : null;
  }
}