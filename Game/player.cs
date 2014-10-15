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
    public static class player
    {
        public static float translationX = 0.0f;
        public static float translationY = 0.0f;
        public static float translationZ = 0.0f;
        public static float angle = 0.0f;
        public static float velocity = 0.0f;
        public static Matrix playerMatrix { get; set; }
        public static Matrix[] playerTransforms;
        public static Vector3 playerTempPosition { get; set; }
        public static float playerTempAngle = 0.0f;
        public static BoundingSphere playerBounding;
        public static Model playerModel;
        public static MyKeyboard mykb = new MyKeyboard();
        public static bool boost = false;
        public static void movement(float elaspedTime,List<obstacleItem> obstacleItemList)
        {
            translationX = playerMatrix.Translation.X;
            translationY = playerMatrix.Translation.Y;
            translationZ = playerMatrix.Translation.Z;
                mykb.Update();
            if (mykb.IsKeyDown(Keys.Left))
                angle += GlobalObject.rotation;
            if (mykb.IsKeyDown(Keys.Right))
                angle -= GlobalObject.rotation;
            if (mykb.IsKeyDown(Keys.Space) && GlobalObject.accelerationEnergy > 0)
            {
                boost = true;
                GlobalObject.accelerationEnergy -= elaspedTime * 100;

            }
            else
                boost = false;
            if (mykb.IsKeyDown(Keys.LeftShift) && GlobalObject.slowMotionPower > 0)
            {
                GlobalObject.slowMotionOn = true;
                GlobalObject.slowMotionPower-= elaspedTime * 100;

            }
            else
            {

                GlobalObject.slowMotionOn = false;
            }
            if (GlobalObject.accelerationEnergy < 300 && boost == false)
            {
                GlobalObject.accelerationEnergy += elaspedTime * 30;
            }
            if (GlobalObject.slowMotionPower < 300 && GlobalObject.slowMotionOn == false)
            {
                GlobalObject.slowMotionPower += elaspedTime * 30;
            }
            if (boost == true)
            {
                if (mykb.IsKeyDown(Keys.Up))
                {
                    if (velocity < GlobalObject.velocityMax*1.5)
                        velocity += GlobalObject.acceleration * 2 * elaspedTime;

                }
                if (mykb.IsKeyDown(Keys.Down))
                {

                    if (velocity > GlobalObject.velocityMin*1.25)
                        velocity -= GlobalObject.acceleration / 3 * 2 * elaspedTime;
                }
            }
            else
            {
                if (mykb.IsKeyDown(Keys.Up))
                {
                    if (velocity < GlobalObject.velocityMax)
                        velocity += GlobalObject.acceleration * elaspedTime;

                }
                if (mykb.IsKeyDown(Keys.Down))
                {

                    if (velocity > GlobalObject.velocityMin)
                        velocity -= GlobalObject.acceleration / 1.5f * elaspedTime;
                }
            }
            if (velocity > 0)
            {
                velocity -= GlobalObject.friction * elaspedTime;
                if (velocity < 0)
                    velocity = 0;

            }
            else if (velocity < 0)
            {
                velocity += GlobalObject.friction * elaspedTime;
                if (velocity > 0)
                    velocity = 0;
            }
            if (GlobalObject.slowMotionOn == true)
            {
                velocity = velocity *2 / 3;
            }
            playerTempPosition = new Vector3(translationX+ ((float)Math.Sin(angle)) * velocity, translationY, translationZ+ ((float)Math.Cos(angle)) * velocity);

            translationX -= ((float)Math.Sin(angle)) * velocity;
            translationZ -= ((float)Math.Cos(angle)) * velocity;
            // Matrix a = Matrix.CreateTranslation(translationX, translationY, translationZ);
            // Matrix b = Matrix.CreateRotationY(angle);
          
            if (translationX > GlobalObject.maxX)
                translationX = GlobalObject.maxX;
            else if
                (translationX < GlobalObject.minX)
                translationX = GlobalObject.minX;
            if (translationZ > GlobalObject.maxZ)
                translationZ = GlobalObject.maxZ;
            else if (translationZ < GlobalObject.minZ)
                translationZ = GlobalObject.minZ;
            int count = 0;
            foreach (obstacleItem obstacleItem in obstacleItemList)
            {
                if (Vector3.Distance(obstacleItem.obstacleBounding.Center, new Vector3(translationX, translationY, translationZ)) < obstacleItem.obstacleBounding.Radius+60)
                    count++;
            }
            if (count == 0)
                playerMatrix = Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(translationX, translationY, translationZ);
            else
            {
                GlobalObject.pong.Play();
                //GlobalObject.rotation += (float)Math.PI;
               // angle = angle + (float)Math.PI;
                playerMatrix = Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(playerTempPosition);
                velocity = -velocity*3/4;
               // angle = playerTempAngle;
            }
        }
        public static void cancelMovement()
        {
            playerMatrix = Matrix.CreateRotationY(playerTempAngle) * Matrix.CreateTranslation(playerTempPosition);
        }
    }
  

}

