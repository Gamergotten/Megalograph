using System.Windows.Media;

namespace Megalograph.code
{
    public static class BrushPresets
    {
        public static SolidColorBrush trigger_header = new SolidColorBrush(Color.FromArgb(255, 212, 117, 0));
        public static SolidColorBrush condition_header = new SolidColorBrush(Color.FromArgb(255, 158, 0, 158));
        public static SolidColorBrush action_header = new SolidColorBrush(Color.FromArgb(255, 214, 0, 0));

        public static SolidColorBrush selection_border = new SolidColorBrush(Color.FromArgb(255, 220, 173, 14));
        public static SolidColorBrush finder_border = new SolidColorBrush(Color.FromArgb(255, 78, 235, 144));
    }
}
