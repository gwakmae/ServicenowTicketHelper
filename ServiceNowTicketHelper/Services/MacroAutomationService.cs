using ServiceNowTicketHelper.Models;
using System;
using System.Threading.Tasks;
using System.Windows;
using InputSimulatorEx;
using InputSimulatorEx.Native;

namespace ServiceNowTicketHelper.Services
{
    public class MacroAutomationService : IAutomationService
    {
        private readonly IInputSimulator _inputSimulator = new InputSimulator();

        public async Task FillTicketFormAsync(TicketTemplate template)
        {
            if (template == null) return;
            try
            {
                var serviceAppPos = (X: template.ServiceAppX, Y: template.ServiceAppY);
                var shortDescPos = (X: template.ShortDescX, Y: template.ShortDescY);
                var detailDescPos = (X: template.DetailDescX, Y: template.DetailDescY);

                // 사용자가 브라우저를 클릭할 시간을 2초간 줍니다.
                await Task.Delay(2000);

                // 배율 조절 로직을 제거합니다.

                // '서비스/어플리케이션' 입력
                MoveAndClick(serviceAppPos.X, serviceAppPos.Y);
                await Task.Delay(200);
                _inputSimulator.Keyboard.TextEntry(template.ServiceApplication);
                await Task.Delay(500);
                _inputSimulator.Keyboard.KeyPress(VirtualKeyCode.RETURN);
                await Task.Delay(5000);
                _inputSimulator.Keyboard.KeyPress(VirtualKeyCode.RETURN);
                await Task.Delay(500);

                // '간단한 설명' 입력
                MoveAndClick(shortDescPos.X, shortDescPos.Y);
                await Task.Delay(200);
                _inputSimulator.Keyboard.TextEntry(template.ShortDescription);
                await Task.Delay(500);

                // '상세 설명' 입력
                MoveAndClick(detailDescPos.X, detailDescPos.Y);
                await Task.Delay(200);
                _inputSimulator.Keyboard.TextEntry(template.DetailedDescription);
                await Task.Delay(500);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"자동화 중 오류가 발생했습니다: {ex.Message}", ex);
            }
        }

        private void MoveAndClick(int x, int y)
        {
            if (x == 0 && y == 0) return;
            var screenWidth = SystemParameters.PrimaryScreenWidth;
            var screenHeight = SystemParameters.PrimaryScreenHeight;
            var absoluteX = (int)Math.Round((double)x * 65535 / screenWidth);
            var absoluteY = (int)Math.Round((double)y * 65535 / screenHeight);
            _inputSimulator.Mouse.MoveMouseTo(absoluteX, absoluteY);
            _inputSimulator.Mouse.LeftButtonClick();
        }
    }
}