using isometricgame.GameEngine;
using isometricgame.GameEngine.Components.Rendering;
using isometricgame.GameEngine.Events.Arguments;
using isometricgame.GameEngine.Rendering;
using isometricgame.GameEngine.Scenes;
using isometricgame.GameEngine.Systems.Input;
using isometricgame.GameEngine.Systems.Rendering;
using MonkeyDungeon_UI.Physics;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_UI.Prefabs.UI
{
    public class UI_Button : GameObject
    {
        public static InputHandler mouseInputHandler;
        private Action<UI_Button> buttonClickHandler;
        private PrimitiveHitbox hitbox;
        public string Text { get; set; }
        public bool Enabled { get; set; }

        public UI_Button(SceneLayer sceneLayer, Vector3 position, Vector2 size, Action<UI_Button> buttonClickHandler, RenderUnit buttonVisual, string text = "") 
            : base(sceneLayer, position)
        {
            if (mouseInputHandler == null)
            {
                mouseInputHandler = sceneLayer.Game.InputSystem.RegisterHandler(InputType.Mouse_Button);

                mouseInputHandler.DeclarePulse(MouseButton.Left.ToString());
            }

            Enabled = true;
            AddComponent(hitbox = new PrimitiveHitbox(size));
            SpriteComponent = new SpriteComponent();
            SpriteComponent.SetSprite(buttonVisual);
            Position = position;

            this.buttonClickHandler = buttonClickHandler;
            Text = text;
        }

        public override void OnUpdate(FrameArgument args)
        {
            base.OnUpdate(args);

            if (Enabled && mouseInputHandler.EvaluatePulseState("Left", true))
            {
                Point position = mouseInputHandler.Mouse_Button.Position;
                position.X -= SceneLayer.Game.Width / 2;
                position.Y -= SceneLayer.Game.Height / 2;
                if (hitbox.WithinHitBox(Position, new Vector3(position.X, position.Y, 0)))
                {
                    if (mouseInputHandler.EvaluatePulseState("Left"))
                    {
                        GainFocus();
                    }
                }
                else
                {
                    LoseFocus();
                }
            }
        }

        protected virtual void GainFocus()
        {
            buttonClickHandler.Invoke(this);
        }

        protected virtual void LoseFocus()
        {

        }

        protected override void HandleDraw(RenderService renderService)
        {
            base.HandleDraw(renderService);
            if (SpriteComponent.Enabled)
                SceneLayer.Game.TextDisplayer.DrawText
                    (
                    renderService,
                    Text, 
                    "font", 
                    Position.X - (Text.Length / 2 * 18) + (hitbox.Size.X/2), 
                    Position.Y + (hitbox.Size.Y/2) - 14);
        }
    }
}
