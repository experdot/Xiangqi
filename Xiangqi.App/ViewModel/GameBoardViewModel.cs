using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Xiangqi.App.Render;
using Xiangqi.App.Speaker;
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

            Game.OnMoved += Game_OnMoved;
        }

        public void Load()
        {
            Task.Run(async () =>
            {
                var steps = new string[] { "炮二平五", "马８进７", "马二进三", "车９平８", "车一平二", "马２进３", "兵七进一", "卒７进１", "马八进七", "炮２进４", "兵五进一", "炮８进４", "车九进一", "炮２平３", "相七进九", "车１平２", "车九平六", "炮３平６", "车六进六", "象７进５", "车六平七", "炮６进１", "炮八进五", "炮６平３", "马三退五", "炮３退１", "炮五平八", "车２平１", "车二进二", "车１进２", "车七平六", "马７进６", "后炮进四", "士６进５", "车六平七", "炮３平２", "前炮平五", "马６退５", "车七平九", "象３进１", "马五进四", "炮２平５", "马四进三", "车８进４", "兵三进一", "炮８退１", "车二平四", "卒５进１", "炮八退二", "炮８平５", "马三退五", "卒５进１", "炮八平五", "车８平５", "炮五进三", "炮５退４", "仕六进五", "车５平１" };

                foreach (var item in steps)
                {
                    BoardGrid.Dispatcher.Invoke(() =>
                    {
                        var move = MoveParser.FromChineseWXF(Game.Board, item);
                        MoveExecutor.Execute(Game, move);
                    });
                    await Task.Delay(2000);
                }
            });
        }

        private void Game_OnMoved(object sender, OnMovedEventArgs e)
        {
            Render.RenderAnimation(PieceCanvas, e.OldLocation, e.NewLocation);
            SystemSpeaker.Speak(MoveParser.ToChineseWXF(Game.BoardStepHistory.LastOrDefault().Move));
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

            //PieceCanvas.Children.Clear();
            //Render.RenderPiece(Game.Board, PieceCanvas);
        }
    }
}
