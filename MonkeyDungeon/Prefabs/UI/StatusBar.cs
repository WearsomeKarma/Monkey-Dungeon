using isometricgame.GameEngine;
using isometricgame.GameEngine.Events.Arguments;
using isometricgame.GameEngine.Rendering;
using isometricgame.GameEngine.Scenes;
using isometricgame.GameEngine.Systems.Rendering;
using MonkeyDungeon.Components;
using MonkeyDungeon.GameFeatures;
using MonkeyDungeon.GameFeatures.Implemented.EntityResources;
using MonkeyDungeon.Prefabs.Entities;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.Prefabs.UI
{
    public class StatusBar : GameObject
    {
        private SpriteLibrary spriteLibrary;

        Health health;
        Stamina stamina;
        Mana mana;

        private RenderUnit target;
        public void SetTarget(EntityComponent target_EC)
        {
            target = spriteLibrary.ExtractRenderUnit(target_EC.Race + CreatureGameObject.Suffix_Head);

            health = target_EC.Get_ResourceByType<Health>();
            stamina = target_EC.Get_ResourceByType<Stamina>();
            mana = target_EC.Get_ResourceByType<Mana>();
        }

        public override void OnUpdate(FrameArgument args)
        {
            ResourceBar_Health.Percentage = health.Resource_StrictValue / health.Max_Value;
            ResourceBar_Stamina.Percentage = stamina.Resource_StrictValue / stamina.Max_Value;
            ResourceBar_Mana.Percentage = mana.Resource_StrictValue / mana.Max_Value;
            base.OnUpdate(args);
        }

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
                isometricgame.GameEngine.Systems.MathHelper.Color_To_Vec4(Color.Red)
            );

            ResourceBar_Stamina = new ResourceBar(
                sceneLayer,
                Position + new Vector3(320, 310, 0),
                isometricgame.GameEngine.Systems.MathHelper.Color_To_Vec4(Color.Yellow)
            );

            ResourceBar_Mana = new ResourceBar(
                sceneLayer,
                Position + new Vector3(320, 280, 0),
                isometricgame.GameEngine.Systems.MathHelper.Color_To_Vec4(Color.Blue)
            );
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
