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
    Public Property BoardHistory As Queue(Of Board)
    Public Property MoveHistory As Queue(Of Move)
    Public Property RuleManager As RuleManager
    Public Property GameStaus As GameStaus

    Public Event OnMoved As EventHandler(Of OnMovedEventArgs)

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
                    Dim move As Move = GenerateMove(Board, oldLocation, newLocation)
                    MoveHistory.Enqueue(move)

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

    Private Function GenerateMove(board As Board, oldLocation As Vector2, newLocation As Vector2) As Move
        Dim count = 0
        Dim order = 0

        Dim piece = board.PieceMap(oldLocation.X, oldLocation.Y)

        If piece.PieceType = PieceType.Pawn Then
            Dim pieceList = New List(Of Piece)
            For x = 8 To 0 Step -1
                For y = 0 To 9
                    Dim item = board.PieceMap(x, y)
                    If item?.Camp = piece.Camp Then
                        If item?.PieceType = piece.PieceType Then
                            pieceList.Add(item)
                        End If
                    End If
                Next
            Next
            Dim pieceGroups = pieceList.GroupBy(Of Integer)(Function(p As Piece)
                                                                Return p.Location.X
                                                            End Function)
            For Each group In pieceGroups
                Dim index = group.ToList().IndexOf(piece)
                If index >= 0 Then
                    If group.Count = 1 Then
                        count = 1
                        order = 0
                        Exit For
                    Else
                        order = count + index
                    End If
                End If

                If group.Count > 1 Then
                    count += group.Count
                End If
            Next
        Else
            For index = 0 To 9
                Dim item = board.PieceMap(oldLocation.X, index)
                If item?.Camp = piece.Camp Then
                    If item?.PieceType = piece.PieceType Then
                        If item Is piece Then
                            order = count
                        End If
                        count += 1
                    End If
                End If
            Next
        End If

        If piece.Camp = Camp.Black Then
            order = count - order - 1
        End If

        Dim move = New Move() With {
            .Camp = piece.Camp,
            .PieceType = piece.PieceType,
            .StartLocation = oldLocation,
            .EndLocation = newLocation,
            .VerticalCount = count,
            .VerticalOrder = order
        }
        Return move
    End Function

    Public Sub GoBack()

    End Sub

    Public Sub GoForward()

    End Sub
End Class
