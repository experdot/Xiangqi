Public Class StepForm

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Me.Close()
    End Sub

    Private Sub StepForm_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        ListBox1.Items.Clear()
        For i = 0 To stepUP
            ListBox1.Items.Add("第" & i & "步")
        Next
        ListBox1.SelectedIndex = walkstep
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged
        For x = 0 To 8
            For y = 0 To 9
                Form1.UserControl1.pMap(x, y) = walkline(x, y, ListBox1.SelectedIndex)
            Next
        Next
        walkstep = ListBox1.SelectedIndex
        Form1.UserControl1.Turnchange(IIf(ListBox1.SelectedIndex Mod 2 = 1, 1, -1), 1)
        My.Computer.Audio.Play("f:\落子.wav")
        gameIsOver = False
    End Sub
End Class