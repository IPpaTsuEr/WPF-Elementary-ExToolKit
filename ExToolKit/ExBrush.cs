using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;

namespace ExToolKit
{
    public class ExBrush : MarkupExtension
    {
        private Color FallBackColor { get; set; } = Colors.Black;
        public Color HighLightColor { get; set; } = Colors.White;
        public Color NormalColor { get; set; } = Colors.Transparent;
       
        public double GradientRadius { get; set; } = 80;
        public double BrushOpacity { get; set; } = 0.8;
        public Transform BrushTransform { get; set; } = Transform.Identity;
        public Transform RelativeBrushTransform { get; set; } = Transform.Identity;
        public bool GloabalEffect { get; set; } = true;

        private RadialGradientBrush brush { get; set; }
        private FrameworkElement element { get; set; }
        private Window window { get; set; }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            IProvideValueTarget pvt = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (pvt == null) return this;
            if (pvt.TargetObject.GetType().Name.EndsWith("SharedDp")) return this;
            element = pvt.TargetObject as FrameworkElement;
            if (element == null) return this;
            if (DesignerProperties.GetIsInDesignMode(element)) return new SolidColorBrush(FallBackColor);
            window = Window.GetWindow(element);
            Binding(element);
            return brush;
        }



        private RadialGradientBrush CreateBrush()
        {
            var brush = new RadialGradientBrush(HighLightColor, NormalColor)
            {
                MappingMode = BrushMappingMode.Absolute,
                RadiusX = GradientRadius,
                RadiusY = GradientRadius,
                Opacity = BrushOpacity,
                Transform = BrushTransform,
                RelativeTransform = RelativeBrushTransform,
                Center = new Point(Double.NegativeInfinity, double.NegativeInfinity)
            };
            return brush;

        }

        private void UpdateBrush(RadialGradientBrush brush, Point target)
        {

            if (Mouse.PrimaryDevice != null)
            {
                brush.GradientOrigin = target;
                brush.Center = target;
            }
            else
            {
                brush.Center = new Point(double.NegativeInfinity, double.NegativeInfinity);
            }

        }


        private void Binding(FrameworkElement element)
        {
            brush = CreateBrush();
            if (GloabalEffect)
            {
                CompositionTarget.Rendering -= CompositionTarget_Rendering;
                CompositionTarget.Rendering += CompositionTarget_Rendering;

                //if (window == null) return;
                //window.MouseMove += Window_MouseMove;
                //window.Closed += Window_Closed;
            }
            else
            {
                element.MouseLeave += Element_MouseLeave;
                element.MouseEnter += Element_MouseEnter;
                element.MouseMove -= Element_MouseMove;
                element.MouseMove += Element_MouseMove;
            }


        }

        private void Element_MouseMove(object sender, MouseEventArgs e)
        {
            //Console.WriteLine("Element Move {0}",DateTime.Now.Ticks);
            UpdateBrush(brush, Mouse.GetPosition(element));
        }

        private void Element_MouseLeave(object sender, MouseEventArgs e)
        {
            brush.Center = new Point(double.NegativeInfinity, double.NegativeInfinity);
            element.MouseEnter -= Element_MouseEnter;
        }

        private void Element_MouseEnter(object sender, MouseEventArgs e)
        {

            if (brush == null)
            {
                brush = CreateBrush();
            }
        }


        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            UpdateBrush(brush, Mouse.GetPosition(element));
        }




        //private void Window_Closed(object sender, EventArgs e)
        //{
        //    window.MouseMove -= Window_MouseMove;
        //    window.Closed -= Window_Closed;
        //}

        //private void Window_MouseMove(object sender, MouseEventArgs e)
        //{
        //    Console.WriteLine(DateTime.Now.Ticks);
        //    UpdateBrush(brush, Mouse.GetPosition(element));
        //}
    }
}
