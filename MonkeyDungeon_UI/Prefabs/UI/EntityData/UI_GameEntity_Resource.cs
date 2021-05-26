using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_UI.Prefabs.UI.EntityData
{
    public class UI_GameEntity_Resource
    {
        public event Action Resource_Removed;

        public event Action<float> Resource_Updated;
        private float resource_Percentage;
        public float Resource_Percentage { get => resource_Percentage;
            set
            {
                resource_Percentage = value;
                Resource_Updated?.Invoke(resource_Percentage);
            }
        }

        public string Resource_Name { get; private set; }

        public UI_GameEntity_Resource(string name, float initalPercentage=1)
        {
            Resource_Percentage = initalPercentage;
            Resource_Name = name;
        }

        internal void Remove_Resource()
        {
            Resource_Removed?.Invoke();
        }
    }
}
