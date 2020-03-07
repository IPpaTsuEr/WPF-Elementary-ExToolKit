using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace ExToolKit.Helpers
{
    public class CornerElementHelper
    {

        public static CornerRadius GetCorner(DependencyObject obj)
        {
            return (CornerRadius)obj.GetValue(CornerProperty);
        }

        public static void SetCorner(DependencyObject obj, CornerRadius value)
        {
            obj.SetValue(CornerProperty, value);
        }

        // Using a DependencyProperty as the backing store for Corner.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CornerProperty =
            DependencyProperty.RegisterAttached("Corner", typeof(CornerRadius), typeof(CornerElementHelper), new PropertyMetadata(new CornerRadius(5), new PropertyChangedCallback(CornerValueChanged)));

        private static void CornerValueChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            var _frameElement = dp as FrameworkElement;
            if (_frameElement.IsLoaded)
            {
                SetCornerBinding(dp, (CornerRadius)e.NewValue);
                //BindingOperations.GetBindingExpression(_element, Border.CornerRadiusProperty);
            }
            else
            {
                _frameElement.Dispatcher.BeginInvoke(new Action(() =>
                {
                    SetCornerBinding(dp, (CornerRadius)e.NewValue);
                }), DispatcherPriority.Loaded);
            }
        }
        private static void SetCornerBinding(DependencyObject dp, CornerRadius cornerRadius)
        {
            var _element = VisualHelper.GetChildren<Border>(dp) as Border;
            if (_element == null) return;
            //var b = DependencyPropertyHelper.GetValueSource(_element, Border.CornerRadiusProperty);
            //if (b.BaseValueSource != BaseValueSource.Local)
            //{
            //    _element.SetBinding(Border.CornerRadiusProperty, new Binding() { RelativeSource = RelativeSource.TemplatedParent, Path = new PropertyPath("(CornerElementHelper.Corner)") });
            //}
            _element.CornerRadius = cornerRadius;
        }

    }
}
