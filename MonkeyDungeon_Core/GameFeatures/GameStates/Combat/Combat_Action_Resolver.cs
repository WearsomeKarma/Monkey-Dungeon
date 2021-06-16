using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities;
using MonkeyDungeon_Core.GameFeatures.GameStates.Combat.ActionResolutionStages;

namespace MonkeyDungeon_Core.GameFeatures.GameStates.Combat
{
    public class Combat_Action_Resolver
    {
        internal Combat_GameState Combat { get; set; }

        private readonly Combat_Action_Resolution_Stage[] BASE_PROCEDURE;
        private int Resolution_Stage_Index { get; set; }

        public Combat_Action_Resolver(Combat_GameState combat)
        {
            Combat = combat;

            BASE_PROCEDURE = new Combat_Action_Resolution_Stage[]
            {
                new Resolution_Stage_Cast(),
                new Resolution_Stage_Hit_Bonus(),
                new Resolution_Stage_Redirect_Chance(),
                new Resolution_Stage_Dodge_Bonus(),
                new Resolution_Stage_Damage()
            };
            foreach (Combat_Action_Resolution_Stage stage in BASE_PROCEDURE)
                stage.Bind_To_Resolver(this, combat);
        }

        public void Resolve_Action(Combat_Action action)
        {
            action.Begin_Action_Resolution(Combat.Game_Field);
            
            Resolution_Stage_Index = 0;
            while(Resolution_Stage_Index < BASE_PROCEDURE.Length)
            {
                BASE_PROCEDURE[Resolution_Stage_Index].Begin_Stage(action);
                Resolution_Stage_Index++;
            }
        }
    }
}
