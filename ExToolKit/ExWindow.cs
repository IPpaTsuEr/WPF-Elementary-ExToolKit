using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ExToolKit
{
    public class ExWindow: Window
    {
        static ExWindow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExWindow), new FrameworkPropertyMetadata(typeof(ExWindow)));
        }

        public object FunctionBar
        {
            get { return (object)GetValue(FunctionBarProperty); }
            set { SetValue(FunctionBarProperty, value); }
        }

        // Using a DependencyProperty as the backing store for FunctionBar.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty FunctionBarProperty =
            DependencyProperty.Register("FunctionBar", typeof(object), typeof(ExWindow), new PropertyMetadata(null));


    }
}
