using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isometricgame.GameEngine.Scenes;
using MonkeyDungeon.Components.Implemented.Enemies.Goblins;
using OpenTK;

namespace MonkeyDungeon.Prefabs.Entities.Enemies
{
    public class Goblin : CreatureGameObject
    {
        public Goblin(SceneLayer sceneLayer, Vector3 position, int level=1) 
            : base(sceneLayer, position, new EC_Goblin(level), 2, 8)
        {

        }
    }
}
