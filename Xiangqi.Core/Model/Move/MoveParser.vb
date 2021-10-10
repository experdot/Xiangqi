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

    Public Shared StepDictionary As Dictionary(Of String, Integer)
    Public Shared StepDictionarySource As String() = New String() {"一1１", "二2２", "三3３", "四4４", "五5５", "六6６", "七7７", "八8８", "九9９"}

    Shared Sub New()
        PieceDictionary = New Dictionary(Of String, PieceType)
        For index = 0 To PieceDictionarySource.Length - 1
            For Each item In PieceDictionarySource(index)
                PieceDictionary(item) = CType(index, PieceType)
            Next
        Next

        StepDictionary = New Dictionary(Of String, Integer)
        For index = 0 To StepDictionarySource.Length - 1
            For Each item In StepDictionarySource(index)
                StepDictionary(item) = index + 1
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
            Dim useStepOffset = Not (move.PieceType = PieceType.Adviser OrElse move.PieceType = PieceType.Elephant OrElse move.PieceType = PieceType.Horse)

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
        Dim possibleStartLocations As New List(Of Vector2)
        Dim endLocation As Vector2

        Dim verticalCount As Integer
        Dim verticalOrder As Integer

        If OrderPositionString.Contains(letter1) Then
            pieceType = PieceDictionary(letter2)
            If pieceType = PieceType.Pawn Then
            Else
                Dim pieces = From piece As Piece In board.PieceMap Where piece?.Camp = board.Camp AndAlso piece?.PieceType = pieceType
                If letter1 = "前" Then
                    verticalCount = 2
                    verticalOrder = 0
                ElseIf letter1 = "中" Then
                    verticalCount = 3
                    verticalOrder = 1
                ElseIf letter1 = "后" Then
                    verticalCount = 2
                    verticalOrder = 1
                End If

                If board.Camp = Camp.Black Then
                    verticalOrder = verticalCount - 1 - verticalOrder
                End If

                possibleStartLocations.Add(pieces(verticalOrder).Location)
            End If
        Else
            pieceType = PieceDictionary(letter1)
            Dim hIndex As Integer
            If board.Camp = Camp.Red Then
                hIndex = AxisCount - StepDictionary(letter2)
            Else
                hIndex = StepDictionary(letter2) - 1
            End If
            For Each piece In board.PieceMap
                If piece IsNot Nothing Then
                    If piece.Camp = board.Camp AndAlso piece.PieceType = pieceType AndAlso piece.Location.X = hIndex Then
                        possibleStartLocations.Add(piece.Location)
                        If letter3 = "平" Then
                            Exit For
                        End If
                    End If
                End If
            Next
        End If

        Dim invert As Integer = 1
        If board.Camp = Camp.Black Then
            invert = -1
        End If

        If letter3 = "平" Then
            Dim x As Integer
            If board.Camp = Camp.Red Then
                x = AxisCount - StepDictionary(letter4)
            Else
                x = StepDictionary(letter4) - 1
            End If

            startLocation = possibleStartLocations.FirstOrDefault()
            endLocation = New Vector2(x, startLocation.Y)
        Else
            If letter3 = "进" Then
                invert = -invert
            ElseIf letter3 = "退" Then
                invert = invert
            End If

            If invert = 1 Then
                startLocation = possibleStartLocations.FirstOrDefault()
            Else
                startLocation = possibleStartLocations.LastOrDefault()
            End If

            If pieceType = PieceType.Adviser Then
                Dim x As Integer
                Dim xoffset As Integer

                If board.Camp = Camp.Red Then
                    x = AxisCount - StepDictionary(letter4)
                    xoffset = Math.Abs(startLocation.X - x)
                Else
                    x = StepDictionary(letter4) - 1
                    xoffset = Math.Abs(startLocation.X - x)
                End If

                endLocation = New Vector2(x, startLocation.Y + 1 * invert)

            ElseIf pieceType = PieceType.Elephant Then
                Dim x As Integer
                Dim xoffset As Integer

                If board.Camp = Camp.Red Then
                    x = AxisCount - StepDictionary(letter4)
                    xoffset = Math.Abs(startLocation.X - x)
                Else
                    x = StepDictionary(letter4) - 1
                    xoffset = Math.Abs(startLocation.X - x)
                End If

                endLocation = New Vector2(x, startLocation.Y + 2 * invert)
            ElseIf pieceType = PieceType.Horse Then
                Dim x As Integer
                Dim xoffset As Integer

                If board.Camp = Camp.Red Then
                    x = AxisCount - StepDictionary(letter4)
                    xoffset = Math.Abs(startLocation.X - x)
                Else
                    x = StepDictionary(letter4) - 1
                    xoffset = Math.Abs(startLocation.X - x)
                End If

                endLocation = New Vector2(x, startLocation.Y + (3 - xoffset) * invert)
            Else
                Dim vOffset As Integer = StepDictionary(letter4)
                endLocation = startLocation + New Vector2(0, vOffset * invert)
            End If
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
