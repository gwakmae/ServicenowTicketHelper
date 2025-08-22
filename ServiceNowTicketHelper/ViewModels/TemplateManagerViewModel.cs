using ServiceNowTicketHelper.Commands;
using ServiceNowTicketHelper.Models;
using ServiceNowTicketHelper.Services;
using System.Collections.ObjectModel;
using System.Linq; // ToList()를 사용하기 위해 추가
using System.Windows;
using System.Windows.Input;

namespace ServiceNowTicketHelper.ViewModels
{
    public class TemplateManagerViewModel : ViewModelBase
    {
        // 1. 서비스 의존성
        private readonly ITemplateService _templateService;

        // 2. UI와 바인딩될 속성
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

        // 3. UI와 바인딩될 커맨드
        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SaveCommand { get; }

        // 4. 생성자
        public TemplateManagerViewModel(ITemplateService templateService)
        {
            _templateService = templateService;

            // 커맨드 초기화
            AddCommand = new RelayCommand(ExecuteAdd);
            DeleteCommand = new RelayCommand(ExecuteDelete, CanExecuteDelete);
            SaveCommand = new RelayCommand(ExecuteSave);

            // 데이터 로드
            LoadTemplates();
        }

        // 5. 커맨드 실행 메서드들
        private void ExecuteAdd(object parameter)
        {
            // 새 템플릿 객체 생성
            var newTemplate = new TicketTemplate
            {
                TemplateName = "새 템플릿",
                ServiceApplication = "",
                ShortDescription = "",
                DetailedDescription = ""
            };

            // 컬렉션에 추가하고, 새로 추가된 항목을 선택 상태로 만듦
            Templates.Add(newTemplate);
            SelectedTemplate = newTemplate;
        }

        private void ExecuteDelete(object parameter)
        {
            if (SelectedTemplate != null)
            {
                // 사용자에게 삭제 여부 확인
                var result = MessageBox.Show(
                    $"'{SelectedTemplate.TemplateName}' 템플릿을 정말로 삭제하시겠습니까?",
                    "삭제 확인",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    Templates.Remove(SelectedTemplate);
                }
            }
        }

        private bool CanExecuteDelete(object parameter)
        {
            // 선택된 템플릿이 있을 때만 삭제 버튼 활성화
            return SelectedTemplate != null;
        }

        private void ExecuteSave(object parameter)
        {
            try
            {
                // 현재 컬렉션의 상태를 파일에 저장
                _templateService.SaveTemplates(Templates.ToList());
                MessageBox.Show("템플릿이 성공적으로 저장되었습니다.", "저장 완료", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"저장 중 오류가 발생했습니다: {ex.Message}", "저장 오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // 6. 내부 로직 메서드
        private void LoadTemplates()
        {
            var loadedTemplates = _templateService.LoadTemplates();
            Templates = new ObservableCollection<TicketTemplate>(loadedTemplates);
        }
    }
}