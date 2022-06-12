using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static gamtetyper.MainWindow;
using System.Windows.Shapes;

namespace gamtetyper
{
    public class Gametype
    {
        public struct GametypeLoaded
        {
            public string tabname { get; set; }

            public string BIN_File {  get; set; }
            public string SSGF_File { get; set; }

            public string Target_Halo { get; set; }
            
            // make these refer to the UI counterparts


        }

        public class TriggerUI
        {
            public CodeBlock UI { get; set; }
            public int CHILD_elements_key { get; set; }
            public trigger stored_trigger { get; set; }

        }

        public class ActionUI
        {
            public CodeBlock UI { get; set; }
            public int linked_elements_key { get; set; }
            public action stored_action { get; set; }

        }
        public class ConditionUI
        {
            public CodeBlock UI { get; set; }
            public int linked_elements_key { get; set; }
            public condition stored_condition { get; set; }

        }

        public class BranchUI
        {
            public BranchBlock UI { get; set; }
            public int CHILD_elements_key { get; set; }
            public int linked_elements_key { get; set; }
            public action stored_action { get; set; }
        }

        //
        // MEGALO DATA STRUCTURES
        //
        public struct Vector2D
        {
            public bool has_position;
            public double x { get; set; }
            public double y { get; set; }
        }
        public struct trigger
        {
            public Vector2D position;
            public string Name { get; set; }
            public Ebum Type { get; set; }
            public Ebum Attribute { get; set; }
            public int Conditions_insert { get; set; }
            public int Conditions_count { get; set; }
            public int Actions_insert { get; set; }
            public int Actions_count { get; set; }
            public int unknown1 { get; set; }
            public int unknown2 { get; set; }

            // UI stuff

        }
        public struct action
        {
            public Vector2D position;
            public Ebum Type { get; set; }
        }
        public struct condition
        {
            public Vector2D position;
            public Ebum Type { get; set; }
            public int Not { get; set; }
            public int insertionpoint { get; set; } // we have to calculate this when exporting to triggers, otherwise isn't relevant
            public int OR_Group { get; set; }
        }

        public class Ebum
        {
            public string Name { get; set; }
            public string V { get; set; }
            public string XMLDoc { get; set; }
            public string XMLPath { get; set; }
            public List<Ebum> Params { get; set; }
            public string Type { get; set; }
            public int Size { get; set; }
            public string FUCKK_YOU { get; set; } // the parent node in instances where we need a parent xml node
            public List<string> nodes_list_yes_i_did_just_do_that { get; set; }
        }

    }
}
