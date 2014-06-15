using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MSC.Phone.Shared.Contracts.ViewModels;
using WordPressReader.Phone.Contracts.Views;
using System.Windows.Media;

namespace WordPressReader.Phone.Views
{
    public partial class RichCommentsPageView : PhoneApplicationPage, ICommentsPageView
    {
        public RichCommentsPageView()
        {
            InitializeComponent();
        }

        public IPageViewModel ViewModel
        {
            get { return DataContext as IPageViewModel; }
        }

        private void TextBoxMessage_GotFocus(object sender, RoutedEventArgs e)
        {
            var tb = (PhoneTextBox)sender;
            tb.Background = new SolidColorBrush(Colors.Transparent);
            tb.BorderBrush = new SolidColorBrush(Colors.Transparent);
            tb.SelectionBackground = new SolidColorBrush(Colors.Transparent);

            InputBubble.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 0, 132, 255));
        }

        private void TextBoxMessage_LostFocus(object sender, RoutedEventArgs e)
        {
            var tb = (PhoneTextBox)sender;
            tb.Background = new SolidColorBrush(Colors.Transparent);
            tb.BorderBrush = new SolidColorBrush(Colors.Transparent);
            tb.SelectionBackground = new SolidColorBrush(Colors.Transparent);

            InputBubble.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 235, 235, 235));
        }

        private void TextBoxMessage_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void HyperlinkButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {

        }
    }
}