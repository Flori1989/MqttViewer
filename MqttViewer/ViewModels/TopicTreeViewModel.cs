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
    /// <summary>
    /// ViewModel for the TopicTreeView.
    /// </summary>
    class TopicTreeViewModel : BaseViewModel
    {
        private TopicTreeModel _model;
        private TopicViewModel _root;
        private TopicViewModel _selectedItem;


        /// <summary>
        /// Gets the root element of the topic tree.
        /// </summary>
        public TopicViewModel root
        {
            get { return _root; }
        }

        /// <summary>
        /// Gets or sets the selectedItem of the TopicTree.
        /// </summary>
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


        /// <summary>
        /// Initializes a new instance of the TopicTreeViewModel class.
        /// </summary>
        /// <param name="model">Model to be visualized.</param>
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

        /// <summary>
        /// Called when the CollectionChanged event is raised.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">EventArgs for the event.</param>
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
