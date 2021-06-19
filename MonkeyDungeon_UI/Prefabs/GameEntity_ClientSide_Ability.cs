using System;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.GameStates.Combat;

namespace MonkeyDungeon_UI.Prefabs
{
    public class GameEntity_ClientSide_Ability
    {
        public event Action<GameEntity_ClientSide_Ability> Target_Type_Changed;

        public GameEntity_Attribute_Name_Ability Ability_Name { get; private set; }
        public Combat_Target_Type Target_Type { get; private set; }
        public Combat_Survey_Target SurveyTarget { get; private set; }
        public bool Ability_Usage_Finished => SurveyTarget.Has_Legal_Targets();
        
        internal void Set_Target_Type(Combat_Target_Type targetType)
        {
            Target_Type = targetType;
            SurveyTarget.Target_Type = targetType;
            
            Target_Type_Changed?.Invoke(this);
        }

        internal GameEntity_ClientSide_Ability(GameEntity_Attribute_Name_Ability abilityName)
        {
            Ability_Name = abilityName;
            SurveyTarget = new Combat_Survey_Target();
        }

        public override string ToString()
        {
            return Ability_Name;
        }
    }
}