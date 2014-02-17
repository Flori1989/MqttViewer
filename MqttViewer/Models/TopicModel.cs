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

namespace MqttViewer.Models
{
    /// <summary>
    /// Model for a MQTT topic.
    /// </summary>
    class TopicModel : BaseModel
    {
        private String _topic;
        private byte[] _message;
        private DateTime _timestamp;

        /// <summary>
        /// Gets the topic string.
        /// </summary>
        public String topic
        {
            get { return _topic; }
        }

        /// <summary>
        /// Gets the last received message from this topic.
        /// </summary>
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

        /// <summary>
        /// Gets the reception time of the last message.
        /// </summary>
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


        /// <summary>
        /// Initializes a new instance of the TopicModel class.
        /// </summary>
        /// <param name="topic">Topic name for the new TopicModel.</param>
        /// <param name="message">Initial message for the new TopicModel..</param>
        public TopicModel(String topic, byte[] message)
        {
            _topic = topic;
            _message = message;
            _timestamp = DateTime.Now;
        }
    }
}
