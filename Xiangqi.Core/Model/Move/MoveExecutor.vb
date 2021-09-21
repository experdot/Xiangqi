Public Class MoveExecutor
    Public Shared Sub Execute(game As XiangqiGame, move As Move)
        game.Board.SelectedPiece = Nothing
        game.Move(move.StartLocation)
        game.Move(move.EndLocation)
    End Sub
End Class
