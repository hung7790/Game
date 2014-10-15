using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Game
{
   public class obstacleItem
    {
        public int obstacleItemId { get; set; }
        public Matrix obstacleItemMatrix { get; set; }
        public Matrix[] obstacleTransforms;
        public BoundingSphere obstacleBounding;
        public Model obstacle;

        public obstacleItem(int obstacleItemId)
        {
            this.obstacleItemId = obstacleItemId;
            newLocation();
        }
        public void newLocation()
        {
            obstacleItemMatrix = Matrix.CreateTranslation(GlobalObject.rng.Next((int)GlobalObject.minX, (int)GlobalObject.maxX + 1), 0, GlobalObject.rng.Next((int)GlobalObject.minX, (int)GlobalObject.maxX + 1));
            if (Vector3.Distance(new Vector3(obstacleItemMatrix.Translation.X,obstacleItemMatrix.Translation.Y,obstacleItemMatrix.Translation.Z), new Vector3(0, 0, 0)) <80)
                newLocation();
        }

    }
}
