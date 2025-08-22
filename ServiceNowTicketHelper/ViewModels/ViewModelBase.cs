using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ServiceNowTicketHelper.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        // event 뒤에 '?'를 추가하여 null이 될 수 있음을 명시합니다.
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}