using System;
using isometricgame.GameEngine.Rendering;
using MonkeyDungeon_UI.Scenes.GameScenes;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using OpenTK;

namespace MonkeyDungeon_UI.Prefabs.UI
{
    public class UI_GameEntity_Survey_Target_Buttons : GameEntity_Survey<UI_Button_Target>
    {
        public event Action<GameEntity_Position, UI_Button_Target> Button_Clicked;
        
        private UI_Combat_Layer UI_Combat_Layer { get; set; }
        
        private readonly GameEntity_Position_Vector_Survey VECTOR_SURVEY;

        public UI_Button_Target[] Get_Buttons()
            => Get__Reduced_Field__Survey();
        
        internal UI_GameEntity_Survey_Target_Buttons(UI_Combat_Layer sceneLayer, Vector3[] vectorSpace, RenderUnit buttonVisual) 
            : base(null)
        {
            VECTOR_SURVEY = new GameEntity_Position_Vector_Survey(vectorSpace);
            
            GameEntity_Position.For_Each__Position(GameEntity_Team_ID.ID_NULL,
                (p) =>
                {
                    UI_Button_Target buttonTarget = new UI_Button_Target
                    (
                        sceneLayer,
                        VECTOR_SURVEY.Map(p),
                        new Vector2(50, 50),
                        p,
                        Handle_Button_Clicked,
                        buttonVisual
                    );

                    buttonTarget.Enabled = false;
                    buttonTarget.SpriteComponent.Enabled = false;
                    
                    Set__Entry_By_Position__Survey
                    (
                        p, 
                        buttonTarget
                    );
                });
        }

        public void Set_Button_States(GameEntity_Team_ID teamID, bool state)
        {
            GameEntity_Position.For_Each__Position(teamID, (p) =>
            {
                Set_Button_State(p, state);
            });
        }

        public void Set_Button_State(GameEntity_Position position, bool state)
        {
            FIELD[position].Enabled = state;
            FIELD[position].SpriteComponent.Enabled = state;
        }

        private void Handle_Button_Clicked(GameEntity_Position position, UI_Button button)
        {
            Button_Clicked?.Invoke(position, button as UI_Button_Target);
        }
    }
}