using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MqttViewer.Models;
using System.Collections.Specialized;
using System.Windows;

namespace MqttViewer.ViewModels
{
    public class TopicTreeViewModel : BaseViewModel
    {
        private TopicTreeModel _model;
        private TopicViewModel _root;
        private TopicViewModel _selectedItem;


        public TopicViewModel root
        {
            get { return _root; }
        }

        public TopicViewModel selectedItem
        {
            get { return _selectedItem; }
            set
            {
                if (value != _selectedItem)
                {
                    _selectedItem = value;
                    NotifyPropertyChanged("selectedItem");
                }
            }
        }

        public TopicTreeViewModel(TopicTreeModel model)
        {
            _model = model;
            _model.topics.CollectionChanged += onCollectionChanged;
            _root = new TopicViewModel("");

            foreach (TopicModel topic in _model.topics)
            {
                root.updateTree(topic);
            }
        }

        private void onCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (TopicModel topic in e.NewItems)
                    {
                        root.updateTree(topic);
                    }
                    break;
            }
        }
    }
}
