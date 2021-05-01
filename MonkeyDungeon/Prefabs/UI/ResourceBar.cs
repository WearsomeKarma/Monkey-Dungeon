using isometricgame.GameEngine;
using isometricgame.GameEngine.Components.Rendering;
using isometricgame.GameEngine.Scenes;
using isometricgame.GameEngine.Systems.Rendering;
using MonkeyDungeon.GameFeatures;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.Prefabs.UI
{
    public class ResourceBar : GameObject
    {
        public static Vector4 NO_COLOR = new Vector4(0,0,0,0);

        public double Percentage { get; set; }

        private Vector4 color;

        public ResourceBar(SceneLayer sceneLayer, Vector3 position, Vector4 color) 
            : base(sceneLayer, position, "resourceBar")
        {
            this.color = color;
            Percentage = 1;
        }

        protected override void HandleDraw(RenderService renderService)
        {
            base.HandleDraw(renderService);
            renderService.SetShader(1);
            renderService.SetUniform1(renderService.GetUniformLocation(1, "scale"), (float)Percentage);
            renderService.SetUniform4(renderService.GetUniformLocation(1, "reColor"), color);
            base.HandleDraw(renderService);
            renderService.SetShader(0);
        }
    }
}
