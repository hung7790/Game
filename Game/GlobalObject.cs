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
    public static class GlobalObject
    {
        public static Random rng = new Random();
        public static float timeleft = 50.0f;
        public static float speed = 10f;
        public static float rotation = 0.05f;
        public static float friction = 8f;
        public static float acceleration = 20f;
        public static float accelerationEnergy = 300;
        public static float slowMotionPower = 300;
        public static bool slowMotionOn = false;
        public static float velocityMax = 20f;
        public static float velocityMin = -15f;
        public static float enemySpeed = 500f;
        public static int totalBullet = 15;
        public static int bullectLeft = totalBullet;
        public static float bulletShootInterval = 0.25f;
        public static float bulletSpeed = 2000f;
        public static float usedBulletTime = -1;
        public static float nextBulletRegenTime = 9999;
        public static float bulletRegenTime = 5.0f;
        public static float totalEnemy = 5;
        public static float totalScore = 10;
        public static float enemyHealth = 2;
        public static float enemyViewDistance = 500;
        public static float maxX = 1000;
        public static float minX = -1000;
        public static float maxZ = 1000;
        public static float minZ = -1000;
        public static float totalObstacle = 10;
        public static int miniMapWidthPosition = 650;
        public static int miniMapHeightPosition = 70;
        public static int miniMapSize = 100;
        public static int mapItemPixalSize = 4;
        public static Matrix matrix0 = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4.0f, 800.0f / 600.0f, 1.0f, 1000.0f);

   //     public static Matrix matrix1 = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 2.0f, 800.0f / 600.0f, 1.0f, 2000.0f);

        public static Matrix matrix1 = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4.0f, 1600.0f / 900.0f, 1.0f, 2000.0f);
        public static Matrix matrix2 = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 1.5f, 800.0f / 600.0f, 1.0f, 4000.0f);
        public static SoundEffect pong;
        public static float timeUpdate = 60f;
        public static int level;
    }
}
