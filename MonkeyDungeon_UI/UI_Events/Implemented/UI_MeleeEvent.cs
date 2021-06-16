using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isometricgame.GameEngine.Tools;
using MonkeyDungeon_UI.Prefabs;
using MonkeyDungeon_UI.Prefabs.Entities;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;
using MonkeyDungeon_Vanilla_Domain.Multiplayer;
using OpenTK;

namespace MonkeyDungeon_UI.UI_Events.Implemented
{
    public class UI_MeleeEvent : UI_GameEvent
    {
        private GameEntity_WorldLayer_Roster WorldLayer_Roster { get; set; }

        private UI_EntityObject ally, enemy;

        internal GameEntity_ID Ally_Id { get; set; }
        internal GameEntity_ID Enemy_Id { get; set; }

        private Vector3 Ally_Side_Position { get; set; }
        private Vector3 Enemy_Side_Position { get; set; }

        internal UI_MeleeEvent
            (
            EventScheduler eventScheduler, 
            double duration, 
            Vector3 allySide, 
            Vector3 enemySide, 
            GameEntity_WorldLayer_Roster worldLayer_Roster
            )
            : base(eventScheduler, MD_VANILLA_UI_EVENT_NAMES.UI_EVENT_MELEE, duration)
        {
            WorldLayer_Roster = worldLayer_Roster;

            Ally_Side_Position = allySide;
            Enemy_Side_Position = enemySide;
        }



        protected override void Callback_Reset(double newDuration)
        {
            ally = WorldLayer_Roster.Get_UI_EntityObject(Ally_Id);
            enemy = WorldLayer_Roster.Get_UI_EntityObject(Enemy_Id);
        }

        protected override void Callback_DeltaTime(Timer timer)
        {
            float duration = (float)Duration;
            float deltaTime = (float)timer.Frame_DeltaTime;

            if (timer.TimeElapsed < Duration / 3)
            {
                Pan_From_Position(ally, ally.Inital_Position, Ally_Side_Position, deltaTime, duration / 3);
                Pan_From_Position(enemy, enemy.Inital_Position, Enemy_Side_Position, deltaTime, duration / 3);
                return;
            }
            else if (timer.TimeElapsed < Duration * 2 / 3)
                return;
            
            Pan_From_Position(ally, Ally_Side_Position, ally.Inital_Position, deltaTime, duration / 3);
            Pan_From_Position(enemy, Enemy_Side_Position, enemy.Inital_Position, deltaTime, duration / 3);
        }

        protected override void Callback_Elapsed()
        {
            ally.Position = ally.Inital_Position;
            enemy.Position = enemy.Inital_Position;
        }
    }
}
