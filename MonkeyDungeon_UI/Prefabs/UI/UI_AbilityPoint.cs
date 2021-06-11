using isometricgame.GameEngine;
using isometricgame.GameEngine.Components.Rendering;
using isometricgame.GameEngine.Events.Arguments;
using isometricgame.GameEngine.Rendering.Animation;
using isometricgame.GameEngine.Scenes;
using isometricgame.GameEngine.Tools;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_UI.Prefabs.UI
{
    public class UI_AbilityPoint : GameObject
    {
        private AnimationComponent AnimationComponent { get; set; }

        public UI_AbilityPoint(SceneLayer sceneLayer, Vector3 position) 
            : base(sceneLayer, position)
        {
            AnimationSchematic schem = new AnimationSchematic(4, 0.05);

            AnimationNode[] nodes = new AnimationNode[4];

            for(int i=0;i<nodes.Length;i++)
            {
                int[] vbo_indices = new int[8];

                for (int j = 0; j < 8; j++)
                    vbo_indices[j] = (i * 8) + j;

                nodes[i] = new AnimationNode(
                    schem, 
                    vbo_indices,
                    -1
                    );
            }

            nodes[0].Pauses_OnCompletion = true;
            nodes[1].Transitions_OnComplete = true;
            nodes[1].Transitioning_Node = 3;
            nodes[2].Pauses_OnCompletion = true;
            nodes[3].Pauses_OnCompletion = true;
            nodes[3].Loop_Delay = 1.5;

            for (int i = 0; i < nodes.Length; i++)
                schem.DefineNode(i, nodes[i]);

            SpriteComponent = AnimationComponent = new AnimationComponent(schem);
            SpriteComponent.SetSprite("AbilityPoint");
            AnimationComponent.Play(3);
        }

        public void Use_Point()
            => AnimationComponent.Play(0);

        public void Replenish()
            => AnimationComponent.Play(1);

        public void Waste()
            => AnimationComponent.Play(2);
    }
}
