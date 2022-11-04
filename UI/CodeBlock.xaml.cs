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

        public bool is_hidden;

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



        // stuff for dynamically created textbox
        public TextBox Rename_trigger_box;
        private void typename_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (trigger_parent != null && Rename_trigger_box == null)
            {
                Rename_trigger_box = new TextBox { Margin = new Thickness(31, 1, 31, 1) };
                Rename_trigger_box.LostFocus += TextBox_LostFocus;
                Rename_trigger_box.KeyDown += TextBox_KeyDown;

                main.Children.Add(Rename_trigger_box);
                Rename_trigger_box.Focus();
            }
            else if (action_parent != null)
            {
                if (action_parent.stored_action.Type.V == "Megl.RunTrigger") // run move to that trigger mechanic
                {
                    main_window.runtrigger_goto(action_parent.stored_action.Type.Params[0].V);
                }
            }
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            int selected_num;
            try
            {
                string apricott = Rename_trigger_box.Text;
                selected_num = Convert.ToInt32(apricott);
            }
            catch
            {
                Rename_trigger_box.BorderBrush = Brushes.Red;
                clear_text_thing();
                return;
            }
            save_choice_check(selected_num);
        }
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space || e.Key == Key.Enter)
            {
                // ptoceed to update this nodeblock
                int selected_num;
                try
                {
                    string apricott = Rename_trigger_box.Text;
                    selected_num = Convert.ToInt32(apricott);
                }
                catch
                {
                    Rename_trigger_box.BorderBrush = Brushes.Red;
                    return;
                }
                save_choice_check(selected_num);
            }
        }
        public void save_choice_check(int new_index)
        {
            main_window.rearrange_trigger_node(this, new_index);
            clear_text_thing();
        }
        public void clear_text_thing()
        {
            Rename_trigger_box.LostFocus -= TextBox_LostFocus;
            Rename_trigger_box.KeyDown -= TextBox_KeyDown;
            main.Children.Remove(Rename_trigger_box);
            Rename_trigger_box = null;
        }
    }
}
