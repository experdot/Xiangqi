Imports System.Numerics

Public Class Move
    Public StartLocation As Vector2
    Public EndLocation As Vector2
    Public VerticalOrder As Integer
    Public VerticalCount As Integer
    Public PieceType As PieceType
    Public Camp As Camp

    Public OrderPosition2String As String() = New String() {"前", "后"}
    Public OrderPosition3String As String() = New String() {"前", "中", "后"}
    Public OrderPosition5String As String() = New String() {"一", "二", "三", "四", "五"}

    Public AxisRedString As String() = New String() {"一", "二", "三", "四", "五", "六", "七", "八", "九"}
    Public AxisBlackString As String() = New String() {"1", "2", "3", "4", "5", "6", "7", "8", "9"}

    Public PieceRedString As String() = New String() {"帥", "仕", "相", "馬", "車", "砲", "兵"}
    Public PieceBlackString As String() = New String() {"將", "士", "象", "馬", "車", "炮", "卒"}

    Public ActionString As String() = New String() {"进", "退", "平"}
    Public ActionStepRedString As String() = New String() {"一", "二", "三", "四", "五", "六", "七", "八", "九"}
    Public ActionStepBlackString As String() = New String() {"1", "2", "3", "4", "5", "6", "7", "8", "9"}

    Public AxisCount As Integer = 9

    Public Function ToChineseWXF() As String
        Dim piece As String

        If Camp = Camp.Red Then
            piece = PieceRedString(PieceType)
        Else
            piece = PieceBlackString(PieceType)
        End If

        Dim piecePrefix = ""
        Dim pieceSuffix = ""
        If VerticalCount > 3 Then
            piecePrefix = OrderPosition5String(VerticalOrder)
        ElseIf VerticalCount > 2 Then
            piecePrefix = OrderPosition3String(VerticalOrder)
        ElseIf VerticalCount > 1 AndAlso Not (PieceType = PieceType.Adviser OrElse PieceType = PieceType.Elephant) Then
            piecePrefix = OrderPosition2String(VerticalOrder)
        Else
            If Camp = Camp.Red Then
                pieceSuffix = AxisRedString(AxisCount - 1 - StartLocation.X)
            Else
                pieceSuffix = AxisBlackString(StartLocation.X)
            End If
        End If

        Dim action As String
        Dim actionSuffix As String

        Dim offset = EndLocation.Y - StartLocation.Y

        If offset = 0 Then
            action = ActionString(2)
            If Camp = Camp.Red Then
                actionSuffix = AxisRedString(AxisCount - 1 - EndLocation.X)
            Else
                actionSuffix = AxisBlackString(EndLocation.X)
            End If
        Else
            Dim useStepOffset = Not (PieceType = PieceType.Elephant OrElse PieceType = PieceType.Horse OrElse PieceType = PieceType.Adviser)

            If Camp = Camp.Red Then
                If offset > 0 Then
                    action = ActionString(1)
                Else
                    action = ActionString(0)
                End If
                If useStepOffset Then
                    actionSuffix = ActionStepRedString(Math.Abs(offset) - 1)
                Else
                    actionSuffix = AxisRedString(AxisCount - 1 - EndLocation.X)
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
                    actionSuffix = AxisBlackString(EndLocation.X)
                End If
            End If
        End If

        Return Converter.ToSBC(piecePrefix + piece + pieceSuffix + action + actionSuffix)
    End Function

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
