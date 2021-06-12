using System;
using isometricgame.GameEngine.Rendering;
using isometricgame.GameEngine.Scenes;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using OpenTK;

namespace MonkeyDungeon_UI.Prefabs.UI
{
    public class UI_Button_Target : UI_Button
    {
        private readonly GameEntity_Position POSITION;
        protected Action<GameEntity_Position, UI_Button> Button_Target_Click_Handler;
        
        public UI_Button_Target
            (
            SceneLayer sceneLayer, 
            Vector3 position, 
            Vector2 size, 
            GameEntity_Position gameLogicPosition,
            Action<GameEntity_Position, UI_Button> buttonClickHandler, 
            RenderUnit buttonVisual, 
            string text = "") 
            : base(sceneLayer, position, size, null, buttonVisual, text)
        {
            POSITION = gameLogicPosition;
            Button_Click_Handler = Handle_Button_Click;

            Button_Target_Click_Handler = buttonClickHandler;
        }

        private void Handle_Button_Click(UI_Button button)
        {
            Button_Target_Click_Handler?.Invoke(POSITION, button);
        }
    }
}