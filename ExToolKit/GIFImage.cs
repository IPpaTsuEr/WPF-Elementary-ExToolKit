using FreeImageAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ExToolKit
{
    public class GIFImage:Image
    {
        static GIFImage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GIFImage), new FrameworkPropertyMetadata(typeof(GIFImage)));
        }

        public GIFImage()
        {
            //var dp = DependencyPropertyDescriptor.FromProperty(SourceProperty, typeof(GIFImage));
            //dp.AddValueChanged(this,(d,e)=> {
            //    var _target = d as GIFImage;
            //    if (_target != null)
            //    {
            //    }

            //});
            timer = new Timer((o) =>
            {
                if (List.Count == 0) return;
                this.Dispatcher.BeginInvoke(new Action(() =>
                {
                    Index++;
                    if (Index >= List.Count) Index = 0;
                    Source = List.ElementAt(Index);
                }));


            }, null, -1, -1);
        }
        private LinkedList<BitmapImage> List = new LinkedList<BitmapImage>();
        private Timer timer;


        private void Load(string path)
        {
            List.Clear();
            //FreeImage.Load(FREE_IMAGE_FORMAT.FIF_GIF, path, FREE_IMAGE_LOAD_FLAGS.DEFAULT);
            if (!File.Exists(path))
            {
                return;
            }
            
            //var _FIF = FreeImage.GetFileType(path, 0);
            //var _FBM = FreeImage.OpenMultiBitmap(_FIF, path, false, true, true, FREE_IMAGE_LOAD_FLAGS.DEFAULT);
            var _FBM = FreeImage.OpenMultiBitmapEx(path, false, true, false);
            LoadeDate(_FBM);
            FreeImage.CloseMultiBitmap(_FBM, FREE_IMAGE_SAVE_FLAGS.DEFAULT);

        }

        private void Load(Stream stream )
        {
            if (stream == null || stream.Length == 0) return;
            stream.Position = 0;
            var _FBM = FreeImage.OpenMultiBitmapFromStream(stream);
            LoadeDate(_FBM);
            FreeImage.CloseMultiBitmap(_FBM, FREE_IMAGE_SAVE_FLAGS.DEFAULT);
        }

        private void LoadeDate(FIMULTIBITMAP bitmap)
        {
            var _PageCount = FreeImage.GetPageCount(bitmap);
            for (int _index = 0; _index < _PageCount; _index++)
            {

                var _PageData = FreeImage.LockPage(bitmap, _index);
                var _Bitmap = FreeImage.GetBitmap(_PageData);
                var _MS = new MemoryStream();
                _Bitmap.Save(_MS, ImageFormat.Png);

                var _P = FreeImage.GetBits(_PageData);
                

                BitmapImage _Image = new BitmapImage();
                _Image.BeginInit();
                _Image.CacheOption = BitmapCacheOption.OnLoad;
                _Image.StreamSource = _MS;
                _Image.EndInit();
                _Image.Freeze();
                FreeImage.UnlockPage(bitmap, _PageData, false);

                _Bitmap.Dispose();
                _MS.Dispose();
                List.AddLast(_Image);
            }
        }




        private static  void GIFDataChanged(DependencyObject dp,DependencyPropertyChangedEventArgs e)
        {
            var _target = dp as GIFImage;
            if (_target == null) return;
            if(e.NewValue is string)
            {
                _target.Load(e.NewValue as string);
            }
            else if(e.NewValue is Stream)
            {
                _target.Load(e.NewValue as Stream);
            }
            _target.PlayThis();
        }
        private void PlayThis()
        {
            if (Rate >= 1000 || Rate <= 0) Rate = 30 ;
            if (Play) timer.Change(0, (int)Math.Floor(1000 / Rate));
            else timer.Change(-1, -1);
        }

        private static void PlayChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            var _target = dp as GIFImage;
            if (_target == null) return;
            if (_target.IsLoaded) _target.PlayThis();
            else
                _target.Dispatcher.BeginInvoke(new Action(() => {
                    _target.PlayThis();
                }),DispatcherPriority.Loaded);
            
        }

        private static void RateChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            var _target = dp as GIFImage;
            if (_target == null) return;
            if (_target.IsLoaded) _target.PlayThis();
            else
                _target.Dispatcher.BeginInvoke(new Action(() =>
                {
                    _target.PlayThis();
                }), DispatcherPriority.Loaded);
        }


        public object GIFData
        {
            get { return (object)GetValue(GIFDataProperty); }
            set { SetValue(GIFDataProperty, value); }
        }

        // Using a DependencyProperty as the backing store for GIFData.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty GIFDataProperty =
            DependencyProperty.Register("GIFData", typeof(object), typeof(GIFImage), new PropertyMetadata(null,new PropertyChangedCallback(GIFDataChanged)));



        public double Rate
        {
            get { return (double)GetValue(RateProperty); }
            set { SetValue(RateProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Rate.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty RateProperty =
            DependencyProperty.Register("Rate", typeof(double), typeof(GIFImage), new PropertyMetadata(30.0,new PropertyChangedCallback(RateChanged)));



        public int Index
        {
            get { return (int)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Index.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IndexProperty =
            DependencyProperty.Register("Index", typeof(int), typeof(GIFImage), new PropertyMetadata(0));



        public bool Play
        {
            get { return (bool)GetValue(PlayProperty); }
            set { SetValue(PlayProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Play.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty PlayProperty =
            DependencyProperty.Register("Play", typeof(bool), typeof(GIFImage), new PropertyMetadata(true,new PropertyChangedCallback(PlayChanged)));


    }
}
