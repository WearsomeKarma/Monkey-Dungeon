using isometricgame.GameEngine;
using isometricgame.GameEngine.Scenes;
using isometricgame.GameEngine.Tools;
using MonkeyDungeon_UI.Prefabs.UI;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_UI.Scenes.GameScenes
{
    public class UI_PauseMenu_Layer : SceneLayer
    {
        private EventScheduler EventScheduler { get; set; }

        internal AnnouncementMessage announcement;

        internal UI_PauseMenu_Layer(GameScene parentScene) 
            : base(parentScene)
        {
            EventScheduler = parentScene.EventScheduler;

            Add_StaticObject(announcement = new AnnouncementMessage(this, new Vector3(-Game.Width / 2 - 310, Game.Height / 4, 0)));
        }
    }
}
