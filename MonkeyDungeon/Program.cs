using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon
{
    class Program
    {
        static void Main(string[] args)
        {
            string dir = "C:\\Users\\grauslysd\\source\\repos\\MonkeyDungeon\\MonkeyDungeon";
            MonkeyGame game = new MonkeyGame(
                dir,
                Path.Combine(dir, "Assets\\")
                );
            game.Run();
        }
    }
}
