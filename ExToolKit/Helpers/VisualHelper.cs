using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace ExToolKit.Helpers
{
    class VisualHelper
    {
        public static object GetChildren<T>(DependencyObject dp, string name = "") where T : Visual
        {
            var _count = VisualTreeHelper.GetChildrenCount(dp);
            for (int i = 0; i < _count; i++)
            {
                var _children = VisualTreeHelper.GetChild(dp, i);
                if (string.IsNullOrWhiteSpace(name))
                {
                    if (_children.GetType() == typeof(T)) return _children;
                }
                else
                {
                    if (_children.GetType() == typeof(T) && (string)_children.GetValue(FrameworkElement.NameProperty) == name) return _children;
                }
            }
            for (int j = 0; j < _count; j++)
            {
                return GetChildren<T>(VisualTreeHelper.GetChild(dp, j), name);
            }
            return null;
        }
    }
}
