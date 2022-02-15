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
    public partial class MV_enum_block : UserControl
    {
        public MV_enum_block()
        {
            InitializeComponent();
        }
        public Ebum child;

        public NodeWindow main;

        public MV_container_block containing_container;

        public bool troggle_children;

        public bool is_setting_up;

        private void value_combox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (is_setting_up)
                return;

            string selected_enum = value_combox.SelectedItem.ToString();

            child.V = selected_enum;

            main.main.WHY4(this);
            childs_panel.Children.Clear();

            main.main.write_node(child);

            if (child.Params != null)
            {
                foreach (Ebum ebama in child.Params)
                {
                    MainWindow.obama_abducted_my_children(ebama, childs_panel, main);
                }
                //save_children_too(child);
            }
        }
        public void save_children_too(Ebum inherit)
        {
            foreach (Ebum ebama in inherit.Params)
            {
                main.main.write_node(ebama);
                save_children_too(ebama);
            }
        }
    }
}
