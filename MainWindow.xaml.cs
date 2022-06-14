using Microsoft.Win32;
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
using System.IO;
using System.Text.RegularExpressions;
using static gamtetyper.InterpStruct;
using System.Diagnostics;
using System.Windows.Markup;
using static gamtetyper.Gametype;
using gamtetyper.UI;
using gamtetyper.metaviewer;
using gamtetyper.code;
using Megalograph.UI;

namespace gamtetyper
{
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
    public partial class MainWindow : Window
    {
        
        public static void catchexception_and_duly_ignore(Exception ex)
        {
            crashlog crash_window = new();
            
            crash_window.error_title.Text = ex.Message.ToString();
            if (ex.StackTrace != null)
                crash_window.error_message.Text = ex.StackTrace.ToString();

            if (ex.Message == "Root element is missing.")
            {
                crash_window.dev_note.Text = "XML error";
            }
            if (ex.Message == "Object reference not set to an instance of an object.")
            {
                crash_window.dev_note.Text = "a variable broke";
            }
            if (ex.Message.Contains(", is an invalid character."))
            {
                crash_window.dev_note.Text = "database/gamemode is broken, try a different gamemode";
            }

            crash_window.Show();
            crash_window.Focus();

        }

        public XML_Process XP = new XML_Process();

        // tldr_message -> count & UI //
        public Dictionary<string, KeyValuePair<int, ConsoleMessage>> console_out_list = new();

        public bool console_flushed_state = false;
        public void PostConsole(string Message, string tldr_message, string color, bool flush)
        {
            
            if (console_out_list.ContainsKey(tldr_message))
            {
                var v = console_out_list[tldr_message];
                v.Value.tldr.Text = tldr_message + " ("+ (v.Key+1) +")";

                console_out_list[tldr_message] = new(v.Key + 1, v.Value);
            }
            else
            {
                if (flush || flush==false&&console_flushed_state==true)
                {
                    ConsolePanel.Children.Clear();
                    console_out_list.Clear();
                }
                console_flushed_state = flush;
                
                ConsoleMessage tb = new ConsoleMessage();
                tb.message.Text = Message;
                tb.tldr.Text = tldr_message + " (1)";

                if (color == "red")
                {
                    tb.color.Background = Brushes.DarkRed;
                }
                else if (color == "green")
                {
                    tb.color.Background = Brushes.LimeGreen;
                }
                else if (color == "white")
                {
                    tb.color.Background = Brushes.WhiteSmoke;
                }

                ConsolePanel.Children.Add(tb);
                console_out_list.Add(tldr_message, new KeyValuePair<int, ConsoleMessage>(1, tb));
            }

        }


        //private void parent_nodegraph_KeyDown(object sender, KeyEventArgs e)
        //{
        //    if (Current_Gametype != -1)
        //    {
        //        Loaded_Gametypes_phys[Current_Gametype].parent_nodegraph_KeyDown(sender, e);
        //        IInputElement focusedControl = Keyboard.FocusedElement;
        //        Debug.WriteLine(focusedControl);
        //    }
        //}

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            console_flushed_state = false;
            ConsolePanel.Children.Clear();
            console_out_list.Clear();
        }

        public MainWindow()
        {
            try
            {

                InitializeComponent();
                XP.XMLdirectory = Directory.GetCurrentDirectory() + @"\XMLs\";

                string[] files = System.IO.Directory.GetFiles(Directory.GetCurrentDirectory() + @"\XMLs\Halos", "*.xml");
                List<string> poopy = new List<string>();

                foreach (string XMLFile in files)
                {
                    string XMLName = System.IO.Path.GetFileNameWithoutExtension(XMLFile);
                    Loaded_XMLS.Add(XMLName);
                    poopy.Add(XMLName);
                }
                XMLFILTER_in.ItemsSource = poopy;

                XMLFILTER_out.ItemsSource = poopy;

                is_loading_xmls = false;
            }
            catch (Exception ex)
            {
                catchexception_and_duly_ignore(ex);
            }
        }
        public string returnmegldoc_fromhalo(string halo)
        {
            switch (halo)
            {
                case "HR":
                    return @"\Halo reach\var enums.xml";
                case "H4":
                    return @"\Halo 4\var enums.xml";
                case "H2A":
                    return @"\Halo 2A\var enums.xml";
            }
            return "";
        }

        // public Dictionary<string, string> Loaded_XMLS = new Dictionary<string, string>();
        public List<string> Loaded_XMLS = new List<string>();

        public List<Gametype.GametypeLoaded> Loaded_Gametypes = new List<Gametype.GametypeLoaded>();
        public List<NodeWindow> Loaded_Gametypes_phys = new();
        //public List<>


        public int Current_Gametype = -1;


        private void OnSelectionChanged(Object sender, SelectionChangedEventArgs args) // change 
        {
            var tc = sender as TabControl; 

            if (tc != null)
            {
                if (tc.SelectedIndex == Current_Gametype)
                    return;
                // FUCK YOU WHATEVER IS CAUSING THIS

                //var item = tc.SelectedItem;

                Current_Gametype = tc.SelectedIndex; // XMLFILTER.SelectedIndex;

                //Open_SGF(Loaded_Gametypes[Current_Gametype].SSGF_File);
                Load_Tab();


            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            try 
            { 
                if (Current_Gametype != -1)
                {
                    int index_to_wipe = gmaetypeTabs.SelectedIndex;
                    gmaetypeTabs.SelectedIndex = -1;

                    Loaded_Gametypes.RemoveAt(index_to_wipe);
                    Loaded_Gametypes_phys.RemoveAt(index_to_wipe);
                    gmaetypeTabs.Items.RemoveAt(index_to_wipe);
                }
            }
            catch (Exception ex)
            {
                catchexception_and_duly_ignore(ex);
            }
        }






        // ----------------------
        // -- NODEGRAPH SAVING --
        // ----------------------
        //
        private void SaveAsButton_Click(object sender, RoutedEventArgs e)
        {
            var va = Loaded_Gametypes[Current_Gametype];

            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = va.tabname; // Default file name
            dlg.DefaultExt = ".sgf"; // Default file extension
            dlg.Filter = "Sussy Gametyper Files (.sgf)|*.sgf"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;

                if (!File.Exists(filename))
                {
                    File.Copy(va.SSGF_File, filename);
                }

                va.SSGF_File = filename;
                Loaded_Gametypes[Current_Gametype] = va;

                XP.intakeDecompiledmode(filename);

                SaveButton_Click(null, null); // yes, yes i know this is incredibly lazy, but its 5am and i've only been awake for 2 hours now
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Loaded_Gametypes_phys[Current_Gametype].SaveButton_Click();
                PostConsole(Loaded_Gametypes[Current_Gametype].SSGF_File + " Successfully Saved!",
                            "File Saved", "green", true);
            }
            catch (Exception ex)
            {
                catchexception_and_duly_ignore(ex);
            }
        }


        public void Load_Tab()
        {
            meta_panel.Children.Clear();
            mettext.Text = "Meta Viewer";
            if (Current_Gametype == -1)
            {
                DecompButton.IsEnabled = false;
                DecompAsButton.IsEnabled = false;
                CompButton.IsEnabled = false;
                CompAsButton.IsEnabled = false;
                SaveButton.IsEnabled = false;
                SaveAsButton.IsEnabled = false;
                ExportScript.IsEnabled = false;

                XP.wipe_decompiled_mode();
                meta_button.IsEnabled = false;

                XMLFILTER_out.IsEnabled = false;

                OpnBinText.Text = "";
                OpnDumpText.Text = "";

                curr_file_name.Text = "";

                RecenterButton.IsEnabled = false;
                CloseButton.IsEnabled = false;
                return;
            }


            if (Loaded_Gametypes[Current_Gametype].BIN_File != null)
            {
                DecompButton.IsEnabled = true;
                DecompAsButton.IsEnabled = true;
            }
            else
            {
                DecompButton.IsEnabled = false;
                DecompAsButton.IsEnabled = false;
            }

            if (Loaded_Gametypes[Current_Gametype].SSGF_File != null)
            {
                CompButton.IsEnabled = true;
                CompAsButton.IsEnabled = true;
                SaveButton.IsEnabled = true;
                SaveAsButton.IsEnabled = true;
                ExportScript.IsEnabled = true;

                XP.intakeDecompiledmode(Loaded_Gametypes[Current_Gametype].SSGF_File);
                meta_button.IsEnabled = true;
            }
            else
            {
                CompButton.IsEnabled = false;
                CompAsButton.IsEnabled = false;
                SaveButton.IsEnabled = false;
                SaveAsButton.IsEnabled = false;
                ExportScript.IsEnabled = false;

                XP.wipe_decompiled_mode();
                meta_button.IsEnabled = false;
            }

            RecenterButton.IsEnabled = true;
            CloseButton.IsEnabled = true;

            XMLFILTER_out.IsEnabled = true;

            XMLFILTER_out.SelectedItem = Loaded_Gametypes[Current_Gametype].Target_Halo;

            OpnBinText.Text = Loaded_Gametypes[Current_Gametype].BIN_File;
            OpnDumpText.Text = Loaded_Gametypes[Current_Gametype].SSGF_File;

            curr_file_name.Text = Loaded_Gametypes[Current_Gametype].tabname;

            //DecompAllButton.IsEnabled = true;
            //CompAllButton.IsEnabled = true;
            //SaveAllButton.IsEnabled = true;
            // Open_SGF(Loaded_Gametypes[Current_Gametype].SSGF_File);
        }

        bool is_loading_xmls= true;
        private void XMLFILTER_out_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (is_loading_xmls)
                return;
            var u = Loaded_Gametypes[Current_Gametype];
            u.Target_Halo = XMLFILTER_out.SelectedItem.ToString();
            Loaded_Gametypes[Current_Gametype] = u;
        }

        public void Import_SGF(string SGF_to_load)
        {
            try 
            { 
                if (Current_Gametype == -1)
                {
                    import_new_sgf(SGF_to_load);
                }
                else
                {
                    Gametype.GametypeLoaded edit = Loaded_Gametypes[Current_Gametype];
                    edit.SSGF_File = SGF_to_load;
                    Loaded_Gametypes[Current_Gametype] = edit;
                    Load_Tab();
                }
            }
            catch (Exception ex)
            {
                catchexception_and_duly_ignore(ex);
            }
        }
        public void import_new_sgf(string SGF_to_load)
        {
            Gametype.GametypeLoaded SGFLoad = new Gametype.GametypeLoaded();
            SGFLoad.SSGF_File = SGF_to_load;

            SGFLoad.Target_Halo = Loaded_XMLS[XMLFILTER_in.SelectedIndex];


            string tabname = System.IO.Path.GetFileNameWithoutExtension(SGF_to_load);
            tabname = Regex.Replace(tabname, "[^a-zA-Z0-9]", String.Empty);

            SGFLoad.tabname = tabname;
            Loaded_Gametypes.Add(SGFLoad);

            TabItem newTabItem = new TabItem
            {
                Header = tabname,
                Name = tabname
            };
            gmaetypeTabs.Items.Add(newTabItem);
            gmaetypeTabs.SelectedItem = newTabItem;

            NodeWindow n = new NodeWindow();
            n.main = this;
            Loaded_Gametypes_phys.Add(n);

            newTabItem.Content = n;


            //Open_SGF(SGF_to_load);
        }

        public void Import_BIN(string BIN_to_load)
        {
            try 
            { 
                if (Current_Gametype == -1)
                {
                    import_new_BIN(BIN_to_load);
                }
                else
                {
                    Gametype.GametypeLoaded edit = Loaded_Gametypes[Current_Gametype];
                    edit.BIN_File = BIN_to_load;
                    Loaded_Gametypes[Current_Gametype] = edit;
                    Load_Tab();
                }
            }
            catch (Exception ex)
            {
                catchexception_and_duly_ignore(ex);
            }
        }
        public void import_new_BIN(string BIN_to_load)
        {
            Gametype.GametypeLoaded BinLoad = new Gametype.GametypeLoaded();
            BinLoad.BIN_File = BIN_to_load;
            BinLoad.Target_Halo = Loaded_XMLS[XMLFILTER_in.SelectedIndex];

            string tabname = System.IO.Path.GetFileNameWithoutExtension(BIN_to_load);
            tabname = Regex.Replace(tabname, "[^a-zA-Z0-9]", String.Empty);

            BinLoad.tabname = tabname;
            Loaded_Gametypes.Add(BinLoad);

            TabItem newTabItem = new TabItem
            {
                Header = tabname,
                Name = tabname
            };
            gmaetypeTabs.Items.Add(newTabItem);
            gmaetypeTabs.SelectedItem = newTabItem;


            NodeWindow n = new NodeWindow();
            n.main = this;
            Loaded_Gametypes_phys.Add(n);

            newTabItem.Content = n;
        }

        private void OpnBINButton_Click(object sender, RoutedEventArgs e) // ... button
        {
            try
            {
                // our button has been clicked
                // find and load a Binary file

                OpenFileDialog dlg = new OpenFileDialog();

                // Set filter for file extension and default file extension 
                dlg.DefaultExt = ".bin";
                dlg.Filter = "Gametype Files (*.bin)|*.bin";

                // Display OpenFileDialog by calling ShowDialog method 
                Nullable<bool> result = dlg.ShowDialog();

                // Get the selected file name and display in a TextBox 
                if (result == true)
                {
                    // Open document 
                    string filename = dlg.FileName;
                    Import_BIN(filename);
                }
            }
            catch (Exception ex)
            {
                catchexception_and_duly_ignore(ex);
            }
        }

        private void OpnDmpButton_Click(object sender, RoutedEventArgs e) // ... button
        {
            try
            {
                // our button has been clicked
                // find and load an unpacked binary file aka our .SGF 
                OpenFileDialog dlg = new OpenFileDialog();

                // Set filter for file extension and default file extension 
                dlg.DefaultExt = ".sgf";
                dlg.Filter = "Unpacked .BIN files (*.sgf)|*.sgf";

                // Display OpenFileDialog by calling ShowDialog method 
                Nullable<bool> result = dlg.ShowDialog();

                // Get the selected file name and display in a TextBox 
                if (result == true)
                {
                    // Open document 
                    string filename = dlg.FileName;
                    Import_SGF(filename);

                    XP.intakeDecompiledmode(filename);
                    Loaded_Gametypes_phys[Current_Gametype].dothething();
                }
            }
            catch (Exception ex)
            {
                catchexception_and_duly_ignore(ex);
            }

        }


        //bin
        private void importBINButton_Click(object sender, RoutedEventArgs e) // ... button
        {
            try
            {
                // our button has been clicked
                // find and load a Binary file

                OpenFileDialog dlg = new OpenFileDialog();

                // Set filter for file extension and default file extension 
                dlg.DefaultExt = ".bin";
                dlg.Filter = "Gametype Files (*.bin)|*.bin";

                // Display OpenFileDialog by calling ShowDialog method 
                Nullable<bool> result = dlg.ShowDialog();

                // Get the selected file name and display in a TextBox 
                if (result == true)
                {
                    // Open document 
                    string filename = dlg.FileName;
                    import_new_BIN(filename);
                }
            }
            catch (Exception ex)
            {
                catchexception_and_duly_ignore(ex);
            }
        }
        //sgf
        private void importDmpButton_Click(object sender, RoutedEventArgs e) // ... button
        {
            try
            {
                // our button has been clicked
                // find and load an unpacked binary file aka our .SGF 
                OpenFileDialog dlg = new OpenFileDialog();

                // Set filter for file extension and default file extension 
                dlg.DefaultExt = ".sgf";
                dlg.Filter = "Unpacked .BIN files (*.sgf)|*.sgf";

                // Display OpenFileDialog by calling ShowDialog method 
                Nullable<bool> result = dlg.ShowDialog();

                // Get the selected file name and display in a TextBox 
                if (result == true)
                {
                    // Open document 
                    string filename = dlg.FileName;


                    import_new_sgf(filename);

                    XP.intakeDecompiledmode(filename);
                    bool version_match = XP.intakedecompiledmode_verscheck(filename, "Halos\\" + Loaded_Gametypes[Current_Gametype].Target_Halo + ".xml");
                    if (version_match)
                    {
                        Loaded_Gametypes_phys[Current_Gametype].dothething();
                    }
                    else
                    {
                        MessageBoxResult rsltMessageBox = MessageBox.Show("Database version mismatch. Continue?", "Database version mismatch", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                        switch (rsltMessageBox)
                        {
                            case MessageBoxResult.Yes:
                                Loaded_Gametypes_phys[Current_Gametype].dothething();
                                break;

                            case MessageBoxResult.No:
                                XP.wipe_decompiled_mode();
                                CloseButton_Click(null, null);
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                catchexception_and_duly_ignore(ex);
            }

        }





        private void DecompAsButton_Click(object sender, RoutedEventArgs e)
        {
            var va = Loaded_Gametypes[Current_Gametype];
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "placeholdername"; // Default file name
            dlg.DefaultExt = ".sgf"; // Default file extension
            dlg.Filter = "Sussy Gametyper Files (.sgf)|*.sgf"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document

                string filename = dlg.FileName;
                if (!File.Exists(filename))
                {
                    File.Copy(va.SSGF_File, filename);
                }

                va.SSGF_File = filename;
                Loaded_Gametypes[Current_Gametype] = va;

                XP.intakeDecompiledmode(filename);

                DecompButton_Click(null, null); // yes, yes i know this is incredibly lazy, but its 5am and i've only been awake for 2 hours now
            }

        }
        private void DecompButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BitReader b = new BitReader();
                b.m_process = XP;

                // hopefully this works
                string haloxml2 = Loaded_Gametypes[Current_Gametype].Target_Halo;
                //string haloxml = returnmegldoc_fromhalo(haloxml2);
                bool is_Reach = false;
                if (haloxml2 == "HR")
                    is_Reach = true;

                b.isreach = is_Reach;

                if (Loaded_Gametypes[Current_Gametype].SSGF_File == null)
                {
                    Gametype.GametypeLoaded edit = Loaded_Gametypes[Current_Gametype];
                    string s = Loaded_Gametypes[Current_Gametype].BIN_File;
                    edit.SSGF_File = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(s), System.IO.Path.GetFileNameWithoutExtension(s)) + ".sgf";
                    Loaded_Gametypes[Current_Gametype] = edit;
                }

                b.m_process.instaniaterwrite(Loaded_Gametypes[Current_Gametype].SSGF_File, "Halos\\" + Loaded_Gametypes[Current_Gametype].Target_Halo + ".xml");

                byte[] fileBytes = File.ReadAllBytes(Loaded_Gametypes[Current_Gametype].BIN_File);
                b.readbin(fileBytes, "Halos\\" + Loaded_Gametypes[Current_Gametype].Target_Halo + ".xml");

                b.m_process.enprocess();


                XP.intakeDecompiledmode(Loaded_Gametypes[Current_Gametype].SSGF_File);
                Loaded_Gametypes_phys[Current_Gametype].dothething();

                Load_Tab();

                PostConsole(Loaded_Gametypes[Current_Gametype].BIN_File + " Successfully Decompiled!",
                            "Decompiler Done", "green", true);

            }
            catch (Exception ex)
            {
                catchexception_and_duly_ignore(ex);
            }
        }

        private void CompAsButton_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "placeholdername"; // Default file name
            dlg.DefaultExt = ".bin"; // Default file extension
            dlg.Filter = "Binary Files (.bin)|*.bin"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                var va = Loaded_Gametypes[Current_Gametype];
                va.BIN_File = dlg.FileName;
                Loaded_Gametypes[Current_Gametype] = va;

                CompButton_Click(null, null); // yes, yes i know this is incredibly lazy, but its 5am and i've only been awake for 2 hours now
            }

        }
        private void CompButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Loaded_Gametypes_phys[Current_Gametype].SaveButton_Click();

                string haloxml2 = Loaded_Gametypes[Current_Gametype].Target_Halo;
                //string haloxml = returnmegldoc_fromhalo(haloxml2);
                bool is_Reach = false;
                if (haloxml2 == "HR")
                    is_Reach = true;

                BitWriter y = new BitWriter();
                y.m_process = XP;

                y.isreach = is_Reach;

                y.m_process.intakeDecompiledmode(Loaded_Gametypes[Current_Gametype].SSGF_File);

                byte[] OUTPUT = y.writebin("Halos\\" + Loaded_Gametypes[Current_Gametype].Target_Halo + ".xml");

                if (Loaded_Gametypes[Current_Gametype].BIN_File == null)
                {
                    Gametype.GametypeLoaded edit = Loaded_Gametypes[Current_Gametype];
                    string s = Loaded_Gametypes[Current_Gametype].SSGF_File;
                    edit.BIN_File = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(s), System.IO.Path.GetFileNameWithoutExtension(s)) + ".bin";
                    Loaded_Gametypes[Current_Gametype] = edit;
                    Load_Tab();
                }


                File.WriteAllBytes(Loaded_Gametypes[Current_Gametype].BIN_File, OUTPUT);

                PostConsole(Loaded_Gametypes[Current_Gametype].SSGF_File + " Successfully Compiled!",
                                "Compiler Done", "green", true);

            }
            catch (Exception ex)
            {
                catchexception_and_duly_ignore(ex);
            }
        }

        // have fun f
        private void export_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
                SaveButton_Click(null, null);
                SyntaxWindow sw = new();
                sw.main = this;

                string s_ = sw.this_is_where_the_fun_begins();
                Clipboard.SetText(s_);

                PostConsole("Successessfully exported code to clipboard!",
                                "exported code", "white", false);
            //}
            //catch (Exception ex)
            //{
            //    catchexception_and_duly_ignore(ex);
            //}
        }




        // run all movement events
        private void RecenterButton_Click(object sender, RoutedEventArgs e)
        {
            if (Current_Gametype != -1)
                Loaded_Gametypes_phys[Current_Gametype].reset_movement();
        }
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (Current_Gametype != -1)
                Loaded_Gametypes_phys[Current_Gametype].recieved_MouseMove(e);
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (Current_Gametype != -1)
                Loaded_Gametypes_phys[Current_Gametype].Window_MouseLeftButtonUp();
            //drop all nodes too
        }





        // -----------------------
        // -- META VIEWER STUFF --
        // -----------------------
        //
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string haloxml = "Halos\\" + Loaded_Gametypes[Current_Gametype].Target_Halo + ".xml";

                mettext.Text = "Meta Viewer (" + Loaded_Gametypes[Current_Gametype].tabname+")";
                meta_panel.Children.Clear();

                var the_thing = XP.READ_THE_WHOLE_FILE("/base/ExTypes", "ExTypes", haloxml);

                foreach(Ebum v in the_thing)
                {
                    MV_container_block cotainer = new();
                    cotainer.main = Loaded_Gametypes_phys[Current_Gametype];
                    cotainer.child = v;
                    cotainer.block_name.Text = v.Name;
                    meta_panel.Children.Add(cotainer);
                }
            }
            catch (Exception ex)
            {
                catchexception_and_duly_ignore(ex);
            }
        }
        static public void obama_abducted_my_children(Ebum e, StackPanel new_home_for, NodeWindow main)
        {
            switch (e.Type)
            {
                case "Int":
                    create_value_block(e, new_home_for, main);
                        break;
                case "UInt":
                    create_value_block(e, new_home_for, main);
                        break;
                case "Long":
                    create_value_block(e, new_home_for, main);
                        break;
                case "ULong":
                    create_value_block(e, new_home_for, main);
                        break;
                case "String":
                    create_value_block(e, new_home_for, main);
                        break;
                case "String16":
                    create_value_block(e, new_home_for, main);
                        break;
                case "UString8":
                    create_value_block(e, new_home_for, main);
                        break;
                case "UString16":
                    create_value_block(e, new_home_for, main);
                        break;
                case "Hex":
                    create_value_block(e, new_home_for, main);
                        break;
                case "Blank":
                    create_value_block(e, new_home_for, main);
                        break;
                case "Enum":
                    // enum
                    // this'll be interesting
                    create_enum(e, new_home_for, main);
                        break;
                case "Container":
                    // container
                    create_container(e, new_home_for, main);
                        break;
                case "Count":
                    // count
                    create_count(e, new_home_for, main);
                        break;
                case "HCount":
                    // count
                    create_Hcount(e, new_home_for, main);
                    break;
                default:
                    if (e.Type.Contains("External"))
                    {
                        // container
                        create_container(e, new_home_for, main);
                    }
                    if (e.Type.Contains("Enumref"))
                    {
                        // container
                        create_container(e, new_home_for, main);
                    }
                    break;

            }


        }
        static public void biden_abducted_my_children(Ebum e, StackPanel new_home_for, NodeWindow main)
        {
            create_count_item(e, new_home_for, main);
        }
        static void create_container(Ebum data, StackPanel spot, NodeWindow main)
        {
            MV_container_block cotainer = new();
            cotainer.main = main;
            cotainer.child = data;
            if (data.FUCKK_YOU != null)
                cotainer.block_name.Text = data.FUCKK_YOU;
            else
                cotainer.block_name.Text = data.Name;
            spot.Children.Add(cotainer);
        }
        static void create_count(Ebum data, StackPanel spot, NodeWindow main)
        {
            MV_count_block count = new();
            count.main = main;
            count.child = data;
            if (data.FUCKK_YOU != null)
                count.block_name.Text = data.FUCKK_YOU;
            else
                count.block_name.Text = data.Name;

            double d = Math.Pow(2.0, data.Size-1);
            count.max_text.Text = "(" + d + ")";
            count.max = (int)d;
            count.update_count();

            spot.Children.Add(count);
        }
        static void create_Hcount(Ebum data, StackPanel spot, NodeWindow main)
        {
            MV_string_block count = new();
            count.main = main;
            count.child = data;
            if (data.FUCKK_YOU != null)
                count.block_name.Text = data.FUCKK_YOU;
            else
                count.block_name.Text = data.Name;

            double d = Math.Pow(2.0, data.Size - 1);
            count.max_text.Text = "(" + d + ")";
            count.max = (int)d;

            // count.update_count();

            data.nodes_list_yes_i_did_just_do_that.Add("UseCompression"); // epic lazy moment
            string is_compressed = main.main.XP.returnfromdump(data.nodes_list_yes_i_did_just_do_that);
            data.nodes_list_yes_i_did_just_do_that.RemoveAt(data.nodes_list_yes_i_did_just_do_that.Count -1);
            if (is_compressed == "True")
            {
                count.compbox.IsChecked = true;
            }

            count.update_count();

            spot.Children.Add(count);
        }
        static void create_value_block(Ebum data, StackPanel spot, NodeWindow main)
        {
            MV_value_block block = new();
            block.main = main;
            block.child = data;
            if (data.FUCKK_YOU != null)
                block.name_text.Text = data.FUCKK_YOU;
            else
                block.name_text.Text = data.Name;
            block.value_text.Text = data.V;
            block.bits_text.Text = data.Size.ToString();
            block.type_text.Text = data.Type;
            spot.Children.Add(block);
        }
        static void create_enum(Ebum data, StackPanel spot, NodeWindow main)
        {
            MV_enum_block enumb = new();
            enumb.main = main;
            enumb.child = data; // ysdfgvdsgjh;

            enumb.is_setting_up = true;

            if (data.FUCKK_YOU != null)
                enumb.name_text.Text = data.FUCKK_YOU;
            else
                enumb.name_text.Text = data.Name;

            enumb.bits_text.Text = data.Size.ToString();

            enumb.value_combox.ItemsSource = main.WHY2(data.XMLDoc, data.XMLPath);
            int current_index = 0;
            int selected_index = -1;
            foreach (var v in enumb.value_combox.ItemsSource)
            {
                string s = v as string;
                if (s == data.V)
                {
                    selected_index = current_index;
                }
                current_index++;
            }
            enumb.value_combox.SelectedIndex = selected_index;

            spot.Children.Add(enumb);
            enumb.is_setting_up = false;

            if (enumb.child.Params != null)
            {
                foreach (Ebum ebama in enumb.child.Params)
                {
                    MainWindow.obama_abducted_my_children(ebama, enumb.childs_panel, main);
                }
            }
        }
        public void WHY4(MV_enum_block parent_thing)
        {
            //string bad_fix_trim_his_BALLS = String.Join("", parent_thing.linkedthing.XMLPath.Split('/').SkipLast(1));
            string path = @"/base/" + parent_thing.child.XMLPath + "/Var[@name='" + parent_thing.child.V + "']";
            string path2 = parent_thing.child.XMLPath;
            string doc = parent_thing.child.XMLDoc;

            parent_thing.child.Params = XP.readchildren(XP.readdata(path, doc), path2, doc, new List<string>(parent_thing.child.nodes_list_yes_i_did_just_do_that));

            //var ebums = XP.readchildren(XP.readdata(path, doc), path2, doc, new List<string> { "BALLS" });
            //parent_thing.child.Params = new List<Ebum>();
            //parent_thing.childs_panel.Children.Clear();
            //if (ebums != null)
            //{
            //    foreach (var v in ebums)
            //    {
            //        parent_thing.child.Params.Add(v);
            //    }
            //}
        }
        static void create_count_item(Ebum data, StackPanel spot, NodeWindow main)
        {
            MV_count_item count = new();
            count.main = main;
            count.child = data;
            if (data.FUCKK_YOU != null)
                count.block_name.Text = data.FUCKK_YOU;
            else
                count.block_name.Text = data.Name;
            spot.Children.Add(count);
        }
        public void create_count_item(MV_count_block parent_thing)
        {
            string bad_fix_trim_his_BALLS = String.Join("/", parent_thing.child.XMLPath.Split('/').SkipLast(1));


            string path = @"/base/" + parent_thing.child.XMLPath; // + "/Var[@name='" + parent_thing.child.V + "']";
            string path2 = bad_fix_trim_his_BALLS;
            string doc = parent_thing.child.XMLDoc;

            var w = new List<string>(parent_thing.child.nodes_list_yes_i_did_just_do_that);
            w.Add(parent_thing.child.Name +"-child"+parent_thing.child.Params.Count);

            //parent_thing.child.Params;


            Ebum wrapper = new();
            InterpStruct e = XP.readdata(path, doc);

            wrapper.Name = parent_thing.child.Name + "-child" + parent_thing.child.Params.Count;
            wrapper.Params = XP.readchildren(e, path2, doc, w);
            wrapper.nodes_list_yes_i_did_just_do_that = w;
            parent_thing.child.Params.Add(wrapper);

            write_node(wrapper);


            //    = XP.readchildren(XP.readdata(path, doc), path2, doc, w);
        }
        public void clear_node(Ebum e)
        {
            try
            {
                XP.clear_xml_thingo_thanks(e);
                PostConsole(Loaded_Gametypes[Current_Gametype].SSGF_File + " Meta Changes Saved!",
                            "Changes Saved", "green", false);
            }
            catch (Exception ex)
            {
                PostConsole(ex.Message,
                            "Meta Changes Failed", "red", true);
            }
        }
        public void remove_node(List<string> nodes)
        {
            XP.remove_xml_node(nodes);
            PostConsole(Loaded_Gametypes[Current_Gametype].SSGF_File + " Meta Changes Saved!",
                        "Changes Saved", "green", false);
        }

        public void write_node(Ebum e)
        {
            //try
            //{
                XP.WRITE_NODE_OF_FILE(e);
                PostConsole(Loaded_Gametypes[Current_Gametype].SSGF_File + " Meta Changes Saved!",
                            "Changes Saved", "green", false);
            //}
            //catch (Exception ex)
            //{
            //    PostConsole(ex.Message,
            //                "Meta Changes Failed", "red", true);
            //}
        }
        public void write_enum_node(Ebum e)
        {
            //try
            //{
            XP.WRITE_ENUM_NODE_OF_FILE(e);
            PostConsole(Loaded_Gametypes[Current_Gametype].SSGF_File + " Meta Changes Saved!",
                        "Changes Saved", "green", false);
            //}
            //catch (Exception ex)
            //{
            //    PostConsole(ex.Message,
            //                "Meta Changes Failed", "red", true);
            //}
        }

        public void write_using_compression_node(List<string> nodes, bool? use_compression)
        {
            XP.write_string_compression_type(nodes, use_compression);
            PostConsole(Loaded_Gametypes[Current_Gametype].SSGF_File + " Meta Changes Saved!",
                            "Changes Saved", "green", false);
        }
        public void add_new_table_string(List<string> nodes)
        {
            string haloxml2 = Loaded_Gametypes[Current_Gametype].Target_Halo;
            bool is_Reach = false;
            if (haloxml2 == "HR")
                is_Reach = true;

            XP.add_table_string(nodes, is_Reach);
            PostConsole(Loaded_Gametypes[Current_Gametype].SSGF_File + " Meta Changes Saved!",
                            "Changes Saved", "green", false);
        }
        public void table_write_string(string location, bool? use, string the_other_thing)
        {
            XP.write_string_use(location, use, the_other_thing);
            PostConsole(Loaded_Gametypes[Current_Gametype].SSGF_File + " Meta Changes Saved!",
                            "Changes Saved", "green", false);
        }

        private void XMLFILTER_in_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        #region WindowStyling
        // Can execute
        private void CommandBinding_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        // Minimize
        private void CommandBinding_Executed_Minimize(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MinimizeWindow(this);
        }

        // Maximize
        private void CommandBinding_Executed_Maximize(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.MaximizeWindow(this);
        }

        // Restore
        private void CommandBinding_Executed_Restore(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.RestoreWindow(this);
        }

        // Close
        private void CommandBinding_Executed_Close(object sender, ExecutedRoutedEventArgs e)
        {
            SystemCommands.CloseWindow(this);
        }
        #endregion
    }
}
