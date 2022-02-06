using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace gamtetyper
{
    /// <summary>
    /// Interaction logic for editwindowblock.xaml
    /// </summary>
    public partial class editwindowblock : UserControl
    {
        public editwindowblock()
        {
            InitializeComponent();
        }
        public void loadinfo(string title, string value)
        {
            UItitle.Text = title;
            UIvalue.Text = value;
            if (value == "")
            {
                dock.Children.Remove(UIvalue);
                
            }
        }
        bool fold = false;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (fold)
            {
                resizepanel.Height = double.NaN;
                wrapbutton.Content = "-";
            }
            else
            {
                resizepanel.Height = 0;
                wrapbutton.Content = "+";
            }
            fold = !fold;
        }
    }
}
