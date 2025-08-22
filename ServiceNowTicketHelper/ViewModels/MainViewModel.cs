using ServiceNowTicketHelper.Models;
using ServiceNowTicketHelper.Services;
using ServiceNowTicketHelper.Commands;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows; // MessageBox를 사용하기 위해 추가

namespace ServiceNowTicketHelper.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        // 1. 서비스 의존성 (외부에서 주입받음)
        private readonly IAutomationService _automationService;
        private readonly ITemplateService _templateService;

        // 2. UI와 바인딩될 속성들
        private ObservableCollection<TicketTemplate> _templates;
        public ObservableCollection<TicketTemplate> Templates
        {
            get => _templates;
            set { _templates = value; OnPropertyChanged(); }
        }

        private TicketTemplate _selectedTemplate;
        public TicketTemplate SelectedTemplate
        {
            get => _selectedTemplate;
            set { _selectedTemplate = value; OnPropertyChanged(); }
        }

        private string _statusText;
        public string StatusText
        {
            get => _statusText;
            set { _statusText = value; OnPropertyChanged(); }
        }

        // 3. UI의 버튼과 바인딩될 커맨드
        public ICommand FillTicketCommand { get; }
        public ICommand ManageTemplatesCommand { get; }

        // 4. 생성자
        public MainViewModel(IAutomationService automationService, ITemplateService templateService)
        {
            // 의존성 주입: 외부에서 생성된 서비스 인스턴스를 받아옴
            _automationService = automationService;
            _templateService = templateService;

            // 커맨드 초기화
            FillTicketCommand = new RelayCommand(ExecuteFillTicket, CanExecuteFillTicket);
            ManageTemplatesCommand = new RelayCommand(ExecuteManageTemplates);

            // 초기 데이터 로드
            LoadTemplates();
            StatusText = "준비 완료. 템플릿을 선택하고 버튼을 누르세요.";
        }

        // 5. 커맨드에 연결될 메서드들
        private async void ExecuteFillTicket(object parameter)
        {
            StatusText = "자동화를 시작합니다...";
            try
            {
                // 자동화 서비스 실행
                await _automationService.FillTicketFormAsync(SelectedTemplate);
                StatusText = "자동화 성공! 웹 페이지를 확인하세요.";
            }
            catch (System.Exception ex)
            {
                StatusText = "오류가 발생했습니다.";
                // 사용자에게 상세 오류 메시지 표시
                MessageBox.Show(ex.Message, "자동화 오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanExecuteFillTicket(object parameter)
        {
            // 선택된 템플릿이 있을 때만 버튼이 활성화되도록 함
            return SelectedTemplate != null;
        }

        private void ExecuteManageTemplates(object parameter)
        {
            // 템플릿 관리 창을 위한 새로운 ViewModel을 생성합니다.
            // ITemplateService를 그대로 전달하여 템플릿 데이터를 공유하게 합니다.
            var templateManagerViewModel = new TemplateManagerViewModel(_templateService);

            // 템플릿 관리 창 View를 생성하고 ViewModel을 DataContext에 연결합니다.
            var templateManagerView = new Views.TemplateManagerView
            {
                DataContext = templateManagerViewModel
            };

            // 창을 띄웁니다. ShowDialog()는 창이 닫힐 때까지 기다립니다.
            templateManagerView.ShowDialog();

            // 관리 창이 닫힌 후, 변경사항이 있을 수 있으므로 메인 창의 템플릿 목록을 새로고침합니다.
            LoadTemplates();
        }

        // 6. 내부 로직 메서드
        private void LoadTemplates()
        {
            // 템플릿 서비스에서 데이터를 불러와 컬렉션에 채움
            var loadedTemplates = _templateService.LoadTemplates();
            Templates = new ObservableCollection<TicketTemplate>(loadedTemplates);
        }
    }
}