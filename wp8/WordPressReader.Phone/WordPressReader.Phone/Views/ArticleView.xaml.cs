using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace WordPressReader.Phone.Views
{
    public partial class ArticleView : UserControl
    {
        public ArticleView()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty FlipHorizontalCommandProperty = DependencyProperty.Register(
            "FlipHorizontalCommand",
            typeof(ICommand),
            typeof(ArticleView),
            new PropertyMetadata(null)
            );

        public ICommand FlipHorizontalCommand
        {
            get { return (ICommand)GetValue(FlipHorizontalCommandProperty); }
            set { SetValue(FlipHorizontalCommandProperty, value); }
        }

        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(
            "Title",
            typeof(string),
            typeof(ArticleView),
            new PropertyMetadata(null, OnTitlePropertyChanged)
            );

        public string Title
        {
            get { return (string)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        private static void OnTitlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var articleView = (ArticleView)d;

            if (e.NewValue != null)
            {
                articleView.TitleText.Text = (string)e.NewValue;
            }
        }

        public static readonly DependencyProperty LeadProperty = DependencyProperty.Register(
            "Lead",
            typeof(string),
            typeof(ArticleView),
            new PropertyMetadata(null, OnLeadPropertyChanged)
            );

        public string Lead
        {
            get { return (string)GetValue(LeadProperty); }
            set { SetValue(LeadProperty, value); }
        }

        private static void OnLeadPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var articleView = (ArticleView)d;

            if (e.NewValue != null)
            {
                articleView.LeadText.Text = (string)e.NewValue;
            }
        }

        public static readonly DependencyProperty HtmlProperty = DependencyProperty.Register(
            "Html",
            typeof(string),
            typeof(ArticleView),
            new PropertyMetadata(null, OnHtmlPropertyChanged)
            );

        public string Html
        {
            get { return (string)GetValue(HtmlProperty); }
            set { SetValue(HtmlProperty, value); }
        }

        private static void OnHtmlPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var articleView = (ArticleView)d;

            if (e.NewValue != null)
            {
                articleView.HtmlView.Html = (string)e.NewValue;
            }
        }
    }
}
