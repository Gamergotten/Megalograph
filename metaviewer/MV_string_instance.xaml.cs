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
    /// Interaction logic for MV_string_instance.xaml
    /// </summary>
    public partial class MV_string_instance : UserControl
    {
        public MV_string_instance()
        {
            InitializeComponent();
        }

        public string XML_location;

        public MV_container_block containing_container;

        public bool troggle_children;

        public NodeWindow main;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            troggle_children = !troggle_children;
            if (troggle_children)
            {
                expand_button.Content = "-";
                // do the languages instances
                var amongus = main.main.XP.summon_xmlnode_children(XML_location);
                foreach (string s in amongus)
                {
                    MV_string_text language = new();

                    string check = main.main.XP.return_from_dump_location(XML_location +"/"+ s);
                    if (check == "True")
                    {
                        language.stringtoggle.IsChecked = true;

                        language.stringtext_box.Text = main.main.XP.return_from_dump_location(XML_location + "/" + s + "/String");
                    }
                    language.xmllocation = XML_location + "/" + s;

                    language.main = main;
                    language.block_name.Text = s;


                    childs_panel.Children.Add(language);
                    language.figure_out_checkbox();
                }
            }
            else
            {
                expand_button.Content = "+";
                childs_panel.Children.Clear();
                
            }

        }
    }
}