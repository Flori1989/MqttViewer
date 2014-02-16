﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace MqttViewer.ViewModels
{
    /// <summary>
    /// Base class for all ViewModels
    /// </summary>
    abstract class BaseViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Occurs when a property's value changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">Name of the changed property.</param>
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
