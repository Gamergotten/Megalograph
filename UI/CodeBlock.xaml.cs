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
    public partial class CodeBlock : UserControl
    {



        public CodeBlock()
        {
            InitializeComponent();
        }

        public NodeWindow main_window;

        public TriggerUI trigger_parent;
        public ConditionUI condition_parent;
        public ActionUI action_parent;

        public bool is_grabbed;
        public Line Inpath;
        public Line Outpath;


        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            main_window.codeblockclick(this);
            e.Handled = true;
        }

        private void Label_MouseUp(object sender, MouseButtonEventArgs e)
        {
            main_window.codeblockrelease(this);
            e.Handled = true;
        }

        bool managing_outpath_line;
        // START MANAGING OUTPATH LINE
        private void out_connection_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            main_window.cb_start_line_drag(this);
        }

        // HOOK 
        private void in_connection_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            //e.Handled = true;
            main_window.cb_stop_line_drag_on_in(this);
        }

        private void in_connection_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled= true;
        }
    }
}
