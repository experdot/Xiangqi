﻿Imports System.Numerics

Public Enum PieceType
    Chariot
    Horse
    Cannon
    Elephant
    Adviser
    King
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
End Class