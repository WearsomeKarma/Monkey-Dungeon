using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_UI.Prefabs.UI
{
    public class UI_GameEntity_Descriptor
    {
        public readonly string RACE;
        public uint UNIQUE_IDENTIFIER { get; internal set; }
        public string[] Ability_Names { get; private set; }

        public float Percentage_Health { get; private set; }
        public float Percentage_Stamina { get; private set; }
        public float Percentage_Mana { get; private set; }

        public event Action Resources_Updated;

        public readonly int SCENE_ID;
        public readonly int INITATIVE_ORDER;

        public UI_GameEntity_Descriptor(string race, float pHealth, float pStamina=1, float pMana=1, uint uid=0, string[] abilityNames=null)
        {
            RACE = race;
            UNIQUE_IDENTIFIER = uid;
            abilityNames = abilityNames ?? new string[] { };

            Percentage_Health = pHealth;
            Percentage_Stamina = pStamina;
            Percentage_Mana = pMana;
        }

        internal void Set_Abilities(string[] abilities)
        {
            Ability_Names = abilities;
        }

        internal void Set_Resource_Percentage(string resourceName, float percentage)
        {
            //TODO: Make for any resource.
            switch(resourceName)
            {
                case "Health":
                    Percentage_Health = percentage;
                    break;
                case "Stamina":
                    Percentage_Stamina = percentage;
                    break;
                case "Mana":
                    Percentage_Mana = percentage;
                    break;
                default:
                    break;
            }
            Resources_Updated?.Invoke();
        }
    }
}
