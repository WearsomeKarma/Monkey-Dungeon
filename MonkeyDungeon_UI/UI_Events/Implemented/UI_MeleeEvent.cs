using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using isometricgame.GameEngine.Tools;
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
        private CreatureGameObject[] Players { get; set; }
        private CreatureGameObject[] Enemies { get; set; }

        private CreatureGameObject ally, enemy;

        internal int Ally_Id { get; set; }
        internal int Enemy_Id { get; set; }

        private Vector3 Ally_Side_Position { get; set; }
        private Vector3 Enemy_Side_Position { get; set; }

        internal UI_MeleeEvent(
            EventScheduler eventScheduler, 
            double duration, 
            Vector3 allySide, 
            Vector3 enemySide, 
            CreatureGameObject[] players,
            CreatureGameObject[] enemies)
            : base(eventScheduler, MD_VANILLA_UI_EVENTS.UI_EVENT_MELEE, duration)
        {
            Players = players;
            Enemies = enemies;
        }



        protected override void Callback_Reset(double newDuration)
        {
            ally = Players[Ally_Id];
            enemy = Enemies[Enemy_Id];
        }

        protected override void Callback_DeltaTime(Timer timer)
        {
            float duration = (float)Duration;
            float deltaTime = (float)timer.Frame_DeltaTime;

            CreatureGameObject ally = Players[Ally_Id];
            CreatureGameObject enemy = Enemies[Enemy_Id];

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
            Players[Ally_Id].Position = Players[Ally_Id].Inital_Position;
            Enemies[Enemy_Id].Position = Enemies[Enemy_Id].Inital_Position;
        }
    }
}
