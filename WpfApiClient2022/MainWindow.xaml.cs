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

namespace WpfApiClient2022
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }



        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            CreateActor createActor = new CreateActor();
            createActor.Show();
        }
        private void MenuItem2_Click(object sender, RoutedEventArgs e)
        {
            CreateMovie createMovie = new CreateMovie();
            createMovie.Show();
        }
        
        
    }
}
