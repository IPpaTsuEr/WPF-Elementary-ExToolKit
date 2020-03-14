using FreeImageAPI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ExToolKit
{

    public class GIFImage : Image 
    {
        private Dictionary<int,BitmapImage> List;

        DispatcherTimer _DT;
        private int _DefaultRate = 0;
        private bool _CanAnimate = true;
        private double _FrameHeight, _FrameWidth;
        private object _GIFData;
        private bool _GifLoaded = false;
        private int _FBMP = 0;

        static GIFImage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(GIFImage), new FrameworkPropertyMetadata(typeof(GIFImage)));
        }

        public GIFImage()
        {
            /*var dp = DependencyPropertyDescriptor.FromProperty(SourceProperty, typeof(GIFImage));
            dp.AddValueChanged(this, (d, e) =>
            {
                var _target = d as GIFImage;
                if (_target != null)
                {
                }

            });*/
            //有一定的高度能够使Virtualizing容器计算虚化个数，防止一次性加载
            Height = 400; Width = 600;

            Loaded += GIFImage_Loaded;
        }

        ~GIFImage()
        {
            if (_DT != null)
            {
                _DT.Stop();_DT = null;
            }
            if(List!=null)List.Clear();

            Console.WriteLine("Gif Release!");
        }


        private void GIFImage_Loaded(object sender, RoutedEventArgs e)
        {

            _GIFData = GIFData;
            //使用非UI线程载入图片
            LoadGifData();
            
        }
        
        private async Task LoadGifData()
        {
            await Task.Run(() => {
                List = new Dictionary<int, BitmapImage>();
                _GifLoaded = false;
                var path = _GIFData as string;
                if(!string.IsNullOrEmpty(path) && File.Exists(path))
                {
                    var _FT = FreeImage.GetFileType(path,0);

                    if (_FT == FREE_IMAGE_FORMAT.FIF_GIF)
                        LoadMultiImage(_FT,path);
                    else
                        LoadSingleImage(path);
                }
                _GifLoaded = true;
                _CanAnimate = true;
                Dispatcher.BeginInvoke(new Action(()=> { Height = _FrameHeight;Width = _FrameWidth; PlayThis(); }));
            });
        }

        private void LoadSingleImage(string path)
        {
            try
            {
                _CanAnimate = false;
                //List.Add(0, new BitmapImage(new Uri(Path.GetFullPath(path),UriKind.Absolute)));
            }
            finally
            {

            }
        }

        private void LoadMultiImage(FREE_IMAGE_FORMAT _FT,string path)
        {
            var _FMP = FreeImage.OpenMultiBitmap(_FT, path, false, true, true, FREE_IMAGE_LOAD_FLAGS.GIF_PLAYBACK);
            _FBMP = FreeImage.GetPageCount(_FMP);
            for (int i = 0; i < _FBMP; i++)
            {
                var _Page = FreeImage.LockPage(_FMP,i);
                if (_DefaultRate == 0)
                {
                    FreeImage.GetMetadata(FREE_IMAGE_MDMODEL.FIMD_ANIMATION, _Page, "FrameTime", out FITAG _tag);
                    _DefaultRate = Marshal.ReadInt32(FreeImage.GetTagValue(_tag));
                    //Console.WriteLine("Gif Rate :{0}",_DefaultRate);
                    if (_DefaultRate == 0) _DefaultRate = 80;
                    _FrameWidth = FreeImage.GetWidth(_Page);
                    _FrameHeight = FreeImage.GetHeight(_Page);
                }
                var _BM = FreeImage.GetBitmap(_Page);
                var _SM = new MemoryStream();
                _BM.Save(_SM, ImageFormat.Png);
                var _Image = new BitmapImage();
                _Image.BeginInit();
                _Image.CacheOption = BitmapCacheOption.OnLoad;
                _Image.StreamSource = _SM;
                _Image.EndInit();
                _Image.Freeze();
                List.Add(i, _Image);
                _SM.Dispose();
                _BM.Dispose();

                if (i == 0)
                {
                    Dispatcher.BeginInvoke(new Action(() => { Source = _Image; }));
                }
            }
            FreeImage.CloseMultiBitmap(_FMP, FREE_IMAGE_SAVE_FLAGS.DEFAULT);
        }
       
        
        /// <summary>
        /// 停止GIF播放并卸载图像资源，当需要重新播放时需重载入
        /// </summary>
        public void ReleaseImages()
        {
            _GifLoaded = false;
            if (_DT != null) { _DT.Stop(); _DT = null; }
            if (List != null) List.Clear();
            _FBMP = 0;
        }
        /// <summary>
        /// 重载入GIF图像资源
        /// </summary>
        public void ReLoadImages()
        {
            LoadGifData();
        }



        private void PlayThis()
        {
            if (!_CanAnimate) return;

            if (Rate >= 1000 || Rate <= 0) Rate = _DefaultRate;

            if (_DT == null)
            {
                _DT = new DispatcherTimer();
                _DT.Tick +=(o,e) =>
                {
                    if (!_GifLoaded) return;
                    try
                    {
                            Index++;
                            if (Index >= _FBMP) Index = 0;
                            if (_FBMP != 0 && Index >= _FBMP) { Index = Index % _FBMP; }
                            List.TryGetValue(Index, out BitmapImage _Page);
                            Source = _Page;
                            //Console.WriteLine("Run ---->{0}",List.Count);
                    }
                    catch (Exception error)
                    {
                        Console.WriteLine("Play Gif Error : {0}",error.Message);
                    }
                };
            }
            if (Play)
            {
                _DT.Interval = TimeSpan.FromMilliseconds(Rate);
                _DT.Start();
            }
            else _DT.Stop();
        }

        private static  void GIFDataChanged(DependencyObject dp,DependencyPropertyChangedEventArgs e)
        {
            var _target = dp as GIFImage;
            if (_target == null || !_target.IsLoaded) return;
            _target.ReleaseImages();
            _target.LoadGifData();
        }

        private static void PlayChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            var _target = dp as GIFImage;
            if (_target == null || !_target.IsLoaded) return;

            if (_target.IsLoaded) _target.PlayThis();
            else
                _target.Dispatcher.BeginInvoke(new Action(() => {
                    _target.PlayThis();
                }),DispatcherPriority.Loaded);
            
        }

        private static void RateChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            var _target = dp as GIFImage;
            if (_target == null || !_target.IsLoaded) return;

            if (_target.IsLoaded && e.NewValue is double)
            {
                if (_target._DT == null) return;
                _target._DT.Interval = TimeSpan.FromMilliseconds((double)e.NewValue);
            }
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
            DependencyProperty.Register("Rate", typeof(double), typeof(GIFImage), new PropertyMetadata(0.0,new PropertyChangedCallback(RateChanged)));



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
