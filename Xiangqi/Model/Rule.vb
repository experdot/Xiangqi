Imports System.Numerics

Public Class RuleManager
    Public Sub Initialize(board As Board)
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
        ReDim board.PieceMap(8, 9)
        For i = 0 To 9
            For j = 0 To 8
                Dim position = NewPosition(i * 9 + j)
                If Not position = 0 Then
                    board.PieceMap(j, i) = New Piece() With
                    {
                        .Location = New Vector2(j, i),
                        .Camp = position \ 8,
                        .PieceType = (position Mod 8) - 1
                    }
                End If
            Next
        Next
    End Sub
    Public Function GetMoveable(map As Piece(,), oldLocation As Vector2, newLocation As Vector2) As Boolean
        Return True
    End Function

    Public Sub Move(map As Piece(,), oldLocation As Vector2, newLocation As Vector2)
        map(newLocation.X, newLocation.Y) = map(oldLocation.X, oldLocation.Y)
        map(newLocation.X, newLocation.Y).Location = New Vector2(newLocation.X, newLocation.Y)
        map(oldLocation.X, oldLocation.Y) = Nothing
    End Sub

    Public Function GetIsOver(map As Piece(,)) As Boolean
        Return False
    End Function
End Class

Public Class Rule
    Public Function GetMoveable(map As Piece(,), oldLocation As Vector2, newLocation As Vector2) As Boolean
        Return True
    End Function
End Class
