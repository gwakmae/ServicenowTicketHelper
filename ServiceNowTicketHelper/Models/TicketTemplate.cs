using System;

namespace ServiceNowTicketHelper.Models
{
    /// <summary>
    /// 하나의 티켓 템플릿에 대한 데이터를 정의합니다.
    /// </summary>
    public class TicketTemplate
    {
        // UI의 템플릿 목록에 표시될 고유한 이름
        public string TemplateName { get; set; }

        // '서비스/어플리케이션' 필드에 입력될 값
        public string ServiceApplication { get; set; }

        // '간단한 설명' 필드에 입력될 값
        public string ShortDescription { get; set; }

        // '상세 설명' 필드에 입력될 값
        public string DetailedDescription { get; set; }
    }
}