﻿Public Class NewForm
    Private Sub NewForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ComboBox1.SelectedIndex = redPlayer
        ComboBox2.SelectedIndex = blackPlayer
        ComboBox3.SelectedIndex = aiPlayerDepth
        ComboBox4.SelectedIndex = lackMode
    End Sub

    Private Sub NewForm_Shown(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Shown
        Me.Left = Form1.Left + 25
        Me.Top = Form1.Top + 46
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        redPlayer = ComboBox1.SelectedIndex
        blackPlayer = ComboBox2.SelectedIndex
        aiPlayerDepth = ComboBox3.SelectedIndex
        lackMode = ComboBox4.SelectedIndex
        beginNewgame = True
        Me.Hide()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        beginNewgame = False
        Me.Hide()
    End Sub
End Class