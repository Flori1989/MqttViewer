using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MqttViewer.Models;

namespace MqttViewer.ViewModels
{
    public class ConnectionViewModel : BaseViewModel
    {
        private ConnectionModel _model;
        private TopicTreeViewModel _tree;

        private String _brokerAddress = "test.mosquitto.org:1883";
        private DelegateCommand _buttonCommand;


        public TopicTreeViewModel tree
        {
            get { return _tree; }
            set
            {
                if (value != _tree)
                {
                    _tree = value;
                    NotifyPropertyChanged("tree");
                }
            }
        }

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

        public String buttonText
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

        public DelegateCommand buttonCommand
        {
            get { return _buttonCommand; }
        }

        public bool isBrokerAddressEnabled
        {
            get
            {
                return !_model.isConnected;
            }
        }


        public ConnectionViewModel()
        {
            _model = new ConnectionModel();
            _model.PropertyChanged += onModelChanged;
            _buttonCommand = new DelegateCommand(x => buttonAction(), x => brokerAddress != "");
        }

        private void buttonAction()
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

        private void onModelChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "isConnected":
                    NotifyPropertyChanged("buttonText");
                    NotifyPropertyChanged("isBrokerAddressEnabled");
                    break;

                case "topics":
                    tree = new TopicTreeViewModel(_model.topics);
                    break;
            }
        }
    }
}
