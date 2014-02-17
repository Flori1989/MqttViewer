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
