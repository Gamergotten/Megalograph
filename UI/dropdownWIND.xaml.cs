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
using Megalograph.UI;
using gamtetyper.UI;

namespace Megalograph.UI
{
    /// <summary>
    /// Interaction logic for dropdownWIND.xaml
    /// </summary>
    public partial class dropdownWIND : Window
    {
        public dropdownWIND()
        {
            InitializeComponent();
        }

        public bool is_disabled = true;
        public node_ebum_s? parent_return;

        public List<string> souroce_selection;
        public void parse_elements(List<string> elements)
        {
            souroce_selection = elements;

        }
        void update_source_via_filter()
        {

        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!is_disabled && parent_return != null && _content.SelectedItem != null)
            {

                parent_return.recieve_selection(_content.SelectedItem.ToString(), _content.SelectedIndex);
                closethis();
            }
        }

        public void closethis()
        {
            if (this != null && !is_disabled)
            {   // cleanup the previous window
                is_disabled = true;
                parent_return.xmlp.main.sasuage = null;
                Close();
            }
        }


        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            _content.SelectedItem = null;
            if (string.IsNullOrEmpty(search.Text))
            {
                _content.ItemsSource = souroce_selection;
            }
            else
            {
                _content.ItemsSource = souroce_selection.Where(X => X.Contains(search.Text, StringComparison.CurrentCultureIgnoreCase));
            }
        }

        private void Window_Deactivated(object sender, EventArgs e)
        {
            closethis();
        }
    }
}
