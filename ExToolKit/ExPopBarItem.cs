using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ExToolKit
{
    public class ExPopBarItem:ContentControl
    {
        int _mouseDownTime = -1;
        public static RoutedCommand ItemClick { get; set; } = new RoutedCommand();

        static ExPopBarItem()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExPopBarItem), new FrameworkPropertyMetadata(typeof(ExPopBarItem)));
        }

        #region Click\Command 激发
        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            _mouseDownTime = e.Timestamp;
            base.OnMouseLeftButtonDown(e);
        }
        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
             
            //Console.WriteLine("{0}:{1} = {2}",e.Timestamp , _mouseDownTime, e.Timestamp - _mouseDownTime);
            if (_mouseDownTime != -1 && e.Timestamp - _mouseDownTime <= 300)
            {
                this.RaiseEvent(new RoutedEventArgs(Click, this));

                ItemClick.Execute(null, this);

                if (Command != null) {
                    if (Command is RoutedCommand)
                    {
                        ((RoutedCommand)Command).Execute(CommandParameter, this);
                    }
                    else Command.Execute(CommandParameter);
                    //Console.WriteLine("---{0}---",_mouseDownTime);
                }
            }
            _mouseDownTime = -1;

            base.OnMouseLeftButtonUp(e);
        }

        public static readonly RoutedEvent Click = EventManager.RegisterRoutedEvent(
            "Click", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(ExPopBarItem));

        public event RoutedEventHandler OnClick
        {
            add { AddHandler(Click, value); }
            remove { RemoveHandler(Click, value); }
        }

        #endregion

        #region DP


        public object Icon
        {
            get { return (object)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Icon.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register("Icon", typeof(object), typeof(ExPopBarItem), new PropertyMetadata(null));



        public DataTemplate IconTemplate
        {
            get { return (DataTemplate)GetValue(IconTemplateProperty); }
            set { SetValue(IconTemplateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IconTemplate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IconTemplateProperty =
            DependencyProperty.Register("IconTemplate", typeof(DataTemplate), typeof(ExPopBarItem), new PropertyMetadata(null));



        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(ExPopBarItem), new PropertyMetadata(null));



        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(ExPopBarItem), new PropertyMetadata(null));



        #endregion
    }
}
