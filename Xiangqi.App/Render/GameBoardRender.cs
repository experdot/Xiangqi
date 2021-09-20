using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Shapes;
using Xiangqi.Core;

namespace Xiangqi.App.Render
{
    public class GameBoardRender
    {
        public double Size { get; set; }
        public Dictionary<Vector2, Grid> PieceGrid { get; set; }
        public Dictionary<Camp, string[]> PieceNames { get; set; }

        public GameBoardRender(double size)
        {
            Size = size;
            PieceGrid = new Dictionary<Vector2, Grid>();
            PieceNames = new Dictionary<Camp, string[]>
            {
                [Camp.Red] = new[] { "帥", "仕", "相", "馬", "車", "砲", "兵" },
                [Camp.Black] = new[] { "將", "士", "象", "馬", "車", "炮", "卒" }
            };
        }

        public void RenderBackground(Board board, Canvas canvas)
        {
            var width = Size * 9;
            var height = Size * 10;

            for (var i = 0; i <= 9; i += 8)
            {
                var margin = Size / (double)2.0F;
                var offset = Size * i + margin;
                canvas.Children.Add(DrawLine(Brushes.Black, offset, margin, offset, height - margin));
            }

            for (var i = 0; i < 9; i++)
            {
                var margin = Size / (double)2.0F;
                var offset = Size * i + margin;
                canvas.Children.Add(DrawLine(Brushes.Black, offset, margin, offset, Size * 4 + margin));
            }

            for (var i = 0; i < 9; i++)
            {
                var margin = Size / (double)2.0F;
                var offset = Size * i + margin;
                canvas.Children.Add(DrawLine(Brushes.Black, offset, Size * 5 + margin, offset, height - margin));
            }

            for (var j = 0; j <= 9; j++)
            {
                var margin = Size / (double)2.0F;
                var offset = Size * j + margin;
                canvas.Children.Add(DrawLine(Brushes.Black, margin, offset, width - margin, offset));
            }

            canvas.Children.Add(DrawLine(Brushes.Black, Size * 3.5F, Size / (double)2.0F, Size * 5.5F, Size / (double)2.0F + Size * 2.0F));
            canvas.Children.Add(DrawLine(Brushes.Black, Size * 3.5F, Size / (double)2.0F + Size * 2.0F, Size * 5.5F, Size / (double)2.0F));

            canvas.Children.Add(DrawLine(Brushes.Black, Size * 3.5F, Size * 7.5F, Size * 5.5F, Size * 9.5F));
            canvas.Children.Add(DrawLine(Brushes.Black, Size * 3.5F, Size * 9.5F, Size * 5.5F, Size * 7.5F));
        }


        public void RenderPiece(Board board, Canvas canvas)
        {
            var pieceMap = board.PieceMap;
            double border = Size / 16;
            for (int i = 0; i <= 9; i++)
            {
                for (int j = 0; j <= 8; j++)
                {
                    var target = pieceMap[j, i];
                    if (target is object)
                    {
                        var brush = target.Camp == Camp.Red ? Brushes.Red : Brushes.Black;

                        var circle = DrawCircle(brush, border + j * Size, border + i * Size, Size - border * 2f, Size - border * 2f);

                        var text = PieceNames[target.Camp][(int)(target.PieceType)];
                        var textControl = DrawText(text, Size / 2, Brushes.White, border + j * Size, border + i * Size, Size - border * 2f, Size - border * 2f);
                        circle.Children.Add(textControl);

                        canvas.Children.Add(circle);
                        PieceGrid[new Vector2(j, i)] = circle;
                    }
                }
            }
        }

        public void RenderAnimation(Canvas pieceCanvas, Vector2 oldLocation, Vector2 newLocation)
        {
            var margin = (float)Size / 16f;
            var oldPosition = oldLocation * (float)Size + new Vector2(margin, margin);
            var newPosition = newLocation * (float)Size + new Vector2(margin, margin);

            CubicEase cubicEase = new CubicEase() { EasingMode = EasingMode.EaseIn };
            var duration = TimeSpan.FromMilliseconds(200);

            DoubleAnimation animeX = new DoubleAnimation()
            {
                From = oldPosition.X,
                To = newPosition.X,
                Duration = duration,
                EasingFunction = cubicEase
            };

            DoubleAnimation animeY = new DoubleAnimation()
            {
                From = oldPosition.Y,
                To = newPosition.Y,
                Duration = duration,
                EasingFunction = cubicEase
            };

            var grid = PieceGrid[oldLocation];
            grid.BeginAnimation(Canvas.LeftProperty, animeX);
            grid.BeginAnimation(Canvas.TopProperty, animeY);
            if (PieceGrid.ContainsKey(newLocation))
            {
                pieceCanvas.Children.Remove(PieceGrid[newLocation]);
            }

            PieceGrid[oldLocation] = null;
            PieceGrid[newLocation] = grid;
        }

        private Line DrawLine(Brush brush, double x1, double y1, double x2, double y2)
        {
            var line = new Line();
            line.X1 = x1;
            line.Y1 = y1;
            line.X2 = x2;
            line.Y2 = y2;
            line.Stroke = brush;
            line.StrokeThickness = 1;
            return line;
        }

        private Grid DrawCircle(Brush brush, double x1, double y1, double width, double height)
        {
            var grid = new Grid();
            grid.SetValue(Canvas.LeftProperty, x1);
            grid.SetValue(Canvas.TopProperty, y1);
            grid.Width = width;
            grid.Height = height;

            var line = new Ellipse();
            line.Width = width;
            line.Height = height;
            line.Fill = brush;
            line.Stroke = Brushes.White;
            line.StrokeThickness = 3;
            line.Effect = new DropShadowEffect() { BlurRadius = 8 };
            grid.Children.Add(line);
            return grid;
        }

        private Border DrawText(string text, double fontSize, Brush brush, double x1, double y1, double width, double height)
        {
            var line = new TextBlock();
            line.Text = text;
            line.FontSize = fontSize;
            line.Foreground = brush;
            line.VerticalAlignment = System.Windows.VerticalAlignment.Center;
            line.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;

            var border = new Border();
            border.Width = width;
            border.Height = height;
            border.SetValue(Canvas.LeftProperty, x1);
            border.SetValue(Canvas.TopProperty, y1);
            border.Child = line;

            return border;
        }
    }
}
