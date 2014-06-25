using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Aero
{
    class Level
    {
        static protected Cruiser[] cruisers;///
        protected const int maxCruisers = 50;//
        static protected Fighter fighter;////
        static protected Kamacazie kamacazie;//////
        static protected FatBoss fatBoss;//////
        static protected float spawnCruiserCooldown;//////
        static protected float spawnFighterCooldown;
        static protected float spawnKamicazeCooldown;
        static protected float spawnPowerUpCooldown;
        static protected float startShopCooldown;
        static protected PowerUp powerUp;///
        static protected int objectsSpawned;
        static protected Random rand;
        static public List<AeroObject> activeObjects;
        protected Boss boss;
        protected Rectangle viewportRect;
        protected bool done;
        static SoundEffect backgroundMusic;
        protected float levelTimeout;
        protected float maxTimeout;
        protected bool bossSpawned;
        static protected SoundEffect soundPowerUpPickup;
        static protected SoundEffect soundPowerUpKill;

        bool updateTwice = false;

        public Level()
        {
            viewportRect = new Rectangle(0, 0, AeroGame.Graphics.GraphicsDevice.Viewport.Width,
                                                         AeroGame.Graphics.GraphicsDevice.Viewport.Height);
            objectsSpawned = 0;
            startShopCooldown = 0;
            bossSpawned = false;
            maxTimeout = 30;
        }

        static public void LoadContent()
        {
            rand = new Random();
            activeObjects = new List<AeroObject>();
            activeObjects.Capacity = 1024;
            cruisers = new Cruiser[maxCruisers];
            for (int i = 0; i < maxCruisers; i++)
            {
                cruisers[i] = new Cruiser();
            }
            fighter = new Fighter();//////////////////Level Two+
            kamacazie = new Kamacazie();/////////////////////Level 3+
            fatBoss = new FatBoss();////////////////////////////Survival
            //boss = new BossOne(game,player);////////////////Each Level
            powerUp = new PowerUpRapidFire();
            soundPowerUpPickup = AeroGame.ContentManager.Load<SoundEffect>("Sounds\\PowerupPickup");
            soundPowerUpKill = AeroGame.ContentManager.Load<SoundEffect>("Sounds\\PowerupKill");
            //backgroundMusic = AeroGame.ContentManager.Load<SoundEffect>("Sounds\\SoundBackground");
            //backgroundMusic.Play(1.0f, 0, 0, true);
        }

        public virtual void Update(TimeSpan elapsedTime)
        {
            Player.Update(elapsedTime);
            for (int i = 0; i < activeObjects.Count; i++)
            {
                activeObjects[i].Update(elapsedTime);
            }
            for (int i = 0; i < activeObjects.Count; i++)
            {
                if (!activeObjects[i].Active)
                    activeObjects.RemoveAt(i);
            }
            //if (powerUp.Alive)
            //    powerUp.Update(elapsedTime);
            //if (kamacazie.Alive || kamacazie.Exploding)
            //    kamacazie.Update(elapsedTime);////////////Level 3+
            //fighter.Update(elapsedTime);/////Level 2+
            //foreach (Cruiser c in cruisers)
            //    if (c.Alive || c.Exploding)
            //        c.Update(elapsedTime);

            //Calculate when to spawn enemy Cruisers
            /*if (!boss.Exploding)
            {
                spawnCruiserCooldown -= (float)elapsedTime.TotalSeconds;
                if (spawnCruiserCooldown < 0)
                {
                    spawnCruiserCooldown = 0.5f;
                    foreach (Cruiser e in cruisers)
                        if (!e.Active)
                        {
                            spawnEnemy(e);
                            return;
                        }
                }
            }*/
            if (startShopCooldown > 0)
            {
                startShopCooldown -= (float)elapsedTime.TotalSeconds;
                if (startShopCooldown <= 0)
                {
                    done = true;
                    startShopCooldown = 0;
                }
            }

            //Decide when to spawn a PowerUp. 
            spawnPowerUpCooldown += (float)elapsedTime.TotalSeconds;
            if (!boss.Alive && !powerUp.Alive)
            {
                if (spawnPowerUpCooldown > 5.0f)
                {
                    spawnPowerUp(4);
                    spawnPowerUpCooldown = 0.0f;
                }
            }
            else if(!powerUp.Alive)
            {
                if (spawnPowerUpCooldown > 15.0f)
                {
                    spawnPowerUp(4);
                    spawnPowerUpCooldown = 0.0f;
                }
            }
                
            //if (objectsSpawned % 3 == 0 && objectsSpawned > 0 && !boss.Alive && !powerUp.Alive)
            //    spawnPowerUp(3);
        }

        public virtual void Draw()
        {
            for (int i = 0; i < activeObjects.Count; i++)
                activeObjects[i].Draw();
        }

        protected void spawnEnemy(Enemy enemy)
        {
            if (!boss.Active)
                objectsSpawned += 1;
            if (!enemy.Active)
            {
                enemy.spawn(new Vector2(rand.Next(viewportRect.Width - enemy.Texture.Width), 0), Player.Position.X);
                activeObjects.Add(enemy);
            }
        }

        protected void spawnBoss()
        {
            if (boss is TwinBoss)
            {
                TwinBoss twinBoss = (TwinBoss)boss;
                twinBoss.Spawn(new Vector2(viewportRect.Width / 2 - boss.Texture.Width / 2, boss.Texture.Height * -1.0f));
                activeObjects.Add(twinBoss);
                twinBoss.TwinOne.Spawn(new Vector2(viewportRect.Width / 2 - boss.Texture.Width / 2, boss.Texture.Height * -1.0f));
                activeObjects.Add(twinBoss.TwinOne);
                twinBoss.TwinTwo.Spawn(new Vector2((viewportRect.Width / 2 - boss.Texture.Width / 2) + 170, boss.Texture.Height * -1.0f));
                activeObjects.Add(twinBoss.TwinTwo);
            }
            else
            {
                boss.Spawn(new Vector2(viewportRect.Width / 2 - boss.Texture.Width / 2, boss.Texture.Height * -1.0f));
                activeObjects.Add(boss);
            }
            bossSpawned = true;
        }

        protected void spawnPowerUp(int seed)
        {
            objectsSpawned += 1;
            Vector2 randomPosition = new Vector2(rand.Next(viewportRect.Width - powerUp.Texture.Width), 0);
            int randomType = rand.Next(seed);
            if (randomType == 0)
                powerUp = new PowerUpTriCannon();
            else if (randomType == 1)
                powerUp = new PowerUpQuintuCannon();
            else if (randomType == 2)
                powerUp = new PowerUpInvincibility();
            else if (randomType == 3)
                powerUp = new PowerUpRapidFire();
            else if (randomType == 4)
                powerUp = new PowerUpShop();
            powerUp.spawn(randomPosition);
            activeObjects.Add(powerUp);
        }

        static public void InitLevelEnd()
        {
            startShopCooldown = 5.5f;
            spawnCruiserCooldown = 3.0f;
        }

        public virtual void OffScreenDetection()
        {
            for (int i = 0; i < activeObjects.Count; i++)
            {
                //if (!viewportRect.Contains(new Point(activeObjects[i].BoundingRectangle.Center.X + activeObjects[i].Texture.Width, 
                //    activeObjects[i].BoundingRectangle.Center.Y - activeObjects[i].Texture.Width)))
                if(activeObjects[i].BoundingRectangle.Center.Y - (activeObjects[i].Texture.Height / 2) > viewportRect.Bottom ||
                    activeObjects[i].BoundingRectangle.Center.Y + (activeObjects[i].Texture.Height / 2) < viewportRect.Top ||
                    activeObjects[i].BoundingRectangle.Center.X - (activeObjects[i].Texture.Width / 2) > viewportRect.Right ||
                    activeObjects[i].BoundingRectangle.Center.X + (activeObjects[i].Texture.Width / 2) < viewportRect.Left)
                {
                    activeObjects[i].killOffScreen();
                }
            }
            //foreach (Round r in Player.MainWeapon.Rounds)
            //    if (r.Alive)
            //        if (!viewportRect.Contains(r.BoundingRectangle.Center + r.Texture.Height))
            //            r.killOffScreen();

            //foreach (Round r in boss.MainWeapon.Rounds)
            //    if (r.Alive)
            //        if (!viewportRect.Contains(r.BoundingRectangle.Center + r.Texture.Height))
            //            r.killOffScreen();

            //foreach (Cruiser c in cruisers)
            //    if (c.Alive)
            //        if (!viewportRect.Contains(c.BoundingRectangle.Center + new Point(c.Texture.Height / 2, c.Texture.Width / 2)))
            //            c.killOffScreen();

            //if (fighter.Alive)
            //{
            //    if (!viewportRect.Contains(fighter.BoundingRectangle.Center + new Point(fighter.Texture.Height / 2, fighter.Texture.Width / 2)))
            //        fighter.killOffScreen();
            //    foreach (Round r in fighter.MainWeapon.Rounds)
            //        if (r.Alive)
            //            if (!viewportRect.Contains(r.BoundingRectangle.Center + new Point(r.Texture.Height / 2, r.Texture.Width / 2)))
            //                r.killOffScreen();
            //}

            //if (kamacazie.Alive)
            //    if (!viewportRect.Contains(kamacazie.BoundingRectangle.Center + new Point(kamacazie.Texture.Height / 2, kamacazie.Texture.Width / 2)))
            //        kamacazie.killOffScreen();

            //if (powerUp.Alive)
            //    if (!viewportRect.Contains(powerUp.BoundingRectangle.Center + new Point(powerUp.Texture.Height / 2, powerUp.Texture.Width / 2)))
            //        powerUp.killOffScreen();
        }

        protected static bool CollisionCheck(AeroObject object1, AeroObject object2)
        {
            if (object1.BoundingRectangle.Intersects(object2.BoundingRectangle))
                if (IntersectPixels(object1.Rotation, object1.Texture.Width, object1.Texture.Height, object1.TextureData,
                    object2.Rotation, object2.Texture.Width, object2.Texture.Height, object2.TextureData))
                    return true;
            return false;
        }

        protected static bool CollisionCheckWithPlayer(AeroObject object2)
        {
            if (Player.BoundingRectangle.Intersects(object2.BoundingRectangle))
                if (IntersectPixels(Player.Rotation, Player.Texture.Width, Player.Texture.Height, Player.TextureData,
                    object2.Rotation, object2.Texture.Width, object2.Texture.Height, object2.TextureData))
                    return true;
            return false;
        }

        protected static bool IntersectPixels(
                Matrix transformA, int widthA, int heightA, Color[] dataA,
                Matrix transformB, int widthB, int heightB, Color[] dataB)
        {
            // Calculate a matrix which transforms from A's local space into
            // world space and then into B's local space
            Matrix transformAToB = transformA * Matrix.Invert(transformB);

            // When a point moves in A's local space, it moves in B's local space with a
            // fixed direction and distance proportional to the movement in A.
            // This algorithm steps through A one pixel at a time along A's X and Y axes
            // Calculate the analogous steps in B:
            Vector2 stepX = Vector2.TransformNormal(Vector2.UnitX, transformAToB);
            Vector2 stepY = Vector2.TransformNormal(Vector2.UnitY, transformAToB);

            // Calculate the top left corner of A in B's local space
            // This variable will be reused to keep track of the start of each row
            Vector2 yPosInB = Vector2.Transform(Vector2.Zero, transformAToB);

            // For each row of pixels in A
            for (int yA = 0; yA < heightA; yA++)
            {
                // Start at the beginning of the row
                Vector2 posInB = yPosInB;

                // For each pixel in this row
                for (int xA = 0; xA < widthA; xA++)
                {
                    // Round to the nearest pixel
                    int xB = (int)Math.Round(posInB.X);
                    int yB = (int)Math.Round(posInB.Y);

                    // If the pixel lies within the bounds of B
                    if (0 <= xB && xB < widthB &&
                        0 <= yB && yB < heightB)
                    {
                        // Get the colors of the overlapping pixels
                        Color colorA = dataA[xA + yA * widthA];
                        Color colorB = dataB[xB + yB * widthB];

                        // If both pixels are not completely transparent,
                        if (colorA.A != 0 && colorB.A != 0)
                        {
                            // then an intersection has been found
                            return true;
                        }
                    }

                    // Move to the next pixel in the row
                    posInB += stepX;
                }

                // Move to the next row
                yPosInB += stepY;
            }

            // No intersection found
            return false;
        }
        //Check between friendly and non-friendly objects.
        public virtual void CollisionDetection()
        {
            for (int f = 0; f < activeObjects.Count; f++)
            {
                if (activeObjects[f].Friendly && activeObjects[f].Alive)
                {
                    for (int e = 0; e < activeObjects.Count; e++)
                    {
                        if (!activeObjects[e].Friendly && activeObjects[e].Alive)
                        {
                            if (CollisionCheck(activeObjects[f], activeObjects[e]))
                            {
                                activeObjects[f].Kill();
                                activeObjects[e].Hit(Player.MainWeaponPower);
                                if (activeObjects[e] is PowerUp)
                                    soundPowerUpKill.Play();
                            }
                        }
                    }
                }
            }
            //Check between non-friendly objects and player.
            for (int e = 0; e < activeObjects.Count; e++)
            {
                if (!activeObjects[e].Friendly && activeObjects[e].Alive && !Player.Reviving && !Player.Exploding)
                {
                    if (CollisionCheckWithPlayer(activeObjects[e]))
                    {
                        if (activeObjects[e] is PowerUp)
                        {
                            if (activeObjects[e] is PowerUpShop)
                                done = true;
                            else
                                Player.PowerUpType = activeObjects[e].PowerUpType;
                            activeObjects[e].Kill();
                            soundPowerUpPickup.Play();
                        }
                        else
                        {
                            Player.Hit(10);
                            if (activeObjects[e] is Boss)
                            {
                                //Do Nothing
                            }
                            else
                                activeObjects[e].Kill();
                        }
                    }
                }
            }

            ////Collision check of player's rounds
            //foreach (Round r in Player.PrimaryWeapon.Rounds)
            //{
            //    //Check between player rounds and enemies.
            //    if (r.Alive)
            //    {
            //        foreach (Cruiser c in cruisers)
            //            if (c.Alive)
            //                if (CollisionCheck(r, c))
            //                {
            //                    r.kill();
            //                    c.hit(Player.MainWeaponPower);
            //                    if (!boss.Alive && !boss.Exploding && !c.Alive)
            //                        Player.Score += 10;
            //                    break;
            //                }

            //        if (boss.Alive)
            //            if (CollisionCheck(r, boss))
            //            {
            //                r.kill();
            //                boss.hit(Player.MainWeaponPower);
            //                if (boss.Health <= 0)
            //                {
            //                    boss.kill();
            //                    Player.Score += 500;
            //                    startShopCooldown = 5.5f;
            //                    spawnCruiserCooldown = 3.0f;
            //                }
            //            }
            //        if (fighter.Alive)
            //            if (CollisionCheck(r, fighter))
            //            {
            //                r.kill();
            //                fighter.hit(Player.MainWeaponPower);
            //                if (!boss.Alive && !boss.Exploding && !fighter.Alive)
            //                    Player.Score += 20;
            //            }
            //        if (kamacazie.Alive)
            //        {
            //            if (CollisionCheck(r, kamacazie))
            //            {
            //                r.kill();
            //                kamacazie.hit(Player.MainWeaponPower);
            //                if (!boss.Alive && !boss.Exploding && !kamacazie.Alive)
            //                    Player.Score += 30;
            //            }
            //        }
            //    }
            //}
            /////////////////////////////////////////////
            ////Check between player missile and enemies.
            //    if (Player.SecondaryWeapon.Projectile.Active)
            //    {
            //        foreach (Cruiser c in cruisers)
            //            if (c.Alive)
            //                if (CollisionCheck(Player.SecondaryWeapon.Projectile, c))
            //                {
            //                    Player.SecondaryWeapon.Projectile.Kill();
            //                    c.hit(Player.MainWeaponPower);
            //                    if (!boss.Alive && !boss.Exploding && !c.Alive)
            //                        Player.Score += 10;
            //                    break;
            //                }

            //        if (boss.Alive)
            //            if (CollisionCheck(Player.SecondaryWeapon.Projectile, boss))
            //            {
            //                Player.SecondaryWeapon.Projectile.Kill();
            //                boss.hit(Player.MainWeaponPower);
            //                if (boss.Health <= 0)
            //                {
            //                    boss.kill();
            //                    Player.Score += 500;
            //                    startShopCooldown = 5.5f;
            //                    spawnCruiserCooldown = 3.0f;
            //                }
            //            }
            //        if (fighter.Alive)
            //            if (CollisionCheck(Player.SecondaryWeapon.Projectile, fighter))
            //            {
            //                Player.SecondaryWeapon.Projectile.Kill();
            //                fighter.hit(Player.MainWeaponPower);
            //                if (!boss.Alive && !boss.Exploding && !fighter.Alive)
            //                    Player.Score += 20;
            //            }
            //        if (kamacazie.Alive)
            //        {
            //            if (CollisionCheck(Player.SecondaryWeapon.Projectile, kamacazie))
            //            {
            //                Player.SecondaryWeapon.Projectile.Kill();
            //                kamacazie.hit(Player.MainWeaponPower);
            //                if (!boss.Alive && !boss.Exploding && !kamacazie.Alive)
            //                    Player.Score += 30;
            //            }
            //        }
            //    }
            ////Collision check between player and enemy's; and
            ////Collision check between boss rounds and player; and
            ////Collision check between fighter rounds and player
            //if (Player.Alive && !Player.Reviving)
            //{
            //    foreach (Cruiser c in cruisers)
            //        if (c.Alive)
            //            if (CollisionCheckWithPlayer(c))
            //            {
            //                if (!Player.Invincible && Player.PowerUpType != PowerUpType.Invincibility)
            //                    Player.hit(10);
            //                c.kill();
            //            }
            //    if (powerUp.Alive)
            //    {
            //        if (CollisionCheckWithPlayer(powerUp))
            //        {
            //            Player.PowerUpType = powerUp.PowerUpType;
            //            powerUp.kill();
            //            Player.Score += 50;
            //        }
            //    }

            //    if (boss.Alive)
            //    {
            //        foreach (Round r in boss.MainWeapon.Rounds)
            //            if (CollisionCheckWithPlayer(r))
            //                if (r.Alive)
            //                {
            //                    if (!Player.Invincible)
            //                        Player.hit(10);
            //                    r.kill();
            //                }
            //    }

            //    if (fighter.Alive)
            //    {
            //        if (CollisionCheckWithPlayer(fighter))
            //        {
            //            if (!Player.Invincible)
            //                Player.hit(10);
            //            fighter.kill();
            //        }
            //        foreach (Round r in fighter.MainWeapon.Rounds)
            //            if (r.Alive)
            //                if (CollisionCheckWithPlayer(r))
            //                {
            //                    if (!Player.Invincible)
            //                        Player.hit(10);
            //                    r.kill();
            //                }
            //    }

            //    if (kamacazie.Alive)
            //        if (CollisionCheckWithPlayer(kamacazie))
            //        {
            //            if (!Player.Invincible)
            //                Player.hit(10);
            //            kamacazie.kill();
            //        }
            //}
        }

        public virtual void HandleInput(InputState input)
        {
            //GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
#if !XBOX
            //KeyboardState keyboardState = Keyboard.GetState();
            //if (input.IsPauseGame(ControllingPlayer))
            //{
            //    ScreenManager.
            //    ScreenManager.AddScreen(new PauseScreen(game), ControllingPlayer);
            //    paused = true;
            //}
            //else if (shopActive)
            //{
            //    ScreenManager.AddScreen(new LevelTwo(game), ControllingPlayer);
            //    ShopScreen shopScreen = new ShopScreen(player, game);
            //    ScreenManager.AddScreen(shopScreen, ControllingPlayer);
            //    paused = true;
            //    shopActive = false;
            //}
            //if
            //{
            //    paused = false;
            //    if (keyboardState.IsKeyDown(Keys.Up) && !player.Reviving && !paused)
            //    {
            //        player.moveUp();
            //    }
            //    if (keyboardState.IsKeyDown(Keys.Down) && !player.Reviving && !paused)
            //    {
            //        player.moveDown();
            //    }
            //    if (keyboardState.IsKeyDown(Keys.Left) && !player.Reviving && !paused)
            //    {
            //        player.moveLeft();
            //    }
            //    if (keyboardState.IsKeyDown(Keys.Right) && !player.Reviving && !paused)
            //    {
            //        player.moveRight();
            //    }
            //    if (keyboardState.IsKeyDown(Keys.Z) && !paused)
            //    {
            //        if (player.MainWeapon.CoolDown <= 0 && player.Alive)
            //        {
            //            player.MainWeapon.Fire();
            //        }
            //    }
#endif
        }
        static public int ObjectsSpawned
        {
            get
            {
                return objectsSpawned;
            }
        }
        public Boss Boss
        {
            get
            {
                return boss;
            }
        }
        public bool Done
        {
            get
            {
                return done;
            }
            set
            {
                done = value;
            }
        }
    }
}
