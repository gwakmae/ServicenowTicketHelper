using ServiceNowTicketHelper.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json; // .NET에 내장된 JSON 라이브러리

namespace ServiceNowTicketHelper.Services
{
    public class JsonTemplateService : ITemplateService
    {
        private readonly string _filePath;
        private readonly JsonSerializerOptions _options;

        public JsonTemplateService()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string appFolderPath = Path.Combine(appDataPath, "ServiceNowTicketHelper");
            Directory.CreateDirectory(appFolderPath);
            _filePath = Path.Combine(appFolderPath, "ServiceNowTicketHelper_Templates.json");
            _options = new JsonSerializerOptions { WriteIndented = true };
        }

        public List<TicketTemplate> LoadTemplates()
        {
            if (!File.Exists(_filePath))
            {
                return new List<TicketTemplate>();
            }
            try
            {
                string jsonString = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<List<TicketTemplate>>(jsonString) ?? new List<TicketTemplate>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading templates: {ex.Message}");
                return new List<TicketTemplate>();
            }
        }

        public void SaveTemplates(List<TicketTemplate> templates)
        {
            try
            {
                string jsonString = JsonSerializer.Serialize(templates, _options);
                File.WriteAllText(_filePath, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving templates: {ex.Message}");
            }
        }
    }
}
