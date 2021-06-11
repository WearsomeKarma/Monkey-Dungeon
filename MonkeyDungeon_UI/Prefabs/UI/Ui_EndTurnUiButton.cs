using isometricgame.GameEngine;
using isometricgame.GameEngine.Components.Rendering;
using isometricgame.GameEngine.Rendering;
using isometricgame.GameEngine.Rendering.Animation;
using isometricgame.GameEngine.Scenes;
using MonkeyDungeon_UI.Scenes.GameScenes;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_UI.Prefabs.UI
{
    public class Ui_EndTurnUiButton : UI_Button
    {
        private AnimationComponent AnimationComponent { get; set; }
        private UI_Combat_Layer UI_Combat_Layer { get; set; }

        private bool clicked = false;

        public Ui_EndTurnUiButton(
            UI_Combat_Layer sceneLayer, 
            Vector3 position) 
            : base(sceneLayer, position, new Vector2(128, 128), null, new RenderUnit(), "")
        {
            UI_Combat_Layer = sceneLayer;

            AnimationSchematic schem = new AnimationSchematic(2, 0.05);
            AnimationNode[] nodes = new AnimationNode[] {
                new AnimationNode(schem, new int[] { 0,1,2,3,4,5,6,7 }, -1, true),
                new AnimationNode(schem, new int[] { 8,9,10,11,12,13,14,15 }, -1, true)
            };

            schem.DefineNode(0, nodes[0]);
            schem.DefineNode(1, nodes[1]);

            SpriteComponent = AnimationComponent = new AnimationComponent(schem);
            AnimationComponent.SetSprite("EndTurnButton");
            AnimationComponent.Play(1);
        }

        public void RefreshButton()
        {
            clicked = false;
            AnimationComponent.Play(1);
        }

        protected override void GainFocus()
        {
            if (clicked)
                return;

            clicked = true;
            UI_Combat_Layer.Inform_EndTurn();
            AnimationComponent.Play(0);
        }
    }
}
