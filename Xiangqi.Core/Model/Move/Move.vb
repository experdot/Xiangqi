Imports System.Numerics

Public Class Move
    Public StartLocation As Vector2
    Public EndLocation As Vector2
    Public VerticalOrder As Integer
    Public VerticalCount As Integer
    Public PieceType As PieceType
    Public Camp As Camp

    Public Shared Function GenerateMove(board As Board, oldLocation As Vector2, newLocation As Vector2) As Move
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

End Class
