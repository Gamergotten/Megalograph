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
using System.Windows.Shapes;

namespace gamtetyper.UI
{
    /// <summary>
    /// Interaction logic for new_node_window.xaml
    /// </summary>
    public partial class new_node_window : Window
    {
        public new_node_window()
        {
            InitializeComponent();
        }

        public MainWindow main;
        public double node_X;
        public double node_Y;

        public bool is_fucking_closing;

        private void new_trigg_click(object sender, RoutedEventArgs e)
        {
            main.create_trigger(node_X, node_Y);
            is_fucking_closing = true;
            Close();
        }
        private void new_cond_click(object sender, RoutedEventArgs e)
        {
            main.create_condition(node_X, node_Y);
            is_fucking_closing = true;
            Close();
        }
        private void new_action_click(object sender, RoutedEventArgs e)
        {
            main.create_action(node_X, node_Y);
            is_fucking_closing = true;
            Close();
        }
        private void new_branch_click(object sender, RoutedEventArgs e)
        {
            main.create_branch(node_X, node_Y);
            is_fucking_closing = true;
            Close();
        }

        private void Window_LostFocus(object sender, RoutedEventArgs e)
        {
            if (is_fucking_closing == false)
                Close();
        }
    }
}
