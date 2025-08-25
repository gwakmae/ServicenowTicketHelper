using ServiceNowTicketHelper.Commands;
using ServiceNowTicketHelper.Models;
using ServiceNowTicketHelper.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace ServiceNowTicketHelper.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private readonly IAutomationService _automationService;
        private readonly ITemplateService _templateService;

        private ObservableCollection<TicketTemplate> _templates = new();
        public ObservableCollection<TicketTemplate> Templates
        {
            get => _templates;
            set { _templates = value; OnPropertyChanged(); }
        }

        private TicketTemplate? _selectedTemplate;
        public TicketTemplate? SelectedTemplate
        {
            get => _selectedTemplate;
            set { _selectedTemplate = value; OnPropertyChanged(); }
        }

        private string _statusText = string.Empty;
        public string StatusText
        {
            get => _statusText;
            set { _statusText = value; OnPropertyChanged(); }
        }

        public ICommand FillTicketCommand { get; }
        public ICommand ManageTemplatesCommand { get; }

        public MainViewModel(IAutomationService automationService, ITemplateService templateService)
        {
            _automationService = automationService;
            _templateService = templateService;

            FillTicketCommand = new RelayCommand(ExecuteFillTicket, CanExecuteFillTicket);
            ManageTemplatesCommand = new RelayCommand(ExecuteManageTemplates);

            LoadTemplates();
            StatusText = "준비 완료. 템플릿을 선택하고 버튼을 누르세요.";
        }

        private async void ExecuteFillTicket(object? parameter)
        {
            if (SelectedTemplate is null) return;
            StatusText = "자동화를 시작합니다...";
            try
            {
                await _automationService.FillTicketFormAsync(SelectedTemplate);
                StatusText = "자동화 성공! 웹 페이지를 확인하세요.";
            }
            catch (System.Exception ex)
            {
                StatusText = "오류가 발생했습니다.";
                MessageBox.Show(ex.Message, "자동화 오류", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool CanExecuteFillTicket(object? parameter)
        {
            return SelectedTemplate != null;
        }

        private void ExecuteManageTemplates(object? parameter)
        {
            var templateManagerViewModel = new TemplateManagerViewModel(_templateService);
            var templateManagerView = new Views.TemplateManagerView
            {
                DataContext = templateManagerViewModel
            };
            templateManagerView.ShowDialog();
            LoadTemplates();
        }

        private void LoadTemplates()
        {
            var loadedTemplates = _templateService.LoadTemplates();
            Templates = new ObservableCollection<TicketTemplate>(loadedTemplates);
        }
    }
}