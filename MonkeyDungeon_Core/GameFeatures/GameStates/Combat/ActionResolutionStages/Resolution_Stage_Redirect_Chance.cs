using MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;

namespace MonkeyDungeon_Core.GameFeatures.GameStates.Combat.ActionResolutionStages
{
    public class Resolution_Stage_Redirect_Chance : Combat_Action_Resolution_Stage
    {
        protected override void Handle_Stage(Combat_Action action)
        {
            GameEntity_RosterEntry ownerEntry = Get_Owner_Entity_Of_Action(action.Action_Owner);
            GameEntity owner = ownerEntry.Game_Entity;
            GameEntity_Ability ability = owner.Ability_Manager.Get_Ability<GameEntity_Ability>(action.Selected_Ability);

            Combat_Assault_Type assaultType = ability.Assault_Type;
            Combat_Redirection_Chance abilityChance;
            Combat_Redirection_Chance[] statusEffectChances;

            GameEntity_Position_Type assaulterPositionType, targetPositionType;

            assaulterPositionType = (GameEntity_Position_Type)ownerEntry.World_Position;

            GameEntity_ID[] targets = action.Target.Get_Targets();
            GameEntity_RosterEntry target;
            Combat_Redirection_Chance[] chances = new Combat_Redirection_Chance[targets.Length];

            for (int i = 0; i < targets.Length; i++)
            {
                target = Entity_Field.Get_Entity(targets[i]);
                targetPositionType = (GameEntity_Position_Type) target.World_Position;
                chances[i] = MD_VANILLA_COMBAT.Base_Redirection_Chance(assaultType, assaulterPositionType, targetPositionType);

                abilityChance = ability.Calculate_Redirect_Chance
                (
                    action,
                    assaulterPositionType,
                    targetPositionType,
                    chances[i]
                );

                statusEffectChances = target.Game_Entity.StatusEffect_Manager.React_To_Redirect_Chance
                (
                    action,
                    assaultType,
                    assaulterPositionType,
                    targetPositionType,
                    chances[i]
                );
            }
        }
    }
}
