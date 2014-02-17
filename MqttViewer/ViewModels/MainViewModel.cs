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
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttViewer.ViewModels
{
    /// <summary>
    /// ViewModel for the MainWindow.
    /// </summary>
    class MainViewModel : BaseViewModel
    {
        private ConnectionViewModel _connection;


        /// <summary>
        /// Gets the ViewModel for the MQTT connection.
        /// </summary>
        public ConnectionViewModel connection
        {
            get { return _connection; }
        }

        /// <summary>
        /// Initializes a new instance of the MainViewModel.
        /// </summary>
        public MainViewModel()
        {
            _connection = new ConnectionViewModel();
        }
    }
}
