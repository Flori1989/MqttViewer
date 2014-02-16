using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MqttLib;

namespace MqttViewer.Models
{
    /// <summary>
    /// Manages the connection to the MQTT broker.
    /// </summary>
    class ConnectionModel : BaseModel
    {
        private IMqtt _client;
        private TopicTreeModel _topics;

        /// <summary>
        /// Gets the received messages for this connection.
        /// </summary>
        public TopicTreeModel topics
        {
            get { return _topics; }
            private set
            {
                if (value != topics)
                {
                    _topics = value;
                    NotifyPropertyChanged("topics");
                }
            }
        }

        /// <summary>
        /// Gets a value that indicate wether the connection is active.
        /// </summary>
        public bool isConnected
        {
            get
            {
                if (_client != null)
                {
                    return _client.IsConnected;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Connect to a MQTT broker.
        /// </summary>
        /// <param name="brokerAddress">Address/Hostname and Port of the broker to connect to.</param>
        public void connect(String brokerAddress)
        {
            if (_client == null || !_client.IsConnected)
            {
                topics = new TopicTreeModel();
                _client = MqttClientFactory.CreateClient("TCP://" + brokerAddress, "testClient1");
                // Setup some useful client delegate callbacks
                _client.Connected += new ConnectionDelegate(onConnected);
                _client.ConnectionLost += new ConnectionDelegate(onConnectionLost);
                _client.PublishArrived += new PublishArrivedDelegate(onPublishArrived);

                //Console.WriteLine("Client connecting\n");
                _client.Connect(true);
            }
        }

        /// <summary>
        /// Disconnect from the connected broker.
        /// </summary>
        public void disconnect()
        {
            if (_client != null && _client.IsConnected)
            {
                Console.WriteLine("Client disconnecting\n");
                _client.Disconnect();
                Console.WriteLine("Client disconnected\n");
                NotifyPropertyChanged("isConnected");
            }
        }

        /// <summary>
        /// Called when the Connected event is raised.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">EventArgs for the event.</param>
        private void onConnected(object sender, EventArgs e)
        {
            Console.WriteLine("Client connected\n");
            NotifyPropertyChanged("isConnected");
            RegisterSubscriptions();
        }

        /// <summary>
        /// Subscribe to the default topic.
        /// </summary>
        private void RegisterSubscriptions()
        {
            String subscription = "#";
            _client.Subscribe(subscription, QoS.BestEfforts);
        }

        /// <summary>
        /// Called when the ConnectionLost event is raised.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">EventArgs for the event.</param>
        private void onConnectionLost(object sender, EventArgs e)
        {
            Console.WriteLine("Client connection lost\n");
            NotifyPropertyChanged("isConnected");
        }

        /// <summary>
        /// Called when the PublishArrived event is raised.
        /// Updates the TopicTreeModel with the received message.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">EventArgs for the event.</param>
        /// <returns>All messages are accepted.</returns>
        private bool onPublishArrived(object sender, PublishArrivedArgs e)
        {
            _topics.update(e.Topic, e.Payload.TrimmedBuffer);
            return true;
        }
    }
}
