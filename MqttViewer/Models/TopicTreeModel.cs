using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttViewer.Models
{
    /// <summary>
    /// Stores the TopicModels for all received topics.
    /// </summary>
    class TopicTreeModel : BaseModel
    {
        private ObservableCollection<TopicModel> _topics;


        /// <summary>
        /// Gets the TopicModels
        /// </summary>
        public ObservableCollection<TopicModel> topics
        {
            get { return _topics; }
        }


        /// <summary>
        /// Initializes a new instance of the TopicTreeModel class.
        /// </summary>
        public TopicTreeModel()
        {
            _topics = new ObservableCollection<TopicModel>();
        }

        /// <summary>
        /// Stores newly received data.
        /// </summary>
        /// <param name="topic">Topic of the received message.</param>
        /// <param name="message">Content of the received message.</param>
        public void update(String topic, byte[] message)
        {
            TopicModel model = _topics.SingleOrDefault(x => x.topic == topic);

            if (model == null)
            {
                model = new TopicModel(topic, message);
                _topics.Add(model);
            }
            else
            {
                model.message = message;
                model.timestamp = DateTime.Now;
            }

            NotifyPropertyChanged("topics");
        }
    }
}
