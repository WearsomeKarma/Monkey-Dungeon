using isometricgame.GameEngine;
using isometricgame.GameEngine.Components.Rendering;
using isometricgame.GameEngine.Scenes;
using isometricgame.GameEngine.Systems.Rendering;
using isometricgame.GameEngine.Tools;
using MonkeyDungeon.Prefabs.Components;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.Prefabs.UI
{
    public class AnnouncementMessage : GameObject
    {
        private GameAnnouncement_MovementController gamc;
        internal TimedCallback EventTimer => gamc.EventTimer;
        private string announcement;

        public AnnouncementMessage(SceneLayer sceneLayer, Vector3 position) 
            : base(sceneLayer, position, "announcement")
        {
            AddComponent(
                gamc = new GameAnnouncement_MovementController(
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
