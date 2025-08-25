using ServiceNowTicketHelper.Services;
using ServiceNowTicketHelper.ViewModels;
using ServiceNowTicketHelper.Views;
using System.Windows;
using InputSimulatorEx;
using InputSimulatorEx.Native;
using System.Threading.Tasks;
using System; // MainViewModel 생성자 오류 방지를 위해 추가

namespace ServiceNowTicketHelper
{
    public partial class App : Application
    {
        private IAutomationService? _automationService;
        private ITemplateService? _templateService;
        private MainViewModel? _mainViewModel;
        private MainWindow? _mainWindow;

        public App()
        {
        }

        protected override async void OnStartup(StartupEventArgs e)
        {
            var result = MessageBox.Show("프로그램을 시작합니다.\n\n" +
                                     "1. 엣지(Edge) 브라우저를 최대화하고 100% 배율로 맞춰주세요.\n" +
                                     "2. '확인'을 누른 후, 3초 안에 엣지 브라우저를 클릭하여 활성화해주세요.\n" +
                                     "   (자동으로 배율을 75%로 조절합니다.)",
                                     "초기 설정 안내", MessageBoxButton.OKCancel, MessageBoxImage.Information);

            if (result == MessageBoxResult.Cancel)
            {
                Current.Shutdown();
                return;
            }

            // 사용자가 브라우저를 클릭할 시간
            await Task.Delay(3000);

            // 브라우저 배율을 75%로 설정
            var simulator = new InputSimulator();
            await SetBrowserZoomTo75Percent(simulator);

            MessageBox.Show("브라우저 배율이 75%로 설정되었습니다.\n이제 프로그램을 사용할 수 있습니다.", "설정 완료");

            _automationService = new MacroAutomationService();
            _templateService = new JsonTemplateService();
            _mainViewModel = new MainViewModel(_automationService, _templateService);

            _mainWindow = new MainWindow
            {
                DataContext = _mainViewModel
            };

            _mainWindow.Show();

            base.OnStartup(e);
        }

        private async Task SetBrowserZoomTo75Percent(IInputSimulator simulator)
        {
            simulator.Keyboard.ModifiedKeyStroke(VirtualKeyCode.CONTROL, VirtualKeyCode.VK_0);
            await Task.Delay(500);
            simulator.Keyboard.KeyDown(VirtualKeyCode.CONTROL);
            simulator.Mouse.VerticalScroll(-1);
            await Task.Delay(100);
            simulator.Mouse.VerticalScroll(-1);
            await Task.Delay(100);
            simulator.Mouse.VerticalScroll(-1);
            simulator.Keyboard.KeyUp(VirtualKeyCode.CONTROL);
            await Task.Delay(500);
        }
    }
}