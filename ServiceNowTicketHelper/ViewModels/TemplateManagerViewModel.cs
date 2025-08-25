using InputSimulatorEx;
using InputSimulatorEx.Native;
using ServiceNowTicketHelper.Commands;
using ServiceNowTicketHelper.Models;
using ServiceNowTicketHelper.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Forms = System.Windows.Forms;
using Point = System.Drawing.Point;

namespace ServiceNowTicketHelper.ViewModels
{
    public class TemplateManagerViewModel : ViewModelBase
    {
        private readonly ITemplateService _templateService;
        private ObservableCollection<TicketTemplate> _templates = new();
        public ObservableCollection<TicketTemplate> Templates { get => _templates; set { _templates = value; OnPropertyChanged(); } }
        private TicketTemplate? _selectedTemplate;
        public TicketTemplate? SelectedTemplate { get => _selectedTemplate; set { _selectedTemplate = value; OnPropertyChanged(); } }

        public ICommand AddCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SaveCommand { get; }
        public ICommand CaptureServiceAppCoordCommand { get; }
        public ICommand CaptureShortDescCoordCommand { get; }
        public ICommand CaptureDetailDescCoordCommand { get; }
        public ICommand CopyTemplateCommand { get; }

        public TemplateManagerViewModel(ITemplateService templateService)
        {
            _templateService = templateService;
            AddCommand = new RelayCommand(ExecuteAdd);
            DeleteCommand = new RelayCommand(ExecuteDelete, CanExecuteDelete);
            SaveCommand = new RelayCommand(ExecuteSave);

            CaptureServiceAppCoordCommand = new RelayCommand(async (p) => await ExecuteCaptureCoord(SetServiceAppCoord), CanExecuteCaptureCoord);
            CaptureShortDescCoordCommand = new RelayCommand(async (p) => await ExecuteCaptureCoord(SetShortDescCoord), CanExecuteCaptureCoord);
            CaptureDetailDescCoordCommand = new RelayCommand(async (p) => await ExecuteCaptureCoord(SetDetailDescCoord), CanExecuteCaptureCoord);
            CopyTemplateCommand = new RelayCommand(ExecuteCopyTemplate);
            LoadTemplates();
        }

        private void ExecuteCopyTemplate(object? parameter)
        {
            if (parameter is not TicketTemplate originalTemplate) return;

            var newTemplate = new TicketTemplate
            {
                TemplateName = $"{originalTemplate.TemplateName} (Copy)",
                ServiceApplication = originalTemplate.ServiceApplication,
                ShortDescription = originalTemplate.ShortDescription,
                DetailedDescription = originalTemplate.DetailedDescription,
                ServiceAppX = originalTemplate.ServiceAppX,
                ServiceAppY = originalTemplate.ServiceAppY,
                ShortDescX = originalTemplate.ShortDescX,
                ShortDescY = originalTemplate.ShortDescY,
                DetailDescX = originalTemplate.DetailDescX,
                DetailDescY = originalTemplate.DetailDescY
            };
            Templates.Add(newTemplate);
        }

        private bool CanExecuteCaptureCoord(object? parameter) => SelectedTemplate != null;
        private bool CanExecuteDelete(object? parameter) => SelectedTemplate != null;

        private void SetServiceAppCoord(Point p) { if (SelectedTemplate != null) { SelectedTemplate.ServiceAppX = p.X; SelectedTemplate.ServiceAppY = p.Y; } }
        private void SetShortDescCoord(Point p) { if (SelectedTemplate != null) { SelectedTemplate.ShortDescX = p.X; SelectedTemplate.ShortDescY = p.Y; } }
        private void SetDetailDescCoord(Point p) { if (SelectedTemplate != null) { SelectedTemplate.DetailDescX = p.X; SelectedTemplate.DetailDescY = p.Y; } }

        private async Task ExecuteCaptureCoord(Action<Point> setCoordAction)
        {
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            if (window == null) return;

            // --- ▼▼▼ 여기가 수정되었습니다 (배율 조절 로직 제거) ▼▼▼ ---
            MessageBox.Show("3초 후에 마우스 커서의 위치를 캡처합니다.\n" +
                          "(브라우저는 75% 배율 상태여야 합니다)\n\n" +
                          "원하는 입력창 위로 마우스를 이동시키세요.", "좌표 캡처", MessageBoxButton.OK, MessageBoxImage.Information);

            window.Topmost = true;
            await Task.Delay(3000); // 좌표를 찍을 시간

            Point cursorPosition = Forms.Cursor.Position;
            setCoordAction(cursorPosition);

            window.Topmost = false;
            window.Activate();
            MessageBox.Show($"좌표가 캡처되었습니다: X={cursorPosition.X}, Y={cursorPosition.Y}", "캡처 완료");
            // --- ▲▲▲ 수정 완료 ▲▲▲ ---
        }

        private void ExecuteAdd(object? parameter)
        {
            var newTemplate = new TicketTemplate { TemplateName = "새 템플릿" };
            Templates.Add(newTemplate);
            SelectedTemplate = newTemplate;
        }

        private void ExecuteDelete(object? parameter)
        {
            if (SelectedTemplate != null)
            {
                if (MessageBox.Show($"'{SelectedTemplate.TemplateName}' 템플릿을 정말로 삭제하시겠습니까?", "삭제 확인", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    Templates.Remove(SelectedTemplate);
                }
            }
        }

        private void ExecuteSave(object? parameter)
        {
            _templateService.SaveTemplates(Templates.ToList());
            MessageBox.Show("템플릿이 성공적으로 저장되었습니다.", "저장 완료", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void LoadTemplates()
        {
            Templates = new ObservableCollection<TicketTemplate>(_templateService.LoadTemplates());
        }
    }
}