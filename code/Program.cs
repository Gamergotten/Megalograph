using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.Intrinsics.X86;
using System.Security.AccessControl;
using System.Linq;

namespace gamtetyper
{
    class Program
    {
        //
        //
        // USE THIS FOR REFERENCE AND THEN REMOVE
        //
        //



        //    run();

        //}

        /*    static void run()
            {
                Console.WriteLine("Type 'help' for list of commands");


                if (p[0] == "help")
                {
                    Console.WriteLine();
                    Console.WriteLine(">Append a prefix, a type and a command");
                    Console.WriteLine("eg. HR read");
                    Console.WriteLine();
                    Console.WriteLine("PREFIXES");
                    Console.WriteLine("'hr' - halo reach");
                    Console.WriteLine("'h4' - halo 4");
                    Console.WriteLine("'h2' - halo 2A");
                    Console.WriteLine();
                    Console.WriteLine("COMMANDS ");
                    Console.WriteLine("'comp' - ");
                    Console.WriteLine("'read' - decompile an array of binary into megalo code");

                }
                if (p[0] == "convert")
                {
                    string nospace = String.Concat(p[1].Where(c => !Char.IsWhiteSpace(c)));
                    var hex = string.Join(" ",
                Enumerable.Range(0, nospace.Length / 8)
                .Select(i => Convert.ToByte(nospace, 2).ToString("X2")));

                    Console.WriteLine(String.Concat(hex.Where(c => !Char.IsWhiteSpace(c))));
                } // im pretty sure this doesnt work

                if (p.Length > 1 && p.Length < 3)
                {
                    string w = "";
                    string xmldir = @"C:\Users\Connor\Documents\Yiff (SSD)\GAMETYPE PROJECT\GAMETYPE XMLS\XMLs";
                    if (p[0] == "hr")
                    {
                        w = @"\HR.xml";
                    }
                    if (p[0] == "h4")
                    {
                        w = @"\H4.xml";
                    }
                    if (p[0] == "h2")
                    { 
                        w = @"\H2A.xml";
                    }

                    if (p[1] == "comp")
                    {

                    }
                    if (p[1] == "read")
                    {

                    }
                }
                run();
            } */
    }
} 