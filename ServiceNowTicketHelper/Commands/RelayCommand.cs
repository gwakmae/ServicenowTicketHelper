using System;
using System.Windows.Input;

namespace ServiceNowTicketHelper.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Predicate<object?>? _canExecute; // Predicate 자체가 null일 수 있음

        // event 뒤에 '?'를 추가
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        // Predicate<object?>? canExecute: canExecute 파라미터가 null일 수 있음을 명시
        public RelayCommand(Action<object?> execute, Predicate<object?>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        // object 뒤에 '?'를 추가
        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        // object 뒤에 '?'를 추가
        public void Execute(object? parameter)
        {
            _execute(parameter);
        }
    }
}


