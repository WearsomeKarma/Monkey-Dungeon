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
        protected override void Handle_Stage(GameEntity_ServerSide_Action action)
        {
            GameEntity_ServerSide owner = Get_Entity(action.Action__Invoking_Entity);
            
            Initalize__Redirection_Survey(action);
            Apply__Invoker_Modifiers(action);
            Apply__Targeted_Entity_Modifiers(action);

            Determine__Redirections(action);
            
            Relay_UI_Event(owner, action);
        }

        private void Initalize__Redirection_Survey(GameEntity_ServerSide_Action action)
        {
            GameEntity_ServerSide_Ability ability = action.Action__Selected_Ability;
            GameEntity_Position_Type assaulterPositionType = (GameEntity_Position_Type) Get_Entity(action).GameEntity_Position;
            GameEntity_Position[] targetedPositions = action.Action__Survey_Target.Get__Targeted_Hostile_Positions__Survey_Target(action.Action__Invoking_Entity.Team_Id);

            action.Action__Survey_Redirection.Reset();

            foreach (GameEntity_Position targetedPosition in targetedPositions)
            {
                GameEntity_Position_Type targetPositionType = (GameEntity_Position_Type) targetedPosition;
                
                action.Action__Survey_Redirection[targetedPosition].Combine__Redirection_Chance
                (
                    Combat_Redirection_Chance.Base_Redirection_Chance
                    (
                        ability.Ability__Combat_Assault_Type,
                        assaulterPositionType,
                        targetPositionType
                    )
                );
            }
        }

        private void Apply__Invoker_Modifiers(GameEntity_ServerSide_Action action)
        {
            GameEntity_ServerSide_Ability ability = action.Action__Selected_Ability;
            GameEntity_Position[] targetedPositions =
                action.Action__Survey_Redirection.Get__Positions_Of_Hostiles__Survey_Redirection(action.Action__Invoking_Entity.Team_Id);
            GameEntity_Position_Type ownerPositionType =
                (GameEntity_Position_Type) Get_Entity(action).GameEntity_Position;
            
            foreach (GameEntity_Position targetedPosition in targetedPositions)
            {
                GameEntity_Position_Type targetPositionType = (GameEntity_Position_Type) targetedPosition;

                Combat_Redirection_Chance chance = ability.Calculate__Redirect_Chance__Ability
                (
                    ownerPositionType,
                    targetPositionType,
                    action.Action__Survey_Redirection[targetedPosition]
                );
                
                action.Action__Survey_Redirection[targetedPosition].Combine__Redirection_Chance(chance);
            }
        }

        private void Apply__Targeted_Entity_Modifiers(GameEntity_ServerSide_Action action)
        {
            GameEntity_ServerSide_Ability ability = action.Action__Selected_Ability;
            GameEntity_Position[] targetedPositions =
                action.Action__Survey_Redirection.Get__Positions_Of_Hostiles__Survey_Redirection(action.Action__Invoking_Entity.Team_Id);
            GameEntity_Position_Type assaulterPositionType =
                (GameEntity_Position_Type) Get_Entity(action).GameEntity_Position;

            foreach (GameEntity_Position targetedPosition in targetedPositions)
            {
                GameEntity_Position_Type targetPositionType = (GameEntity_Position_Type) targetedPosition;

                GameEntity_ServerSide targetedEntity = Get_Entity(targetedPosition);

                Combat_Redirection_Chance[] chances = targetedEntity.React_To__Redirect_Chance__GameEntity
                (
                    ability.Ability__Combat_Assault_Type,
                    assaulterPositionType,
                    targetPositionType,
                    action.Action__Survey_Redirection[targetedPosition]
                );

                foreach (Combat_Redirection_Chance chance in chances)
                {
                    action.Action__Survey_Redirection[targetedPosition].Combine__Redirection_Chance(chance);
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
                    action.Action__Survey_Target.Remove_Target(targetedPosition);

                    GameEntity_Position redirectedPosition = Combat_Redirection_Chance.Redirect(targetedPosition,
                        chance.Redirection_Chance__Redirection_Type);

                    action.Action__Survey_Target.Add_Target(redirectedPosition);
                }
            }
        }
        
        private void Relay_UI_Event(GameEntity_ServerSide entityServerSide, GameEntity_ServerSide_Action action)
        {
            GameEntity_ServerSide_Ability ability = action.Action__Selected_Ability;
            Combat_Assault_Type assaultType = ability.Ability__Combat_Assault_Type;

            GameEntity_Position[] positions = action.Action__Survey_Target.Get__Targeted_Positions__Survey_Target();
            GameEntity_ServerSide[] targets = Get_Entities(positions);
            
            //TOOD: fix
            switch (assaultType)
            {
                case Combat_Assault_Type.Melee:
                    Resolver.GameStateCombat.Act_Melee_Attack(entityServerSide.GameEntity_ID, targets[0].GameEntity_ID);
                    break;
                case Combat_Assault_Type.Ranged:
                    Resolver.GameStateCombat.Act_Ranged_Attack(entityServerSide.GameEntity_ID, targets[0].GameEntity_ID, ability.Ability__Particle_Name);
                    break;
            }
        }
    }
}
