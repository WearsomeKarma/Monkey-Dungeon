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

        private UI_GameEntity_Descriptor EntityDescription { get; set; }
        private RenderUnit target;
        public ResourceBar ResourceBar_Health { get; private set; }
        public ResourceBar ResourceBar_Stamina { get; private set; }
        public ResourceBar ResourceBar_Mana { get; private set; }

        private AbilityPoint[] abilityPoints = new AbilityPoint[3];
        private int abilityPointCount = 1;
        private int abilityPointIndex = 1;
                
        public StatusBar(SceneLayer sceneLayer, Vector3 position) 
            : base(sceneLayer, position, "statusBar")
        {
            spriteLibrary = sceneLayer.Game.SpriteLibrary;
            
            ResourceBar_Health = new ResourceBar(
                sceneLayer,
                Position + new Vector3(320, 340, 0),
                new Vector4(255, 0, 0, 1)
            );

            ResourceBar_Stamina = new ResourceBar(
                sceneLayer,
                Position + new Vector3(320, 310, 0),
                new Vector4(255, 255, 0, 1)
            );
            
            ResourceBar_Mana = new ResourceBar(
                sceneLayer,
                Position + new Vector3(320, 280, 0),
                new Vector4(0, 0, 255, 1)
            );

            for(int i=0;i<abilityPoints.Length;i++)
            {
                abilityPoints[i] = new AbilityPoint(sceneLayer, position + new Vector3(40 * i, 220, 0));
            }

            abilityPoints[2].SpriteComponent.Enabled = false;
        }

        public void Set_EntityFocus(UI_GameEntity_Descriptor entityDescription)
        {
            //TODO: make this better.
            if (EntityDescription != null)
                EntityDescription.Resources_Updated -= EntityDescription_Resources_Updated;

            EntityDescription = entityDescription;
            EntityDescription.Resources_Updated += EntityDescription_Resources_Updated;
            target = spriteLibrary.ExtractRenderUnit(entityDescription.RACE +  CreatureGameObject.Suffix_Head);
            Entity_SceneID = EntityDescription.SCENE_ID;

            update_Entity(EntityDescription);
        }

        private void EntityDescription_Resources_Updated()
        {
            update_Entity(EntityDescription);
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
            foreach (AbilityPoint ap in abilityPoints)
                ap.OnUpdate(args);
        }

        public void ReplenishPoints()
        {
            abilityPointIndex = abilityPointCount;
            for (int i = 0; i < abilityPointCount + 1; i++)
                abilityPoints[i].Replenish();
        }

        public void Use_Point()
        {
            abilityPoints[abilityPointIndex].Use_Point();
            abilityPointIndex--;
        }

        protected override void HandleDraw(RenderService renderService)
        {
            base.HandleDraw(renderService);
            renderService.DrawObj(ResourceBar_Health);
            renderService.DrawObj(ResourceBar_Stamina);
            renderService.DrawObj(ResourceBar_Mana);

            foreach (AbilityPoint ap in abilityPoints)
                if (ap.SpriteComponent.Enabled)
                    renderService.DrawObj(ap);

            renderService.DrawSprite(ref target, Position.X + 20, Position.Y + 240);
        }
    }
}
