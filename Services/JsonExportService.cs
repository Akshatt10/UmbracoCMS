using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace UmbracoProject.Services
{
    public class JsonExportService
    {
        private readonly ILogger<JsonExportService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public JsonExportService(ILogger<JsonExportService> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task ExportContentToFileAsync(string apiUrl, string filePath)
        {
            try
            {
                _logger.LogInformation("Starting JSON export from Umbraco Content Delivery API...");

                using var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync(apiUrl);

                response.EnsureSuccessStatusCode();

                var jsonData = await response.Content.ReadAsStringAsync();

                // Save the JSON response to the specified file path
                await File.WriteAllTextAsync(filePath, jsonData);

                _logger.LogInformation($"JSON data exported successfully to: {filePath}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during JSON export process.");
                throw;
            }
        }
    }
}
