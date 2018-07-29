using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;

namespace ContentImporter.Controllers
{
    [Route("/import")]
    public class ImportController : Controller
    {
        private Configuration config;
        private string tempFolder;

        public ImportController()
        {
            this.config = new Configuration();
            this.tempFolder = Path.GetTempPath() + "content";
        }

        public string Index()
        {
            return "It's on";
        }

        [HttpPost]
        public IActionResult Import()
        {
            try
            {
                var file = GetFileFromRequest(Request);
                var filePath = BuildFilePath(this.tempFolder, file.FileName);

                HandleDirectories(this.tempFolder);

                SaveFile(filePath, file);

                ZipFile.ExtractToDirectory(filePath, config.ContentFolder);

                return new StatusCodeResult(StatusCodes.Status201Created);
            }
            catch (System.Exception ex)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }

        private void HandleDirectories(params string[] paths)
        {
            foreach (var path in paths)
            {
                if (Directory.Exists(path))
                {
                    Directory.Delete(path, true);
                }

                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }

                Directory.CreateDirectory(path);
            }
        }

        private IFormFile GetFileFromRequest(HttpRequest request) => request.Form.Files[0];

        private void SaveFile(string filePath, IFormFile file)
        {
            if (file.Length > 0)
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
            }
        }

        public static string BuildFilePath(string path, string fileName)
        {
            var isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            var separator = isWindows ? '\\' : '/';
            return path.EndsWith(separator) ? path + fileName : path + separator + fileName;
        }
    }
}