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
