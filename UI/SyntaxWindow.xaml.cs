using System;
using System.Collections.Generic;
using static gamtetyper.Gametype;
using gamtetyper;

namespace Megalograph.UI
{
    /// <summary>
    /// Interaction logic for SyntaxWindow.xaml
    /// </summary>
    public partial class SyntaxWindow
    {
        public SyntaxWindow()
        {
            InitializeComponent();
        }

        public MainWindow main;

        public bool print_conjoined_conditions_on_new_lines = false;
        public int indent_spacing = 3;

        public class code_chunk
        {
            public string chunk_name;
            public List<string_line> contained_lines = new();
            public bool is_top_level = false;

            public int trigger_references_count;
        }
        public class string_line
        {
            public string line;
            public int trigger_index = -1;
            public code_chunk branch_block;
            public int condition_group = -1;
            public bool condition_invert = false;
        }

        List<trigger> t;
        List<action> a;
        List<condition> c;

        List<code_chunk> code_chunks = new();
        public string this_is_where_the_fun_begins()
        {
            string haloxml2 = main.Loaded_Gametypes[main.Current_Gametype].Target_Halo;
            string haloxml = main.returnmegldoc_fromhalo(haloxml2);
            bool is_Reach = false;
            if (haloxml2 == "HR")
                is_Reach = true;

            t = main.XP.returnScripts(is_Reach, haloxml);
            a = main.XP.doactions(haloxml);
            c = main.XP.doconditions(haloxml);

            

            foreach (var trigger in t)
            {
                var benis = read_trigg_instructions(trigger.Actions_count, trigger.Actions_insert, trigger.Conditions_count, trigger.Conditions_insert);

                string trig_name = "";
                if (trigger.Attribute.V != "OnCall" && trigger.Attribute.V != "OnTick")
                    trig_name += trigger.Attribute.V + ": ";

                

                if (trigger.Type.Params != null)
                {
                    string trig_params_test = string_ify_code(trigger.Type);
                    if (trig_params_test != "")
                    {
                        trig_name += trig_params_test;
                    }
                    else
                    {
                        string pee = "debugmoment";
                    }
                }
                else
                {
                    trig_name += trigger.Type.V;
                }
               
                benis.chunk_name = trig_name;

                if (trigger.Attribute.V != "OnCall")
                    benis.is_top_level = true;


                code_chunks.Add(benis);
            }
            string output_text = "";
            for (int i = 0; i < code_chunks.Count; i++) 
            {
                code_chunk cc = code_chunks[i];
                cc.trigger_references_count = trigger_references_map.ContainsKey(i)? trigger_references_map[i] : 0;

                if (cc.is_top_level && cc.trigger_references_count > 0)
                    output_text += "Trigger" + i + "() " + cc.chunk_name + "\n";
                else if (cc.is_top_level)
                    output_text += cc.chunk_name + "\n";
                else continue;
                output_text += spit_out_text_of_chunk(cc, 1);
                output_text += "end\n\n";

            }
            return output_text;
        }
        
        public string spit_out_text_of_chunk(code_chunk cc, int depth)
        {
            string output = "";
            int actuated_depth = depth;


            string_line? last_cond = null;
            
            for (int i = 0; i < cc.contained_lines.Count; i++)
            {
                var sl = cc.contained_lines[i];
                if (sl.branch_block != null) // is a branch block
                {
                    output += space_times(actuated_depth) + "branch\n";
                    output += spit_out_text_of_chunk(sl.branch_block, actuated_depth + 1);
                    output += space_times(actuated_depth) + "end\n";
                    last_cond = null;
                }
                else if (sl.trigger_index != -1) // is a trigger ref
                {
                    var peepee = code_chunks[sl.trigger_index];

                    if (peepee.is_top_level || peepee.trigger_references_count > 1)
                    {
                        output += space_times(actuated_depth) + "Trigger" + sl.trigger_index + "()\n";
                    }
                    else
                    {
                        output += space_times(actuated_depth) + peepee.chunk_name + "\n";
                        output += spit_out_text_of_chunk(peepee, actuated_depth + 1);
                        output += space_times(actuated_depth) + "end\n";
                    }
                    last_cond = null;
                }
                else
                {
                    if (sl.condition_group != -1) // condition
                    {
                        if (last_cond != null)
                        {
                            if (print_conjoined_conditions_on_new_lines)
                                output += space_times(actuated_depth - 1);
                            
                            if (last_cond.condition_group == sl.condition_group) // or
                            {
                                output += "or " + sl.line;

                                if (sl.condition_invert)
                                    output += "NOT ";

                                if (cc.contained_lines.Count > i + 1)
                                {
                                    if (cc.contained_lines[i + 1].condition_group == -1)
                                    {
                                        output += " then\n";
                                    }
                                    else
                                    {
                                        if (print_conjoined_conditions_on_new_lines)
                                            output += "\n";
                                        else
                                            output += " ";
                                    }
                                }
                                else
                                {
                                    output += " then\n";
                                }
                            }
                            else // and
                            {
                                output += "and " + sl.line;

                                if (sl.condition_invert)
                                    output += "NOT ";

                                if (cc.contained_lines.Count > i + 1)
                                {
                                    if (cc.contained_lines[i + 1].condition_group == -1)
                                    {
                                        output += " then\n";
                                    }
                                    else
                                    {
                                        if (print_conjoined_conditions_on_new_lines)
                                            output += "\n";
                                        else
                                            output += " ";
                                    }
                                }
                                else
                                {
                                    output += " then\n";
                                }
                            }
                        }
                        else
                        {
                            output += space_times(actuated_depth) + "if ";
                            if (sl.condition_invert)
                                output += "NOT ";
                            output += sl.line;
                            if (cc.contained_lines.Count > i + 1)
                            {
                                if (cc.contained_lines[i + 1].condition_group == -1)
                                {
                                    output += " then\n";
                                }
                                else
                                {
                                    if (print_conjoined_conditions_on_new_lines)
                                        output += "\n";
                                    else
                                        output += " ";
                                }
                            }
                            else
                            {
                                output += " then\n";
                            }
                            actuated_depth += 1;
                        }
                        last_cond = sl;
                    }
                    else // action
                    {
                        output += space_times(actuated_depth) + sl.line + "\n";
                        last_cond = null;
                    }
                }
            }
            while (actuated_depth > depth)
            {
                actuated_depth--;
                output += space_times(actuated_depth) + "end\n";
            }
            return output;
        }
        public string space_times(int depth)
        {
            string poopy = new String(' ', depth * indent_spacing);
            return poopy;
        }
        public Dictionary<int, int> trigger_references_map = new();
        public code_chunk read_trigg_instructions(int Actions_count, int Actions_insert, int Conditions_count, int Conditions_insert)
        {
            code_chunk curr_chunk = new();
            

            for (int w = 0; w < Actions_count; w++)  // LINE UP EVERY ACTION ELEMENT
            {
                int index_for_action = w + Actions_insert;
                var c_action = a[index_for_action];
                if (c_action.Type.V == "Virtual Trigger")
                {
                    curr_chunk.contained_lines.Add(new string_line { branch_block = read_trigg_instructions(Convert.ToInt16(c_action.Type.Params[0].Params[3].V), 
                                                                                                            Convert.ToInt16(c_action.Type.Params[0].Params[2].V)-1, 
                                                                                                            Convert.ToInt16(c_action.Type.Params[0].Params[1].V), 
                                                                                                            Convert.ToInt16(c_action.Type.Params[0].Params[0].V)-1 )});
                }
                else if (c_action.Type.V == "Megl.RunTrigger")
                {
                    int trigger_i = Convert.ToInt16(c_action.Type.Params[0].V);
                    curr_chunk.contained_lines.Add(new string_line { trigger_index = trigger_i });

                    if (trigger_references_map.ContainsKey(trigger_i))
                    {
                        trigger_references_map[trigger_i] += 1;
                    }
                    else
                    {
                        trigger_references_map[trigger_i] = 1;
                    }
                    //code_chunks[].trigger_references_count += 1;
                }
                else // is normal action
                {
                    curr_chunk.contained_lines.Add(new string_line { line = string_ify_code(c_action.Type) });  ;
                }
            }

            List<int> insertion_points = new List<int>();
            for (int w = 0; w < Conditions_count; w++)  // LINE UP EVERY CONDITION ELEMENT
            {
                int index_for_condition = w + Conditions_insert;

                var c_condition = c[index_for_condition];

                int condition_insert = c_condition.insertionpoint;
                int factored_insertion = condition_insert;
                for (int q = 0; q < insertion_points.Count; q++)
                {
                    if (insertion_points[q] <= condition_insert)
                    {
                        factored_insertion++;
                    }
                }
                insertion_points.Add(condition_insert);

                if (curr_chunk.contained_lines.Count < factored_insertion)
                {
                    curr_chunk.contained_lines.Add(new string_line { line = string_ify_code(c_condition.Type), condition_group = c_condition.OR_index_helper, condition_invert = c_condition.Not==1 });
                }
                else
                {
                    curr_chunk.contained_lines.Insert(factored_insertion, new string_line { line = string_ify_code(c_condition.Type), condition_group = c_condition.OR_index_helper, condition_invert = c_condition.Not==1 });
                }

            }
            return curr_chunk;
        }

        public string string_ify_code(Ebum input)
        {
            string output_s = input.V;
            output_s += "(";

            
            output_s += loop_on_params(input);

            output_s += ")";
            return output_s;
        }
        public string loop_on_params(Ebum Params)
        {
            if (Params.Params != null)
            {
                string sussy_amongus = "";
                for (int i = 0; i< Params.Params.Count; i++)
                {
                    if (i > 0)
                        sussy_amongus += ", ";
                    sussy_amongus += loop_on_params(Params.Params[i]);
                }
                return sussy_amongus;
            }
            else
            {
                return Params.V;
            }
        }
    }
}
