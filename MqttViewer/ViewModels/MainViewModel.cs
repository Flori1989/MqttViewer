using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttViewer.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private ConnectionViewModel _connection;


        public ConnectionViewModel connection
        {
            get { return _connection; }
        }

        public MainViewModel()
        {
            _connection = new ConnectionViewModel();
        }
    }
}
