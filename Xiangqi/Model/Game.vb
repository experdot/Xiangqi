Imports System.Numerics

Public Enum GameStaus
    Ready
    Running
    Paused
    Stopped
End Enum

Public Class XiangqiGame
    Public Property Size As Integer
    Public Property Board As Board
    Public Property BoardHistory As Queue(Of Board)
    Public Property RuleManager As RuleManager
    Public Property GameStaus As GameStaus

    Public Sub New()
        Size = 20
        Board = New Board()
        RuleManager = New RuleManager()
    End Sub

    Public Sub Start()
        GameStaus = GameStaus.Running
        RuleManager.Initialize(Board)
    End Sub

    Public Sub [Stop]()
        GameStaus = GameStaus.Stopped
    End Sub

    Public Sub Pause()
        GameStaus = GameStaus.Paused
    End Sub

    Public Sub [Resume]()
        GameStaus = GameStaus.Running
    End Sub

    Public Sub Move(location As Vector2)
        If Board.SelectedPiece Is Nothing Then
            Dim target = Board.PieceMap(location.X, location.Y)
            If target?.Camp = Board.Camp Then
                Board.SelectedPiece = Board.PieceMap(location.X, location.Y)
            End If
        Else
            Dim oldLocation = Board.SelectedPiece.Location
            Dim newLocation = location
            Dim moveable As Boolean = RuleManager.GetMoveable(Board.PieceMap, oldLocation, newLocation)
            If moveable Then
                RuleManager.Move(Board.PieceMap, oldLocation, newLocation)
                If (RuleManager.GetIsOver(Board.PieceMap)) Then
                    [Stop]()
                Else
                    Board.ExchangeCamp()
                    Board.SelectedPiece = Nothing
                End If
            End If
        End If

    End Sub

    Public Sub GoBack()

    End Sub

    Public Sub GoForward()

    End Sub
End Class
