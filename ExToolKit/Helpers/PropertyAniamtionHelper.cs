using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace ExToolKit.Helpers
{
    public class PropertyAniamtionHelper
    {
        private static void ValueChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            var _target = dp as FrameworkElement;
            if (_target == null) return;
            var _propertyName = GetTargetPropertyName(dp);
            if (string.IsNullOrWhiteSpace(_propertyName)) return;
            var _dpd = DependencyPropertyDescriptor.FromName(_propertyName, _target.GetType(), _target.GetType());
            if (_dpd == null) return;

            var _targetProperty = _dpd.DependencyProperty;
            

            if ((bool)GetEnabelPropertyAnimation(dp))
            {
                var _animation = GetPropertyAnimation(dp) as AnimationTimeline;
                _target.BeginAnimation(_targetProperty, _animation);
            }
            else
            {
                _target.BeginAnimation(_targetProperty, null);
            }
        }

        #region DA



        public static DependencyProperty GetTargetProperty(DependencyObject obj)
        {
            return (DependencyProperty)obj.GetValue(TargetPropertyProperty);
        }

        public static void SetTargetProperty(DependencyObject obj, DependencyProperty value)
        {
            obj.SetValue(TargetPropertyProperty, value);
        }

        // Using a DependencyProperty as the backing store for TargetProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TargetPropertyProperty =
            DependencyProperty.RegisterAttached("TargetProperty", typeof(DependencyProperty), typeof(PropertyAniamtionHelper), new PropertyMetadata(null));



        public static string GetTargetPropertyName(DependencyObject obj)
        {
            return (string)obj.GetValue(TargetPropertyNameProperty);
        }

        public static void SetTargetPropertyName(DependencyObject obj, string value)
        {
            obj.SetValue(TargetPropertyNameProperty, value);
        }

        // Using a DependencyProperty as the backing store for TargetPropertyName.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TargetPropertyNameProperty =
            DependencyProperty.RegisterAttached("TargetPropertyName", typeof(string), typeof(PropertyAniamtionHelper), new PropertyMetadata(null,new PropertyChangedCallback(ValueChanged)));




        public static object GetPropertyAnimation(DependencyObject obj)
        {
            return (object)obj.GetValue(PropertyAnimationProperty);
        }

        public static void SetPropertyAnimation(DependencyObject obj, object value)
        {
            obj.SetValue(PropertyAnimationProperty, value);
        }

        // Using a DependencyProperty as the backing store for PropertyAnimation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PropertyAnimationProperty =
            DependencyProperty.RegisterAttached("PropertyAnimation", typeof(object), typeof(PropertyAniamtionHelper), new PropertyMetadata(null, new PropertyChangedCallback(ValueChanged)));



        public static bool GetEnabelPropertyAnimation(DependencyObject obj)
        {
            return (bool)obj.GetValue(EnabelPropertyAnimationProperty);
        }

        public static void SetEnabelPropertyAnimation(DependencyObject obj, bool value)
        {
            obj.SetValue(EnabelPropertyAnimationProperty, value);
        }

        // Using a DependencyProperty as the backing store for EnabelPropertyAnimation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnabelPropertyAnimationProperty =
            DependencyProperty.RegisterAttached("EnabelPropertyAnimation", typeof(bool), typeof(PropertyAniamtionHelper), new PropertyMetadata(false, new PropertyChangedCallback(ValueChanged)));


        #endregion
    }
}
