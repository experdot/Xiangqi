Imports System.Numerics
Imports System.Linq

Public Class MoveParser
    Public Shared OrderPositionString As String() = New String() {"一", "二", "三", "四", "五", "前", "中", "后"}

    Public Shared AxisRedString As String() = New String() {"一", "二", "三", "四", "五", "六", "七", "八", "九"}
    Public Shared AxisBlackString As String() = New String() {"1", "2", "3", "4", "5", "6", "7", "8", "9"}

    Public Shared PieceRedString As String() = New String() {"帥", "仕", "相", "馬", "車", "砲", "兵"}
    Public Shared PieceBlackString As String() = New String() {"將", "士", "象", "馬", "車", "炮", "卒"}

    Public Shared ActionString As String() = New String() {"进", "退", "平"}
    Public Shared ActionStepRedString As String() = New String() {"一", "二", "三", "四", "五", "六", "七", "八", "九"}
    Public Shared ActionStepBlackString As String() = New String() {"1", "2", "3", "4", "5", "6", "7", "8", "9"}

    Public Shared AxisCount As Integer = 9

    Public Shared PieceDictionary As Dictionary(Of String, PieceType)
    Public Shared PieceDictionarySource As String() = New String() {"帅将帥將", "仕士", "相象", "马馬傌豖", "车車谙硨俥", "砲炮包", "兵卒"}

    Shared Sub New()
        PieceDictionary = New Dictionary(Of String, PieceType)
        For index = 0 To PieceDictionarySource.Length - 1
            For Each str In PieceDictionarySource(index)
                PieceDictionary(str) = CType(index, PieceType)
            Next
        Next
    End Sub

    Public Shared Function ToChineseWXF(move As Move) As String
        Dim piece As String

        If move.Camp = Camp.Red Then
            piece = PieceRedString(move.PieceType)
        Else
            piece = PieceBlackString(move.PieceType)
        End If

        Dim piecePrefix = ""
        Dim pieceSuffix = ""
        If move.VerticalCount > 3 Then
            piecePrefix = OrderPositionString(move.VerticalOrder)
        ElseIf move.VerticalCount > 2 Then
            piecePrefix = OrderPositionString(move.VerticalOrder + 5)
        ElseIf move.VerticalCount > 1 AndAlso Not (move.PieceType = PieceType.Adviser OrElse move.PieceType = PieceType.Elephant) Then
            piecePrefix = OrderPositionString(move.VerticalOrder * 2 + 5)
        Else
            If move.Camp = Camp.Red Then
                pieceSuffix = AxisRedString(AxisCount - 1 - move.StartLocation.X)
            Else
                pieceSuffix = AxisBlackString(move.StartLocation.X)
            End If
        End If

        Dim action As String
        Dim actionSuffix As String

        Dim offset = move.EndLocation.Y - move.StartLocation.Y

        If offset = 0 Then
            action = ActionString(2)
            If move.Camp = Camp.Red Then
                actionSuffix = AxisRedString(AxisCount - 1 - move.EndLocation.X)
            Else
                actionSuffix = AxisBlackString(move.EndLocation.X)
            End If
        Else
            Dim useStepOffset = Not (move.PieceType = PieceType.Elephant OrElse move.PieceType = PieceType.Horse OrElse move.PieceType = PieceType.Adviser)

            If move.Camp = Camp.Red Then
                If offset > 0 Then
                    action = ActionString(1)
                Else
                    action = ActionString(0)
                End If
                If useStepOffset Then
                    actionSuffix = ActionStepRedString(Math.Abs(offset) - 1)
                Else
                    actionSuffix = AxisRedString(AxisCount - 1 - move.EndLocation.X)
                End If
            Else
                If offset > 0 Then
                    action = ActionString(0)
                Else
                    action = ActionString(1)
                End If
                If useStepOffset Then
                    actionSuffix = ActionStepBlackString(Math.Abs(offset) - 1)
                Else
                    actionSuffix = AxisBlackString(move.EndLocation.X)
                End If
            End If
        End If

        Return Converter.ToSBC(piecePrefix + piece + pieceSuffix + action + actionSuffix)
    End Function

    Public Shared Function FromChineseWXF(board As Board, content As String) As Move
        Dim letter1 = content(0)
        Dim letter2 = content(1)
        Dim letter3 = content(2)
        Dim letter4 = content(3)
        Dim pieceType As PieceType
        Dim startLocation As Vector2
        Dim endLocation As Vector2

        Dim verticalCount As Integer
        Dim verticalOrder As Integer

        If OrderPositionString.Contains(letter1) Then

        Else
            pieceType = PieceDictionary(letter1)
            Dim hIndex = AxisCount - AxisRedString.ToList().IndexOf(letter2) - 1
            For Each piece In board.PieceMap
                If piece IsNot Nothing Then
                    If piece.Camp = board.Camp AndAlso piece.PieceType = pieceType AndAlso piece.Location.X = hIndex Then
                        startLocation = piece.Location
                        Exit For
                    End If
                End If
            Next
        End If

        If letter3 = "进" Then
            Dim vOffset = ActionStepRedString.ToList().IndexOf(letter4) + 1
            endLocation = startLocation + New Vector2(0, -vOffset)
        End If

        Return New Move() With {
            .StartLocation = startLocation,
            .EndLocation = endLocation,
            .PieceType = pieceType,
            .VerticalCount = verticalCount,
            .VerticalOrder = verticalOrder
        }
    End Function
End Class

Public Class Converter
    Public Shared Function ToSBC(input As String)
        Dim c = input.ToCharArray()
        For index = 0 To c.Length - 1
            If c(index) = ChrW(32) Then
                c(index) = ChrW(12288)
            Else
                If c(index) < ChrW(127) Then
                    c(index) = ChrW(AscW(c(index)) + 65248)
                End If
            End If
        Next
        Return New String(c)
    End Function
End Class
