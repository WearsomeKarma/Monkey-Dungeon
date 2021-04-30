using isometricgame.GameEngine.Systems;
using MonkeyDungeon.Components;
using MonkeyDungeon.GameFeatures.Implemented.GameStates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures
{
    public static class ENTITY_RESOURCES
    {
        // Managed Resources
        public static readonly string HEALTH = "Health";
        public static readonly string STAMINA = "Stamina";
        public static readonly string MANA = "Mana";

        // Unmanaged Resources
        internal static readonly string LEVEL = "Level";
        internal static readonly string ABILITY_POINTPOOL = "Ability_PointPool";
    }

    public class EntityResource
    {
        public readonly string Resource_Name;

        public event Action<EntityResource> BaseValueChanged;
        private float baseValue;
        public float Get_BaseValue() { return baseValue; }

        public ScalingValue Max_Value { get; private set; }
        public ScalingValue Min_Value { get; private set; }

        /// <summary>
        /// Does not invoke from bonus additions/removals.
        /// </summary>
        public event Action<EntityResource> ValueIncreased;
        /// <summary>
        /// Does not invoke from bonus additions/removals.
        /// </summary>
        public event Action<EntityResource> ValueDecreased;
        /// <summary>
        /// Does not invoke from bonus additions/removals.
        /// </summary>
        public event Action<EntityResource> StrictValueIncreased;
        /// <summary>
        /// Does not invoke from bonus additions/removals.
        /// </summary>
        public event Action<EntityResource> StrictValueDecreased;

        public float Resource_Value => baseValue + Get_TotalBonus();
        public float Resource_StrictValue => baseValue + Get_TotalStrictBonus();

        public bool TryPay(float amount, bool peek = false)
        {
            if (amount < 0)
                return false;
            if (Resource_Value - amount >= Min_Value)
            {
                if (!peek)
                    offset_Total_ByValue(-amount);
                return true;
            }
            return false;
        }

        public ScalingValue Max_ForReplenish { get; private set; }
        public float Rate_Replenish { get; private set; }

        public event Action<EntityResource> Depleted;
        public bool IsDepleted => Resource_Value == Min_Value;

        private List<EntityResourceBonus> bonuses               = new List<EntityResourceBonus>();
        public EntityResourceBonus[] Bonuses                    => bonuses.ToArray();
        public void Add_Bonus                                   (EntityResourceBonus bonus) { bonuses.Add(bonus); bonus.handle_NewResource(this); }
        public bool Remove_Bonus                                (EntityResourceBonus bonus) { bool ret; if (ret = bonuses.Contains(bonus)) { bonuses.Remove(bonus); bonus.handle_LoseResouce(this); } return ret; }
        public float Get_TotalBonus                             () { float ret = 0; foreach(EntityResourceBonus bonus in bonuses) ret += bonus.Value; return ret; }
        public float Get_TotalStrictBonus                       () { float ret = 0; foreach (EntityResourceBonus bonus in bonuses) { if (bonus.IsStrict) ret += bonus.Value; } return ret; }
        private EntityResourceBonus LastBonus                   => (bonuses.Count > 0) ? bonuses[bonuses.Count - 1] : null;

        public EntityComponent Entity { get; private set; }

        public event Action<EntityResource> Enabled;
        public event Action<EntityResource> Disabled;
        private bool isEnabled;
        public bool IsEnabled { get => isEnabled; set
            {
                bool invoke = isEnabled != value;
                isEnabled = value;
                if (invoke)
                {
                    if (isEnabled)
                    {
                        Handle_Enabled();
                        Enabled?.Invoke(this);
                    }
                    else
                    {
                        Handle_Disabled();
                        Disabled?.Invoke(this);
                    }
                }
            } }
        
        public EntityResource(
            string resourceName, 
            float baseValue, 
            float max, 
            float min, 
            float replenishRate, 
            float maxProgresionRate, 
            float minProgresionRate=0
            )
        {
            Resource_Name = resourceName;
            Max_Value = new ScalingValue(max, 0, maxProgresionRate);
            Min_Value = new ScalingValue(min, 0, minProgresionRate);
            set_BaseValue(baseValue);
            Rate_Replenish = replenishRate;
            IsEnabled = true;
        }

        internal void Attach_ToEntity(EntityComponent entity)
        {
            if (Entity != null)
                UnattachFromEntity();
            Entity = entity;
            PerformLevelChange();
        }

        internal void UnattachFromEntity()
        {
            Entity = null;
            PerformLevelChange();
        }

        internal void PerformLevelChange()
        {
            HandleLevelChange();
        }

        protected virtual void HandleLevelChange()
        {
            int? level = Entity?.Level;
            Max_Value.ChangeScalingLevel((level != null) ? (int)level : 1);
        }

        internal float Offset(float offset)
        {
            float ret = offset_Total_ByValue(offset);

            if (ret < 0)
            {
                ValueDecreased?.Invoke(this);
            }
            else
            {
                ValueIncreased?.Invoke(this);
            }

            if (IsDepleted)
            {
                Handle_Depleted();
                Depleted?.Invoke(this);
            }
            return ret;
        }

        private float offset_Total_ByValue(float offset)
        {
            bool isOffsetNegative = offset < 0;

            float totalValue = Resource_Value;
            float totalBonus = Get_TotalBonus();

            float sum = offset + totalValue;
            float diffTarget = MathHelper.Clamp(sum, Min_Value, Max_Value);
            float diff = diffTarget - totalValue;

            bool isOverflow = offset + totalValue > Max_Value;
            bool isUnderflow = offset + totalValue < Min_Value;
            
            if (isOverflow)
            {
                set_BaseValue(Max_Value - totalBonus);
                return diff;
            }

            if (isOffsetNegative)
            {
                float bleed = deduct_FromBonuses(offset);
                set_BaseValue(baseValue + bleed);
                return diff;
            }
            set_BaseValue(diffTarget - totalBonus);
            return diff;
        }

        /// <summary>
        /// Returns remaining bleed offset.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private float deduct_FromBonuses(float value)
        {
            //ignore positive values.
            if (value >= 0)
            {
                //TODO: setup debug stuff.
                Console.WriteLine("[Warning: EntityResource.cs] Improper usage of deduct_FromBonuses.");
                return 0;
            }
            
            float bleed = value;
            while(bleed < 0 && bonuses.Count > 0)
            {
                bleed = LastBonus.Offset_Bonus(bleed);
                if (bleed < 0)
                    Remove_Bonus(LastBonus);
            }
            
            return MathHelper.ClampMax(bleed, 0);
        }

        private float set_BaseValue(float value)
        {
            //parameter constraint:
            value = MathHelper.Clamp(value, Min_Value, Max_Value);


            float dif = value - baseValue;

            baseValue = value;

            return dif;
        }

        internal virtual void Combat_BeginTurn_ReplenishResource(Combat_GameState combat)
        {
            baseValue += HandleCombat_BeginTurn_ReplenishResource(combat);
        }
        
        protected virtual float HandleCombat_BeginTurn_ReplenishResource(Combat_GameState combat) { return Rate_Replenish; }
        protected virtual void Handle_Enabled() { }
        protected virtual void Handle_Disabled() { }
        protected virtual void Handle_Depleted() { }

        public virtual EntityResource Clone()
        {
            EntityResource clone = new EntityResource(
                Resource_Name, 
                baseValue, 
                Max_Value, 
                Min_Value, 
                Rate_Replenish, 
                Max_Value.ScalingRate,
                Min_Value.ScalingRate);
            return clone;
        }
    }
}
