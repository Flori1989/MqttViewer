// Copyright (C) 2014 Florian Jungbluth
// 
// This file is part of MqttViewer.
// 
// MqttViewer is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MqttViewer is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MqttViewer.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace MqttViewer.ViewModels
{
    /// <summary>
    /// Bindable Command for WPF elements like buttons.
    /// </summary>
    class DelegateCommand : ICommand
    {
        private readonly Action<object> _executeDelegate;
        private readonly Func<object, bool> _canExecuteDelegate;

        /// <summary>
        /// Initializes a new instance of the DelegateCommand class.
        /// </summary>
        /// <param name="executeDelegate">Delegate to execute.</param>
        public DelegateCommand(Action<object> executeDelegate) : this(executeDelegate, null) { }

        /// <summary>
        /// Initializes a new instance of the DelegateCommand class with executability check.
        /// </summary>
        /// <param name="executeDelegate">Delegate to execute.</param>
        /// <param name="canExecuteDelegate">Delegate for checking if executable.</param>
        public DelegateCommand(Action<object> executeDelegate, Func<object, bool> canExecuteDelegate)
        {
            _executeDelegate = executeDelegate;
            _canExecuteDelegate = canExecuteDelegate;
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="parameter">Parameter for the executeDelegate.</param>
        public void Execute(object parameter)
        {
            _executeDelegate(parameter);
        }


        /// <summary>
        /// Check for executability.
        /// </summary>
        /// <param name="parameter">Parameter for the canExecuteDelegate.</param>
        /// <returns>Returns an value that indicates wether the command can be executed.</returns>
        public bool CanExecute(object parameter)
        {
            return _canExecuteDelegate == null ? true : _canExecuteDelegate(parameter);
        }

        /// <summary>
        /// Occurs when the value of canExecuteChanged changed.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
