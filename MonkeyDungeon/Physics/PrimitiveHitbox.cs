using isometricgame.GameEngine;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonkeyDungeon.Physics
{
    public class PrimitiveHitbox : GameComponent
    {
        public Vector2 Size { get; private set; }
        //0: TL, 1: TR, 2: BL, 3: BR
        Vector3[] vertices;
        
        public PrimitiveHitbox(Vector2 size)
        {
            Size = size;
            vertices = new Vector3[]
            {
                    new Vector3(0,-size.Y,0),
                    new Vector3(size.X,-size.Y,0),
                    new Vector3(0,0,0),
                    new Vector3(size.X,0,0)
            };
        }

        public bool WithinHitBox(Vector3 boxPosition, Vector3 subjectPosition)
        {
            Vector3[] offset = new Vector3[vertices.Length];
            for (int i = 0; i < offset.Length; i++)
                offset[i] = vertices[i] + (boxPosition * new Vector3(1,-1,1));

            return 
                (
                offset[0].Y < subjectPosition.Y &&
                offset[1].Y < subjectPosition.Y &&
                offset[2].Y > subjectPosition.Y &&
                offset[3].Y > subjectPosition.Y &&
                offset[0].X < subjectPosition.X &&
                offset[1].X > subjectPosition.X &&
                offset[2].X < subjectPosition.X &&
                offset[3].X > subjectPosition.X
                );
        }
    }
}
