using System;
using System.Collections.Generic;
using System.Linq;
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
    class scoreItem
    {
        public int scoreItemId { get; set; }
        public Matrix scoreItemMatrix { get; set; }
        public Matrix[] scoreTransforms;
        public BoundingSphere scoreBounding;
        public Model score;
        public bool appear = true;
        public float time =  0;

        public scoreItem(int scoreItemId)
        {
            this.scoreItemId = scoreItemId;
            newLocation();
        }
        public void newLocation()
        {
            scoreItemMatrix = Matrix.CreateTranslation(GlobalObject.rng.Next((int)GlobalObject.minX, (int)GlobalObject.maxX + 1), 0, GlobalObject.rng.Next((int)GlobalObject.minX, (int)GlobalObject.maxX + 1));
        }

    }

}
