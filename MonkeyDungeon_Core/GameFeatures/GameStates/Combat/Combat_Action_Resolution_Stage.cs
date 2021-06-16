using System.Collections.Generic;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures.GameStates.Combat
{
    public abstract class Combat_Action_Resolution_Stage
    {
        private Combat_GameState Combat { get; set; }
        protected GameEntity_ServerSide_Roster Entity_Field => Combat.Game_Field;
        protected GameEntity_ServerSide Get_Entity(GameEntity_ID id)
            => Entity_Field.Get_Entity(id);

        protected GameEntity_ServerSide Get_Entity(GameEntity_Position position)
            => Entity_Field.Get_Entity(position);

        protected GameEntity_ServerSide[] Get_Entities(GameEntity_Position[] positions)
            => Entity_Field.Get_Entities(positions);
        
        protected Combat_Action_Resolver Resolver { get; set; }
        internal void Bind_To_Resolver(Combat_Action_Resolver resolver, Combat_GameState combat)
        {
            Resolver = resolver;
            Combat = combat;
        }

        protected GameEntity_ServerSide Get_Entity(Combat_Action action)
            => Get_Entity(action.Action_Owner);
        
        internal void Begin_Stage(Combat_Action action)
        {
            Handle_Stage(action);
        }
        protected abstract void Handle_Stage(Combat_Action action);
    }
}
