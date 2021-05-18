using isometricgame.GameEngine;
using isometricgame.GameEngine.Events.Arguments;
using isometricgame.GameEngine.Rendering;
using isometricgame.GameEngine.Scenes;
using isometricgame.GameEngine.Systems.Rendering;
using MonkeyDungeon_UI.Prefabs.Entities;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_UI.Prefabs.UI
{
    public class StatusBar : GameObject
    {
        private SpriteLibrary spriteLibrary;

        private int Entity_SceneID { get; set; }

        private RenderUnit target;
        public ResourceBar ResourceBar_Health { get; private set; }
        public ResourceBar ResourceBar_Stamina { get; private set; }
        public ResourceBar ResourceBar_Mana { get; private set; }
                
        public StatusBar(SceneLayer sceneLayer, Vector3 position) 
            : base(sceneLayer, position, "statusBar")
        {
            spriteLibrary = sceneLayer.Game.SpriteLibrary;
            
            ResourceBar_Health = new ResourceBar(
                sceneLayer,
                Position + new Vector3(320, 340, 0),
                new Vector4(0,0,0,1)
            );

            ResourceBar_Stamina = new ResourceBar(
                sceneLayer,
                Position + new Vector3(320, 310, 0),
                new Vector4(0, 0, 0, 1)
            );

            ResourceBar_Mana = new ResourceBar(
                sceneLayer,
                Position + new Vector3(320, 280, 0),
                new Vector4(0, 0, 0, 1)
            );
        }

        public void Set_EntityFocus(UI_GameEntity_Descriptor entityDescription)
        {
            target = spriteLibrary.ExtractRenderUnit(entityDescription.RACE + CreatureGameObject.Suffix_Head);
            Entity_SceneID = entityDescription.SCENE_ID;

            update_Entity(entityDescription);
        }

        internal void Update_Entity(UI_GameEntity_Descriptor entityDescription)
        {
            if (entityDescription.SCENE_ID == Entity_SceneID)
            {
                update_Entity(entityDescription);
            }
        }

        private void update_Entity(UI_GameEntity_Descriptor entityDescription)
        {
            ResourceBar_Health.Percentage = entityDescription.Percentage_Health;
            ResourceBar_Stamina.Percentage = entityDescription.Percentage_Stamina;
            ResourceBar_Mana.Percentage = entityDescription.Percentage_Mana;
        }

        public override void OnUpdate(FrameArgument args)
        {
            base.OnUpdate(args);
        }

        protected override void HandleDraw(RenderService renderService)
        {
            base.HandleDraw(renderService);
            renderService.DrawSprite(ref target, Position.X + 20, Position.Y + 240);
            renderService.DrawObj(ResourceBar_Health);
            renderService.DrawObj(ResourceBar_Stamina);
            renderService.DrawObj(ResourceBar_Mana);
        }
    }
}
