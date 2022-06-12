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

namespace gamtetyper.UI
{
    /// <summary>
    /// Interaction logic for NodeWindow.xaml
    /// </summary>
    public partial class NodeWindow : UserControl
    {
        public NodeWindow()
        {
            InitializeComponent();
        }

        public MainWindow main;



        public List<string> WHY2(string a_xml, string path)
        {
            return main.XP.big_fella_test(a_xml, path);
        }

        public void WHY3(node_ebum_s parent_thing)
        {
            //string bad_fix_trim_his_BALLS = String.Join("", parent_thing.linkedthing.XMLPath.Split('/').SkipLast(1));
            string path = @"/base/" + parent_thing.linkedthing.XMLPath + "/Var[@name='" + parent_thing.linkedthing.V + "']";
            string path2 = parent_thing.linkedthing.XMLPath;
            string doc = parent_thing.linkedthing.XMLDoc;

            var ebums = main.XP.readchildren(main.XP.readdata(path, doc), path2, doc, new List<string> { "BALLS" });

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
            this.Focus();
            if (nodewind != null)
            {
                nodewind.is_fucking_closing = true;
                close_new_node_window();
                nodewind = null;
            }
            nodewind = new inline_new_node_thing();


            nodewind.main = this;
            nodewind.node_X = testmouseX;
            nodewind.node_Y = testmouseY;

            //if () // if target is halo reach then hide the branch option

            string haloxml2 = main.Loaded_Gametypes[main.Current_Gametype].Target_Halo;
            if (haloxml2 == "HR")
                nodewind.new_branch_butt.Visibility = Visibility.Collapsed;

            testab.Children.Add(nodewind);

            nodewind.transfrom_location.X = testmouseX; 
            nodewind.transfrom_location.Y = testmouseY; 
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
            string haloxml = main.returnmegldoc_fromhalo(main.Loaded_Gametypes[main.Current_Gametype].Target_Halo);

            Gametype.trigger trigger = new();
            trigger.Attribute = main.XP.cheat_to_do_the_node_creation("/base/ExTypes/Var[@name='MegaloScript']/Var[@name='TriggerCount']/Var[@name='Attribute']",
                                                                  "ExTypes/Var[@name='MegaloScript']/Var[@name='TriggerCount']", haloxml);
            trigger.Type = main.XP.cheat_to_do_the_node_creation("/base/ExTypes/Var[@name='MegaloScript']/Var[@name='TriggerCount']/Var[@name='Type']",
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
            string haloxml = main.returnmegldoc_fromhalo(main.Loaded_Gametypes[main.Current_Gametype].Target_Halo);


            Gametype.action action = new();
            action.Type = main.XP.cheat_to_do_the_node_creation("/base/ExTypes/Var[@name='MegaloScript']/Var[@name='ActionCount']/Var[@name='Type']",
                                                           "ExTypes/Var[@name='MegaloScript']/Var[@name='ActionCount']", haloxml);

            CodeBlock cb = new CodeBlock { VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, main_window = this };
            cb.transfrom_location.X = x;
            cb.transfrom_location.Y = y;

            //active_actions.Add(ind, new Gametype.ActionUI { UI = cb, stored_action = action });
            testab.Children.Add(cb);
            active_actions.Add(cb);
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
            string haloxml = main.returnmegldoc_fromhalo(main.Loaded_Gametypes[main.Current_Gametype].Target_Halo);

            Gametype.condition condition = new();
            condition.Not = 0;
            condition.OR_Group = 0;
            condition.insertionpoint = -1;
            condition.Type = main.XP.cheat_to_do_the_node_creation("/base/ExTypes/Var[@name='MegaloScript']/Var[@name='ConditionCount']/Var[@name='Type']",
                                                              "ExTypes/Var[@name='MegaloScript']/Var[@name='ConditionCount']", haloxml);

            CodeBlock cb = new CodeBlock { VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, main_window = this };
            cb.transfrom_location.X = x;
            cb.transfrom_location.Y = y;

            testab.Children.Add(cb);
            active_conditions.Add(cb);
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

            string haloxml = main.returnmegldoc_fromhalo(main.Loaded_Gametypes[main.Current_Gametype].Target_Halo);

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
                                nodes_list_yes_i_did_just_do_that = new List<string> { "balls" } // lol how does this even work - for some reason we're intentionally sabotaging this
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
                                nodes_list_yes_i_did_just_do_that = new List<string> { "balls" }
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
                                nodes_list_yes_i_did_just_do_that = new List<string> { "balls" }
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
                                nodes_list_yes_i_did_just_do_that = new List<string> { "balls" }
                            },
                        },
                        nodes_list_yes_i_did_just_do_that = new List<string> { "balls" }
                    }
                },
                nodes_list_yes_i_did_just_do_that = new List<string> {"balls"}
            };

            BranchBlock cb = new BranchBlock { VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, main_window = this };
            cb.transfrom_location.X = x;
            cb.transfrom_location.Y = y;

            testab.Children.Add(cb);
            active_branches.Add(cb);
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

        public void delete_codeblock(CodeBlock cb)
        {
            try
            { 
                //check&clear for trigger
                if (cb.trigger_parent != null)
                {
                    // now snap the trigger part
                    int poop2 = cb.trigger_parent.CHILD_elements_key;
                    if (poop2 != -1)
                        snap_links_connection(poop2, 0);

                    run_the_trigger_index_mover(cb, -1, "0");

                    active_triggers.Remove(cb);
                    testab.Children.Remove(cb);
                    return;
                }

                // first snap the in part
                int owning_KEY_thing = -1;
                if (cb.action_parent != null)
                    owning_KEY_thing = cb.action_parent.linked_elements_key;
                if (cb.condition_parent != null)
                    owning_KEY_thing = cb.condition_parent.linked_elements_key;
                if (owning_KEY_thing != -1)
                {
                    List<potential_block> base_link_to_find_index_from = links[owning_KEY_thing];
                    int breanpointbase = -1;
                    for (int p_index = 0; p_index < base_link_to_find_index_from.Count; p_index++)
                    {
                        var poop = base_link_to_find_index_from[p_index];
                        if (poop._action == cb || poop._condition == cb)
                        {
                            breanpointbase = p_index;
                            break;
                        }
                    }
                    snap_links_connection(owning_KEY_thing, breanpointbase);
                    // then snap the second part of it
                    owning_KEY_thing = -1;
                    if (cb.action_parent != null)
                        owning_KEY_thing = cb.action_parent.linked_elements_key;
                    if (cb.condition_parent != null)
                        owning_KEY_thing = cb.condition_parent.linked_elements_key;
                    base_link_to_find_index_from = links[owning_KEY_thing];
                    breanpointbase = -1;
                    for (int p_index = 0; p_index < base_link_to_find_index_from.Count; p_index++)
                    {
                        var poop = base_link_to_find_index_from[p_index];
                        if (poop._action == cb || poop._condition == cb)
                        {
                            breanpointbase = p_index;
                            break;
                        }
                    }
                    snap_links_connection(owning_KEY_thing, breanpointbase + 1);
                }

                testab.Children.Remove(cb);

                owning_KEY_thing = -1;
                if (cb.action_parent != null)
                {
                    owning_KEY_thing = cb.action_parent.linked_elements_key;
                    active_actions.Remove(cb);
                }
                if (cb.condition_parent != null)
                {
                    owning_KEY_thing = cb.condition_parent.linked_elements_key;
                    active_conditions.Remove(cb);
                }

                links.Remove(owning_KEY_thing);
            }
            catch (Exception ex)
            {
                MainWindow.catchexception_and_duly_ignore(ex);
            }
        }

        public void rearrange_trigger_node(CodeBlock target_trigger, int target_index)
        {
            int curr_trigger_index = run_the_trigger_index_mover(target_trigger, -1, "-1");
            active_triggers.RemoveAt(curr_trigger_index);

            int capped_target_index = target_index;
            if (capped_target_index < 0)
                capped_target_index = 0;

            if (capped_target_index > active_triggers.Count)
            {
                capped_target_index = active_triggers.Count;
                active_triggers.Add(target_trigger);
            }
            else
            {
                active_triggers.Insert(target_index, target_trigger);
            }
            run_the_trigger_index_mover(target_trigger, 1, (capped_target_index + 1).ToString());

            for (int index = 0; index < active_actions.Count; index++)
            {
                var action = active_actions[index];
                if (action.action_parent.stored_action.Type.V == "Megl.RunTrigger") // only do this for run trig nodes
                {
                    int test_their_index = int.Parse(action.action_parent.stored_action.Type.Params[0].V);
                    if (test_their_index == -1)
                    {
                        action.action_parent.stored_action.Type.Params[0].V = capped_target_index.ToString();
                        var benis = action.Content_panel.Children[0] as node_ebum_s;
                        var benis2 = benis.child_panel.Children[0] as node_ebum_s;
                        benis2.source_text.Content = capped_target_index.ToString();
                    }
                }
            }
            target_trigger.typename.Content = "Trigger" + capped_target_index;
        }

        public int run_the_trigger_index_mover(CodeBlock cb, int either_1_or_neg1, string new_value_for_deleted_refs)
        {
            int found_thing_now_engaging = -1;
            //int index_to_Remove = -1;
            for (int index = 0; index < active_triggers.Count; index++)
            {
                var trigger = active_triggers[index];
                if (found_thing_now_engaging != -1)
                {
                    
                    if (either_1_or_neg1 == 1)
                    {
                        trigger.typename.Content = "Trigger" + index;
                    }
                    else
                    {
                        trigger.typename.Content = "Trigger" + (index + either_1_or_neg1);

                    }
                }
                else
                {
                    if (cb == trigger) // why did we do it this wayyyy
                    {
                        found_thing_now_engaging = index;
                        continue;
                    }
                }
            }
            for (int index = 0; index < active_actions.Count; index++)
            {
                var action = active_actions[index];
                if (action.action_parent.stored_action.Type.V == "Megl.RunTrigger") // only do this for run trig nodes
                {
                    int test_their_index = int.Parse(action.action_parent.stored_action.Type.Params[0].V);
                    if (test_their_index == found_thing_now_engaging)
                    {
                        action.action_parent.stored_action.Type.Params[0].V = new_value_for_deleted_refs;
                        var benis = action.Content_panel.Children[0] as node_ebum_s;
                        var benis2 = benis.child_panel.Children[0] as node_ebum_s;
                        benis2.source_text.Content = new_value_for_deleted_refs;
                    }
                    if (test_their_index > found_thing_now_engaging)
                    {
                        action.action_parent.stored_action.Type.Params[0].V = (test_their_index + either_1_or_neg1).ToString();
                        var benis = action.Content_panel.Children[0] as node_ebum_s;
                        var benis2 = benis.child_panel.Children[0] as node_ebum_s;
                        benis2.source_text.Content = action.action_parent.stored_action.Type.Params[0].V;

                    }

                }
            }
            return found_thing_now_engaging;
        }

        public void delete_branch(BranchBlock bb)
        {
            try
            { 
                // first snap the in part
                int owning_KEY_thing = bb.branch_parent.linked_elements_key;
                List<potential_block> base_link_to_find_index_from = links[owning_KEY_thing];
                int breanpointbase = -1;
                for (int p_index = 0; p_index < base_link_to_find_index_from.Count; p_index++)
                {
                    var poop = base_link_to_find_index_from[p_index];
                    if (poop._branch == bb)
                    {
                        breanpointbase = p_index;
                        break;
                    }
                }
                snap_links_connection(owning_KEY_thing, breanpointbase);
                // then snap the second part of it
                owning_KEY_thing = bb.branch_parent.linked_elements_key;
                base_link_to_find_index_from = links[owning_KEY_thing];
                breanpointbase = -1;
                for (int p_index = 0; p_index < base_link_to_find_index_from.Count; p_index++)
                {
                    var poop = base_link_to_find_index_from[p_index];
                    if (poop._branch == bb)
                    {
                        breanpointbase = p_index;
                        break;
                    }
                }
                snap_links_connection(owning_KEY_thing, breanpointbase + 1);

                // now snap the trigger part
                int poop2 = bb.branch_parent.CHILD_elements_key;
                if (poop2 != -1)
                    snap_links_connection(poop2, 0);

                active_branches.Remove(bb);
                testab.Children.Remove(bb);
                owning_KEY_thing = bb.branch_parent.linked_elements_key;
                links.Remove(owning_KEY_thing);

            }
            catch (Exception ex)
            {
                MainWindow.catchexception_and_duly_ignore(ex);
            }
        }


        public List<CodeBlock> active_triggers = new();
        // im *pretty sure* that we only use the number for converting to nodes
        public List<CodeBlock> active_conditions = new();

        public List<CodeBlock> active_actions = new();

        public List<BranchBlock> active_branches = new();

        public Dictionary<int, CodeBlock> temp_active_conditions = new();
        public Dictionary<int, CodeBlock> temp_active_actions = new();
        public Dictionary<int, BranchBlock> temp_active_branches = new();

        List<Gametype.trigger> t;
        List<Gametype.action> a;
        List<Gametype.condition> c;

        public int height = 0; // hopefully this should fix it
        public void dothething()
        {
            active_triggers.Clear();

            temp_active_conditions.Clear();
            temp_active_actions.Clear();
            temp_active_branches.Clear();

            height = 0;
            links.Clear();
            trigger_links_index = 0;
            testab.Children.Clear();

            // bla lab la return struct data from xml

            string haloxml2 = main.Loaded_Gametypes[main.Current_Gametype].Target_Halo;
            string haloxml = main.returnmegldoc_fromhalo(haloxml2);
            bool is_Reach = false;
            if (haloxml2 == "HR")
                is_Reach = true;

            t = main.XP.returnScripts(is_Reach, haloxml);
            a = main.XP.doactions(haloxml);
            c = main.XP.doconditions(haloxml);

            for (int ind = 0; ind < a.Count; ind++)  //   Gametype.action
            {
                Gametype.action action = a[ind];
                //, Margin = new Thickness(400*ind, -300, 0, 0) 
                if (action.Type.V != "Virtual Trigger")
                {
                    CodeBlock cb = new CodeBlock { VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, main_window = this };
                    if (action.position.has_position)
                    {
                        cb.transfrom_location.X = action.position.x;
                        cb.transfrom_location.Y = action.position.y;
                    }
                    else
                    {
                        cb.transfrom_location.X = 400 * ind;
                        cb.transfrom_location.Y = -700;
                    }

                    //active_actions.Add(ind, new Gametype.ActionUI { UI = cb, stored_action = action });
                    testab.Children.Add(cb);
                    temp_active_actions.Add(ind, cb);
                    cb.action_parent = new Gametype.ActionUI { UI = cb, stored_action = action };

                    cb.typename.Content = "Action";

                    handleType(cb, cb.Content_panel, action.Type);

                    cb.head.Background = new SolidColorBrush(Color.FromArgb(255, 214, 0, 0));
                }
                else
                {
                    BranchBlock cb = new BranchBlock { VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, main_window = this };
                    if (action.position.has_position)
                    {
                        cb.transfrom_location.X = action.position.x;
                        cb.transfrom_location.Y = action.position.y;
                    }
                    else
                    {
                        cb.transfrom_location.X = 400 * ind;
                        cb.transfrom_location.Y = -700;
                    }

                    testab.Children.Add(cb);
                    temp_active_branches.Add(ind, cb);
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
                if (condition.position.has_position)
                {
                    cb.transfrom_location.X = condition.position.x;
                    cb.transfrom_location.Y = condition.position.y;
                }
                else
                {
                    cb.transfrom_location.X = 400 * ind;
                    cb.transfrom_location.Y = -700;
                }

                testab.Children.Add(cb);
                temp_active_conditions.Add(ind, cb);
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
                if (trigger.position.has_position)
                {
                    cb.transfrom_location.X = trigger.position.x;
                    cb.transfrom_location.Y = trigger.position.y;
                }
                else
                {
                    cb.transfrom_location.X = 0;
                    cb.transfrom_location.Y = height;
                }
                


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

            active_conditions.Clear();
            active_actions.Clear();
            active_branches.Clear();

            foreach (var v in temp_active_conditions)
            {
                active_conditions.Add(v.Value);
            }
            foreach (var v in temp_active_actions)
            {
                active_actions.Add(v.Value);
            }
            foreach (var v in temp_active_branches)
            {
                active_branches.Add(v.Value);
            }
            temp_active_conditions.Clear();
            temp_active_actions.Clear();
            temp_active_branches.Clear();
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
                    Gametype.action action = temp_active_actions[index_for_action].action_parent.stored_action;

                    CodeBlock A = temp_active_actions[index_for_action];
                    A.action_parent.linked_elements_key = trigger_links_index;

                    NODELINEUP.Add(new nodelineupthing { g_action = action, g_cb = A.action_parent.UI });

                    linkedstuff.Add(new potential_block { _action = A });

                }
                else
                {
                    Gametype.action action = temp_active_branches[index_for_action].branch_parent.stored_action;

                    BranchBlock B = temp_active_branches[index_for_action];
                    B.branch_parent.linked_elements_key = trigger_links_index;

                    NODELINEUP.Add(new nodelineupthing { g_action = action, g_bb = B.branch_parent.UI });

                    linkedstuff.Add(new potential_block { _branch = B });
                }

            }

            List<int> insertion_points = new List<int>();

            for (int w = 0; w < trigger.Conditions_count; w++)  // LINE UP EVERY CONDITION ELEMENT
            {
                int index_for_condition = w + trigger.Conditions_insert;


                CodeBlock C = temp_active_conditions[index_for_condition];
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
                if (factored_insertion > NODELINEUP.Count)
                {
                    NODELINEUP.Add(new nodelineupthing { g_cb = C.condition_parent.UI });
                    linkedstuff.Add(new potential_block { _condition = C });
                }
                else
                {
                    NODELINEUP.Insert(factored_insertion, new nodelineupthing { g_cb = C.condition_parent.UI });
                    linkedstuff.Insert(factored_insertion, new potential_block { _condition = C });
                }
            }

            CodeBlock prev_to_link = caller;
            BranchBlock prev_to_link_branch = altcaller;


            foreach (nodelineupthing kv in NODELINEUP)
            {
                if (kv.g_bb == null)
                { // normal code block
                    longitude += 220;

                    bool skip_autoplace = false;
                    if (kv.g_cb.condition_parent != null)
                    {
                        skip_autoplace = kv.g_cb.condition_parent.stored_condition.position.has_position == true;
                    }
                    else if (kv.g_cb.action_parent != null)
                    {
                        skip_autoplace = kv.g_cb.action_parent.stored_action.position.has_position == true;
                    }
                    



                    if (!skip_autoplace)
                    {
                        kv.g_cb.transfrom_location.X = longitude;
                        kv.g_cb.transfrom_location.Y = height;
                    }

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
                    if (kv.g_bb.branch_parent.stored_action.position.has_position != true)
                    {
                        kv.g_bb.transfrom_location.X = longitude;
                        kv.g_bb.transfrom_location.Y = height;
                    }

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

        // ----------------------
        // -- NODEGRAPH SAVING --
        // ----------------------
        //
        List<Gametype.trigger> export_triggs = new();
        List<Gametype.action> export_actions = new();
        List<Gametype.condition> export_condis = new();

        Dictionary<int, trigger_things> mapped_trig_groups = new();

        private void TheGrid_OnLoaded(object sender, RoutedEventArgs e)
        {
            parent_nodegraph.Focus();
        }


        public void select_all()
        {
            foreach (CodeBlock cb in active_triggers)
            {
                cb.is_grabbed = true;
                cb.status_border.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 220, 173, 14));
            }
            foreach (CodeBlock cb in active_conditions)
            {
             cb.is_grabbed = true;
             cb.status_border.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 220, 173, 14));
            }
            foreach (CodeBlock cb in active_actions)
            {
                cb.is_grabbed = true;
                cb.status_border.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 220, 173, 14));
            }            
            foreach (BranchBlock cb in active_branches)
            {
             cb.is_grabbed = true;
             cb.status_border.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 220, 173, 14));
            }
        }

        public void parent_nodegraph_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control)
            {



                if (e.Key == Key.C)
                {
                    copy_selection();
                }
                if (e.Key == Key.V)
                {
                    paste_selection();
                }
                if (e.Key == Key.A)
                {
                    select_all();
                }
            }
            else if (e.Key == Key.Delete)
            {
                for (int i=0; i<active_triggers.Count; i++)
                {
                    CodeBlock cb = active_triggers[i];
                    if (cb.is_grabbed)
                    {
                        delete_codeblock(cb);
                        i--;
                    }
                }
                for (int i=0; i < active_actions.Count; i++)
                {
                    CodeBlock cb = active_actions[i];
                    if (cb.is_grabbed)
                    {
                        delete_codeblock(cb);
                        i--;
                    }
                }
                for (int i=0; i < active_conditions.Count; i++)
                {
                    CodeBlock cb = active_conditions[i];
                    if (cb.is_grabbed)
                    {
                        delete_codeblock(cb);
                        i--;
                    }
                }
                for (int i=0; i < active_branches.Count; i++)
                {
                    BranchBlock cb = active_branches[i];
                    if (cb.is_grabbed)
                    {
                        delete_branch(cb);
                        i--;
                    }
                }
            }
        }
        public void copy_selection()
        {
            try
            { 
                export_triggs.Clear();
                export_actions.Clear();
                export_condis.Clear();
                mapped_trig_groups.Clear();

                string haloxml2 = main.Loaded_Gametypes[main.Current_Gametype].Target_Halo;
                bool is_Reach = false;
                if (haloxml2 == "HR")
                    is_Reach = true;

                int things_to_copy = 0;

                bool has_found_location_bias = false;
                double pos_X = 0;
                double pos_Y = 0;
                foreach (var v in active_triggers) // triggers
                {
                    if (v.is_grabbed)
                    {
                        if (!has_found_location_bias)
                        {
                            pos_X = v.transfrom_location.X;
                            pos_Y = v.transfrom_location.Y;
                            has_found_location_bias = true;
                        }
                        var m = v.trigger_parent.stored_trigger;
                        m.position.has_position = true;
                        m.position.x = pos_X - v.transfrom_location.X;
                        m.position.y = pos_Y - v.transfrom_location.Y;
                        export_triggs.Add(m);
                        things_to_copy++;
                    }
                }
                foreach (var v in active_actions) // actions
                {
                    if (v.is_grabbed)
                    {
                        if (!has_found_location_bias)
                        {
                            pos_X = v.transfrom_location.X;
                            pos_Y = v.transfrom_location.Y;
                            has_found_location_bias = true;
                        }
                        var m = v.action_parent.stored_action;
                        m.position.has_position = true;
                        m.position.x = pos_X - v.transfrom_location.X;
                        m.position.y = pos_Y - v.transfrom_location.Y;
                        export_actions.Add(m);
                        things_to_copy++;
                    }
                }
                foreach (var v in active_branches) // branch actions
                {
                    if (v.is_grabbed)
                    {
                        if (!has_found_location_bias)
                        {
                            pos_X = v.transfrom_location.X;
                            pos_Y = v.transfrom_location.Y;
                            has_found_location_bias = true;
                        }
                        var m = v.branch_parent.stored_action;
                        m.position.has_position = true;
                        m.position.x = pos_X - v.transfrom_location.X;
                        m.position.y = pos_Y - v.transfrom_location.Y;
                        export_actions.Add(m);
                        things_to_copy++;
                    }
                }
                foreach (var v in active_conditions) // conditions
                {
                    if (v.is_grabbed)
                    {
                        if (!has_found_location_bias)
                        {
                            pos_X = v.transfrom_location.X;
                            pos_Y = v.transfrom_location.Y;
                            has_found_location_bias = true;
                        }
                        var m = v.condition_parent.stored_condition;
                        m.position.has_position = true;
                        m.position.x = pos_X - v.transfrom_location.X;
                        m.position.y = pos_Y - v.transfrom_location.Y;
                        export_condis.Add(m);
                        things_to_copy++;
                    }
                }
                if (things_to_copy > 0)
                {
                    var backup_our_open_xml = main.XP.XMLdump;
                    main.XP.create_virtual_xdoc();

                    main.XP.exportScripts(export_triggs, is_Reach);
                    main.XP.exportactions(export_actions);
                    main.XP.exportconditions(export_condis);

                    Clipboard.SetText(":::"+main.XP.XMLdump.OuterXml);

                    main.XP.XMLdump = backup_our_open_xml;

                    main.PostConsole("Selection ("
                                     +export_triggs.Count+" triggers "
                                     +export_condis.Count+" conditions "
                                     +export_actions.Count+ " actions) copied successfully",
                                "Nodes Copied", "white", false);
                }
            }
            catch (Exception ex)
            {
                MainWindow.catchexception_and_duly_ignore(ex);
            }
        }
        public void paste_selection()
        {
            try
            { 
                string test_paste = Clipboard.GetText();
                string check_header = test_paste.Substring(0, 3);
                if (check_header == ":::")
                {
                    string outter_xml = test_paste.Substring(3);

                    var backup_our_open_xml = main.XP.XMLdump;

                    main.XP.create_blank_virtual_xdoc_but_then_paste_a_string_in_it(outter_xml);



                 
                    string haloxml2 = main.Loaded_Gametypes[main.Current_Gametype].Target_Halo;
                    string haloxml = main.returnmegldoc_fromhalo(haloxml2);
                    bool is_Reach = false;
                    if (haloxml2 == "HR")
                        is_Reach = true;

                    t = main.XP.returnScripts(is_Reach, haloxml);
                    a = main.XP.doactions(haloxml);
                    c = main.XP.doconditions(haloxml);

                    for (int ind = 0; ind < a.Count; ind++)  //   Gametype.action
                    {
                        Gametype.action action = a[ind];
                        //, Margin = new Thickness(400*ind, -300, 0, 0) 
                        if (action.Type.V != "Virtual Trigger")
                        {
                            CodeBlock cb = new CodeBlock { VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, main_window = this };
                            if (action.position.has_position)
                            {
                                cb.transfrom_location.X = testmouseX - action.position.x;
                                cb.transfrom_location.Y = testmouseY - action.position.y;
                            }
                            else
                            {
                                cb.transfrom_location.X = testmouseX;
                                cb.transfrom_location.Y = testmouseY;
                            }

                            //active_actions.Add(ind, new Gametype.ActionUI { UI = cb, stored_action = action });
                            testab.Children.Add(cb);
                            active_actions.Add(cb);
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
                        else
                        {
                            BranchBlock cb = new BranchBlock { VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, main_window = this };
                            if (action.position.has_position)
                            {
                                cb.transfrom_location.X = testmouseX - action.position.x;
                                cb.transfrom_location.Y = testmouseY - action.position.y;
                            }
                            else
                            {
                                cb.transfrom_location.X = testmouseX;
                                cb.transfrom_location.Y = testmouseY;
                            }

                            testab.Children.Add(cb);
                            active_branches.Add(cb);
                            cb.branch_parent = new Gametype.BranchUI
                            {
                                UI = cb,
                                stored_action = action
                            };

                            int test_this_mofo = links.Count;
                            while (links.ContainsKey(test_this_mofo))
                            {
                                test_this_mofo++;
                            }
                            List<potential_block> linkedstuff = new();
                            linkedstuff.Add(new potential_block { _branch = cb });
                            links.Add(test_this_mofo, linkedstuff);
                            cb.branch_parent.linked_elements_key = test_this_mofo;

                            cb.branch_parent.CHILD_elements_key = -1;
                        }
                    }
                    for (int ind = 0; ind < c.Count; ind++)  //   Gametype.condition
                    {
                        Gametype.condition condition = c[ind];
                        //, Margin = new Thickness(400 * ind, 300, 0, 0)
                        CodeBlock cb = new CodeBlock { VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, main_window = this };
                        if (condition.position.has_position)
                        {
                            cb.transfrom_location.X = testmouseX - condition.position.x;
                            cb.transfrom_location.Y = testmouseY - condition.position.y;
                        }
                        else
                        {
                            cb.transfrom_location.X = testmouseX;
                            cb.transfrom_location.Y = testmouseY;
                        }

                        testab.Children.Add(cb);
                        active_conditions.Add(cb);
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
                    foreach (Gametype.trigger trigger in t)  //   Gametype.trigger
                    {
                        CodeBlock cb = new CodeBlock { VerticalAlignment = VerticalAlignment.Center, HorizontalAlignment = HorizontalAlignment.Center, main_window = this };
                        if (trigger.position.has_position)
                        {
                            cb.transfrom_location.X = testmouseX - trigger.position.x;
                            cb.transfrom_location.Y = testmouseY - trigger.position.y;
                        }
                        else
                        {
                            cb.transfrom_location.X = testmouseX;
                            cb.transfrom_location.Y = testmouseY;
                        }


                        testab.Children.Add(cb);
                        cb.main.Children.Remove(cb.in_connection);

                        //active_triggers.Add(new Gametype.TriggerUI { UI = cb, stored_trigger = trigger });
                        cb.trigger_parent = new Gametype.TriggerUI { UI = cb, stored_trigger = trigger };

                        cb.typename.Content = "Trigger" + active_triggers.Count;
                        active_triggers.Add(cb);


                        handleType(cb, cb.Content_panel, trigger.Type);

                        handleType(cb, cb.Content_panel, trigger.Attribute);

                        cb.head.Background = new SolidColorBrush(Color.FromArgb(255, 212, 117, 0));

                        cb.trigger_parent.CHILD_elements_key = -1;
                        //run_the_loop_on_mf(trigger, longitude, cb, null);
                    }



                    main.XP.XMLdump = backup_our_open_xml;

                    main.PostConsole("Selection ("
                                     + a.Count + " triggers "
                                     + c.Count + " conditions "
                                     + t.Count + " actions) pasted successfully",
                                "Nodes Copied", "white", false);
                }
            }
            catch (Exception ex)
            {
                MainWindow.catchexception_and_duly_ignore(ex);
            }
        }

        public void SaveButton_Click()
        {
            try
            { 
                export_triggs.Clear();
                export_actions.Clear();
                export_condis.Clear();

                mapped_trig_groups.Clear();

                string haloxml2 = main.Loaded_Gametypes[main.Current_Gametype].Target_Halo;
                bool is_Reach = false;
                if (haloxml2 == "HR")
                    is_Reach = true;

                foreach (var v in active_actions)
                {
                    var par = v.action_parent.stored_action;
                    par.position.has_position = true;
                    par.position.x = v.transfrom_location.X;
                    par.position.y = v.transfrom_location.Y;

                    v.action_parent.stored_action = par;
                }
                foreach (var v in active_conditions)
                {
                    var par = v.condition_parent.stored_condition;
                    par.position.has_position = true;
                    par.position.x = v.transfrom_location.X;
                    par.position.y = v.transfrom_location.Y;

                    v.condition_parent.stored_condition = par;
                }
                foreach (var v in active_branches)
                {
                    var par = v.branch_parent.stored_action; 
                    par.position.has_position = true;
                    par.position.x = v.transfrom_location.X;
                    par.position.y = v.transfrom_location.Y;

                    v.branch_parent.stored_action = par;
                }

                foreach (var v in active_triggers)
                {
                    Gametype.trigger outthis = v.trigger_parent.stored_trigger;

                    int poop = v.trigger_parent.CHILD_elements_key;
                    trigger_things t = read_trigger_thing(poop, is_Reach);
                    outthis.Conditions_count = t.cond_count;
                    outthis.Conditions_insert = t.cond_offset;
                    outthis.Actions_count = t.acti_count;
                    outthis.Actions_insert = t.acti_offset;

                    outthis.position.has_position = true;
                    outthis.position.x = v.transfrom_location.X;
                    outthis.position.y = v.transfrom_location.Y;

                    export_triggs.Add(outthis);
                }
                main.XP.exportScripts(export_triggs, is_Reach);
                main.XP.exportactions(export_actions);
                main.XP.exportconditions(export_condis);
                main.XP.quick_save_the_xmls();
            }
            catch (Exception ex)
            {
                MainWindow.catchexception_and_duly_ignore(ex);
            }
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

        // ----------------------------------
        // -- RUNTIME NODEGRAPH MANAGEMENT --
        // ----------------------------------
        //

        public void snap_links_connection(int link, int breakpoint, int KEY_to_inherit = -1)
        {
            try
                {
                List<potential_block> link_to_break = links[link];

                // if breakpoint is 0 and key_to_inherit isnt -1 then we wanna take it off a trigger and put it on an action
                if (breakpoint == 0) // && KEY_to_inherit != -1
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
                    foreach (BranchBlock t in active_branches)
                    {
                        if (t.branch_parent.CHILD_elements_key == link)
                        {
                            testab.Children.Remove(t.DOpath);
                            t.branch_parent.CHILD_elements_key = -1;
                            t.DOpath = null;
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
                    var bigw1 = bigw[bigw.Count - 1];
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
                if (new_blocklist.Count > 0) // KEY_to_inherit == -1 &&
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
            catch (Exception ex)
            {
                MainWindow.catchexception_and_duly_ignore(ex);
            }
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
                    X2 = testmouseX,
                    Y2 = testmouseY
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
                        snap_links_connection(owning_KEY_thing, breanpointbase + 1);

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
                    X2 = testmouseX,
                    Y2 = testmouseY
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
                    X2 = testmouseX,
                    Y2 = testmouseY
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

        public double mouseX;
        public double mouseY;

        public double testmouseX;
        public double testmouseY;

        public double zoom = 1;

        public bool nodegraph_grabbed;
        public bool graph_in_lod_state;


        private void testab_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            this.Focus();
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                nodegraph_grabbed = true;
                move_mouse2(0, (float)e.Delta * -1.1);
                nodegraph_grabbed = false;
            }
            else if (Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightShift))
            {
                nodegraph_grabbed = true;
                move_mouse2((float)e.Delta * -1.1, 0);
                nodegraph_grabbed = false;
            }
            else
            {
                zoom += ((float)e.Delta) / 750 * zoom;
                if (zoom > 2) zoom = 2;
                else if (zoom < 0.025f) zoom = 0.025f;
                nodegraph_scale.ScaleX = zoom;
                nodegraph_scale.ScaleY = zoom;

                testab.RenderTransformOrigin = new((1f / (testab.ActualWidth)) * -nodegraph_trans.X + 0.5f,
                                                   (1f / (testab.ActualHeight)) * -nodegraph_trans.Y + 0.5f);

                //DEBUG_LOCAITON.X = RenderTransformOrigin.X * testab.ActualWidth;
                //DEBUG_LOCAITON.Y = RenderTransformOrigin.Y * testab.ActualHeight;

                if (!graph_in_lod_state && zoom <= 0.2f)
                {
                    graph_in_lod_state = true;
                    foreach (var v in active_triggers)
                    {
                        v.Content_panel.Visibility = Visibility.Hidden;
                    }
                    foreach (var v in active_conditions)
                    {
                        v.Content_panel.Visibility = Visibility.Hidden;
                    }
                    foreach (var v in active_actions)
                    {
                        v.Content_panel.Visibility = Visibility.Hidden;
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
                        v.Content_panel.Visibility = Visibility.Visible;
                    }
                    foreach (var v in active_actions)
                    {
                        v.Content_panel.Visibility = Visibility.Visible;
                    }
                }

            }
        }
        public void recieved_MouseMove(MouseEventArgs e)
        {
            double DeltaX = e.GetPosition(parent_nodegraph).X;
            double DeltaY = e.GetPosition(parent_nodegraph).Y;
            testmouseX = e.GetPosition(testab).X;
            testmouseY = e.GetPosition(testab).Y;
            move_mouse(DeltaX, DeltaY);
        }

        public void reset_movement()
        {
            //nodegraph_grabbed = true;
            //move_mouse(0, 0);
            //nodegraph_grabbed = false;
            nodegraph_trans.X = 0;
            nodegraph_trans.Y = 0;

            double tab_x = ((1f / (testab.ActualWidth)) * -nodegraph_trans.X) + 0.5f;
            double tab_y = ((1f / (testab.ActualHeight)) * -nodegraph_trans.Y) + 0.5f;
            testab.RenderTransformOrigin = new(tab_x, tab_y);

        }
        public void move_mouse(double mX, double mY)
        {
            if (testab.ActualWidth <= 0 || testab.ActualHeight <= 0)
                return;

            double DeltaX = mouseX - mX;
            double DeltaY = mouseY - mY;
            move_mouse2(DeltaX, DeltaY);
            mouseX = mX;
            mouseY = mY;
        }
        public void move_mouse2(double DeltaX, double DeltaY)
        {
            double factored_X = DeltaX / nodegraph_scale.ScaleX;
            double factored_Y = DeltaY / nodegraph_scale.ScaleY;

            // 
            if (isdragging_nodes)
            {
                foreach (CodeBlock cb in active_triggers)
                {
                    if (cb.is_grabbed)
                    {
                        drag_block(cb, factored_X, factored_Y);
                    }
                }
                foreach (CodeBlock cb in active_actions)
                {
                    if (cb.is_grabbed)
                    {
                        drag_block(cb, factored_X, factored_Y);
                    }
                }
                foreach (CodeBlock cb in active_conditions)
                {
                    if (cb.is_grabbed)
                    {
                        drag_block(cb, factored_X, factored_Y);
                    }
                }
                foreach (BranchBlock cb in active_branches)
                {
                    if (cb.is_grabbed)
                    {
                        drag_retarded_block(cb, factored_X, factored_Y);
                    }
                }
            }

            if (nodegraph_grabbed)
            {
                //testab.
                //nodegraph_scale
                nodegraph_trans.X -= factored_X;
                nodegraph_trans.Y -= factored_Y;

                double tab_x = ((1f / (testab.ActualWidth)) * -nodegraph_trans.X) + 0.5f;
                double tab_y = ((1f / (testab.ActualHeight)) * -nodegraph_trans.Y) + 0.5f;
                testab.RenderTransformOrigin = new(tab_x, tab_y);
            }

            // move lines that are currently held
            if (line_dragging_from_branchblockTHEN != null)
            {
                line_dragging_from_branchblockTHEN.THENpath.X2 = testmouseX;
                line_dragging_from_branchblockTHEN.THENpath.Y2 = testmouseY;
            }
            if (line_dragging_from_branchblockDO != null)
            {
                line_dragging_from_branchblockDO.DOpath.X2 = testmouseX;
                line_dragging_from_branchblockDO.DOpath.Y2 = testmouseY;
            }
            if (line_dragging_from_codeblock != null)
            {
                line_dragging_from_codeblock.Outpath.X2 = testmouseX;
                line_dragging_from_codeblock.Outpath.Y2 = testmouseY;
            }

            if (selection_box != null)
            {
                double testdeltaX = selectstartX - testmouseX;
                double testdeltaY = selectstartY - testmouseY;





                if (testdeltaX < 0)
                {
                    selection_box.transfrom_location.X = testmouseX + testdeltaX;
                    testdeltaX *= -1;
                }
                else
                {
                    selection_box.transfrom_location.X = testmouseX; // - testdeltaX/2;
                }
                if (testdeltaY < 0)
                {
                    selection_box.transfrom_location.Y = testmouseY + testdeltaY;
                    testdeltaY *= -1;
                }
                else
                {
                    selection_box.transfrom_location.Y = testmouseY; // - testdeltaY/2;
                }

                selection_box.border.Width = testdeltaX;
                selection_box.border.Height = testdeltaY;

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

        public selectionBorder selection_box;
        double selectstartX;
        double selectstartY;

        bool isdragging_nodes;
        //bool any_nodes_selected;
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Focus();
            // before we spawn in our selecty square, lets remove the creation box
            if (nodewind != null)
            {
                nodewind.is_fucking_closing = true;
                close_new_node_window();
                nodewind = null;
            }
            // ugh, make sure we cleanup the last one incase someone breaks it
            if (selection_box != null)
            {
                testab.Children.Remove(selection_box);
                selection_box = null;
            }
            //ok so lets create a square to start selecting things

            selection_box = new selectionBorder();
            selection_box.border.Width = 4;
            selection_box.border.Height = 4;

            selectstartX = testmouseX;
            selectstartY = testmouseY;

            selection_box.transfrom_location.X = testmouseX;
            selection_box.transfrom_location.Y = testmouseY;



            testab.Children.Add(selection_box);

        }
        public void Window_MouseLeftButtonUp()
        {
            isdragging_nodes = false;
            if (selection_box != null)
            {
                testab.Children.Remove(selection_box);
                selection_box = null;

                foreach (CodeBlock cb in active_triggers)
                {
                    if (cb.is_grabbed)
                    {
                        cb.is_grabbed = false;
                        cb.status_border.BorderBrush = Brushes.White;
                    }
                    if (is_code_block_in_selection(cb.transfrom_location.X, cb.transfrom_location.Y))
                    {
                        cb.is_grabbed = true;
                        cb.status_border.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 220, 173, 14));
                    }
                }
                foreach (CodeBlock cb in active_actions)
                {
                    if (cb.is_grabbed)
                    {
                        cb.is_grabbed = false;
                        cb.status_border.BorderBrush = Brushes.White;
                    }
                    if (is_code_block_in_selection(cb.transfrom_location.X, cb.transfrom_location.Y))
                    {
                        cb.is_grabbed = true;
                        cb.status_border.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 220, 173, 14));
                    }
                }
                foreach (CodeBlock cb in active_conditions)
                {
                    if (cb.is_grabbed)
                    {
                        cb.is_grabbed = false;
                        cb.status_border.BorderBrush = Brushes.White;
                    }
                    if (is_code_block_in_selection(cb.transfrom_location.X, cb.transfrom_location.Y))
                    {
                        cb.is_grabbed = true;
                        cb.status_border.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 220, 173, 14));
                    }
                }
                foreach (BranchBlock cb in active_branches)
                {
                    if (cb.is_grabbed)
                    {
                        cb.is_grabbed = false;
                        cb.status_border.BorderBrush = Brushes.White;
                    }
                    if (is_code_block_in_selection(cb.transfrom_location.X, cb.transfrom_location.Y))
                    {
                        cb.is_grabbed = true;
                        cb.status_border.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 220, 173, 14));
                    }
                }
            }
            else
            {
                if (count_grabbed_nodes() == 1)
                {
                    foreach (CodeBlock cb in active_triggers)
                    {
                        if (cb.is_grabbed)
                        {
                            cb.is_grabbed = false;
                            cb.status_border.BorderBrush = Brushes.White;
                        }
                    }
                    foreach (CodeBlock cb in active_actions)
                    {
                        if (cb.is_grabbed)
                        {
                            cb.is_grabbed = false;
                            cb.status_border.BorderBrush = Brushes.White;
                        }
                    }
                    foreach (CodeBlock cb in active_conditions)
                    {
                        if (cb.is_grabbed)
                        {
                            cb.is_grabbed = false;
                            cb.status_border.BorderBrush = Brushes.White;
                        }
                    }
                    foreach (BranchBlock cb in active_branches)
                    {
                        if (cb.is_grabbed)
                        {
                            cb.is_grabbed = false;
                            cb.status_border.BorderBrush = Brushes.White;
                        }
                    }
                }
            }

            // clear dragged lines n junk
            wait_a_second_and_then_solve_our_problem();


        }

        public bool is_code_block_in_selection(double c_x, double c_y)
        {
            if (selectstartX > testmouseX)
            {
                if (c_x > selectstartX || c_x < testmouseX)
                {
                    return false;
                }
            }
            else
            {
                if (c_x < selectstartX || c_x > testmouseX)
                {
                    return false;
                }
            }
            if (selectstartY > testmouseY)
            {
                if (c_y > selectstartY || c_y < testmouseY)
                {
                    return false;
                }
            }
            else
            {
                if (c_y < selectstartY || c_y > testmouseY)
                {
                    return false;
                }
            }
            return true;
        }

        // NODEGRAPH NAVIGATION STUFF
        private void parent_nodegraph_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle && e.ButtonState == MouseButtonState.Pressed)
            {
                nodegraph_grabbed = true;
                this.Focus();
            }
        }
        private void parent_nodegraph_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Middle && e.ButtonState == MouseButtonState.Released)
            {
                nodegraph_grabbed = false;
                //drop all nodes too
            }
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
                    snap_links_connection(key, balls(key) + 1);
                }
                else if (line_dragging_from_codeblock.action_parent != null)
                {
                    int key = line_dragging_from_codeblock.action_parent.linked_elements_key;
                    snap_links_connection(key, balls(key) + 1);
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
                snap_links_connection(key, balls(key) + 1);

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
                    return p_index;
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

        public void branchclick(BranchBlock bb)
        {
            bb.is_grabbed = true;
            bb.status_border.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 220, 173, 14));

            isdragging_nodes = true;
            // start moving all grabbed
        }
        public void branchrelease(BranchBlock bb)
        {
            //bb.is_grabbed = false;
            //bb.status_border.BorderBrush = Brushes.White;

            Window_MouseLeftButtonUp();
        }

        public void codeblockclick(CodeBlock cb)
        {
            if (nodewind != null)
            {
                nodewind.is_fucking_closing = true;
                close_new_node_window();
                nodewind = null;
            }

            cb.is_grabbed = true;
            cb.status_border.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 220, 173, 14));

            isdragging_nodes = true;
            // start moving all grabbed
        }
        public void codeblockrelease(CodeBlock cb)
        {
            //cb.is_grabbed = false;
            //cb.status_border.BorderBrush = Brushes.White;

            Window_MouseLeftButtonUp();
        }

        private void parent_nodegraph_MouseLeave(object sender, MouseEventArgs e)
        {
            // cleanup rectangle thingo
            Window_MouseLeftButtonUp();
        }

        public int count_grabbed_nodes()
        {
            int returnvalue = 0;
            foreach (CodeBlock cb in active_triggers)
            {
                if (cb.is_grabbed)
                {
                    returnvalue++;
                }
            }
            foreach (CodeBlock cb in active_actions)
            {
                if (cb.is_grabbed)
                {
                    returnvalue++;
                }
            }
            foreach (CodeBlock cb in active_conditions)
            {
                if (cb.is_grabbed)
                {
                    returnvalue++;
                }
            }
            foreach (BranchBlock cb in active_branches)
            {
                if (cb.is_grabbed)
                {
                    returnvalue++;
                }
            }
            return returnvalue;
        }

    }
}
