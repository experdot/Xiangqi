Public Class Painter
    Public Property Filp As Boolean
    Public Property Graphics As Graphics
    Public Property Size As Integer = 64
    Public Property PieceName As Dictionary(Of Camp, String())

    Public Sub New(graphics As Graphics)
        Me.Graphics = graphics
        PieceName = New Dictionary(Of Camp, String())
        PieceName(Camp.Red) = {"車", "馬", "砲", "相", "仕", "帥", "兵"}
        PieceName(Camp.Black) = {"車", "馬", "炮", "象", "士", "將", "卒"}
    End Sub

    Public Sub Draw(board As Board)
        Dim pieceMap = board.PieceMap
        Dim width = CInt(Size * (pieceMap.GetUpperBound(0) + 1))
        Dim height = CInt(Size * (pieceMap.GetUpperBound(1) + 1))
        Dim font = New Font(New FontFamily("微软雅黑"), Size / 3)
        Dim bmp As New Bitmap(width, height)
        Dim pg = Graphics.FromImage(bmp)
        pg.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        pg.Clear(Color.Gray)
        Dim border As Single = Size / 16
        For i = 0 To 9
            For j = 0 To 8
                Dim target = pieceMap(j, i)
                If target IsNot Nothing Then
                    Dim myBrush As SolidBrush = IIf(pieceMap(j, i).Camp = Camp.Black, Brushes.Black, Brushes.Red)
                    pg.FillEllipse(myBrush, border + j * Size, border + i * Size, Size - border * 2, Size - border * 2)
                    Dim text = PieceName(target.Camp)(target.PieceType)
                    Dim format = New StringFormat() With
                    {
                        .Alignment = StringAlignment.Center,
                        .LineAlignment = StringAlignment.Center
                    }
                    pg.DrawString(text, font, Brushes.White, (j + 0.5) * Size, (i + 0.5) * Size, format)
                End If
            Next
        Next

        If (board.SelectedPiece IsNot Nothing) Then
            Dim pen As Pen = Pens.Blue
            Dim x = board.SelectedPiece.Location.X
            Dim y = board.SelectedPiece.Location.Y
            pg.DrawEllipse(pen, border + x * Size, border + y * Size, Size - border * 2, Size - border * 2)
        End If

        Graphics.DrawImage(bmp, 0, 0)
    End Sub
End Class
