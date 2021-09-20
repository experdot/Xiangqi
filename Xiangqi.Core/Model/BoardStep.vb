Imports System.Numerics

Public Class BoardStep
    Public Property Order As Integer
    Public Property Move As Move
    Public Property Board As Board

    Public Shared Function GenerateBoardStep(order As Integer, board As Board, oldLocation As Vector2, newLocation As Vector2) As BoardStep
        Dim move As Move
        If order > 0 Then
            move = Move.GenerateMove(board, oldLocation, newLocation)
        Else
            move = Nothing
        End If

        Return New BoardStep() With {
            .Order = order,
            .Board = board.Clone(),
            .Move = move
        }
    End Function

    Public Overrides Function ToString() As String
        If Order = 0 Then
            Return "===开始==="
        Else
            Dim doubleOrder = Order / 2.0F
            If Math.Floor(doubleOrder) = doubleOrder Then
                Return doubleOrder.ToString() + "." + Move.ToChineseWXF()
            Else
                Dim paddingLeft = Math.Floor(doubleOrder / 10)
                Dim padding = "  "
                For index = 1 To paddingLeft
                    padding += " "
                Next
                Return padding + Move.ToChineseWXF()
            End If

        End If
    End Function
End Class
