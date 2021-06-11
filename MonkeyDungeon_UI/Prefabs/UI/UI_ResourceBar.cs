using isometricgame.GameEngine;
using isometricgame.GameEngine.Components.Rendering;
using isometricgame.GameEngine.Scenes;
using isometricgame.GameEngine.Systems.Rendering;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon_UI.Prefabs.UI
{
    public class UI_ResourceBar : GameObject
    {
        public static Vector4 NO_COLOR = new Vector4(0,0,0,0);

        public float Percentage { get; set; }
        public string Resource_Name { get; set; }

        private Vector4 color;

        private GameEntity_ClientSide_Resource Attached_ClientSideResource { get; set; }

        public UI_ResourceBar(SceneLayer sceneLayer, Vector3 position, Vector4 color, string resourceName) 
            : base(sceneLayer, position, "resourceBar")
        {
            this.color = color;
            Percentage = 1;
            Resource_Name = resourceName;
        }

        internal void Attach_To_Resource(GameEntity_ClientSide_Resource clientSideResource)
        {
            if (Attached_ClientSideResource != null)
                Attached_ClientSideResource.Resource_Updated -= Resource_Changed;

            if (clientSideResource == null)
            {
                Percentage = 0;
                return;
            }

            Attached_ClientSideResource = clientSideResource;
            Attached_ClientSideResource.Resource_Updated += Resource_Changed;
            Percentage = Attached_ClientSideResource.Resource_Percentage;
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
