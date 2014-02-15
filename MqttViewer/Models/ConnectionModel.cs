using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MqttLib;

namespace MqttViewer.Models
{
    class ConnectionModel : BaseModel
    {
        IMqtt _client;
        TopicTreeModel _topics;


        public TopicTreeModel topics
        {
            get { return _topics; }
            set
            {
                if (value != topics)
                {
                    _topics = value;
                    NotifyPropertyChanged("topics");
                }
            }
        }

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

        void onConnected(object sender, EventArgs e)
        {
            Console.WriteLine("Client connected\n");
            NotifyPropertyChanged("isConnected");
            RegisterSubscriptions();
        }

        void RegisterSubscriptions()
        {
            String subscription = "#";
            //Console.WriteLine("Subscribing to " + subscription + "\n");
            _client.Subscribe(subscription, QoS.BestEfforts);
        }

        void onConnectionLost(object sender, EventArgs e)
        {
            Console.WriteLine("Client connection lost\n");
            NotifyPropertyChanged("isConnected");
        }

        bool onPublishArrived(object sender, PublishArrivedArgs e)
        {
            //Console.WriteLine("Received Message");
            //Console.WriteLine("Topic: " + e.Topic);
            //Console.WriteLine("Payload: " + e.Payload);
            //Console.WriteLine();
            _topics.update(e.Topic, e.Payload.TrimmedBuffer);
            return true;
        }
    }
}
