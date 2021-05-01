using isometricgame.GameEngine;
using isometricgame.GameEngine.Scenes;
using isometricgame.GameEngine.Tools;
using MonkeyDungeon.Prefabs.UI;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.Scenes.GameScenes
{
    public class UI_Base_Layer : SceneLayer
    {
        private EventScheduler EventScheduler { get; set; }

        internal AnnouncementMessage announcement;

        public UI_Base_Layer(GameScene parentScene) 
            : base(parentScene)
        {
            EventScheduler = parentScene.EventScheduler;

            Add_StaticObject(announcement = new AnnouncementMessage(this, new Vector3(-Game.Width / 2 - 310, Game.Height / 4, 0)));
            EventScheduler.Register_Event("announce", announcement.EventTimer);
        }

        internal void Announce(string msg)
        {
            announcement.SetAnnouncement(msg);
            EventScheduler.Invoke_Event("announce");
        }
    }
}
