using isometricgame.GameEngine;
using isometricgame.GameEngine.Events.Arguments;
using isometricgame.GameEngine.Rendering;
using isometricgame.GameEngine.Scenes;
using isometricgame.GameEngine.Systems.Input;
using isometricgame.GameEngine.Systems.Rendering;
using OpenTK;
using OpenTK.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_UI.Prefabs.UI
{
    public class TextField : Button
    {
        public static InputHandler QWERTY_KeyboardInputHandler;
        public static readonly string BLANK = "____";
        public static readonly List<string> NUMBERS = new List<string>()
        { "Number0", "Number1", "Number2", "Number3", "Number4", "Number5", "Number6", "Number7", "Number8", "Number9" };
        public static readonly Dictionary<string, string> UNIQUE_CHARS = new Dictionary<string, string>()
        {
            { Key.Slash.ToString(), "/" },
            { Key.BackSlash.ToString(), "\\" },
            { Key.Minus.ToString(), "-" },
            { Key.Plus.ToString(), "=" },
            { Key.Period.ToString(), "." },
            { Key.Comma.ToString(), "," },
            { Key.Semicolon.ToString(), ";" }
        };
        public static readonly Dictionary<string, string> UNIQUE_CHARS_SHIFT = new Dictionary<string, string>()
        {
            { Key.Slash.ToString(), "?" },
            { Key.BackSlash.ToString(), "|" },
            { Key.Minus.ToString(), "_" },
            { Key.Plus.ToString(), "+" },
            { Key.Period.ToString(), ">" },
            { Key.Comma.ToString(), "<" },
            { Key.Semicolon.ToString(), ":" }
        };

        string label = "Text Field";
        public bool caps = false;

        public TextField(SceneLayer sceneLayer, Vector3 position, Vector2 size, RenderUnit buttonVisual, string label="Text Field:") 
            : base(sceneLayer, position, size, (b) => { }, buttonVisual, "")
        {
            this.label = label;
            if (QWERTY_KeyboardInputHandler == null)
            {
                QWERTY_KeyboardInputHandler = SceneLayer.Game.InputSystem.RegisterHandler(InputType.Keyboard_UpDown);
                QWERTY_KeyboardInputHandler.DeclareSwitch(Key.CapsLock.ToString());
            }
        }

        public override void OnUpdate(FrameArgument args)
        {
            base.OnUpdate(args);
            
            if (QWERTY_KeyboardInputHandler.EvaluatePulseState("keyboard_any", true))
            {
                if (QWERTY_KeyboardInputHandler.EvaluateSwitchState(Key.CapsLock.ToString()))
                {
                    caps = !caps;
                }
                else if (QWERTY_KeyboardInputHandler.EvaluatePulseState("keyboard_any"))
                {
                    if (Text == BLANK)
                        Text = "";

                    Key key = QWERTY_KeyboardInputHandler.Keyboard_UpDown.Key;
                    KeyboardState keyboard = QWERTY_KeyboardInputHandler.Keyboard_UpDown.Keyboard;
                    bool isShift = keyboard.IsKeyDown(Key.ShiftLeft) || keyboard.IsKeyDown(Key.ShiftRight);

                    string stringKey = key.ToString();
                    int index;
                    if ((index = NUMBERS.IndexOf(stringKey)) > -1)
                        stringKey = index.ToString();
                    if (UNIQUE_CHARS.ContainsKey(stringKey))
                        stringKey = (isShift) ? UNIQUE_CHARS_SHIFT[stringKey] : UNIQUE_CHARS[stringKey];

                    stringKey = (caps || isShift) ? stringKey : stringKey.ToLower();
                    if (stringKey.Length == 1)
                        Text += stringKey;
                    else if (key == Key.Space)
                        Text += ' ';
                    else if (key == Key.BackSpace && Text.Length > 0)
                        Text = Text.Substring(0, Text.Length - 1);
                }
            }
        }

        protected override void GainFocus()
        {
            Text = BLANK;
        }

        protected override void LoseFocus()
        {
            if (Text == BLANK)
                Text = "";
        }

        protected override void HandleDraw(RenderService renderService)
        {
            SceneLayer.Game.TextDisplayer.DrawText(renderService, label, "font", Position.X, Position.Y + 110);
            base.HandleDraw(renderService);
        }
    }
}
