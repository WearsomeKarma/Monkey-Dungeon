using isometricgame.GameEngine;
using isometricgame.GameEngine.Components.Rendering;
using isometricgame.GameEngine.Rendering;
using isometricgame.GameEngine.Rendering.Animation;
using isometricgame.GameEngine.Scenes;
using isometricgame.GameEngine.Systems.Rendering;
using MonkeyDungeon.Components;
using MonkeyDungeon.GameFeatures;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.Prefabs.Entities
{
    public class CreatureGameObject : GameObject
    {
        public static readonly string Suffix_Unique = "Unique", Suffix_Body = "Body", Suffix_Head = "Head";

        private EntityComponent entityComponent;
        public EntityComponent EntityComponent
        {
            get => GetReservedField(ref entityComponent);
            internal set
            {
                ReplaceComponent(value);
                entityComponent = value;
                SpriteComponent.SetSprite(SceneLayer.Game.SpriteLibrary.ExtractRenderUnit(entityComponent.Race + Suffix_Head), true);
                Body = SceneLayer.Game.SpriteLibrary.ExtractRenderUnit(entityComponent.Race + Suffix_Body);
                Has_UniqueIdentifier = SceneLayer.Game.SpriteLibrary.HasSprite(entityComponent.Race + Suffix_Unique);
                if (Has_UniqueIdentifier)
                {
                    UniqueIdentifier = SceneLayer.Game.SpriteLibrary.ExtractRenderUnit(entityComponent.Race + Suffix_Unique);
                    UniqueIdentifier.VAO_Index = entityComponent.Unique_ID;
                }
            }
        }
        public string Name { get; private set; }
        protected RenderUnit UniqueIdentifier;
        public bool Has_UniqueIdentifier { get; private set; }
        protected RenderUnit Body;

        private AnimationComponent AnimationComponent;

        public CreatureGameObject(SceneLayer sceneLayer, Vector3 position, EntityComponent ec, int animRow=1, int animCol=8) 
            : base(sceneLayer, position)
        {
            //head anim
            AnimationSchematic schem = new AnimationSchematic(8);
            for (int i = 0; i < animRow; i++)
            {
                int[] subNodes = new int[animCol];
                for (int j = 0; j < animCol; j++)
                    subNodes[j] = i * animCol + j;
                schem.DefineNode(i, subNodes);
            }

            schem.SetSpeed(0.1);
            SpriteComponent = AnimationComponent = new AnimationComponent(schem);
            AnimationComponent.SetNode(0);
            EntityComponent = ec;
        }
        
        protected override void HandleDraw(RenderService renderService)
        {
            bool state = EntityComponent != null;
            float bodyHeight = 1;
            if (state)
            {
                bodyHeight = SceneLayer.Game.SpriteLibrary.GetSprite(EntityComponent.Race + Suffix_Body).SubHeight;
                if (SpriteComponent.Enabled)
                    renderService.DrawSprite(ref Body, Position.X, Position.Y - (bodyHeight * 0.3f));
            }
            base.HandleDraw(renderService);
            if (state && SpriteComponent.Enabled && Has_UniqueIdentifier)
                renderService.DrawSprite(ref UniqueIdentifier, Position.X, Position.Y + (bodyHeight * 0.3f));
        }
    }
}
