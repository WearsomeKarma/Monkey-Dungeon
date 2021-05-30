using MonkeyDungeon_Core.GameFeatures.GameEntities.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MonkeyDungeon_Core.GameFeatures.GameEntities.Abilities
{
    public class GameEntity_Ability_Manager
    {
        public static readonly string NAME_ABILITY_POINTPOOL = "Ability_PointPool";

        private readonly GameEntity Entity;

        internal GameEntity_Resource Ability_PointPool = new GameEntity_Resource(NAME_ABILITY_POINTPOOL, 0, 2);
        public int Ability_PointPool_Count => (int)Ability_PointPool.Value;

        public bool Ability_PointPool_IsDepleted => Ability_PointPool.IsDepleted;

        internal bool TryPay_Ability_PointPool(int amount, bool peek = false) => Ability_PointPool.Try_Offset(amount, peek);

        private List<GameEntity_Ability> Abilities = new List<GameEntity_Ability>();
        public GameEntity_Ability[] Get_Abilities() => Abilities.ToArray();
        public string[] Get_Ability_Names() { string[] abilityNames = new string[Abilities.Count]; for (int i = 0; i < Abilities.Count; i++) { abilityNames[i] = Abilities[i].ATTRIBUTE_NAME; } return abilityNames; }
        public T Get_Ability<T>() where T : GameEntity_Ability { foreach (T ability in Abilities) return ability; return null; }
        public T Get_Ability<T>(string abilityName) where T : GameEntity_Ability { foreach (T ability in Abilities.OfType<T>()) { if (ability.ATTRIBUTE_NAME == abilityName) return ability; } return null; }
        public void Add_Ability(GameEntity_Ability ability) { Abilities.Add(ability); ability.Attach_To_Entity(Entity); }

        public GameEntity_Ability_Manager(GameEntity managingEntity, List<GameEntity_Ability> abilities = null)
        {
            Entity = managingEntity;
            Ability_PointPool.Attach_To_Entity(managingEntity);
            
            if (abilities != null)
                foreach (GameEntity_Ability ability in abilities)
                    Add_Ability(ability);
        }

        public void Combat_BeginTurn()
        {
            Ability_PointPool.Force_Offset(Ability_PointPool.Max_Quantity);
        }

        internal bool CheckIf_CanUseAbilities()
        {
            bool ret = false;
            bool? check;

            foreach (GameEntity_Ability ability in Abilities)
            {
                check = Entity.Resource_Manager.Get_ResourceByType<GameEntity_Resource>(ability.Resource_Name)?.Try_Offset(ability.Cost, true);
                //TODO: implement debugging
                if (check == null)
                    Console.WriteLine("[Warning EntityComponent.cs] Ability bound to entity without usable resource.");
                ret = ret || (check ?? false);
            }

            return ret;
        }
    }
}
