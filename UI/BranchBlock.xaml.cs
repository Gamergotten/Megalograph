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
namespace gamtetyper
{
    /// <summary>
    /// Interaction logic for CodeBlock.xaml
    /// </summary>
    public partial class BranchBlock : UserControl
    {
        public BranchBlock()
        {
            InitializeComponent();
        }

        public NodeWindow main_window;

        public BranchUI branch_parent;

        public bool is_grabbed;
        public Line DOpath;
        public Line THENpath;
        public Line Inpath;


        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            is_grabbed = true;
            status_border.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 220, 173, 14));
            e.Handled = true;
        }
        private void Label_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            is_grabbed = false;
            status_border.BorderBrush = Brushes.White;
            // e.Handled = true;
        }

        private void in_connection_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            main_window.bb_stop_line_drag_on_in(this);
        }

        private void do_connection_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            main_window.bb_start_lineDO_drag(this);
        }

        private void then_connection_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            main_window.bb_start_lineTHEN_drag(this);
        }

        private void in_connection_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void Label_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            main_window.delete_branch(this);
        }
    }
}
