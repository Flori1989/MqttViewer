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
using MqttViewer.Models;
using System.Collections.ObjectModel;
using System.Windows;
using System.ComponentModel;

namespace MqttViewer.ViewModels
{
    /// <summary>
    /// ViewModel for the TopicView.
    /// </summary>
    class TopicViewModel : BaseViewModel, IComparable
    {
        private TopicModel _model;
        private ObservableCollection<TopicViewModel> _children;
        private String _fullName;
        private bool _isExpanded;
        private bool _isSelected;
        private int _messageLenghtLimit = 20000;


        /// <summary>
        /// Gets or sets the children of this element.
        /// </summary>
        public ObservableCollection<TopicViewModel> children
        {
            get { return _children; }
            set
            {
                if (value != _children)
                {
                    _children = value;
                    NotifyPropertyChanged("children");
                }
            }
        }

        /// <summary>
        /// Gets the MQTT topic string.
        /// </summary>
        public String fullName
        {
            get
            {
                return _fullName;
            }
        }

        /// <summary>
        /// Gets the last part of the MQTT topic string (after the last slash).
        /// </summary>
        public String name
        {
            get { return fullName.Split('/').Last(); }
        }

        /// <summary>
        /// Gets the display name for this element
        /// </summary>
        public String displayName
        {
            get {
                if (name == "")
                {
                    return "[EMPTY]";
                }
                else
                {
                    return name;
                }
            }
        }

        /// <summary>
        /// Gets the path part of the MQTT topic string (until the last slash).
        /// </summary>
        public String path
        {
            get
            {
                return fullNameToPath(fullName);
            }
        }

        /// <summary>
        /// Gets the payload of the MQTT message.
        /// </summary>
        public byte[] message
        {
            get
            {
                if (_model != null)
                {
                    return _model.message;
                }
                else
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Gets the payload of the MQTT message encoded as string if possible.
        /// </summary>
        public String messageString
        {
            get
            {
                if (message != null)
                {
                    UTF8Encoding encoder = new UTF8Encoding(false, true);
                    try
                    {
                        String s = encoder.GetString(message);
                        if (s.Length > _messageLenghtLimit)
                        {
                            s = s.Remove(_messageLenghtLimit) + "\n[REMOVED]\n";
                        }
                        return s;
                    }
                    catch (ArgumentException)
                    {
                        return "[BINARY]";
                    }
                }
                else
                {
                    return "[NO MESSAGE]";
                }
            }
        }

        /// <summary>
        /// Gets the reception time of the MQTT message.
        /// </summary>
        public String timestamp
        {
            get
            {
                if (_model != null)
                {
                    return _model.timestamp.ToShortDateString() + " " + _model.timestamp.ToLongTimeString();
                }
                else
                {
                    return "";
                }
            }
        }

        /// <summary>
        /// Gets or sets whether the children items are expanded or collapsed. 
        /// </summary>
        public bool isExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (value != _isExpanded)
                {
                    _isExpanded = value;
                    NotifyPropertyChanged("isExpanded");
                }
            }
        }

        /// <summary>
        /// Gets or sets whether this item is selected is selected.
        /// </summary>
        public bool isSelected
        {
            get { return _isSelected; }
            set
            {
                if (value != _isSelected)
                {
                    _isSelected = value;
                    NotifyPropertyChanged("isSelected");
                }
            }
        }


        /// <summary>
        /// Initalizes a new instance of the TopicViewModel class.
        /// </summary>
        /// <param name="model">Model for the new TopicViewModel.</param>
        public TopicViewModel(TopicModel model)
        {
            _children = new ObservableCollection<TopicViewModel>();
            _model = model;
            _model.PropertyChanged += onModelChanged;
            _fullName = "/" + model.topic;
        }

        /// <summary>
        /// Initalizes a new instance of the TopicViewModel class.
        /// </summary>
        /// <param name="name">Name for the new TopicViewModel.</param>
        public TopicViewModel(String name)
        {
            _children = new ObservableCollection<TopicViewModel>();
            _fullName = name;
        }

        /// <summary>
        /// Compares this instance with another instance.
        /// </summary>
        /// <param name="obj">Instance to compare with.</param>
        /// <returns>A value that determines wether the instances are identical.</returns>
        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            TopicViewModel otherViewModel = obj as TopicViewModel;
            if (otherViewModel != null)
                return this.name.CompareTo(otherViewModel.name);
            else
                throw new ArgumentException("Object is not a TopicViewModel");
        }

        /// <summary>
        /// Update this element an its children.
        /// </summary>
        /// <param name="topic">New MQTT message.</param>
        public void updateTree(TopicModel topic)
        {
            TopicViewModel nextStep = null;
            TopicViewModel newViewModel = null;

            String newFullName = "/" + topic.topic;
            String newName = newFullName.Split('/').Last();
            String newPath = fullNameToPath(newFullName);
            String childPath = fullName + "/";
            if (childPath == newPath)
            {
                if (children.Any(x => x.name == newName))
                {
                    nextStep = children.First(x => x.name == newName);
                }
                else
                {
                    newViewModel = new TopicViewModel(topic);
                }
            }
            else if (fullName == newFullName)
            {
                _model = topic;
                _model.PropertyChanged += onModelChanged;
                NotifyPropertyChanged("message");
            }
            else
            {
                String subPath = getSubpath(childPath, newPath);
                String nextPathStep = subPath.Split('/').First();

                if (children.Any(x => x.name == nextPathStep))
                {
                    nextStep = children.First(x => x.name == nextPathStep);
                }
                else
                {
                    newViewModel = new TopicViewModel(childPath + nextPathStep);
                    nextStep = newViewModel;
                }
            }

            if (newViewModel != null)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _children.AddSorted(newViewModel);
                });
            }

            if (nextStep != null)
            {
                nextStep.updateTree(topic);
            }
        }

        /// <summary>
        /// Gets the path of  afull topic string.
        /// </summary>
        /// <param name="name">MQTT topic string.</param>
        /// <returns>Path of the MQTT topic.</returns>
        private String fullNameToPath(String name)
        {
            if(!name.Contains('/')){
                throw new ArgumentException("name does not contain '/'");
            }

            if (name.Last() == '/')
            {
                return name;
            }
            else
            {
                int index = name.LastIndexOf('/') + 1;
                return name.Remove(index);
            }
        }

        /// <summary>
        /// Gets the relative path from currentPath element to the targetPath.
        /// </summary>
        /// <param name="targetPath">Full path of the target element.</param>
        /// <returns>Returns the relative path to the target.</returns>
        private String getSubpath(String currentPath, String targetPath)
        {
            int lenght = currentPath.Length;
            return targetPath.Remove(0, lenght);
        }

        /// <summary>
        /// Called when the PropertyChanged event is raised.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">EventArgs for the event.</param>
        private void onModelChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "message":
                    NotifyPropertyChanged("message");
                    break;

                case "timestamp":
                    NotifyPropertyChanged("message");
                    break;
            }
        }
    }
}
