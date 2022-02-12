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

namespace gamtetyper.UI
{
    /// <summary>
    /// Interaction logic for NodeWindow.xaml
    /// </summary>
    public partial class NodeWindow : UserControl
    {
        public NodeWindow()
        {
            InitializeComponent();
        }





        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            double DeltaX = mouseX - e.GetPosition(testab).X;
            double DeltaY = mouseY - e.GetPosition(testab).Y;

            double factored_X = DeltaX * nodegraph_scale.ScaleX;
            double factored_Y = DeltaY * nodegraph_scale.ScaleY;

            // 
            foreach (CodeBlock cb in active_triggers)
            {
                if (cb.is_grabbed)
                {
                    drag_block(cb, DeltaX, DeltaY);
                }
            }
            foreach (KeyValuePair<int, CodeBlock> cb in active_actions)
            {
                if (cb.Value.is_grabbed)
                {
                    drag_block(cb.Value, DeltaX, DeltaY);
                }
            }
            foreach (KeyValuePair<int, CodeBlock> cb in active_conditions)
            {
                if (cb.Value.is_grabbed)
                {
                    drag_block(cb.Value, DeltaX, DeltaY);
                }
            }
            foreach (KeyValuePair<int, BranchBlock> cb in active_branches)
            {
                if (cb.Value.is_grabbed)
                {
                    drag_retarded_block(cb.Value, DeltaX, DeltaY);
                }
            }

            if (nodegraph_grabbed)
            {
                //testab.
                //nodegraph_scale
                nodegraph_trans.X -= factored_X;
                nodegraph_trans.Y -= factored_Y;


            }
            //nodegraph_scale.CenterX = -nodegraph_trans.X * nodegraph_scale.ScaleX;
            //nodegraph_scale.CenterY = -nodegraph_trans.Y * nodegraph_scale.ScaleY;

            //testab.RenderTransformOrigin.X = testab.RenderSize.Width / nodegraph_trans.X;
            //testab.RenderTransformOrigin.Y = testab.RenderSize.Height / nodegraph_trans.Y;

            testab.RenderTransformOrigin = new(((1f / (testab.ActualWidth)) * -nodegraph_trans.X) + 0.5f, ((1f / (testab.ActualHeight)) * -nodegraph_trans.Y) + 0.5f);


            DEBUG_LOCAITON.X = RenderTransformOrigin.X * testab.ActualWidth;
            DEBUG_LOCAITON.Y = RenderTransformOrigin.Y * testab.ActualHeight;

            mouseX = e.GetPosition(testab).X;
            mouseY = e.GetPosition(testab).Y;

            //
            // -- move lines that are currently held
            //

            if (line_dragging_from_branchblockTHEN != null)
            {
                line_dragging_from_branchblockTHEN.THENpath.X2 = mouseX;
                line_dragging_from_branchblockTHEN.THENpath.Y2 = mouseY;
            }
            if (line_dragging_from_branchblockDO != null)
            {
                line_dragging_from_branchblockDO.DOpath.X2 = mouseX;
                line_dragging_from_branchblockDO.DOpath.Y2 = mouseY;
            }
            if (line_dragging_from_codeblock != null)
            {
                line_dragging_from_codeblock.Outpath.X2 = mouseX;
                line_dragging_from_codeblock.Outpath.Y2 = mouseY;
            }
        }
        public void drag_block(CodeBlock cb, double Delta_X, double Delta_Y)
        {
            cb.transfrom_location.X -= Delta_X;
            cb.transfrom_location.Y -= Delta_Y;
            if (cb.Inpath != null)
            {
                cb.Inpath.X2 = cb.transfrom_location.X + 8;
                cb.Inpath.Y2 = cb.transfrom_location.Y + 17;
            }
            if (cb.Outpath != null)
            {
                cb.Outpath.X1 = cb.transfrom_location.X + 192;
                cb.Outpath.Y1 = cb.transfrom_location.Y + 17;
            }
        }

        public void drag_retarded_block(BranchBlock cb, double Delta_X, double Delta_Y)
        {
            cb.transfrom_location.X -= Delta_X;
            cb.transfrom_location.Y -= Delta_Y;
            if (cb.Inpath != null)
            {
                cb.Inpath.X2 = cb.transfrom_location.X + 8;
                cb.Inpath.Y2 = cb.transfrom_location.Y + 17;
            }
            if (cb.DOpath != null)
            {
                cb.DOpath.X1 = cb.transfrom_location.X + 192;
                cb.DOpath.Y1 = cb.transfrom_location.Y + 17;
            }
            if (cb.THENpath != null)
            {
                cb.THENpath.X1 = cb.transfrom_location.X + 192;
                cb.THENpath.Y1 = cb.transfrom_location.Y + 42;
            }
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            nodegraph_grabbed = true;
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            nodegraph_grabbed = false;
            wait_a_second_and_then_solve_our_problem();
            //drop all nodes too
        }
        async void wait_a_second_and_then_solve_our_problem()
        {
            //await Task.Delay(5);


            if (line_dragging_from_codeblock != null)
            {
                if (line_dragging_from_codeblock.trigger_parent != null)
                {
                    if (line_dragging_from_codeblock.trigger_parent.CHILD_elements_key > -1)
                    {
                        if (links.ContainsKey(line_dragging_from_codeblock.trigger_parent.CHILD_elements_key))
                        {
                            var BALLSinHD = links[line_dragging_from_codeblock.trigger_parent.CHILD_elements_key];
                            if (BALLSinHD.Count > 0)
                            {
                                potential_block p = BALLSinHD[0];
                                if (p._action != null)
                                {
                                    p._action.Inpath = null;
                                }
                                else if (p._condition != null)
                                {
                                    p._condition.Inpath = null;

                                }
                                else if (p._branch != null)
                                {
                                    p._branch.Inpath = null;
                                }
                            }
                        }
                        else
                        {
                            string debug_moment = ":(";
                        }
                    }
                    line_dragging_from_codeblock.trigger_parent.CHILD_elements_key = -1;
                }
                else if (line_dragging_from_codeblock.condition_parent != null)
                {
                    int key = line_dragging_from_codeblock.condition_parent.linked_elements_key;
                    snap_links_connection(key, balls(key) + 1);
                }
                else if (line_dragging_from_codeblock.action_parent != null)
                {
                    int key = line_dragging_from_codeblock.action_parent.linked_elements_key;
                    snap_links_connection(key, balls(key) + 1);
                }

                testab.Children.Remove(line_dragging_from_codeblock.Outpath);
                line_dragging_from_codeblock.Outpath = null;
            }
            if (line_dragging_from_branchblockDO != null)
            {
                if (line_dragging_from_branchblockDO.branch_parent.CHILD_elements_key > -1)
                {
                    if (links.ContainsKey(line_dragging_from_branchblockDO.branch_parent.CHILD_elements_key))
                    {
                        potential_block p = links[line_dragging_from_branchblockDO.branch_parent.CHILD_elements_key][0];
                        if (p._action != null)
                        {
                            p._action.Inpath = null;
                        }
                        else if (p._condition != null)
                        {
                            p._condition.Inpath = null;

                        }
                        else if (p._branch != null)
                        {
                            p._branch.Inpath = null;
                        }
                    }
                    else
                    {
                        string debug_moment = ":(";
                    }
                }
                line_dragging_from_branchblockDO.branch_parent.CHILD_elements_key = -1;

                testab.Children.Remove(line_dragging_from_branchblockDO.DOpath);
                line_dragging_from_branchblockDO.DOpath = null;
            }
            if (line_dragging_from_branchblockTHEN != null)
            {
                int key = line_dragging_from_branchblockTHEN.branch_parent.linked_elements_key;
                snap_links_connection(key, balls(key) + 1);

                testab.Children.Remove(line_dragging_from_branchblockTHEN.THENpath);
                line_dragging_from_branchblockTHEN.THENpath = null;
            }
            line_dragging_from_codeblock = null;
            line_dragging_from_branchblockDO = null;
            line_dragging_from_branchblockTHEN = null;
        }
        public int balls(int KEY)
        {
            List<potential_block> link_to_find_index_from = links[KEY];
            for (int p_index = 0; p_index < link_to_find_index_from.Count; p_index++)
            {
                if (link_to_find_index_from[p_index]._action == line_dragging_from_codeblock && line_dragging_from_codeblock != null)
                {
                    return p_index;
                }
                if (link_to_find_index_from[p_index]._condition == line_dragging_from_codeblock && line_dragging_from_codeblock != null)
                {
                    return p_index;
                }
                if (link_to_find_index_from[p_index]._branch == line_dragging_from_branchblockTHEN && line_dragging_from_branchblockTHEN != null)
                {
                    return p_index;
                }
            }
            return -1;
        }
    }
}
