Imports System.Numerics
Imports System.Collections

Public Interface IRule
    Function GetMoveable(map As Piece(,), oldLocation As Vector2, newLocation As Vector2) As Boolean
End Interface

Public Class RuleManager
    Public Rules As New Dictionary(Of PieceType, IRule)

    Public Sub Initialize(board As Board)
        Dim NewPosition() As Byte = {
            5, 4, 3, 2, 1, 2, 3, 4, 5,
            0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 6, 0, 0, 0, 0, 0, 6, 0,
            7, 0, 7, 0, 7, 0, 7, 0, 7,
            0, 0, 0, 0, 0, 0, 0, 0, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0,
            15, 0, 15, 0, 15, 0, 15, 0, 15,
            0, 14, 0, 0, 0, 0, 0, 14, 0,
            0, 0, 0, 0, 0, 0, 0, 0, 0,
            13, 12, 11, 10, 9, 10, 11, 12, 13
        }
        ReDim board.PieceMap(8, 9)
        For i = 0 To 9
            For j = 0 To 8
                Dim position = NewPosition(i * 9 + j)
                If Not position = 0 Then
                    board.PieceMap(j, i) = New Piece() With
                    {
                        .Location = New Vector2(j, i),
                        .Camp = 1 - (position \ 8),
                        .PieceType = (position Mod 8) - 1
                    }
                End If
            Next
        Next

        Rules(PieceType.King) = New KingRule()
        Rules(PieceType.Adviser) = New AdviserRule()
        Rules(PieceType.Elephant) = New ElephantRule()
        Rules(PieceType.Horse) = New HorseRule()
        Rules(PieceType.Chariot) = New ChariotRule()
        Rules(PieceType.Cannon) = New CannonRule()
        Rules(PieceType.Pawn) = New PawnRule()
    End Sub
    Public Function GetMoveable(map As Piece(,), oldLocation As Vector2, newLocation As Vector2) As Boolean
        Return Rules(map(oldLocation.X, oldLocation.Y).PieceType).GetMoveable(map, oldLocation, newLocation)
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

Public Class ChariotRule
    Implements IRule
    Public Function GetMoveable(map As Piece(,), oldLocation As Vector2, newLocation As Vector2) As Boolean Implements IRule.GetMoveable
        Dim old_X = oldLocation.X, old_Y = oldLocation.Y, new_X = newLocation.X, new_Y = newLocation.Y
        If new_X - old_X = 0 Or new_Y - old_Y = 0 Then
            If new_X - old_X > 1 Then
                For i = old_X + 1 To new_X - 1
                    If Not map(i, old_Y) Is Nothing Then
                        Return 0
                    End If
                Next i
            ElseIf old_X - new_X > 1 Then
                For i = new_X + 1 To old_X - 1
                    If Not map(i, old_Y) Is Nothing Then
                        Return 0
                    End If
                Next i
            ElseIf new_Y - old_Y > 1 Then
                For i = old_Y + 1 To new_Y - 1
                    If Not map(old_X, i) Is Nothing Then
                        Return 0
                    End If
                Next i
            ElseIf old_Y - new_Y > 1 Then
                For i = new_Y + 1 To old_Y - 1
                    If Not map(old_X, i) Is Nothing Then
                        Return 0
                    End If
                Next i
            End If
            Return 1
        End If
        Return False
    End Function
End Class

Public Class HorseRule
    Implements IRule
    Public Function GetMoveable(map As Piece(,), oldLocation As Vector2, newLocation As Vector2) As Boolean Implements IRule.GetMoveable
        Dim old_X = oldLocation.X, old_Y = oldLocation.Y, new_X = newLocation.X, new_Y = newLocation.Y
        If Math.Abs((new_X - old_X) * (new_Y - old_Y)) = 2 Then
            If Math.Abs(new_X - old_X) = 2 And map(old_X + (new_X - old_X) / 2, old_Y) Is Nothing Then
                Return 1
            ElseIf Math.Abs(new_Y - old_Y) = 2 And map(old_X, old_Y + (new_Y - old_Y) / 2) Is Nothing Then
                Return 1
            End If
        End If
        Return False
    End Function
End Class


Public Class CannonRule
    Implements IRule
    Public Function GetMoveable(map As Piece(,), oldLocation As Vector2, newLocation As Vector2) As Boolean Implements IRule.GetMoveable
        Dim old_X = oldLocation.X, old_Y = oldLocation.Y, new_X = newLocation.X, new_Y = newLocation.Y
        If new_X - old_X = 0 Or new_Y - old_Y = 0 Then
            Dim temp As Integer = 0
            If new_X - old_X > 1 Then
                For i = old_X + 1 To new_X - 1
                    If Not map(i, old_Y) Is Nothing Then
                        temp = temp + 1
                    End If
                Next i
            ElseIf old_X - new_X > 1 Then
                For i = new_X + 1 To old_X - 1
                    If Not map(i, old_Y) Is Nothing Then
                        temp = temp + 1
                    End If
                Next i
            ElseIf new_Y - old_Y > 1 Then
                For i = old_Y + 1 To new_Y - 1
                    If Not map(old_X, i) Is Nothing Then
                        temp = temp + 1
                    End If
                Next i
            ElseIf old_Y - new_Y > 1 Then
                For i = new_Y + 1 To old_Y - 1
                    If Not map(old_X, i) Is Nothing Then
                        temp = temp + 1
                    End If
                Next i
            End If
            If temp = 0 And map(new_X, new_Y) Is Nothing Then
                Return 1
            ElseIf temp = 1 And (1 - 2 * map(old_X, old_Y).Camp) * (1 - 2 * map(new_X, new_Y).Camp) < 0 And Not map(new_X, new_Y) Is Nothing Then
                Return 1
            Else
                Return 0
            End If
        End If
        Return False
    End Function
End Class

Public Class ElephantRule
    Implements IRule
    Public Function GetMoveable(map As Piece(,), oldLocation As Vector2, newLocation As Vector2) As Boolean Implements IRule.GetMoveable
        Dim old_X = oldLocation.X, old_Y = oldLocation.Y, new_X = newLocation.X, new_Y = newLocation.Y
        Dim camp = 1 - 2 * map(old_X, old_Y).Camp
        If Math.Abs(new_X - old_X) = 2 And Math.Abs(new_Y - old_Y) = 2 And camp * (new_Y - 4.5) > 0 Then
            If map(CInt((old_X + new_X) / 2), CInt((old_Y + new_Y) / 2)) Is Nothing Then
                Return 1
            End If
        End If
        Return False
    End Function
End Class

Public Class AdviserRule
    Implements IRule
    Public Function GetMoveable(map As Piece(,), oldLocation As Vector2, newLocation As Vector2) As Boolean Implements IRule.GetMoveable
        Dim old_X = oldLocation.X, old_Y = oldLocation.Y, new_X = newLocation.X, new_Y = newLocation.Y
        Dim camp = 1 - 2 * map(old_X, old_Y).Camp
        If Math.Abs((new_X - old_X) * (new_Y - old_Y)) = 1 And new_X >= 3 And new_X <= 5 And -camp * new_Y <= -camp * (4.5 + camp * 2.5) Then
            Return 1
        End If
        Return False
    End Function
End Class

Public Class KingRule
    Implements IRule
    Public Function GetMoveable(map As Piece(,), oldLocation As Vector2, newLocation As Vector2) As Boolean Implements IRule.GetMoveable
        Dim old_X = oldLocation.X, old_Y = oldLocation.Y, new_X = newLocation.X, new_Y = newLocation.Y
        If new_X >= 3 And new_X <= 5 Then
            If new_X - old_X = 0 And Math.Abs(new_Y - old_Y) = 1 Then
                Return 1
            ElseIf Math.Abs(new_X - old_X) = 1 And new_Y - old_Y = 0 Then
                Return 1
            End If
        End If
        Return False
    End Function
End Class

Public Class PawnRule
    Implements IRule
    Public Function GetMoveable(map As Piece(,), oldLocation As Vector2, newLocation As Vector2) As Boolean Implements IRule.GetMoveable
        Dim old_X = oldLocation.X, old_Y = oldLocation.Y, new_X = newLocation.X, new_Y = newLocation.Y
        Dim camp = 1 - 2 * map(old_X, old_Y).Camp
        If camp * (old_Y - 4.5) > 0 Then '己方阵营内
            If new_X - old_X = 0 And Math.Abs(new_Y - old_Y) = 1 And camp * (new_Y - old_Y) < 0 Then
                Return 1
            End If
        ElseIf camp * (old_Y - 4.5) < 0 Then '对方阵营内
            If (old_Y - 4.5) * (new_Y - old_Y) >= 0 And Math.Abs(new_Y - old_Y) + Math.Abs(new_X - old_X) = 1 Then
                Return 1
            End If
        End If
        Return False
    End Function
End Class

