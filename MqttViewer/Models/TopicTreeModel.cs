using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttViewer.Models
{
    public class TopicTreeModel : BaseModel
    {
        private ObservableCollection<TopicModel> _topics;


        public ObservableCollection<TopicModel> topics
        {
            get { return _topics; }
        }


        public TopicTreeModel()
        {
            _topics = new ObservableCollection<TopicModel>();
        }

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
