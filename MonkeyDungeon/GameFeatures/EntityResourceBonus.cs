﻿using isometricgame.GameEngine.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures
{
    public class EntityResourceBonus
    {
        public readonly float InitalValue;

        public float Value { get; private set; }
        public bool IsDepleted => Value <= 0;
        public event Action<EntityResourceBonus> Manipulated;

        /// <summary>
        /// If strict, it manipulates logic for determining if a resource is depleted.
        /// You can set this to true in the constructor to use increase the base value
        /// and you can keep this false to represent 'shields.'
        /// </summary>
        public bool IsStrict { get; private set; }

        public EntityResource AttachedResource { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bonus"></param>
        /// <param name="isStrict">
        /// Example: if HP is 0, this bonus prevents death
        /// if (dictates_MinLogic == true) 
        /// Use this to differentiate from a shield verses
        /// bonus max health.
        /// </param>
        public EntityResourceBonus(float bonus, bool isStrict=false)
        {
            InitalValue = bonus;
            Value = bonus;
            IsStrict = isStrict;
        }

        /// <summary>
        /// Returns any offset bleed.
        /// </summary>
        /// <param name="offset"></param>
        /// <returns></returns>
        internal float Offset_Bonus(float offset)
        {
            float diff = Value + offset;
            float clampDiff = MathHelper.Clamp(diff, 0, InitalValue);

            //constraint
            Value = clampDiff;
            Handle_Manipulated();
            Manipulated?.Invoke(this);

            return diff;
        }

        internal void handle_NewResource(EntityResource newResource)
        {
            AttachedResource?.Remove_Bonus(this);
            Handle_NewResource(newResource);
        }

        internal void handle_LoseResouce(EntityResource oldResource)
        {
            Handle_LoseResource(oldResource);
        }
        
        protected virtual void Handle_Manipulated() { }
        protected virtual void Handle_NewResource(EntityResource newResource) { }
        protected virtual void Handle_LoseResource(EntityResource oldResource) { }
    }
}
