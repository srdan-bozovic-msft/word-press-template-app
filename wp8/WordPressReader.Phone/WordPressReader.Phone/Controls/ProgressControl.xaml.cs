using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace WordPressReader.Phone.Controls
{
    public partial class ProgressControl : UserControl
    {
        public ProgressControl()
        {
            InitializeComponent();

            myCortana.Begin();
        }




    }
}
