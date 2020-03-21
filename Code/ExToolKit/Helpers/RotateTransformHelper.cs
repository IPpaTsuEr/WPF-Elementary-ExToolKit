using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ExToolKit.Helpers
{
    public class RotateTransformHelper
    {
        private static void ValueChange(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            var _target = dp as FrameworkElement;
            if (_target == null) return;
            
            var _enable = GetEnableAutoLoop(dp);
            if (_enable == null) return;

            if (_enable == true) {
                var _animation = GetAnimation(dp) as AnimationTimeline;
                if (_animation == null) return;
                if (!_target.IsLoaded)
                {
                    _target.Dispatcher.BeginInvoke(new Action(()=> {
                        _target.RenderTransform = new RotateTransform();
                        _target.RenderTransform.BeginAnimation(RotateTransform.AngleProperty, _animation);
                    }),System.Windows.Threading.DispatcherPriority.Loaded);
                }
                else
                {
                    _target.RenderTransform = new RotateTransform();
                    _target.RenderTransform.BeginAnimation(RotateTransform.AngleProperty, _animation);
                }
            }
            else
            {
                if(_target.RenderTransform != null)
                    try
                    {
                        _target.RenderTransform = null;
                       // _target.RenderTransform.BeginAnimation(RotateTransform.AngleProperty, null);
                    }catch(Exception re) { Console.WriteLine("RotateHelper error :",re.Message); }
                    
            }
        }
        #region DA



        public static object GetAnimation(DependencyObject obj)
        {
            return (object)obj.GetValue(AnimationProperty);
        }

        public static void SetAnimation(DependencyObject obj, object value)
        {
            obj.SetValue(AnimationProperty, value);
        }

        // Using a DependencyProperty as the backing store for Animation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AnimationProperty =
            DependencyProperty.RegisterAttached("Animation", typeof(object), typeof(RotateTransformHelper), new PropertyMetadata(null, new PropertyChangedCallback(ValueChange)));



        public static bool? GetEnableAutoLoop(DependencyObject obj)
        {
            return (bool?)obj.GetValue(EnableAutoLoopProperty);
        }

        public static void SetEnableAutoLoop(DependencyObject obj, bool value)
        {
            obj.SetValue(EnableAutoLoopProperty, value);
        }

        // Using a DependencyProperty as the backing store for EnableAutoLoop.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EnableAutoLoopProperty =
            DependencyProperty.RegisterAttached("EnableAutoLoop", typeof(bool?), typeof(RotateTransformHelper), new PropertyMetadata(null,new PropertyChangedCallback(ValueChange)));


        #endregion
    }
}
