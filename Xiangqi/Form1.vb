Imports System.Threading
Public Class Form1
    Dim StartTime As Date '新游戏开始时间

    Private Sub UserControl1_TurnChanged(ByVal turn As Object, ByVal mode As Integer) Handles UserControl1.TurnChanged
        Dim pg As Graphics = PictureBox1.CreateGraphics
        If turn = 1 Then
            pg.FillEllipse(Brushes.Black, 0, 0, 21, 21)
            pg.DrawString("將", Me.Font, Brushes.White, 3, 4)
        Else
            pg.FillEllipse(Brushes.Red, 0, 0, 21, 21)
            pg.DrawString("帥", Me.Font, Brushes.White, 3, 4)
        End If
        pg.Dispose()
        '记录走步
        If mode = 0 Then
            walkstep = walkstep + 1
        End If

        ToolStripLabel4.Text = walkstep

        If walkstep >= stepUP Or mode = 0 Then
            ReDim Preserve walkline(9, 10, walkstep)
            stepUP = walkstep
        End If
        If mode = 0 Then
            For x = 0 To 8
                For y = 0 To 9
                    walkline(x, y, walkstep) = UserControl1.pMap(x, y)
                Next
            Next
            '检查AI移动
            If ((UserControl1.pTurn = -1 And redPlayer = 1) Or (UserControl1.pTurn = 1 And blackPlayer = 1)) And gameIsOver = False Then
                'Dim movings() As Integer
                'movings = AiMove(UserControl1.pMap, UserControl1.pTurn, aiPlayerDepth)
                'UserControl1.Moving(movings(0), movings(1), movings(2), movings(3))
                Dim tempThread As New Thread(AddressOf aiThread)
                tempThread.Start()
            End If
        End If
    End Sub
    Sub aiThread()
        Dim movings() As Integer
        movings = AiMove(UserControl1.pMap, UserControl1.pTurn, aiPlayerDepth)
        UserControl1.Moving(movings(0), movings(1), movings(2), movings(3))
    End Sub
    Private Sub Initialise() '初始化棋盘
        Dim NewPosition() As Byte = {
 1, 2, 4, 5, 6, 5, 4, 2, 1,
 0, 0, 0, 0, 0, 0, 0, 0, 0,
 0, 3, 0, 0, 0, 0, 0, 3, 0,
 7, 0, 7, 0, 7, 0, 7, 0, 7,
 0, 0, 0, 0, 0, 0, 0, 0, 0,
 0, 0, 0, 0, 0, 0, 0, 0, 0,
 15, 0, 15, 0, 15, 0, 15, 0, 15,
 0, 11, 0, 0, 0, 0, 0, 11, 0,
 0, 0, 0, 0, 0, 0, 0, 0, 0,
 9, 10, 12, 13, 14, 13, 12, 10, 9
 }
        For i = 0 To 9
            For j = 0 To 8
                UserControl1.pMap(j, i) = NewPosition(i * 9 + j)
            Next
        Next
        '  My.Computer.Audio.Play("f:\NewGame.wav")
    End Sub
    '菜单
    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click
        MenuForm.Visible = True
        MenuForm.Left = Me.Left + 18
        MenuForm.Top = Me.Top + 90
    End Sub

    '新的游戏
    Private Sub ToolStripButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton2.Click
        NewForm.ShowDialog()
        If beginNewgame = True Then
            gameIsOver = False
            walkstep = -1
            Initialise()
            UserControl1.Turnchange(-1, 0)
            StartTime = Now
            Timer1.Enabled = True
        End If
    End Sub
    '设置
    Private Sub ToolStripButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton3.Click
        SettingForm.ShowDialog()
    End Sub
    '悔棋
    Private Sub ToolStripButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton4.Click
        If walkstep > 0 Then
            For x = 0 To 8
                For y = 0 To 9
                    UserControl1.pMap(x, y) = walkline(x, y, walkstep - 1)
                Next
            Next
            walkstep = walkstep - 1
            UserControl1.Turnchange(-UserControl1.pTurn, 1)
            '  My.Computer.Audio.Play("f:\落子.wav")
            gameIsOver = False
        End If
    End Sub
    '提示
    Private Sub ToolStripButton5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton5.Click
        Dim movings() As Integer
        If gameIsOver = False Then
            movings = AiMove(UserControl1.pMap, UserControl1.pTurn, aiHelperDepth)
            UserControl1.Moving(movings(0), movings(1), movings(2), movings(3))
        End If
    End Sub

    '记录时间
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        ToolStripLabel2.Text = Format(TimeSerial(0, 0, DateDiff("s", StartTime, Now)), "mm:ss")
    End Sub

    Private Sub UserControl1_Load(sender As Object, e As EventArgs) Handles UserControl1.Load
        Dim XiangqiGame = New XiangqiGame
        XiangqiGame.Start()
    End Sub
End Class