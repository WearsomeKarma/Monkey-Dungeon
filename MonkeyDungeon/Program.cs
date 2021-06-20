using System;
using System.IO;

namespace MonkeyDungeon_Core
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var dir = AppDomain.CurrentDomain.BaseDirectory;
            var game = new MonkeyDungeon_Game(
                dir,
                Path.Combine(dir, "Assets" + Path.DirectorySeparatorChar)
            );
            game.Run();
        }
    }
}