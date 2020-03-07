using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ExToolKit
{
    /// <summary>
    /// 按照步骤 1a 或 1b 操作，然后执行步骤 2 以在 XAML 文件中使用此自定义控件。
    ///
    /// 步骤 1a) 在当前项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:ExToolKit"
    ///
    ///
    /// 步骤 1b) 在其他项目中存在的 XAML 文件中使用该自定义控件。
    /// 将此 XmlNamespace 特性添加到要使用该特性的标记文件的根 
    /// 元素中: 
    ///
    ///     xmlns:MyNamespace="clr-namespace:ExToolKit;assembly=ExToolKit"
    ///
    /// 您还需要添加一个从 XAML 文件所在的项目到此项目的项目引用，
    /// 并重新生成以避免编译错误: 
    ///
    ///     在解决方案资源管理器中右击目标项目，然后依次单击
    ///     “添加引用”->“项目”->[选择此项目]
    ///
    ///
    /// 步骤 2)
    /// 继续操作并在 XAML 文件中使用控件。
    ///
    ///     <MyNamespace:CustomControl1/>
    ///
    /// </summary>
    [ContentProperty("Content")]
    public class ArcProgress : Control
    {
        static ArcProgress()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ArcProgress), new FrameworkPropertyMetadata(typeof(ArcProgress)));
        }


        public Point CalculatePoint(double angle)
        {
            Point SP = new Point(Center.X, Center.Y - Radius);
            double arc = 0.0;
            if (angle <= 90)
            {
                arc = Math.PI * angle / 180;
                LargeArc = false;
                return new Point(Math.Sin(arc) * Radius + SP.X, SP.Y + Radius - Math.Cos(arc) * Radius);
            }
            else if (angle <= 180)
            {
                arc = Math.PI * (angle - 90) / 180;
                LargeArc = false;
                return new Point(Math.Cos(arc) * Radius + SP.X, Math.Sin(arc) * Radius + SP.Y + Radius);
            }
            else if (angle <= 270)
            {
                arc = Math.PI * (angle - 180) / 180;
                LargeArc = true;
                return new Point(SP.X - Math.Sin(arc) * Radius, Math.Cos(arc) * Radius + SP.Y + Radius);
            }
            else if (angle < 360)
            {
                arc = Math.PI * (angle - 270) / 180;
                LargeArc = true;
                return new Point(SP.X - Math.Cos(arc) * Radius, SP.Y + Radius - Math.Sin(arc) * Radius);

            }
            else if(angle == 360)
            {
                LargeArc = true;
                return new Point(StartPoint.X - 0.0001, StartPoint.Y);
            }
            else
            {
                angle %= 360;
                var  p = CalculatePoint(angle);
                LargeArc = true;
                return p;
            }

        }


        public void Updata()
        {
            StartPoint = CalculatePoint(StartAngle);
            EndPoint = CalculatePoint(EndAngle + StartAngle);
            ArcSize = new Size(Radius, Radius);
            MakeData();

        }

        public void MakeData()
        {
            ArcSegment asm = new ArcSegment(EndPoint, new Size(Radius, Radius), 0, LargeArc, SweepDirection.Clockwise, true);
            
            PathSegmentCollection psc = new PathSegmentCollection();
            psc.Add(asm);
            PathFigure pf = new PathFigure();
            pf.StartPoint = StartPoint;
            pf.Segments = psc;
            PathFigureCollection pfc = new PathFigureCollection();
            pfc.Add(pf);
            PathGeometry pg = new PathGeometry();
            pg.Figures = pfc;
            GeometryData = pg;
        }


        private static void ValueChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            var sender = dp as ArcProgress;
            if(sender?.IsLoaded  == true) sender.Updata();
            else
            {
                sender?.Dispatcher.BeginInvoke(new Action(() =>{
                    sender.Updata();
                }),DispatcherPriority.Loaded);
            }
        }


        #region DP




        public object Content
        {
            get { return (object)GetValue(ContentProperty); }
            set { SetValue(ContentProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Content.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register("Content", typeof(object), typeof(ArcProgress), new PropertyMetadata(null));



        public Geometry GeometryData
        {
            get { return (Geometry)GetValue(GeometryDataProperty); }
            set { SetValue(GeometryDataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GeometryData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GeometryDataProperty =
            DependencyProperty.Register("GeometryData", typeof(Geometry), typeof(ArcProgress), new PropertyMetadata(null));



        public double StartAngle
        {
            get { return (double)GetValue(StartAngleProperty); }
            set { SetValue(StartAngleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartAngleProperty =
            DependencyProperty.Register("StartAngle", typeof(double), typeof(ArcProgress), new PropertyMetadata(0.0, new PropertyChangedCallback(ValueChanged)));



        public double EndAngle
        {
            get { return (double)GetValue(EndAngleProperty); }
            set { SetValue(EndAngleProperty, value);}
        }

        // Using a DependencyProperty as the backing store for EndAngle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndAngleProperty =
            DependencyProperty.Register("EndAngle", typeof(double), typeof(ArcProgress), new PropertyMetadata(0.0,new PropertyChangedCallback(ValueChanged)));



        public double StrokeThickness
        {
            get { return (double)GetValue(StrokeThicknessProperty); }
            set { SetValue(StrokeThicknessProperty, value);}
        }

        // Using a DependencyProperty as the backing store for StorkeThickness.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StrokeThicknessProperty =
            DependencyProperty.Register("StrokeThickness", typeof(double), typeof(ArcProgress), new PropertyMetadata(1.0));



        public Point Center
        {
            get { return (Point)GetValue(CenterProperty); }
            set { SetValue(CenterProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Center.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CenterProperty =
            DependencyProperty.Register("Center", typeof(Point), typeof(ArcProgress), new PropertyMetadata(new Point(0,0), new PropertyChangedCallback(ValueChanged)));



        public double Radius
        {
            get { return (double)GetValue(RadiusProperty); }
            set { SetValue(RadiusProperty, value);  }
        }

        // Using a DependencyProperty as the backing store for Radius.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RadiusProperty =
            DependencyProperty.Register("Radius", typeof(double), typeof(ArcProgress), new PropertyMetadata(1.0, new PropertyChangedCallback(ValueChanged)));





        public Size ArcSize
        {
            get { return (Size)GetValue(ArcSizeProperty); }
            set { SetValue(ArcSizeProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ArcSize.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ArcSizeProperty =
            DependencyProperty.Register("ArcSize", typeof(Size), typeof(ArcProgress), new PropertyMetadata(new Size(1,1)));



        public Point EndPoint
        {
            get { return (Point)GetValue(EndPointProperty); }
            set { SetValue(EndPointProperty, value); }
        }

        // Using a DependencyProperty as the backing store for EndPoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty EndPointProperty =
            DependencyProperty.Register("EndPoint", typeof(Point), typeof(ArcProgress), new PropertyMetadata(new Point(0,0)));





        public Point StartPoint
        {
            get { return (Point)GetValue(StartPointProperty); }
            set { SetValue(StartPointProperty, value); }
        }

        // Using a DependencyProperty as the backing store for StartPoint.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty StartPointProperty =
            DependencyProperty.Register("StartPoint", typeof(Point), typeof(ArcProgress), new PropertyMetadata(new Point(0,0)));



        public bool LargeArc
        {
            get { return (bool)GetValue(LargeArcProperty); }
            set { SetValue(LargeArcProperty, value); }
        }

        // Using a DependencyProperty as the backing store for LargeArc.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty LargeArcProperty =
            DependencyProperty.Register("LargeArc", typeof(bool), typeof(ArcProgress), new PropertyMetadata(false));



        #endregion
    }
}
