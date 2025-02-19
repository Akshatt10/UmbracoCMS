using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace UmbracoProject.Services
{
    public class JsonLoaderService
    {
        private readonly string _jsonFilePath;
        private Dictionary<string, (string id, long version)> _moduleMappings;

        public JsonLoaderService(IOptions<JsonLoaderOptions> options)
        {
            _jsonFilePath = options.Value.JsonFilePath;
            LoadJson();
        }

        private void LoadJson()
        {
            try
            {
                if (!File.Exists(_jsonFilePath))
                {
                    Console.WriteLine($"JSON file not found at {_jsonFilePath}");
                    return;
                }

                // Read and parse JSON file
                var jsonData = File.ReadAllText(_jsonFilePath);
                var jsonDoc = JsonDocument.Parse(jsonData);

                // Map the JSON structure
                _moduleMappings = jsonDoc.RootElement
                    .GetProperty("modules")
                    .EnumerateArray()
                    .ToDictionary(
                        module => module.GetProperty("name").GetString().ToLower(),
                        module => (
                            id: module.GetProperty("id").GetString(),
                            version: module.GetProperty("version").GetInt64()
                        )
                    );
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading JSON: {ex.Message}");
            }
        }

        public (string id, long version)? GetModuleData(string moduleName)
        {
            _moduleMappings.TryGetValue(moduleName.ToLower(), out var result);
            return result;
        }
    }
}
