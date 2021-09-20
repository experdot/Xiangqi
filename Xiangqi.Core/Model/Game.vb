Imports System.Numerics

Public Enum GameStaus
    Ready
    Running
    Paused
    Stopped
End Enum

Public Class OnMovedEventArgs
    Public OldLocation As Vector2
    Public NewLocation As Vector2
    Public Piece As Piece
    Public DestroyedPiece As Piece
End Class

Public Class XiangqiGame
    Public Property Size As Integer
    Public Property Board As Board
    Public Property BoardStepHistory As Queue(Of BoardStep)
    Public Property RuleManager As RuleManager
    Public Property GameStaus As GameStaus

    Public Event OnMoved As EventHandler(Of OnMovedEventArgs)

    Public Sub New()
        Size = 20
        Board = New Board()
        BoardStepHistory = New Queue(Of BoardStep)
        RuleManager = New RuleManager()
    End Sub

    Public Sub Start()
        GameStaus = GameStaus.Running
        RuleManager.Initialize(Board)

        BoardStepHistory.Clear()
        Dim boardStep As BoardStep = BoardStep.GenerateBoardStep(0, Board, Nothing, Nothing)
        BoardStepHistory.Enqueue(boardStep)
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
        If Not GameStaus = GameStaus.Running Then
            Return
        End If

        If Board.SelectedPiece Is Nothing Then
            Dim target = Board.PieceMap(location.X, location.Y)
            If target?.Camp = Board.Camp Then
                Board.SelectedPiece = Board.PieceMap(location.X, location.Y)
            End If
        Else
            Dim oldLocation = Board.SelectedPiece.Location
            Dim newLocation = location

            Dim target = Board.PieceMap(location.X, location.Y)
            If target?.Camp = Board.SelectedPiece.Camp Then
                Board.SelectedPiece = Board.PieceMap(location.X, location.Y)
            Else
                Dim moveable As Boolean = RuleManager.GetMoveable(Board.PieceMap, oldLocation, newLocation)
                If moveable Then
                    Dim boardStep As BoardStep = BoardStep.GenerateBoardStep(BoardStepHistory.Count + 1, Board, oldLocation, newLocation)
                    BoardStepHistory.Enqueue(boardStep)

                    Dim destroyedPiece = Board.PieceMap(newLocation.X, newLocation.Y)

                    RuleManager.Move(Board.PieceMap, oldLocation, newLocation)

                    RaiseEvent OnMoved(Me, New OnMovedEventArgs() With {
                        .OldLocation = oldLocation,
                        .NewLocation = newLocation,
                        .Piece = Board.PieceMap(oldLocation.X, oldLocation.Y),
                        .DestroyedPiece = destroyedPiece
                    })

                    If (RuleManager.GetIsOver(Board.PieceMap)) Then
                        [Stop]()
                    Else
                        Board.ExchangeCamp()
                        Board.SelectedPiece = Nothing
                    End If
                End If
            End If
        End If
    End Sub



    Public Sub GoBack()

    End Sub

    Public Sub GoForward()

    End Sub
End Class
