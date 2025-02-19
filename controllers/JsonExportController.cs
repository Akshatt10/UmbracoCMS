using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using UmbracoProject.Services;

namespace UmbracoProject.Controllers
{
    [Route("umbraco/api/[controller]")]
    public class JsonExportController : ControllerBase
    {
        private readonly JsonExportService _jsonExportService;

        public JsonExportController(JsonExportService jsonExportService)
        {
            _jsonExportService = jsonExportService;
        }

        [HttpGet("export")]
        public async Task<IActionResult> Export()
        {
            // URL for Umbraco Content Delivery API
            string apiUrl = "http://localhost:49884/umbraco/delivery/api/v2/content";
            
            // File path to save the JSON file inside the wwwroot folder
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "App_Data", "content-bundle.json");

            // Call the service to export the content and save it to the file
            await _jsonExportService.ExportContentToFileAsync(apiUrl, filePath);

            return Ok(new { message = "Export successful", filePath });
        }
    }
}
