using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using RestaurantAPI.Exceptions;

namespace RestaurantAPI.Controllers
{
    [Route("files")]
    [Authorize]

    public class FileController : ControllerBase
    {
        [HttpGet]
        [AllowAnonymous]
        [ResponseCache(Duration = 180, VaryByQueryKeys =  new []{ "fileName" })]

        public ActionResult GetFile([FromQuery] string fileName)
        {
            var rootPath = Directory.GetCurrentDirectory();
            var filePath = rootPath + "\\PrivateFiles\\" + fileName;

            var fileExists= System.IO.File.Exists(filePath);
            if (!fileExists)
                throw new NotFoundException("File not exist");

            var fileExtensionContentTypeProvider = new FileExtensionContentTypeProvider();
            fileExtensionContentTypeProvider.TryGetContentType(filePath, out string contentType);
            var fileContent = System.IO.File.ReadAllBytes(filePath);
            
            return File(fileContent, contentType, fileName);
        }

        [HttpPost]
        public ActionResult UploadFile([FromForm] IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var rootPath = Directory.GetCurrentDirectory();
                var fullPath = rootPath + "\\PrivateFiles\\" + file.FileName;
                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }
                return NoContent();
            }

            throw new BadRequestException("File is empty");
        }
    }
}
