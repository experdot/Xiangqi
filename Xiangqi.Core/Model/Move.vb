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

        Return piecePrefix + piece + pieceSuffix + action + actionSuffix
    End Function

End Class
