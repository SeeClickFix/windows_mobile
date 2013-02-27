using GalaSoft.MvvmLight.Ioc;
using SeeClickFix.WP8.Common;
using SeeClickFix.WP8.Services;
using SeeClickFix.WP8.Services.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Telerik.Windows.Controls;

namespace SeeClickFix.WP8.Controls
{
    public class ImageControl : Control
    {
        RadBusyIndicator busyIndicator;
        BitmapImage source;
        Image image;
        bool imageDownloaded;

        //#region 

        //public bool ShowBusyIndicator
        //{
        //    get
        //    {
        //        return this.GetValue(SourceProperty) as ImageSource;
        //    }
        //    set
        //    {
        //        this.SetValue(SourceProperty, value);
        //    }
        //}


        //#endregion


        #region FullSizeSource property
        public ImageSource FullSizeSource
        {
            get
            {
                return this.GetValue(FullSizeSourceProperty) as ImageSource;
            }
            set
            {
                this.SetValue(FullSizeSourceProperty, value);
            }
        }

        public static readonly DependencyProperty FullSizeSourceProperty =
           DependencyProperty.Register("FullSizeSource", typeof(ImageSource), typeof(ImageControl), new PropertyMetadata(null));
        #endregion

        //#region DetailsUriQueryParameterFormat property
        //public string DetailsUriQueryParameterFormat
        //{
        //    get
        //    {
        //        return this.GetValue(DetailsUriQueryParameterFormatProperty) as string;
        //    }
        //    set
        //    {
        //        this.SetValue(DetailsUriQueryParameterFormatProperty, value);
        //    }
        //}

        //public static readonly DependencyProperty DetailsUriQueryParameterFormatProperty =
        //   DependencyProperty.Register("DetailsUriQueryParameterFormat", typeof(string), typeof(ImageControl), new PropertyMetadata(null));
        //#endregion

        //#region DetailsUri property
        //public Uri DetailsUri
        //{
        //    get
        //    {
        //        return this.GetValue(DetailsUriProperty) as Uri;
        //    }
        //    set
        //    {
        //        this.SetValue(DetailsUriProperty, value);
        //    }
        //}

        //public static readonly DependencyProperty DetailsUriProperty =
        //   DependencyProperty.Register("DetailsUri", typeof(Uri), typeof(ImageControl), new PropertyMetadata(null));
        //#endregion

        #region Source property
        /// <summary>
        /// Gets or sets the <see cref="ImageSource"/> instance that contains the picture data to be displayed.
        /// </summary>
        public ImageSource Source
        {
            get
            {
                return this.GetValue(SourceProperty) as ImageSource;
            }
            set
            {
                this.SetValue(SourceProperty, value);
            }
        }


        /// <summary>
        /// Identifies the <see cref="Source"/> property.
        /// </summary>
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ImageSource), typeof(ImageControl), new PropertyMetadata(null, OnSourceChanged));

        private static void OnSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as ImageControl).OnSourceChanged((ImageSource)e.NewValue, (ImageSource)e.OldValue);
        }
        #endregion

         

        /// <summary>
        /// This event is fired if the image specified by the image
        /// source fails to open for some reason.
        /// </summary>
        public event EventHandler<ExceptionRoutedEventArgs> ImageFailed;

        /// <summary>
        /// This event is fired when the image specified by the image
        /// source is opened successfully.
        /// </summary>
        public event EventHandler<RoutedEventArgs> ImageOpened;


        public ImageControl()
        {
            this.DefaultStyleKey = typeof(ImageControl);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            this.image = (Image)this.GetTemplateChild("image");
            this.busyIndicator = (RadBusyIndicator)this.GetTemplateChild("busyIndicator");
            this.image.ImageFailed += this.OnImageImageFailed;
            this.image.ImageOpened += this.OnImageOpened;
        }

        protected override void OnTap(System.Windows.Input.GestureEventArgs e)
        {
            base.OnTap(e);
            //if (this.DetailsUri != null)
            //{
            //    string query = string.Empty;
            //    if (!string.IsNullOrWhiteSpace(this.DetailsUriQueryParameterFormat))
            //    {
            //        string.Format(DetailsUriQueryParameterFormat, );
            //    }
            //    SimpleIoc.Default.GetInstance<INavigationService>().NavigateTo(new Uri(string.Format("{0}?{1}", this.DetailsUri, this.DetailsUriQueryParameterFormat), UriKind.Relative));
            //}
            //else
            //{
                var fullSizeImage = this.FullSizeSource as BitmapImage;

                // make it work only for absolute uris
                if (fullSizeImage != null && fullSizeImage.UriSource != null && fullSizeImage.UriSource.IsAbsoluteUri)
                {
                    SimpleIoc.Default.GetInstance<INavigationService>().NavigateTo(new Uri(string.Format("{0}?ImageUri={1}", Constants.ViewImagePageUri, HttpUtility.UrlEncode(fullSizeImage.UriSource.ToString())), UriKind.Relative));
                }
            // }
        }

        private void OnSourceChanged(ImageSource newValue, ImageSource oldValue)
        {
            this.imageDownloaded = false;

            if (oldValue is BitmapImage)
            {
                (oldValue as BitmapImage).DownloadProgress -= this.OnDownloadProgress;
            }

            this.source = newValue as BitmapImage;
            if (this.source != null)
            {
                if (this.source.PixelHeight > 0 || this.source.PixelWidth > 0)
                {
                    this.imageDownloaded = true;
                    this.OnImageOpened(this.image, new RoutedEventArgs());
                }
                else
                {
                    this.source.DownloadProgress += this.OnDownloadProgress;
                }
            }
            else if (newValue is WriteableBitmap)
            {
                this.imageDownloaded = true;
            }
        }

        private void OnDownloadProgress(object sender, DownloadProgressEventArgs e)
        {
            if (this.busyIndicator.IsRunning || e.Progress == 100)
            {
                return;
            }

            this.StartBusyIndicator();
        }

        private void StartBusyIndicator()
        {
            if (this.busyIndicator == null)
            {
                return;
            }

            this.busyIndicator.IsRunning = true;
        }

        private void StopBusyIndicator()
        {
            if (this.busyIndicator == null)
            {
                return;
            }

            this.busyIndicator.IsRunning = false;
        }

        private void OnImageOpened(object sender, RoutedEventArgs e)
        {
            this.imageDownloaded = true;
            this.StopBusyIndicator();

            if (this.ImageOpened == null)
            {
                return;
            }

            this.ImageOpened(sender, e);
        }

        private void OnImageImageFailed(object sender, ExceptionRoutedEventArgs e)
        {
            this.imageDownloaded = false;
            this.StopBusyIndicator();

            if (this.ImageFailed == null)
            {
                return;
            }

            this.ImageFailed(sender, e);
        }
    }
}
