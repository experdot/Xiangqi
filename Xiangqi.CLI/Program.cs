using System;
using Xiangqi.PGN;

namespace Xiangqi.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var str = "前炮进三";
            var chinStr = MoveParser.chin2File(str);

            var simplePos = new SimplePos();
            var pos = MoveParser.file2Move(chinStr, simplePos);

        }
    }
}
