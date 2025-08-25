using ServiceNowTicketHelper.ViewModels;

namespace ServiceNowTicketHelper.Models
{
    // ViewModelBase�� ��ӹ޾� INotifyPropertyChanged ����� ������ �մϴ�.
    public class TicketTemplate : ViewModelBase
    {
        private string _templateName = string.Empty;
        public string TemplateName
        {
            get => _templateName;
            set { _templateName = value; OnPropertyChanged(); }
        }

        private string _serviceApplication = string.Empty;
        public string ServiceApplication
        {
            get => _serviceApplication;
            set { _serviceApplication = value; OnPropertyChanged(); }
        }

        private string _shortDescription = string.Empty;
        public string ShortDescription
        {
            get => _shortDescription;
            set { _shortDescription = value; OnPropertyChanged(); }
        }

        private string _detailedDescription = string.Empty;
        public string DetailedDescription
        {
            get => _detailedDescription;
            set { _detailedDescription = value; OnPropertyChanged(); }
        }

        // --- ��ǥ �Ӽ��鵵 ��� OnPropertyChanged�� ȣ���ϵ��� ���� ---

        private int _serviceAppX;
        public int ServiceAppX
        {
            get => _serviceAppX;
            set { _serviceAppX = value; OnPropertyChanged(); }
        }

        private int _serviceAppY;
        public int ServiceAppY
        {
            get => _serviceAppY;
            set { _serviceAppY = value; OnPropertyChanged(); }
        }

        private int _shortDescX;
        public int ShortDescX
        {
            get => _shortDescX;
            set { _shortDescX = value; OnPropertyChanged(); }
        }

        private int _shortDescY;
        public int ShortDescY
        {
            get => _shortDescY;
            set { _shortDescY = value; OnPropertyChanged(); }
        }

        private int _detailDescX;
        public int DetailDescX
        {
            get => _detailDescX;
            set { _detailDescX = value; OnPropertyChanged(); }
        }

        private int _detailDescY;
        public int DetailDescY
        {
            get => _detailDescY;
            set { _detailDescY = value; OnPropertyChanged(); }
        }
    }
}