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
using MqttViewer.Models;

namespace MqttViewer.ViewModels
{
    /// <summary>
    /// ViewModel for the MQTT connection.
    /// </summary>
    class ConnectionViewModel : BaseViewModel
    {
        private ConnectionModel _model;
        private TopicTreeViewModel _tree;

        private String _brokerAddress = "test.mosquitto.org:1883";
        private DelegateCommand _connectionButtonCommand;


        /// <summary>
        /// Gets the ViewModel for the TopicTree.
        /// </summary>
        public TopicTreeViewModel tree
        {
            get { return _tree; }
            private set
            {
                if (value != _tree)
                {
                    _tree = value;
                    NotifyPropertyChanged("tree");
                }
            }
        }

        /// <summary>
        /// Gets or sets the address for the MQTT broker.
        /// </summary>
        public String brokerAddress
        {
            get { return _brokerAddress; }
            set
            {
                if (value != _brokerAddress)
                {
                    _brokerAddress = value;
                    NotifyPropertyChanged("brokerAddress");
                }
            }
        }

        /// <summary>
        /// Gets the text for the connection button.
        /// </summary>
        public String connectionButtonText
        {
            get
            {
                if (_model.isConnected)
                {
                    return "Disconnect";
                }
                else
                {
                    return "Connect";
                }
            }
        }

        /// <summary>
        /// Gets the command for the connection button.
        /// </summary>
        public DelegateCommand connectionButtonCommand
        {
            get { return _connectionButtonCommand; }
        }

        /// <summary>
        /// Gets a value that indicates if the address field can be edited.
        /// </summary>
        public bool isBrokerAddressEnabled
        {
            get
            {
                return !_model.isConnected;
            }
        }


        /// <summary>
        /// Initializes a new instance of the ConnectionViewModel class.
        /// </summary>
        public ConnectionViewModel()
        {
            _model = new ConnectionModel();
            _model.PropertyChanged += onModelChanged;
            _connectionButtonCommand = new DelegateCommand(x => connectionButtonAction(), x => brokerAddress != "");
        }

        /// <summary>
        /// Connect or disconnect MQTT broker.
        /// </summary>
        private void connectionButtonAction()
        {
            if (_model.isConnected)
            {
                _model.disconnect();
            }
            else
            {
                _model.connect(brokerAddress);
            }
        }

        /// <summary>
        /// Called when the PropertyChanged event is raised.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">EventArgs for the event.</param>
        private void onModelChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "isConnected":
                    NotifyPropertyChanged("connectionButtonText");
                    NotifyPropertyChanged("isBrokerAddressEnabled");
                    break;

                case "topics":
                    tree = new TopicTreeViewModel(_model.topics);
                    break;
            }
        }
    }
}
