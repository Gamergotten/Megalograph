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
using static gamtetyper.Gametype;
using gamtetyper.UI;

namespace gamtetyper.metaviewer
{
    /// <summary>
    /// Interaction logic for MV_string_text.xaml
    /// </summary>
    public partial class MV_string_text : UserControl
    {
        public MV_string_text()
        {
            InitializeComponent();
        }

        public string xmllocation;

        public MV_container_block containing_container;

        public NodeWindow main;

        public bool stringenabled;
        private void edit_value_box(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                send_changes();
                Keyboard.ClearFocus();
            }
        }

        private void value_text_LostFocus(object sender, RoutedEventArgs e)
        {
            send_changes();
        }
        public void send_changes()
        {
            main.main.table_write_string(xmllocation, stringtoggle.IsChecked, stringtext_box.Text);
        }

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            send_changes();
            figure_out_checkbox();
        }
        public void figure_out_checkbox() 
        { 
            if (stringtoggle.IsChecked == true)
            {
                stringenabled = true;
                stringtext_box.IsEnabled = true;
            }
            else
            {
                stringenabled = false;
                stringtext_box.IsEnabled = false;
            }

        }
    }
}
