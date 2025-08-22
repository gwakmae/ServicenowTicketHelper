using ServiceNowTicketHelper.Models;
using System.Threading.Tasks;

namespace ServiceNowTicketHelper.Services
{
    /// <summary>
    /// 웹 브라우저 자동화 기능에 대한 계약을 정의합니다.
    /// </summary>
    public interface IAutomationService
    {
        /// <summary>
        /// 주어진 템플릿 데이터를 사용하여 ServiceNow 티켓 폼을 비동기적으로 채웁니다.
        /// </summary>
        /// <param name="template">웹 폼에 채울 데이터가 담긴 템플릿 객체</param>
        /// <returns>비동기 작업을 나타내는 Task</returns>
        Task FillTicketFormAsync(TicketTemplate template);
    }
}