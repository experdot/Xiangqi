Public Class UserControl1
    Public Event TurnChanged(ByVal turn As Integer, ByVal mode As Integer)

    Dim Game As New XiangqiGame

    Private Sub PaintPage()
        Dim graphics = Me.CreateGraphics()
        Dim Painter = New Painter(graphics)
        Painter.Draw(Game.Board.PieceMap)
    End Sub

    Public Sub Start()
        Game.Start()
        PaintPage()
    End Sub

    Private Sub UserControl1_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseClick
        Dim size As Integer = 64
        Dim border As Single = size / 16
        Dim x, y As Integer
        x = (e.X - 14) / size : y = (e.Y - 14) / size
        If x > 8 Then x = 8 : If y > 9 Then y = 9

        Game.Move(New Numerics.Vector2(x, y))

        PaintPage()
    End Sub
End Class
