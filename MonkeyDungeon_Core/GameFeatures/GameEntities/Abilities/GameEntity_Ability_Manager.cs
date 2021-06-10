using MonkeyDungeon_Vanilla_Domain;
using System.Collections.Generic;
using System.Linq;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Resources;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities
{
    public class GameEntity_Ability_Manager
    {
        private readonly GameEntity Entity;

        private List<GameEntity_Ability> Abilities = new List<GameEntity_Ability>();
        public GameEntity_Ability[] Get_Abilities() => Abilities.ToArray();
        public GameEntity_Attribute_Name[] Get_Ability_Names() { GameEntity_Attribute_Name[] abilityNames = new GameEntity_Attribute_Name[Abilities.Count]; for (int i = 0; i < Abilities.Count; i++) { abilityNames[i] = Abilities[i].ATTRIBUTE_NAME; } return abilityNames; }
        public T Get_Ability<T>() where T : GameEntity_Ability { foreach (T ability in Abilities) return ability; return null; }
        public T Get_Ability<T>(GameEntity_Attribute_Name abilityName) where T : GameEntity_Ability { foreach (T ability in Abilities.OfType<T>()) { if (ability.ATTRIBUTE_NAME == abilityName) return ability; } return null; }
        public void Add_Ability(GameEntity_Ability ability) { Abilities.Add(ability); ability.Attach_To_Entity(Entity); }

        //TODO: think about doing this differently.
        internal readonly GameEntity_Resource Ability_Point_Pool =
            new GameEntity_Resource(MD_VANILLA_RESOURCES.RESOURCE_ABILITYPOINTS, 0, 2);
        
        public GameEntity_Ability_Manager(GameEntity managingEntity, List<GameEntity_Ability> abilities = null)
        {
            Entity = managingEntity;
            
            if (abilities != null)
                foreach (GameEntity_Ability ability in abilities)
                    Add_Ability(ability);
        }
    }
}
