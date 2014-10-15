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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        Random rng = new Random();
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        List<scoreItem> scoreItemList = new List<scoreItem>();
        List<enemyItem> enemyItemList = new List<enemyItem>();
        List<bulletItem> bulletItemList = new List<bulletItem>();
        List<obstacleItem> obstacleItemList = new List<obstacleItem>();
        float timeleft = 50.0f;
        int score = 0;
        bool gameEnd = false;
        Texture2D acceratorBar;
        SpriteFont Font;
        MyKeyboard kb;
        Effect effect;
        TextureCube mapEnv;
        Model SkySphere;
        Effect SkySphereEffect;
        TextureCube SkyboxTexture;
        bool gameStarted = false;
        int cameraSetting = 1;

        MyMouse myms;
        float viewAngle = 0.0f;
        float viewRotation = 1.0f;
        Song song;
        SoundEffect pong;
        bool changeView = false;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";




        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //     player.playerModel = Content.Load<Model>("helic");
            player.playerModel = Content.Load<Model>("SpaceShip");
            song = Content.Load<Song>("1056568_SOUNDDOGS__fi");
            pong = Content.Load<SoundEffect>("pong");
            GlobalObject.pong = pong;
            MediaPlayer.Play(song);
            myms = new MyMouse();
            MediaPlayer.IsRepeating = true;

            for (int i = 0; i < GlobalObject.totalObstacle; i++)
            {
                obstacleItemList.Add(new obstacleItem(i));
            }
            for (int i = 0; i < obstacleItemList.Count(); i++)
            {
                obstacleItemList[i].obstacle = Content.Load<Model>("Column_Made_By_Tyro_Smith");
            }


            for (int i = 0; i < GlobalObject.totalScore; i++)
            {
                scoreItemList.Add(new scoreItem(i));
            }
            for (int i = 0; i < scoreItemList.Count(); i++)
            {
                scoreItemList[i].score = Content.Load<Model>("score");
            }
            for (int i = 0; i < GlobalObject.totalEnemy; i++)
            {
                enemyItemList.Add(new enemyItem(i));
            }
            for (int i = 0; i < enemyItemList.Count(); i++)
            {
                enemyItemList[i].enemy = Content.Load<Model>("player");
            }
            for (int i = 0; i < GlobalObject.totalBullet; i++)
            {
                bulletItemList.Add(new bulletItem());
            }
            for (int i = 0; i < GlobalObject.totalBullet; i++)
            {
                bulletItemList[i].bullet = Content.Load<Model>("bullet");
            }
            effect = Content.Load<Effect>("CubeMapEffect");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Font = Content.Load<SpriteFont>("Fuente");
            kb = new MyKeyboard();

            mapEnv = TextureCube.FromFile(graphics.GraphicsDevice, "Content/CubeMap.dds");
            SkySphereEffect = Content.Load<Effect>("SkySphere");
            SkyboxTexture = TextureCube.FromFile(graphics.GraphicsDevice, "Content/CubeMap.dds");
            SkySphere = Content.Load<Model>("SphereHighPoly");

            //audioEngine = new AudioEngine("Content/sound.xgs");
            //soundBank = new SoundBank(audioEngine, "Content/Sound Bank.xsb");
            //waveBank = new WaveBank(audioEngine, "Content/Wave Bank.xwb");
            //bgMusic = soundBank.GetCue("bgWav");
            //hornMusic = soundBank.GetCue("horn");

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            acceratorBar = new Texture2D(GraphicsDevice, 1, 1);
            acceratorBar.SetData(new[] { Color.White });
            Font = Content.Load<SpriteFont>("Fuente");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            GlobalObject.level = Convert.ToInt32(gameTime.TotalGameTime.Seconds / GlobalObject.timeUpdate);
            float elapsedTime = gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
            kb.Update();

            if (kb.IsKeyHit(Keys.Escape))
                this.Exit();
            if (gameStarted == false)
            {
                if (kb.IsKeyDown(Keys.Enter))
                {
                    gameStarted = true;
                }
            }
            else
            {
                if (gameEnd == false)
                    if (kb.IsKeyDown(Keys.A))
                    {
                        foreach (bulletItem bulletItem in bulletItemList)
                        {
                            if (bulletItem.used == false && GlobalObject.usedBulletTime + GlobalObject.bulletShootInterval < gameTime.TotalGameTime.TotalMilliseconds / 1000f)
                            {
                                if (GlobalObject.bullectLeft == GlobalObject.totalBullet)
                                {
                                    GlobalObject.nextBulletRegenTime = (float)gameTime.TotalGameTime.TotalMilliseconds / 1000f + GlobalObject.bulletRegenTime;
                                }
                                GlobalObject.usedBulletTime = (float)gameTime.TotalGameTime.TotalMilliseconds / 1000f;
                                bulletItem.used = true;
                                GlobalObject.bullectLeft -= 1;
                                bulletItem.shoot();
                                break;
                            }
                        }
                    }
                Vector2 vecMoved;
                myms.Update();
                vecMoved = myms.MouseMoved();

                if (kb.IsKeyHit(Keys.V))
                {
                    if (changeView == false)
                        changeView = true;
                    else
                        changeView = false;
                }
                if (changeView == true)
                {
                    if (vecMoved.X != 0)
                        viewAngle += vecMoved.X * viewRotation * elapsedTime;

                    if (viewAngle > 2 * Math.PI)
                        viewAngle -= 2 * (float)Math.PI;
                    if (viewAngle < 0.0f)
                        viewAngle += 2 * (float)Math.PI;

                }
                if (kb.IsKeyDown(Keys.NumPad1))
                    cameraSetting = 1;
                if (kb.IsKeyDown(Keys.NumPad2))
                    cameraSetting = 2;

                if (kb.IsKeyDown(Keys.NumPad3))
                    cameraSetting = 3;

                foreach (bulletItem bulletItem in bulletItemList)
                    bulletItem.shootingBullet(elapsedTime);

                if (GlobalObject.bullectLeft < GlobalObject.totalBullet & GlobalObject.nextBulletRegenTime < gameTime.TotalGameTime.TotalMilliseconds / 1000f)
                {
                    GlobalObject.bullectLeft++;
                    foreach (bulletItem bulletItem in bulletItemList)
                    {
                        if (bulletItem.used == true)
                        {
                            bulletItem.used = false;
                            break;
                        }
                    }
                    if (GlobalObject.bullectLeft != GlobalObject.totalBullet)
                    {
                        GlobalObject.nextBulletRegenTime = (float)gameTime.TotalGameTime.TotalMilliseconds / 1000f + GlobalObject.bulletRegenTime;
                    }
                    else
                    {
                        GlobalObject.nextBulletRegenTime = 9999;
                    }
                }
                if (gameEnd == false)
                {
                    timeleft -= gameTime.ElapsedGameTime.Milliseconds / 1000.0f;
                    player.movement(elapsedTime, obstacleItemList);
                    // player.checkCollision(obstacleItemList, elapsedTime);
                    if (timeleft <= 0)
                    {
                        gameEnd = true;
                    }
                    foreach (obstacleItem obstacleItem in obstacleItemList)
                        if (player.playerBounding.Intersects(obstacleItem.obstacleBounding))
                        {
                            obstacleItem.newLocation();
                        }

                    foreach (scoreItem scoreItem in scoreItemList)
                        if (player.playerBounding.Intersects(scoreItem.scoreBounding) && scoreItem.appear == true)
                        {
                            scoreItem.appear = false;
                            if (GlobalObject.level < 6)
                                timeleft += 10 - GlobalObject.level;
                            else
                                timeleft += 5;
                            score += 10;
                            scoreItem.time = (float)gameTime.TotalGameTime.TotalMilliseconds / 1000f + 10;
                        }
                    foreach (enemyItem enemyItem in enemyItemList)
                        if (player.playerBounding.Intersects(enemyItem.enemyBounding) && enemyItem.appear == true)
                        {
                            enemyItem.appear = false;
                            timeleft -= 5;
                            if (timeleft < 0)
                            {
                                timeleft = 0;
                            }
                            score -= 5;
                            enemyItem.time = (float)gameTime.TotalGameTime.TotalMilliseconds / 1000f + 10;
                        }

                    foreach (bulletItem bulletItem in bulletItemList)
                    {
                        foreach (enemyItem enemyItem in enemyItemList)
                        {
                            if (bulletItem.bulletBounding.Intersects(enemyItem.enemyBounding) && enemyItem.appear == true && bulletItem.shooting == true)
                            {
                                bulletItem.shooting = false;
                                enemyItem.health--;
                                if (enemyItem.health == 0)
                                {
                                    enemyItem.appear = false;
                                    score += 10;
                                }
                            }
                        }
                    }

                    for (int i = 0; i < scoreItemList.Count(); i++)
                        if (scoreItemList[i].appear == false && scoreItemList[i].time < gameTime.TotalGameTime.TotalMilliseconds / 1000f)
                        {
                            scoreItemList[i].appear = true;
                            scoreItemList[i].newLocation();
                        }
                    for (int i = 0; i < enemyItemList.Count(); i++)
                        if (enemyItemList[i].appear == false && enemyItemList[i].time < gameTime.TotalGameTime.TotalMilliseconds / 1000f)
                        {
                            enemyItemList[i].appear = true;
                            enemyItemList[i].newLocation();
                        }
                    foreach (obstacleItem obstacleItem in obstacleItemList)
                        foreach (scoreItem scoreItem in scoreItemList)
                            if (scoreItem.scoreBounding.Intersects(obstacleItem.obstacleBounding) && scoreItem.appear == true)
                            {
                                scoreItem.newLocation();
                            }

                    //foreach (obstacleItem obstacleItem in obstacleItemList)
                    //    foreach (enemyItem enemyItem in enemyItemList)
                    //        if (enemyItem.enemyBounding.Intersects(obstacleItem.obstacleBounding) && enemyItem.appear == true)
                    //        {
                    //            enemyItem.newLocation();
                    //        }
                    for (int i = 0; i < enemyItemList.Count(); i++)
                    {
                        enemyItemList[i].movement(elapsedTime, obstacleItemList);
                    }
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Vector3 cameraPosition = new Vector3(player.translationX + (float)Math.Sin(viewAngle) * 400, player.translationY + 400f, player.translationZ + 400f);
            Vector3 cameraUp = Vector3.Up;
            Vector3 cameraLookAt = new Vector3(player.translationX, player.translationY, player.translationZ);
            //  Vector3 cameraLookAt = Vector3.Zero;
            // Matrix playerTranslation = Matrix.CreateTranslation(0, 0, 0);

            player.playerTransforms = new Matrix[player.playerModel.Bones.Count];

            foreach (scoreItem scoreItem in scoreItemList)
                scoreItem.scoreTransforms = new Matrix[scoreItem.score.Bones.Count];
            foreach (enemyItem enemyItem in enemyItemList)
                enemyItem.enemyTransforms = new Matrix[enemyItem.enemy.Bones.Count];
            foreach (obstacleItem obstacleItem in obstacleItemList)
                obstacleItem.obstacleTransforms = new Matrix[obstacleItem.obstacle.Bones.Count];

            foreach (bulletItem bulletItem in bulletItemList)
                bulletItem.bulletTransforms = new Matrix[bulletItem.bullet.Bones.Count];


            Matrix projection = Matrix.CreatePerspectiveFieldOfView((float)Math.PI / 4.0f, 800.0f / 600.0f, 1.0f, 1000.0f);
            switch (cameraSetting)
            {
                case 1:
                    projection = GlobalObject.matrix0;
                    break;
                case 2:
                    projection = GlobalObject.matrix1;
                    break;
                case 3:
                    projection = GlobalObject.matrix2;
                    break;
            }
            Matrix view = Matrix.CreateLookAt(cameraPosition, cameraLookAt, cameraUp);
            player.playerModel.CopyAbsoluteBoneTransformsTo(player.playerTransforms);
            foreach (scoreItem scoreItem in scoreItemList)
                scoreItem.score.CopyAbsoluteBoneTransformsTo(scoreItem.scoreTransforms);
            foreach (enemyItem enemyItem in enemyItemList)
                enemyItem.enemy.CopyAbsoluteBoneTransformsTo(enemyItem.enemyTransforms);
            foreach (obstacleItem obstacleItem in obstacleItemList)
                obstacleItem.obstacle.CopyAbsoluteBoneTransformsTo(obstacleItem.obstacleTransforms);

            foreach (bulletItem bulletItem in bulletItemList)
                bulletItem.bullet.CopyAbsoluteBoneTransformsTo(bulletItem.bulletTransforms);


            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            //foreach (ModelMesh mesh in SkySphere.Meshes)
            //{
            //    foreach (ModelMeshPart part in mesh.MeshParts)
            //    {
            //        part.Effect = SkySphereEffect;
            //    }
            //    mesh.Draw();
            //}

            foreach (ModelMesh mesh in player.playerModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = Matrix.CreateScale(10.0f) * player.playerTransforms[mesh.ParentBone.Index] * Matrix.CreateRotationY(MathHelper.Pi / 2.0f) * player.playerMatrix;
                    effect.View = view;
                    effect.Projection = projection;

                    mesh.Draw();
                }
                player.playerBounding = TransformBoundingSphere(mesh.BoundingSphere, Matrix.CreateScale(10.0f) * player.playerMatrix);
            }

            foreach (bulletItem bulletItem in bulletItemList)
                if (bulletItem.shooting == true)
                    foreach (ModelMesh mesh in bulletItem.bullet.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.EnableDefaultLighting();
                            effect.World = bulletItem.bulletTransforms[mesh.ParentBone.Index] * Matrix.CreateScale(2f) * Matrix.CreateRotationY(MathHelper.Pi * 19 / 21) * bulletItem.bulletItemMatrix;
                            effect.View = view;
                            effect.Projection = projection;

                            mesh.Draw();
                        }
                        bulletItem.bulletBounding = TransformBoundingSphere(mesh.BoundingSphere, Matrix.CreateScale(2f) * bulletItem.bulletItemMatrix);
                    }
            foreach (scoreItem scoreItem in scoreItemList)
                if (scoreItem.appear == true)
                    foreach (ModelMesh mesh in scoreItem.score.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.EnableDefaultLighting();
                            effect.World = Matrix.CreateScale(0.8f) * scoreItem.scoreTransforms[mesh.ParentBone.Index] * scoreItem.scoreItemMatrix;
                            effect.View = view;
                            effect.Projection = projection;

                            mesh.Draw();
                        }
                        scoreItem.scoreBounding = TransformBoundingSphere(mesh.BoundingSphere, scoreItem.scoreItemMatrix);
                    }
            foreach (enemyItem enemyItem in enemyItemList)
                if (enemyItem.appear == true)
                    foreach (ModelMesh mesh in enemyItem.enemy.Meshes)
                    {
                        foreach (BasicEffect effect in mesh.Effects)
                        {
                            effect.EnableDefaultLighting();
                            effect.World = enemyItem.enemyTransforms[mesh.ParentBone.Index] * enemyItem.enemyItemMatrix;
                            effect.View = view;
                            effect.Projection = projection;

                            mesh.Draw();
                        }
                        enemyItem.enemyBounding = TransformBoundingSphere(mesh.BoundingSphere, enemyItem.enemyItemMatrix);
                    }
            foreach (obstacleItem obstacleItem in obstacleItemList)
                foreach (ModelMesh mesh in obstacleItem.obstacle.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.World = Matrix.CreateScale(10.0f) * obstacleItem.obstacleTransforms[mesh.ParentBone.Index] * obstacleItem.obstacleItemMatrix;
                        effect.View = view;
                        effect.Projection = projection;

                        mesh.Draw();
                    }
                    obstacleItem.obstacleBounding = TransformBoundingSphere(mesh.BoundingSphere, obstacleItem.obstacleItemMatrix);
                }
            spriteBatch.Begin();
            //spriteBatch.Draw(interface2D, new Vector2(-20, 500), Color.White);
            drawMiniMap();
            if (gameStarted == false)
                spriteBatch.DrawString(Font, "Please Press Enter to Start the Game.", new Vector2(200, 300), Color.Black);
            spriteBatch.DrawString(Font, "Acceration Energy:", new Vector2(10, 40), Color.White);
            spriteBatch.Draw(acceratorBar, new Vector2(10, 70), null,
        Color.Chocolate, 0, Vector2.Zero, new Vector2(GlobalObject.accelerationEnergy, 30),
        SpriteEffects.None, 0f);

            spriteBatch.DrawString(Font, "Slow Motion Power:", new Vector2(10, 100), Color.White);
            spriteBatch.Draw(acceratorBar, new Vector2(10, 130), null,
Color.GreenYellow, 0, Vector2.Zero, new Vector2(GlobalObject.slowMotionPower, 30),
SpriteEffects.None, 0f);
            spriteBatch.DrawString(Font, "Time Left: " + (int)timeleft, new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(Font, "Score: " + score, new Vector2(600, 10), Color.White);
            spriteBatch.DrawString(Font, "Number of Bullet: " + GlobalObject.bullectLeft, new Vector2(10, 550), Color.White);
            if (gameEnd == true)
            {
                spriteBatch.DrawString(Font, "Game End", new Vector2(300, 300), Color.White);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
        public void drawMiniMap()
        {
            spriteBatch.Draw(acceratorBar, new Vector2(GlobalObject.miniMapWidthPosition - 2, GlobalObject.miniMapHeightPosition - 2), null,
Color.Black, 0, Vector2.Zero, new Vector2(GlobalObject.miniMapSize + 4, GlobalObject.miniMapSize + 4),
SpriteEffects.None, 0f);
            spriteBatch.Draw(acceratorBar, new Vector2(GlobalObject.miniMapWidthPosition, GlobalObject.miniMapHeightPosition), null,
Color.White, 0, Vector2.Zero, new Vector2(GlobalObject.miniMapSize, GlobalObject.miniMapSize),
SpriteEffects.None, 0f);
            foreach (obstacleItem obstacleItem in obstacleItemList)
            {
                spriteBatch.Draw(acceratorBar, getPositionOnMiniMap(obstacleItem.obstacleItemMatrix), null,
Color.Green, 0, Vector2.Zero, new Vector2(GlobalObject.mapItemPixalSize, GlobalObject.mapItemPixalSize),
SpriteEffects.None, 0f);
            }
            foreach (scoreItem scoreItem in scoreItemList)
            {
                if (scoreItem.appear == true)
                    spriteBatch.Draw(acceratorBar, getPositionOnMiniMap(scoreItem.scoreItemMatrix), null,
    Color.Violet, 0, Vector2.Zero, new Vector2(GlobalObject.mapItemPixalSize, GlobalObject.mapItemPixalSize),
    SpriteEffects.None, 0f);
            }
            foreach (enemyItem enemyItem in enemyItemList)
            {
                if (enemyItem.appear == true)
                    spriteBatch.Draw(acceratorBar, getPositionOnMiniMap(enemyItem.enemyItemMatrix), null,
    Color.Blue, 0, Vector2.Zero, new Vector2(GlobalObject.mapItemPixalSize, GlobalObject.mapItemPixalSize),
    SpriteEffects.None, 0f);
            }
            spriteBatch.Draw(acceratorBar, getPositionOnMiniMap(player.playerMatrix), null,
Color.Red, 0, Vector2.Zero, new Vector2(GlobalObject.mapItemPixalSize, GlobalObject.mapItemPixalSize),
SpriteEffects.None, 0f);

        }
        public Vector2 getPositionOnMiniMap(Matrix position)
        {

            int positionX = Convert.ToInt32((position.Translation.X - GlobalObject.minX) / (GlobalObject.maxX - GlobalObject.minX) * GlobalObject.miniMapSize);
            int positionZ = Convert.ToInt32((position.Translation.Z - GlobalObject.minZ) / (GlobalObject.maxZ - GlobalObject.minZ) * GlobalObject.miniMapSize);
            return new Vector2(positionX + GlobalObject.miniMapWidthPosition, positionZ + GlobalObject.miniMapHeightPosition);
        }
        private static BoundingSphere TransformBoundingSphere(BoundingSphere sphere, Matrix transform)
        {
            BoundingSphere transformedSphere;
            Vector3 scale3 = new Vector3(sphere.Radius, sphere.Radius, sphere.Radius);
            scale3 = Vector3.TransformNormal(scale3, transform);
            transformedSphere.Radius = Math.Max(scale3.X, Math.Max(scale3.Y, scale3.Z));
            transformedSphere.Center = Vector3.Transform(sphere.Center, transform);
            return transformedSphere;
        }
    }

}



//if (timeleft < 0)
//{
//    timeleft = 0;
//}

//if (mykb.IsKeyDown(Keys.Up))
//{
//    GlobalObject.playerTranslationZ -= speed * accerator;
//}
//if (mykb.IsKeyDown(Keys.Down))
//{
//    GlobalObject.playerTranslationZ += speed * accerator;
//}

//if (mykb.IsKeyDown(Keys.Right))
//{
//    GlobalObject.playerTranslationX += speed * accerator;
//}
//if (mykb.IsKeyDown(Keys.Left))
//{
//    GlobalObject.playerTranslationX -= speed * accerator;
//}
//if (mykb.IsKeyDown(Keys.Space) && accerationEnergy > 0)
//{
//    accerator = 1.5f;
//    accerationEnergy -= elapsedTime * 100;
//}
//else
//    accerator = 1.0f;
//if (accerationEnergy < 300)
//{
//    accerationEnergy += elapsedTime * 30;
//}
// TODO: Add your update logic here