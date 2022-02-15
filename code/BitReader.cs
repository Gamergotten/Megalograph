using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace gamtetyper
{
    public class BitReader
    {
        public int i = 0;
        public XML_Process m_process;
        public string bitclump;

        public string fordump;

        public string infstring;

        public bool skip;

        public void readbin(byte[] fileBytes, string XML)
        {
            
            StringBuilder sb = new StringBuilder();

            foreach (byte b in fileBytes)
            {
                sb.Append(Convert.ToString(b, 2).PadLeft(8, '0'));
            }
            bitclump = sb.ToString();
            processbin(sb.ToString(), XML);
            Console.WriteLine("bits left(" + (bitclump.Length - i) + ")");
            Console.WriteLine(bitclump.Substring(i));
        }

        public void processbin(string c, string path)
        {
            m_process.writenode("base", "");
            foreach (paramheader2 p in m_process.returnchildren(path))
            {
                fordump = "Halo ";
                readblock(p, "ExTypes");
                //runstack();
                Console.WriteLine(fordump);
            }
            m_process.endnode();
        }

        public void readblock(paramheader2 ph, string wrap)
        {
            fordump += "@" + i;
            if (ph.type == "Int")
            {
                string s = bitclumpBIN(ph.bits).ToString();
                m_process.writenode(ph.name, s);
                m_process.endnode();
                fordump += "(" + ph.bits + "-bit " + ph.type + " - " + ph.name + ": " + s  + ") ";
            }
            if (ph.type == "UInt")
            {
                string s = bitclumpBIN(ph.bits).ToString();
                m_process.writenode(ph.name, s);
                m_process.endnode();
                fordump += "(" + ph.bits + "-bit " + ph.type + " - " + ph.name + ": " + s + ") ";
            }
            if (ph.type == "Long")
            {
                string s = bitclumpBIN1(ph.bits).ToString();
                m_process.writenode(ph.name, s);
                m_process.endnode();
                fordump += "(" + ph.bits + "-bit " + ph.type + " - " + ph.name + ": " + s + ") ";
            }
            if (ph.type == "ULong")
            {
                string s = bitclumpBIN1(ph.bits).ToString();
                m_process.writenode(ph.name, s);
                m_process.endnode();
                fordump += "(" + ph.bits + "-bit " + ph.type + " - " + ph.name + ": " + s + ") ";
            }
            if (ph.type == "String")
            {
                string s = bitclumpBIN3(ph.bits);
                m_process.writenode(ph.name, s);
                m_process.endnode();
                fordump += "(" + ph.bits + "-bit " + ph.type + " - " + ph.name + ": " + s + ") ";

            }
            if (ph.type == "String16")
            {
                string s = bitclumpBIN3(ph.bits);
                m_process.writenode(ph.name, s);
                m_process.endnode();
                fordump += "(" + ph.bits + "-bit " + ph.type + " - " + ph.name + ": " + s + ") ";

            }
            if (ph.type == "UString8")
            {
                string s1 = BinaryToString16(bitclumpBIN5());
                string s = s1.Replace("\0\0", "");
                m_process.writenode(ph.name, s);
                m_process.endnode();
                fordump += "(" + ph.bits + "-bit " + ph.type + " - " + ph.name + ": " + s + ") ";
            }
            if (ph.type == "UString16")
            {
                string s1 = BinaryToString16(bitclumpBIN4());
                string s = s1.Replace("\0\0", "");
                m_process.writenode(ph.name, s);
                m_process.endnode();
                fordump += "(" + ph.bits + "-bit " + ph.type + " - " + ph.name + ": " + s + ") ";
            }
            if (ph.type == "Hex")
            {
                string s = bitclumpBIN3(ph.bits);
                m_process.writenode(ph.name, s);
                m_process.endnode();
                fordump += "(" + ph.bits + "-bit " + ph.type + " - " + ph.name + ": " + s + ") ";
            }
            if (ph.type == "Blank")
            {
                string s = bitclumpBIN2(ph.bits);
                m_process.writenode(ph.name, s);
                m_process.endnode();
                fordump += "(" + ph.bits + "-bit " + ph.type + " - " + ph.name + ": " + s + ") ";
            }

            if (ph.type.Contains("Enumref"))
            {
                string[] w = ph.type.Split(":");
                ph.node1 = "RefTypes";
                paramheader2 ph2 = VarInterpret(ph.node1, w[1], ph.node);
                ph2.node = ph.node;
                ph2.node1 = "RefTypes";

                m_process.writenode(ph.name, "");
                readblock(ph2, ph2.node1);
                m_process.endnode();
            }

            if (ph.type == "Enum")
            {
                string i = bitclumpBIN2(ph.bits);
                fordump += "(" + ph.bits + "-bit " + ph.type + " - " + ph.name + ": " + i + ") ";

                if (ph.offset != -1)
                {
                    callstack cw = new callstack();
                    cw.w = m_process.readdata("base/" + wrap + "/Var[@name='" + ph.name + "']/Var[@ID='" + i + "']", ph.node);
                    cw.w.name = ph.name;
                    cw.depth = ph.offset;
                    cw.parentnode = wrap;
                    cw.docx = ph.node;
                    cw.read2 = ph.node1 + "/Var[@name='" + ph.name + "']";
                    stack.Add(cw);
                }
                else
                {
                    InterpStruct e = m_process.readdata("base/" + wrap + "/Var[@name='" + ph.name + "']/Var[@ID='" + i + "']", ph.node);
                    m_process.writenode(ph.name, e.blockname);
                    skip = true;
                    readchildren(e, wrap + "/Var[@name='" + ph.name + "']", ph.node);
                    m_process.endnode();
                    skip = false;
                }

            }
            if (ph.type == "Container")
            {
                m_process.writenode(ph.name, "");
                fordump += "(" + ph.bits + "-bit " + ph.type + " - " + ph.name + ": ) ";
                InterpStruct e = m_process.readdata("/base/" + ph.node1 + "/Var[@name='" + ph.name + "']", ph.node);
                readchildren(e, wrap, ph.node);
                m_process.endnode();
            }
            if (ph.type == "Count")
            {
                m_process.writenode(ph.name, "");
                int m = bitclumpBIN(ph.bits);
                fordump += "(" + ph.bits + "-bit " + ph.type + " - " + ph.name + ": " + m + ") ";
                for (int i = 0; i < m; i += 1)
                {
                     m_process.writenode(ph.name+"-child"+ i, "");
                     InterpStruct e = m_process.readdata("/base/" + ph.node1 + "/Var[@name='" + ph.name + "']", ph.node);
                     readchildren(e, wrap, ph.node);
                     m_process.endnode();
                }
                m_process.endnode();
            }
            if (ph.type == "HCount")
            {
                m_process.writenode(ph.name, "");
                int m = bitclumpBIN(ph.bits);
                m_process.writenode("TableLength", m.ToString());
                m_process.endnode();
                fordump += "(" + ph.bits + "-bit " + ph.type + " - " + ph.name + ": " + m + ") \r\n";
                int d = bitclumpBIN(1);
                m_process.writenode("Compressed", d.ToString());
                fordump += "(" + 1 + "-bit Bool - " + ph.name + ": " + d + ") \r\n";
                if (d == 0)
                {
                    string b = bitclumpBIN3(m * 8);
                    fordump += "(" + 8 + "-bit Hex - " + ph.name + ": " + b + ") \r\n";
                    m_process.writenode("TableBytes", b);
                    m_process.endnode();
                }
                else
                {
                    int m1 = bitclumpBIN(ph.bits);
                    m_process.writenode("CompressedLength", m1.ToString());
                    string b = bitclumpBIN3(m1 * 8);
                    fordump += "(" + m1*8 + "-bit Hex - " + ph.name + ": " + b + ") \r\n";
                    m_process.writenode("CompressedBytes", b);
                    m_process.endnode();
                    m_process.endnode();
                }
                m_process.endnode();
                m_process.endnode();
            }
            if (ph.type.Contains("External"))
            {
                string[] w = ph.type.Split(":");
                m_process.writenode(ph.name, "");
                int w2 = i;
                foreach (paramheader2 p in m_process.returnchildren(w[1]))
                {
                    
                    fordump = "\r\r"+ ph.name+" ";
                    readblock(p, "ExTypes");
                   // runstack();
                    Console.WriteLine(fordump);
                    fordump = "";
                }
                if (ph.bits > 0)
                {
                    int poop = i - w2;
                    i += ph.bits - poop;
                    Console.WriteLine("bits skipped " + (ph.bits - poop));
                    if ((ph.bits - poop) < 0)
                        Console.ReadLine();
                }
                m_process.endnode();
            }
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
                    callstack cs = stack.ElementAt(q);
                    if (cs.depth <= 0)
                    {
                        stack.RemoveAt(q);
                        q--;
                        w2--;
                        skip = true;
                        m_process.writenode(cs.w.name, cs.w.blockname);
                        if (cs.read2 == null)
                            readchildren(cs.w, cs.parentnode, cs.docx);

                        if (cs.read2 != null)
                            readchildren(cs.w, cs.read2, cs.docx);

                        m_process.endnode();
                        skip = false;
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

        public List<callstack> stack = new List<callstack>();

        public struct callstack
        {
            public int depth { get; set; }
            public InterpStruct w { get; set; }
            public string parentnode { get; set; }
            public string read2 { get; set; }
            public string docx { get; set; }
        }

        public static string BinaryToString16(string data)
        {
            //List<Byte> byteList = new List<Byte>();

            //for (int i = 0; i < data.Length; i += 8)
            //{
            //    byteList.Add(Convert.ToByte(data.Substring(i, 8), 2));
            //}
            //return Encoding.ASCII.GetString(byteList.ToArray());
            var hex = string.Join(" ",
            Enumerable.Range(0, data.Length / 8)
            .Select(i => Convert.ToByte(data.Substring(i * 8, 8), 2).ToString("X2")));
            return String.Concat(hex.Where(c => !Char.IsWhiteSpace(c))); ;
        }

        public static string BinaryToString8(string data)
        {
            //List<Byte> byteList = new List<Byte>();

            //for (int i = 0; i < data.Length; i += 8)
            //{
            //    byteList.Add(Convert.ToByte(data.Substring(i, 8), 2));
            //}
            //return Encoding.UTF8.GetString(byteList.ToArray());
            var hex = string.Join(" ",
            Enumerable.Range(0, data.Length / 8)
            .Select(i => Convert.ToByte(data.Substring(i * 8, 8), 2).ToString("X2")));
            return String.Concat(hex.Where(c => !Char.IsWhiteSpace(c))); ;
        }

        public int bitclumpBIN(int length) //count forward and convert binary to integer
        {
            string n = bitclump.Substring(i, length);
            i += length;
            return Convert.ToInt32(n, 2);
        }
        public long bitclumpBIN1(int length) //count forward and convert binary to long
        {
            string n = bitclump.Substring(i, length);
            i += length;
            return Convert.ToInt64(n, 2);
        }
        public string bitclumpBIN2(int length) //count forward dont convert binary
        {
            string n = bitclump.Substring(i, length);
            i += length;
            return n;
        }

        public string bitclumpBIN3(int length) //count forward and convert binary to hex
        {
            string n = bitclump.Substring(i, length);
            var hex = string.Join(" ",
            Enumerable.Range(0, n.Length / 8)
            .Select(i => Convert.ToByte(n.Substring(i * 8, 8), 2).ToString("X2")));

            i += length;
            return String.Concat(hex.Where(c => !Char.IsWhiteSpace(c)));
        }

        public string bitclumpBIN4() //count forward and locate binary string
        {
            infstring = "";
            continuehunt();
            return infstring;
        }
        void continuehunt()
        {
            string n = bitclump.Substring(i, 16);
            i += 16;
            infstring += n;
            //Console.WriteLine(n);
            if (n != "0000000000000000")
                continuehunt();
        }
        public string bitclumpBIN5() //count forward and locate binary string
        {
            infstring = "";
            continuehunt8();
            return infstring;
        }
        void continuehunt8()
        {
            string n = bitclump.Substring(i, 8);
            i += 8;
            infstring += n;
            //Console.WriteLine(n);
            if (n != "00000000")
                continuehunt8();
        }


        public void readchildren(InterpStruct e, string parentloc, string docx)
        {
            fordump += "\r\n" + (e.blockname) + "-child: ";
            if (e.blockparams != null && e.blockparams.Count > 0) // simplify pls
            {
                foreach (InterpStruct.s w in e.blockparams)
                {
                    paramheader2 ph = VarInterpret(parentloc + "/Var[@name='" + e.blockname + "']" + "", w.name, docx);
                    ph.node = docx;
                    ph.node1 = parentloc + "/Var[@name='" + e.blockname + "']";
                    readblock(ph, parentloc + "/Var[@name='" + e.blockname + "']");
                }
            }
            fordump += "\r\n";
        }

        public paramheader2 VarInterpret(string type, string lookfor, string docx) // need to fix this to use a shared param rather than an array
        {
            paramheader2 m = m_process.handleVarHeader(type, lookfor, docx);
            return m;
        }


        struct parambase
        {
            public paramheader header { get; set; }
            public List<param> paramaters { get; set; }
        }


        public struct paramheader
        {
            public string name { get; set; }
            public string type { get; set; }
            public int bits { get; set; }
            
        }

        public struct paramheader2
        {
            public string node { get; set; }
            public string node1 { get; set; }
            public string name { get; set; }
            public string type { get; set; }
            public int bits { get; set; }
            public int offset { get; set; }

        }

        struct param
        {
            public string type { get; set; }
            public string value_real { get; set; }
        }


    }
}
