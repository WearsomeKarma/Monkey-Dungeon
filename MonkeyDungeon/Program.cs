using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core
{
    class Program
    {
        static void Main(string[] args)
        {
            string dir = "C:\\Users\\grauslysd\\source\\repos\\MonkeyDungeon\\MonkeyDungeon";
            MonkeyDungeon_Game game = new MonkeyDungeon_Game(
                dir,
                Path.Combine(dir, "Assets\\")
                );
            game.Run();
        }
    }
}
