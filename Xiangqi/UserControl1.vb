Public Class UserControl1

    Dim pieceMap(9, 10) As Integer
    Dim pieceName() As String = {"", "車", "馬", "炮", "象", "士", "將", "卒", " ", "車", "馬", "砲", "相", "仕", "帥", "兵"}
    Dim pieceOldx As Integer, pieceOldy As Integer, pieceNewx As Integer, pieceNewy As Integer
    Dim pieceFocus As Boolean = False '棋子是否锁定
    Dim pieceTurn As Integer = -1 '行子权,-1为红方,1为黑方
    Dim pg As Graphics
    Dim Flip As Boolean = False '棋盘是否翻转
    Public Event TurnChanged(ByVal turn As Integer, ByVal mode As Integer)

    Property pMap As Array
        Get
            Return pieceMap
        End Get
        Set(ByVal value As Array)
            pieceMap = value
        End Set
    End Property
    Property pTurn As Integer
        Get
            Return pieceTurn
        End Get
        Set(ByVal value As Integer)
            pieceTurn = value
        End Set
    End Property

    Private Sub PaintPage()
        Dim bmp As New Bitmap(Me.BackgroundImage)
        pg = Graphics.FromImage(bmp)
        For i = 0 To 9
            For j = 0 To 8
                If flip = False And Not pieceMap(j, i) = 0 Then
                    Dim myBrush As SolidBrush = IIf(pieceMap(j, i) < 8, Brushes.Black, Brushes.Red)
                    pg.FillEllipse(myBrush, 2 + j * 24, 2 + i * 24, 21, 21)
                    pg.DrawString(pieceName(pieceMap(j, i)), Me.Font, Brushes.White, 5 + j * 24, 6 + i * 24)
                ElseIf flip = True And Not pieceMap(8 - j, 9 - i) = 0 Then
                    Dim myBrush As SolidBrush = IIf(pieceMap(8 - j, 9 - i) < 8, Brushes.Black, Brushes.Red)
                    pg.FillEllipse(myBrush, 2 + j * 24, 2 + i * 24, 21, 21)
                    pg.DrawString(pieceName(pieceMap(8 - j, 9 - i)), Me.Font, Brushes.White, 5 + j * 24, 6 + i * 24)
                End If
            Next
        Next
        pg = Me.CreateGraphics
        pg.DrawImage(bmp, 0, 0)
    End Sub
    '改变行子权
    Public Sub Turnchange(ByVal TURN, ByVal mode)
        pieceTurn = TURN
        pieceFocus = False
        PaintPage()
        RaiseEvent TurnChanged(TURN, mode)
    End Sub
    '翻转棋盘
    Public Sub FlipChange()
        flip = IIf(flip = True, False, True)
        PaintPage()
    End Sub
    Private Sub UserControl1_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseClick
        Dim x, y, x2, y2 As Integer
        Dim x3, y3 As Integer
        Dim temp As Integer
        x = (e.X - 14) / 24 : y = (e.Y - 14) / 24
        If x > 8 Then x = 8 : If y > 9 Then y = 9
        If flip = True Then
            x2 = 8 - x : y2 = 9 - y
        Else
            x2 = x : y2 = y
        End If
        pg = Me.CreateGraphics
        If pieceFocus = False And (pieceMap(x2, y2) - 8) * pieceTurn < 0 And Not pieceMap(x2, y2) = 0 Then '如果没有已选择的棋子，则记录
            pieceOldx = x2
            pieceOldy = y2
            pg.DrawRectangle(Pens.Black, 2 + x * 24, 2 + y * 24, 21, 21)
            pieceFocus = True
            '  My.Computer.Audio.Play("f:\Check.wav")
            '自动画出可移动位置
            Dim Movings() As Array
            Movings = CreateSituation(pieceMap, pieceOldx, pieceOldy)
            For p = 0 To Movings.Length - 1
                x3 = IIf(Flip = True, 8 - Movings(p)(2), Movings(p)(2))
                y3 = IIf(Flip = True, 9 - Movings(p)(3), Movings(p)(3))
                If pieceMap(Movings(p)(2), Movings(p)(3)) = 0 Then
                    pg.FillEllipse(Brushes.Gray, 8 + x3 * 24, 8 + y3 * 24, 9, 9)
                Else
                    pg.DrawEllipse(New Pen(Brushes.Gray, 2), 2 + x3 * 24, 2 + y3 * 24, 21, 21)
                End If
            Next
            Exit Sub
        End If
        If pieceFocus = True Then
            pieceNewx = x2
            pieceNewy = y2
            If pieceNewx >= 0 And pieceNewy >= 0 And pieceNewx <= 8 And pieceNewy <= 9 Then
                temp = Moveable(pieceMap, pieceOldx, pieceOldy, pieceNewx, pieceNewy)
                If temp = 1 Then
                    Moving(pieceOldx, pieceOldy, pieceNewx, pieceNewy)
                ElseIf temp = 2 Then
                    pieceOldx = x2
                    pieceOldy = y2
                    PaintPage()
                    pg.DrawRectangle(Pens.Black, 2 + x * 24, 2 + y * 24, 21, 21)
                    '自动画出可移动位置
                    Dim Movings() As Array
                    Movings = CreateSituation(pieceMap, pieceOldx, pieceOldy)
                    For p = 0 To Movings.Length - 1
                        x3 = IIf(Flip = True, 8 - Movings(p)(2), Movings(p)(2))
                        y3 = IIf(Flip = True, 9 - Movings(p)(3), Movings(p)(3))
                        If pieceMap(Movings(p)(2), Movings(p)(3)) = 0 Then
                            pg.FillEllipse(Brushes.Gray, 8 + x3 * 24, 8 + y3 * 24, 9, 9)
                        Else
                            pg.DrawEllipse(New Pen(Brushes.Gray, 2), 2 + x3 * 24, 2 + y3 * 24, 21, 21)
                        End If
                    Next
                    My.Computer.Audio.Play("f:\Check.wav")
                    Exit Sub
                Else
                    Exit Sub '无效移动
                End If
                pieceFocus = False
            End If
        End If
    End Sub
    Public Sub Moving(ByVal old_x, ByVal old_y, ByVal new_x, ByVal new_y)
        If IIf(pieceMap(new_x, new_y) > 0, 1, 0) = 0 Then
            '    My.Computer.Audio.Play("f:\落子.wav")
        Else
            '   My.Computer.Audio.Play("f:\Eating.wav")
        End If
        pieceMap(new_x, new_y) = pieceMap(old_x, old_y)
        pieceMap(old_x, old_y) = 0
        If CheckKing(pieceMap, -pieceTurn) = 1 Then
            MsgBox("将会被将军")
        End If
        GameOver(CheckOver(pieceMap)) '检查局面是否结束
        Turnchange(-pieceTurn, 0) '交换行子权 
        PaintPage()
    End Sub
    Private Sub GameOver(ByVal overmode)
        If overmode = 2 Then
            MsgBox("红方胜利", , "游戏结束")
            gameIsOver = True
        ElseIf overmode = 1 Then
            MsgBox("黑方胜利", , "游戏结束")
            gameIsOver = True
        End If

    End Sub
    Private Function CheckOver(ByVal map)
        Dim temp, temp2 As Integer
        For i = 3 To 5
            If map(i, 0) = 6 Or map(i, 1) = 6 Or map(i, 2) = 6 Then
                temp = temp + 1
            End If
            If map(i, 7) = 14 Or map(i, 8) = 14 Or map(i, 9) = 14 Then
                temp2 = temp2 + 1
            End If
        Next
        If temp = 1 And temp2 = 1 Then
            Return 0
        ElseIf temp = 1 And temp2 = 0 Then
            Return 1
        Else
            Return 2
        End If
    End Function

    Private Sub UserControl1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Function CheckKing(ByVal map, ByVal turn) '检查是否将军
        Dim KingX As Integer = 0, KingY As Integer = 0
        For i = 3 To 5
            For j = 0 To 2
                If turn = -1 Then
                    If map(i, j) = 6 Then
                        KingX = i
                        KingY = j
                        Exit For
                    End If
                Else
                    If map(i, j + 7) = 14 Then
                        KingX = i
                        KingY = j + 7
                        Exit For
                    End If
                End If
            Next
        Next
        For i = 0 To 8
            For j = 0 To 9
                If (map(i, j) - 8) * turn < 0 Then
                    If Moveable(map, i, j, KingX, KingY) = 1 Then
                        'MsgBox(i & j & KingX & KingY)
                        Return 1
                    End If
                End If
            Next
        Next
        Return 0
    End Function
    Private Function Moveable(ByVal Map As Array, ByVal old_X As Integer, ByVal old_Y As Integer, ByVal new_X As Integer, ByVal new_Y As Integer)
        If (Map(old_X, old_Y) - 8) * (Map(new_X, new_Y) - 8) > 0 And Not Map(new_X, new_Y) = 0 Or (new_X = old_X And new_Y = old_Y) Then
            Return 2
        End If
        '車的移位合法判断
        If Map(old_X, old_Y) = 1 Or Map(old_X, old_Y) = 9 Then
            If new_X - old_X = 0 Or new_Y - old_Y = 0 Then
                If new_X - old_X > 1 Then
                    For i = old_X + 1 To new_X - 1
                        If Not Map(i, old_Y) = 0 Then
                            Return 0
                        End If
                    Next i
                ElseIf old_X - new_X > 1 Then
                    For i = new_X + 1 To old_X - 1
                        If Not Map(i, old_Y) = 0 Then
                            Return 0
                        End If
                    Next i
                ElseIf new_Y - old_Y > 1 Then
                    For i = old_Y + 1 To new_Y - 1
                        If Not Map(old_X, i) = 0 Then
                            Return 0
                        End If
                    Next i
                ElseIf old_Y - new_Y > 1 Then
                    For i = new_Y + 1 To old_Y - 1
                        If Not Map(old_X, i) = 0 Then
                            Return 0
                        End If
                    Next i
                End If
                Return 1
            End If
        End If
        '馬的移位合法判断
        If Map(old_X, old_Y) = 2 Or Map(old_X, old_Y) = 10 Then
            If Math.Abs((new_X - old_X) * (new_Y - old_Y)) = 2 Then
                If Math.Abs(new_X - old_X) = 2 And Map(old_X + (new_X - old_X) / 2, old_Y) = 0 Then
                    Return 1
                ElseIf Math.Abs(new_Y - old_Y) = 2 And Map(old_X, old_Y + (new_Y - old_Y) / 2) = 0 Then
                    Return 1
                End If
            End If
        End If
        '砲的移位合法判断
        If Map(old_X, old_Y) = 3 Or Map(old_X, old_Y) = 11 Then
            If new_X - old_X = 0 Or new_Y - old_Y = 0 Then
                Dim temp As Integer = 0
                If new_X - old_X > 1 Then
                    For i = old_X + 1 To new_X - 1
                        If Not Map(i, old_Y) = 0 Then
                            temp = temp + 1
                        End If
                    Next i
                ElseIf old_X - new_X > 1 Then
                    For i = new_X + 1 To old_X - 1
                        If Not Map(i, old_Y) = 0 Then
                            temp = temp + 1
                        End If
                    Next i
                ElseIf new_Y - old_Y > 1 Then
                    For i = old_Y + 1 To new_Y - 1
                        If Not Map(old_X, i) = 0 Then
                            temp = temp + 1
                        End If
                    Next i
                ElseIf old_Y - new_Y > 1 Then
                    For i = new_Y + 1 To old_Y - 1
                        If Not Map(old_X, i) = 0 Then
                            temp = temp + 1
                        End If
                    Next i
                End If
                If temp = 0 And Map(new_X, new_Y) = 0 Then
                    Return 1
                ElseIf temp = 1 And (Map(old_X, old_Y) - 8) * (Map(new_X, new_Y) - 8) < 0 And Not Map(new_X, new_Y) = 0 Then
                    Return 1
                Else
                    Return 0
                End If
            End If
        End If
        '象的移位合法判断
        If Map(old_X, old_Y) = 4 Or Map(old_X, old_Y) = 12 Then
            If Math.Abs(new_X - old_X) = 2 And Math.Abs(new_Y - old_Y) = 2 And (Map(old_X, old_Y) - 7) * (new_Y - 4.5) > 0 Then
                If Map(Int((old_X + new_X) / 2), Int((old_Y + new_Y) / 2)) = 0 Then
                    Return 1
                End If
            End If
        End If
        '仕的移位合法判断
        If (Map(old_X, old_Y) = 5 And new_Y <= 2) Or (Map(old_X, old_Y) = 13 And new_Y >= 7) Then
            If Math.Abs((new_X - old_X) * (new_Y - old_Y)) = 1 And new_X >= 3 And new_X <= 5 Then
                Return 1
            End If
        End If
        '将的移位合法判断
        If (Map(old_X, old_Y) = 6 And new_Y <= 2) Or (Map(old_X, old_Y) = 14 And new_Y >= 7) Then
            If new_X >= 3 And new_X <= 5 Then
                If new_X - old_X = 0 And Math.Abs(new_Y - old_Y) = 1 Then
                    Return 1
                ElseIf Math.Abs(new_X - old_X) = 1 And new_Y - old_Y = 0 Then
                    Return 1
                End If
            End If
        End If
        '卒的移位合法判断
        If Map(old_X, old_Y) = 7 Or Map(old_X, old_Y) = 15 Then
            If (Map(old_X, old_Y) - 8) * (old_Y - 4.5) > 0 Then '己方阵营内
                If new_X - old_X = 0 And Math.Abs(new_Y - old_Y) = 1 And (Map(old_X, old_Y) - 8) * (new_Y - old_Y) < 0 Then
                    Return 1
                End If
            ElseIf (Map(old_X, old_Y) - 8) * (old_Y - 4.5) < 0 Then '对方阵营内
                If (old_Y - 4.5) * (new_Y - old_Y) >= 0 And Math.Abs(new_Y - old_Y) + Math.Abs(new_X - old_X) = 1 Then
                    Return 1
                End If
            End If
        End If
        Return 0
    End Function
    '  Private isMouseDown As Boolean = False
    '  Private mouseOffset As Point '记录鼠标指针坐标
    ' Private Sub UserControl1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
    '  If e.Button = Windows.Forms.MouseButtons.Left Then
    '     mouseOffset.X = e.X
    '     mouseOffset.Y = e.Y
    '      isMouseDown = True
    '  End If
    ' End Sub

    'Private Sub UserControl1_MouseLeave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MouseEnter
    '    Me.Left = 8
    '     Me.Top = 8
    ' End Sub


    'Private Sub UserControl1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
    '    If isMouseDown = True Then
    'Dim left As integer = Me.Left ' + e.X - mouseOffset.X
    ' Dim top As integer = Me.Top + e.Y - mouseOffset.Y
    '        Me.Location = New Point(left, top)
    '     End If
    ' End Sub

    ' Private Sub UserControl1_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseUp
    '    If e.Button = Windows.Forms.MouseButtons.Left Then
    '       isMouseDown = False
    '       Me.Left = 8
    '       Me.Top = 8
    '   End If
    ' End Sub

    ' Private Sub UserControl1_MouseWheel(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseWheel
    '   Me.Top = Me.Top + e.Delta / 12

    'End Sub
    Private Function CreateSituation(ByVal map As Array, ByVal X As Integer, ByVal Y As Integer) '寻找相关合法移位,并返回新局面
        Dim Movings(100) As Array
        Dim J As Integer = 0
        '車的移位合法判断
        If map(X, Y) = 1 Or map(X, Y) = 9 Then
            If X <= 7 Then
                For i = X + 1 To 8 '向右寻找
                    If map(i, Y) = 0 Then
                        Movings(J) = {X, Y, i, Y}
                        J = J + 1
                    ElseIf (map(i, Y) - 8) * (map(X, Y) - 8) < 0 And Not map(i, Y) = 0 Then
                        Movings(J) = {X, Y, i, Y}
                        J = J + 1
                        Exit For
                    Else
                        Exit For
                    End If
                Next
            End If
            If X >= 1 Then
                For i = 0 To X - 1 '向左寻找
                    If map(X - i - 1, Y) = 0 Then
                        Movings(J) = {X, Y, X - i - 1, Y}
                        J = J + 1
                    ElseIf (map(X - 1 - i, Y) - 8) * (map(X, Y) - 8) < 0 And Not map(X - i - 1, Y) = 0 Then
                        Movings(J) = {X, Y, X - i - 1, Y}
                        J = J + 1
                        Exit For
                    Else
                        Exit For
                    End If
                Next
            End If
            If Y <= 8 Then
                For i = Y + 1 To 9 '向下寻找
                    If map(X, i) = 0 Then
                        Movings(J) = {X, Y, X, i}
                        J = J + 1
                    ElseIf (map(X, i) - 8) * (map(X, Y) - 8) < 0 And Not map(X, i) = 0 Then
                        Movings(J) = {X, Y, X, i}
                        J = J + 1
                        Exit For
                    Else
                        Exit For
                    End If

                Next
            End If
            If Y >= 1 Then
                For i = 0 To Y - 1 '向上寻找
                    If map(X, Y - i - 1) = 0 Then
                        Movings(J) = {X, Y, X, Y - i - 1}
                        J = J + 1
                    ElseIf (map(X, Y - i - 1) - 8) * (map(X, Y) - 8) < 0 And Not map(X, Y - i - 1) = 0 Then
                        Movings(J) = {X, Y, X, Y - i - 1}
                        J = J + 1
                        Exit For
                    Else
                        Exit For
                    End If
                Next
            End If
            ReDim Preserve Movings(J - 1)
            Return Movings
        End If

        '馬的移位合法判断
        If map(X, Y) = 2 Or map(X, Y) = 10 Then
            If X >= 2 And Y <= 8 Then '向左下寻找            
                If (map(X - 2, Y + 1) = 0 Or (map(X - 2, Y + 1) - 8) * (map(X, Y) - 8) < 0) And map(X - 1, Y) = 0 Then
                    Movings(J) = {X, Y, X - 2, Y + 1}
                    J = J + 1
                End If
            End If
            If X >= 2 And Y >= 1 Then '向左上寻找
                If (map(X - 2, Y - 1) = 0 Or (map(X - 2, Y - 1) - 8) * (map(X, Y) - 8) < 0) And map(X - 1, Y) = 0 Then
                    Movings(J) = {X, Y, X - 2, Y - 1}

                    J = J + 1
                End If
            End If
            If Y <= 7 And X >= 1 Then '向下左寻找
                If (map(X - 1, Y + 2) = 0 Or (map(X - 1, Y + 2) - 8) * (map(X, Y) - 8) < 0) And map(X, Y + 1) = 0 Then
                    Movings(J) = {X, Y, X - 1, Y + 2}
                    J = J + 1
                End If
            End If
            If Y <= 7 And X <= 7 Then '向下右寻找
                If (map(X + 1, Y + 2) = 0 Or (map(X + 1, Y + 2) - 8) * (map(X, Y) - 8) < 0) And map(X, Y + 1) = 0 Then
                    Movings(J) = {X, Y, X + 1, Y + 2}
                    J = J + 1
                End If
            End If
            If X <= 6 And Y <= 8 Then '向右下寻找
                If (map(X + 2, Y + 1) = 0 Or (map(X + 2, Y + 1) - 8) * (map(X, Y) - 8) < 0) And map(X + 1, Y) = 0 Then
                    Movings(J) = {X, Y, X + 2, Y + 1}
                    J = J + 1
                End If
            End If
            If X <= 6 And Y >= 1 Then '向右上寻找
                If (map(X + 2, Y - 1) = 0 Or (map(X + 2, Y - 1) - 8) * (map(X, Y) - 8) < 0) And map(X + 1, Y) = 0 Then
                    Movings(J) = {X, Y, X + 2, Y - 1}
                    J = J + 1
                End If
            End If
            If Y >= 2 And X >= 1 Then '向上左寻找
                If (map(X - 1, Y - 2) = 0 Or (map(X - 1, Y - 2) - 8) * (map(X, Y) - 8) < 0) And map(X, Y - 1) = 0 Then
                    Movings(J) = {X, Y, X - 1, Y - 2}
                    J = J + 1
                End If
            End If
            If Y >= 2 And X <= 7 Then '向上右寻找
                If (map(X + 1, Y - 2) = 0 Or (map(X + 1, Y - 2) - 8) * (map(X, Y) - 8) < 0) And map(X, Y - 1) = 0 Then
                    Movings(J) = {X, Y, X + 1, Y - 2}
                    J = J + 1
                End If
            End If
            ReDim Preserve Movings(J - 1)
            Return Movings
        End If


        '砲的移位合法判断
        If map(X, Y) = 3 Or map(X, Y) = 11 Then

            If X <= 7 Then
                For i = X + 1 To 8 '向右寻找
                    If map(i, Y) = 0 Then
                        Movings(J) = {X, Y, i, Y}
                        J = J + 1

                    Else
                        If i < 8 Then
                            For p = i + 1 To 8
                                If Not map(p, Y) = 0 Then
                                    If (map(p, Y) - 8) * (map(X, Y) - 8) < 0 Then
                                        Movings(J) = {X, Y, p, Y}
                                        J = J + 1
                                    End If
                                    Exit For
                                End If
                            Next
                            Exit For
                        End If
                    End If
                Next
            End If

            If X >= 1 Then
                For i = 0 To X - 1 '向左寻找
                    If map(X - i - 1, Y) = 0 Then
                        Movings(J) = {X, Y, X - i - 1, Y}
                        J = J + 1
                    Else
                        If i < X - 1 Then
                            For p = i + 1 To X - 1
                                If Not map(X - p - 1, Y) = 0 Then
                                    If (map(X - p - 1, Y) - 8) * (map(X, Y) - 8) < 0 Then
                                        Movings(J) = {X, Y, X - p - 1, Y}
                                        J = J + 1
                                    End If
                                    Exit For
                                End If
                            Next
                            Exit For
                        End If
                    End If
                Next
            End If

            If Y <= 8 Then
                For i = Y + 1 To 9 '向下寻找
                    If map(X, i) = 0 Then
                        Movings(J) = {X, Y, X, i}
                        J = J + 1
                    Else
                        If i < 9 Then
                            For p = i + 1 To 9
                                If Not map(X, p) = 0 Then
                                    If (map(X, p) - 8) * (map(X, Y) - 8) < 0 Then
                                        Movings(J) = {X, Y, X, p}
                                        J = J + 1
                                    End If
                                    Exit For
                                End If
                            Next
                            Exit For
                        End If
                    End If
                Next
            End If
            If Y >= 1 Then
                For i = 0 To Y - 1 '向上寻找
                    If map(X, Y - i - 1) = 0 Then
                        Movings(J) = {X, Y, X, Y - i - 1}
                        J = J + 1
                    Else
                        If i < Y - 1 Then
                            For p = i + 1 To Y - 1
                                If Not map(X, Y - p - 1) = 0 Then
                                    If (map(X, Y - p - 1) - 8) * (map(X, Y) - 8) < 0 Then
                                        Movings(J) = {X, Y, X, Y - p - 1}
                                        J = J + 1
                                    End If
                                    Exit For
                                End If
                            Next
                            Exit For
                        End If
                    End If
                Next
            End If
            ReDim Preserve Movings(J - 1)
            Return Movings
        End If
        '象的移位合法判断
        If map(X, Y) = 4 Or map(X, Y) = 12 Then
            '向左上搜索
            If X >= 2 And ((Y >= 2 And map(X, Y) = 4) Or (Y >= 7 And map(X, Y) = 12)) Then
                If map(X - 1, Y - 1) = 0 And (map(X - 2, Y - 2) = 0 Or (map(X - 2, Y - 2) - 8) * (map(X, Y) - 8) < 0) Then
                    Movings(J) = {X, Y, X - 2, Y - 2}
                    J = J + 1
                End If
            End If
            '向左下搜索
            If X >= 2 And ((Y <= 2 And map(X, Y) = 4) Or (Y <= 7 And map(X, Y) = 12)) Then
                If map(X - 1, Y + 1) = 0 And (map(X - 2, Y + 2) = 0 Or (map(X - 2, Y + 2) - 8) * (map(X, Y) - 8) < 0) Then
                    Movings(J) = {X, Y, X - 2, Y + 2}
                    J = J + 1
                End If
            End If
            '  向右上寻找
            If X <= 6 And ((Y >= 2 And map(X, Y) = 4) Or (Y >= 7 And map(X, Y) = 12)) Then
                If map(X + 1, Y - 1) = 0 And (map(X + 2, Y - 2) = 0 Or (map(X + 2, Y - 2) - 8) * (map(X, Y) - 8) < 0) Then
                    Movings(J) = {X, Y, X + 2, Y - 2}
                    J = J + 1
                End If
            End If
            '向右下搜索
            If X <= 6 And ((Y <= 2 And map(X, Y) = 4) Or (Y <= 7 And map(X, Y) = 12)) Then

                If map(X + 1, Y + 1) = 0 And (map(X + 2, Y + 2) = 0 Or (map(X + 2, Y + 2) - 8) * (map(X, Y) - 8) < 0) Then
                    Movings(J) = {X, Y, X + 2, Y + 2}
                    J = J + 1
                End If
            End If
            ReDim Preserve Movings(J - 1)
            Return Movings
        End If

        '仕的移位合法判断
        If map(X, Y) = 5 Or map(X, Y) = 13 Then
            '向左上搜索
            If X >= 4 And ((Y >= 1 And map(X, Y) = 5) Or (Y >= 8 And map(X, Y) = 13)) Then
                If map(X - 1, Y - 1) = 0 Or (map(X - 1, Y - 1) - 8) * (map(X, Y) - 8) < 0 Then
                    Movings(J) = {X, Y, X - 1, Y - 1}
                    J = J + 1
                End If
            End If
            '向左下搜索
            If X >= 4 And ((Y <= 1 And map(X, Y) = 5) Or (Y <= 8 And map(X, Y) = 13)) Then
                If map(X - 1, Y + 1) = 0 Or (map(X - 1, Y + 1) - 8) * (map(X, Y) - 8) < 0 Then
                    Movings(J) = {X, Y, X - 1, Y + 1}
                    J = J + 1
                End If
            End If
            '向右上搜索
            If X <= 4 And ((Y >= 1 And map(X, Y) = 5) Or (Y >= 8 And map(X, Y) = 13)) Then
                If map(X + 1, Y - 1) = 0 Or (map(X + 1, Y - 1) - 8) * (map(X, Y) - 8) < 0 Then
                    Movings(J) = {X, Y, X + 1, Y - 1}
                    J = J + 1
                End If
            End If
            '向右下搜索
            If X <= 4 And ((Y <= 1 And map(X, Y) = 5) Or (Y <= 8 And map(X, Y) = 13)) Then
                If map(X + 1, Y + 1) = 0 Or (map(X + 1, Y + 1) - 8) * (map(X, Y) - 8) < 0 Then
                    Movings(J) = {X, Y, X + 1, Y + 1}
                    J = J + 1
                End If
            End If
            ReDim Preserve Movings(J - 1)
            Return Movings
        End If
        '将的移位合法判断
        If map(X, Y) = 6 Or map(X, Y) = 14 Then
            '向上搜索
            If (Y >= 1 And map(X, Y) = 6) Or (Y >= 8 And map(X, Y) = 14) Then
                If map(X, Y - 1) = 0 Or (map(X, Y - 1) - 8) * (map(X, Y) - 8) < 0 Then
                    Movings(J) = {X, Y, X, Y - 1}
                    J = J + 1
                End If
            End If
            '向下搜索
            If (Y <= 1 And map(X, Y) = 6) Or (Y <= 8 And map(X, Y) = 14) Then
                If map(X, Y + 1) = 0 Or (map(X, Y + 1) - 8) * (map(X, Y) - 8) < 0 Then
                    Movings(J) = {X, Y, X, Y + 1}
                    J = J + 1
                End If
            End If
            '向左搜索
            If X >= 4 Then
                If map(X - 1, Y) = 0 Or (map(X - 1, Y) - 8) * (map(X, Y) - 8) < 0 Then
                    Movings(J) = {X, Y, X - 1, Y}
                    J = J + 1
                End If
            End If
            '向右搜索
            If X <= 4 Then
                If map(X + 1, Y) = 0 Or (map(X + 1, Y) - 8) * (map(X, Y) - 8) < 0 Then
                    Movings(J) = {X, Y, X + 1, Y}
                    J = J + 1
                End If
            End If
            ReDim Preserve Movings(J - 1)
            Return Movings
        End If
        '卒的移位合法判断
        If map(X, Y) = 7 Or map(X, Y) = 15 Then
            '向下搜索
            If map(X, Y) = 7 And Y <= 8 Then
                If map(X, Y + 1) = 0 Or (map(X, Y + 1) - 8) * (map(X, Y) - 8) < 0 Then
                    Movings(J) = {X, Y, X, Y + 1}
                    J = J + 1
                End If
            End If
            '向上搜索
            If map(X, Y) = 15 And Y >= 1 Then
                If map(X, Y - 1) = 0 Or (map(X, Y - 1) - 8) * (map(X, Y) - 8) < 0 Then
                    Movings(J) = {X, Y, X, Y - 1}
                    J = J + 1
                End If
            End If
            '向左搜索
            If (map(X, Y) - 8) * (Y - 4.5) < 0 And X >= 1 Then
                If map(X - 1, Y) = 0 Or (map(X - 1, Y) - 8) * (map(X, Y) - 8) < 0 Then
                    Movings(J) = {X, Y, X - 1, Y}
                    J = J + 1
                End If
            End If
            '向右搜索
            If (map(X, Y) - 8) * (Y - 4.5) < 0 And X <= 7 Then
                If map(X + 1, Y) = 0 Or (map(X + 1, Y) - 8) * (map(X, Y) - 8) < 0 Then
                    Movings(J) = {X, Y, X + 1, Y}
                    J = J + 1
                End If
            End If
            ReDim Preserve Movings(J - 1)
            Return Movings
        End If
        ReDim Preserve Movings(J - 1)
        Return Movings
    End Function
End Class
