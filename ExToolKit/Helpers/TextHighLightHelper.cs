using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace ExToolKit.Helpers
{
    public class TextHighLightHelper
    {

        private static void TextValueChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            var _textBlock = dp as TextBlock;
            if (_textBlock == null) return;
            MakeHighLight(_textBlock, GetHighLightStyle(dp), GetHighLightText(dp));

        }

        private static void MakeHighLight(TextBlock textBlock, Style style, string key)
        {
            var _text = textBlock.Text;

            textBlock.Inlines.Clear();
            if (string.IsNullOrWhiteSpace(key))
            {
                textBlock.Inlines.Add(new Run() { Text = _text });
                return;
            }
            else if (_text.CompareTo(key) == 0)
            {
                textBlock.Inlines.Add(new Run() { Text = _text, Style = style });
                return;
            }

            while (_text.Length > 0)
            {
                var _index = _text.IndexOf(key, StringComparison.CurrentCultureIgnoreCase);
                switch (_index)
                {
                    case 0:
                        textBlock.Inlines.Add(new Run() { Text = key, Style = style });
                        if (_text.Length > key.Length) _text = _text.Substring(key.Length);
                        else _text = "";
                        break;
                    case -1:
                        textBlock.Inlines.Add(new Run() { Text = _text });
                        _text = "";
                        break;
                    default:
                        textBlock.Inlines.Add(new Run() { Text = _text.Substring(0, _index) });
                        textBlock.Inlines.Add(new Run() { Text = _text.Substring(_index, key.Length), Style = style });
                        if (_text.Length > key.Length + _index) _text = _text.Substring(_index + key.Length);
                        else _text = "";
                        break;
                }

            }

        }

        #region DA



        public static Style GetHighLightStyle(DependencyObject obj)
        {
            return (Style)obj.GetValue(HighLightStyleProperty);
        }

        public static void SetHighLightStyle(DependencyObject obj, Style value)
        {
            obj.SetValue(HighLightStyleProperty, value);
        }

        // Using a DependencyProperty as the backing store for HighLightStyle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HighLightStyleProperty =
            DependencyProperty.RegisterAttached("HighLightStyle", typeof(Style), typeof(TextHighLightHelper), new PropertyMetadata(null,new PropertyChangedCallback(TextValueChanged)));



        public static string GetHighLightText(DependencyObject obj)
        {
            return (string)obj.GetValue(HighLightTextProperty);
        }

        public static void SetHighLightText(DependencyObject obj, string value)
        {
            obj.SetValue(HighLightTextProperty, value);
        }

        // Using a DependencyProperty as the backing store for HighLightText.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty HighLightTextProperty =
            DependencyProperty.RegisterAttached("HighLightText", typeof(string), typeof(TextHighLightHelper), new PropertyMetadata(null,new PropertyChangedCallback(TextValueChanged)));


        #endregion
    }
}
