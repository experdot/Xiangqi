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
using Xiangqi.Core;

namespace Xiangqi.App
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GameBoardControl.ViewModel.Game.OnMoved += Game_OnMoved;
        }

        private void Game_OnMoved(object sender, OnMovedEventArgs e)
        {
            MoveListBox.ItemsSource = new string[] { };
            MoveListBox.ItemsSource = GameBoardControl.ViewModel.Game.MoveHistory.Select(v => v.ToChineseWXF());
        }
    }
}
