using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ExToolKit
{
    public class ExPopBar:HeaderedItemsControl
    {
        bool SubpopHasOpend { get; set; }

        static ExPopBar()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ExPopBar),new FrameworkPropertyMetadata(typeof(ExPopBar)));
        }

        #region Host-Item关联
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is ExPopBarItem;
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ExPopBarItem();
        }
        #endregion

        public ExPopBar()
        {
            Loaded += PopButton_Loaded;
        }

        #region 任意子控件单击将popup关闭

        private void PopButton_Loaded(object sender, RoutedEventArgs e)
        {
            CommandBindings.Add(new CommandBinding(ExPopBarItem.ItemClick, new ExecutedRoutedEventHandler(ExcuteSubItemClick), new CanExecuteRoutedEventHandler(CanExcuteSubItemClick)));
        }
        private void ExcuteSubItemClick(object sender, ExecutedRoutedEventArgs e)
        {
            var _item = e.OriginalSource as ExPopBarItem;
            if (_item == null) return;
            IsSubPoped = false;
        }
        private void CanExcuteSubItemClick(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }
        #endregion

        #region popup打开与关闭

        protected override void OnPreviewMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnPreviewMouseLeftButtonDown(e);
            SubpopHasOpend = IsSubPoped;
        }

        protected override void OnMouseLeftButtonUp(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonUp(e);
            if (SubpopHasOpend) e.Handled = true;
            else if (!IsSubPoped){
                if (Items.Count > 0) IsSubPoped = true;
                else if (Command != null)
                {
                    if(Command is RoutedCommand)
                    {
                        ((RoutedCommand)Command).Execute(CommandParameter, CommandTarget);
                    }
                    else
                    {
                        Command.Execute(CommandParameter);
                    }

                }
            }
        }
        #endregion


        #region DP


        public bool IsSubPoped
        {
            get { return (bool)GetValue(IsSubPopedProperty); }
            set { SetValue(IsSubPopedProperty, value); }
        }

        // Using a DependencyProperty as the backing store for IsSubPoped.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IsSubPopedProperty =
            DependencyProperty.Register("IsSubPoped", typeof(bool), typeof(ExPopBar), new PropertyMetadata(false));



        public SolidColorBrush PopBackgroud
        {
            get { return (SolidColorBrush)GetValue(PopBackgroudProperty); }
            set { SetValue(PopBackgroudProperty, value); }
        }


        // Using a DependencyProperty as the backing store for PopBackgroud.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PopBackgroudProperty =
            DependencyProperty.Register("PopBackgroud", typeof(SolidColorBrush), typeof(ExPopBar), new PropertyMetadata(Brushes.White));

        

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Command.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.Register("Command", typeof(ICommand), typeof(ExPopBar), new PropertyMetadata(null));



        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandParameter.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandParameterProperty =
            DependencyProperty.Register("CommandParameter", typeof(object), typeof(ExPopBar), new PropertyMetadata(null));



        public IInputElement CommandTarget
        {
            get { return (IInputElement)GetValue(CommandTargetProperty); }
            set { SetValue(CommandTargetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CommandTarget.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CommandTargetProperty =
            DependencyProperty.Register("CommandTarget", typeof(IInputElement), typeof(ExPopBar), new PropertyMetadata(null));

        #endregion
    }
}
