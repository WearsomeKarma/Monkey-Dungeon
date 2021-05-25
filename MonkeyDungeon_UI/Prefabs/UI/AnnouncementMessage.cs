using isometricgame.GameEngine;
using isometricgame.GameEngine.Components.Rendering;
using isometricgame.GameEngine.Scenes;
using isometricgame.GameEngine.Systems.Rendering;
using isometricgame.GameEngine.Tools;
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
        private string announcement;

        public AnnouncementMessage(SceneLayer sceneLayer, Vector3 position) 
            : base(sceneLayer, position, "announcement")
        {
        }

        public void Set_Announcement(string msg) => announcement = msg;

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
