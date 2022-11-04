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
    /// Interaction logic for MV_enum_block.xaml
    /// </summary>
    public partial class MV_reference_block : UserControl
    {
        public MV_reference_block()
        {
            InitializeComponent();
        }
        public Ebum child;

        public NodeWindow main;

        public MV_container_block containing_container;

        public bool troggle_children;

        public bool is_setting_up;

        public bool has_none_entry = false;

        private void value_combox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (is_setting_up)
                return;

            child.V = value_combox.SelectedIndex.ToString();

            main.main.write_node(child);
        }

        private void value_combox_Selected(object sender, RoutedEventArgs e)
        {   // fill out combobox entries
        }

        private void value_combox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            is_setting_up = true;


            if (child.Type.Split(":")[0] == "Ref0")
                value_combox.ItemsSource = main.main.XP.return_all_Entries_for_reference_block(child.Type.Split(":")[1]);
            else // is +1
                value_combox.ItemsSource = main.main.XP.return_all_entries_and_none(child.Type.Split(":")[1]);







            is_setting_up = false;
        }
    }
}
