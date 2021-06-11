using isometricgame.GameEngine;
using isometricgame.GameEngine.Events.Arguments;
using isometricgame.GameEngine.Rendering;
using isometricgame.GameEngine.Scenes;
using isometricgame.GameEngine.Systems.Rendering;
using MonkeyDungeon_UI.Prefabs.Entities;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;

namespace MonkeyDungeon_UI.Prefabs.UI
{
    public class UI_StatusBar : GameObject
    {
        private SpriteLibrary spriteLibrary;

        private int Entity_SceneID { get; set; }

        private GameEntity_ClientSide EntityDescription { get; set; }
        private RenderUnit target;
        public UI_ResourceBar UiResourceBar_Health { get; private set; }
        public UI_ResourceBar UiResourceBar_Stamina { get; private set; }
        public UI_ResourceBar UiResourceBar_Mana { get; private set; }
        public UI_ResourceBar[] ResourceBars { get; private set; }

        private UI_AbilityPoint[] abilityPoints = new UI_AbilityPoint[3];
        private int abilityPointCount = 2;
                
        public UI_StatusBar(SceneLayer sceneLayer, Vector3 position) 
            : base(sceneLayer, position, "statusBar")
        {
            spriteLibrary = sceneLayer.Game.SpriteLibrary;
            
            UiResourceBar_Health = new UI_ResourceBar(
                sceneLayer,
                Position + new Vector3(320, 340, 0),
                new Vector4(255, 0, 0, 1),
                MD_VANILLA_RESOURCE_NAMES.RESOURCE_HEALTH
            );

            UiResourceBar_Stamina = new UI_ResourceBar(
                sceneLayer,
                Position + new Vector3(320, 310, 0),
                new Vector4(255, 255, 0, 1),
                MD_VANILLA_RESOURCE_NAMES.RESOURCE_STAMINA
            );
            
            UiResourceBar_Mana = new UI_ResourceBar(
                sceneLayer,
                Position + new Vector3(320, 280, 0),
                new Vector4(0, 0, 255, 1),
                MD_VANILLA_RESOURCE_NAMES.RESOURCE_MANA
            );

            ResourceBars = new UI_ResourceBar[]
            {
                UiResourceBar_Health,
                UiResourceBar_Mana,
                UiResourceBar_Stamina
            };

            for(int i=0;i<abilityPoints.Length;i++)
            {
                abilityPoints[i] = new UI_AbilityPoint(sceneLayer, position + new Vector3(40 * i, 220, 0));
            }

            abilityPoints[2].SpriteComponent.Enabled = false;
        }

        public void Set_EntityFocus(GameEntity_ClientSide entityDescription)
        {
            if (EntityDescription != null)
                Detatch_From_Entity();

            Attach_From_Entity(entityDescription);
        }

        private void Detatch_From_Entity()
        {
            EntityDescription.Unsubscribe_To_Resource_Changes(ResourceBars);
            EntityDescription.Ability_Points.Resource_Updated -= Handle_AbilityPoint_Change;
        }

        private void Attach_From_Entity(GameEntity_ClientSide entity)
        {
            EntityDescription = entity;
            EntityDescription.Subscribe_To_Resource_Changes(ResourceBars);
            EntityDescription.Ability_Points.Resource_Updated += Handle_AbilityPoint_Change;
            Handle_AbilityPoint_Change((int)EntityDescription.Ability_Points.Resource_Percentage);

            target = spriteLibrary.ExtractRenderUnit(EntityDescription.GameEntity_Race + UI_EntityObject.Suffix_Head);
            Entity_SceneID = EntityDescription.SCENE_ID;
        }

        public override void OnUpdate(FrameArgument args)
        {
            base.OnUpdate(args);
            foreach (UI_AbilityPoint ap in abilityPoints)
                ap.OnUpdate(args);
        }

        internal void Dump_AbilityPoints()
        {
            foreach (UI_AbilityPoint ap in abilityPoints)
                ap.Waste();
        }

        private void Handle_AbilityPoint_Change(float val)
        {
            int index = 0;
            if (abilityPointCount > 0)
                index = abilityPointCount - 1;
            int ival = (int)val;
            int diff = abilityPointCount - ival;
            if (diff == 0)
                return;
            int stepDist = Math.Abs(diff);
            abilityPointCount -= diff;

            bool waste = diff > 0;
            int step = (waste) ? -1 : 1;

            for (int i = 0; i < stepDist; i++)
            {
                if (waste)
                {
                    abilityPoints[index + (i * step)].Use_Point();
                    continue;
                }
                abilityPoints[i].Replenish();
            }
        }

        protected override void HandleDraw(RenderService renderService)
        {
            base.HandleDraw(renderService);
            renderService.DrawObj(UiResourceBar_Health);
            renderService.DrawObj(UiResourceBar_Stamina);
            renderService.DrawObj(UiResourceBar_Mana);

            foreach (UI_AbilityPoint ap in abilityPoints)
                if (ap.SpriteComponent.Enabled)
                    renderService.DrawObj(ap);

            if (EntityDescription != null)
                renderService.DrawSprite(ref target, Position.X + 20, Position.Y + 240);
        }
    }
}
