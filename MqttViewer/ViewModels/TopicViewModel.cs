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
    public class TopicViewModel : BaseViewModel, IComparable
    {
        private TopicModel _model;
        private ObservableCollection<TopicViewModel> _children;
        private String _fullName;
        private bool _isExpanded;
        private bool _isSelected;
        private int _messageLenghtLimit = 20000;

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

        public String timestamp
        {
            get
            {
                if (_model != null)
                {
                    return _model.timestamp.ToShortDateString() + " " + _model.timestamp.ToLongTimeString() + "." + _model.timestamp.Millisecond.ToString("D3");
                }
                else
                {
                    return "";
                }
            }
        }

        public String fullName
        {
            get
            {
                return _fullName;
            }
        }

        public String name
        {
            get { return fullName.Split('/').Last(); }
        }

        public String visibleName
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

        public String path
        {
            get
            {
                return fullNameToPath(fullName);
            }
        }

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

        public TopicViewModel(TopicModel model)
        {
            _children = new ObservableCollection<TopicViewModel>();
            _model = model;
            _model.PropertyChanged += onModelChanged;
            _fullName = "/" + model.topic;
        }

        public TopicViewModel(String name)
        {
            _children = new ObservableCollection<TopicViewModel>();
            _fullName = name;
        }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            TopicViewModel otherViewModel = obj as TopicViewModel;
            if (otherViewModel != null)
                return this.name.CompareTo(otherViewModel.name);
            else
                throw new ArgumentException("Object is not a TopicViewModel");
        }

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

        private String getSubpath(String currentPath, String fullPath)
        {
            int lenght = currentPath.Length;
            return fullPath.Remove(0, lenght);
        }

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
