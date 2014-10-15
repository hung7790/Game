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
    class bulletItem
    {
        public bool used = false;
        public int bullectItemId { get; set; }
        public Matrix bulletItemMatrix { get; set; }
        public Matrix[] bulletTransforms;
        public BoundingSphere bulletBounding;
        public Model bullet;
        //public Vector3 shootDirection;
        public float shootAngle = 0;
        public bool shooting = false;
        public int time = 0;
        public float traveled = 0;
        public void shoot()
        {

            //   shootDirection =  new Vector3(player.translationX, player.translationY, player.translationZ);
            //if (Vector3.Zero != shootDirection)
            //   shootDirection.Normalize();
               shootAngle = player.angle;
               bulletItemMatrix = Matrix.CreateRotationY(shootAngle) * Matrix.CreateTranslation(new Vector3(player.translationX, 0, player.translationZ));
               traveled = 0;
               shooting = true;
          
        }
        public void shootingBullet(float elaspedTime)
        {
            if(shooting == true)
            {
                if (traveled < 2000)
                {
                    float newX = bulletItemMatrix.Translation.X - ((float)Math.Sin(shootAngle)) * GlobalObject.bulletSpeed * elaspedTime;
                    float newZ = bulletItemMatrix.Translation.Z - ((float)Math.Cos(shootAngle)) * GlobalObject.bulletSpeed * elaspedTime;
                    bulletItemMatrix = Matrix.CreateRotationY(shootAngle) * Matrix.CreateTranslation(new Vector3(newX, 0, newZ));
                    traveled += GlobalObject.bulletSpeed * elaspedTime;
                }
                else shooting = false;
            }

            }
    }
}
