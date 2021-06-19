using System;
using MonkeyDungeon_Core.GameFeatures.GameComponents.EntityAttributes.Abilities;
using MonkeyDungeon_Core.GameFeatures.GameStates.Combat.ActionResolutionStages;

namespace MonkeyDungeon_Core.GameFeatures.GameStates.Combat
{
    public class Combat_Action_Resolver
    {
        internal GameState_Combat GameStateCombat { get; set; }

        private readonly Combat_Action_Resolution_Stage[] BASE_PROCEDURE;
        private int Resolution_Stage_Index { get; set; }

        public Combat_Action_Resolver(GameState_Combat gameStateCombat)
        {
            GameStateCombat = gameStateCombat;

            BASE_PROCEDURE = new Combat_Action_Resolution_Stage[]
            {
                new Resolution_Stage_Cast(),
                new Resolution_Stage_Hit_Bonus(),
                new Resolution_Stage_Redirection(),
                new Resolution_Stage_Dodge_Bonus(),
                new Resolution_Stage_Damage()
            };
            foreach (Combat_Action_Resolution_Stage stage in BASE_PROCEDURE)
                stage.Bind_To_Resolver(this, gameStateCombat);
        }

        public void Resolve_Action(GameEntity_ServerSide_Action action)
        {
            action.Begin_Action_Resolution(GameStateCombat.Game_Field);
            
            Resolution_Stage_Index = 0;
            while(Resolution_Stage_Index < BASE_PROCEDURE.Length)
            {
                Console.WriteLine(BASE_PROCEDURE[Resolution_Stage_Index]);
                BASE_PROCEDURE[Resolution_Stage_Index].Begin_Stage(action);
                Resolution_Stage_Index++;
            }
        }
    }
}
