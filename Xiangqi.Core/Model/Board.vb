Public Class Board
    Public Property PieceMap As Piece(,)
    Public Property SelectedPiece As Piece
    Public Property Camp As Camp

    Public Sub ExchangeCamp()
        Camp = 1 - Camp
    End Sub
End Class
