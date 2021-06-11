using System.CodeDom;
using System.Linq;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames;
using MonkeyDungeon_Vanilla_Domain.GameFeatures.AttributeNames.Definitions;

namespace MonkeyDungeon_Vanilla_Domain.GameFeatures
{
    public class GameEntity_Attribute_Name
    {
        public static GameEntity_Attribute_Name NULL_ATTRIBUTE_NAME = new GameEntity_Attribute_Name("");

        public readonly string NAME;

        internal GameEntity_Attribute_Name(string name)
        {
            NAME = name;
        }

        public override string ToString()
            => NAME;

        public static implicit operator string(GameEntity_Attribute_Name gameEntity_AttributeName)
            => gameEntity_AttributeName.NAME ?? NULL_ATTRIBUTE_NAME;

        public static T Cast<T>(GameEntity_Attribute_Name attributeName, GameEntity_Attribute_Type type)
            where T : GameEntity_Attribute_Name
        {
            switch (type)
            {
                case GameEntity_Attribute_Type.RACE_NAMES:
                    if (IsNotOfType<T, GameEntity_Attribute_Name_Race>())
                        break;
                    return MD_VANILLA_RACE_NAMES.STRINGS[Index_Of(MD_VANILLA_RACE_NAMES.STRINGS, attributeName)] as T;
                case GameEntity_Attribute_Type.STAT_NAMES:
                    if (IsNotOfType<T, GameEntity_Attribute_Name_Stat>())
                        break;
                    return MD_VANILLA_STAT_NAMES.STRINGS[Index_Of(MD_VANILLA_STAT_NAMES.STRINGS, attributeName)] as T;
                case GameEntity_Attribute_Type.ABILITY_NAMES:
                    if (IsNotOfType<T, GameEntity_Attribute_Name_Ability>())
                        break;
                    return MD_VANILLA_ABILITY_NAMES.STRINGS[Index_Of(MD_VANILLA_ABILITY_NAMES.STRINGS, attributeName)]
                        as T;
                case GameEntity_Attribute_Type.UI_EVENT_NAMES:
                    if (IsNotOfType<T, GameEntity_Attribute_Name_UI_Event>())
                        break;
                    return MD_VANILLA_UI_EVENT_NAMES.STRINGS[Index_Of(MD_VANILLA_UI_EVENT_NAMES.STRINGS, attributeName)] 
                        as T;
                case GameEntity_Attribute_Type.PARTICLE_NAMES:
                    if (IsNotOfType<T, GameEntity_Attribute_Name_Particle>())
                        break;
                    return MD_VANILLA_PARTICLE_NAMES.STRINGS[Index_Of(MD_VANILLA_PARTICLE_NAMES.STRINGS, attributeName)]
                        as T;
                case GameEntity_Attribute_Type.RESOURCE_NAMES:
                    if (IsNotOfType<T, GameEntity_Attribute_Name_Resource>())
                        break;
                    return MD_VANILLA_RESOURCE_NAMES.STRINGS[Index_Of(MD_VANILLA_RESOURCE_NAMES.STRINGS, attributeName)]
                        as T;
            }

            return null;
        }

        private static int Index_Of<T>(T[] strings, string compiledName)
            where T : GameEntity_Attribute_Name
        {
            return strings.ToList().FindIndex((s) => s.NAME == compiledName);
        }

        private static bool IsOfType<T, Y>() where T : GameEntity_Attribute_Name where Y : GameEntity_Attribute_Name
            => typeof(T) == typeof(Y);

        private static bool IsNotOfType<T, Y>() where T : GameEntity_Attribute_Name where Y : GameEntity_Attribute_Name
            => !IsOfType<T, Y>();
    }
}
