using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MqttViewer.Views
{
    /// <summary>
    /// Helper class to make the SelectedItem property of the TreeView bindable.
    /// </summary>
    static class BindableSelectedItemHelper
    {
        #region Properties

        /// <summary>
        /// Gets or sets the SelectedItemProperty.
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty = DependencyProperty.RegisterAttached("SelectedItem", typeof(object), typeof(BindableSelectedItemHelper),
            new FrameworkPropertyMetadata(null, OnSelectedItemPropertyChanged));

        /// <summary>
        /// Gets or sets the Attach property.
        /// </summary>
        public static readonly DependencyProperty AttachProperty = DependencyProperty.RegisterAttached("Attach", typeof(bool), typeof(BindableSelectedItemHelper), new PropertyMetadata(false, Attach));

        /// <summary>
        /// Gets or sets the IsUpdating property.
        /// </summary>
        private static readonly DependencyProperty IsUpdatingProperty = DependencyProperty.RegisterAttached("IsUpdating", typeof(bool), typeof(BindableSelectedItemHelper));

        #endregion

        #region Implementation
        /// <summary>
        /// Set the value of the AttachProperty.
        /// </summary>
        /// <param name="dp">DependencyObject to set the value for.</param>
        /// <param name="value">Value to set.</param>
        public static void SetAttach(DependencyObject dp, bool value)
        {
            dp.SetValue(AttachProperty, value);
        }

        /// <summary>
        /// Get the value of the AttachProperty.
        /// </summary>
        /// <param name="dp">DependencyObject to get the value for.</param>
        /// <returns>Value of the AttachProperty.</returns>
        public static bool GetAttach(DependencyObject dp)
        {
            return (bool)dp.GetValue(AttachProperty);
        }

        /// <summary>
        /// Get the value of the SelectedItemProperty.
        /// </summary>
        /// <param name="dp">DependencyObject to get the value for.</param>
        /// <returns>Value of the SelectedItemProperty.</returns>
        public static string GetSelectedItem(DependencyObject dp)
        {
            return (string)dp.GetValue(SelectedItemProperty);
        }

        /// <summary>
        /// Set the value of the SelectedItemProperty.
        /// </summary>
        /// <param name="dp">DependencyObject to set the value for.</param>
        /// <param name="value">Value to set.</param>
        public static void SetSelectedItem(DependencyObject dp, object value)
        {
            dp.SetValue(SelectedItemProperty, value);
        }

        /// <summary>
        /// Get the value of the IsUpdatingProperty.
        /// </summary>
        /// <param name="dp">DependencyObject to get the value for.</param>
        /// <returns>Value of the IsUpdatingProperty.</returns>
        private static bool GetIsUpdating(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsUpdatingProperty);
        }

        /// <summary>
        /// Set the value of the IsUpdatingProperty.
        /// </summary>
        /// <param name="dp">DependencyObject to set the value for.</param>
        /// <param name="value">Value to set.</param>
        private static void SetIsUpdating(DependencyObject dp, bool value)
        {
            dp.SetValue(IsUpdatingProperty, value);
        }

        /// <summary>
        /// Raised when the AttachProperty changed.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">EventArgs for the event.</param>
        private static void Attach(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TreeView treeListView = sender as TreeView;
            if (treeListView != null)
            {
                if ((bool)e.OldValue)
                    treeListView.SelectedItemChanged -= SelectedItemChanged;

                if ((bool)e.NewValue)
                    treeListView.SelectedItemChanged += SelectedItemChanged;
            }
        }

        /// <summary>
        /// Raised when the SelectedItemProperty changed.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">EventArgs for the event.</param>
        private static void OnSelectedItemPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TreeView treeListView = sender as TreeView;
            if (treeListView != null)
            {
                treeListView.SelectedItemChanged -= SelectedItemChanged;

                if (!(bool)GetIsUpdating(treeListView))
                {
                    foreach (TreeViewItem item in treeListView.Items)
                    {
                        if (item == e.NewValue)
                        {
                            item.IsSelected = true;
                            break;
                        }
                        else
                            item.IsSelected = false;
                    }
                }

                treeListView.SelectedItemChanged += SelectedItemChanged;
            }
        }

        /// <summary>
        /// Raised when the SelectedItem changed.
        /// </summary>
        /// <param name="sender">Object that raised the event.</param>
        /// <param name="e">EventArgs for the event.</param>
        private static void SelectedItemChanged(object sender, RoutedEventArgs e)
        {
            TreeView treeListView = sender as TreeView;
            if (treeListView != null)
            {
                SetIsUpdating(treeListView, true);
                SetSelectedItem(treeListView, treeListView.SelectedItem);
                SetIsUpdating(treeListView, false);
            }
        }
        #endregion
    }
}