namespace E_Commerce.Helpers
{
    public static class ImageHelper
    {
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        private const long MaxFileSize = 2 * 1024 * 1024; 

        public static async Task<string> UploadFileAsync(IFormFile image, string folderName)
        {
            if (image == null || image.Length == 0) return null;

            var extension = Path.GetExtension(image.FileName).ToLower();
            if (!AllowedExtensions.Contains(extension))
                throw new InvalidOperationException("Invalid file type.");

            if (image.Length > MaxFileSize)
                throw new InvalidOperationException("File is too large.");

            string relativeFolder = Path.Combine("images", folderName);
            string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativeFolder);

            if (!Directory.Exists(uploadFolder))
            {
                Directory.CreateDirectory(uploadFolder);
            }

            string fileName = $"{Guid.NewGuid()}{extension}";
            string filePath = Path.Combine(uploadFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await image.CopyToAsync(stream);
            }

            return $"/{relativeFolder}/{fileName}".Replace("\\", "/");
        }
    }
}

