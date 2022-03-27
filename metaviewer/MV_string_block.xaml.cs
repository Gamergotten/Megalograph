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
    /// Interaction logic for MV_string_block.xaml
    /// </summary>
    public partial class MV_string_block : UserControl
    {
        public MV_string_block()
        {
            InitializeComponent();
        }

        public Ebum child;

        public MV_container_block containing_container;

        public bool troggle_children;

        public int max;

        public NodeWindow main;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            troggle_children = !troggle_children;
            if (troggle_children)
            {
                expand_button.Content = "-";
                // SPAWN IN ALL THE STRINGS
                var STRINGSLIST = main.main.XP.summon_tables_strings(child.nodes_list_yes_i_did_just_do_that);
                if (STRINGSLIST != null)
                {
                    // compile this into a string so we can just pass it to instances 
                    string parent_thingo_node = @"Gametype/base";
                    foreach (string s in child.nodes_list_yes_i_did_just_do_that)
                    {
                        string test = String.Concat(s.Where(c => !Char.IsWhiteSpace(c) && !Char.IsSymbol(c)));
                        parent_thingo_node += "/" + test;
                    }
                    parent_thingo_node += "/Strings";

                    foreach (string s in STRINGSLIST)
                    {
                        MV_string_instance stringchunk = new MV_string_instance();
                        stringchunk.main = main;
                        stringchunk.block_name.Text = s;
                        stringchunk.XML_location = parent_thingo_node +"/"+ s;
                        
                        string sus = main.main.XP.return_from_dump_location(parent_thingo_node + "/" + s + "/EnglishString/String");
                        stringchunk.block_preview.Text = sus;

                        childs_panel.Children.Add(stringchunk);
                    }
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
            // CREATE NEW STRING THING

            troggle_children = false;
            expand_button.Content = "+";
            childs_panel.Children.Clear();

            var STRINGSLIST = main.main.XP.summon_tables_strings(child.nodes_list_yes_i_did_just_do_that);

            List<string> children = new List<string>(child.nodes_list_yes_i_did_just_do_that);
            children.Add("Strings");
            if (STRINGSLIST != null)
                children.Add("String" + STRINGSLIST.Count);
            else
                children.Add("String0");

            main.main.add_new_table_string(children);

            update_count();
        }
        private void remove_button_Click(object sender, RoutedEventArgs e)
        {
            troggle_children = false;
            expand_button.Content = "+";
            childs_panel.Children.Clear();

            var STRINGSLIST = main.main.XP.summon_tables_strings(child.nodes_list_yes_i_did_just_do_that);


            List<string> children = new List<string>(child.nodes_list_yes_i_did_just_do_that);
            children.Add("Strings");
            children.Add(STRINGSLIST.Last());
            main.main.remove_node(children);

            update_count();
        }
        public void update_count()
        {
            var STRINGSLIST = main.main.XP.summon_tables_strings(child.nodes_list_yes_i_did_just_do_that);
            if (STRINGSLIST != null)
            {
                count_text.Text = "(" + STRINGSLIST.Count + ")";
                if (STRINGSLIST.Count > 0)
                {
                    expand_button.IsEnabled = true;
                    remov_button.IsEnabled = true;
                    if (STRINGSLIST.Count < max)
                    {
                        add_button.IsEnabled = true;
                    }
                    else
                    {
                        add_button.IsEnabled = false;
                    }
                }
                else
                {
                    expand_button.IsEnabled = false;
                    remov_button.IsEnabled = false;
                    add_button.IsEnabled = true;
                }
            }
            else
            {
                count_text.Text = "(0)";
                expand_button.IsEnabled = false;
                remov_button.IsEnabled = false;
                add_button.IsEnabled = true;

            }
        }

        // write to xml that we've toggled compression
        private void CheckBox_Click(object sender, RoutedEventArgs e)
        {
            main.main.write_using_compression_node(child.nodes_list_yes_i_did_just_do_that, compbox.IsChecked);
        }
    }
}
