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

namespace gamtetyper
{
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
    public partial class MainWindow : Window
    {

        static XML_Process XP = new XML_Process();

        public List<string> WHY2(string a_xml, string path)
        {
            return XP.big_fella_test(a_xml, path);
        }
        public void WHY3(node_ebum_s parent_thing)
        {
            //string bad_fix_trim_his_BALLS = String.Join("", parent_thing.linkedthing.XMLPath.Split('/').SkipLast(1));
            string path = @"/base/" + parent_thing.linkedthing.XMLPath + "/Var[@name='"+parent_thing.linkedthing.V+ "']";
            string path2 = parent_thing.linkedthing.XMLPath;
            string doc = parent_thing.linkedthing.XMLDoc;

            var ebums = XP.readchildren(XP.readdata(path, doc), path2, doc, new List<string> { "BALLS" });

            parent_thing.linkedthing.Params = new List<Ebum>();

            parent_thing.child_panel.Children.Clear();
            if (ebums != null)
            {
                foreach (var v in ebums)
                {
                    parent_thing.linkedthing.Params.Add(v);
                    handleType(null, parent_thing.child_panel, v);
                }
            }
        }


        public inline_new_node_thing nodewind;
        private void parent_nodegraph_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (nodewind != null)
            {
                nodewind.is_fucking_closing = true;
                close_new_node_window();
                nodewind = null;
            }
            nodewind = new inline_new_node_thing();


            nodewind.main = this;
            nodewind.node_X = mouseX;
            nodewind.node_Y = mouseY;

            //if () // if target is halo reach then hide the branch option

            string haloxml2 = Loaded_Gametypes[Current_Gametype].Target_Halo;
            if (haloxml2 == "HR")
                nodewind.new_branch_butt.Visibility = Visibility.Collapsed;

            testab.Children.Add(nodewind);

            nodewind.transfrom_location.X = mouseX;
            nodewind.transfrom_location.Y = mouseY;

            nodewind.Focus();
        }
        public void close_new_node_window()
        {
            if (nodewind != null)
            {
                testab.Children.Remove(nodewind);
                nodewind = null;
            }
        }
        public void create_trigger(double x, double y)
        {
            string haloxml = returnmegldoc_fromhalo(Loaded_Gametypes[Current_Gametype].Target_Halo);

            Gametype.trigger trigger = new();
            trigger.Attribute = XP.cheat_to_do_the_node_creation("/base/ExTypes/Var[@name='MegaloScript']/Var[@name='TriggerCount']/Var[@name='Attribute']",
                                                                  "ExTypes/Var[@name='MegaloScript']/Var[@name='TriggerCount']", haloxml);
            trigger.Type = XP.cheat_to_do_the_node_creation("/base/ExTypes/Var[@name='MegaloScript']/Var[@name='TriggerCount']/Var[@name='Type']",
                                                            "ExTypes/Var[@name='MegaloScript']/Var[@name='TriggerCount']", haloxml);
            trigger.Name = "Trigger" + active_triggers.Count;
            trigger.unknown1 = 1;
            trigger.unknown2 = 1;

            CodeBlock cb = new CodeBlock { VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, main_window = this };
            cb.transfrom_location.X = x;
            cb.transfrom_location.Y = y;


            testab.Children.Add(cb);
            cb.main.Children.Remove(cb.in_connection);
            active_triggers.Add(cb);

            //active_triggers.Add(new Gametype.TriggerUI { UI = cb, stored_trigger = trigger });
            cb.trigger_parent = new Gametype.TriggerUI { UI = cb, stored_trigger = trigger };

            cb.typename.Content = trigger.Name;
            //cb.

            handleType(cb, cb.Content_panel, trigger.Type);

            handleType(cb, cb.Content_panel, trigger.Attribute);

            cb.head.Background = new SolidColorBrush(Color.FromArgb(255, 212, 117, 0));

            cb.trigger_parent.CHILD_elements_key = -1;

        }


        public void create_action(double x, double y)
        {
            string haloxml = returnmegldoc_fromhalo(Loaded_Gametypes[Current_Gametype].Target_Halo);


            Gametype.action action = new();
            action.Type = XP.cheat_to_do_the_node_creation("/base/ExTypes/Var[@name='MegaloScript']/Var[@name='ActionCount']/Var[@name='Type']",
                                                           "ExTypes/Var[@name='MegaloScript']/Var[@name='ActionCount']", haloxml);

            CodeBlock cb = new CodeBlock { VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, main_window = this };
            cb.transfrom_location.X = x;
            cb.transfrom_location.Y = y;

            //active_actions.Add(ind, new Gametype.ActionUI { UI = cb, stored_action = action });
            testab.Children.Add(cb);
            active_actions.Add(give_us_a_stupid_number(), cb);
            cb.action_parent = new Gametype.ActionUI { UI = cb, stored_action = action };
            cb.typename.Content = "Action";
            handleType(cb, cb.Content_panel, action.Type);
            cb.head.Background = new SolidColorBrush(Color.FromArgb(255, 214, 0, 0));

            int test_this_mofo = links.Count;
            while (links.ContainsKey(test_this_mofo))
            {
                test_this_mofo++;
            }
            List<potential_block> linkedstuff = new();
            linkedstuff.Add(new potential_block { _action = cb });
            links.Add(test_this_mofo, linkedstuff);
            cb.action_parent.linked_elements_key = test_this_mofo;
        }
        public void create_condition(double x, double y)
        {
            string haloxml = returnmegldoc_fromhalo(Loaded_Gametypes[Current_Gametype].Target_Halo);

            Gametype.condition condition = new();
            condition.Not = 0;
            condition.OR_Group = 0;
            condition.insertionpoint = -1;
            condition.Type = XP.cheat_to_do_the_node_creation("/base/ExTypes/Var[@name='MegaloScript']/Var[@name='ConditionCount']/Var[@name='Type']",
                                                              "ExTypes/Var[@name='MegaloScript']/Var[@name='ConditionCount']", haloxml);

            CodeBlock cb = new CodeBlock { VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, main_window = this };
            cb.transfrom_location.X = x;
            cb.transfrom_location.Y = y;

            testab.Children.Add(cb);
            active_conditions.Add(give_us_a_stupid_number(), cb);
            //active_conditions.Add(ind,new Gametype.ConditionUI { UI = cb, stored_condition = condition });
            cb.condition_parent = new Gametype.ConditionUI { UI = cb, stored_condition = condition };

            cb.typename.Content = "Condition";
            //cb.

            condition_top_blocks phee = new();
            phee.OR_group.Text = condition.OR_Group.ToString();
            phee.knot_box.IsChecked = (condition.Not == 1);
            cb.Content_panel.Children.Add(phee);
            phee.cb = cb;


            handleType(cb, cb.Content_panel, condition.Type);

            cb.head.Background = new SolidColorBrush(Color.FromArgb(255, 158, 0, 158));

            int test_this_mofo = links.Count;
            while (links.ContainsKey(test_this_mofo))
            {
                test_this_mofo++;
            }
            List<potential_block> linkedstuff = new();
            linkedstuff.Add(new potential_block { _condition = cb });
            links.Add(test_this_mofo, linkedstuff);
            cb.condition_parent.linked_elements_key = test_this_mofo;
        }
        public void create_branch(double x, double y)
        {
            Gametype.action action = new();

            // do i really even need to do something here?
            // im pretty sure we handle branches just off of the related nodes
            // ok i was wrong, we do need to do the thing here

            string haloxml = returnmegldoc_fromhalo(Loaded_Gametypes[Current_Gametype].Target_Halo);

            action.Type = new Ebum()
            {
                FUCKK_YOU = null,
                Name = "Type",
                Size = 8,
                Type = "Enum",
                V = "Virtual Trigger",
                XMLDoc = haloxml,
                XMLPath = "ExTypes / Var[@name = 'MegaloScript'] / Var[@name = 'ActionCount'] / Var[@name = 'Type']",
                Params = new()
                {
                    new Ebum()
                    {
                        FUCKK_YOU = "VirtualTriggerRef",
                        Name = "VirtualTriggerRef",
                        Size = 9,
                        Type = "Container",
                        V = null,
                        XMLDoc = haloxml,
                        XMLPath = "RefTypes/ Var[@name = 'VirtualTriggerRef']",
                        Params = new()
                        {
                            new Ebum()
                            {
                                FUCKK_YOU = null,
                                Name = "UnanmmedInt1",
                                Size = 10,
                                Type = "Int",
                                V = "-1",
                                XMLDoc = haloxml,
                                XMLPath = "RefTypes/Var[@name='VirtualTriggerRef']/ Var[@name = 'UnanmmedInt1']",
                                Params = null,
                            },
                            new Ebum()
                            {
                                FUCKK_YOU = null,
                                Name = "UnanmmedInt2",
                                Size = 10,
                                Type = "Int",
                                V = "-1",
                                XMLDoc = haloxml,
                                XMLPath = "RefTypes/Var[@name='VirtualTriggerRef']/ Var[@name = 'UnanmmedInt2']",
                                Params = null,
                            },
                            new Ebum()
                            {
                                FUCKK_YOU = null,
                                Name = "UnanmmedInt3",
                                Size = 11,
                                Type = "Int",
                                V = "-1",
                                XMLDoc = haloxml,
                                XMLPath = "RefTypes/Var[@name='VirtualTriggerRef']/ Var[@name = 'UnanmmedInt3']",
                                Params = null,
                            },
                            new Ebum()
                            {
                                FUCKK_YOU = null,
                                Name = "UnanmmedInt4",
                                Size = 11,
                                Type = "Int",
                                V = "-1",
                                XMLDoc = haloxml,
                                XMLPath = "RefTypes/Var[@name='VirtualTriggerRef']/ Var[@name = 'UnanmmedInt4']",
                                Params = null,
                            },
                        },
                    }
                },
            };

            BranchBlock cb = new BranchBlock { VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, main_window = this };
            cb.transfrom_location.X = x;
            cb.transfrom_location.Y = y;

            testab.Children.Add(cb);
            active_branches.Add(give_us_a_stupid_number(), cb);
            cb.branch_parent = new Gametype.BranchUI
            {
                UI = cb,
                stored_action = action
            };
            cb.branch_parent.CHILD_elements_key = -1;

            int test_this_mofo = links.Count;
            while (links.ContainsKey(test_this_mofo))
            {
                test_this_mofo++;
            }
            List<potential_block> linkedstuff = new();
            linkedstuff.Add(new potential_block { _branch = cb });
            links.Add(test_this_mofo, linkedstuff);
            cb.branch_parent.linked_elements_key = test_this_mofo;
        }
        public int give_us_a_stupid_number()
        {
            return active_actions.Count + active_branches.Count + active_conditions.Count;
        }


        

        public MainWindow()
        {
            InitializeComponent();

            XP.XMLdirectory = Directory.GetCurrentDirectory() + @"\XMLs\";

            string[] files = System.IO.Directory.GetFiles(Directory.GetCurrentDirectory() + @"\XMLs\Halos", "*.xml");
            foreach (string XMLFile in files)
            {
                string XMLName = System.IO.Path.GetFileNameWithoutExtension(XMLFile);
                Loaded_XMLS.Add(XMLName);
                ComboBoxItem newTabItem = new ComboBoxItem
                {
                    Name = System.IO.Path.GetFileNameWithoutExtension(XMLName),
                    Content = System.IO.Path.GetFileNameWithoutExtension(XMLName)
                };
                XMLFILTER.Items.Add(newTabItem);
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

        private List<Gametype.GametypeLoaded> Loaded_Gametypes = new List<Gametype.GametypeLoaded>();

        public int Current_Gametype = -1;


        private void OnSelectionChanged(Object sender, SelectionChangedEventArgs args) // change 
        {
            var tc = sender as TabControl; 

            if (tc != null)
            {
                var item = tc.SelectedItem;

                Current_Gametype = 0; // XMLFILTER.SelectedIndex;

                //Open_SGF(Loaded_Gametypes[Current_Gametype].SSGF_File);
                Load_Tab();


            }
        }


        public List<CodeBlock> active_triggers = new ();
        // im *pretty sure* that we only use the number for converting to nodes
        public Dictionary<int, CodeBlock> active_conditions = new();

        public Dictionary<int, CodeBlock> active_actions = new ();

        public Dictionary<int, BranchBlock> active_branches = new ();

        List<Gametype.trigger> t;
        List<Gametype.action> a;
        List<Gametype.condition> c;


        public int height = 0; // hopefully this should fix it
        public void dothething()
        {
            // bla lab la return struct data from xml

            string haloxml2 = Loaded_Gametypes[Current_Gametype].Target_Halo;
            string haloxml = returnmegldoc_fromhalo(haloxml2);
            bool is_Reach = false;
            if (haloxml2 == "HR")
                is_Reach = true;

            t = XP.returnScripts(is_Reach, haloxml);
            a = XP.doactions(haloxml);
            c = XP.doconditions(haloxml);

            for (int ind = 0; ind < a.Count; ind++)  //   Gametype.action
            {
                Gametype.action action = a[ind];
                //, Margin = new Thickness(400*ind, -300, 0, 0) 
                if (action.Type.V != "Virtual Trigger")
                {
                    CodeBlock cb = new CodeBlock { VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, main_window=this };
                    cb.transfrom_location.X = 400 * ind;
                    cb.transfrom_location.Y = -700;

                    //active_actions.Add(ind, new Gametype.ActionUI { UI = cb, stored_action = action });
                    testab.Children.Add(cb);
                    active_actions.Add(ind, cb);
                    cb.action_parent = new Gametype.ActionUI { UI = cb, stored_action = action };

                    cb.typename.Content = "Action";

                    handleType(cb, cb.Content_panel, action.Type);

                    cb.head.Background = new SolidColorBrush(Color.FromArgb(255, 214, 0, 0));
                }
                else
                {
                    BranchBlock cb = new BranchBlock { VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, main_window = this };
                    cb.transfrom_location.X = 400 * ind;
                    cb.transfrom_location.Y = -700;

                    testab.Children.Add(cb);
                    active_branches.Add(ind, cb);
                    cb.branch_parent = new Gametype.BranchUI
                    {
                        UI = cb,
                        stored_action = action
                    };
                }

                
                //cb.


            }

            for (int ind = 0; ind < c.Count; ind++)  //   Gametype.condition
            {
                Gametype.condition condition = c[ind];
                //, Margin = new Thickness(400 * ind, 300, 0, 0)
                CodeBlock cb = new CodeBlock { VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, main_window = this };
                cb.transfrom_location.X = 400 * ind;
                cb.transfrom_location.Y = -300;

                testab.Children.Add(cb);
                active_conditions.Add(ind, cb);
                //active_conditions.Add(ind,new Gametype.ConditionUI { UI = cb, stored_condition = condition });
                cb.condition_parent = new Gametype.ConditionUI { UI = cb, stored_condition = condition };

                cb.typename.Content = "Condition";
                //cb.

                condition_top_blocks phee = new();
                phee.OR_group.Text = condition.OR_Group.ToString();
                phee.knot_box.IsChecked = (condition.Not == 1);
                cb.Content_panel.Children.Add(phee);
                phee.cb = cb;


                handleType(cb, cb.Content_panel, condition.Type);

                cb.head.Background = new SolidColorBrush(Color.FromArgb(255, 158, 0, 158));
            }

            height = 0;
            int longitude = 0;


            foreach (Gametype.trigger trigger in t)  //   Gametype.trigger
            {
                longitude = 0;
                height += 300;

                CodeBlock cb = new CodeBlock { VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, main_window = this };
                cb.transfrom_location.X = 0;
                cb.transfrom_location.Y = height;


                testab.Children.Add(cb);
                cb.main.Children.Remove(cb.in_connection);
                active_triggers.Add(cb);

                //active_triggers.Add(new Gametype.TriggerUI { UI = cb, stored_trigger = trigger });
                cb.trigger_parent = new Gametype.TriggerUI { UI = cb, stored_trigger = trigger };

                cb.typename.Content = trigger.Name;
                //cb.

                handleType(cb, cb.Content_panel, trigger.Type);

                handleType(cb, cb.Content_panel, trigger.Attribute);

                cb.head.Background = new SolidColorBrush(Color.FromArgb(255, 212, 117, 0));

                run_the_loop_on_mf(trigger, longitude, cb, null);
            }

        }

        public struct nodelineupthing
        {
            public Gametype.action? g_action { get; set; }
            public CodeBlock g_cb { get; set; }
            public BranchBlock g_bb { get; set; }
        }

        public void run_the_loop_on_mf(Gametype.trigger trigger, int longitude, CodeBlock caller, BranchBlock? altcaller)
        {
            // clear those keys so it hopefully doesn't do something silly
            if (caller != null) // is a trigger
            {
                caller.trigger_parent.CHILD_elements_key = -1;
            }
            else // is a branch
            {
                altcaller.branch_parent.CHILD_elements_key = -1;
            }


            List<potential_block> linkedstuff = new();

            List<nodelineupthing> NODELINEUP = new();

            for (int w = 0; w < trigger.Actions_count; w++)  // LINE UP EVERY ACTION ELEMENT
            {
                int index_for_action = w + trigger.Actions_insert;

                if (a[index_for_action].Type.V != "Virtual Trigger")
                {
                    Gametype.action action = active_actions[index_for_action].action_parent.stored_action;

                    CodeBlock A = active_actions[index_for_action];
                    A.action_parent.linked_elements_key = trigger_links_index;

                    NODELINEUP.Add(new nodelineupthing { g_action = action, g_cb = A.action_parent.UI });

                    linkedstuff.Add(new potential_block { _action = A });

                }
                else
                {
                    Gametype.action action = active_branches[index_for_action].branch_parent.stored_action;

                    BranchBlock B = active_branches[index_for_action];
                    B.branch_parent.linked_elements_key = trigger_links_index;

                    NODELINEUP.Add(new nodelineupthing { g_action = action, g_bb = B.branch_parent.UI });

                    linkedstuff.Add(new potential_block { _branch = B });
                }

            }

            List<int> insertion_points = new List<int>();

            for (int w = 0; w < trigger.Conditions_count; w++)  // LINE UP EVERY CONDITION ELEMENT
            {
                int index_for_condition = w + trigger.Conditions_insert;

                CodeBlock C = active_conditions[index_for_condition];
                C.condition_parent.linked_elements_key = trigger_links_index;

                int condition_insert = C.condition_parent.stored_condition.insertionpoint;
                int factored_insertion = condition_insert;
                for (int q = 0; q < insertion_points.Count; q++)
                {
                    if (insertion_points[q] <= condition_insert)
                    {
                        factored_insertion++;
                    }
                }
                insertion_points.Add(condition_insert);

                NODELINEUP.Insert(factored_insertion, new nodelineupthing { g_cb = C.condition_parent.UI });
                linkedstuff.Insert(factored_insertion, new potential_block { _condition = C });
            }

            CodeBlock prev_to_link = caller;
            BranchBlock prev_to_link_branch = altcaller;


            foreach ( nodelineupthing kv in NODELINEUP)
            {
                if (kv.g_bb == null)
                { // normal code block
                    longitude += 220;
                    kv.g_cb.transfrom_location.X = longitude;
                    kv.g_cb.transfrom_location.Y = height;

                    double _x1 = 0;
                    double _y1 = 0;
                    if (prev_to_link != null)
                    {
                        _x1 = prev_to_link.transfrom_location.X;
                        _y1 = prev_to_link.transfrom_location.Y;
                    }
                    else
                    {
                        if (altcaller == prev_to_link_branch)
                        {
                            _x1 = prev_to_link_branch.transfrom_location.X;
                            _y1 = prev_to_link_branch.transfrom_location.Y;
                        }
                        else
                        {
                            _x1 = prev_to_link_branch.transfrom_location.X;
                            _y1 = prev_to_link_branch.transfrom_location.Y + 25;
                        }
                    }

                    Line node_connect = new Line
                    {
                        StrokeThickness = 5,
                        IsHitTestVisible = false,
                        Stroke = new SolidColorBrush(Colors.White),
                        X1 = _x1 + 192,
                        Y1 = _y1 + 17,
                        X2 = kv.g_cb.transfrom_location.X + 8,
                        Y2 = kv.g_cb.transfrom_location.Y + 17
                    };
                    testab.Children.Add(node_connect);

                    if (prev_to_link != null)
                    {
                        prev_to_link.Outpath = node_connect;
                    }
                    else
                    {
                        if (altcaller == prev_to_link_branch)
                        {
                            prev_to_link_branch.DOpath = node_connect;
                        }
                        else
                        {
                            prev_to_link_branch.THENpath = node_connect;
                        }
                    }
                    kv.g_cb.Inpath = node_connect;

                    if (kv.g_cb != null)
                    { // codeblock
                        prev_to_link = kv.g_cb;
                        prev_to_link_branch = null;
                    }
                    else
                    { // branchblock
                        prev_to_link_branch = kv.g_bb;
                        prev_to_link = null;
                    }
                }
                else
                { // branchblock
                    longitude += 220;
                    kv.g_bb.transfrom_location.X = longitude;
                    kv.g_bb.transfrom_location.Y = height;

                    bool is_reach = false;
                    int c_insert = int.Parse(kv.g_action.Value.Type.Params[0].Params[0].V);
                    int a_insert = int.Parse(kv.g_action.Value.Type.Params[0].Params[2].V);
                    if (!is_reach)
                    {
                        c_insert -= 1;
                        a_insert -= 1;
                    }
                    run_the_loop_on_mf(new Gametype.trigger
                    {

                        Conditions_insert = c_insert,
                        Actions_insert = a_insert,

                        Conditions_count = int.Parse(kv.g_action.Value.Type.Params[0].Params[1].V),
                        Actions_count = int.Parse(kv.g_action.Value.Type.Params[0].Params[3].V)
                    }, longitude, null, kv.g_bb);
                    height += 300;

                    double _x1 = 0;
                    double _y1 = 0;
                    if (prev_to_link != null)
                    {
                        _x1 = prev_to_link.transfrom_location.X;
                        _y1 = prev_to_link.transfrom_location.Y;
                    }
                    else
                    {
                        if (altcaller == prev_to_link_branch)
                        {
                            _x1 = prev_to_link_branch.transfrom_location.X;
                            _y1 = prev_to_link_branch.transfrom_location.Y;
                        }
                        else
                        {
                            _x1 = prev_to_link_branch.transfrom_location.X;
                            _y1 = prev_to_link_branch.transfrom_location.Y + 25;
                        }
                    }
                    Line node_connect = new Line
                    {
                        StrokeThickness = 5,
                        IsHitTestVisible = false,
                        Stroke = new SolidColorBrush(Colors.White),
                        X1 = _x1 + 192,
                        Y1 = _y1 + 17,
                        X2 = kv.g_bb.transfrom_location.X + 8,
                        Y2 = kv.g_bb.transfrom_location.Y + 17
                    };
                    testab.Children.Add(node_connect);

                    if (prev_to_link != null)
                    {
                        prev_to_link.Outpath = node_connect;
                    }
                    else
                    {
                        // check if node is root 
                        if (altcaller == prev_to_link_branch)
                        {
                            prev_to_link_branch.DOpath = node_connect;
                        }
                        else
                        {
                            prev_to_link_branch.THENpath = node_connect;
                        }
                    }
                    kv.g_bb.Inpath = node_connect;

                    if (kv.g_cb != null)
                    { // codeblock
                        prev_to_link = kv.g_cb;
                        prev_to_link_branch = null;
                    }
                    else
                    { // branchblock
                        prev_to_link_branch = kv.g_bb;
                        prev_to_link = null;
                    }
                }

                if (caller != null) // run for a normal trigger codeblock
                {
                    caller.trigger_parent.CHILD_elements_key = trigger_links_index;
                }
                else // run for the mentally challenged branch codeblock
                {
                    altcaller.branch_parent.CHILD_elements_key = trigger_links_index;
                }

                //links.actions = trigger.Actions;

                //links.conditions = trigger.Conditions;



                links.Add(trigger_links_index, linkedstuff);
                trigger_links_index++;
            }

        }

        public Dictionary<int, List<potential_block>> links = new();
        public int trigger_links_index = 0;
        public struct potential_block
        {
            public CodeBlock _action;
            public CodeBlock _condition;
            public BranchBlock _branch;
        }

        void handleType(CodeBlock target, StackPanel t_element, Gametype.Ebum append)
        {
            node_ebum_s y = new node_ebum_s();
            t_element.Children.Add(y);
            y.source_text.Content = append.V;

            //y.type_text.Text = append.Type;
            //y.bit_text.Text = append.Size.ToString();
            //rip bozo, you will be missed

            if (append.FUCKK_YOU != null)
                y.block_name.Text = append.FUCKK_YOU;
            else
                y.block_name.Text = append.Name;

            y.linkedthing = append;
            y.xmlp = this;

            if (append.Params != null)
            {
                foreach (Gametype.Ebum eb in append.Params)
                {
                    handleType(target, y.child_panel, eb);
                }
            }
        }

        // ----------------------------------
        // -- RUNTIME NODEGRAPH MANAGEMENT --
        // ----------------------------------
        //

        public void snap_links_connection(int link, int breakpoint, int KEY_to_inherit = -1)
        {
            List<potential_block> link_to_break = links[link];

            // if breakpoint is 0 and key_to_inherit isnt -1 then we wanna take it off a trigger and put it on an action
            if (breakpoint==0) // && KEY_to_inherit != -1
            {
                foreach (CodeBlock t in active_triggers)
                {
                    if (t.trigger_parent.CHILD_elements_key == link)
                    {
                        testab.Children.Remove(t.Outpath);
                        t.trigger_parent.CHILD_elements_key = -1;
                        t.Outpath = null;
                    }
                }
                foreach (KeyValuePair<int, BranchBlock> t in active_branches)
                {
                    if (t.Value.branch_parent.CHILD_elements_key == link)
                    {
                        testab.Children.Remove(t.Value.DOpath);
                        t.Value.branch_parent.CHILD_elements_key = -1;
                        t.Value.DOpath = null;
                    }
                }
            }
            int poopoint = breakpoint;
            //if (breakpoint == 0 && KEY_to_inherit != -1)
                poopoint--;
            // if we're actually breaking a link - conjoining doesn't need any links broken
            if (poopoint > -1)
            {
                var bigw = link_to_break[poopoint];
                if (bigw._action != null)
                {
                    testab.Children.Remove(bigw._action.Outpath);
                    bigw._action.Outpath = null;
                }
                if (bigw._branch != null)
                {
                    testab.Children.Remove(bigw._branch.THENpath);
                    bigw._branch.THENpath = null;
                }
                if (bigw._condition != null)
                {
                    testab.Children.Remove(bigw._condition.Outpath);
                    bigw._condition.Outpath = null;
                }
            }
            else if (KEY_to_inherit != -1)
            {
                var bigw = links[KEY_to_inherit];
                var bigw1 = bigw[bigw.Count-1];
                if (bigw1._action != null)
                {
                    testab.Children.Remove(bigw1._action.Outpath);
                    bigw1._action.Outpath = null;
                }
                if (bigw1._branch != null)
                {
                    testab.Children.Remove(bigw1._branch.THENpath);
                    bigw1._branch.THENpath = null;
                }
                if (bigw1._condition != null)
                {
                    testab.Children.Remove(bigw1._condition.Outpath);
                    bigw1._condition.Outpath = null;
                }
            }
            if (link_to_break.Count > poopoint + 1) // no idea why i did bp+1 // so we break the link of the next element as well
            {
                var bigw2 = link_to_break[poopoint + 1];
                if (bigw2._action != null)
                {
                    testab.Children.Remove(bigw2._action.Inpath);
                    bigw2._action.Inpath = null;
                }
                if (bigw2._branch != null)
                {
                    testab.Children.Remove(bigw2._branch.Inpath);
                    bigw2._branch.Inpath = null;
                }
                if (bigw2._condition != null)
                {
                    testab.Children.Remove(bigw2._condition.Inpath);
                    bigw2._condition.Inpath = null;
                }
            }
            

            List<potential_block> new_blocklist = link_to_break.Skip(poopoint + 1).ToList();

            List<potential_block> old_blocklist = link_to_break;
            while (old_blocklist.Count > poopoint + 1)
            {
                old_blocklist.RemoveAt(poopoint + 1);
            }
            links[link] = old_blocklist;

            // now assort the new list things


            int test_this_mofo = KEY_to_inherit;
            if (test_this_mofo == -1)
            {
                test_this_mofo = links.Count;
                while (links.ContainsKey(test_this_mofo))
                {
                    test_this_mofo++;
                }
            }
            else
            {
                string debug_moment = ":)";
            }

            foreach (potential_block pp in new_blocklist)
            {
                if (pp._action != null)
                {
                    pp._action.action_parent.linked_elements_key = test_this_mofo;
                }
                if (pp._branch != null)
                {
                    pp._branch.branch_parent.linked_elements_key = test_this_mofo;
                }
                if (pp._condition != null)
                {
                    pp._condition.condition_parent.linked_elements_key = test_this_mofo;
                }
            }
            if ( new_blocklist.Count>0) // KEY_to_inherit == -1 &&
            {
                potential_block thingo_to_disjoin = new_blocklist[0];
                if (thingo_to_disjoin._action != null)
                {
                    if (thingo_to_disjoin._action.Inpath != null)
                    {
                        testab.Children.Remove(thingo_to_disjoin._action.Inpath);
                        thingo_to_disjoin._action.Inpath = null;
                    }
                }
                if (thingo_to_disjoin._branch != null)
                {
                    if (thingo_to_disjoin._branch.Inpath != null)
                    {
                        testab.Children.Remove(thingo_to_disjoin._branch.Inpath);
                        thingo_to_disjoin._branch.Inpath = null;
                    }
                }
                if (thingo_to_disjoin._condition != null)
                {
                    if (thingo_to_disjoin._condition.Inpath != null)
                    {
                        testab.Children.Remove(thingo_to_disjoin._condition.Inpath);
                        thingo_to_disjoin._condition.Inpath = null;
                    }
                }

                if (links.ContainsKey(test_this_mofo))
                {
                    //links[test_this_mofo] = 
                    links[test_this_mofo].AddRange(new_blocklist);
                }
                else
                {
                    links.Add(test_this_mofo, new_blocklist);
                }
            }
            // add them to 
        }



        public CodeBlock? line_dragging_from_codeblock;
        public BranchBlock? line_dragging_from_branchblockTHEN;
        public BranchBlock? line_dragging_from_branchblockDO;
        public void cb_start_line_drag(CodeBlock cb)
        {
            line_dragging_from_branchblockTHEN = null;
            line_dragging_from_branchblockDO = null;
            line_dragging_from_codeblock = cb;

            if (cb.Outpath == null)
            {
                double _x1 = cb.transfrom_location.X;
                double _y1 = cb.transfrom_location.Y;
                cb.Outpath = new Line
                {
                    StrokeThickness = 5,
                    IsHitTestVisible = false,
                    Stroke = new SolidColorBrush(Colors.White),
                    X1 = _x1 + 192,
                    Y1 = _y1 + 17,
                    X2 = mouseX,
                    Y2 = mouseY
                };
                testab.Children.Add(cb.Outpath);
            }
        }
        public void cb_stop_line_drag_on_in(CodeBlock cb)
        {
            if (line_dragging_from_codeblock == null && line_dragging_from_branchblockTHEN == null && line_dragging_from_branchblockDO == null)
                return;
            if (line_dragging_from_codeblock != cb)
            {
                bool is_not_a_line_from_the_parent_block = true;
                // remove any prior link
                int owning_KEY_thing = -1;
                if (line_dragging_from_codeblock != null)
                {
                    if (line_dragging_from_codeblock.action_parent != null)
                    {
                        owning_KEY_thing = line_dragging_from_codeblock.action_parent.linked_elements_key;
                    }
                    else if (line_dragging_from_codeblock.condition_parent != null)
                    {
                        owning_KEY_thing = line_dragging_from_codeblock.condition_parent.linked_elements_key;
                    }
                    if (line_dragging_from_codeblock.Outpath == cb.Inpath) // bruh what the hell is this
                        is_not_a_line_from_the_parent_block = false;
                }

                if (line_dragging_from_branchblockTHEN != null)
                {
                    if (line_dragging_from_branchblockTHEN.branch_parent != null)
                    {
                        owning_KEY_thing = line_dragging_from_branchblockTHEN.branch_parent.linked_elements_key;
                    }
                    if (line_dragging_from_branchblockTHEN.THENpath == cb.Inpath)
                        is_not_a_line_from_the_parent_block = false;
                }

                if (line_dragging_from_branchblockDO != null)
                {
                    if (line_dragging_from_branchblockDO.DOpath == cb.Inpath)
                        is_not_a_line_from_the_parent_block = false;
                }


                if (is_not_a_line_from_the_parent_block)
                {
                    // ----------- DO CODE HERE TO DELINK FROM PREVIOUS GROUP

                    if (owning_KEY_thing != -1) // means that this ISNT a trigger type
                    {
                        //snap_links_connection(key_to_snap, breanpoint, owning_KEY_thing);
                        List<potential_block> base_link_to_find_index_from = links[owning_KEY_thing];
                        int breanpointbase = -1;
                        for (int p_index = 0; p_index < base_link_to_find_index_from.Count; p_index++)
                        {
                            var poop = base_link_to_find_index_from[p_index];
                            if (poop._action == line_dragging_from_codeblock && line_dragging_from_codeblock != null)
                            {
                                breanpointbase = p_index;
                                break;
                            }
                            else if (poop._condition == line_dragging_from_codeblock && line_dragging_from_codeblock != null)
                            {
                                breanpointbase = p_index;
                                break;
                            }
                            else if (poop._branch == line_dragging_from_branchblockTHEN && line_dragging_from_branchblockTHEN != null)
                            {
                                breanpointbase = p_index;
                                break;
                            }
                        }
                        snap_links_connection(owning_KEY_thing, breanpointbase+1);

                        int key_to_snap = -1;
                        if (cb.action_parent != null)
                        {
                            key_to_snap = cb.action_parent.linked_elements_key;
                        }
                        else if (cb.condition_parent != null)
                        {
                            key_to_snap = cb.condition_parent.linked_elements_key;
                        }
                        List<potential_block> snap_link_to_find_index_from = links[key_to_snap];
                        int breanpointsnap = -1;
                        for (int p_index = 0; p_index < snap_link_to_find_index_from.Count; p_index++)
                        {
                            if (snap_link_to_find_index_from[p_index]._action == cb || snap_link_to_find_index_from[p_index]._condition == cb)
                            {
                                breanpointsnap = p_index;
                                break;
                            }
                        }

                        snap_links_connection(key_to_snap, breanpointsnap, owning_KEY_thing);

                        // check if line was only snapped and not just appended

                        Line node_connect = new Line
                        {
                            StrokeThickness = 5,
                            IsHitTestVisible = false,
                            Stroke = new SolidColorBrush(Colors.White)
                        };
                        testab.Children.Add(node_connect);

                        if (line_dragging_from_codeblock != null)
                        {
                            line_dragging_from_codeblock.Outpath = node_connect;
                            node_connect.X1 = line_dragging_from_codeblock.transfrom_location.X + 192;
                            node_connect.Y1 = line_dragging_from_codeblock.transfrom_location.Y + 17;
                        }

                        if (line_dragging_from_branchblockTHEN != null)
                        {
                            line_dragging_from_branchblockTHEN.THENpath = node_connect;
                            node_connect.X1 = line_dragging_from_branchblockTHEN.transfrom_location.X + 192;
                            node_connect.Y1 = line_dragging_from_branchblockTHEN.transfrom_location.Y + 42;
                        }

                        if (line_dragging_from_branchblockDO != null)
                        {
                            line_dragging_from_branchblockDO.DOpath = node_connect;
                            node_connect.X1 = line_dragging_from_branchblockDO.transfrom_location.X + 192;
                            node_connect.Y1 = line_dragging_from_branchblockDO.transfrom_location.Y + 17;
                        }
                    }
                    else
                    {

                        int poop = -1;
                        if (line_dragging_from_branchblockDO != null)
                        {
                            poop = line_dragging_from_branchblockDO.branch_parent.CHILD_elements_key;
                        }
                        else // its a trigger
                        {
                            poop = line_dragging_from_codeblock.trigger_parent.CHILD_elements_key;
                        }
                        if (poop != -1)
                            snap_links_connection(poop, 0);
                        else
                        {
                            if (line_dragging_from_branchblockDO != null)
                            {
                                testab.Children.Remove(line_dragging_from_branchblockDO.DOpath);
                                line_dragging_from_branchblockDO.DOpath = null;
                            }
                            else // its a trigger
                            {
                                testab.Children.Remove(line_dragging_from_codeblock.Outpath);
                                line_dragging_from_codeblock.Outpath = null;
                            }
                        }

                        int key_to_snap = -1;
                        if (cb.action_parent != null)
                        {
                            key_to_snap = cb.action_parent.linked_elements_key;
                        }
                        else if (cb.condition_parent != null)
                        {
                            key_to_snap = cb.condition_parent.linked_elements_key;
                        }
                        List<potential_block> snap_link_to_find_index_from = links[key_to_snap];
                        int breanpointsnap = -1;
                        for (int p_index = 0; p_index < snap_link_to_find_index_from.Count; p_index++)
                        {
                            if (snap_link_to_find_index_from[p_index]._action == cb || snap_link_to_find_index_from[p_index]._condition == cb)
                            {
                                breanpointsnap = p_index;
                                break;
                            }
                        }

                        snap_links_connection(key_to_snap, breanpointsnap);

                        Line node_connect = new Line
                        {
                            StrokeThickness = 5,
                            IsHitTestVisible = false,
                            Stroke = new SolidColorBrush(Colors.White)
                        };
                        testab.Children.Add(node_connect);

                        if (line_dragging_from_codeblock != null)
                        {
                            line_dragging_from_codeblock.Outpath = node_connect;
                            node_connect.X1 = line_dragging_from_codeblock.transfrom_location.X + 192;
                            node_connect.Y1 = line_dragging_from_codeblock.transfrom_location.Y + 17;
                        }

                        if (line_dragging_from_branchblockTHEN != null)
                        {
                            line_dragging_from_branchblockTHEN.THENpath = node_connect;
                            node_connect.X1 = line_dragging_from_branchblockTHEN.transfrom_location.X + 192;
                            node_connect.Y1 = line_dragging_from_branchblockTHEN.transfrom_location.Y + 42;
                        }

                        if (line_dragging_from_branchblockDO != null)
                        {
                            line_dragging_from_branchblockDO.DOpath = node_connect;
                            node_connect.X1 = line_dragging_from_branchblockDO.transfrom_location.X + 192;
                            node_connect.Y1 = line_dragging_from_branchblockDO.transfrom_location.Y + 17;
                        }


                        key_to_snap = -1;
                        if (cb.action_parent != null)
                        {
                            key_to_snap = cb.action_parent.linked_elements_key;
                        }
                        else if (cb.condition_parent != null)
                        {
                            key_to_snap = cb.condition_parent.linked_elements_key;
                        }

                        if (line_dragging_from_branchblockDO != null)
                        {
                            line_dragging_from_branchblockDO.branch_parent.CHILD_elements_key = key_to_snap;
                        }
                        else // its a trigger
                        {
                            line_dragging_from_codeblock.trigger_parent.CHILD_elements_key = key_to_snap;
                        }
                    }
                }
                // do a different thing down here for triggers and branch DO's


                // check if codeblock is action or not
                if (line_dragging_from_codeblock != null) // bad way to check but ok
                {
                    // parent block is a trigger
                    if (line_dragging_from_codeblock.trigger_parent != null)
                    {
                        //if (cb.action_parent.linked_elements_key )
                        //line_dragging_from_codeblock.trigger_parent.CHILD_elements_key = cb.s
                        cb.Inpath = line_dragging_from_codeblock.Outpath;
                        drag_block(cb, 0, 0);
                        drag_block(line_dragging_from_codeblock, 0, 0);

                    }
                    else // parent block is an action/condition
                    {
                        //var xd = links[line_dragging_from_codeblock.trigger_parent.CHILD_elements_key];
                        // ----------- DO CODE HERE TO LINK TO NEW GROUP
                        cb.Inpath = line_dragging_from_codeblock.Outpath;
                        drag_block(cb, 0, 0);
                        drag_block(line_dragging_from_codeblock, 0, 0);

                    }
                }
                else // line_dragging_from_branchblock != null
                {
                    // parent branch block is THEN
                    if (line_dragging_from_branchblockTHEN != null)
                    {
                        cb.Inpath = line_dragging_from_branchblockTHEN.THENpath;
                        drag_block(cb, 0, 0);
                        drag_retarded_block(line_dragging_from_branchblockTHEN, 0, 0);

                        // ----------- DO CODE HERE TO LINK TO NEW GROUP
                    }
                    // parent branch block is DO
                    else if (line_dragging_from_branchblockDO != null)
                    {
                        cb.Inpath = line_dragging_from_branchblockDO.DOpath;
                        drag_block(cb, 0, 0);
                        drag_retarded_block(line_dragging_from_branchblockDO, 0, 0);
                        // ----------- DO CODE HERE TO LINK TO NEW GROUP
                    }
                }
                line_dragging_from_codeblock = null;
                line_dragging_from_branchblockTHEN = null;
                line_dragging_from_branchblockDO = null;
            }
        }
        public void bb_start_lineDO_drag(BranchBlock bb)
        {
            line_dragging_from_codeblock = null;
            line_dragging_from_branchblockTHEN = null;
            line_dragging_from_branchblockDO = bb;

            if (line_dragging_from_branchblockDO.DOpath == null)
            {
                double _x1 = bb.transfrom_location.X;
                double _y1 = bb.transfrom_location.Y;
                line_dragging_from_branchblockDO.DOpath = new Line
                {
                    StrokeThickness = 5,
                    IsHitTestVisible = false,
                    Stroke = new SolidColorBrush(Colors.White),
                    X1 = _x1 + 192,
                    Y1 = _y1 + 17,
                    X2 = mouseX,
                    Y2 = mouseY
                };
                testab.Children.Add(line_dragging_from_branchblockDO.DOpath);
            }
        }
        public void bb_start_lineTHEN_drag(BranchBlock bb)
        {
            line_dragging_from_codeblock = null;
            line_dragging_from_branchblockTHEN = bb;
            line_dragging_from_branchblockDO = null;

            if (line_dragging_from_branchblockTHEN.THENpath == null)
            {
                double _x1 = bb.transfrom_location.X;
                double _y1 = bb.transfrom_location.Y;
                line_dragging_from_branchblockTHEN.THENpath = new Line
                {
                    StrokeThickness = 5,
                    IsHitTestVisible = false,
                    Stroke = new SolidColorBrush(Colors.White),
                    X1 = _x1 + 192,
                    Y1 = _y1 + 42,
                    X2 = mouseX,
                    Y2 = mouseY
                };
                testab.Children.Add(line_dragging_from_branchblockTHEN.THENpath);
            }
        }
        public void bb_stop_line_drag_on_in(BranchBlock bb)
        {
            if (line_dragging_from_codeblock == null && line_dragging_from_branchblockTHEN == null && line_dragging_from_branchblockDO == null)
                return;
            if (line_dragging_from_branchblockTHEN != bb && line_dragging_from_branchblockDO != bb)
            {
                bool is_not_a_line_from_the_parent_block = true;
                // remove any prior link
                int owning_KEY_thing = -1;
                if (line_dragging_from_codeblock != null)
                {
                    if (line_dragging_from_codeblock.action_parent != null)
                    {
                        owning_KEY_thing = line_dragging_from_codeblock.action_parent.linked_elements_key;
                    }
                    else if (line_dragging_from_codeblock.condition_parent != null)
                    {
                        owning_KEY_thing = line_dragging_from_codeblock.condition_parent.linked_elements_key;
                    }
                    if (line_dragging_from_codeblock.Outpath == bb.Inpath) // bruh what the hell is this
                        is_not_a_line_from_the_parent_block = false;
                }

                if (line_dragging_from_branchblockTHEN != null)
                {
                    if (line_dragging_from_branchblockTHEN.branch_parent != null)
                    {
                        owning_KEY_thing = line_dragging_from_branchblockTHEN.branch_parent.linked_elements_key;
                    }
                    if (line_dragging_from_branchblockTHEN.THENpath == bb.Inpath)
                        is_not_a_line_from_the_parent_block = false;
                }

                if (line_dragging_from_branchblockDO != null)
                {
                    if (line_dragging_from_branchblockDO.DOpath == bb.Inpath)
                        is_not_a_line_from_the_parent_block = false;
                }


                if (is_not_a_line_from_the_parent_block)
                {
                    // ----------- DO CODE HERE TO DELINK FROM PREVIOUS GROUP


                    if (owning_KEY_thing != -1) // means that this ISNT a trigger type
                    {
                        //snap_links_connection(key_to_snap, breanpoint, owning_KEY_thing);
                        List<potential_block> base_link_to_find_index_from = links[owning_KEY_thing];
                        int breanpointbase = -1;
                        for (int p_index = 0; p_index < base_link_to_find_index_from.Count; p_index++)
                        {
                            var poop = base_link_to_find_index_from[p_index];
                            if (poop._action == line_dragging_from_codeblock && line_dragging_from_codeblock != null)
                            {
                                breanpointbase = p_index;
                                break;
                            }
                            else if (poop._condition == line_dragging_from_codeblock && line_dragging_from_codeblock != null)
                            {
                                breanpointbase = p_index;
                                break;
                            }
                            else if (poop._branch == line_dragging_from_branchblockTHEN && line_dragging_from_branchblockTHEN != null)
                            {
                                breanpointbase = p_index;
                                break;
                            }
                        }
                        snap_links_connection(owning_KEY_thing, breanpointbase + 1);

                        int key_to_snap = -1;
                        if (bb.branch_parent != null)
                        {
                            key_to_snap = bb.branch_parent.linked_elements_key;
                        }
                        List<potential_block> snap_link_to_find_index_from = links[key_to_snap];
                        int breanpointsnap = -1;
                        for (int p_index = 0; p_index < snap_link_to_find_index_from.Count; p_index++)
                        {
                            if (snap_link_to_find_index_from[p_index]._branch == bb)
                            {
                                breanpointsnap = p_index;
                                break;
                            }
                        }

                        snap_links_connection(key_to_snap, breanpointsnap, owning_KEY_thing);

                        // check if line was only snapped and not just appended

                        Line node_connect = new Line
                        {
                            StrokeThickness = 5,
                            IsHitTestVisible = false,
                            Stroke = new SolidColorBrush(Colors.White)
                        };
                        testab.Children.Add(node_connect);

                        if (line_dragging_from_codeblock != null)
                        {
                            line_dragging_from_codeblock.Outpath = node_connect;
                            node_connect.X1 = line_dragging_from_codeblock.transfrom_location.X + 192;
                            node_connect.Y1 = line_dragging_from_codeblock.transfrom_location.Y + 17;
                        }

                        if (line_dragging_from_branchblockTHEN != null)
                        {
                            line_dragging_from_branchblockTHEN.THENpath = node_connect;
                            node_connect.X1 = line_dragging_from_branchblockTHEN.transfrom_location.X + 192;
                            node_connect.Y1 = line_dragging_from_branchblockTHEN.transfrom_location.Y + 42;
                        }

                        if (line_dragging_from_branchblockDO != null)
                        {
                            line_dragging_from_branchblockDO.DOpath = node_connect;
                            node_connect.X1 = line_dragging_from_branchblockDO.transfrom_location.X + 192;
                            node_connect.Y1 = line_dragging_from_branchblockDO.transfrom_location.Y + 17;
                        }
                    }
                    else
                    {

                        int poop = -1;
                        if (line_dragging_from_branchblockDO != null)
                        {
                            poop = line_dragging_from_branchblockDO.branch_parent.CHILD_elements_key;
                        }
                        else // its a trigger
                        {
                            poop = line_dragging_from_codeblock.trigger_parent.CHILD_elements_key;
                        }
                        if (poop != -1)
                            snap_links_connection(poop, 0);
                        else
                        {
                            if (line_dragging_from_branchblockDO != null)
                            {
                                testab.Children.Remove(line_dragging_from_branchblockDO.DOpath);
                                line_dragging_from_branchblockDO.DOpath = null;
                            }
                            else // its a trigger
                            {
                                testab.Children.Remove(line_dragging_from_codeblock.Outpath);
                                line_dragging_from_codeblock.Outpath = null;
                            }
                        }

                        int key_to_snap = -1;
                        if (bb.branch_parent != null)
                        {
                            key_to_snap = bb.branch_parent.linked_elements_key;
                        }
                        List<potential_block> snap_link_to_find_index_from = links[key_to_snap];
                        int breanpointsnap = -1;
                        for (int p_index = 0; p_index < snap_link_to_find_index_from.Count; p_index++)
                        {
                            if (snap_link_to_find_index_from[p_index]._branch == bb)
                            {
                                breanpointsnap = p_index;
                                break;
                            }
                        }

                        snap_links_connection(key_to_snap, breanpointsnap);

                        Line node_connect = new Line
                        {
                            StrokeThickness = 5,
                            IsHitTestVisible = false,
                            Stroke = new SolidColorBrush(Colors.White)
                        };
                        testab.Children.Add(node_connect);

                        if (line_dragging_from_codeblock != null)
                        {
                            line_dragging_from_codeblock.Outpath = node_connect;
                            node_connect.X1 = line_dragging_from_codeblock.transfrom_location.X + 192;
                            node_connect.Y1 = line_dragging_from_codeblock.transfrom_location.Y + 17;
                        }

                        if (line_dragging_from_branchblockTHEN != null)
                        {
                            line_dragging_from_branchblockTHEN.THENpath = node_connect;
                            node_connect.X1 = line_dragging_from_branchblockTHEN.transfrom_location.X + 192;
                            node_connect.Y1 = line_dragging_from_branchblockTHEN.transfrom_location.Y + 42;
                        }

                        if (line_dragging_from_branchblockDO != null)
                        {
                            line_dragging_from_branchblockDO.DOpath = node_connect;
                            node_connect.X1 = line_dragging_from_branchblockDO.transfrom_location.X + 192;
                            node_connect.Y1 = line_dragging_from_branchblockDO.transfrom_location.Y + 17;
                        }


                        key_to_snap = -1;
                        if (bb.branch_parent != null)
                        {
                            key_to_snap = bb.branch_parent.linked_elements_key;
                        }

                        if (line_dragging_from_branchblockDO != null)
                        {
                            line_dragging_from_branchblockDO.branch_parent.CHILD_elements_key = key_to_snap;
                        }
                        else // its a trigger
                        {
                            line_dragging_from_codeblock.trigger_parent.CHILD_elements_key = key_to_snap;
                        }
                    }
                }
                // do a different thing down here for triggers and branch DO's


                // check if codeblock is action or not
                if (line_dragging_from_codeblock != null) // bad way to check but ok
                {
                    // parent block is a trigger
                    if (line_dragging_from_codeblock.trigger_parent != null)
                    {
                        //if (cb.action_parent.linked_elements_key )
                        //line_dragging_from_codeblock.trigger_parent.CHILD_elements_key = cb.s
                        bb.Inpath = line_dragging_from_codeblock.Outpath;
                        drag_retarded_block(bb, 0, 0);
                        drag_block(line_dragging_from_codeblock, 0, 0);

                    }
                    else // parent block is an action/condition
                    {
                        //var xd = links[line_dragging_from_codeblock.trigger_parent.CHILD_elements_key];
                        // ----------- DO CODE HERE TO LINK TO NEW GROUP
                        bb.Inpath = line_dragging_from_codeblock.Outpath;
                        drag_retarded_block(bb, 0, 0);
                        drag_block(line_dragging_from_codeblock, 0, 0);

                    }
                }
                else // line_dragging_from_branchblock != null
                {
                    // parent branch block is THEN
                    if (line_dragging_from_branchblockTHEN != null)
                    {
                        bb.Inpath = line_dragging_from_branchblockTHEN.THENpath;
                        drag_retarded_block(bb, 0, 0);
                        drag_retarded_block(line_dragging_from_branchblockTHEN, 0, 0);

                        // ----------- DO CODE HERE TO LINK TO NEW GROUP
                    }
                    // parent branch block is DO
                    else if (line_dragging_from_branchblockDO != null)
                    {
                        bb.Inpath = line_dragging_from_branchblockDO.DOpath;
                        drag_retarded_block(bb, 0, 0);
                        drag_retarded_block(line_dragging_from_branchblockDO, 0, 0);
                        // ----------- DO CODE HERE TO LINK TO NEW GROUP
                    }
                }
                line_dragging_from_codeblock = null;
                line_dragging_from_branchblockTHEN = null;
                line_dragging_from_branchblockDO = null;
            }
        }

        // ----------------------
        // -- NODEGRAPH SAVING --
        // ----------------------
        //
        List<Gametype.trigger> export_triggs = new();
        List<Gametype.action> export_actions = new();
        List<Gametype.condition> export_condis = new();

        Dictionary<int, trigger_things> mapped_trig_groups = new();
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            export_triggs.Clear();
            export_actions.Clear();
            export_condis.Clear();

            mapped_trig_groups.Clear();

            foreach (var v in active_triggers)
            {
                Gametype.trigger outthis = v.trigger_parent.stored_trigger;

                int poop = v.trigger_parent.CHILD_elements_key;
                trigger_things t = read_trigger_thing(poop, false);
                outthis.Conditions_count = t.cond_count;
                outthis.Conditions_insert = t.cond_offset;
                outthis.Actions_count = t.acti_count;
                outthis.Actions_insert = t.acti_offset;


                export_triggs.Add(outthis);
            }

            string haloxml2 = Loaded_Gametypes[Current_Gametype].Target_Halo;
            bool is_Reach = false;
            if (haloxml2 == "HR")
                is_Reach = true;

            XP.exportScripts(export_triggs, is_Reach);
            XP.exportactions(export_actions);
            XP.exportconditions(export_condis);
        }
        public trigger_things read_trigger_thing(int poop, bool ishaloreach)
        {
            List<Gametype.action> temp_actions = new();
            List<Gametype.condition> temp_condis = new();

            if (mapped_trig_groups.ContainsKey(poop))
            { // use the previous link
                return mapped_trig_groups[poop];
            }
            else
            { // make a new link

                int _actions_count = 0;
                int _conditions_count = 0;
                trigger_things t = new trigger_things();
                if (links.ContainsKey(poop))
                {
                    List<potential_block> pe = links[poop];
                    for (int se = 0; se < pe.Count; se++)
                    {
                        potential_block X = pe[se];
                        if (X._action != null)
                        {
                            _actions_count++;
                            temp_actions.Add(X._action.action_parent.stored_action);
                        }
                        else if (X._branch != null)
                        {
                            _actions_count++;
                            int branch_link_key = X._branch.branch_parent.CHILD_elements_key;
                            trigger_things branch_values = read_trigger_thing(branch_link_key, ishaloreach);

                            Gametype.action branchthingo = X._branch.branch_parent.stored_action;
                            // cond offset
                            Ebum e1 = branchthingo.Type.Params[0].Params[0];
                            e1.V = branch_values.cond_offset.ToString();
                            branchthingo.Type.Params[0].Params[0] = e1;
                            // cond count
                            Ebum e2 = branchthingo.Type.Params[0].Params[1];
                            e2.V = branch_values.cond_count.ToString();
                            branchthingo.Type.Params[0].Params[1] = e2;
                            // acti offset
                            Ebum e3 = branchthingo.Type.Params[0].Params[2];
                            e3.V = branch_values.acti_offset.ToString();
                            branchthingo.Type.Params[0].Params[2] = e3;
                            // acti count
                            Ebum e4 = branchthingo.Type.Params[0].Params[3];
                            e4.V = branch_values.acti_count.ToString();
                            branchthingo.Type.Params[0].Params[3] = e4;

                            temp_actions.Add(branchthingo);

                        }
                        else if (X._condition != null)
                        {
                            Gametype.condition gp = X._condition.condition_parent.stored_condition;

                            gp.insertionpoint = se - _conditions_count;
                            _conditions_count++;

                            temp_condis.Add(gp);
                        }
                    }

                }
                t.acti_count = temp_actions.Count;
                t.cond_count = temp_condis.Count;
                t.acti_offset = export_actions.Count;
                t.cond_offset = export_condis.Count;

                if (!ishaloreach)
                {
                    t.acti_offset++;
                    t.cond_offset++;
                }

                mapped_trig_groups.Add(poop, t);

                export_actions.AddRange(temp_actions);
                export_condis.AddRange(temp_condis);

                return t;
            }
        }
        public struct trigger_things
        {
            public int cond_offset;
            public int cond_count;
            public int acti_offset;
            public int acti_count;
        }

        public void Load_Tab()
        {
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
            }
            else
            {
                CompButton.IsEnabled = false;
                CompAsButton.IsEnabled = false;
                SaveButton.IsEnabled = false;
                SaveAsButton.IsEnabled = false;
            }

            XMLFILTER.SelectedItem = Loaded_Gametypes[Current_Gametype].Target_Halo;

            OpnBinText.Text = Loaded_Gametypes[Current_Gametype].BIN_File;
            OpnDumpText.Text = Loaded_Gametypes[Current_Gametype].SSGF_File;

           // Open_SGF(Loaded_Gametypes[Current_Gametype].SSGF_File);
        }

        public void Import_SGF(string SGF_to_load)
        {
            if (Current_Gametype == -1)
            {
                Gametype.GametypeLoaded SGFLoad = new Gametype.GametypeLoaded();
                SGFLoad.SSGF_File = SGF_to_load;

                SGFLoad.Target_Halo = Loaded_XMLS[XMLFILTER.SelectedIndex];


                Loaded_Gametypes.Add(SGFLoad);
                string tabname = System.IO.Path.GetFileNameWithoutExtension(SGF_to_load);
                tabname = Regex.Replace(tabname, "[^a-zA-Z0-9]", String.Empty);
                TabItem newTabItem = new TabItem
                {
                    Header = tabname,
                    Name = tabname
                };
                gmaetypeTabs.Items.Add(newTabItem);
                gmaetypeTabs.SelectedItem = newTabItem;

                //Open_SGF(SGF_to_load);
            }
            else
            {
                Gametype.GametypeLoaded edit = Loaded_Gametypes[Current_Gametype];
                edit.SSGF_File = SGF_to_load;
                Loaded_Gametypes[Current_Gametype] = edit;
                Load_Tab();
            }
        }

        public void Import_BIN(string BIN_to_load)
        {
            if (Current_Gametype == -1)
            {
                Gametype.GametypeLoaded BinLoad = new Gametype.GametypeLoaded();
                BinLoad.BIN_File = BIN_to_load;
                BinLoad.Target_Halo = Loaded_XMLS[XMLFILTER.SelectedIndex];

                Loaded_Gametypes.Add(BinLoad);
                string tabname = System.IO.Path.GetFileNameWithoutExtension(BIN_to_load);
                tabname = Regex.Replace(tabname, "[^a-zA-Z0-9]", String.Empty);
                TabItem newTabItem = new TabItem
                {
                    Header = tabname,
                    Name = tabname
                };
                gmaetypeTabs.Items.Add(newTabItem);
                gmaetypeTabs.SelectedItem = newTabItem;
            }
            else
            {
                Gametype.GametypeLoaded edit = Loaded_Gametypes[Current_Gametype];
                edit.BIN_File = BIN_to_load;
                Loaded_Gametypes[Current_Gametype] = edit;
                Load_Tab();
            }
        }

        private void OpnBINButton_Click(object sender, RoutedEventArgs e)
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

        private void OpnDmpButton_Click(object sender, RoutedEventArgs e)
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
                dothething();
            }

        }

        private void DecompButton_Click(object sender, RoutedEventArgs e)
        {

            BitReader b = new BitReader();
            b.m_process = XP;
            

            if (Loaded_Gametypes[Current_Gametype].SSGF_File == null)
            {
                Gametype.GametypeLoaded edit = Loaded_Gametypes[Current_Gametype];
                string s = Loaded_Gametypes[Current_Gametype].BIN_File;
                edit.SSGF_File = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(s), System.IO.Path.GetFileNameWithoutExtension(s)) + ".sgf";
                Loaded_Gametypes[Current_Gametype] = edit;
                Load_Tab();
            }

            b.m_process.instaniaterwrite(Loaded_Gametypes[Current_Gametype].SSGF_File);

            byte[] fileBytes = File.ReadAllBytes(Loaded_Gametypes[Current_Gametype].BIN_File);
            b.readbin(fileBytes, "Halos\\" + Loaded_Gametypes[Current_Gametype].Target_Halo + ".xml");
            b.m_process.enprocess();

            PostConsole(Loaded_Gametypes[Current_Gametype].BIN_File + " Successfully Decompiled!");
        }

        private void CompButton_Click(object sender, RoutedEventArgs e)
        {

            BitWriter y = new BitWriter();
            y.m_process = XP;


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

            PostConsole(Loaded_Gametypes[Current_Gametype].SSGF_File + " Successfully Compiled!");
        }


        public void PostConsole(string Message)
        {
            TextBlock tb = new TextBlock();
            tb.Text = Message;
            ConsolePanel.Children.Add(tb);
        }


        public double mouseX;
        public double mouseY;

        public double zoom = 1;

        public bool nodegraph_grabbed;
        public bool graph_in_lod_state;
        private void testab_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            zoom += ((float)e.Delta) / 1000;
            if (zoom > 2) zoom = 2;
            else if (zoom < 0.2f) zoom = 0.2f;
            nodegraph_scale.ScaleX = zoom;
            nodegraph_scale.ScaleY = zoom;

            testab.RenderTransformOrigin = new(((1f / (testab.ActualWidth)) * -nodegraph_trans.X) + 0.5f, ((1f / (testab.ActualHeight)) * -nodegraph_trans.Y) + 0.5f);

            DEBUG_LOCAITON.X = RenderTransformOrigin.X * testab.ActualWidth;
            DEBUG_LOCAITON.Y = RenderTransformOrigin.Y * testab.ActualHeight;

            if (!graph_in_lod_state && zoom <= 0.2f)
            {
                graph_in_lod_state = true;
                foreach(var v in active_triggers)
                {
                    v.Content_panel.Visibility = Visibility.Hidden;
                }
                foreach (var v in active_conditions)
                {
                    v.Value.Content_panel.Visibility = Visibility.Hidden;
                }
                foreach (var v in active_actions)
                {
                    v.Value.Content_panel.Visibility = Visibility.Hidden;
                }
            }
            if (graph_in_lod_state && zoom > 0.2f)
            {
                graph_in_lod_state = false;
                foreach (var v in active_triggers)
                {
                    v.Content_panel.Visibility = Visibility.Visible;
                }
                foreach (var v in active_conditions)
                {
                    v.Value.Content_panel.Visibility = Visibility.Visible;
                }
                foreach (var v in active_actions)
                {
                    v.Value.Content_panel.Visibility = Visibility.Visible;
                }
            }
        }
        // run all movement events
        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            double DeltaX = mouseX - e.GetPosition(testab).X;
            double DeltaY = mouseY - e.GetPosition(testab).Y;

            double factored_X = DeltaX * nodegraph_scale.ScaleX;
            double factored_Y = DeltaY * nodegraph_scale.ScaleY;

            // 
            foreach (CodeBlock cb in active_triggers)
            {
                if (cb.is_grabbed)
                {
                    drag_block(cb, DeltaX, DeltaY);
                }
            }
            foreach (KeyValuePair<int, CodeBlock> cb in active_actions)
            {
                if (cb.Value.is_grabbed)
                {
                    drag_block(cb.Value, DeltaX, DeltaY);
                }
            }
            foreach (KeyValuePair<int, CodeBlock> cb in active_conditions)
            {
                if (cb.Value.is_grabbed)
                {
                    drag_block(cb.Value, DeltaX, DeltaY);
                }
            }
            foreach (KeyValuePair<int, BranchBlock> cb in active_branches)
            {
                if (cb.Value.is_grabbed)
                {
                    drag_retarded_block(cb.Value, DeltaX, DeltaY);
                }
            }

            if (nodegraph_grabbed)
            {
                //testab.
                //nodegraph_scale
                nodegraph_trans.X -= factored_X;
                nodegraph_trans.Y -= factored_Y;


            }
            //nodegraph_scale.CenterX = -nodegraph_trans.X * nodegraph_scale.ScaleX;
            //nodegraph_scale.CenterY = -nodegraph_trans.Y * nodegraph_scale.ScaleY;

            //testab.RenderTransformOrigin.X = testab.RenderSize.Width / nodegraph_trans.X;
            //testab.RenderTransformOrigin.Y = testab.RenderSize.Height / nodegraph_trans.Y;

            testab.RenderTransformOrigin = new (((1f / (testab.ActualWidth)) * -nodegraph_trans.X)+0.5f, ((1f / (testab.ActualHeight)) * -nodegraph_trans.Y) +0.5f);


            DEBUG_LOCAITON.X = RenderTransformOrigin.X*testab.ActualWidth;
            DEBUG_LOCAITON.Y = RenderTransformOrigin.Y * testab.ActualHeight;

            mouseX = e.GetPosition(testab).X;
            mouseY = e.GetPosition(testab).Y;

            //
            // -- move lines that are currently held
            //

            if (line_dragging_from_branchblockTHEN != null)
            {
                line_dragging_from_branchblockTHEN.THENpath.X2 = mouseX;
                line_dragging_from_branchblockTHEN.THENpath.Y2 = mouseY;
            }
            if (line_dragging_from_branchblockDO != null)
            {
                line_dragging_from_branchblockDO.DOpath.X2 = mouseX;
                line_dragging_from_branchblockDO.DOpath.Y2 = mouseY;
            }
            if (line_dragging_from_codeblock != null)
            {
                line_dragging_from_codeblock.Outpath.X2 = mouseX;
                line_dragging_from_codeblock.Outpath.Y2 = mouseY;
            }
        }

        public void drag_block(CodeBlock cb, double Delta_X, double Delta_Y)
        {
            cb.transfrom_location.X -= Delta_X;
            cb.transfrom_location.Y -= Delta_Y;
            if (cb.Inpath != null)
            {
                cb.Inpath.X2 = cb.transfrom_location.X + 8;
                cb.Inpath.Y2 = cb.transfrom_location.Y + 17;
            }
            if (cb.Outpath != null)
            {
                cb.Outpath.X1 = cb.transfrom_location.X + 192;
                cb.Outpath.Y1 = cb.transfrom_location.Y + 17;
            }
        }

        public void drag_retarded_block(BranchBlock cb, double Delta_X, double Delta_Y)
        {
            cb.transfrom_location.X -= Delta_X;
            cb.transfrom_location.Y -= Delta_Y;
            if (cb.Inpath != null)
            {
                cb.Inpath.X2 = cb.transfrom_location.X + 8;
                cb.Inpath.Y2 = cb.transfrom_location.Y + 17;
            }
            if (cb.DOpath != null)
            {
                cb.DOpath.X1 = cb.transfrom_location.X + 192;
                cb.DOpath.Y1 = cb.transfrom_location.Y + 17;
            }
            if (cb.THENpath != null)
            {
                cb.THENpath.X1 = cb.transfrom_location.X + 192;
                cb.THENpath.Y1 = cb.transfrom_location.Y + 42;
            }
        }

        private void hide_panel_Click(object sender, RoutedEventArgs e)
        {
            meta_viewer.Visibility = Visibility.Collapsed;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            nodegraph_grabbed = true;
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            nodegraph_grabbed = false;
            wait_a_second_and_then_solve_our_problem();
            //drop all nodes too
        }
        async void wait_a_second_and_then_solve_our_problem()
        {
            //await Task.Delay(5);


            if (line_dragging_from_codeblock != null)
            {
                if (line_dragging_from_codeblock.trigger_parent != null)
                {
                    if (line_dragging_from_codeblock.trigger_parent.CHILD_elements_key > -1)
                    {
                        if (links.ContainsKey(line_dragging_from_codeblock.trigger_parent.CHILD_elements_key))
                        {
                            var BALLSinHD = links[line_dragging_from_codeblock.trigger_parent.CHILD_elements_key];
                            if (BALLSinHD.Count > 0)
                            {
                                potential_block p = BALLSinHD[0];
                                if (p._action != null)
                                {
                                    p._action.Inpath = null;
                                }
                                else if (p._condition != null)
                                {
                                    p._condition.Inpath = null;

                                }
                                else if (p._branch != null)
                                {
                                    p._branch.Inpath = null;
                                }
                            }
                        }
                        else
                        {
                            string debug_moment = ":(";
                        }
                    }
                    line_dragging_from_codeblock.trigger_parent.CHILD_elements_key = -1;
                }
                else if (line_dragging_from_codeblock.condition_parent != null)
                {
                    int key = line_dragging_from_codeblock.condition_parent.linked_elements_key;
                    snap_links_connection(key, balls(key)+1);
                }
                else if (line_dragging_from_codeblock.action_parent != null)
                {
                    int key = line_dragging_from_codeblock.action_parent.linked_elements_key;
                    snap_links_connection(key, balls(key)+1);
                }

                testab.Children.Remove(line_dragging_from_codeblock.Outpath);
                line_dragging_from_codeblock.Outpath = null;
            }
            if (line_dragging_from_branchblockDO != null)
            {
                if (line_dragging_from_branchblockDO.branch_parent.CHILD_elements_key > -1)
                {
                    if (links.ContainsKey(line_dragging_from_branchblockDO.branch_parent.CHILD_elements_key))
                    {
                        potential_block p = links[line_dragging_from_branchblockDO.branch_parent.CHILD_elements_key][0];
                        if (p._action != null)
                        {
                            p._action.Inpath = null;
                        }
                        else if (p._condition != null)
                        {
                            p._condition.Inpath = null;

                        }
                        else if (p._branch != null)
                        {
                            p._branch.Inpath = null;
                        }
                    }
                    else
                    {
                        string debug_moment = ":(";
                    }
                }
                line_dragging_from_branchblockDO.branch_parent.CHILD_elements_key = -1;

                testab.Children.Remove(line_dragging_from_branchblockDO.DOpath);
                line_dragging_from_branchblockDO.DOpath = null;
            }
            if (line_dragging_from_branchblockTHEN != null)
            {
                int key = line_dragging_from_branchblockTHEN.branch_parent.linked_elements_key;
                snap_links_connection(key, balls(key)+1);

                testab.Children.Remove(line_dragging_from_branchblockTHEN.THENpath);
                line_dragging_from_branchblockTHEN.THENpath = null;
            }
            line_dragging_from_codeblock = null;
            line_dragging_from_branchblockDO = null;
            line_dragging_from_branchblockTHEN = null;
        }
        public int balls(int KEY)
        {
            List<potential_block> link_to_find_index_from = links[KEY];
            for (int p_index = 0; p_index < link_to_find_index_from.Count; p_index++)
            {
                if (link_to_find_index_from[p_index]._action == line_dragging_from_codeblock && line_dragging_from_codeblock != null)
                {
                    return  p_index;
                }
                if (link_to_find_index_from[p_index]._condition == line_dragging_from_codeblock && line_dragging_from_codeblock != null)
                {
                    return p_index;
                }
                if (link_to_find_index_from[p_index]._branch == line_dragging_from_branchblockTHEN && line_dragging_from_branchblockTHEN != null)
                {
                    return p_index;
                }
            }
            return -1;
        }


        // -----------------------
        // -- META VIEWER STUFF --
        // -----------------------
        //
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string haloxml = "Halos\\" + Loaded_Gametypes[Current_Gametype].Target_Halo + ".xml";

            var the_thing = XP.READ_THE_WHOLE_FILE("/base/ExTypes", "ExTypes", haloxml);

            foreach(Ebum v in the_thing)
            {
                MV_container_block cotainer = new();
                cotainer.main = this;
                cotainer.child = v;
                cotainer.block_name.Text = v.Name;
                meta_panel.Children.Add(cotainer);
            }

        }
        static public void obama_abducted_my_children(Ebum e, StackPanel new_home_for, MainWindow main)
        {
            if (e.Type == "Int")
            {
                create_value_block(e, new_home_for, main);
            }
            if (e.Type == "UInt")
            {
                create_value_block(e, new_home_for, main);
            }
            if (e.Type == "Long")
            {
                create_value_block(e, new_home_for, main);
            }
            if (e.Type == "ULong")
            {
                create_value_block(e, new_home_for, main);
            }
            if (e.Type == "String")
            {
                create_value_block(e, new_home_for, main);
            }
            if (e.Type == "String16")
            {
                create_value_block(e, new_home_for, main);
            }
            if (e.Type == "UString8")
            {
                create_value_block(e, new_home_for, main);
            }
            if (e.Type == "UString16")
            {
                create_value_block(e, new_home_for, main);
            }
            if (e.Type == "Hex")
            {
                create_value_block(e, new_home_for, main);
            }
            if (e.Type == "Blank")
            {
                create_value_block(e, new_home_for, main);
            }

            if (e.Type.Contains("Enumref"))
            {
                // container
                create_container(e, new_home_for, main);
            }
            if (e.Type == "Enum")
            {
                // enum
                // this'll be interesting
                create_enum(e, new_home_for, main);
            }
            if (e.Type == "Container")
            {
                // container
                create_container(e, new_home_for, main);
            }
            if (e.Type == "Count")
            {
                // count
                create_count(e, new_home_for, main);
            }
            if (e.Type.Contains("External"))
            {
                // container
                create_container(e, new_home_for, main);
            }
        }
        static public void biden_abducted_my_children(Ebum e, StackPanel new_home_for, MainWindow main)
        {
            create_count_item(e, new_home_for, main);
        }
        static void create_container(Ebum data, StackPanel spot, MainWindow main)
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
        static void create_count(Ebum data, StackPanel spot, MainWindow main)
        {
            MV_count_block count = new();
            count.main = main;
            count.child = data;
            if (data.FUCKK_YOU != null)
                count.block_name.Text = data.FUCKK_YOU;
            else
                count.block_name.Text = data.Name;

            double d = Math.Pow(2.0, (double)data.Size);
            count.max_text.Text = "(" + d + ")";
            count.max = (int)d;
            count.update_count();

            spot.Children.Add(count);
        }
        static void create_value_block(Ebum data, StackPanel spot, MainWindow main)
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
        static void create_enum(Ebum data, StackPanel spot, MainWindow main)
        {
            MV_enum_block enumb = new();
            enumb.main = main;
            enumb.child = data;

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
        static void create_count_item(Ebum data, StackPanel spot, MainWindow main)
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
            string bad_fix_trim_his_BALLS = String.Join("", parent_thing.child.XMLPath.Split('/').SkipLast(1));


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
            XP.clear_xml_thingo_thanks(e);
        }
        public void write_node(Ebum e)
        {
            XP.WRITE_NODE_OF_FILE(e);
        }
    }
}
