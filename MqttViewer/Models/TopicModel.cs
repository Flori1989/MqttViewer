using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttViewer.Models
{
    public class TopicModel : BaseModel
    {
        private String _topic;
        private byte[] _message;
        private DateTime _timestamp;


        public String topic
        {
            get { return _topic; }
        }

        public byte[] message
        {
            get { return _message; }
            set
            {
                if (value != _message)
                {
                    _message = value;

                    NotifyPropertyChanged("message");
                }
            }
        }

        public DateTime timestamp
        {
            get { return _timestamp; }
            set
            {
                if (!value.Equals(_timestamp))
                {
                    _timestamp = value;
                    NotifyPropertyChanged("timestamp");
                }
            }
        }


        public TopicModel(String topic, byte[] message)
        {
            _topic = topic;
            _message = message;
            _timestamp = DateTime.Now;
        }
    }
}
