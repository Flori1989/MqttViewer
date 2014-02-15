using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace MqttViewer.ViewModels
{
    public class DelegateCommand : ICommand
    {
        private readonly Action<object> _executeDelegate;
        private readonly Func<object, bool> _canExecuteDelegate;
        public DelegateCommand(Action<object> executeDelegate) : this(executeDelegate, null) { }

        public DelegateCommand(Action<object> executeDelegate, Func<object, bool> canExecuteDelegate)
        {
            _executeDelegate = executeDelegate;
            _canExecuteDelegate = canExecuteDelegate;
        }
        public void Execute(object parameter)
        {
            _executeDelegate(parameter);
        }

        public bool CanExecute(object parameter)
        {
            return _canExecuteDelegate == null ? true : _canExecuteDelegate(parameter);
        }
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
