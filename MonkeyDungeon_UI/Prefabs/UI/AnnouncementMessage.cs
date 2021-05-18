using isometricgame.GameEngine;
using isometricgame.GameEngine.Components.Rendering;
using isometricgame.GameEngine.Scenes;
using isometricgame.GameEngine.Systems.Rendering;
using isometricgame.GameEngine.Tools;
using MonkeyDungeon_UI.Prefabs.Components;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_UI.Prefabs.UI
{
    public class AnnouncementMessage : GameObject
    {
        public static readonly string EVENT_TAG = "event_announcment";

        private readonly GameAnnouncement_MovementController UI_MovementController;
        internal TimedCallback EventController => UI_MovementController.EventTimer;
        
        private string announcement;

        public AnnouncementMessage(SceneLayer sceneLayer, Vector3 position) 
            : base(sceneLayer, position, "announcement")
        {
            AddComponent(
                UI_MovementController = new GameAnnouncement_MovementController(
                    new Vector3(-150, position.Y, 0),
                    1.5
                    )
                );
        }

        public void SetAnnouncement(string msg) => announcement = msg;

        protected override void HandleDraw(RenderService renderService)
        {
            base.HandleDraw(renderService);
            
            SceneLayer.Game.TextDisplayer.DrawText(
                renderService,
                announcement ?? "",
                "font",
                Position.X + 150,
                Position.Y + 100
                );
        }
    }
}
