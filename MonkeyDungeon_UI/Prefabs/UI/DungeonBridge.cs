using isometricgame.GameEngine;
using isometricgame.GameEngine.Rendering;
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
    public class DungeonBridge : GameObject
    {
        static float X_CUTOFF = -300;

        RenderUnit[] bridgePieces = new RenderUnit[5];
        Vector3 hopVector = new Vector3(1430,810,0);
        Vector3 scrollVector;

        int leading_piece_index = 4;
        int trailing_piece_index = 0;
        
        public DungeonBridge(SceneLayer sceneLayer, Vector3 position, float windowWidth, RenderUnit bridgePiece) 
            : base(sceneLayer, position)
        {
            X_CUTOFF = -windowWidth - hopVector.X;
            scrollVector = hopVector.Normalized();

            for (int i = 0; i < bridgePieces.Length; i++)
            {
                bridgePieces[i] = bridgePiece;
                bridgePieces[i].Position = (hopVector * (i - (bridgePieces.Length/2))) + Position;
            }
        }

        protected override void HandleDraw(RenderService renderService)
        {
            int index;
            for(int i=0;i<bridgePieces.Length;i++)
            {
                index = (i + trailing_piece_index) % bridgePieces.Length;
                renderService.DrawSprite(ref bridgePieces[index], bridgePieces[index].X, bridgePieces[index].Y);
            }
        }

        internal void Scroll_Bridge(float distance, float deltaTimeF)
        {
            Vector3 offset = scrollVector * distance * deltaTimeF;

            for (int i = 0; i < bridgePieces.Length; i++)
                bridgePieces[i].Position += offset;

            if (bridgePieces[trailing_piece_index].X < X_CUTOFF)
            {
                Hop_Pieces();
            }
        }

        private void Hop_Pieces()
        {
            bridgePieces[trailing_piece_index].Position = 2 * hopVector + Position;

            leading_piece_index = trailing_piece_index;
            trailing_piece_index = (trailing_piece_index + 1) % bridgePieces.Length;
        }
    }
}
