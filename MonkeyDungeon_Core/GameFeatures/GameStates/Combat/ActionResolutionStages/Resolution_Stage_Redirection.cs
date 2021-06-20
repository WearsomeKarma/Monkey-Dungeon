using System;
using System.Collections.Generic;
using System.ComponentModel;
using MonkeyDungeon_Core.GameFeatures.GameComponents.EntityAttributes.Abilities;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;

namespace MonkeyDungeon_Core.GameFeatures.GameStates.Combat.ActionResolutionStages
{
    public class Resolution_Stage_Redirection : Combat_Action_Resolution_Stage
    {
        protected override Combat_Action_Conclusion_Type Handle__Resolve_Action__Resolution_Stage(GameEntity_ServerSide_Action action)
        {
            GameEntity_ServerSide owner = Get__Entity__Resolution_Stage(action.Action__Invoking_Entity);
            
            Initalize__Redirection_Survey(action);
            Apply__Invoker_Modifiers(action);
            Apply__Targeted_Entity_Modifiers(action);

            Determine__Redirections(action);

            if (!action.Has_Targets)
                return Combat_Action_Conclusion_Type.FAIL__INVALID_TARGETS;
            
            Relay_UI_Event(owner, action);

            return Combat_Action_Conclusion_Type.SUCCESS;
        }

        private void Initalize__Redirection_Survey(GameEntity_ServerSide_Action action)
        {
            GameEntity_ServerSide_Ability ability = action.Action__Selected_Ability;
            GameEntity_Position_Type assaulterPositionType = (GameEntity_Position_Type) Get__Entity__Resolution_Stage(action).GameEntity__Position;
            GameEntity_Position[] targetedPositions = action.Action__Survey_Target.Get__Targeted_Hostile_Positions__Survey_Target(action.Action__Invoking_Entity.Team_Id);

            action.Action__Survey_Redirection.Reset();

            foreach (GameEntity_Position targetedPosition in targetedPositions)
            {
                GameEntity_Position_Type targetPositionType = (GameEntity_Position_Type) targetedPosition;

                Combat_Redirection_Chance baseChance = Combat_Redirection_Chance.Base_Redirection_Chance
                (
                    ability.Ability__Combat_Assault_Type,
                    assaulterPositionType,
                    targetPositionType
                );
                
                action.Action__Survey_Redirection[targetedPosition].Modify__By_Quantity__Quantity
                (
                    baseChance,
                    baseChance.MODIFICATION_TYPE
                );
            }
        }

        private void Apply__Invoker_Modifiers(GameEntity_ServerSide_Action action)
        {
            GameEntity_ServerSide_Ability ability = action.Action__Selected_Ability;
            GameEntity_Position[] targetedPositions =
                action.Action__Survey_Redirection.Get__Positions_Of_Hostiles__Survey_Redirection(action.Action__Invoking_Entity.Team_Id);
            GameEntity_Position_Type ownerPositionType =
                (GameEntity_Position_Type) Get__Entity__Resolution_Stage(action).GameEntity__Position;
            
            foreach (GameEntity_Position targetedPosition in targetedPositions)
            {
                GameEntity_Position_Type targetPositionType = (GameEntity_Position_Type) targetedPosition;

                Combat_Redirection_Chance chance = ability.Calculate__Redirect_Chance__Ability
                (
                    ownerPositionType,
                    targetPositionType,
                    action.Action__Survey_Redirection[targetedPosition]
                );
                
                action.Action__Survey_Redirection[targetedPosition].Modify__By_Quantity__Quantity
                    (
                    chance,
                    chance.MODIFICATION_TYPE
                    );
            }
        }

        private void Apply__Targeted_Entity_Modifiers(GameEntity_ServerSide_Action action)
        {
            GameEntity_ServerSide_Ability ability = action.Action__Selected_Ability;
            GameEntity_Position[] targetedPositions =
                action.Action__Survey_Redirection.Get__Positions_Of_Hostiles__Survey_Redirection(action.Action__Invoking_Entity.Team_Id);
            GameEntity_Position_Type assaulterPositionType =
                (GameEntity_Position_Type) Get__Entity__Resolution_Stage(action).GameEntity__Position;

            foreach (GameEntity_Position targetedPosition in targetedPositions)
            {
                GameEntity_Position_Type targetPositionType = (GameEntity_Position_Type) targetedPosition;

                GameEntity_ServerSide targetedEntity = Get__Entity__Resolution_Stage(targetedPosition);

                Combat_Redirection_Chance[] chances = targetedEntity.React_To__Redirect_Chance__GameEntity
                (
                    ability.Ability__Combat_Assault_Type,
                    assaulterPositionType,
                    targetPositionType,
                    action.Action__Survey_Redirection[targetedPosition]
                );

                foreach (Combat_Redirection_Chance chance in chances)
                {
                    action.Action__Survey_Redirection[targetedPosition].Modify__By_Quantity__Quantity(chance, chance.MODIFICATION_TYPE);
                }
            }
        }

        private void Determine__Redirections(GameEntity_ServerSide_Action action)
        {
            GameEntity_Position[] targetedPositions = action.Action__Survey_Redirection.Get__Positions__Survey_Redirection();

            foreach (GameEntity_Position targetedPosition in targetedPositions)
            {
                Combat_Redirection_Chance chance = action.Action__Survey_Redirection[targetedPosition];
                
                if (chance.Determine__If_Redirection_Occurs__Redirection_Chance())
                {
                    GameEntity_Position redirectedPosition = Combat_Redirection_Chance.Redirect(targetedPosition,
                        chance.Redirection_Chance__Redirection_Type);

                    GameEntity_ServerSide redirectedEntity = Get__Entity__Resolution_Stage(redirectedPosition);

                    //Ignore redirections to deceased/missing redirected targets.
                    if (redirectedEntity?.GameEntity__Is_Not_Present ?? false)
                        continue;
                    
                    action.Action__Survey_Target.Remove_Target(targetedPosition);

                    if (redirectedEntity != null)
                        action.Action__Survey_Target.Add_Target(redirectedPosition);
                }
            }
        }
        
        private void Relay_UI_Event(GameEntity_ServerSide entityServerSide, GameEntity_ServerSide_Action action)
        {
            GameEntity_ServerSide_Ability ability = action.Action__Selected_Ability;
            Combat_Assault_Type assaultType = ability.Ability__Combat_Assault_Type;

            GameEntity_Position[] positions = action.Action__Survey_Target.Get__Targeted_Positions__Survey_Target();
            GameEntity_ServerSide[] targets = Get__Entities__Resolution_Stage(positions);
            
            //TOOD: fix
            switch (assaultType)
            {
                case Combat_Assault_Type.Melee:
                    Resolver.GameStateCombat.Relay__Melee_Attack__GameState_Combat(entityServerSide.GameEntity__ID, targets[0].GameEntity__ID);
                    break;
                case Combat_Assault_Type.Ranged:
                    Resolver.GameStateCombat.Relay__Ranged_Attack__GameState_Combat(entityServerSide.GameEntity__ID, targets[0].GameEntity__ID, ability.Ability__Particle_Name);
                    break;
            }
        }
    }
}
