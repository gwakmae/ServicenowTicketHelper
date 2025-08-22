using ServiceNowTicketHelper.Services;
using ServiceNowTicketHelper.ViewModels;
using ServiceNowTicketHelper.Views; // MainWindow를 사용하기 위해 추가
using System.Windows;

namespace ServiceNowTicketHelper
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        // 애플리케이션 전체에서 사용할 서비스와 뷰모델 인스턴스
        private readonly IAutomationService _automationService;
        private readonly ITemplateService _templateService;
        private readonly MainViewModel _mainViewModel;
        private readonly MainWindow _mainWindow;

        public App()
        {
            // 1. 서비스 인스턴스 생성 (프로그램 전체에서 하나만 사용 - Singleton)
            _automationService = new PlaywrightAutomationService();
            _templateService = new JsonTemplateService();

            // 2. ViewModel 인스턴스 생성 및 서비스 주입
            //    MainViewModel이 필요로 하는 서비스들을 생성자의 인자로 전달합니다.
            _mainViewModel = new MainViewModel(_automationService, _templateService);

            // 3. 메인 뷰(창) 인스턴스 생성 및 ViewModel 연결
            //    MainWindow의 DataContext에 우리가 만든 MainViewModel을 할당합니다.
            _mainWindow = new MainWindow
            {
                DataContext = _mainViewModel
            };
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            // 4. 애플리케이션 시작 시, 설정이 완료된 메인 창을 보여줍니다.
            _mainWindow.Show();
            base.OnStartup(e);
        }
    }
}