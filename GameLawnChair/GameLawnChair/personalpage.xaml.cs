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
using System.Windows.Shapes;

namespace GameLawnChair
{
    /// <summary>
    /// personalpage.xaml 的交互逻辑
    /// </summary>
    public partial class personalpage : Window
    {
        public personalpage()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            //logout to main window.
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            //reset password
            var win = new resetpswd();
            win.Show();
            this.Close();
        }

    }
}
