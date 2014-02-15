using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MqttViewer.Views
{
    public static class SelectedItemObserver
    {
        public static readonly DependencyProperty ObserveProperty = DependencyProperty.RegisterAttached(
            "Observe",
            typeof(bool),
            typeof(SelectedItemObserver),
            new FrameworkPropertyMetadata(OnObserveChanged));

        public static readonly DependencyProperty ObservedSelectedItemProperty = DependencyProperty.RegisterAttached(
            "ObservedSelectedItem",
            typeof(double),
            typeof(SelectedItemObserver));


        public static bool GetObserve(FrameworkElement frameworkElement)
        {
            if (frameworkElement == null)
            {
                throw new ArgumentNullException();
            }
            
            return (bool)frameworkElement.GetValue(ObserveProperty);
        }

        public static void SetObserve(FrameworkElement frameworkElement, bool observe)
        {
            if (frameworkElement == null)
            {
                throw new ArgumentNullException();
            }
            
            frameworkElement.SetValue(ObserveProperty, observe);
        }

        public static object GetObservedSelectedItem(FrameworkElement frameworkElement)
        {
            if (frameworkElement == null)
            {
                throw new ArgumentNullException();
            }
            
            return (double)frameworkElement.GetValue(ObservedSelectedItemProperty);
        }

        public static void SetObservedSelectedItem(FrameworkElement frameworkElement, object observedSelectedItem)
        {
            if (frameworkElement == null)
            {
                throw new ArgumentNullException();
            }

            frameworkElement.SetValue(ObservedSelectedItemProperty, observedSelectedItem);
        }

        private static void OnObserveChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var treeView = (TreeView)dependencyObject;

            if ((bool)e.NewValue)
            {
                treeView.SelectedItemChanged += OnTreeViewSelectedItemChanged;
                UpdateSelectedItemForTreeView(treeView);
            }
            else
            {
                treeView.SelectedItemChanged -= OnTreeViewSelectedItemChanged;
            }
        }

        private static void OnTreeViewSelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            UpdateSelectedItemForTreeView((TreeView)sender);
        }

        private static void UpdateSelectedItemForTreeView(TreeView treeView)
        {
            // WPF 4.0 onwards
            //treeView.SetCurrentValue(ObservedSelectedItemProperty, treeView.SelectedItem);

            // WPF 3.5 and prior
            if (treeView.SelectedItem != null)
            {
                SetObservedSelectedItem(treeView, treeView.SelectedItem);
            }
            else
            {
                SetObservedSelectedItem(treeView, "");
            }
            ////SetObservedHeight(frameworkElement, frameworkElement.ActualHeight);
        }
    }
}
