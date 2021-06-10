using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures.GameStates.Combat
{
    public abstract class Combat_Action_Resolution_Stage
    {
        private Combat_GameState Combat { get; set; }
        protected GameEntity_Field_RosterEntry Entity_FieldRosterEntry => Combat.Game_FieldRosterEntry;
        protected GameEntity_RosterEntry Get_Owner_Entity_Of_Action(GameEntity_ID id)
            => Entity_FieldRosterEntry.Get_Entity(id);

        protected Combat_Action_Resolver Resolver { get; set; }
        internal void Bind_To_Resolver(Combat_Action_Resolver resolver, Combat_GameState combat)
        {
            Resolver = resolver;
            Combat = combat;
        }

        protected GameEntity_RosterEntry Get_Owner_Entity_Of_Action(Combat_Action action)
            => Get_Owner_Entity_Of_Action(action.Action_Owner);

        protected GameEntity_Ability Get_Ability(Combat_Action action)
            => Get_Owner_Entity_Of_Action(action).Entity.Ability_Manager.Get_Ability<GameEntity_Ability>(action.Selected_Ability);
        
        internal void Begin_Stage(Combat_Action action)
        {
            Handle_Stage(action);
        }
        protected abstract void Handle_Stage(Combat_Action action);
    }
}
