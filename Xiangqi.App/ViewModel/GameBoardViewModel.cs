using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using Xiangqi.App.Render;
using Xiangqi.Core;

namespace Xiangqi.App.ViewModel
{
    public class GameBoardViewModel
    {
        public Grid BoardGrid { get; set; }
        public Canvas BackgroundCanvas { get; set; }
        public Canvas PieceCanvas { get; set; }

        public XiangqiGame Game { get; set; }
        public GameBoardRender Render { get; set; }

        public double Size { get; set; } = 96;

        public GameBoardViewModel(Grid boardGrid, Canvas backgroundCanvas, Canvas pieceCanvas)
        {
            BoardGrid = boardGrid;
            BackgroundCanvas = backgroundCanvas;
            PieceCanvas = pieceCanvas;

            Game = new XiangqiGame();
            Game.Start();

            Render = new GameBoardRender(Size);

            Render.RenderBackground(Game.Board, BackgroundCanvas);
            Render.RenderPiece(Game.Board, pieceCanvas);

            BoardGrid.Background = Brushes.Transparent;
            BoardGrid.MouseDown += BoardGrid_MouseDown;
        }

        private void BoardGrid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var point = e.GetPosition(PieceCanvas);
            double size = Size;
            float border = (float)(size / 2);
            float x, y;
            x = (float)Math.Round((point.X - border) / size);
            y = (float)Math.Round((point.Y - border) / size);

            Game.Move(new System.Numerics.Vector2(x, y));

            PieceCanvas.Children.Clear();
            Render.RenderPiece(Game.Board, PieceCanvas);

            Debug.WriteLine(Game.MoveHistory.LastOrDefault()?.ToChineseWXF());
        }
    }
}
