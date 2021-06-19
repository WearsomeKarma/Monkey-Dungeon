using System.Collections.Generic;
using MonkeyDungeon_Core.GameFeatures.GameComponents.EntityAttributes.Abilities;
using MonkeyDungeon_Vanilla_Domain;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;

namespace MonkeyDungeon_Core.GameFeatures.GameStates.Combat
{
    public abstract class Combat_Action_Resolution_Stage
    {
        private GameState_Combat GameStateCombat { get; set; }
        protected GameEntity_ServerSide_Roster Entity_Field => GameStateCombat.Game_Field;
        protected GameEntity_ServerSide Get_Entity(GameEntity_ID id)
            => Entity_Field.Get_Entity(id);

        protected GameEntity_ServerSide Get_Entity(GameEntity_Position position)
            => Entity_Field.Get_Entity(position);

        protected GameEntity_ServerSide[] Get_Entities(GameEntity_Position[] positions)
            => Entity_Field.Get_Entities(positions);
        
        protected Combat_Action_Resolver Resolver { get; set; }
        internal void Bind_To_Resolver(Combat_Action_Resolver resolver, GameState_Combat gameStateCombat)
        {
            Resolver = resolver;
            GameStateCombat = gameStateCombat;
        }

        protected GameEntity_ServerSide Get_Entity(GameEntity_ServerSide_Action action)
            => Get_Entity(action.Action__Invoking_Entity);
        
        internal void Begin_Stage(GameEntity_ServerSide_Action action)
        {
            Handle_Stage(action);
        }
        protected abstract void Handle_Stage(GameEntity_ServerSide_Action action);
    }
}
