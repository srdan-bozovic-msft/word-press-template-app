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
using WordPressReader.Phone.ViewModels;
using WordPressReader.Phone.Resources;

namespace WordPressReader.Phone.Views
{
    public partial class RichCommentsPageView : PhoneApplicationPage, ICommentsPageView
    {
        private ApplicationBarIconButton SendButton;
        private ApplicationBarIconButton ReloadButton;
        public RichCommentsPageView()
        {
            InitializeComponent();

            SendButton = ApplicationBar.Buttons[0] as ApplicationBarIconButton;
            ReloadButton = ApplicationBar.Buttons[1] as ApplicationBarIconButton;

            SendButton.IsEnabled = false;
            SendButton.Text = AppResources.AppBarButtonSend;
            ReloadButton.Text = AppResources.AppBarButtonReload;
        }

        public IPageViewModel ViewModel
        {
            get { return DataContext as IPageViewModel; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var viewModel = ViewModel as RichCommentsPageViewModel;
            viewModel.VerifyCredentials();
            base.OnNavigatedTo(e);
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
            CheckSendButtonEnabled();
        }

        private async void HyperlinkButton_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var viewModel = ViewModel as RichCommentsPageViewModel;
            viewModel.ReplyTo = null;
        }

        private void CheckSendButtonEnabled()
        {
            SendButton.IsEnabled = !string.IsNullOrEmpty(TextBoxMessage.Text.Trim());
        }


        private async void SendButton_Click(object sender, EventArgs e)
        {
            var viewModel = ViewModel as RichCommentsPageViewModel;
            viewModel.Message = TextBoxMessage.Text;
            SendButton.IsEnabled = false;
            await viewModel.SendMessageAsync();
            this.Focus();
            SendButton.IsEnabled = true;
        }

        private void ReloadButton_Click(object sender, EventArgs e)
        {
            var viewModel = ViewModel as RichCommentsPageViewModel; 
            viewModel.ReloadCommand.Execute(null);
        }
    }
}