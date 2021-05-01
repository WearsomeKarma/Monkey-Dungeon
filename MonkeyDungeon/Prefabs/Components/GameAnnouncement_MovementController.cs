using isometricgame.GameEngine.Components.Rendering;
using isometricgame.GameEngine.Tools;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.Prefabs.Components
{
    public class GameAnnouncement_MovementController : MovementController
    {
        public GameAnnouncement_MovementController(Vector3 targetPosition, double time)
            : base(targetPosition, time)
        {
            //TODO: make it work with the future UI framework system!
        }
    }
}
