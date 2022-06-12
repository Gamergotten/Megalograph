using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using static gamtetyper.Gametype;
using static gamtetyper.BitReader;
using System.Diagnostics;
using static gamtetyper.BitWriter;
using System.Collections;
using System.Windows.Controls.Primitives;
using gamtetyper.code;

namespace gamtetyper
{


    public class XML_Process
    {
        // weve probably done this else where in this script but whatever
        public void write_string_use(string location, bool? use, string string_to_write)
        {
            XmlNode parenmt_node = XMLdump.SelectSingleNode(location);

            parenmt_node.Attributes.RemoveAll();
            XmlAttribute Attr = XMLdump.CreateAttribute("v");
            XmlNode string_node = XMLdump.SelectSingleNode(location + "/String");
            if (use == true)
            {
                Attr.Value = "True";
                // add new string node
                if (string_node != null)
                    parenmt_node.RemoveChild(string_node);
                XmlNode newstring = XMLdump.CreateNode("element", "String", "");
                XmlAttribute newAttr = XMLdump.CreateAttribute("v");
                if (string_to_write == null)
                    newAttr.Value = ""; 
                else
                    newAttr.Value = string_to_write; 
                newstring.Attributes.Append(newAttr); 
                parenmt_node.AppendChild(newstring);
            }
            else
            {
                Attr.Value = "False";
                // remove string node
                if (string_node != null)
                {
                    parenmt_node.RemoveChild(string_node);
                }
            }
            parenmt_node.Attributes.Append(Attr);

            XMLdump.Save(XMLdump_directory);
        }
        public void remove_xml_node(List<string> nodes)
        {
            string parent_thingo_node = @"Gametype/base";
            foreach (string s in nodes.SkipLast(1))
            {
                string test = String.Concat(s.Where(c => !Char.IsWhiteSpace(c) && !Char.IsSymbol(c)));
                parent_thingo_node += "/" + test;
            }
            string child_node = parent_thingo_node + "/" + nodes.Last();

            XmlNode node_parent = XMLdump.SelectSingleNode(parent_thingo_node);
            XmlNode node_child = XMLdump.SelectSingleNode(child_node);

            node_parent.RemoveChild(node_child);

            XMLdump.Save(XMLdump_directory);
        }
        public void add_table_string(List<string> nodes, bool is_reach)
        {
            string parent_thingo_node = @"Gametype/base";
            foreach (string s in nodes.SkipLast(1))
            {
                string test = String.Concat(s.Where(c => !Char.IsWhiteSpace(c) && !Char.IsSymbol(c)));
                parent_thingo_node += "/" + test;
            }
            string child_node = parent_thingo_node + "/" + nodes.Last();

            XmlNode node_parent = XMLdump.SelectSingleNode(parent_thingo_node);
            XmlNode node_child = XMLdump.SelectSingleNode(child_node);

            if (node_child != null)
                node_parent.RemoveChild(node_child);
            XmlNode child = XMLdump.CreateNode("element", nodes.Last(), "");
            node_parent.AppendChild(child);

            // now append every single language node

            append_node_(child, "EnglishString");
            append_node_(child, "JapaneseString");
            append_node_(child, "GermanString");
            append_node_(child, "FrenchString");
            append_node_(child, "SpanishString");
            append_node_(child, "MexicanString");
            append_node_(child, "ItalianString");
            append_node_(child, "KoreanString");
            append_node_(child, "Chinese1String");
            append_node_(child, "Chinese2String");
            append_node_(child, "PortugeseString");
            append_node_(child, "PolishString");

            if (!is_reach)
            {
                append_node_(child, "RussianString");
                append_node_(child, "DanishString");
                append_node_(child, "FinnishString");
                append_node_(child, "DutchString");
                append_node_(child, "NorwegianString");
            }
            

            XMLdump.Save(XMLdump_directory);
        }
        public void append_node_(XmlNode parent, string nodename)
        {
            XmlNode language = XMLdump.CreateNode("element", nodename, "");
            parent.AppendChild(language);

            XmlAttribute Attr = XMLdump.CreateAttribute("v");
            Attr.Value = "False";
            language.Attributes.Append(Attr);
        }
        // make it a keyvalue pair once its done
        public string return_from_dump_location(string location)
        {
            XmlNode a = XMLdump.SelectSingleNode(location);
            if (a != null)
                return a.Attributes["v"]?.InnerText;
            return null;
        }
        public List<string>? summon_xmlnode_children(string location)
        {
            XmlNode parenmt_node = XMLdump.SelectSingleNode(location);

            if (parenmt_node != null)
            {
                List<string> outpuit = new();

                foreach (XmlNode node in parenmt_node.ChildNodes)
                {
                    outpuit.Add(node.Name);
                }

                return outpuit;

            }
            return null;
        }
        public List<string>? summon_tables_strings(List<string> nodes)
        {
            string parent_thingo_node = @"Gametype/base";
            foreach (string s in nodes)
            {
                string test = String.Concat(s.Where(c => !Char.IsWhiteSpace(c) && !Char.IsSymbol(c)));
                parent_thingo_node += "/" + test;
            }
            parent_thingo_node += "/Strings";
            return summon_xmlnode_children(parent_thingo_node);
        }
        public void write_string_compression_type(List<string> nodes, bool? use_compression)
        {
            string parent_thingo_node = @"Gametype/base";
            foreach (string s in nodes)
            {
                string test = String.Concat(s.Where(c => !Char.IsWhiteSpace(c) && !Char.IsSymbol(c)));
                parent_thingo_node += "/" + test;
            }
            XmlNode parenmt_node = XMLdump.SelectSingleNode(parent_thingo_node);


            string doodoo = "UseCompression";
            string sus = String.Concat(doodoo.Where(c => !Char.IsWhiteSpace(c)));
            XmlNode nodde_child = parenmt_node.SelectSingleNode(sus);

            if (nodde_child != null)
                parenmt_node.RemoveChild(nodde_child);



            XmlNode child = XMLdump.CreateNode("element", "UseCompression", "");

            XmlAttribute Attr = XMLdump.CreateAttribute("v");
            if (use_compression == true)
                Attr.Value = "True"; // true / false
            else
                Attr.Value = "False"; // true / false

            child.Attributes.Append(Attr); // surely this wont work, its set specifically as "get"

            parenmt_node.AppendChild(child);

            XMLdump.Save(XMLdump_directory);
        }

        public void WRITE_NODE_OF_FILE(Ebum target)
        {
            var mmmmmmm = target.nodes_list_yes_i_did_just_do_that;

            string parent_thingo_node = @"Gametype/base";
            foreach (string s in mmmmmmm.SkipLast(1))
            {
                string test = String.Concat(s.Where(c => !Char.IsWhiteSpace(c) && !Char.IsSymbol(c)));
                parent_thingo_node += "/" + test;
            }

            XmlNode node_parent = XMLdump.SelectSingleNode(parent_thingo_node);
            //XmlNode nodde_child = XMLdump.SelectSingleNode(child_node);

            //if (nodde_child != null)
            //    node_parent.RemoveChild(nodde_child);

            //XmlNode acthead = XMLdump.CreateNode("element", target.Name, "");
            //node_parent.AppendChild(acthead); // why are we completely scrubbing this node lol

            //XmlAttribute Attr = XMLdump.CreateAttribute("v");
            //Attr.Value = target.V;
            //acthead.Attributes.Append(Attr); // surely this wont work, its set specifically as "get"


            append_children_from_ebum_export(node_parent, target);

            XMLdump.Save(XMLdump_directory);
        }
        public void WRITE_ENUM_NODE_OF_FILE(Ebum target)
        {
            var mmmmmmm = target.nodes_list_yes_i_did_just_do_that;

            string parent_thingo_node = @"Gametype/base";
            foreach (string s in mmmmmmm.SkipLast(2))
            {
                string test = String.Concat(s.Where(c => !Char.IsWhiteSpace(c) && !Char.IsSymbol(c)));
                parent_thingo_node += "/" + test;
            }

            XmlNode node_parent = XMLdump.SelectSingleNode(parent_thingo_node);
            //XmlNode nodde_child = XMLdump.SelectSingleNode(child_node);

            //if (nodde_child != null)
            //    node_parent.RemoveChild(nodde_child);

            //XmlNode acthead = XMLdump.CreateNode("element", target.Name, "");
            //node_parent.AppendChild(acthead); // why are we completely scrubbing this node lol

            //XmlAttribute Attr = XMLdump.CreateAttribute("v");
            //Attr.Value = target.V;
            //acthead.Attributes.Append(Attr); // surely this wont work, its set specifically as "get"


            append_children_from_ebum_export(node_parent, target);

            XMLdump.Save(XMLdump_directory);
        }

        public void clear_xml_thingo_thanks(Ebum target)
        {
            var mmmmmmm = target.nodes_list_yes_i_did_just_do_that;

            string parent_thingo_node = @"Gametype/base";
            foreach (string s in mmmmmmm.SkipLast(1))
            {
                string test = String.Concat(s.Where(c => !Char.IsWhiteSpace(c)));
                parent_thingo_node += "/" + test;
            }
            string child_node = @"Gametype/base";
            foreach (string s in mmmmmmm) //.Take(mmmmmmm.Count - 1))
            {
                string test = String.Concat(s.Where(c => !Char.IsWhiteSpace(c)));
                child_node += "/" + test;
            }

            XmlNode node_parent = XMLdump.SelectSingleNode(parent_thingo_node);
            XmlNode nodde_child = XMLdump.SelectSingleNode(child_node);

            if (nodde_child != null)
                node_parent.RemoveChild(nodde_child);
            else
            {
                string debug_moment = "we tried to delete something that didnt exist :C"; // ##DEBUG
            }

            XMLdump.Save(XMLdump_directory);
        }
        public List<Ebum> READ_THE_WHOLE_FILE(string node_directory, string shorter_directory, string haloxml)
        {
            string xml = haloxml;
            XmlDocument c = xina(xml);
            //XmlNode o = c.SelectSingleNode(node_directory);
            //paramheader2 ph = test(o, xml, "ExTypes");

            List<Ebum> bases = new();
            //foreach (XmlNode xn in o.ChildNodes)

            //_blf
            XmlNode o1 = c.SelectSingleNode("/base/ExTypes/Var[@name='_blf']");
            paramheader2 ph1 = test(o1, xml, "ExTypes");
            bases.Add(readblock(ph1, "ExTypes", new List<string> { }));
            //chdr
            XmlNode o2 = c.SelectSingleNode("/base/ExTypes/Var[@name='chdr']");
            paramheader2 ph2 = test(o2, xml, "ExTypes");
            bases.Add(readblock(ph2, "ExTypes", new List<string> { }));
            //mpvr
            XmlNode o3 = c.SelectSingleNode("/base/ExTypes/Var[@name='mpvr']");
            paramheader2 ph3 = test(o3, xml, "ExTypes");
            bases.Add(readblock(ph3, "ExTypes", new List<string> { }));
            //_eof
            XmlNode o4 = c.SelectSingleNode("/base/ExTypes/Var[@name='_eof']");
            paramheader2 ph4 = test(o4, xml, "ExTypes");
            bases.Add(readblock(ph4, "ExTypes", new List<string> { }));

            return bases;
        }

        public Ebum cheat_to_do_the_node_creation(string node_directory, string shorter_directory, string haloxml)
        {

            string xml = haloxml;
            XmlDocument c = xina(xml);
            XmlNode o = c.SelectSingleNode(node_directory);
            paramheader2 ph2 = test(o, xml, "ExTypes");

            return readblock(ph2, shorter_directory, new List<string> { "AMONGSUS" }); // purposely fail to retrieve from xmldump
        }


        XmlWriterSettings settings = new XmlWriterSettings()
        {
            Indent = true,
        };

        public string XMLdirectory;


        public Dictionary<string, XmlDocument> External = new Dictionary<string, XmlDocument>();
        public XmlWriter xmlWriter;

        public XmlDocument XMLdump;
        public string XMLdump_directory;

        public List<string> big_fella_test(string a_xml, string path)
        {
            XmlDocument a_c = xina(a_xml);
            XmlNode a_o = a_c.SelectSingleNode("/base/" + path);
            List<string> list = new List<string>();
            foreach (XmlNode xm in a_o.ChildNodes)
            {
                list.Add(xm.Attributes["name"]?.InnerText);
            }
            return list;
        }

        public List<Gametype.trigger> returnScripts(bool is_reach, string haloxml)
        {
            try
            {
                List<Gametype.trigger> triggers = new List<Gametype.trigger>();


                XmlNode a = XMLdump.SelectSingleNode("/Gametype/base/mpvr/megl/MegaloScript/TriggerCount");
                XmlNodeList b = a.ChildNodes;

                int i = 0;
                foreach (XmlNode xmln in b)
                {

                    string node = xmln.Name;

                    Gametype.trigger curr_trigg = new Gametype.trigger();

                    var valid_test = xmln.Attributes["p"];
                    if (valid_test != null)
                    {
                        // we are going to need to check to see if this attribute is not null if we ever add more attributes
                        // however i'll probably figure that out pretty easy if we ever have that problem because this text will be here lol
                        string[] s = valid_test.InnerText.Split(",");
                        curr_trigg.position.x = Convert.ToDouble(s[0]);
                        curr_trigg.position.y = Convert.ToDouble(s[1]);
                        curr_trigg.position.has_position = true;
                    }

                    // find our saved name, else give a new one ; we cant save a name right now
                    curr_trigg.Name = "Trigger" + i;

                    // do attribute
                    //Gametype.Ebum attribute = new Gametype.Ebum();
                    //XmlNode w = XMLdump.SelectSingleNode("/Gametype/base/mpvr/megl/MegaloScript/TriggerCount/" + node + "/Attribute");
                    //attribute.V = w.Attributes["v"]?.InnerText;
                    //XmlNodeList e = w.ChildNodes;
                    //if (e.Count > 0)
                    //    attribute.Params = new List<Ebum>();
                    //foreach (XmlNode attr_child in e)
                    //{
                    //    attribute.Params.Add(new Gametype.Ebum { V = attr_child.Attributes["v"]?.InnerText });
                    //}
                    //curr_trigg.Attribute = attribute;
                    // grab our ph
                    string a_xml = haloxml;
                    XmlDocument a_c = xina(a_xml);
                    XmlNode a_o = a_c.SelectSingleNode("/base/ExTypes/Var[@name='MegaloScript']/Var[@name='TriggerCount']/Var[@name='Attribute']");
                    paramheader2 a_ph2 = test(a_o, a_xml, "ExTypes");

                    curr_trigg.Attribute = readblock(a_ph2, "ExTypes/Var[@name='MegaloScript']/Var[@name='TriggerCount']", new List<string> { "mpvr", "megl", "MegaloScript", "TriggerCount", node });

                    // do type
                    //Gametype.Ebum type = new Gametype.Ebum();
                    //w = XMLdump.SelectSingleNode("/Gametype/base/mpvr/megl/MegaloScript/TriggerCount/" + node + "/Type");
                    //type.V = w.Attributes["v"]?.InnerText;
                    //e = w.ChildNodes;
                    //if (e.Count > 0)
                    //    type.Params = new List<Ebum>();
                    //foreach (XmlNode type_child in e)
                    //{
                    //    type.Params.Add(new Gametype.Ebum { V =  type_child.Attributes["v"]?.InnerText });
                    //}
                    //curr_trigg.Type = type;

                    // grab our ph
                    string t_xml = haloxml; // how do i mark this so i can fix it later
                    XmlDocument t_c = xina(t_xml);
                    XmlNode t_o = t_c.SelectSingleNode("/base/ExTypes/Var[@name='MegaloScript']/Var[@name='TriggerCount']/Var[@name='Type']");
                    paramheader2 t_ph2 = test(t_o, t_xml, "ExTypes");

                    curr_trigg.Type = readblock(t_ph2, "ExTypes/Var[@name='MegaloScript']/Var[@name='TriggerCount']", new List<string> { "mpvr", "megl", "MegaloScript", "TriggerCount", node });


                    //
                    // now we find the actions and conditions Oooh tricky
                    //

                    int total_actions = int.Parse(XMLdump.SelectSingleNode("/Gametype/base/mpvr/megl/MegaloScript/TriggerCount/" + node + "/ActionCount").Attributes["v"].InnerText);

                    int actions_offset = int.Parse(XMLdump.SelectSingleNode("/Gametype/base/mpvr/megl/MegaloScript/TriggerCount/" + node + "/ActionOffset").Attributes["v"].InnerText);
                    if (!is_reach) actions_offset -= 1;
                    //actions_offset -= 1;
                    XmlNodeList Actions = XMLdump.SelectSingleNode("/Gametype/base/mpvr/megl/MegaloScript/ActionCount").ChildNodes;

                    curr_trigg.Actions_insert = actions_offset;
                    curr_trigg.Actions_count = total_actions;

                    //
                    // COPY ACTIONS FOR CONDITIONS
                    int total_conditions = int.Parse(XMLdump.SelectSingleNode("/Gametype/base/mpvr/megl/MegaloScript/TriggerCount/" + node + "/ConditionCount").Attributes["v"].InnerText);

                    int conditions_offset = int.Parse(XMLdump.SelectSingleNode("/Gametype/base/mpvr/megl/MegaloScript/TriggerCount/" + node + "/ConditionOffset").Attributes["v"].InnerText);
                    if (!is_reach) conditions_offset -= 1;
                    //conditions_offset -= 1;
                    XmlNodeList Conditions = XMLdump.SelectSingleNode("/Gametype/base/mpvr/megl/MegaloScript/ConditionCount").ChildNodes;

                    curr_trigg.Conditions_insert = conditions_offset;
                    curr_trigg.Conditions_count = total_conditions;


                    // we could just set these to 1, because thats all they ever seem to be
                    if (!is_reach)
                    {
                        curr_trigg.unknown1 = int.Parse(XMLdump.SelectSingleNode("/Gametype/base/mpvr/megl/MegaloScript/TriggerCount/" + node + "/Unknown1").Attributes["v"].InnerText);
                        curr_trigg.unknown2 = int.Parse(XMLdump.SelectSingleNode("/Gametype/base/mpvr/megl/MegaloScript/TriggerCount/" + node + "/Unknown2").Attributes["v"].InnerText);
                    }
                    triggers.Add(curr_trigg);
                    i++;
                }
                return triggers;

            }
            catch (Exception ex)
            {
                MainWindow.catchexception_and_duly_ignore(ex);
                return null;
            }
        }

        public List<Gametype.action> doactions(string haloxml)
        {
            XmlNodeList Actions = XMLdump.SelectSingleNode("/Gametype/base/mpvr/megl/MegaloScript/ActionCount").ChildNodes;

            List<action> ActionDump = new List<action>();

            for (int Index = 0; Index < Actions.Count; Index++)
            {
                action newaction = new action();

                XmlNode w = Actions[Index].SelectSingleNode("Type");

                XmlNode actionnodexml = Actions[Index];
                var valid_test = actionnodexml.Attributes["p"];
                if (valid_test != null)
                {
                    // we are going to need to check to see if this attribute is not null if we ever add more attributes
                    // however i'll probably figure that out pretty easy if we ever have that problem because this text will be here lol
                    string[] s = valid_test.InnerText.Split(",");
                    newaction.position.x = Convert.ToDouble(s[0]);
                    newaction.position.y = Convert.ToDouble(s[1]);
                    newaction.position.has_position = true;
                }

                // grab our ph
                string xml = haloxml;
                XmlDocument c = xina(xml);
                XmlNode o = c.SelectSingleNode("/base/ExTypes/Var[@name='MegaloScript']/Var[@name='ActionCount']/Var[@name='Type']");
                paramheader2 ph2 = test(o, xml, "ExTypes");

                newaction.Type = readblock(ph2, "ExTypes/Var[@name='MegaloScript']/Var[@name='ActionCount']", new List<string> { "mpvr", "megl", "MegaloScript", "ActionCount", w.ParentNode.Name });

                ActionDump.Add(newaction);
            }

            return ActionDump;
        }

        public List<Gametype.condition> doconditions(string haloxml)
        {
            XmlNodeList Conditions = XMLdump.SelectSingleNode("/Gametype/base/mpvr/megl/MegaloScript/ConditionCount").ChildNodes;

            List<condition> ConditionsDump = new List<condition>();

            for (int Index = 0; Index < Conditions.Count; Index++)
            {
                condition newcondition = new condition();

                XmlNode actionnodexml = Conditions[Index];
                var valid_test = actionnodexml.Attributes["p"];
                if (valid_test != null)
                {
                    // we are going to need to check to see if this attribute is not null if we ever add more attributes
                    // however i'll probably figure that out pretty easy if we ever have that problem because this text will be here lol
                    string[] s = valid_test.InnerText.Split(",");
                    newcondition.position.x = Convert.ToDouble(s[0]);
                    newcondition.position.y = Convert.ToDouble(s[1]);
                    newcondition.position.has_position = true;
                }

                XmlNode w = Conditions[Index].SelectSingleNode("Type");

                // grab our ph
                string xml = haloxml;
                XmlDocument c = xina(xml);
                XmlNode o = c.SelectSingleNode("/base/ExTypes/Var[@name='MegaloScript']/Var[@name='ConditionCount']/Var[@name='Type']");
                paramheader2 ph2 = test(o, xml, "ExTypes");

                newcondition.Type = readblock(ph2, "ExTypes/Var[@name='MegaloScript']/Var[@name='ConditionCount']", new List<string> { "mpvr", "megl", "MegaloScript", "ConditionCount", w.ParentNode.Name });

                string node = Conditions[Index].Name;
                newcondition.Not = int.Parse(XMLdump.SelectSingleNode("/Gametype/base/mpvr/megl/MegaloScript/ConditionCount/" + node + "/NOT").Attributes["v"].InnerText);
                newcondition.insertionpoint = int.Parse(XMLdump.SelectSingleNode("/Gametype/base/mpvr/megl/MegaloScript/ConditionCount/" + node + "/ConditionOffset").Attributes["v"].InnerText);
                newcondition.OR_Group = int.Parse(XMLdump.SelectSingleNode("/Gametype/base/mpvr/megl/MegaloScript/ConditionCount/" + node + "/ORsequence").Attributes["v"].InnerText);

                ConditionsDump.Add(newcondition);
            }

            return ConditionsDump;
        }




        public void append_children_from_ebum_export(XmlNode parent, Ebum info)
        {
            string nodename = info.Name.Replace(" ", "");

            // i dont think we store the names of the nodes we got the ebums from
            if (info.FUCKK_YOU != "" && info.FUCKK_YOU != null)
            {
                //string parnode = info.FUCKK_YOU.Replace(" ", "");

                // i for real have no idea what this *was* for, so i will work around it

                if (parent.Name != info.FUCKK_YOU)
                {
                    string parnode = String.Concat(info.FUCKK_YOU.Where(c => !Char.IsWhiteSpace(c)));
                    XmlNode enumref_parent = XMLdump.CreateNode("element", parnode, "");
                    parent.AppendChild(enumref_parent);
                    parent = enumref_parent;
                }
            //    }
            }

            // THIS WOULD BREAK WHEN AN ACTION'S TRIGGER POSITION WAS CHANGED
            //var mmmmmmm = info.nodes_list_yes_i_did_just_do_that;

            //string child_node = @"Gametype/base";
            //foreach (string s in mmmmmmm) //.Take(mmmmmmm.Count - 1))
            //{
            //    string test = String.Concat(s.Where(c => !Char.IsWhiteSpace(c)));
            //    child_node += "/" + test;
            //}

            //XmlNode nodde_child1 = XMLdump.SelectSingleNode(child_node);

            string doodoo = info.nodes_list_yes_i_did_just_do_that.Last();
            string sus = String.Concat(doodoo.Where(c => !Char.IsWhiteSpace(c)));
            XmlNode nodde_child = parent.SelectSingleNode(sus);

            if (nodde_child != null)
                parent.RemoveChild(nodde_child);



            XmlNode child = XMLdump.CreateNode("element", nodename, "");

            XmlAttribute Attr = XMLdump.CreateAttribute("v");
            Attr.Value = info.V;
            child.Attributes.Append(Attr); // surely this wont work, its set specifically as "get"

            parent.AppendChild(child);
            parent = child;

            if (info.Params != null)
            {
                for (int i = 0; i < info.Params.Count; i++)
                {
                    append_children_from_ebum_export(parent, info.Params[i]);
                }
            }
        }

        public void create_and_embed_node_w_attribute(string node_name, string attribute_value, XmlNode parent)
        {
            XmlNode ConditionOffset = XMLdump.CreateNode("element", node_name, "");
            XmlAttribute Attr = XMLdump.CreateAttribute("v");
            Attr.Value = attribute_value;
            ConditionOffset.Attributes.Append(Attr); // surely this wont work, its set specifically as "get"
            parent.AppendChild(ConditionOffset);
        }

        public void exportScripts(List<Gametype.trigger> things_to_export, bool is_reach)
        {
            XmlNode a = XMLdump.SelectSingleNode("/Gametype/base/mpvr/megl/MegaloScript/TriggerCount");
            a.RemoveAll();

            for (int Index = 0; Index < things_to_export.Count; Index++)
            {
                Gametype.trigger trig = things_to_export[Index];
                XmlNode trighead = XMLdump.CreateNode("element", "TriggerCount-child" + Index, "");
                a.AppendChild(trighead);

                XmlAttribute posit = XMLdump.CreateAttribute("p");
                posit.Value = trig.position.x + "," + trig.position.y;
                trighead.Attributes.Append(posit);


                append_children_from_ebum_export(trighead, trig.Attribute);

                append_children_from_ebum_export(trighead, trig.Type);

                //
                // now we find the actions and conditions Oooh tricky

                // condition offset
                create_and_embed_node_w_attribute("ConditionOffset", trig.Conditions_insert.ToString(), trighead);

                // condition count
                create_and_embed_node_w_attribute("ConditionCount", trig.Conditions_count.ToString(), trighead);

                // action offset
                create_and_embed_node_w_attribute("ActionOffset", trig.Actions_insert.ToString(), trighead);

                // action count
                create_and_embed_node_w_attribute("ActionCount", trig.Actions_count.ToString(), trighead);

                // we could just set these to 1, because thats all they ever seem to be
                if (!is_reach)
                {
                    // unknown1
                    create_and_embed_node_w_attribute("Unknown1", trig.unknown1.ToString(), trighead);

                    // unknown2
                    create_and_embed_node_w_attribute("Unknown2", trig.unknown2.ToString(), trighead);
                }
            }

        }

        public void exportactions(List<Gametype.action> things_to_export)
        {
            XmlNode a = XMLdump.SelectSingleNode("/Gametype/base/mpvr/megl/MegaloScript/ActionCount");
            a.RemoveAll();

            for (int Index = 0; Index < things_to_export.Count; Index++)
            {
                Gametype.action action = things_to_export[Index];
                XmlNode acthead = XMLdump.CreateNode("element", "ActionCount-child" + Index, "");
                a.AppendChild(acthead);

                XmlAttribute posit = XMLdump.CreateAttribute("p");
                posit.Value = action.position.x + "," + action.position.y;
                acthead.Attributes.Append(posit);

                append_children_from_ebum_export(acthead, action.Type);
            }
        }

        public void exportconditions(List<Gametype.condition> things_to_export)
        {
            XmlNode a = XMLdump.SelectSingleNode("/Gametype/base/mpvr/megl/MegaloScript/ConditionCount");
            a.RemoveAll();

            for (int Index = 0; Index < things_to_export.Count; Index++)
            {
                Gametype.condition cond = things_to_export[Index];
                XmlNode condhead = XMLdump.CreateNode("element", "ConditionCount-child" + Index, "");
                a.AppendChild(condhead);

                XmlAttribute posit = XMLdump.CreateAttribute("p");
                posit.Value = cond.position.x + "," + cond.position.y;
                condhead.Attributes.Append(posit);

                create_and_embed_node_w_attribute("NOT", cond.Not.ToString(), condhead);

                create_and_embed_node_w_attribute("ORsequence", cond.OR_Group.ToString(), condhead);

                create_and_embed_node_w_attribute("ConditionOffset", cond.insertionpoint.ToString(), condhead);

                append_children_from_ebum_export(condhead, cond.Type);
            }
        }
        public void quick_save_the_xmls()
        {
            XMLdump.Save(XMLdump_directory);
        }

        public void create_virtual_xdoc()
        {
            XMLdump = new XmlDocument();

            XmlNode root = XMLdump.CreateNode("element", "Gametype", "");
            XMLdump.AppendChild(root);
            XmlNode _base = XMLdump.CreateNode("element", "base", "");
            root.AppendChild(_base);
            XmlNode mpvr = XMLdump.CreateNode("element", "mpvr", "");
            _base.AppendChild(mpvr);
            XmlNode megl = XMLdump.CreateNode("element", "megl", "");
            mpvr.AppendChild(megl);
            XmlNode MegaloScript = XMLdump.CreateNode("element", "MegaloScript", "");
            megl.AppendChild(MegaloScript);

            XmlNode TriggerCount = XMLdump.CreateNode("element", "TriggerCount", "");
            MegaloScript.AppendChild(TriggerCount);
            XmlNode ConditionCount = XMLdump.CreateNode("element", "ConditionCount", "");
            MegaloScript.AppendChild(ConditionCount);
            XmlNode ActionCount = XMLdump.CreateNode("element", "ActionCount", "");
            MegaloScript.AppendChild(ActionCount);
        }

        public void create_blank_virtual_xdoc_but_then_paste_a_string_in_it(string paste)
        {
            XMLdump = new XmlDocument();
            XMLdump.LoadXml(paste);
        }



















        public Ebum readblock(paramheader2 ph, string wrap, List<string> node)
        {
            Ebum t = new Ebum();
            //t.V = ph.name;
            node.Add(ph.name);

            t.nodes_list_yes_i_did_just_do_that = new List<string>(node);

            t.Name = node[node.Count - 1];
            t.XMLDoc = ph.node;

            t.XMLPath = wrap + "/ Var[@name = '" + ph.name + "']";
            //t.XMLPath = ph.WHERE_WE_FUCKING_FOUND_IT;
            t.Type = ph.type;

            //XmlDocument i = xina(ph.node);
            //XmlNode xnfdsahghvbgdsavghgvhdtf = i.SelectSingleNode(t.XMLPath);

            t.Size = ph.bits; // int.Parse(xnfdsahghvbgdsavghgvhdtf.Attributes["bits"]?.InnerText);

            switch (ph.type)
            {
                case "Int":
                    var test1 = returnfromdump(node);
                    if (test1 == null)
                    {
                        test1 = "0";
                    }
                    t.V = test1;
                    node.RemoveAt(node.Count - 1);
                    return t;
                case "UInt":
                    t.V = returnfromdump(node);
                    node.RemoveAt(node.Count - 1);
                    return t;
                case "Long":
                    t.V = returnfromdump(node);
                    node.RemoveAt(node.Count - 1);
                    return t;
                case "ULong":
                    t.V = returnfromdump(node);
                    node.RemoveAt(node.Count - 1);
                    return t;
                case "String":
                    t.V = returnfromdump(node);
                    node.RemoveAt(node.Count - 1);
                    return t;
                case "String16":
                    t.V = returnfromdump(node);
                    node.RemoveAt(node.Count - 1);
                    return t;
                case "UString8":
                    t.V = returnfromdump(node);
                    node.RemoveAt(node.Count - 1);
                    return t;
                case "UString16":
                    t.V = returnfromdump(node);
                    node.RemoveAt(node.Count - 1);
                    return t;
                case "Hex":
                    t.V = returnfromdump(node);
                    node.RemoveAt(node.Count - 1);
                    return t;
                case "Blank":
                    t.V = returnfromdump(node);
                    node.RemoveAt(node.Count - 1);
                    return t;
                case "Enum":
                    string test2 = returnfromdump(node);
                    if (node.Count > 7)
                    {
                        string debug = "";
                    }
                    if (test2 == null)
                    {
                        XmlDocument c = xina(ph.node);
                        XmlNode w = c.SelectSingleNode("base/" + wrap + "/Var[@name='" + ph.name + "']");
                        test2 = w.ChildNodes[0].Attributes["name"]?.InnerText;
                    }
                    string y1 = "base/" + wrap + "/Var[@name='" + ph.name + "']/Var[@name='" + test2 + "']";
                    // i think we need to read the value of the enum and use this here
                    InterpStruct e1 = readdata(y1, ph.node);
                    t.V = test2;
                    // append the ID to our list... might needa fetch it first
                    t.Params = readchildren(e1, wrap + "/Var[@name='" + ph.name + "']", ph.node, node);
                    break;
                case "Container":
                    // i think this should just work
                    InterpStruct e2 = readdata("/base/" + ph.node1 + "/Var[@name='" + ph.name + "']", ph.node);
                    t.Params = readchildren(e2, wrap, ph.node, node);
                    break;
                case "Count":
                    // convert int to binary and append to compile
                    int m = numbofchild(node);
                    //string v = clippedtolength(Convert.ToString(m, 2), ph.bits);
                    // count children
                    t.Params = new List<Ebum>();
                    for (int i = 0; i < m; i++)
                    {
                        Ebum wrapper = new();
                        node.Add(ph.name + "-child" + i);
                        InterpStruct e = readdata("/base/" + ph.node1 + "/Var[@name='" + ph.name + "']", ph.node);
                        wrapper.Name = ph.name + "-child" + i;
                        wrapper.Params = readchildren(e, wrap, ph.node, node);
                        var w = new List<string>(node);
                        wrapper.nodes_list_yes_i_did_just_do_that = w;
                        t.Params.Add(wrapper);
                        node.RemoveAt(node.Count - 1);
                    }
                    break;
                default:
                    if (ph.type.Contains("External"))
                    {
                        string[] w = ph.type.Split(":");
                        t.Params = new List<Ebum>();
                        foreach (paramheader2 p in returnchildren(w[1]))
                        {
                            t.Params.Add(readblock(p, "ExTypes", node));
                        }
                    }
                    if (ph.type.Contains("Enumref"))
                    {
                        // should just work tbh
                        string[] w = ph.type.Split(":");
                        ph.node1 = "RefTypes";
                        paramheader2 ph2 = VarInterpret(ph.node1, w[1], ph.node);
                        ph2.node = ph.node;
                        ph2.node1 = "RefTypes";
                        // t.Params = new List<Ebum>();
                        // t.Params.Add(readblock(ph2, ph2.node1, node));
                        // node.RemoveAt(node.Count - 1); // FUCK YOU PREVIOUS ME

                        Ebum e3 = readblock(ph2, ph2.node1, node);
                        node.RemoveAt(node.Count - 1);
                        e3.FUCKK_YOU = ph.name;
                        if (ph.name == "Bool1")
                        {
                            string DEBUGTHESEBooLLS = "";
                        }
                        return e3;
                    }
                    break;

            }
            node.RemoveAt(node.Count - 1);
            return t;
        }

        public paramheader2 VarInterpret(string type, string lookfor, string docx) // need to fix this to use a shared param rather than an array
        {
            paramheader2 m = handleVarHeader(type, lookfor, docx);
            return m;
        }

        public List<Ebum> readchildren(InterpStruct e, string parentloc, string docx, List<string> nodes)
        {
            if (e.blockparams != null && e.blockparams.Count > 0) // simplify pls
            {
                List<Ebum> output = new List<Ebum>();
                foreach (InterpStruct.s w in e.blockparams)
                {
                    paramheader2 ph = VarInterpret(parentloc + "/Var[@name='" + e.blockname + "']" + "", w.name, docx);
                    ph.node = docx;
                    ph.node1 = parentloc + "/Var[@name='" + e.blockname + "']";
                    output.Add(readblock(ph, parentloc + "/Var[@name='" + e.blockname + "']", nodes));
                }
                return output;
            }
            return null;
        }


















        public string fetchenumID(string path, string doc)
        {
            XmlDocument c = xina(doc);
            XmlNode w = c.SelectSingleNode(path);
            return w.Attributes["ID"]?.InnerText;
        }
        public int numbofchild(List<string> URI)
        {
            string URItogo = @"Gametype/base";
            foreach (string s in URI)
            {
                string test = String.Concat(s.Where(c => !Char.IsWhiteSpace(c)));
                URItogo += "/" + test;
            }
            XmlNodeList? null_check = XMLdump.SelectSingleNode(URItogo)?.ChildNodes;

            if (null_check == null) return 0;
            return null_check.Count;
        }
        public bool intakedecompiledmode_verscheck(string s, string targethalo)
        {
            intakeDecompiledmode(s);
            string db_vers = check_db_revision(targethalo);

            string target_vers = XMLdump.SelectSingleNode("Gametype").Attributes["revision"]?.InnerText;
            return db_vers == target_vers;
        }
        public void intakeDecompiledmode(string s)
        {
            //if (XMLdump != null)
            //    XMLdump = null;
            XMLdump = new XmlDocument();
            XMLdump.Load(s);
            XMLdump_directory = s;
        }
        public void wipe_decompiled_mode()
        {
            if (XMLdump != null)
                XMLdump = null;
            XMLdump_directory = null;
        }

        public string returnfromdump(List<string> height)
        {
            string URItogo = @"Gametype/base";
            foreach (string s in height)
            {
                string test = String.Concat(s.Where(c => !Char.IsWhiteSpace(c)));
                URItogo += "/" + test;
            }
            XmlNode a = XMLdump.SelectSingleNode(URItogo);
            if (a != null)
                return a.Attributes["v"]?.InnerText;
            return null;
        }

        public string hexstringblock;
        public string returnstringtable_to_binary(List<string> node, int ph_bits,  int ph_chars, bool isreach, bool use_compression, bool is_the_annoying_one)
        {
            // maybe someday we'll add error checking here

            List<BitReader.zoingoboingo> stringbitindexes = new();
            Dictionary<string, int> string_collection_indexes = new();
            hexstringblock = "";

            string URItogo = @"Gametype/base";
            foreach (string s in node)
            {
                string test = String.Concat(s.Where(c => !Char.IsWhiteSpace(c)));
                URItogo += "/" + test;
            }
            XmlNode a = XMLdump.SelectSingleNode(URItogo);
            // now we've got our dump string node
            XmlNodeList all_Strings = a.ChildNodes;
            if (all_Strings.Count > 0)
            {
                foreach (XmlNode n in all_Strings)
                {
                    BitReader.zoingoboingo current_string = new();
                    current_string.EnglishStringIndex = stringtablelanguage("EnglishString", n, string_collection_indexes);
                    current_string.JapaneseStringIndex = stringtablelanguage("JapaneseString", n, string_collection_indexes);
                    current_string.GermanStringIndex = stringtablelanguage("GermanString", n, string_collection_indexes);
                    current_string.FrenchStringIndex = stringtablelanguage("FrenchString", n, string_collection_indexes);
                    current_string.SpanishStringIndex = stringtablelanguage("SpanishString", n, string_collection_indexes);
                    current_string.MexicanStringIndex = stringtablelanguage("MexicanString", n, string_collection_indexes);
                    current_string.ItalianStringIndex = stringtablelanguage("ItalianString", n, string_collection_indexes);
                    current_string.KoreanStringIndex = stringtablelanguage("KoreanString", n, string_collection_indexes);
                    current_string.Chinese1StringIndex = stringtablelanguage("Chinese1String", n, string_collection_indexes);
                    current_string.Chinese2StringIndex = stringtablelanguage("Chinese2String", n, string_collection_indexes);
                    current_string.PortugeseStringIndex = stringtablelanguage("PortugeseString", n, string_collection_indexes);
                    current_string.PolishStringIndex = stringtablelanguage("PortugeseString", n, string_collection_indexes);
                    if (!isreach) // H4 & 2A
                    {
                        current_string.RussianStringIndex = stringtablelanguage("RussianString", n, string_collection_indexes);
                        current_string.DanishStringIndex = stringtablelanguage("DanishString", n, string_collection_indexes);
                        current_string.FinnishStringIndex = stringtablelanguage("DanishString", n, string_collection_indexes);
                        current_string.DutchStringIndex = stringtablelanguage("DutchString", n, string_collection_indexes);
                        current_string.NorwegianStringIndex = stringtablelanguage("NorwegianString", n, string_collection_indexes);
                    }
                    stringbitindexes.Add(current_string);
                }

                string binaryblock = BitWriter.clippedtolength(Convert.ToString(stringbitindexes.Count, 2), ph_bits);
                foreach (var va in stringbitindexes)
                {
                    binaryblock += clipwrapper(va.EnglishStringIndex, ph_chars);
                    binaryblock += clipwrapper(va.JapaneseStringIndex, ph_chars);
                    binaryblock += clipwrapper(va.GermanStringIndex, ph_chars);
                    binaryblock += clipwrapper(va.FrenchStringIndex, ph_chars);
                    binaryblock += clipwrapper(va.SpanishStringIndex, ph_chars);
                    binaryblock += clipwrapper(va.MexicanStringIndex, ph_chars);
                    binaryblock += clipwrapper(va.ItalianStringIndex, ph_chars);
                    binaryblock += clipwrapper(va.KoreanStringIndex, ph_chars);
                    binaryblock += clipwrapper(va.Chinese1StringIndex, ph_chars);
                    binaryblock += clipwrapper(va.Chinese2StringIndex, ph_chars);
                    binaryblock += clipwrapper(va.PortugeseStringIndex, ph_chars);
                    binaryblock += clipwrapper(va.PolishStringIndex, ph_chars);
                    if (!isreach)
                    {
                        binaryblock += clipwrapper(va.RussianStringIndex, ph_chars);
                        binaryblock += clipwrapper(va.DanishStringIndex, ph_chars);
                        binaryblock += clipwrapper(va.FinnishStringIndex, ph_chars);
                        binaryblock += clipwrapper(va.DutchStringIndex, ph_chars);
                        binaryblock += clipwrapper(va.NorwegianStringIndex, ph_chars);
                    }
                }
                string sussy_b;
                if (isreach && is_the_annoying_one) // for some reason, reaches thingo uses an extra bit for the mfing char count - part 2
                    sussy_b = BitWriter.clippedtolength(Convert.ToString(hexstringblock.Length / 2, 2), ph_chars + 1); // length // 6 bits instead of 5
                else
                    sussy_b = BitWriter.clippedtolength(Convert.ToString(hexstringblock.Length / 2, 2), ph_chars); // length


                binaryblock += sussy_b;

                if (use_compression)
                {
                    binaryblock += 1;

                    // convert hex string to byte array
                    var benis = Enumerable.Range(0, hexstringblock.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(hexstringblock.Substring(x, 2), 16)).ToArray();
                    // compress byte array
                    var compressed_stuff = z_testing.ZLib.LowLevelCompress(benis);
                    // convert byte array to binary
                    string s = string.Join("", compressed_stuff.Select(x => Convert.ToString(x, 2).PadLeft(8, '0')));

                    // 
                    binaryblock += BitWriter.clippedtolength(Convert.ToString(compressed_stuff.Length, 2), ph_chars);

                    binaryblock += s;
                }
                else
                {
                    binaryblock += 0; 

                    string chars_hexchunk = String.Join(String.Empty, hexstringblock.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));

                    binaryblock += chars_hexchunk;
                }
                

                hexstringblock = null;

                return binaryblock;
            }
            else
            {
                // no strings :megamind:
                string blank_stringtable = BitWriter.clippedtolength(Convert.ToString(0, 2), ph_bits);
                return blank_stringtable;
            }

            // checklist
            //
            // string count
            // string index table
            // string char table length
            // string table iscompressed?
            // strint table 
        }
        public string clipwrapper(int thingo_to_clip, int length)
        {
            if (thingo_to_clip == -1)
                return "0";
            
            string int_binary = BitWriter.clippedtolength(Convert.ToString(thingo_to_clip, 2), length);
            return "1" + int_binary;
        }

        public int stringtablelanguage(string lang_node, XmlNode parent, Dictionary<string, int> bozo_check)
        {
            XmlNode es = parent.SelectSingleNode(lang_node);
            string stringcheck = es.Attributes["v"]?.InnerText;
            if (stringcheck == "True")
            {
                XmlNode ss = es.SelectSingleNode("String");
                string charscheck = ss.Attributes["v"]?.InnerText;
                if (charscheck == null)
                    charscheck = "";
                if (bozo_check.ContainsKey(charscheck))
                {
                    return bozo_check[charscheck];
                }
                else // add a new entry
                {
                    int string_ind = hexstringblock.Length / 2;
                    bozo_check.Add(charscheck, string_ind);

                    byte[] ba = Encoding.Default.GetBytes(charscheck);
                    var hexString = BitConverter.ToString(ba);
                    hexString = hexString.Replace("-", "");
                    hexstringblock += hexString + "00";

                    return string_ind;
                }
            }
            else // "False"
            {
                return -1;
            }
        }

        public void instaniaterwrite(string export, string targethaloxml)
        {
            //xmlWriter = new XmlTextWriter(export, Encoding.UTF8);
            xmlWriter = XmlWriter.Create(export, settings);
            // xmlWriter.Settings = new XmlWriterSettings();
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Gametype");
            string rev_number = check_db_revision(targethaloxml);
            xmlWriter.WriteAttributeString("revision", rev_number);
        }
        public string check_db_revision(string targethaloxml)
        {
            XmlDocument xe = xina(targethaloxml);
            return xe.SelectSingleNode("base").Attributes["revision"]?.InnerText;
        }

        public void enprocess()
        {
            xmlWriter.WriteEndElement();
            xmlWriter.Flush();
            xmlWriter.Close();
        }

        public void writenode(string name, string value)
        {
            string test = String.Concat(name.Where(c => !Char.IsWhiteSpace(c)));

            xmlWriter.WriteStartElement(test);
            if (!string.IsNullOrWhiteSpace(value))
                xmlWriter.WriteAttributeString("v", value);
        }
        public void endnode()
        {
            xmlWriter.WriteEndElement();
        }

        public List<BitReader.paramheader2> returnchildren(string xml)
        {
            List<BitReader.paramheader2> w = new List<BitReader.paramheader2>();
            XmlDocument c = xina(xml);
            XmlNode a = c.SelectSingleNode("/base/ExTypes");
            XmlNodeList b = a.ChildNodes;
            foreach (XmlNode x in a)
            {
                if (x.NodeType != XmlNodeType.Comment)
                w.Add(test(x, xml, "ExTypes"));
            }
            
            return w;
        }
        public XmlDocument xina(string g)
        {
            if (External.ContainsKey(g))
            {
                External.TryGetValue(g, out XmlDocument test);
                return test;
            }
            XmlDocument c = new XmlDocument();
            c.Load(XMLdirectory + g);
            External.Add(g, c);
            return c;
        }

        //
        //
        // proto megalo stuff
        //
        public BitReader.paramheader2 handleVarHeader(string type, string Var, string docx)
        {
            XmlDocument c = xina(docx);

            return (test(c.SelectSingleNode("/base/" + type + "/Var[@name='" + Var + "']"), "", ""));

        }

        public BitReader.paramheader2 test(XmlNode w, string q, string h)
        {
            BitReader.paramheader2 o = new BitReader.paramheader2();
            o.node = q;
            o.node1 = h;
            o.name = w.Attributes["name"]?.InnerText;
            o.type = w.Attributes["type"]?.InnerText;
            o.bits = int.Parse(w.Attributes["bits"]?.InnerText);
            o.offset = (w.Attributes["offset"]?.InnerText != null)? int.Parse(w.Attributes["offset"]?.InnerText): -1;
            o.chars = (w.Attributes["chars"]?.InnerText != null) ? int.Parse(w.Attributes["chars"]?.InnerText) : -1;
            return o;
        }
        //
        //
        //
        public InterpStruct readdata(string nodepath, string doc)
        {
            XmlDocument c = xina(doc);
            XmlNode w = c.SelectSingleNode(nodepath);
            XmlNodeList nl = w.ChildNodes;

            InterpStruct iS = new InterpStruct();
            iS.blockname = w.Attributes["name"]?.InnerText;
            iS.blockparams = new List<InterpStruct.s>();

            
            foreach (XmlNode p in nl)
            {
                if (p.NodeType != XmlNodeType.Comment)
                {
                    InterpStruct.s a = new InterpStruct.s();
                    a.name = p.Attributes["name"]?.InnerText;
                    a.refe = p.Attributes["bits"]?.InnerText;
                    a.type = p.Attributes["type"]?.InnerText;
                    iS.blockparams.Add(a);
                }
            }
            return iS;
        }
        

        //
        //
        // proto syntax stuff
        //
        public string[] fetch_plaintextCode()
        {
            string[] ret = new string[0];












            return ret;
        }



    }
}
