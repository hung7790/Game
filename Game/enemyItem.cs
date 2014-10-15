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
    class enemyItem
    {
        public int enemyItemId { get; set; }
        public Matrix enemyItemMatrix { get; set; }
        public Matrix[] enemyTransforms;
        public BoundingSphere enemyBounding;
        public Model enemy;
        public bool appear = true;
        public float time = 0;
        public Vector3 location;
        public float health = GlobalObject.enemyHealth+ GlobalObject.level; 
        public enemyItem(int enemyItemId)
        {
            this.enemyItemId = enemyItemId;
            //location = new Vector3((enemyItemId + 1) * 50 + GlobalObject.rng.Next(0, 20), 0, (enemyItemId + 1) * 50 + GlobalObject.rng.Next(0, 20));
            //enemyItemMatrix = Matrix.CreateTranslation(location);
            newLocation();


        }
        public void newLocation()
        {
            health = GlobalObject.enemyHealth;
            location = new Vector3(GlobalObject.rng.Next((int)GlobalObject.minX, (int)GlobalObject.maxX + 1), 0, GlobalObject.rng.Next((int)GlobalObject.minX, (int)GlobalObject.maxX + 1));
            enemyItemMatrix = Matrix.CreateTranslation(location);
            if (Vector3.Distance(location, new Vector3(player.translationX, player.translationY, player.translationZ)) < GlobalObject.enemyViewDistance)
            {
                newLocation();
            }
            health = GlobalObject.enemyHealth + GlobalObject.level; 
        }
        public float distance()
        {
            Vector3 playerVector = new Vector3(player.translationX, player.translationY, player.translationZ);
            return Vector3.Distance(location, playerVector);

        }
        public void movement(float elapsedTime, List<obstacleItem> obstacleItemList)
        {
            float tempSpeed = GlobalObject.enemySpeed;
            if (GlobalObject.slowMotionOn == true)
                tempSpeed = tempSpeed / 5;
            if (distance() < GlobalObject.enemyViewDistance && appear == true)
            {

                Vector3 playerVector = new Vector3(player.translationX, player.translationY, player.translationZ);
                Vector3 direction;
                if (health == 1)
                {
                    direction = location - playerVector;
                }
                else
                {
                    direction = playerVector - location;

                }
                direction.Normalize();
                location.X += direction.X * tempSpeed * elapsedTime;
                location.Y += direction.Y * tempSpeed * elapsedTime;
                location.Z += direction.Z * tempSpeed * elapsedTime;
                if (location.X > GlobalObject.maxX)
                    location.X = GlobalObject.maxX;
                else if
                    (location.X < GlobalObject.minX)
                    location.X = GlobalObject.minX;
                if (location.Z > GlobalObject.maxZ)
                    location.Z = GlobalObject.maxZ;
                else if (location.Z < GlobalObject.minZ)
                    location.Z = GlobalObject.minZ;

                int count = 0;
                foreach (obstacleItem obstacleItem in obstacleItemList)
                {
                    if (Vector3.Distance(obstacleItem.obstacleBounding.Center, location) < obstacleItem.obstacleBounding.Radius + 50)
                        count++;
                }
                if (count == 0)
                {
                    float tempangle = (float)Math.Asin(direction.X);

                       tempangle += (float)Math.PI;
                    if (direction.Z <0 && direction.X >0)
                        tempangle += (float)Math.PI;

                    enemyItemMatrix = Matrix.CreateRotationY(tempangle) * Matrix.CreateTranslation(location);
                }
                else
                {
                    direction = new Vector3(direction.Z, direction.Y, -direction.X);
                    location.X += direction.X * tempSpeed * elapsedTime;
                    location.Y += direction.Y * tempSpeed * elapsedTime;
                    location.Z += direction.Z * tempSpeed * elapsedTime;
                    if (location.X > GlobalObject.maxX)
                        location.X = GlobalObject.maxX;
                    else if (location.X < GlobalObject.minX)
                        location.X = GlobalObject.minX;
                    if (location.Z > GlobalObject.maxZ)
                        location.Z = GlobalObject.maxZ;
                    else if (location.Z < GlobalObject.minZ)
                        location.Z = GlobalObject.minZ;
                    foreach (obstacleItem obstacleItem in obstacleItemList)
                    {
                        if (Vector3.Distance(obstacleItem.obstacleBounding.Center, location) < obstacleItem.obstacleBounding.Radius + 100)
                            count++;
                    }
                    if (count == 0)
                    {
                        float tempangle = (float)Math.Asin(direction.X);

                        tempangle += (float)Math.PI;
                        if (direction.Z < 0 && direction.X > 0)
                            tempangle += (float)Math.PI;

                        enemyItemMatrix = Matrix.CreateRotationY(tempangle) * Matrix.CreateTranslation(location);
                    }
                    else
                    {
                        direction = new Vector3(-direction.Z, direction.Y, direction.X);
                        location.X += direction.X * tempSpeed * elapsedTime;
                        location.Y += direction.Y * tempSpeed * elapsedTime;
                        location.Z += direction.Z * tempSpeed * elapsedTime;
                        if (location.X > GlobalObject.maxX)
                            location.X = GlobalObject.maxX;
                        else if
            (location.X < GlobalObject.minX)
                            location.X = GlobalObject.minX;
                        if (location.Z > GlobalObject.maxZ)
                            location.Z = GlobalObject.maxZ;
                        else if (location.Z < GlobalObject.minZ)
                            location.Z = GlobalObject.minZ;
                        float tempangle = (float)Math.Asin(direction.X);

                        tempangle += (float)Math.PI;
                        if (direction.Z < 0 && direction.X > 0)
                            tempangle += (float)Math.PI;

                        enemyItemMatrix = Matrix.CreateRotationY(tempangle) * Matrix.CreateTranslation(location);
                    }
                }
            }
        }
    }
}
