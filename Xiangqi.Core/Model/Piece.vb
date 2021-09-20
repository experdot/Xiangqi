Imports System.Numerics

Public Enum PieceType
    King
    Adviser
    Elephant
    Horse
    Chariot
    Cannon
    Pawn
End Enum

Public Enum Camp
    Red
    Black
End Enum

Public Class Piece
    Public Property PieceType As PieceType
    Public Property Camp As Camp
    Public Property Location As Vector2

    Public Function Clone()
        Return New Piece With {
            .PieceType = PieceType,
            .Camp = Camp,
            .Location = Location
        }
    End Function
End Class
