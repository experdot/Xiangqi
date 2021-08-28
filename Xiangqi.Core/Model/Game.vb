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
    Public Property MoveHistory As Queue(Of Move)
    Public Property RuleManager As RuleManager
    Public Property GameStaus As GameStaus

    Public Event OnMoved As EventHandler

    Public Sub New()
        Size = 20
        Board = New Board()
        BoardHistory = New Queue(Of Board)
        MoveHistory = New Queue(Of Move)
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

                    Dim piece = Board.PieceMap(oldLocation.X, oldLocation.Y)
                    Dim count = 0
                    Dim order = 0

                    For index = 0 To 9
                        Dim item = Board.PieceMap(oldLocation.X, index)
                        If item?.Camp = piece.Camp Then
                            If item?.PieceType = piece.PieceType Then
                                If item Is piece Then
                                    order = count
                                End If
                                count += 1
                            End If
                        End If
                    Next

                    MoveHistory.Enqueue(New Move() With {
                        .Camp = piece.Camp,
                        .PieceType = piece.PieceType,
                        .StartLocation = oldLocation,
                        .EndLocation = newLocation,
                        .VerticalCount = count,
                        .VerticalOrder = order
                    })

                    RuleManager.Move(Board.PieceMap, oldLocation, newLocation)

                    RaiseEvent OnMoved(Me, Nothing)

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
