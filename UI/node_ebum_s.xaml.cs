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
using Megalograph.UI;
namespace gamtetyper.UI
{
    /// <summary>
    /// Interaction logic for node_ebum_s.xaml
    /// </summary>
    public partial class node_ebum_s : UserControl
    {
        public node_ebum_s()
        {
            InitializeComponent();
        }
        //public void set_xmlps(MainWindow xmlpp)
        //{
        //    xmlp = xmlpp;
        //}
        public Ebum linkedthing;
        public NodeWindow xmlp;


        private void source_text_Click(object sender, RoutedEventArgs e)
        {
            if (linkedthing != null && xmlp != null)
            {
                setup_valuebox_after_click(linkedthing.Type);
            }
            else
            {
                
            }
        }
        public bool ignore_selection_change;
        public void setup_valuebox_after_click(string type)
        {
            if (type == "Int")
            {
                //number_box = new TextBox() 
                //{
                //     Text = "poop", lo = "source_int_LostFocus" KeyDown = "source_int_KeyDown"
                //};
                source_text.Visibility = Visibility.Collapsed;
                number_box = source_int;

                number_box.Visibility = Visibility.Visible;
                number_box.Text = linkedthing.V;
                number_box.Focus();

            }
            //if (type == "UInt")
            //{
            //    string debug_dis = "hello ";
            //}
            //if (type == "Long")
            //{
            //    string debug_dis = "hello ";
            //}
            //if (type == "ULong")
            //{
            //    string debug_dis = "hello ";
            //}
            //if (type == "String")
            //{
            //    string debug_dis = "hello ";
            //}
            //if (type == "String16")
            //{
            //    string debug_dis = "hello ";
            //}
            //if (type == "UString8")
            //{
            //    string debug_dis = "hello ";
            //}
            //if (type == "UString16")
            //{
            //    string debug_dis = "hello ";
            //}
            //if (type == "Hex")
            //{
            //    string debug_dis = "hello ";
            //}
            //if (type == "Blank")
            //{
            //    string debug_dis = "hello ";
            //}
            //if (type.Contains("Enumref"))
            //{
            //    string debug_dis = "hello ";
            //}
            else if (type == "Enum")
            {
                if (xmlp.main.sasuage != null)
                {   // cleanup the previous window
                    xmlp.main.sasuage.closethis();
                }

                //source_text.Visibility = Visibility.Collapsed;
                //enum_selected_thing = source_enum;

                xmlp.main.sasuage = new();
                var strings_out = xmlp.WHY2(linkedthing.XMLDoc, linkedthing.XMLPath);

                xmlp.main.sasuage.souroce_selection = strings_out;
                xmlp.main.sasuage._content.ItemsSource = strings_out;


                //ignore_selection_change = true;
                int current_index = 0;
                int selected_index = -1;
                foreach (var v in xmlp.main.sasuage._content.ItemsSource)
                {
                    string s = v as string;
                    if (s == linkedthing.V)
                    {
                        selected_index = current_index;
                    }
                    current_index++;
                }
                xmlp.main.sasuage._content.SelectedIndex = selected_index;

                var coords = source_text.PointToScreen(new Point(0, 0));

                xmlp.main.sasuage.Top = coords.Y;
                xmlp.main.sasuage.Left = coords.X;

                xmlp.main.sasuage.Show();
                xmlp.main.sasuage.Focus();
                xmlp.main.sasuage.search.Focus();

                xmlp.main.sasuage.is_disabled = false;
                xmlp.main.sasuage.parent_return = this;
            }
            else if (type.Contains("Ref0:") || type.Contains("Ref1:"))
            {
                if (xmlp.main.sasuage != null)
                {   // cleanup the previous window
                    xmlp.main.sasuage.closethis();
                }

                //source_text.Visibility = Visibility.Collapsed;
                //enum_selected_thing = source_enum;

                xmlp.main.sasuage = new();


                List<string>? v = null; 
                if (linkedthing.Type.Split(":")[0] == "Ref0")
                    v = xmlp.main.XP.return_all_Entries_for_reference_block(type.Split(":")[1]);
                else // is +1
                    v = xmlp.main.XP.return_all_entries_and_none(type.Split(":")[1]);

                xmlp.main.sasuage.souroce_selection = v;
                xmlp.main.sasuage._content.ItemsSource = v;
                xmlp.main.sasuage._content.SelectedIndex = int.Parse(linkedthing.V);



                var coords = source_text.PointToScreen(new Point(0, 0));
                xmlp.main.sasuage.Top = coords.Y;
                xmlp.main.sasuage.Left = coords.X;
                xmlp.main.sasuage.Show();
                xmlp.main.sasuage.Focus();
                xmlp.main.sasuage.search.Focus();
                xmlp.main.sasuage.is_disabled = false;
                xmlp.main.sasuage.parent_return = this;
            }
            //if (type == "Container")
            //{
            //    // literally dont do anything if this ones clicked, not sure how there is even this one
            //}
            //if (type == "Count")
            //{
            //    string debug_dis = "hello ";
            //}
            //if (type.Contains("External"))
            //{
            //    string debug_dis = "hello ";
            //}
        }

        public TextBox number_box;

        private void source_int_LostFocus(object sender, RoutedEventArgs e)
        {
            if (number_box != null)
            {
                close_number_box_w_edit();
            }
        }

        private void source_int_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return && number_box != null)
            {
                close_number_box_w_edit();
            }
        }   
        public void close_number_box_w_edit()
        {
            int number = -1;
            try
            {
                number = int.Parse(number_box.Text);
            }
            catch
            {
                number_box.BorderBrush = Brushes.Red;
                thing_border.BorderBrush = Brushes.Red;
                return;
            }
            if (Convert.ToInt64(Math.Pow(2, linkedthing.Size)) > number)
            {
                // check if number is smaller than max
                // set number on to ebum
                // if not valid number or error catch then red line and dont close it


                source_text.Visibility = Visibility.Visible;
                number_box.Visibility = Visibility.Collapsed;
                linkedthing.V = number.ToString();
                source_text.Content = number.ToString();

                thing_border.BorderBrush = Brushes.White;
                number_box.BorderBrush = Brushes.Gray;
                number_box = null;
                // also delete it when we get to that part
            }
            else
            {
                number_box.BorderBrush = Brushes.Red;
                thing_border.BorderBrush = Brushes.Red;
                return;
            }
        }



        public void recieve_selection(string selected_enum, int index)
        {
            linkedthing.V = selected_enum;
            source_text.Content = selected_enum;

            if (linkedthing.Type.Contains("Ref0:") || linkedthing.Type.Contains("Ref1:"))
            {
                linkedthing.V = index.ToString();
                source_text.Content = selected_enum;
            }
            else // is a reference, and we just need the index instead
            {
                xmlp.WHY3(this);
            }

        }

        //private void source_enum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    if (ignore_selection_change)
        //        return;
            
        //    source_text.Visibility = Visibility.Visible;
        //    enum_selected_thing.Visibility = Visibility.Collapsed;

        //    string selected_enum = enum_selected_thing.SelectedItem.ToString();

        //    linkedthing.V = selected_enum;
        //    source_text.Content = selected_enum;

        //    xmlp.WHY3(this);
                
        //    enum_selected_thing = null;
        //}

        //private void source_enum_DropDownClosed(object sender, EventArgs e)
        //{
        //    if (enum_selected_thing != null)
        //    {
        //        source_text.Visibility = Visibility.Visible;
        //        enum_selected_thing.Visibility = Visibility.Collapsed;

        //        enum_selected_thing = null;
        //    }
        //}
    }
}
