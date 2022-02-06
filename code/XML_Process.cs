using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using static gamtetyper.BitInterper;
using System.Xml.Linq;
using static gamtetyper.Gametype;
using static gamtetyper.BitReader;
using System.Diagnostics;
using static gamtetyper.BitWriter;
using System.Collections;
using System.Windows.Controls.Primitives;

namespace gamtetyper
{


    class XML_Process
    {

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

        public List<Gametype.trigger> returnScripts(bool is_reach)
        {
            List<Gametype.trigger> triggers = new List<Gametype.trigger>();
            

            XmlNode a = XMLdump.SelectSingleNode("/Gametype/base/mpvr/megl/MegaloScript/TriggerCount");
            XmlNodeList b = a.ChildNodes;

            int i = 0;
            foreach (XmlNode xmln in b) 
            {

                string node = xmln.Name;

                Gametype.trigger curr_trigg = new Gametype.trigger();

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
                string a_xml = @"\Halo 2A\var enums.xml";
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
                string t_xml = @"\Halo 2A\var enums.xml"; // how do i mark this so i can fix it later
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
                XmlNodeList Actions = XMLdump.SelectSingleNode("/Gametype/base/mpvr/megl/MegaloScript/ActionCount").ChildNodes;

                curr_trigg.Actions_insert = actions_offset;
                curr_trigg.Actions_count = total_actions;

                //
                // COPY ACTIONS FOR CONDITIONS
                int total_conditions = int.Parse(XMLdump.SelectSingleNode("/Gametype/base/mpvr/megl/MegaloScript/TriggerCount/" + node + "/ConditionCount").Attributes["v"].InnerText);

                int conditions_offset = int.Parse(XMLdump.SelectSingleNode("/Gametype/base/mpvr/megl/MegaloScript/TriggerCount/" + node + "/ConditionOffset").Attributes["v"].InnerText);
                if (!is_reach) conditions_offset -= 1;
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

        public List<Gametype.action> doactions()
        {
            XmlNodeList Actions = XMLdump.SelectSingleNode("/Gametype/base/mpvr/megl/MegaloScript/ActionCount").ChildNodes;

            List<action> ActionDump = new List<action>();

            for (int Index = 0; Index < Actions.Count; Index++)
            {
                action newaction = new action();

                XmlNode w = Actions[Index].SelectSingleNode("Type");




                // grab our ph
                string xml = @"\Halo 2A\var enums.xml";
                XmlDocument c = xina(xml);
                XmlNode o = c.SelectSingleNode("/base/ExTypes/Var[@name='MegaloScript']/Var[@name='ActionCount']/Var[@name='Type']");
                paramheader2 ph2 = test(o, xml, "ExTypes");

                newaction.Type = readblock(ph2, "ExTypes/Var[@name='MegaloScript']/Var[@name='ActionCount']", new List<string> { "mpvr", "megl", "MegaloScript", "ActionCount", w.ParentNode.Name });




                ActionDump.Add(newaction);
            }

            return ActionDump;
        }

        public List<Gametype.condition> doconditions()
        {
            XmlNodeList Conditions = XMLdump.SelectSingleNode("/Gametype/base/mpvr/megl/MegaloScript/ConditionCount").ChildNodes;

            List<condition> ConditionsDump = new List<condition>();

            for (int Index = 0; Index < Conditions.Count; Index++)
            {
                condition newcondition = new condition();

                XmlNode w = Conditions[Index].SelectSingleNode("Type");

                // grab our ph
                string xml = @"\Halo 2A\var enums.xml";
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
                string parnode = info.FUCKK_YOU.Replace(" ", "");
                XmlNode enumref_parent = XMLdump.CreateNode("element", parnode, "");
                parent.AppendChild(enumref_parent);
                parent = enumref_parent;
            }

            XmlNode child = XMLdump.CreateNode("element", nodename, "");

            XmlAttribute Attr = XMLdump.CreateAttribute("v");
            Attr.Value = info.V;
            child.Attributes.Append(Attr); // surely this wont work, its set specifically as "get"

            parent.AppendChild(child);
            if (info.Params != null)
            {
                for (int i = 0; i < info.Params.Count; i++)
                {
                    append_children_from_ebum_export(child, info.Params[i]);
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
                XmlNode trighead = XMLdump.CreateNode("element", "TriggerCount-child"+Index, "");
                a.AppendChild(trighead);

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
            XMLdump.Save(XMLdump_directory);
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

                append_children_from_ebum_export(acthead, action.Type);
            }
            XMLdump.Save(XMLdump_directory);
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

                create_and_embed_node_w_attribute("NOT", cond.Not.ToString(), condhead);

                create_and_embed_node_w_attribute("ORsequence", cond.OR_Group.ToString(), condhead);

                create_and_embed_node_w_attribute("ConditionOffset", cond.insertionpoint.ToString(), condhead);

                append_children_from_ebum_export(condhead, cond.Type);
            }
            XMLdump.Save(XMLdump_directory);
        }
























        public Ebum readblock(paramheader2 ph, string wrap, List<string> node)
        {
            Ebum t = new Ebum();
            //t.V = ph.name;
            node.Add(ph.name);
            t.Name = node[node.Count-1];
            t.XMLDoc = ph.node;

            t.XMLPath = wrap + "/ Var[@name = '" + ph.name + "']";
            //t.XMLPath = ph.WHERE_WE_FUCKING_FOUND_IT;
            t.Type = ph.type;

            //XmlDocument i = xina(ph.node);
            //XmlNode xnfdsahghvbgdsavghgvhdtf = i.SelectSingleNode(t.XMLPath);

            t.Size = ph.bits; // int.Parse(xnfdsahghvbgdsavghgvhdtf.Attributes["bits"]?.InnerText);

            if (ph.type == "Int")
            {
                t.V = returnfromdump(node);

                node.RemoveAt(node.Count - 1);
                return t;
            }
            if (ph.type == "UInt")
            {
                t.V = returnfromdump(node);

                node.RemoveAt(node.Count - 1);
                return t;
            }
            if (ph.type == "Long")
            {
                t.V = returnfromdump(node);

                node.RemoveAt(node.Count - 1);
                return t;
            }
            if (ph.type == "ULong")
            {
                t.V = returnfromdump(node);

                node.RemoveAt(node.Count - 1);
                return t;
            }
            if (ph.type == "String")
            {
                t.V = returnfromdump(node);

                node.RemoveAt(node.Count - 1);
                return t;
            }
            if (ph.type == "String16")
            {
                t.V = returnfromdump(node);

                node.RemoveAt(node.Count - 1);
                return t;
            }
            if (ph.type == "UString8")
            {
                t.V = returnfromdump(node);


                node.RemoveAt(node.Count - 1);
                return t;
            }
            if (ph.type == "UString16")
            {
                t.V = returnfromdump(node);

                node.RemoveAt(node.Count - 1);      
                return t;
            }
            if (ph.type == "Hex")
            {
                t.V = returnfromdump(node);

                node.RemoveAt(node.Count - 1);
                return t;

            }
            if (ph.type == "Blank")
            {
                t.V = returnfromdump(node);

                node.RemoveAt(node.Count - 1);
                return t;
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

                //node.RemoveAt(node.Count - 1); // FUCK YOU PREVIOUS ME
                Ebum e3 = readblock(ph2, ph2.node1, node);
                node.RemoveAt(node.Count - 1);
                e3.FUCKK_YOU = ph.name;
                if (ph.name == "Bool1")
                {
                    string DEBUGTHESEBALLS = "";
                }
                return e3;
            }

            if (ph.type == "Enum")
            {
                string test = returnfromdump(node);
                if (test == null)
                {
                    XmlDocument c = xina(ph.node);
                    XmlNode w = c.SelectSingleNode("base/" + wrap + "/Var[@name='" + ph.name + "']");
                    test = w.ChildNodes[0].Attributes["name"]?.InnerText;
                }

                string y = "base/" + wrap + "/Var[@name='" + ph.name + "']/Var[@name='" + test + "']";

                // i think we need to read the value of the enum and use this here

                InterpStruct e = readdata(y, ph.node);
                t.V = test;
                // append the ID to our list... might needa fetch it first
                t.Params = readchildren(e, wrap + "/Var[@name='" + ph.name + "']", ph.node, node);

            }
            if (ph.type == "Container")
            {
                // i think this should just work
                InterpStruct e = readdata("/base/" + ph.node1 + "/Var[@name='" + ph.name + "']", ph.node);
                t.Params = readchildren(e, wrap, ph.node, node);
            }
            if (ph.type == "Count")
            {
                //// convert int to binary and append to compile
                //int m = m_process.numbofchild(node);
                //string v = clippedtolength(Convert.ToString(m, 2), ph.bits);

                //binarytogo += v; // int to binary
                //// count children
                //for (int i = 0; i < m; i++)
                //{
                //    node.Add(ph.name + "-child" + i);
                //    InterpStruct e = m_process.readdata("/base/" + ph.node1 + "/Var[@name='" + ph.name + "']", ph.node);
                //    readchildren(e, wrap, ph.node, node);
                //    node.RemoveAt(node.Count - 1);
                //}
            }
            if (ph.type.Contains("External"))
            {
                string[] w = ph.type.Split(":");
                t.Params = new List<Ebum>();
                
                foreach (paramheader2 p in returnchildren(w[1]))
                {
                    t.Params.Add(readblock(p, "ExTypes", node));
                }

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

            return XMLdump.SelectSingleNode(URItogo).ChildNodes.Count;
        }

        public void intakeDecompiledmode(string s)
        {
            XMLdump = new XmlDocument();
            XMLdump.Load(s);
            XMLdump_directory = s;
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

        public void instaniaterwrite(string export)
        {
            //xmlWriter = new XmlTextWriter(export, Encoding.UTF8);
            xmlWriter = XmlWriter.Create(export, settings);
            // xmlWriter.Settings = new XmlWriterSettings();
            xmlWriter.WriteStartDocument();
            xmlWriter.WriteStartElement("Gametype");
            
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
        
    }
}
