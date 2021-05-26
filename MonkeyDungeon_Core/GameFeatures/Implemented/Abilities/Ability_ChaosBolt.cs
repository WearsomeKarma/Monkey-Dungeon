using MonkeyDungeon_Core.GameFeatures.CombatObjects;
using MonkeyDungeon_Core.GameFeatures.EntityResourceManagement;
using MonkeyDungeon_Core.GameFeatures.Implemented.CharacterStats;
using MonkeyDungeon_Core.GameFeatures.Implemented.EntityResources;
using MonkeyDungeon_Vanilla_Domain.GameFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures.Implemented.Abilities
{
    public class Ability_ChaosBolt : Ability_Ranged
    {
        private Random rand = new Random();

        public Ability_ChaosBolt() 
            : base(
                  MD_VANILLA_ABILITYNAMES.ABILITY_CHAOS_BOLT, 
                  MD_VANILLA_RESOURCES.RESOURCE_MANA, 
                  MD_VANILLA_STATS.STAT_SMARTYPANTS,
                  MD_VANILLA_PARTICLES.CHAOS_BOLT,
                  DamageType.Magical, 
                  true)
        {
        }

        protected override void Handle_AbilityUsage(Combat_Action combatAction)
        {
            //calculate misfire chance, and if misfired, get new target.
            bool misfire = rand.Next(100) > 75;
            if (misfire)
            {
                bool isEnemySide = combatAction.Target_ID > GameState_Machine.MAX_TEAM_SIZE;
                int teamIndex = combatAction.Target_ID % GameState_Machine.MAX_TEAM_SIZE;
                int newIndex = -1;

                while((newIndex = rand.Next(GameState_Machine.MAX_TEAM_SIZE)) != teamIndex) { }

                combatAction.Target_ID = (isEnemySide ? GameState_Machine.MAX_TEAM_SIZE : 0) + newIndex;
            }

            GameEntity target = Entity.Game.Get_Entity(combatAction.Target_ID);

            target.Damage_This(
                new Combat_Damage(
                    Ability_DamageType,
                    Get_RelevantOutput() * 0.5
                    )
                );

            base.Handle_AbilityUsage(combatAction);
        }

        public override GameEntity_Ability Clone()
        {
            return new Ability_ChaosBolt();
        }
    }
}
