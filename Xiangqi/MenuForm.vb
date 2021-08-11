Public Class MenuForm
    Private Sub MenuForm_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LostFocus
        Me.Hide()
    End Sub
    '翻转棋盘
    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click
        Form1.UserControl1.FlipChange()
    End Sub
    '载入对局
    Private Sub ToolStripButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton2.Click

    End Sub
    '保存对局
    Private Sub ToolStripButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton3.Click

    End Sub
    '摆设棋局
    Private Sub ToolStripButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton4.Click

    End Sub
    '查看棋谱
    Private Sub ToolStripButton5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton5.Click
        StepForm.Visible = True
        StepForm.Left = Me.Left + 18
        StepForm.Top = Me.Top + 90
    End Sub
    '功能设置
    Private Sub ToolStripButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton6.Click
        MsgBox("暂无功能设置", , "功能设置")
        Form1.Focus()
    End Sub
    '软件信息
    Private Sub ToolStripButton7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton7.Click
        MsgBox("中国象棋v1.0开源程序", , "软件信息")
        Form1.Focus()
    End Sub
    '退出游戏
    Private Sub ToolStripButton8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton8.Click
        Form1.Dispose()
    End Sub
End Class