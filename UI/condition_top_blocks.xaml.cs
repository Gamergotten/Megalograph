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

namespace gamtetyper.UI
{
    /// <summary>
    /// Interaction logic for condition_top_blocks.xaml
    /// </summary>
    public partial class condition_top_blocks : UserControl
    {

        public condition_top_blocks()
        {
            InitializeComponent();
        }

        public CodeBlock cb;

        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (cb != null)
            {
                var v = cb.condition_parent.stored_condition;
                v.Not = (bool)(knot_box.IsChecked) ? 1 : 0;
                cb.condition_parent.stored_condition = v;
            }
        }

        private void or_box_Click(object sender, RoutedEventArgs e)
        {
            var v = cb.condition_parent.stored_condition;
            v.OR = (bool)or_box.IsChecked;
            cb.condition_parent.stored_condition = v;
        }
    }
}
