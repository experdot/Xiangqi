Module Module1
    Public beginNewgame As Boolean '是否开始了新的游戏
    Public gameIsOver As Boolean '游戏是否已经结束
    Public redPlayer As Integer '红方玩家 0为棋手，1为电脑
    Public blackPlayer As Integer '黑方玩家 0为棋手，1为电脑
    Public aiPlayerDepth As Integer = 2 '电脑玩家搜索深度 默认为2
    Public aiHelperDepth As Integer = 2 '电脑提示搜索深度 默认为2
    Public lackMode As Integer '让子模式

    Public walkline(9, 10, 0) As Integer
    Public walkstep As Integer = -1 '走棋步数
    Public stepUP As Integer = 0 '走棋上界
End Module
