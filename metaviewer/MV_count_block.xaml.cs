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

namespace gamtetyper.metaviewer
{
    /// <summary>
    /// Interaction logic for MV_count_block.xaml
    /// </summary>
    public partial class MV_count_block : UserControl
    {
        public MV_count_block()
        {
            InitializeComponent();
        }

        public Ebum child;

        public MV_container_block containing_container;

        public bool troggle_children;

        public int max;

        public MainWindow main;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            troggle_children = !troggle_children;
            if (troggle_children)
            {
                expand_button.Content = "-";
                foreach (Ebum ebama in child.Params)
                {
                    MainWindow.biden_abducted_my_children(ebama, childs_panel, main);
                }
            }
            else
            {
                expand_button.Content = "+";
                childs_panel.Children.Clear();
            }
        }

        private void add_button_Click(object sender, RoutedEventArgs e)
        {
            if (child.Params.Count < max)
            {
                main.create_count_item(this);

                troggle_children = false;
                expand_button.Content = "+";
                childs_panel.Children.Clear();
                update_count();
            }
        }
        private void remove_button_Click(object sender, RoutedEventArgs e)
        {
            if (child.Params.Count > 0)
            {
                main.clear_node(child.Params.Last());
                child.Params.RemoveAt(child.Params.Count -1);

                troggle_children = false;
                expand_button.Content = "+";
                childs_panel.Children.Clear();
                update_count();
            }
        }

        public void update_count()
        {
            count_text.Text = "(" + child.Params.Count+")";
        }
    }
}
