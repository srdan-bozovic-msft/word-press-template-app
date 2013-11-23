using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Reactive;
using System.Text;

namespace WordPressReader.Phone.Controls
{
    public partial class HtmlView : UserControl
    {
        public HtmlView()
        {
            InitializeComponent();
            WebBrowser.IsScriptEnabled = true;
            WebBrowser.ScriptNotify += WebBrowser_ScriptNotify;

            WebBrowser.Loaded += browser_Loaded;
        }

        private void browser_Loaded(object sender, RoutedEventArgs e)
        {
            var border = ((WebBrowser)sender).Descendants<Border>().Last() as Border;

            border.ManipulationDelta += Border_ManipulationDelta;
            border.ManipulationCompleted += Border_ManipulationCompleted;
            border.DoubleTap += border_DoubleTap;
        }

        void border_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            e.Handled = true;
        }

        private void Border_ManipulationCompleted(object sender,
                                                  ManipulationCompletedEventArgs e)
        {
            // suppress zoom
            if (e.FinalVelocities.ExpansionVelocity.X != 0.0 ||
                e.FinalVelocities.ExpansionVelocity.Y != 0.0)
                e.Handled = true;
        }

        private void Border_ManipulationDelta(object sender,
                                              ManipulationDeltaEventArgs e)
        {
            // suppress zoom
            if (e.DeltaManipulation.Scale.X != 0.0 ||
                e.DeltaManipulation.Scale.Y != 0.0)
                e.Handled = true;
            //if (e.DeltaManipulation.Translation.X != 0.0)
                
            // optionally suppress scrolling
            //if (ScrollDisabled)
            //{
            //    if (e.DeltaManipulation.Translation.X != 0.0 ||
            //      e.DeltaManipulation.Translation.Y != 0.0)
            //        e.Handled = true;
            //}
        }


        void WebBrowser_ScriptNotify(object sender, NotifyEventArgs e)
        {
            var value = e.Value.Replace("#amp#", "'");
            if (ScriptNotifyCommand.CanExecute(value))
            {
                ScriptNotifyCommand.Execute(value);
            }
        }

        public static readonly DependencyProperty ScriptNotifyCommandProperty = DependencyProperty.Register(
            "ScriptNotifyCommand",
            typeof(ICommand),
            typeof(HtmlView),
            new PropertyMetadata(null)
            );

        public ICommand ScriptNotifyCommand
        {
            get { return (ICommand)GetValue(ScriptNotifyCommandProperty); }
            set { SetValue(ScriptNotifyCommandProperty, value); }
        }

        public static readonly DependencyProperty FlipHorizontalCommandProperty = DependencyProperty.Register(
            "FlipHorizontalCommand",
            typeof(ICommand),
            typeof(HtmlView),
            new PropertyMetadata(null)
        );

        public ICommand FlipHorizontalCommand
        {
            get { return (ICommand)GetValue(FlipHorizontalCommandProperty); }
            set { SetValue(FlipHorizontalCommandProperty, value); }
        }

        public static readonly DependencyProperty HtmlProperty = DependencyProperty.Register(
            "Html",
            typeof (string),
            typeof (HtmlView),
            new PropertyMetadata("", new PropertyChangedCallback(HtmlView.OnHtmlPropertyChanged))
            );

        public string Html
        {
            get { return (string)GetValue(HtmlProperty); }
            set { SetValue(HtmlProperty, value); }
        }

        private static void OnHtmlPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var htmlView = (HtmlView) d;

            if (e.NewValue != null)
            {
                var rawHtml = e.NewValue.ToString();
                var htmlBytes = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, Encoding.Unicode.GetBytes(rawHtml));
                var html = Encoding.Unicode.GetString(htmlBytes, 0, htmlBytes.Length);
                htmlView.WebBrowser.NavigateToString(html);
            }
        }

        public static readonly DependencyProperty LocationProperty = DependencyProperty.Register(
            "Location",
            typeof(Uri),
            typeof(HtmlView),
            new PropertyMetadata(null, new PropertyChangedCallback(HtmlView.OnLocationPropertyChanged))
            );

        public Uri Location
        {
            get { return (Uri)GetValue(LocationProperty); }
            set { SetValue(LocationProperty, value); }
        }

        private static void OnLocationPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var htmlView = (HtmlView)d;

            if (e.NewValue != null)
            {
                var uri = (Uri)e.NewValue;
                htmlView.WebBrowser.Navigate(uri);
            }
        }

        private void OnFlick(object sender, FlickGestureEventArgs e)
        {
            if (e.Direction == Orientation.Horizontal
                && Math.Abs(e.HorizontalVelocity)>600
                )
            {
                if (FlipHorizontalCommand != null
                    && FlipHorizontalCommand.CanExecute(e.HorizontalVelocity))
                {
                    FlipHorizontalCommand.Execute(e.HorizontalVelocity);
                }
            }
        }

        private void OnDragStarted(object sender, DragStartedGestureEventArgs e)
        {
            e.Handled = true;
        }

        private void OnDragDelta(object sender, DragDeltaGestureEventArgs e)
        {
            e.Handled = true;
        }

        private void OnDragCompleted(object sender, DragCompletedGestureEventArgs e)
        {
            e.Handled = true;
        }

        private void GestureListener_PinchStarted(object sender, PinchStartedGestureEventArgs e)
        {
            e.Handled = true;
        }


        private void GestureListener_DoubleTap(object sender, Microsoft.Phone.Controls.GestureEventArgs e)
        {
            e.Handled = true;
        }

        private void WebBrowser_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            e.Handled = true;
        }

    }

}
