using isometricgame.GameEngine;
using isometricgame.GameEngine.Components.Rendering;
using isometricgame.GameEngine.Rendering.Animation;
using isometricgame.GameEngine.Scenes;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_UI.Prefabs.Entities
{
    public class Particle : GameObject
    {
        public Particle(SceneLayer sceneLayer, Vector3 position) 
            : base(sceneLayer, position)
        {
            AnimationSchematic schem = new AnimationSchematic(1, 0.05);
            schem.DefineNode(0, new int[] { 0, 1, 2, 3, 4, 5, 6, 7 });

            SpriteComponent = new AnimationComponent(schem);
        }

        public void Set_Particle(string particleName)
        {
            SpriteComponent.SetSprite(particleName);
        }

        public void Toggle_Sprite(bool state)
        {
            SpriteComponent.Enabled = state;
        }
    }
}
