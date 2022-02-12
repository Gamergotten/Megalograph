﻿using System;
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

namespace gamtetyper.metaviewer
{
    /// <summary>
    /// Interaction logic for MV_value_block.xaml
    /// </summary>
    public partial class MV_value_block : UserControl
    {
        public MV_value_block()
        {
            InitializeComponent();
        }

        public Ebum child;

        public MV_container_block containing_container;

        public MainWindow main;
        private void edit_value_box(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return )
            {
                if (child.Type == "Int")
                {
                    try
                    {
                        child.V = int.Parse(value_text.Text).ToString();
                    }
                    catch
                    {
                        value_text.Foreground = Brushes.Red;
                        return;
                    }
                }
                else
                {
                    child.V = value_text.Text;
                }

                main.write_node(child);
                value_text.Foreground = Brushes.White;
                //OR_group.foc
                Keyboard.ClearFocus();
            }
        }

 
    }
}
