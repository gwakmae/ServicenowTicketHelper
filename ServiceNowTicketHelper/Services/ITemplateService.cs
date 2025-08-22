using ServiceNowTicketHelper.Models;
using System.Collections.Generic;

namespace ServiceNowTicketHelper.Services
{
    /// <summary>
    /// 템플릿 데이터를 로드하고 저장하는 기능에 대한 계약을 정의합니다.
    /// </summary>
    public interface ITemplateService
    {
        /// <summary>
        /// 저장소(예: 파일)에서 모든 티켓 템플릿을 불러옵니다.
        /// </summary>
        /// <returns>템플릿의 리스트</returns>
        List<TicketTemplate> LoadTemplates();

        /// <summary>
        /// 제공된 티켓 템플릿 리스트를 저장소에 저장합니다.
        /// </summary>
        /// <param name="templates">저장할 템플릿 리스트</param>
        void SaveTemplates(List<TicketTemplate> templates);
    }
}