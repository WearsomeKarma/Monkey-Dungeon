using isometricgame.GameEngine;
using isometricgame.GameEngine.Components.Rendering;
using isometricgame.GameEngine.Rendering;
using isometricgame.GameEngine.Rendering.Animation;
using isometricgame.GameEngine.Scenes;
using isometricgame.GameEngine.Systems.Rendering;
using MonkeyDungeon.Components;
using MonkeyDungeon.GameFeatures;
using MonkeyDungeon.GameFeatures.Implemented.EntityResources;
using MonkeyDungeon.Prefabs.Components;
using MonkeyDungeon.Prefabs.UI;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
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
            internal set => SetNew_EntityComponent(value);
        }
        private void SetNew_EntityComponent(EntityComponent value)
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

            if (health != null)
            {
                Remove_HealthReference();
            }
            if ((health = value.Get_Resource(ENTITY_RESOURCES.HEALTH) as Health) != null)
            {
                Add_HealthReference();
            }
        }
        private void Remove_HealthReference()
        {
            health.ValueChanged -= updateHealth;
        }
        private void Add_HealthReference()
        {
            health.ValueChanged += updateHealth;
        }

        public string Name { get; private set; }
        protected RenderUnit UniqueIdentifier;
        public bool Has_UniqueIdentifier { get; private set; }
        protected RenderUnit Body;

        private ResourceBar healthBar;
        private Health health;
        private void updateHealth(EntityResource health)
            => healthBar.Percentage = health.Resource_StrictValue / health.Max_Value;

        private AnimationComponent AnimationComponent;
        internal MovementController Melee_MovementController { get; private set; }

        public CreatureGameObject(SceneLayer sceneLayer, Vector3 position, EntityComponent ec, int animRow=1, int animCol=8) 
            : base(sceneLayer, position)
        {
            healthBar = new ResourceBar(
                sceneLayer, 
                position + new Vector3(0, -35, 0), 
                isometricgame.GameEngine.Systems.MathHelper.Color_To_Vec4(Color.Red)
                );

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

            AddComponent(Melee_MovementController = new MovementController(new Vector3(), 1.5));
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
            if (health != null)
            {
                renderService.DrawObj(healthBar);
            }
        }
    }
}
