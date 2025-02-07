namespace Docsm.Extensions
{
    public static class FileExstension
    {
        public static bool IsValidType(this IFormFile file, string type)
            => file.ContentType.StartsWith("image");
        public static bool IsValidSize(this IFormFile file, int kb)
           => file.Length <= kb * 1024;

        public static async Task<string>UploadAsync(this IFormFile file,params string[] path)
        {
            string uploadPath = Path.Combine(path);
            if (!File.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);
            string fileName = Path.GetRandomFileName() + Path.GetExtension(file.FileName);
            using(Stream stream=File.Create(Path.Combine(uploadPath, fileName)))
            {
                await file.CopyToAsync(stream);
            }
           return fileName;

        }

    }
}
 