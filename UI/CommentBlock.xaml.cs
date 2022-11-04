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
using gamtetyper.UI;

namespace Megalograph.UI
{
    /// <summary>
    /// Interaction logic for CommentBlock.xaml
    /// </summary>
    public partial class CommentBlock : UserControl
    {
        public CommentBlock()
        {
            InitializeComponent();
        }
        public bool is_hidden = false;
        public bool is_grabbed = false;
        public NodeWindow main_window;
        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            main_window.commentblockclick(this);
            e.Handled = true;
        }

        private void Label_MouseUp(object sender, MouseButtonEventArgs e)
        {
            main_window.commentblockrelease(this);
            e.Handled = true;
        }

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {   //engage writing mode
            note.IsHitTestVisible = true;
            note.Focus();
        }

        private void note_LostFocus(object sender, RoutedEventArgs e)
        {
            // check for color code
            note.IsHitTestVisible = false;
            if (note.Text.Length >= 10)
            {
                string colorcode_check = note.Text.Substring(note.Text.Length - 10, 10);
                if (colorcode_check.Substring(0, 2) == "^#") // then its a color code
                {
                    our_color.A = Convert.ToByte(colorcode_check.Substring(2, 2), 16);
                    our_color.R = Convert.ToByte(colorcode_check.Substring(4, 2), 16);
                    our_color.G = Convert.ToByte(colorcode_check.Substring(6, 2), 16);
                    our_color.B = Convert.ToByte(colorcode_check.Substring(8, 2), 16);
                    status_border.Background = new SolidColorBrush(our_color);
                    note.Text = note.Text.Substring(0, note.Text.Length - 10);
                }
            }
        }
        public void update_color()
        {
            status_border.Background = new SolidColorBrush(our_color);
        }
        public Color our_color = Color.FromArgb(0x66, 0xFF, 0xFF, 0xFF);

        public enum direction
        {
            NONE,
            TOP,
            LEFT,
            RIGHT,
            BOTTOM
        };

        public direction drag_direction = direction.NONE;
        private void TOP_DRAG_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            drag_direction = direction.TOP;
            e.Handled = true;
        }
        private void LEFT_DRAG_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            drag_direction = direction.LEFT;
            e.Handled = true;
        }
        private void RIGHT_DRAG_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            drag_direction = direction.RIGHT;
            e.Handled = true;
        }
        private void BOTTOM_DRAG_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            drag_direction = direction.BOTTOM;
            e.Handled = true;
        }

    }
}
