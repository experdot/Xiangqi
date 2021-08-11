Module Module2
    'Private BaseValue() As integer = {0, 0, 0, 0, 0, 0, 10000, 0, 0, 0, 0, 0, 0, 0, -10000, 0}
    Private BaseValue() As Integer = {0, 1000, 500, 600, 300, 300, 10000, 20, 0, -1000, -500, -600, -300, -300, -10000, -20}
    Private PositionValue(15, 8, 9) As Integer
    Private Function CheckOver(ByVal map)
        Dim temp, temp2 As Integer
        For i = 3 To 5
            If map(i, 0) = 6 Or map(i, 1) = 6 Or map(i, 2) = 6 Then
                temp = temp + 1
            End If
            If map(i, 7) = 14 Or map(i, 8) = 14 Or map(i, 9) = 14 Then
                temp2 = temp2 + 1
            End If
        Next
        If temp = 1 And temp2 = 1 Then
            Return 0
        ElseIf temp = 1 And temp2 = 0 Then
            Return 1
        Else
            Return 2
        End If
    End Function

    Public Function Evaluate(ByVal map As Array)
        '車1000马500炮600相300士300将10000兵20
        Dim SituationValue As Integer = 0 '局面价值
        For i = 0 To 8
            For j = 0 To 9
                SituationValue = SituationValue + BaseValue(map(i, j)) + PositionValue(map(i, j), i, j)
            Next
        Next
        Return SituationValue
    End Function
    '  Dim a, b As Integer
    Public Function Search(ByVal map As Array, ByVal alpha As Integer, ByVal beta As Integer, ByVal turn As Integer, ByVal depth As Integer)
        Dim temp As Integer
        Dim Movings() As Array
        Dim tempSituation(8, 9) As Integer
        If depth = 0 Or CheckOver(map) > 0 Then
            '  a = a + 1
            'Form1.Text = a & "  " & b
            Return Evaluate(map)
        Else
            ' b = b + 1
            For i2 = 0 To 8
                For j2 = 0 To 9
                    tempSituation(i2, j2) = map(i2, j2)
                Next
            Next
            For i = 0 To 8
                For j = 0 To 9
                    If Not map(i, j) = 0 And (map(i, j) - 8) * turn < 0 Then
                        Movings = CreateSituation(map, i, j)
                        For p = 0 To Movings.Length - 1
                            tempSituation(Movings(p)(2), Movings(p)(3)) = tempSituation(Movings(p)(0), Movings(p)(1))
                            tempSituation(Movings(p)(0), Movings(p)(1)) = 0
                            '搜索
                            temp = -Search(tempSituation, -beta, -alpha, -turn, depth - 1)
                            tempSituation(Movings(p)(2), Movings(p)(3)) = map(Movings(p)(2), Movings(p)(3))
                            tempSituation(Movings(p)(0), Movings(p)(1)) = map(Movings(p)(0), Movings(p)(1))

                            If temp > beta Then
                                Return beta
                            End If
                            If temp > alpha Then
                                alpha = temp
                            End If
                        Next
                    End If
                Next
            Next
            Return alpha
        End If
    End Function
    Public Function Search2(ByVal map As Array, ByVal turn As Integer, ByVal depth As Integer)
        Dim best As Integer = -100000
        Dim temp As Integer
        Dim Movings() As Array
        Dim tempSituation(8, 9) As Integer
        If depth = 0 Or CheckOver(map) > 0 Then
            ' a = a + 1
            'Form1.Text = a & "  " & b
            Return Evaluate(map)
        Else
            '  b = b + 1
            For i2 = 0 To 8
                For j2 = 0 To 9
                    tempSituation(i2, j2) = map(i2, j2)
                Next
            Next
            For i = 0 To 8
                For j = 0 To 9
                    If Not map(i, j) = 0 And (map(i, j) - 8) * turn < 0 Then
                        Movings = CreateSituation(map, i, j)
                        For p = 0 To Movings.Length - 1
                            tempSituation(Movings(p)(2), Movings(p)(3)) = tempSituation(Movings(p)(0), Movings(p)(1))
                            tempSituation(Movings(p)(0), Movings(p)(1)) = 0
                            '搜索
                            temp = -Search2(tempSituation, -turn, depth - 1)
                            tempSituation(Movings(p)(2), Movings(p)(3)) = map(Movings(p)(2), Movings(p)(3))
                            tempSituation(Movings(p)(0), Movings(p)(1)) = map(Movings(p)(0), Movings(p)(1))
                            If temp > best Then
                                best = temp
                            End If
                        Next
                    End If
                Next
            Next
            Return best
        End If
    End Function
    Public Function AiMove(ByVal map As Array, ByVal Turn As Integer, ByVal Depth As Integer)
        '   a = 0 : b = 0
        Dim Best As Integer, temp As Integer
        Dim Movings() As Array
        Dim BestMove() As Integer = {0, 0, 0, 0}
        Dim tempSituation(8, 9) As Integer
        Best = -100000
        REM value初始化
        SetPositionValue()
        REM 临时备份
        For i2 = 0 To 8
            For j2 = 0 To 9
                tempSituation(i2, j2) = map(i2, j2)
            Next
        Next
        For i = 0 To 8
            For j = 0 To 9
                If Not map(i, j) = 0 And (map(i, j) - 8) * Turn < 0 Then
                    Movings = CreateSituation(map, i, j)
                    For p = 0 To Movings.Length - 1
                        tempSituation(Movings(p)(2), Movings(p)(3)) = tempSituation(Movings(p)(0), Movings(p)(1))
                        tempSituation(Movings(p)(0), Movings(p)(1)) = 0
                        '搜索
                        temp = -Search(tempSituation, -100000, 100000, -Turn, Depth)
                        ' temp = -Search2(tempSituation, -Turn, Depth)
                        tempSituation(Movings(p)(2), Movings(p)(3)) = map(Movings(p)(2), Movings(p)(3))
                        tempSituation(Movings(p)(0), Movings(p)(1)) = map(Movings(p)(0), Movings(p)(1))
                        If temp > Best Then
                            Best = temp
                            BestMove = Movings(p)
                        End If
                    Next
                End If
            Next
        Next
        Return BestMove

    End Function
    Public Function CreateSituation(ByVal map As Array, ByVal X As Integer, ByVal Y As Integer) '寻找相关合法移位,并返回新局面
        Dim Movings(100) As Array
        Dim J As Integer = 0
        '車的移位合法判断
        If map(X, Y) = 1 Or map(X, Y) = 9 Then
            If X <= 7 Then
                For i = X + 1 To 8 '向右寻找
                    If map(i, Y) = 0 Then
                        Movings(J) = {X, Y, i, Y}
                        J = J + 1
                    ElseIf (map(i, Y) - 8) * (map(X, Y) - 8) < 0 And Not map(i, Y) = 0 Then
                        Movings(J) = {X, Y, i, Y}
                        J = J + 1
                        Exit For
                    Else
                        Exit For
                    End If
                Next
            End If
            If X >= 1 Then
                For i = 0 To X - 1 '向左寻找
                    If map(X - i - 1, Y) = 0 Then
                        Movings(J) = {X, Y, X - i - 1, Y}
                        J = J + 1
                    ElseIf (map(X - 1 - i, Y) - 8) * (map(X, Y) - 8) < 0 And Not map(X - i - 1, Y) = 0 Then
                        Movings(J) = {X, Y, X - i - 1, Y}
                        J = J + 1
                        Exit For
                    Else
                        Exit For
                    End If
                Next
            End If
            If Y <= 8 Then
                For i = Y + 1 To 9 '向下寻找
                    If map(X, i) = 0 Then
                        Movings(J) = {X, Y, X, i}
                        J = J + 1
                    ElseIf (map(X, i) - 8) * (map(X, Y) - 8) < 0 And Not map(X, i) = 0 Then
                        Movings(J) = {X, Y, X, i}
                        J = J + 1
                        Exit For
                    Else
                        Exit For
                    End If

                Next
            End If
            If Y >= 1 Then
                For i = 0 To Y - 1 '向上寻找
                    If map(X, Y - i - 1) = 0 Then
                        Movings(J) = {X, Y, X, Y - i - 1}
                        J = J + 1
                    ElseIf (map(X, Y - i - 1) - 8) * (map(X, Y) - 8) < 0 And Not map(X, Y - i - 1) = 0 Then
                        Movings(J) = {X, Y, X, Y - i - 1}
                        J = J + 1
                        Exit For
                    Else
                        Exit For
                    End If
                Next
            End If
            ReDim Preserve Movings(J - 1)
            Return Movings
        End If

        '馬的移位合法判断
        If map(X, Y) = 2 Or map(X, Y) = 10 Then
            If X >= 2 And Y <= 8 Then '向左下寻找            
                If (map(X - 2, Y + 1) = 0 Or (map(X - 2, Y + 1) - 8) * (map(X, Y) - 8) < 0) And map(X - 1, Y) = 0 Then
                    Movings(J) = {X, Y, X - 2, Y + 1}
                    J = J + 1
                End If
            End If
            If X >= 2 And Y >= 1 Then '向左上寻找
                If (map(X - 2, Y - 1) = 0 Or (map(X - 2, Y - 1) - 8) * (map(X, Y) - 8) < 0) And map(X - 1, Y) = 0 Then
                    Movings(J) = {X, Y, X - 2, Y - 1}

                    J = J + 1
                End If
            End If
            If Y <= 7 And X >= 1 Then '向下左寻找
                If (map(X - 1, Y + 2) = 0 Or (map(X - 1, Y + 2) - 8) * (map(X, Y) - 8) < 0) And map(X, Y + 1) = 0 Then
                    Movings(J) = {X, Y, X - 1, Y + 2}
                    J = J + 1
                End If
            End If
            If Y <= 7 And X <= 7 Then '向下右寻找
                If (map(X + 1, Y + 2) = 0 Or (map(X + 1, Y + 2) - 8) * (map(X, Y) - 8) < 0) And map(X, Y + 1) = 0 Then
                    Movings(J) = {X, Y, X + 1, Y + 2}
                    J = J + 1
                End If
            End If
            If X <= 6 And Y <= 8 Then '向右下寻找
                If (map(X + 2, Y + 1) = 0 Or (map(X + 2, Y + 1) - 8) * (map(X, Y) - 8) < 0) And map(X + 1, Y) = 0 Then
                    Movings(J) = {X, Y, X + 2, Y + 1}
                    J = J + 1
                End If
            End If
            If X <= 6 And Y >= 1 Then '向右上寻找
                If (map(X + 2, Y - 1) = 0 Or (map(X + 2, Y - 1) - 8) * (map(X, Y) - 8) < 0) And map(X + 1, Y) = 0 Then
                    Movings(J) = {X, Y, X + 2, Y - 1}
                    J = J + 1
                End If
            End If
            If Y >= 2 And X >= 1 Then '向上左寻找
                If (map(X - 1, Y - 2) = 0 Or (map(X - 1, Y - 2) - 8) * (map(X, Y) - 8) < 0) And map(X, Y - 1) = 0 Then
                    Movings(J) = {X, Y, X - 1, Y - 2}
                    J = J + 1
                End If
            End If
            If Y >= 2 And X <= 7 Then '向上右寻找
                If (map(X + 1, Y - 2) = 0 Or (map(X + 1, Y - 2) - 8) * (map(X, Y) - 8) < 0) And map(X, Y - 1) = 0 Then
                    Movings(J) = {X, Y, X + 1, Y - 2}
                    J = J + 1
                End If
            End If
            ReDim Preserve Movings(J - 1)
            Return Movings
        End If


        '砲的移位合法判断
        If map(X, Y) = 3 Or map(X, Y) = 11 Then

            If X <= 7 Then
                For i = X + 1 To 8 '向右寻找
                    If map(i, Y) = 0 Then
                        Movings(J) = {X, Y, i, Y}
                        J = J + 1

                    Else
                        If i < 8 Then
                            For p = i + 1 To 8
                                If Not map(p, Y) = 0 Then
                                    If (map(p, Y) - 8) * (map(X, Y) - 8) < 0 Then
                                        Movings(J) = {X, Y, p, Y}
                                        J = J + 1
                                    End If
                                    Exit For
                                End If
                            Next
                            Exit For
                        End If
                    End If
                Next
            End If

            If X >= 1 Then
                For i = 0 To X - 1 '向左寻找
                    If map(X - i - 1, Y) = 0 Then
                        Movings(J) = {X, Y, X - i - 1, Y}
                        J = J + 1
                    Else
                        If i < X - 1 Then
                            For p = i + 1 To X - 1
                                If Not map(X - p - 1, Y) = 0 Then
                                    If (map(X - p - 1, Y) - 8) * (map(X, Y) - 8) < 0 Then
                                        Movings(J) = {X, Y, X - p - 1, Y}
                                        J = J + 1
                                    End If
                                    Exit For
                                End If
                            Next
                            Exit For
                        End If
                    End If
                Next
            End If

            If Y <= 8 Then
                For i = Y + 1 To 9 '向下寻找
                    If map(X, i) = 0 Then
                        Movings(J) = {X, Y, X, i}
                        J = J + 1
                    Else
                        If i < 9 Then
                            For p = i + 1 To 9
                                If Not map(X, p) = 0 Then
                                    If (map(X, p) - 8) * (map(X, Y) - 8) < 0 Then
                                        Movings(J) = {X, Y, X, p}
                                        J = J + 1
                                    End If
                                    Exit For
                                End If
                            Next
                            Exit For
                        End If
                    End If
                Next
            End If
            If Y >= 1 Then
                For i = 0 To Y - 1 '向上寻找
                    If map(X, Y - i - 1) = 0 Then
                        Movings(J) = {X, Y, X, Y - i - 1}
                        J = J + 1
                    Else
                        If i < Y - 1 Then
                            For p = i + 1 To Y - 1
                                If Not map(X, Y - p - 1) = 0 Then
                                    If (map(X, Y - p - 1) - 8) * (map(X, Y) - 8) < 0 Then
                                        Movings(J) = {X, Y, X, Y - p - 1}
                                        J = J + 1
                                    End If
                                    Exit For
                                End If
                            Next
                            Exit For
                        End If
                    End If
                Next
            End If
            ReDim Preserve Movings(J - 1)
            Return Movings
        End If
        '象的移位合法判断
        If map(X, Y) = 4 Or map(X, Y) = 12 Then
            '向左上搜索
            If X >= 2 And ((Y >= 2 And map(X, Y) = 4) Or (Y >= 7 And map(X, Y) = 12)) Then
                If map(X - 1, Y - 1) = 0 And (map(X - 2, Y - 2) = 0 Or (map(X - 2, Y - 2) - 8) * (map(X, Y) - 8) < 0) Then
                    Movings(J) = {X, Y, X - 2, Y - 2}
                    J = J + 1
                End If
            End If
            '向左下搜索
            If X >= 2 And ((Y <= 2 And map(X, Y) = 4) Or (Y <= 7 And map(X, Y) = 12)) Then
                If map(X - 1, Y + 1) = 0 And (map(X - 2, Y + 2) = 0 Or (map(X - 2, Y + 2) - 8) * (map(X, Y) - 8) < 0) Then
                    Movings(J) = {X, Y, X - 2, Y + 2}
                    J = J + 1
                End If
            End If
            '  向右上寻找
            If X <= 6 And ((Y >= 2 And map(X, Y) = 4) Or (Y >= 7 And map(X, Y) = 12)) Then
                If map(X + 1, Y - 1) = 0 And (map(X + 2, Y - 2) = 0 Or (map(X + 2, Y - 2) - 8) * (map(X, Y) - 8) < 0) Then
                    Movings(J) = {X, Y, X + 2, Y - 2}
                    J = J + 1
                End If
            End If
            '向右下搜索
            If X <= 6 And ((Y <= 2 And map(X, Y) = 4) Or (Y <= 7 And map(X, Y) = 12)) Then

                If map(X + 1, Y + 1) = 0 And (map(X + 2, Y + 2) = 0 Or (map(X + 2, Y + 2) - 8) * (map(X, Y) - 8) < 0) Then
                    Movings(J) = {X, Y, X + 2, Y + 2}
                    J = J + 1
                End If
            End If
            ReDim Preserve Movings(J - 1)
            Return Movings
        End If

        '仕的移位合法判断
        If map(X, Y) = 5 Or map(X, Y) = 13 Then
            '向左上搜索
            If X >= 4 And ((Y >= 1 And map(X, Y) = 5) Or (Y >= 8 And map(X, Y) = 13)) Then
                If map(X - 1, Y - 1) = 0 Or (map(X - 1, Y - 1) - 8) * (map(X, Y) - 8) < 0 Then
                    Movings(J) = {X, Y, X - 1, Y - 1}
                    J = J + 1
                End If
            End If
            '向左下搜索
            If X >= 4 And ((Y <= 1 And map(X, Y) = 5) Or (Y <= 8 And map(X, Y) = 13)) Then
                If map(X - 1, Y + 1) = 0 Or (map(X - 1, Y + 1) - 8) * (map(X, Y) - 8) < 0 Then
                    Movings(J) = {X, Y, X - 1, Y + 1}
                    J = J + 1
                End If
            End If
            '向右上搜索
            If X <= 4 And ((Y >= 1 And map(X, Y) = 5) Or (Y >= 8 And map(X, Y) = 13)) Then
                If map(X + 1, Y - 1) = 0 Or (map(X + 1, Y - 1) - 8) * (map(X, Y) - 8) < 0 Then
                    Movings(J) = {X, Y, X + 1, Y - 1}
                    J = J + 1
                End If
            End If
            '向右下搜索
            If X <= 4 And ((Y <= 1 And map(X, Y) = 5) Or (Y <= 8 And map(X, Y) = 13)) Then
                If map(X + 1, Y + 1) = 0 Or (map(X + 1, Y + 1) - 8) * (map(X, Y) - 8) < 0 Then
                    Movings(J) = {X, Y, X + 1, Y + 1}
                    J = J + 1
                End If
            End If
            ReDim Preserve Movings(J - 1)
            Return Movings
        End If
        '将的移位合法判断
        If map(X, Y) = 6 Or map(X, Y) = 14 Then
            '向上搜索
            If (Y >= 1 And map(X, Y) = 6) Or (Y >= 8 And map(X, Y) = 14) Then
                If map(X, Y - 1) = 0 Or (map(X, Y - 1) - 8) * (map(X, Y) - 8) < 0 Then
                    Movings(J) = {X, Y, X, Y - 1}
                    J = J + 1
                End If
            End If
            '向下搜索
            If (Y <= 1 And map(X, Y) = 6) Or (Y <= 8 And map(X, Y) = 14) Then
                If map(X, Y + 1) = 0 Or (map(X, Y + 1) - 8) * (map(X, Y) - 8) < 0 Then
                    Movings(J) = {X, Y, X, Y + 1}
                    J = J + 1
                End If
            End If
            '向左搜索
            If X >= 4 Then
                If map(X - 1, Y) = 0 Or (map(X - 1, Y) - 8) * (map(X, Y) - 8) < 0 Then
                    Movings(J) = {X, Y, X - 1, Y}
                    J = J + 1
                End If
            End If
            '向右搜索
            If X <= 4 Then
                If map(X + 1, Y) = 0 Or (map(X + 1, Y) - 8) * (map(X, Y) - 8) < 0 Then
                    Movings(J) = {X, Y, X + 1, Y}
                    J = J + 1
                End If
            End If
            ReDim Preserve Movings(J - 1)
            Return Movings
        End If
        '卒的移位合法判断
        If map(X, Y) = 7 Or map(X, Y) = 15 Then
            '向下搜索
            If map(X, Y) = 7 And Y <= 8 Then
                If map(X, Y + 1) = 0 Or (map(X, Y + 1) - 8) * (map(X, Y) - 8) < 0 Then
                    Movings(J) = {X, Y, X, Y + 1}
                    J = J + 1
                End If
            End If
            '向上搜索
            If map(X, Y) = 15 And Y >= 1 Then
                If map(X, Y - 1) = 0 Or (map(X, Y - 1) - 8) * (map(X, Y) - 8) < 0 Then
                    Movings(J) = {X, Y, X, Y - 1}
                    J = J + 1
                End If
            End If
            '向左搜索
            If (map(X, Y) - 8) * (Y - 4.5) < 0 And X >= 1 Then
                If map(X - 1, Y) = 0 Or (map(X - 1, Y) - 8) * (map(X, Y) - 8) < 0 Then
                    Movings(J) = {X, Y, X - 1, Y}
                    J = J + 1
                End If
            End If
            '向右搜索
            If (map(X, Y) - 8) * (Y - 4.5) < 0 And X <= 7 Then
                If map(X + 1, Y) = 0 Or (map(X + 1, Y) - 8) * (map(X, Y) - 8) < 0 Then
                    Movings(J) = {X, Y, X + 1, Y}
                    J = J + 1
                End If
            End If
            ReDim Preserve Movings(J - 1)
            Return Movings
        End If
        ReDim Preserve Movings(J - 1)
        Return Movings
    End Function
    '棋子的价值，后面255是指在不同位置的价值" 
    Public Sub SetPositionValue()
        REM  子力位置价值表
        '' 帅(将)
        Dim RedJiangPostion() As Byte = { _
     0, 0, 0, 0, 0, 0, 0, 0, 0, _
     0, 0, 0, 0, 0, 0, 0, 0, 0, _
     0, 0, 0, 0, 0, 0, 0, 0, 0, _
     0, 0, 0, 0, 0, 0, 0, 0, 0, _
     0, 0, 0, 0, 0, 0, 0, 0, 0, _
     0, 0, 0, 0, 0, 0, 0, 0, 0, _
     0, 0, 0, 0, 0, 0, 0, 0, 0, _
     0, 0, 0, 1, 1, 1, 0, 0, 0, _
     0, 0, 0, 2, 2, 2, 0, 0, 0, _
     0, 0, 0, 11, 15, 11, 0, 0, 0}

        '' 仕(士)
        Dim RedShiPosition() As Byte = { _
    0, 0, 0, 0, 0, 0, 0, 0, 0, _
    0, 0, 0, 0, 0, 0, 0, 0, 0, _
    0, 0, 0, 0, 0, 0, 0, 0, 0, _
    0, 0, 0, 0, 0, 0, 0, 0, 0, _
    0, 0, 0, 0, 0, 0, 0, 0, 0, _
    0, 0, 0, 0, 0, 0, 0, 0, 0, _
    0, 0, 0, 0, 0, 0, 0, 0, 0, _
    0, 0, 0, 20, 0, 20, 0, 0, 0, _
    0, 0, 0, 0, 23, 0, 0, 0, 0, _
    0, 0, 0, 20, 0, 20, 0, 0, 0 _
    }

        '' 相(象)
        Dim RedXiangPosition() As Byte = { _
    0, 0, 0, 0, 0, 0, 0, 0, 0, _
    0, 0, 0, 0, 0, 0, 0, 0, 0, _
    0, 0, 0, 0, 0, 0, 0, 0, 0, _
    0, 0, 0, 0, 0, 0, 0, 0, 0, _
    0, 0, 0, 0, 0, 0, 0, 0, 0, _
    0, 0, 20, 0, 0, 0, 20, 0, 0, _
    0, 0, 0, 0, 0, 0, 0, 0, 0, _
    18, 0, 0, 0, 23, 0, 0, 0, 18, _
    0, 0, 0, 0, 0, 0, 0, 0, 0, _
    0, 0, 20, 0, 0, 0, 20, 0, 0 _
    }

        '' 马
        Dim RedMaPosition() = { _
     90, 90, 90, 96, 90, 96, 90, 90, 90, _
     90, 96, 103, 97, 94, 97, 103, 96, 90, _
     92, 98, 99, 103, 99, 103, 99, 98, 92, _
     93, 108, 100, 107, 100, 107, 100, 108, 93, _
     90, 100, 99, 103, 104, 103, 99, 100, 90, _
     90, 98, 101, 102, 103, 102, 101, 98, 90, _
     92, 94, 98, 95, 98, 95, 98, 94, 92, _
     93, 92, 94, 95, 92, 95, 94, 92, 93, _
     85, 90, 92, 93, 78, 93, 92, 90, 85, _
     88, 85, 90, 88, 90, 88, 90, 85, 88 _
    }

        '' 车
        Dim RedChePosition() = { _
     206, 208, 207, 213, 214, 213, 207, 208, 206, _
     206, 212, 209, 216, 233, 216, 209, 212, 206, _
     206, 208, 207, 214, 216, 214, 207, 208, 206, _
     206, 213, 213, 216, 216, 216, 213, 213, 206, _
     208, 211, 211, 214, 215, 214, 211, 211, 208, _
     208, 212, 212, 214, 215, 214, 212, 212, 208, _
     204, 209, 204, 212, 214, 212, 204, 209, 204, _
     198, 208, 204, 212, 212, 212, 204, 208, 198, _
     200, 208, 206, 212, 200, 212, 206, 208, 200, _
     194, 206, 204, 212, 200, 212, 204, 206, 194 _
    }

        '' 炮
        Dim RedPaoPosition() = { _
    100, 100, 96, 91, 90, 91, 96, 100, 100, _
     98, 98, 96, 92, 89, 92, 96, 98, 98, _
     97, 97, 96, 91, 92, 91, 96, 97, 97, _
     96, 99, 99, 98, 100, 98, 99, 99, 96, _
     96, 96, 96, 96, 100, 96, 96, 96, 96, _
     95, 96, 99, 96, 100, 96, 99, 96, 95, _
     96, 96, 96, 96, 96, 96, 96, 96, 96, _
     97, 96, 100, 99, 101, 99, 100, 96, 97, _
     96, 97, 98, 98, 98, 98, 98, 97, 96, _
     96, 96, 97, 99, 99, 99, 97, 96, 96 _
    }

        '' 兵(卒)  
        Dim RedBingPosition() = { _
      9, 9, 9, 11, 13, 11, 9, 9, 9, _
     19, 24, 34, 42, 44, 42, 34, 24, 19, _
     19, 24, 32, 37, 37, 37, 32, 24, 19, _
     19, 23, 27, 29, 30, 29, 27, 23, 19, _
     14, 18, 20, 27, 29, 27, 20, 18, 14, _
     7, 0, 13, 0, 16, 0, 13, 0, 7, _
     7, 0, 7, 0, 15, 0, 7, 0, 7, _
     0, 0, 0, 0, 0, 0, 0, 0, 0, _
     0, 0, 0, 0, 0, 0, 0, 0, 0, _
     0, 0, 0, 0, 0, 0, 0, 0, 0 _
    }
        REM 设定好棋子

        '黑方:車1 馬2 砲3 象4 仕5 將6 卒7
        '红方:車9 馬10炮11相12士13帥14兵15
        For i = 0 To 8
            For j = 0 To 9
                PositionValue(15, i, j) = -RedJiangPostion(j * 9 + i)        ''红帅
                PositionValue(7, i, j) = RedJiangPostion(89 - j * 9 - i)        ''黑将

                PositionValue(9, i, j) = -RedChePosition(j * 9 + i)         ''红车
                PositionValue(1, i, j) = RedChePosition(89 - j * 9 - i)         ''黑车

                PositionValue(10, i, j) = -RedMaPosition(j * 9 + i)          ''红马
                PositionValue(2, i, j) = RedMaPosition(89 - j * 9 - i)          ''黑马

                PositionValue(11, i, j) = -RedPaoPosition(j * 9 + i)         ''红炮
                PositionValue(3, i, j) = RedPaoPosition(89 - j * 9 - i)         ''黑炮

                PositionValue(13, i, j) = -RedShiPosition(j * 9 + i)         ''红士
                PositionValue(5, i, j) = RedShiPosition(89 - j * 9 - i)         ''黑士

                PositionValue(12, i, j) = -RedXiangPosition(j * 9 + i)       ''红相
                PositionValue(4, i, j) = RedXiangPosition(89 - j * 9 - i)       ''黑象

                PositionValue(14, i, j) = -RedBingPosition(j * 9 + i)        ''红兵
                PositionValue(6, i, j) = RedBingPosition(89 - j * 9 - i)        ''黑兵
            Next
        Next
    End Sub
End Module
