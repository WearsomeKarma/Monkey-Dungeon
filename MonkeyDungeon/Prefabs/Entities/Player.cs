using isometricgame.GameEngine;
using isometricgame.GameEngine.Components.Rendering;
using isometricgame.GameEngine.Rendering;
using isometricgame.GameEngine.Rendering.Animation;
using isometricgame.GameEngine.Scenes;
using isometricgame.GameEngine.Systems.Rendering;
using MonkeyDungeon.Components;
using MonkeyDungeon.Components.Implemented.PlayerClasses;
using MonkeyDungeon.GameFeatures;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.Prefabs.Entities
{
    public class Player : CreatureGameObject
    {
        public static readonly string[] CLASSES = new string[] 
        { "Warrior", "Wizard", "Archer", "Cleric", "Knight", "Monk" };
        
        private int classId;
        public int ClassId {
            get => classId;
            set
            {
                classId = value;
                UniqueIdentifier.VAO_Index = classId;
            }
        }

        public Player(SceneLayer sceneLayer, Vector3 position, EntityComponent ec) 
            : base(sceneLayer, position, ec, 8, 8)
        {
        }
    }
}
