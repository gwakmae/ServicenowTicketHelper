using ServiceNowTicketHelper.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json; // .NET에 내장된 JSON 라이브러리

namespace ServiceNowTicketHelper.Services
{
    /// <summary>
    /// ITemplateService를 구현하여, JSON 파일을 통해 템플릿 데이터를 관리합니다.
    /// </summary>
    public class JsonTemplateService : ITemplateService
    {
        // 템플릿 파일이 저장될 경로와 파일 이름
        private readonly string _filePath;
        private readonly JsonSerializerOptions _options;

        public JsonTemplateService()
        {
            // 1. AppData\Roaming 폴더 경로를 가져옵니다.
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);

            // 2. 우리 프로그램의 데이터를 저장할 폴더 경로를 만듭니다.
            //    (예: C:\Users\YourUser\AppData\Roaming\ServiceNowTicketHelper)
            string appFolderPath = Path.Combine(appDataPath, "ServiceNowTicketHelper");

            // 3. 만약 폴더가 존재하지 않으면 새로 생성합니다.
            Directory.CreateDirectory(appFolderPath);

            // 4. 최종 파일 경로를 설정합니다.
            _filePath = Path.Combine(appFolderPath, "ServiceNowTicketHelper_Templates.json");

            // JSON을 예쁘게 포맷팅하여 저장하기 위한 옵션
            _options = new JsonSerializerOptions { WriteIndented = true };
        }

        public List<TicketTemplate> LoadTemplates()
        {
            // 템플릿 파일이 존재하지 않으면, 빈 리스트를 반환합니다.
            if (!File.Exists(_filePath))
            {
                return new List<TicketTemplate>();
            }

            try
            {
                string jsonString = File.ReadAllText(_filePath);
                // JSON 문자열을 List<TicketTemplate> 객체로 변환(역직렬화)합니다.
                return JsonSerializer.Deserialize<List<TicketTemplate>>(jsonString);
            }
            catch (Exception ex)
            {
                // 파일 읽기 또는 변환 중 오류 발생 시, 로그를 남기거나 예외 처리를 할 수 있습니다.
                // 지금은 간단히 빈 리스트를 반환합니다.
                Console.WriteLine($"Error loading templates: {ex.Message}");
                return new List<TicketTemplate>();
            }
        }

        public void SaveTemplates(List<TicketTemplate> templates)
        {
            try
            {
                // List<TicketTemplate> 객체를 JSON 문자열로 변환(직렬화)합니다.
                string jsonString = JsonSerializer.Serialize(templates, _options);
                File.WriteAllText(_filePath, jsonString);
            }
            catch (Exception ex)
            {
                // 파일 저장 중 오류 발생 시, 처리 로직을 추가할 수 있습니다.
                Console.WriteLine($"Error saving templates: {ex.Message}");
            }
        }
    }
}