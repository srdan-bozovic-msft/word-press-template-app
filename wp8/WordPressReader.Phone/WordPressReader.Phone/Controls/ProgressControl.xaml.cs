using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Data;

namespace WordPressReader.Phone.Controls
{
    public partial class ProgressControl : UserControl
    {
        public ProgressControl()
        {
            
            InitializeComponent();

            RegisterForNotification("Visibility", this, (d, e) =>
            {
                if(Visibility == System.Windows.Visibility.Collapsed)
                {
                    myCortana.Stop();
                }
                else
                {
                    myCortana.Begin();
                }
            });
        }

        public void RegisterForNotification(string propertyName, FrameworkElement element, PropertyChangedCallback callback) { 
            //Bind to a depedency property 
            Binding b = new Binding(propertyName) { Source = element }; 
            var prop = System.Windows.DependencyProperty.RegisterAttached( "ListenAttached"+propertyName, typeof(object), typeof(UserControl), new System.Windows.PropertyMetadata(callback)); 
            element.SetBinding(prop, b); 
        }
        //- See more at: http://www.amazedsaint.com/2009/12/silverlight-listening-to-dependency.html#sthash.TjtuKbxp.dpuf




    }
}
