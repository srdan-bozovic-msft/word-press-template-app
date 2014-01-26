using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace WordPressReader.Phone.Views
{
    public partial class ErrorPageView : PhoneApplicationPage
    {
        public ErrorPageView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            ErrorMessage.Text = "Došlo je do problema prilikom preuzimanja podataka.\nMolimo Vas pokušajte kasnije.";
        }

        private void Image_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}