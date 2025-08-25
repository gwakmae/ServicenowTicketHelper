using ServiceNowTicketHelper.Models;
using ServiceNowTicketHelper.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ServiceNowTicketHelper.Views
{
    public partial class TemplateManagerView : Window
    {
        private Point _startPoint;
        private TicketTemplate? _draggedItem;

        public TemplateManagerView()
        {
            InitializeComponent();
        }

        private void ListBox_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _startPoint = e.GetPosition(null);
            var listBoxItem = FindVisualParent<ListBoxItem>((DependencyObject)e.OriginalSource);
            if (listBoxItem != null)
            {
                _draggedItem = listBoxItem.DataContext as TicketTemplate;
            }
        }

        private void ListBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && _draggedItem != null)
            {
                Point currentPosition = e.GetPosition(null);
                Vector diff = _startPoint - currentPosition;
                if (Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
                {
                    if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                    {
                        DragDrop.DoDragDrop((DependencyObject)sender, _draggedItem, DragDropEffects.Copy);
                        _draggedItem = null;
                    }
                }
            }
        }

        private void ListBox_Drop(object sender, DragEventArgs e)
        {
            var droppedData = e.Data.GetData(typeof(TicketTemplate)) as TicketTemplate;
            var viewModel = DataContext as TemplateManagerViewModel;
            if (droppedData != null && viewModel != null)
            {
                viewModel.CopyTemplateCommand.Execute(droppedData);
            }
        }

        private static T? FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            if (parentObject == null) return null;
            if (parentObject is T parent) return parent;
            return FindVisualParent<T>(parentObject);
        }
    }
}