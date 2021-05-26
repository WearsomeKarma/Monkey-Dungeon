using isometricgame.GameEngine;
using isometricgame.GameEngine.Components.Rendering;
using isometricgame.GameEngine.Scenes;
using isometricgame.GameEngine.Systems.Rendering;
using MonkeyDungeon_UI.Prefabs.UI.EntityData;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_UI.Prefabs.UI
{
    public class ResourceBar : GameObject
    {
        public static Vector4 NO_COLOR = new Vector4(0,0,0,0);

        public float Percentage { get; set; }
        public string Resource_Name { get; set; }

        private Vector4 color;

        private UI_GameEntity_Resource Attached_Resource { get; set; }

        public ResourceBar(SceneLayer sceneLayer, Vector3 position, Vector4 color, string resourceName) 
            : base(sceneLayer, position, "resourceBar")
        {
            this.color = color;
            Percentage = 1;
            Resource_Name = resourceName;
        }

        internal void Attach_To_Resource(UI_GameEntity_Resource resource)
        {
            if (Attached_Resource != null)
                Attached_Resource.Resource_Updated -= Resource_Changed;

            if (resource == null)
            {
                Percentage = 0;
                return;
            }

            Attached_Resource = resource;
            Attached_Resource.Resource_Updated += Resource_Changed;
            Percentage = Attached_Resource.Resource_Percentage;
        }

        private void Resource_Changed(float percentage)
            => Percentage = percentage;

        protected override void HandleDraw(RenderService renderService)
        {
            base.HandleDraw(renderService);
            renderService.SetShader(1);
            renderService.SetUniform1(renderService.GetUniformLocation(1, "scale"), Percentage);
            renderService.SetUniform4(renderService.GetUniformLocation(1, "reColor"), color);
            base.HandleDraw(renderService);
            renderService.SetShader(0);
        }
    }
}
