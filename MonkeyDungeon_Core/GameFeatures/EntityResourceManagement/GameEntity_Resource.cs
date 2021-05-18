using MonkeyDungeon_Core.GameFeatures.Implemented.GameStates;
using MonkeyDungeon_Core.GameFeatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_Core.GameFeatures.EntityResourceManagement
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

    public class GameEntity_Resource
    {
        public readonly string Resource_Name;

        public event Action<GameEntity_Resource> BaseValueChanged;
        private double baseValue;
        public double Get_BaseValue() { return baseValue; }

        public GameEntity_Resource_ScalingValue Max_Value { get; private set; }
        public GameEntity_Resource_ScalingValue Min_Value { get; private set; }

        public event Action<GameEntity_Resource> ValueChanged;
        /// <summary>
        /// Does not invoke from bonus additions/removals.
        /// </summary>
        public event Action<GameEntity_Resource> ValueIncreased;
        /// <summary>
        /// Does not invoke from bonus additions/removals.
        /// </summary>
        public event Action<GameEntity_Resource> ValueDecreased;
        /// <summary>
        /// Does not invoke from bonus additions/removals.
        /// </summary>
        public event Action<GameEntity_Resource> StrictValueIncreased;
        /// <summary>
        /// Does not invoke from bonus additions/removals.
        /// </summary>
        public event Action<GameEntity_Resource> StrictValueDecreased;

        public double Resource_Value => baseValue + Get_TotalBonus();
        public double Resource_StrictValue => baseValue + Get_TotalStrictBonus();

        public bool TryPay(double amount, bool peek = false)
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

        public GameEntity_Resource_ScalingValue Max_ForReplenish { get; private set; }
        public double Rate_Replenish { get; private set; }

        public event Action<GameEntity_Resource> Depleted;
        public bool IsDepleted => Resource_Value == Min_Value;

        private List<GameEntity_Resource_Bonus> bonuses               = new List<GameEntity_Resource_Bonus>();
        public GameEntity_Resource_Bonus[] Bonuses                    => bonuses.ToArray();
        public void Add_Bonus                                   (GameEntity_Resource_Bonus bonus) { bonuses.Add(bonus); bonus.handle_NewResource(this); }
        public bool Remove_Bonus                                (GameEntity_Resource_Bonus bonus) { bool ret; if (ret = bonuses.Contains(bonus)) { bonuses.Remove(bonus); bonus.handle_LoseResouce(this); } return ret; }
        public double Get_TotalBonus                             () { double ret = 0; foreach(GameEntity_Resource_Bonus bonus in bonuses) ret += bonus.Value; return ret; }
        public double Get_TotalStrictBonus                       () { double ret = 0; foreach (GameEntity_Resource_Bonus bonus in bonuses) { if (bonus.IsStrict) ret += bonus.Value; } return ret; }
        private GameEntity_Resource_Bonus LastBonus                   => (bonuses.Count > 0) ? bonuses[bonuses.Count - 1] : null;

        public GameEntity Entity { get; private set; }

        public event Action<GameEntity_Resource> Enabled;
        public event Action<GameEntity_Resource> Disabled;
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
        
        public GameEntity_Resource(
            string resourceName,
            double baseValue,
            double max,
            double min,
            double replenishRate,
            double maxProgresionRate,
            double minProgresionRate =0
            )
        {
            Resource_Name = resourceName;
            Max_Value = new GameEntity_Resource_ScalingValue(max, 0, maxProgresionRate);
            Min_Value = new GameEntity_Resource_ScalingValue(min, 0, minProgresionRate);
            set_BaseValue(baseValue);
            Rate_Replenish = replenishRate;
            IsEnabled = true;
        }

        internal void Attach_ToEntity(GameEntity entity)
        {
            if (Entity != null)
                Detach_FromEntity();
            Entity = entity;
            Perform_LevelChange();
        }

        internal void Detach_FromEntity()
        {
            Entity = null;
            Perform_LevelChange();
        }

        internal void Perform_LevelChange()
        {
            Handle_LevelChange();
        }

        protected virtual void Handle_LevelChange()
        {
            int? level = Entity?.Level;
            Max_Value.Change_ScalingLevel(level ?? 1);
            set_BaseValue(Max_Value);
        }

        internal double Offset(double offset)
        {
            double ret = offset_Total_ByValue(offset);

            ValueChanged?.Invoke(this);
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

        private double offset_Total_ByValue(double offset)
        {
            bool isOffsetNegative = offset < 0;

            double totalValue = Resource_Value;
            double totalBonus = Get_TotalBonus();

            double sum = offset + totalValue;
            double diffTarget = MathHelper.Clampd(sum, Min_Value, Max_Value);
            double diff = diffTarget - totalValue;

            bool isOverflow = offset + totalValue > Max_Value;
            bool isUnderflow = offset + totalValue < Min_Value;
            
            if (isOverflow)
            {
                set_BaseValue(Max_Value - totalBonus);
                return diff;
            }

            if (isOffsetNegative)
            {
                double bleed = deduct_FromBonuses(offset);
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
        private double deduct_FromBonuses(double value)
        {
            //ignore positive values.
            if (value >= 0)
            {
                //TODO: setup debug stuff.
                Console.WriteLine("[Warning: EntityResource.cs] Improper usage of deduct_FromBonuses.");
                return 0;
            }

            double bleed = value;
            while(bleed < 0 && bonuses.Count > 0)
            {
                bleed = LastBonus.Offset_Bonus(bleed);
                if (bleed < 0)
                    Remove_Bonus(LastBonus);
            }
            
            return MathHelper.ClampMaxd(bleed, 0);
        }

        private double set_BaseValue(double value)
        {
            //parameter constraint:
            value = MathHelper.Clampd(value, Min_Value, Max_Value);


            double dif = value - baseValue;

            baseValue = value;

            return dif;
        }

        internal virtual void Combat_BeginTurn_ReplenishResource(Combat_GameState combat)
        {
            offset_Total_ByValue(Handle_Combat_BeginTurn_ReplenishResource(combat));
        }
        
        protected virtual double Handle_Combat_BeginTurn_ReplenishResource(Combat_GameState combat) { return Rate_Replenish; }
        protected virtual void Handle_Enabled() { }
        protected virtual void Handle_Disabled() { }
        protected virtual void Handle_Depleted() { }

        public virtual GameEntity_Resource Clone()
        {
            GameEntity_Resource clone = new GameEntity_Resource(
                Resource_Name, 
                baseValue, 
                Max_Value, 
                Min_Value, 
                Rate_Replenish, 
                Max_Value.Scaling_Rate,
                Min_Value.Scaling_Rate);
            return clone;
        }
    }
}
