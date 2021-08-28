using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xiangqi.App.ViewModel;

namespace Xiangqi.App.Control
{
    /// <summary>
    /// Interaction logic for GameBoardControl.xaml
    /// </summary>
    public partial class GameBoardControl : UserControl
    {
        public GameBoardViewModel ViewModel { get; set; }
        public GameBoardControl()
        {
            InitializeComponent();

            ViewModel = new GameBoardViewModel(BoardGrid, BackgroundLayer, PieceLayer);
            DataContext = ViewModel;
        }
    }
}
