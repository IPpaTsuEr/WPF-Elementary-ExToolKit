using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace ExToolKit
{
    public class ScrollPanel : ItemsControl
    {
        RepeatButton Add_RP, Sub_RP;
        ScrollViewer Roll_SV;

        static ScrollPanel()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ScrollPanel), new FrameworkPropertyMetadata(typeof(ScrollPanel)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Add_RP = (RepeatButton)this.GetTemplateChild("PART_AddRepeatButton");
            Sub_RP = (RepeatButton)this.GetTemplateChild("PART_SubRepeatButton");
            Roll_SV = (ScrollViewer)this.GetTemplateChild("PART_Scroll");
            
            if (Add_RP != null) Add_RP.Click += Add_RP_Click;
            if (Sub_RP != null) Sub_RP.Click += Sub_RP_Click;
            if (Roll_SV != null) Roll_SV.CanContentScroll = true;
        }

        private void Sub_RP_Click(object sender, RoutedEventArgs e)
        {
            if (Roll_SV != null)
            {
                if (Orientation == Orientation.Horizontal)
                {
                    Roll_SV.ScrollToHorizontalOffset(Roll_SV.HorizontalOffset - 1);
                }
                else
                {
                    Roll_SV.ScrollToVerticalOffset(Roll_SV.VerticalOffset - 1);
                }
                
                
            }
        }

        private void Add_RP_Click(object sender, RoutedEventArgs e)
        {
            if (Roll_SV != null)
            {
                if (Orientation == Orientation.Horizontal)
                {
                    Roll_SV.ScrollToHorizontalOffset(Roll_SV.HorizontalOffset + 1);
                }
                else
                {
                    Roll_SV.ScrollToVerticalOffset(Roll_SV.VerticalOffset + 1);
                }
            }
        }




        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Orientation.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty OrientationProperty =
            DependencyProperty.Register("Orientation", typeof(Orientation), typeof(ScrollPanel), new PropertyMetadata(Orientation.Horizontal));


    }
}
