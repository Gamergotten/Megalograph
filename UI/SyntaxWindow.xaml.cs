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
using gamtetyper;

namespace Megalograph.UI
{
    /// <summary>
    /// Interaction logic for SyntaxWindow.xaml
    /// </summary>
    public partial class SyntaxWindow : UserControl
    {
        public SyntaxWindow()
        {
            InitializeComponent();
        }

        public MainWindow main;



        
        
        public void this_is_where_the_fun_begins()
        {
            string haloxml2 = main.Loaded_Gametypes[main.Current_Gametype].Target_Halo;
            string haloxml = main.returnmegldoc_fromhalo(haloxml2);
            bool is_Reach = false;
            if (haloxml2 == "HR")
                is_Reach = true;

            List<Gametype.trigger> t = main.XP.returnScripts(is_Reach, haloxml);
            List<Gametype.action> a = main.XP.doactions(haloxml);
            List<Gametype.condition> c = main.XP.doconditions(haloxml);

            foreach (var trigger in t)
            {
                List<string> contained_lines = new();

                for (int w = 0; w < trigger.Actions_count; w++)  // LINE UP EVERY ACTION ELEMENT
                {
                    int index_for_action = w + trigger.Actions_insert;

                    if (a[index_for_action].Type.V != "Virtual Trigger")
                    {

                    }
                    else
                    {

                    }

                }

                List<int> insertion_points = new List<int>();

                for (int w = 0; w < trigger.Conditions_count; w++)  // LINE UP EVERY CONDITION ELEMENT
                {
                    int index_for_condition = w + trigger.Conditions_insert;



                    int condition_insert = c[index_for_condition].insertionpoint;
                    int factored_insertion = condition_insert;
                    for (int q = 0; q < insertion_points.Count; q++)
                    {
                        if (insertion_points[q] <= condition_insert)
                        {
                            factored_insertion++;
                        }
                    }
                    insertion_points.Add(condition_insert);
                }


            }
        }
    }
}
