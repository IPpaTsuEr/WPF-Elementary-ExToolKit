using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ExToolKit.Helpers
{
    class SetWindowCommandsBinding
    {
        Window _window;
        public SetWindowCommandsBinding(Window window)
        {
            _window = window;
        }

        public void BindingCommands()
        {
            _window.CommandBindings.Add(new CommandBinding(SystemCommands.CloseWindowCommand, CloseWindow));
            _window.CommandBindings.Add(new CommandBinding(SystemCommands.MaximizeWindowCommand, MaxSizeWindow, CanMaxSizeWindow));
            _window.CommandBindings.Add(new CommandBinding(SystemCommands.MinimizeWindowCommand, MiniSizeWindow, CanMiniSizeWindow));
            _window.CommandBindings.Add(new CommandBinding(SystemCommands.RestoreWindowCommand, RestoreWindow, CanMaxSizeWindow));
            _window.CommandBindings.Add(new CommandBinding(SystemCommands.ShowSystemMenuCommand, ShowMenu));
        }

        private void CloseWindow(object sender, ExecutedRoutedEventArgs e)
        {
            _window.Close();
        }

        private void MaxSizeWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(_window);
            e.Handled = true;
        }
        private void MiniSizeWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(_window);
            e.Handled = true;
        }
        private void RestoreWindow(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(_window);
            e.Handled = true;
        }

        private void CanMaxSizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _window.ResizeMode == ResizeMode.CanResize || _window.ResizeMode == ResizeMode.CanResizeWithGrip;
        }
        private void CanMiniSizeWindow(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = _window.ResizeMode != ResizeMode.NoResize;
        }

        private void ShowMenu(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.ShowSystemMenu(_window, _window.PointToScreen(Mouse.GetPosition(_window)));
            e.Handled = true;
        }
    }

    class ExWindowHelper
    {


        public static bool GetBindingWindowCommands(DependencyObject obj)
        {
            return (bool)obj.GetValue(BindingWindowCommandsProperty);
        }

        public static void SetBindingWindowCommands(DependencyObject obj, bool value)
        {
            obj.SetValue(BindingWindowCommandsProperty, value);
        }

        // Using a DependencyProperty as the backing store for BindingWindowCommands.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty BindingWindowCommandsProperty =
            DependencyProperty.RegisterAttached("BindingWindowCommands", typeof(bool), typeof(ExWindowHelper), new PropertyMetadata(false, new PropertyChangedCallback(BindingWindowCommandsChanged)));

        private static void BindingWindowCommandsChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            var _window = dp as Window;
            if (_window != null && (bool)e.NewValue == true)
            {
                var _seter = new SetWindowCommandsBinding(_window);
                _seter.BindingCommands();
            }
        }

    }
}
