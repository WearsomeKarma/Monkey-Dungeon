using isometricgame.GameEngine;
using isometricgame.GameEngine.Components.Rendering;
using isometricgame.GameEngine.Rendering;
using isometricgame.GameEngine.Rendering.Animation;
using isometricgame.GameEngine.Scenes;
using isometricgame.GameEngine.Systems.Rendering;
using MonkeyDungeon_UI.Prefabs.Components;
using MonkeyDungeon_UI.Prefabs.UI;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_UI.Prefabs.Entities
{
    public class CreatureGameObject : GameObject
    {
        public static readonly string Suffix_Unique = "Unique", Suffix_Body = "Body", Suffix_Head = "Head";

        public string Name { get; internal set; }
        public string Race { get; internal set; }
        public int Scene_ID { get; internal set; }

        protected RenderUnit UniqueIdentifier;
        internal uint UniqueIdentifier_TypeIndex { get => UniqueIdentifier.VAO_Index; set { UniqueIdentifier.VAO_Index = value; Has_UniqueIdentifier = true; } }
        public bool Has_UniqueIdentifier { get; private set; }
        protected RenderUnit Body;

        private ResourceBar healthBar;
        private void updateHealth(float percentageHealth)
            => healthBar.Percentage = percentageHealth;

        private UI_GameEntity_Descriptor entityDescription;
        internal UI_GameEntity_Descriptor EntityDescription
        {
            get => entityDescription; set
            {
                entityDescription = value;
                updateHealth(UI_Percentage_Health);
            }
        }
        public string UI_Race => EntityDescription.RACE;
        public float UI_Percentage_Health => EntityDescription.Percentage_Health;
        public float UI_Percentage_Stamina => EntityDescription.Percentage_Stamina;
        public float UI_Percentage_Mana => EntityDescription.Percentage_Mana;

        private AnimationComponent AnimationComponent;
        internal MovementController Melee_MovementController { get; private set; }

        public CreatureGameObject(SceneLayer sceneLayer, Vector3 position, UI_GameEntity_Descriptor entity = null, int animRow = 1, int animCol = 8)
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

            Set_Race(entity);

            AddComponent(Melee_MovementController = new MovementController(new Vector3(), 1.5));
        }

        internal void Set_Race(UI_GameEntity_Descriptor entity)
        {
            //TODO: Centralize primitives.
            string race = entity?.RACE ?? "Monkey";
            uint uniqueIdentifier = entity?.UNIQUE_IDENTIFIER ?? 0;

            string h = race + Suffix_Head;
            string b = race + Suffix_Body;
            string u = race + Suffix_Unique;

            if (!SceneLayer.Game.SpriteLibrary.HasSprite(h))
                throw new ArgumentException();

            Race = entity?.RACE ?? "Monkey";

            SpriteComponent.SetSprite(h);
            Body = SceneLayer.Game.SpriteLibrary.ExtractRenderUnit(b);

            if (Has_UniqueIdentifier = SceneLayer.Game.SpriteLibrary.HasSprite(u))
            {
                UniqueIdentifier = SceneLayer.Game.SpriteLibrary.ExtractRenderUnit(u);
                UniqueIdentifier.VAO_Index = uniqueIdentifier;
            }
        }

        internal void Set_Unique_ID(uint uid)
        {
            UniqueIdentifier.VAO_Index = uid;
            EntityDescription.UNIQUE_IDENTIFIER = uid;
        }

        protected override void HandleDraw(RenderService renderService)
        {
            float bodyHeight = 1;
            bodyHeight = SceneLayer.Game.SpriteLibrary.GetSprite(Race + Suffix_Body).SubHeight;
            if (SpriteComponent.Enabled)
                renderService.DrawSprite(ref Body, Position.X, Position.Y - (bodyHeight * 0.3f));
            base.HandleDraw(renderService);
            if (SpriteComponent.Enabled && Has_UniqueIdentifier)
                renderService.DrawSprite(ref UniqueIdentifier, Position.X, Position.Y + (bodyHeight * 0.3f));

            renderService.DrawObj(healthBar);
        }
    }
}
