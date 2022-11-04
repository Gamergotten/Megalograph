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
    /// Interaction logic for MV_count_item.xaml
    /// </summary>
    public partial class MV_count_item : UserControl
    {
        public MV_count_item()
        {
            InitializeComponent();
        }

        public Ebum child;

        public MV_container_block containing_container;

        public bool troggle_children;

        public NodeWindow main;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            troggle_children = !troggle_children;
            if (troggle_children)
            {
                expand_button.Content = "-";
                foreach (Ebum ebama in child.Params)
                {
                    MainWindow.obama_abducted_my_children(ebama, childs_panel, main);
                }
            }
            else
            {
                expand_button.Content = "+";
                childs_panel.Children.Clear();
            }

        }

        private void block_name_KeyDown(object sender, KeyEventArgs e)
        {
            // setup the path, should probably bake this in
            var mmmmmmm = child.nodes_list_yes_i_did_just_do_that;
            string thingo_node = @"Gametype/base";
            foreach (string s in mmmmmmm)
            {
                string test = String.Concat(s.Where(c => !Char.IsWhiteSpace(c) && !Char.IsSymbol(c)));
                thingo_node += "/" + test;
            }

            main.main.write_node_refname(thingo_node, block_name.Text);
            if (e.Key == Key.Return)
            {
                Keyboard.ClearFocus();
            }
        }
    }
}
