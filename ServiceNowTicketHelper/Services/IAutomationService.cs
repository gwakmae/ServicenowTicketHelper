using ServiceNowTicketHelper.Models;
using System.Threading.Tasks;

namespace ServiceNowTicketHelper.Services
{
    /// <summary>
    /// �� ������ �ڵ�ȭ ��ɿ� ���� ����� �����մϴ�.
    /// </summary>
    public interface IAutomationService
    {
        /// <summary>
        /// �־��� ���ø� �����͸� ����Ͽ� ServiceNow Ƽ�� ���� �񵿱������� ä��ϴ�.
        /// </summary>
        /// <param name="template">�� ���� ä�� �����Ͱ� ��� ���ø� ��ü</param>
        /// <returns>�񵿱� �۾��� ��Ÿ���� Task</returns>
        Task FillTicketFormAsync(TicketTemplate template);
    }
}