using isometricgame.GameEngine.Systems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.GameFeatures.EntityResourceManagement
{
    public class GameEntity_Resource_Bonus
    {
        public readonly float InitalValue;

        public double Value { get; private set; }
        public bool IsDepleted => Value <= 0;
        public event Action<GameEntity_Resource_Bonus> Manipulated;

        /// <summary>
        /// If strict, it manipulates logic for determining if a resource is depleted.
        /// You can set this to true in the constructor to use increase the base value
        /// and you can keep this false to represent 'shields.'
        /// </summary>
        public bool IsStrict { get; private set; }

        public GameEntity_Resource AttachedResource { get; private set; }

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
        public GameEntity_Resource_Bonus(float bonus, bool isStrict=false)
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
        internal double Offset_Bonus(double offset)
        {
            double diff = Value + offset;
            double clampDiff = MathHelper.Clampd(diff, 0, InitalValue);

            //constraint
            Value = clampDiff;
            Handle_Manipulated();
            Manipulated?.Invoke(this);

            return diff;
        }

        internal void handle_NewResource(GameEntity_Resource newResource)
        {
            AttachedResource?.Remove_Bonus(this);
            Handle_NewResource(newResource);
        }

        internal void handle_LoseResouce(GameEntity_Resource oldResource)
        {
            Handle_LoseResource(oldResource);
        }
        
        protected virtual void Handle_Manipulated() { }
        protected virtual void Handle_NewResource(GameEntity_Resource newResource) { }
        protected virtual void Handle_LoseResource(GameEntity_Resource oldResource) { }
    }
}
