﻿using MonkeyDungeon_Vanilla_Domain;
using System.Collections.Generic;
using System.Linq;
using MonkeyDungeon_Core.GameFeatures.GameEntities.Resources;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities
{
    public sealed class GameEntity_Ability_Manager
    {
        private readonly GameEntity_ServerSide ATTACHED_ENTITY;

        private readonly List<GameEntity_Ability>  ABILITIES           = new List<GameEntity_Ability>();
        public GameEntity_Ability[]                Get__Abilities      () => ABILITIES.ToArray();
        public GameEntity_Attribute_Name[]         Get__Ability_Names  () { GameEntity_Attribute_Name[] abilityNames = new GameEntity_Attribute_Name[ABILITIES.Count]; for (int i = 0; i < ABILITIES.Count; i++) { abilityNames[i] = ABILITIES[i].Attribute_Name; } return abilityNames; }
        public T                                   Get__Ability<T>     () where T : GameEntity_Ability { foreach (T ability in ABILITIES) return ability; return null; }
        public T                                   Get__Ability<T>     (GameEntity_Attribute_Name abilityName) where T : GameEntity_Ability { foreach (T ability in ABILITIES.OfType<T>()) { if (ability.Attribute_Name == abilityName) return ability; } return null; }
        public void                                Add__Ability        (GameEntity_Ability ability) { ABILITIES.Add(ability); ability.Attach_To_Entity(ATTACHED_ENTITY); }

        //TODO: think about doing this differently.
        internal readonly GameEntity_Resource Ability_Point_Pool =
            new GameEntity_Resource(MD_VANILLA_RESOURCE_NAMES.RESOURCE_ABILITYPOINTS, 0, 2);
        
        internal GameEntity_Ability_Manager(GameEntity_ServerSide managingAttachedEntity, List<GameEntity_Ability> abilities = null)
        {
            ATTACHED_ENTITY = managingAttachedEntity;
            
            if (abilities != null)
                foreach (GameEntity_Ability ability in abilities)
                    Add__Ability(ability);
        }
    }
}
