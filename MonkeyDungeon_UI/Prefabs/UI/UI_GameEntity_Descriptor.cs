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
        public readonly uint UNIQUE_IDENTIFIER = 0;
        private readonly string[] abilityNames;
        public string[] ABILITY_NAMES => abilityNames.ToArray();

        public float Percentage_Health { get; private set; }
        public float Percentage_Stamina { get; private set; }
        public float Percentage_Mana { get; private set; }

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
        }
    }
}
