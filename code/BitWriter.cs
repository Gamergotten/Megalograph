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

        public bool isreach;

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

        public static string ToHexString8(string str)
        {
            var sb = new StringBuilder();

            var bytes = Encoding.UTF8.GetBytes(str);
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString(); // returns: "48656C6C6F20776F726C64" for "Hello world"
        }
        public static string ToHexString16(string str)
        {
            var sb = new StringBuilder();

            var bytes = Encoding.BigEndianUnicode.GetBytes(str);
            foreach (var t in bytes)
            {
                sb.Append(t.ToString("X2"));
            }

            return sb.ToString(); // returns: "48656C6C6F20776F726C64" for "Hello world"
        }

        public void readblock(paramheader2 ph, string wrap, List<string> node)
        {
            node.Add(ph.name);
            switch (ph.type)
            {
                case "Int":
                    string test12 = m_process.returnfromdump(node);
                    if (test12 != null)
                    {
                        int e = int.Parse(test12); // string to int
                        string v = clippedtolength(Convert.ToString(e, 2), ph.bits);
                        binarytogo += v; // int to binary
                    }
                    else
                    {
                        binarytogo += new string('0', ph.bits);
                    }
                    break;
                case "UInt":
                    string test11 = m_process.returnfromdump(node);
                    if (test11 != null)
                    {
                        // not implemented
                        int e = int.Parse(test11); // string to int
                        string v = clippedtolength(Convert.ToString(e, 2), ph.bits);
                        binarytogo += v; // int to binary
                    }
                    else
                    {
                        binarytogo += new string('0', ph.bits);
                    }
                    break;
                case "Long":
                    string test10 = m_process.returnfromdump(node);
                    if (test10 != null)
                    {
                        long e = int.Parse(test10); // string to long?
                        string v = clippedtolength(Convert.ToString(e, 2), ph.bits);
                        binarytogo += v; // long to binary
                    }
                    else
                    {
                        binarytogo += new string('0', ph.bits);
                    }
                    break;
                case "ULong":
                    string test9 = m_process.returnfromdump(node);
                    if (test9 != null)
                    {
                        //not implemented
                        long e = int.Parse(test9); // string to long?
                        string v = clippedtolength(Convert.ToString(e, 2), ph.bits);

                        binarytogo += v; // long to binary
                    }
                    else
                    {
                        binarytogo += new string('0', ph.bits);
                    }
                    break;
                case "String":
                    // assuming our strings are hex and not actually strings
                    string test8 = m_process.returnfromdump(node);
                    if (test8 != null)
                    {
                        string v = clippedtobackwardslength(String.Join(String.Empty, ToHexString8(test8).Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0'))), ph.bits);

                        binarytogo += v;
                    }
                    else
                    {
                        binarytogo += new string('0', ph.bits);
                    }
                    break;
                case "String16":
                    // we dont even use this lmao // my balls
                    string test7 = m_process.returnfromdump(node);
                    if (test7 != null)
                    {
                        string v = clippedtobackwardslength(String.Join(String.Empty, ToHexString16(test7).Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0'))), ph.bits);

                        binarytogo += v;
                    }
                    else
                    {
                        binarytogo += new string('0', ph.bits);
                    }
                    break;
                case "UString8":
                    string test6 = m_process.returnfromdump(node);
                    if (test6 != null)
                    {
                        // doesnt have a binary count
                        binarytogo += String.Join(String.Empty, ToHexString8(test6).Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0'))) + new string('0', 8);
                    }
                    else // this has to be "0000 0000"
                    {
                        binarytogo += new string('0', 8);
                    }
                    break;
                case "UString16":
                    // doesnt have a binary count
                    string test5 = m_process.returnfromdump(node);
                    if (test5 != null)
                    {
                        // doesnt have a binary count
                        binarytogo += String.Join(String.Empty, ToHexString16(test5).Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0'))) + new string('0', 16);
                    }
                    else
                    {
                        binarytogo += new string('0', 16);
                    }
                    break;
                case "Hex":
                    string test4 = m_process.returnfromdump(node);
                    if (test4 != null)
                    {
                        // does the same thing as strings because we're gonna pretend that they're all hex.
                        string v3 = clippedtolength(String.Join(String.Empty, m_process.returnfromdump(node).Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0'))), ph.bits);

                        binarytogo += v3;
                    }
                    else
                    {
                        binarytogo += new string('0', ph.bits);
                    }
                    break;
                case "Blank":
                    string test3 = m_process.returnfromdump(node);
                    if (test3 != null)
                    {
                        string v = clippedtolength(test3, ph.bits);

                        binarytogo += v;
                    }
                    else
                    {
                        binarytogo += new string('0', ph.bits);
                    }
                    break;
                case "Enum":
                    string test2 = m_process.returnfromdump(node);
                    string y3 = "base/" + wrap + "/Var[@name='" + ph.name + "']/Var[@name='" + test2 + "']";
                    if (ph.offset != -1)
                    {
                        // this will need some figuring out to get working
                        callstack2 cw = new callstack2();
                        cw.w = m_process.readdata(y3, ph.node);
                        binarytogo += m_process.fetchenumID(y3, ph.node);
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
                        InterpStruct e4 = m_process.readdata(y3, ph.node);
                        // append the ID to our list... might needa fetch it first
                        skip = true;
                        binarytogo += m_process.fetchenumID(y3, ph.node);
                        readchildren(e4, wrap + "/Var[@name='" + ph.name + "']", ph.node, node);
                        skip = false;
                    }
                    break;
                case "Container":
                    // i think this should just work
                    InterpStruct e3 = m_process.readdata("/base/" + ph.node1 + "/Var[@name='" + ph.name + "']", ph.node);
                    readchildren(e3, wrap, ph.node, node);
                    break;
                case "Count":
                    // convert int to binary and append to compile
                    int m2 = m_process.numbofchild(node);
                    string v2 = clippedtolength(Convert.ToString(m2, 2), ph.bits);

                    binarytogo += v2; // int to binary
                                     // count children
                    for (int i3 = 0; i3 < m2; i3++)
                    {
                        node.Add(ph.name + "-child" + i3);
                        InterpStruct e2 = m_process.readdata("/base/" + ph.node1 + "/Var[@name='" + ph.name + "']", ph.node);
                        readchildren(e2, wrap, ph.node, node);
                        node.RemoveAt(node.Count - 1);
                    }
                    break;
                case "HCount":
                    node.Add("UseCompression");
                    string Is_using_compression = m_process.returnfromdump(node); // currently useless
                    node.RemoveAt(node.Count - 1);
                    bool compress_it = false;
                    if (Is_using_compression == "True")
                        compress_it = true;

                    node.Add("Strings");
                    binarytogo += m_process.returnstringtable_to_binary(node, ph.bits, ph.chars ,isreach, compress_it, ph.name == "Teamstring");
                    node.RemoveAt(node.Count - 1);
                    break;
                default:
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
                    if (ph.type.Contains("Enumref"))
                    {
                        // should just work tbh
                        string[] w3 = ph.type.Split(":");
                        ph.node1 = "RefTypes";
                        paramheader2 ph2 = VarInterpret(ph.node1, w3[1], ph.node);
                        ph2.node = ph.node;
                        ph2.node1 = "RefTypes";

                        readblock(ph2, ph2.node1, node);
                    }
                    break;

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

        public static string clippedtolength(string clip, int length)
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
        public static string clippedtobackwardslength(string clip, int length)
        {
            string c = clip;
            if (c.Length < length)
            {
                c += new string('0', length -= c.Length);
            }
            else if (c.Length > length)
            {
                c = c.Substring(0, length-1);
            }
            return c;
        }
    }
}
