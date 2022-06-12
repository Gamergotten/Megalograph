using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using gamtetyper.code;

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

        public bool isreach;

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
                //Console.WriteLine(fordump);
            }
            m_process.endnode();
        }

        public void readblock(paramheader2 ph, string wrap)
        {
            fordump += "@" + i;

            switch (ph.type)
            {
                case "Int":
                    string s1 = bitclumpBIN(ph.bits).ToString();
                    m_process.writenode(ph.name, s1);
                    m_process.endnode();
                    fordump += "(" + ph.bits + "-bit " + ph.type + " - " + ph.name + ": " + s1 + ") ";
                    break;
                case "UInt":
                    string s2 = bitclumpBIN(ph.bits).ToString();
                    m_process.writenode(ph.name, s2);
                    m_process.endnode();
                    fordump += "(" + ph.bits + "-bit " + ph.type + " - " + ph.name + ": " + s2 + ") ";
                    break;
                case "Long":
                    string s3 = bitclumpBIN1(ph.bits).ToString();
                    m_process.writenode(ph.name, s3);
                    m_process.endnode();
                    fordump += "(" + ph.bits + "-bit " + ph.type + " - " + ph.name + ": " + s3 + ") ";
                    break;
                case "ULong":
                    string s4 = bitclumpBIN1(ph.bits).ToString();
                    m_process.writenode(ph.name, s4);
                    m_process.endnode();
                    fordump += "(" + ph.bits + "-bit " + ph.type + " - " + ph.name + ": " + s4 + ") ";
                    break;
                case "String":
                    string s5 = bitclumpBIN3(ph.bits);
                    string s5_1 = FromHexString8(s5.Replace("00", ""));
                    m_process.writenode(ph.name, s5_1);
                    m_process.endnode();
                    fordump += "(" + ph.bits + "-bit " + ph.type + " - " + ph.name + ": " + s5 + ") ";
                    break;
                case "String16":
                    string s6 = bitclumpBIN3(ph.bits);
                    string s6_1 = FromHexString16(s6.Replace("0000", ""));
                    m_process.writenode(ph.name, s6_1);
                    m_process.endnode();
                    fordump += "(" + ph.bits + "-bit " + ph.type + " - " + ph.name + ": " + s6 + ") ";
                    break;
                case "UString8":
                    string s7 = BinaryToString16(bitclumpBIN5());
                    string s7_1 = FromHexString8(s7.Replace("00", ""));
                    m_process.writenode(ph.name, s7_1);
                    m_process.endnode();
                    fordump += "(" + ph.bits + "-bit " + ph.type + " - " + ph.name + ": " + s7 + ") ";
                    break;
                case "UString16":
                    string s8 = BinaryToString16(bitclumpBIN4());
                    string s8_1 = FromHexString16(s8.Replace("0000", ""));
                    m_process.writenode(ph.name, s8_1);
                    m_process.endnode();
                    fordump += "(" + ph.bits + "-bit " + ph.type + " - " + ph.name + ": " + s8 + ") ";
                    break;
                case "Hex":
                    string s9 = bitclumpBIN3(ph.bits);
                    m_process.writenode(ph.name, s9);
                    m_process.endnode();
                    fordump += "(" + ph.bits + "-bit " + ph.type + " - " + ph.name + ": " + s9 + ") ";
                    break;
                case "Blank":
                    string s10 = bitclumpBIN2(ph.bits);
                    m_process.writenode(ph.name, s10);
                    m_process.endnode();
                    fordump += "(" + ph.bits + "-bit " + ph.type + " - " + ph.name + ": " + s10 + ") ";
                    break;
                case "Enum":
                    // enum
                    // this'll be interesting
                    string i1 = bitclumpBIN2(ph.bits);
                    fordump += "(" + ph.bits + "-bit " + ph.type + " - " + ph.name + ": " + i1 + ") ";

                    if (ph.offset != -1)
                    {
                        callstack cw = new callstack();
                        cw.w = m_process.readdata("base/" + wrap + "/Var[@name='" + ph.name + "']/Var[@ID='" + i1 + "']", ph.node);
                        cw.w.name = ph.name;
                        cw.depth = ph.offset;
                        cw.parentnode = wrap;
                        cw.docx = ph.node;
                        cw.read2 = ph.node1 + "/Var[@name='" + ph.name + "']";
                        stack.Add(cw);
                    }
                    else
                    {
                        InterpStruct e = m_process.readdata("base/" + wrap + "/Var[@name='" + ph.name + "']/Var[@ID='" + i1 + "']", ph.node);
                        m_process.writenode(ph.name, e.blockname);
                        skip = true;
                        readchildren(e, wrap + "/Var[@name='" + ph.name + "']", ph.node);
                        m_process.endnode();
                        skip = false;
                    }
                    break;
                case "Container":
                    // container
                    m_process.writenode(ph.name, "");
                    fordump += "(" + ph.bits + "-bit " + ph.type + " - " + ph.name + ": ) ";
                    InterpStruct e2 = m_process.readdata("/base/" + ph.node1 + "/Var[@name='" + ph.name + "']", ph.node);
                    readchildren(e2, wrap, ph.node);
                    m_process.endnode();
                    break;
                case "Count":
                    // count
                    m_process.writenode(ph.name, "");
                    int m = bitclumpBIN(ph.bits);
                    fordump += "(" + ph.bits + "-bit " + ph.type + " - " + ph.name + ": " + m + ") ";
                    for (int i = 0; i < m; i += 1)
                    {
                        m_process.writenode(ph.name + "-child" + i, "");
                        InterpStruct e3 = m_process.readdata("/base/" + ph.node1 + "/Var[@name='" + ph.name + "']", ph.node);
                        readchildren(e3, wrap, ph.node);
                        m_process.endnode();
                    }
                    m_process.endnode();
                    break;
                case "HCount": // why did you have to make this so difficult bungie
                    m_process.writenode(ph.name, "");

                    int m9 = bitclumpBIN(ph.bits);
                    fordump += "(" + ph.bits + "-bit " + ph.type + " - " + ph.name + ": ) ";
                    fordump += "\r\n(" + m9 + "-chars) ";
                    List<zoingoboingo> string_indexes = new();
                    for (int i = 0; i < m9; i += 1)
                    {
                        // do the language thingo
                        zoingoboingo z = new();
                        z.EnglishStringIndex = (bitclumpBIN(1) == 0) ? -1 : bitclumpBIN(ph.chars);
                        z.JapaneseStringIndex = (bitclumpBIN(1) == 0) ? -1 : bitclumpBIN(ph.chars);
                        z.GermanStringIndex = (bitclumpBIN(1) == 0) ? -1 : bitclumpBIN(ph.chars);
                        z.FrenchStringIndex = (bitclumpBIN(1) == 0) ? -1 : bitclumpBIN(ph.chars);
                        z.SpanishStringIndex = (bitclumpBIN(1) == 0) ? -1 : bitclumpBIN(ph.chars);
                        z.MexicanStringIndex = (bitclumpBIN(1) == 0) ? -1 : bitclumpBIN(ph.chars);
                        z.ItalianStringIndex = (bitclumpBIN(1) == 0) ? -1 : bitclumpBIN(ph.chars);
                        z.KoreanStringIndex = (bitclumpBIN(1) == 0) ? -1 : bitclumpBIN(ph.chars);
                        z.Chinese1StringIndex = (bitclumpBIN(1) == 0) ? -1 : bitclumpBIN(ph.chars);
                        z.Chinese2StringIndex = (bitclumpBIN(1) == 0) ? -1 : bitclumpBIN(ph.chars);
                        z.PortugeseStringIndex = (bitclumpBIN(1) == 0) ? -1 : bitclumpBIN(ph.chars);
                        z.PolishStringIndex = (bitclumpBIN(1) == 0) ? -1 : bitclumpBIN(ph.chars);
                        if (!isreach) // H4 & 2A
                        {
                            z.RussianStringIndex = (bitclumpBIN(1) == 0) ? -1 : bitclumpBIN(ph.chars);
                            z.DanishStringIndex = (bitclumpBIN(1) == 0) ? -1 : bitclumpBIN(ph.chars);
                            z.FinnishStringIndex = (bitclumpBIN(1) == 0) ? -1 : bitclumpBIN(ph.chars);
                            z.DutchStringIndex = (bitclumpBIN(1) == 0) ? -1 : bitclumpBIN(ph.chars);
                            z.NorwegianStringIndex = (bitclumpBIN(1) == 0) ? -1 : bitclumpBIN(ph.chars);
                        }
                        string_indexes.Add(z);
                    }
                    zoingo_compressed_chunk = "";
                    string compression_state = "True";
                    if (m9 > 0) // we have at least one string
                    {
                        // table byte length
                        int m3;
                        if (isreach && ph.name == "Teamstring") // for some reason, reaches thingo uses an extra bit for the mfing char count
                            m3 = bitclumpBIN(ph.chars + 1);
                        else
                            m3 = bitclumpBIN(ph.chars);

                        fordump += "\r\n(" + m3 + "-uncompressed_chars) ";
                        // is compressed 
                        int d = bitclumpBIN(1);

                        fordump += "\r\n(" + d + "-is_compressed) \r\n";

                        if (d == 0)
                        {
                            zoingo_compressed_chunk = bitclumpBIN3(m3 * 8);
                            compression_state = "False";
                        }
                        else
                        {
                            int m1 = bitclumpBIN(ph.chars);
                            string b = bitclumpBIN3(m1 * 8);

                            byte[] b2 = Convert.FromHexString(b);
                            var bytethingos = z_testing.ZLib.LowLevelDecompress(b2, m3);
                            zoingo_compressed_chunk = Convert.ToHexString(bytethingos);
                        }
                    }
                    m_process.writenode("UseCompression", compression_state);
                    m_process.endnode();
                    m_process.writenode("Strings", "");
                    for (int i = 0; i< string_indexes.Count; i++)
                    {
                        m_process.writenode("String"+i, "");
                        zoingoboingo current_string = string_indexes[i];

                        do_the_language(current_string.EnglishStringIndex, "EnglishString");
                        do_the_language(current_string.JapaneseStringIndex, "JapaneseString");
                        do_the_language(current_string.GermanStringIndex, "GermanString");
                        do_the_language(current_string.FrenchStringIndex, "FrenchString");
                        do_the_language(current_string.SpanishStringIndex, "SpanishString");
                        do_the_language(current_string.MexicanStringIndex, "MexicanString");
                        do_the_language(current_string.ItalianStringIndex, "ItalianString");
                        do_the_language(current_string.KoreanStringIndex, "KoreanString");
                        do_the_language(current_string.Chinese1StringIndex, "Chinese1String");
                        do_the_language(current_string.Chinese2StringIndex, "Chinese2String");
                        do_the_language(current_string.PortugeseStringIndex, "PortugeseString");
                        do_the_language(current_string.PolishStringIndex, "PolishString");
                        if (!isreach) // H4 & 2A
                        {
                            do_the_language(current_string.RussianStringIndex, "RussianString");
                            do_the_language(current_string.DanishStringIndex, "DanishString");
                            do_the_language(current_string.FinnishStringIndex, "FinnishString");
                            do_the_language(current_string.DutchStringIndex, "DutchString");
                            do_the_language(current_string.NorwegianStringIndex, "NorwegianString");
                        }

                        m_process.endnode();
                    }

                    // then we write it all down here
                    m_process.endnode();
                    m_process.endnode();
                    zoingo_compressed_chunk = null;
                    break;
                default:
                    if (ph.type.Contains("External"))
                    {
                        // container
                        string[] w = ph.type.Split(":");
                        m_process.writenode(ph.name, "");
                        int w2 = i;
                        foreach (paramheader2 p in m_process.returnchildren(w[1]))
                        {

                            fordump += "\r\r" + ph.name + " ";
                            readblock(p, "ExTypes");
                            // runstack();
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
                    if (ph.type.Contains("Enumref"))
                    {
                        // container
                        string[] w = ph.type.Split(":");
                        ph.node1 = "RefTypes";
                        paramheader2 ph2 = VarInterpret(ph.node1, w[1], ph.node);
                        ph2.node = ph.node;
                        ph2.node1 = "RefTypes";

                        m_process.writenode(ph.name, "");
                        readblock(ph2, ph2.node1);
                        m_process.endnode();
                    }
                    break;

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

        public struct zoingoboingo
        {
            public int EnglishStringIndex;
            public int JapaneseStringIndex;
            public int GermanStringIndex;
            public int FrenchStringIndex;
            public int SpanishStringIndex;
            public int MexicanStringIndex;
            public int ItalianStringIndex;
            public int KoreanStringIndex;
            public int Chinese1StringIndex;
            public int Chinese2StringIndex;
            public int PortugeseStringIndex;
            public int PolishStringIndex;
            // H4 & 2A
            public int RussianStringIndex;
            public int DanishStringIndex;
            public int FinnishStringIndex;
            public int DutchStringIndex;
            public int NorwegianStringIndex;
        }
        public string zoingo_compressed_chunk;
        public void do_the_language(int in1, string language_node_name)
        {
            if (in1 < 0)
            {
                m_process.writenode(language_node_name, "False");
                m_process.endnode();
            }
            else // it does point to a thingo
            {
                m_process.writenode(language_node_name, "True");
                bool searching = true;
                string hextringthing = "";
                int depth = in1;
                while (searching)
                {
                    string s_byte = zoingo_compressed_chunk.Substring(depth * 2, 2);
                    depth++;
                    if (s_byte == "00")
                    {
                        searching = false;
                    }
                    else // is a probably normal character
                    {
                        hextringthing += s_byte;
                    }
                }
                // super duper lazy way but whatever
                var bytes = new byte[hextringthing.Length / 2];
                for (var iw = 0; iw < bytes.Length; iw++)
                {
                    string s = hextringthing.Substring(iw * 2, 2);
                    bytes[iw] = Convert.ToByte(s, 16);
                }
                string the_stringo_thingo = Encoding.UTF8.GetString(bytes);

                if (the_stringo_thingo == null)
                {

                }
                m_process.writenode("String", the_stringo_thingo);

                m_process.endnode();
                m_process.endnode();
            }
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
            Enumerable.Range(0, data.Length / 8).Select(i => Convert.ToByte(data.Substring(i * 8, 8), 2).ToString("X2")));
            return String.Concat(hex.Where(c => !Char.IsWhiteSpace(c)));
        }

        public static string FromHexString8(string hexString)
        {
            var bytes = new byte[hexString.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return Encoding.UTF8.GetString(bytes); // returns: "Hello world" for "48656C6C6F20776F726C64"
        }
        public static string FromHexString16(string hexString)
        {
            var bytes = new byte[hexString.Length / 2];
            for (var i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }

            return Encoding.BigEndianUnicode.GetString(bytes); // returns: "Hello world" for "48656C6C6F20776F726C64"
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
            public int chars { get; set; }
        }

        struct param
        {
            public string type { get; set; }
            public string value_real { get; set; }
        }


    }
}
