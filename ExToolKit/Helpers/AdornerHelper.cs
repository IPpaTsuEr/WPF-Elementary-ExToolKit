using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;

namespace ExToolKit.Helpers
{
    public class AdornerHelper : Adorner
    {
        VisualCollection children;

        public AdornerHelper(UIElement e) : base(e)
        {
            children = new VisualCollection(this);
        }


        protected override Visual GetVisualChild(int index)
        {
            return children[index];
        }
        protected override int VisualChildrenCount => children.Count;

        public void AddChildren(FrameworkElement e)
        {
            children.Add(e);
        }
        public void RemoveChildren(FrameworkElement e)
        {
            children.Remove(e);
        }

        protected override Size ArrangeOverride(Size finalSize)
        {
            foreach (UIElement item in children)
            {
                item.Arrange(new Rect(new Point(0, 0), finalSize));
            }
            return base.ArrangeOverride(finalSize);
        }
        protected override Size MeasureOverride(Size constraint)
        {

            foreach (UIElement item in children)
            {
                item.Measure(constraint);
            }
            return base.MeasureOverride(constraint);
        }


        private static void AdornerElementChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            var _newElement = e.NewValue as FrameworkElement;
            if (_newElement == null) return;
            var _sender = dp as FrameworkElement;
            if (_sender == null) return;
            var _oldElement = e.OldValue as FrameworkElement;
            _sender.Dispatcher.BeginInvoke(new Action<FrameworkElement, FrameworkElement, FrameworkElement>((s, n, o) =>
            {
                var _layer = AdornerLayer.GetAdornerLayer(s);
                var _data = GetDataReference(s);
                if (_layer != null)
                {
                    //var gas = _layer.GetAdorners(s);

                    var _adornerManager = new AdornerHelper(s);
                    //if (_data != null)
                    n.DataContext = _data;
                    // n.SetBinding(FrameworkElement.DataContextProperty, new Binding() { Source=DataReferenceProperty });
                    _adornerManager.AddChildren(n);
                    _layer.Add(_adornerManager);
                    
                    //if (o != null) _adornerManager.RemoveChildren(o);
                }
                else
                {
                    Console.WriteLine("Can't Find AdornerLayer "+ s.GetType());
                }
            }), DispatcherPriority.Loaded, _sender, _newElement,_oldElement);

        }

        public static void DataReferenceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var _ele = (FrameworkElement)d;
            if (_ele != null)
            {
                _ele.DataContext = e.NewValue;
            }

        }


        public static object GetAdornerElement(DependencyObject obj)
        {
            return (object)obj.GetValue(AdornerElementProperty);
        }

        public static void SetAdornerElement(DependencyObject obj, object value)
        {
            obj.SetValue(AdornerElementProperty, value);
        }

        // Using a DependencyProperty as the backing store for AdornerElement.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AdornerElementProperty =
            DependencyProperty.RegisterAttached("AdornerElement", typeof(object), typeof(AdornerHelper), new PropertyMetadata(null, new PropertyChangedCallback(AdornerElementChanged)));



        public static object GetDataReference(DependencyObject obj)
        {
            return (object)obj.GetValue(DataReferenceProperty);
        }

        public static void SetDataReference(DependencyObject obj, object value)
        {
            obj.SetValue(DataReferenceProperty, value);
        }

        // Using a DependencyProperty as the backing store for DataReference.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty DataReferenceProperty =
            DependencyProperty.RegisterAttached("DataReference", typeof(object), typeof(AdornerHelper), new PropertyMetadata(null,new PropertyChangedCallback(DataReferenceChanged)));


    }
}
