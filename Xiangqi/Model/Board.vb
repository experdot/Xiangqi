Public Class Board
    Public Property PieceMap As Piece(,)
    Public Property SelectedPiece As Piece
    Public Property Camp As Camp

    Public Sub ExchangeCamp()
        Camp = IIf(Camp = Camp.Black, Camp.Red, Camp.Black)
    End Sub
End Class
