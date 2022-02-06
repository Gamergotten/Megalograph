using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static gamtetyper.BitReader;
using static System.Net.Mime.MediaTypeNames;

namespace gamtetyper
{
    class BitWriter
    {
        public XML_Process m_process;

        public string output = "";

        public string binarytogo = "";

        public bool skip;

        public byte[] writebin(string XML)
        {
            processbin(XML);
            //string hex = string.Join("",
            //Enumerable.Range(0, binarytogo.Length / 8)
            //.Select(i => Convert.ToByte(binarytogo.Substring(i * 8, 8), 2).ToString("X2")));

            ////string x = String.Concat(hex.Where(c => !Char.IsWhiteSpace(c)));

            //string[] s = hex.Split(" ");


            int numOfBytes = binarytogo.Length / 8;
            byte[] bytes = new byte[numOfBytes];
            for (int i = 0; i < numOfBytes; ++i)
            {
                bytes[i] = Convert.ToByte(binarytogo.Substring(8 * i, 8), 2);
            }

            return bytes;
        }

        public void processbin(string path)
        {
            List<string> nodes = new List<string>();
            foreach (paramheader2 p in m_process.returnchildren(path))
            {
                readblock(p, "ExTypes", nodes);
            }
        }

        public void readblock(paramheader2 ph, string wrap, List<string> node)
        {
            node.Add(ph.name);
            if (ph.type == "Int")
            {
                string test = m_process.returnfromdump(node);
                if (test != null)
                {
                    int e = int.Parse(test); // string to int

                    string v = clippedtolength(Convert.ToString(e, 2), ph.bits);
                    
                    binarytogo += v; // int to binary
                }
                else
                {
                    binarytogo += new string('0', ph.bits);
                }


            }
            if (ph.type == "UInt")
            {
                string test = m_process.returnfromdump(node);
                if (test != null)
                {
                    // not implemented
                    int e = int.Parse(test); // string to int

                    string v = clippedtolength(Convert.ToString(e, 2), ph.bits);
                    
                    binarytogo += v; // int to binary
                }
                else
                {
                    binarytogo += new string('0', ph.bits);
                }
            }
            if (ph.type == "Long")
            {
                string test = m_process.returnfromdump(node);
                if (test != null)
                {
                    long e = int.Parse(test); // string to long?
                    string v = clippedtolength(Convert.ToString(e, 2), ph.bits);
                    
                    binarytogo += v; // long to binary
                }
                else
                {
                    binarytogo += new string('0', ph.bits);
                }
            }
            if (ph.type == "ULong")
            {
                string test = m_process.returnfromdump(node);
                if (test != null)
                {
                    //not implemented
                    long e = int.Parse(test); // string to long?
                    string v = clippedtolength(Convert.ToString(e, 2), ph.bits);
                    
                    binarytogo += v; // long to binary
                }
                else
                {
                    binarytogo += new string('0', ph.bits);
                }
            }
            if (ph.type == "String")
            {
                // assuming our strings are hex and not actually strings
                string test = m_process.returnfromdump(node);
                if (test != null)
                {
                    string v = clippedtolength(String.Join(String.Empty, test.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0'))), ph.bits);
                    
                    binarytogo += v;
                }
                else
                {
                    binarytogo += new string('0', ph.bits);
                }
            }
            if (ph.type == "String16")
            {
                // we dont even use this lmao
                string test = m_process.returnfromdump(node);
                if (test != null)
                {
                    string v = clippedtolength(String.Join(String.Empty, test.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0'))), ph.bits);
                    
                    binarytogo += v;
                }
                else
                {
                    binarytogo += new string('0', ph.bits);
                }
            }
            if (ph.type == "UString8")
            {
                string test = m_process.returnfromdump(node);
                if (test != null)
                {
                    // doesnt have a binary count
                    binarytogo += String.Join(String.Empty, test.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
                }
                else
                {
                    binarytogo += new string('0', ph.bits);
                }
            }
            if (ph.type == "UString16")
            {
                // doesnt have a binary count
                string test = m_process.returnfromdump(node);
                if (test != null)
                {
                    // doesnt have a binary count
                    binarytogo += String.Join(String.Empty, test.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
                }
                else
                {
                    binarytogo += new string('0', ph.bits);
                }
            }
            if (ph.type == "Hex")
            {
                string test = m_process.returnfromdump(node);
                if (test != null)
                {
                    // does the same thing as strings because we're gonna pretend that they're all hex.
                    string v = clippedtolength(String.Join(String.Empty, m_process.returnfromdump(node).Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0'))), ph.bits);
                
                    binarytogo += v;
                }
                else
                {
                    binarytogo += new string('0', ph.bits);
                }

            }
            if (ph.type == "Blank")
            {
                string test = m_process.returnfromdump(node);
                if (test != null)
                {
                    string v = clippedtolength(test, ph.bits);
                    
                    binarytogo += v;
                }
                else
                {
                    binarytogo += new string('0', ph.bits);
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

                readblock(ph2, ph2.node1, node);
            }

            if (ph.type == "Enum")
            {
                string test = m_process.returnfromdump(node);
                string y = "base/" + wrap + "/Var[@name='" + ph.name + "']/Var[@name='" + test + "']";
                if (ph.offset != -1)
                {
                    // this will need some figuring out to get working
                    callstack2 cw = new callstack2();
                    cw.w = m_process.readdata(y, ph.node);
                    binarytogo += m_process.fetchenumID(y, ph.node);
                    cw.w.name = ph.name;
                    cw.depth = ph.offset;
                    cw.parentnode = wrap;
                    cw.docx = ph.node;
                    cw.read2 = ph.node1 + "/Var[@name='" + ph.name + "']";
                    cw.nnodes = node;
                    stack.Add(cw);
                }
                else
                {
                    // i think we need to read the value of the enum and use this here
                    
                    InterpStruct e = m_process.readdata(y, ph.node);
                    // append the ID to our list... might needa fetch it first
                    skip = true;
                    binarytogo += m_process.fetchenumID(y, ph.node);
                    readchildren(e, wrap + "/Var[@name='" + ph.name + "']", ph.node, node);
                    skip = false;
                }

            }
            if (ph.type == "Container")
            {
                // i think this should just work
                InterpStruct e = m_process.readdata("/base/" + ph.node1 + "/Var[@name='" + ph.name + "']", ph.node);
                readchildren(e, wrap, ph.node, node);
            }
            if (ph.type == "Count")
            {
                // convert int to binary and append to compile
                int m = m_process.numbofchild(node);
                string v = clippedtolength(Convert.ToString(m, 2), ph.bits);
                
                binarytogo += v; // int to binary
                // count children
                for (int i = 0; i < m; i ++)
                {
                    node.Add(ph.name + "-child" + i);
                    InterpStruct e = m_process.readdata("/base/" + ph.node1 + "/Var[@name='" + ph.name + "']", ph.node);
                    readchildren(e, wrap, ph.node, node);
                    node.RemoveAt(node.Count - 1);
                }
            }
            if (ph.type == "HCount")
            {
                node.Add("TableLength");

                string test = m_process.returnfromdump(node);
                int e = int.Parse(test); // string to int
                string v = clippedtolength(Convert.ToString(e, 2), ph.bits);
                
                binarytogo += v;
                node.RemoveAt(node.Count - 1);

                node.Add("Compressed");
                string m = m_process.returnfromdump(node);
                binarytogo += m;

                if (m == "0")
                {
                    node.Add("TableBytes");
                    // does the same thing as strings because we're gonna pretend that they're all hex.
                    string w = String.Join(String.Empty, m_process.returnfromdump(node).Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
                    
                    binarytogo += w;
                    node.RemoveAt(node.Count - 1);
                }
                else
                {
                    node.Add("CompressedLength");
                    node.Add("CompressedBytes");
                    // write length in a sec
                    string l = m_process.returnfromdump(node);
                    string w = String.Join(String.Empty, l.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));

                    int t = l.Length / 2; 
                    string intq = clippedtolength(Convert.ToString(t, 2), ph.bits);
                    
                    binarytogo += intq;
                    binarytogo += w;
                    node.RemoveAt(node.Count - 1);
                    node.RemoveAt(node.Count - 1);
                }
                node.RemoveAt(node.Count - 1);

            }
            if (ph.type.Contains("External"))
            {
                string[] w = ph.type.Split(":");
                int w2 = binarytogo.Length;
                foreach (paramheader2 p in m_process.returnchildren(w[1]))
                {
                    readblock(p, "ExTypes", node);
                }
                if (ph.bits > 0)
                {
                    int poop = binarytogo.Length - w2;
                    int freespace = ph.bits - poop;
                    //for (int o = 0; o < freespace; o++ )
                    //{
                    //    binarytogo += "0";
                    //}
                    binarytogo += new string('0', freespace);
                    Console.WriteLine("bits skipped " + (freespace));
                    if ((freespace) < 0)
                        Console.ReadLine();
                }
            }
            node.RemoveAt(node.Count - 1);

            if (!skip)
                runstack();

        }
         void runstack()
         {
             int w2 = stack.Count;
             for (int q = 0; q < w2; q++)
             {
                 if (stack.Count > 0)
                 {
                     callstack2 cs = stack.ElementAt(q);
                     if (cs.depth <= 0)
                     {
                        cs.nnodes.Add(cs.w.name);
                         stack.RemoveAt(q);
                         q--;
                         w2--;
                         skip = true;
                         if (cs.read2 == null)
                             readchildren(cs.w, cs.parentnode, cs.docx, cs.nnodes);

                         if (cs.read2 != null)
                             readchildren(cs.w, cs.read2, cs.docx, cs.nnodes);
                         skip = false;
                         cs.nnodes.RemoveAt(cs.nnodes.Count - 1);
                     }
                     else
                     {
                         stack.RemoveAt(q);
                         cs.depth -= 1;
                         stack.Insert(q, cs);
                     }
                 }

             }
         }

        public List<callstack2> stack = new List<callstack2>();
        public struct callstack2
        {
            public int depth { get; set; }
            public InterpStruct w { get; set; }
            public string parentnode { get; set; }
            public string read2 { get; set; }
            public string docx { get; set; }
            public List<string> nnodes { get; set; }
        }

        public paramheader2 VarInterpret(string type, string lookfor, string docx) // need to fix this to use a shared param rather than an array
        {
            paramheader2 m = m_process.handleVarHeader(type, lookfor, docx);
            return m;
        }

        public void readchildren(InterpStruct e, string parentloc, string docx, List<string> nodes)
        {
            if (e.blockparams != null && e.blockparams.Count > 0) // simplify pls
            {
                foreach (InterpStruct.s w in e.blockparams)
                {
                    paramheader2 ph = VarInterpret(parentloc + "/Var[@name='" + e.blockname + "']" + "", w.name, docx);
                    ph.node = docx;
                    ph.node1 = parentloc + "/Var[@name='" + e.blockname + "']";
                    readblock(ph, parentloc + "/Var[@name='" + e.blockname + "']", nodes);
                }
            }
 
        }

        public string clippedtolength(string clip, int length)
        {
            // prolly dont need a new string here
            string c = clip;
            if (c.Length < length)
            {
                c = new string('0', length -= c.Length) + c;
            }
            else if (c.Length > length)
            {
                c = c.Substring(c.Length - length);
            }
            return c;
        }
    }
}
