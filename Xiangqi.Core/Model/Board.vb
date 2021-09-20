Public Class Board
    Public Property PieceMap As Piece(,)
    Public Property SelectedPiece As Piece
    Public Property Camp As Camp

    Public Sub ExchangeCamp()
        Camp = 1 - Camp
    End Sub

    Public Function Clone() As Board
        Dim width = PieceMap.GetUpperBound(0)
        Dim height = PieceMap.GetUpperBound(1)
        Dim clonedMap(,) As Piece
        ReDim clonedMap(width, height)
        For x = 0 To width
            For y = 0 To height
                clonedMap(x, y) = PieceMap(x, y)?.Clone()
            Next
        Next

        Return New Board With {
            .Camp = Camp,
            .SelectedPiece = SelectedPiece?.Clone(),
            .PieceMap = clonedMap
        }
    End Function
End Class
