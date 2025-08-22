using Microsoft.Playwright;
using ServiceNowTicketHelper.Models;
using System;
using System.Threading.Tasks;

namespace ServiceNowTicketHelper.Services
{
    /// <summary>
    /// Playwright를 사용하여 IAutomationService의 브라우저 자동화 기능을 구현합니다.
    /// </summary>
    public class PlaywrightAutomationService : IAutomationService
    {
        // ServiceNow 'Report an Issue' 페이지의 정확한 URL로 변경해야 합니다.
        private const string ServiceNowUrl = "https://tranetechnologies.service-now.com/esc?id=sc_cat_item&sys_id=7ba12284dbec285045be9b1faa96193d";

        public async Task FillTicketFormAsync(TicketTemplate template)
        {
            if (template == null) return;

            try
            {
                // 1. Playwright 인스턴스 생성
                using var playwright = await Playwright.CreateAsync();

                // 2. 브라우저 실행 (Headless: false로 설정하여 사용자에게 보이도록 함)
                await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
                {
                    Headless = false
                });

                // 3. 새 페이지(탭) 생성
                var page = await browser.NewPageAsync();

                // 4. 지정된 ServiceNow URL로 이동
                await page.GotoAsync(ServiceNowUrl);

                // --- 웹 페이지 요소(입력란)를 찾아서 데이터 입력 ---
                // 중요: 아래의 "Selector"는 예시이며, 실제 페이지의 HTML 구조에 맞게 수정해야 합니다.
                // F12 개발자 도구를 사용하여 각 입력란의 id, name, 또는 CSS 선택자를 찾아야 합니다.

                // 5. '서비스/어플리케이션' 필드를 찾아서 값 입력
                // 예시: <input name="service_application_field"> 같은 형태일 경우
                await page.Locator("input[name='service_application_field']").FillAsync(template.ServiceApplication);

                // 6. '간단한 설명' 필드를 찾아서 값 입력
                // 예시: <input id="short_description"> 같은 형태일 경우
                await page.Locator("#short_description").FillAsync(template.ShortDescription);

                // 7. '상세 설명' 필드를 찾아서 값 입력
                // 예시: <textarea class="detailed-description-input"></textarea> 같은 형태일 경우
                await page.Locator("textarea.detailed-description-input").FillAsync(template.DetailedDescription);
            }
            catch (Exception ex)
            {
                // 자동화 중 오류 발생 시 예외를 던져 상위 ViewModel에서 처리하도록 함
                throw new InvalidOperationException($"자동화 중 오류가 발생했습니다: {ex.Message}", ex);
            }
        }
    }
}