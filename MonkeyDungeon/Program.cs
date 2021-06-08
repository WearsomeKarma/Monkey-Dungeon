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
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            MonkeyDungeon_Game game = new MonkeyDungeon_Game(
                dir,
                Path.Combine(dir, "Assets" + Path.DirectorySeparatorChar)
                );
            game.Run();
        }
    }
}
