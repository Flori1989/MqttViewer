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
