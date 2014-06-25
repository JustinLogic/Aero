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
    public struct SaveGameData
    {
        public string playerName;
        public TimeSpan gameTime;
        public int score;
        public SaveGameData(TimeSpan time, int score, string name){
            this.gameTime = time;
            this.score = score;
            this.playerName = name;
        }
    }

    static class Player
    {
        static private int score;
        static ScoreSystem scoreSystem = new ScoreSystem();
        static private int lives;
        static private bool reviving;
        static private bool exploding;
        static private bool invincible;
        static private bool powerUpActive;
        static private float currentShield;
        static private float maxShield;
        static private float percentShield;
        static private float speed;
        static private int mainWeaponPower;
        static private int revivalExpansionValue;
        static private float resetInvincibilityCooldown;
        static private float powerUpCooldown;
        static private float rapidFireCooldown;
        static private float invincibilityCooldown;
        static private Texture2D textureInvincible;
        static private PrimaryWeapon primaryWeapon;
        static private StandardCannon standardCannon;
        static private TriCannon triCannon;
        static private QuintuCannon quintuCannon;
        static SecondaryWeapon secondaryWeapon;
        static FragmentationBombCannon fragCannon;
        static BeamCannon beamCannon;
        static MissleCannon missileCannon;
        static private SmallExplosionAnimation explodingAnimation;
        static private ReviveAnimation reviveAnimation;
        static private PowerUpType powerUpType;
        static private TimeSpan dt;
        static SoundEffect soundFireBullet;
        static SoundEffect soundFireBomb;
        static SoundEffect soundHit;
        static SoundEffect soundKill;
        /// <summary>
        /// Brought from AeroObject
        /// </summary>
        static int scale;
        static float theta;
        static bool alive;
        static Vector2 position;
        static Vector2 startingPosition;
        static Vector2 velocity;
        static Vector2 center;
        static Vector2 origin;
        static Texture2D texture;
        static Color[] textureData;
        static Matrix rotation;
        static Rectangle boundingRectangle;
        static SpriteBatch spriteBatch;
        //Data for bounding rectangle.
        static Vector2 leftTop;
        static Vector2 rightTop;
        static Vector2 leftBottom;
        static Vector2 rightBottom;
        static Vector2 min;
        static Vector2 max;
        static SaveGameData saveData;
        static string stringMainWeaponPower;
        static string stringShield;
        static string stringLives;

        static private void ReviveReset()
        {
            resetInvincibilityCooldown = 1.0f;
            invincible = true;
            revivalExpansionValue = 1;
            CurrentShield = maxShield;
            position = startingPosition;
            primaryWeapon.Center = center;
            alive = true;
            //Reset PowerUp
            powerUpCooldown = 0;
            powerUpType = PowerUpType.None;
            primaryWeapon = standardCannon;
            primaryWeapon.SetCoolDown(0.2f);
            powerUpActive = false;
        }

        static Player()
        {
           
        }

        static public void LoadContent()
        {
            texture = AeroGame.ContentManager.Load<Texture2D>("Textures\\Player");
            textureInvincible = AeroGame.ContentManager.Load<Texture2D>("Textures\\PlayerGlow");
            textureData = new Color[texture.Width * texture.Height];
            texture.GetData(textureData);
            soundFireBullet = AeroGame.ContentManager.Load<SoundEffect>("Sounds\\PlayerLaser");
            soundFireBomb = AeroGame.ContentManager.Load<SoundEffect>("Sounds\\Bomb");
            soundHit = AeroGame.ContentManager.Load<SoundEffect>("Sounds\\PlayerHit");
            soundKill = AeroGame.ContentManager.Load<SoundEffect>("Sounds\\PlayerKill");
            position = startingPosition = new Vector2(AeroGame.Graphics.GraphicsDevice.Viewport.Width / 2 - texture.Width / 2.0f,
                                    AeroGame.Graphics.GraphicsDevice.Viewport.Height * .75f);
            center = new Vector2(position.X + texture.Width / 2.0f, position.Y + texture.Height / 2.0f);
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            speed = 200.0f;
            velocity = new Vector2(speed);
            alive = true;
            reviving = false;
            exploding = false;
            invincible = false;
            powerUpActive = false;
            resetInvincibilityCooldown = 1.0f;
            powerUpCooldown = 0;
            rapidFireCooldown = 0;
            invincibilityCooldown = 0;
            lives = 3;
            mainWeaponPower = 10;
            currentShield = 30;
            maxShield = 30;
            percentShield = (int)((currentShield / maxShield) * 100);
            powerUpType = PowerUpType.None;
            standardCannon = new StandardCannon(true);
            triCannon = new TriCannon(true);
            quintuCannon = new QuintuCannon(true);
            primaryWeapon = standardCannon;
            primaryWeapon.SetCoolDown(0.2f);
            fragCannon = new FragmentationBombCannon(true);
            beamCannon = new BeamCannon(true);
            missileCannon = new MissleCannon(true);
            secondaryWeapon = fragCannon;
            explodingAnimation = new SmallExplosionAnimation();
            reviveAnimation = new ReviveAnimation();
            dt = TimeSpan.Zero;
            theta = 0;
            spriteBatch = new SpriteBatch(AeroGame.Graphics.GraphicsDevice);
            rotation = new Matrix();
            boundingRectangle = new Rectangle();
            scale = 1;
            //Bounding Rectangle Data
            leftTop = Vector2.Zero;
            rightTop = Vector2.Zero;
            leftBottom = Vector2.Zero;
            rightBottom = Vector2.Zero;
            min = Vector2.Zero;
            max = Vector2.Zero;
            stringMainWeaponPower = mainWeaponPower.ToString();
            stringShield = percentShield.ToString() + "%";
            stringLives = "x" + lives.ToString();
            //saveData = new SaveGameData();
            //SignedInGamer gamer;
            
            //saveData.playerName = Gamer.SignedInGamers[0].Gamertag;
        }

        static public void Update(TimeSpan elapsedTime)
        {
            dt = elapsedTime;
            Position = new Vector2(position.X, position.Y);
            primaryWeapon.Update(elapsedTime);
            secondaryWeapon.Update(elapsedTime);
            if (invincible && !powerUpActive)
            {
                resetInvincibilityCooldown -= (float)elapsedTime.TotalSeconds;
                if (resetInvincibilityCooldown < 0)
                {
                    invincible = false;
                    resetInvincibilityCooldown = 1.0f;
                }
            }
            primaryWeapon.Center = center;
            secondaryWeapon.Center = center;

            if (exploding)
            {
                if (explodingAnimation.Active)
                    explodingAnimation.Update(elapsedTime);
                else
                {
                    exploding = false;
                }
            }

            if (reviving)
            {
                if (reviveAnimation.Active)
                    reviveAnimation.Update(elapsedTime);
                else
                {
                    reviving = false;
                    ReviveReset();
                }
            }

            if (powerUpCooldown > 0)
            {
                powerUpCooldown -= (float) elapsedTime.TotalSeconds;
                if (powerUpCooldown <= 0)
                {
                    powerUpCooldown = 0;
                    powerUpType = PowerUpType.None;
                    primaryWeapon = standardCannon;
                    primaryWeapon.SetCoolDown(0.2f);
                    powerUpActive = false;
                }
            }
            if (rapidFireCooldown > 0)
            {
                rapidFireCooldown -= (float)elapsedTime.TotalSeconds;
                if (rapidFireCooldown <= 0)
                {
                    rapidFireCooldown = 0;
                    primaryWeapon.SetCoolDown(0.2f);
                }
            }
            if (invincibilityCooldown > 0)
            {
                invincibilityCooldown -= (float)elapsedTime.TotalSeconds;
                if (invincibilityCooldown <= 0)
                {
                    invincibilityCooldown = 0;
                    invincible = false;
                }
            }
            rotation =
                Matrix.CreateTranslation(new Vector3(-origin, 0.0f)) *
                Matrix.CreateRotationZ(theta) *
                Matrix.CreateTranslation(new Vector3(position, 0.0f));
            boundingRectangle = CalculateBoundingRectangle(new Rectangle(0, 0, texture.Width, texture.Height), rotation);
        }

        static Rectangle CalculateBoundingRectangle(Rectangle rectangle,
                                        Matrix transform)
        {
            //// Get all four corners in local space
            //leftTop = new Vector2(rectangle.Left, rectangle.Top);
            //rightTop = new Vector2(rectangle.Right, rectangle.Top);
            //leftBottom = new Vector2(rectangle.Left, rectangle.Bottom);
            //rightBottom = new Vector2(rectangle.Right, rectangle.Bottom);

            leftTop.X = rectangle.Left;
            leftTop.Y = rectangle.Top;
            rightTop.X = rectangle.Right;
            rightTop.Y = rectangle.Top;
            leftBottom.X = rectangle.Left;
            leftBottom.Y = rectangle.Bottom;
            rightBottom.X = rectangle.Right;
            rightBottom.Y = rectangle.Bottom;

            // Transform all four corners into work space
            Vector2.Transform(ref leftTop, ref transform, out leftTop);
            Vector2.Transform(ref rightTop, ref transform, out rightTop);
            Vector2.Transform(ref leftBottom, ref transform, out leftBottom);
            Vector2.Transform(ref rightBottom, ref transform, out rightBottom);

            // Find the minimum and maximum extents of the rectangle in world space
            min = Vector2.Min(Vector2.Min(leftTop, rightTop),
                                      Vector2.Min(leftBottom, rightBottom));
            max = Vector2.Max(Vector2.Max(leftTop, rightTop),
                                      Vector2.Max(leftBottom, rightBottom));

            // Return as a rectangle
            return new Rectangle((int)min.X, (int)min.Y,
                                 (int)(max.X - min.X), (int)(max.Y - min.Y));
        }

        static public void Hit(int damage)
        {
            if (!invincible)
            {
                if (CurrentShield > 0)
                {
                    CurrentShield -= damage;
                    soundHit.Play();
                }
                else
                {
                    Kill();
                    soundKill.Play();
                }
            }
        }

        static public void Kill()
        {
            if (alive)
            {
                lives -= 1;
                Score -= 100;
                stringLives = "x" + lives.ToString();
            }
            if (lives >= 0)
            {
                reviveAnimation.start(position);
                reviving = true;
                alive = false;
            }
            else
            {
                explodingAnimation.Start(center);
                exploding = true;
                alive = false;
            }
        }

        static public void Reset()
        {
            position = startingPosition = new Vector2(AeroGame.Graphics.GraphicsDevice.Viewport.Width / 2 - texture.Width / 2.0f,
                                    AeroGame.Graphics.GraphicsDevice.Viewport.Height * .75f);
            center = new Vector2(position.X + texture.Width / 2.0f, position.Y);
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            alive = true;
            reviving = false;
            exploding = false;
            invincible = false;
            powerUpActive = false;
            resetInvincibilityCooldown = 1.0f;
            powerUpType = PowerUpType.None;
            fragCannon = new FragmentationBombCannon(true);
            primaryWeapon = standardCannon;
            secondaryWeapon = fragCannon;
            if (AeroGame.Troll == true)
            {
                lives = 42;
                currentShield = maxShield;
                percentShield = (int)((currentShield / maxShield) * 100);
            }
            else
            {
                speed = 200.0f;
                velocity = new Vector2(speed);
                lives = 3;
                mainWeaponPower = 10;
                currentShield = 30;
                maxShield = 30;
                percentShield = (int)((currentShield / maxShield) * 100);
                scoreSystem = new ScoreSystem();
            }
            stringMainWeaponPower = mainWeaponPower.ToString();
            stringShield = percentShield.ToString() + "%";
            stringLives = "x" + lives.ToString();
        }

        static public void Draw()
        {
            //AeroGame.SpriteBatch.Begin();
            //Check player's state.
            if (alive)
            {
                if (invincible)
                {
                    //Change tint if player is invincible.
                    AeroGame.SpriteBatch.Draw(textureInvincible, position, null, Color.White, -theta, origin, scale, SpriteEffects.None, 0);
                }
                else
                {
                    //Draw player normally.
                    AeroGame.SpriteBatch.Draw(texture, position, null, Color.White, -theta, origin, scale, SpriteEffects.None, 0);
                }
            }
            else if (reviving)
            {
                //If player is reviving, expande size by ExpansionValue.
                //spriteBatch.Draw(texture, new Rectangle((int)position.X - revivalExpansionValue / 2 + 16, (int)position.Y - revivalExpansionValue / 2 + 16,
                                    //texture.Width + revivalExpansionValue, texture.Height + revivalExpansionValue), Color.White);
                reviveAnimation.draw();
            }

            else if (exploding)
            {
                //If player is reviving, expande size by ExpansionValue.
                //spriteBatch.Draw(texture, new Rectangle((int)position.X - revivalExpansionValue / 2 + 16, (int)position.Y - revivalExpansionValue / 2 + 16,
                                    //texture.Width + revivalExpansionValue, texture.Height + revivalExpansionValue), Color.White);
                explodingAnimation.Draw();
            }

            //Iterate through player rounds to be drawn.
            //foreach (Round r in mainWeapon.Rounds)
            //{
            //    if (r.Alive || r.Exploding)
            //    {
            //        //Draw fired rounds.
            //        r.Draw();
            //    }
            //}
            //AeroGame.SpriteBatch.End();
        }

        static public void revive(Texture2D loadedTexture)
        {
            if (lives >= 0)
            {
                invincible = true;
                reviving = true;
                texture = loadedTexture;
            }
        }

        static public void move(float stickX, float stickY)
        {
            position = new Vector2(position.X + (velocity.X * stickX) * (float)dt.TotalSeconds,
                                    position.Y - (velocity.Y * stickY) * (float)dt.TotalSeconds);
            if (position.Y < AeroGame.Graphics.GraphicsDevice.Viewport.Height * 0.65f)
                position.Y = AeroGame.Graphics.GraphicsDevice.Viewport.Height * 0.65f;
            if (position.Y > AeroGame.Graphics.GraphicsDevice.Viewport.Height - boundingRectangle.Height / 2)
                position.Y = AeroGame.Graphics.GraphicsDevice.Viewport.Height - boundingRectangle.Height / 2;
            if (position.X < 0)
                position.X = 0;
                //position.X = boundingRectangle.Left + boundingRectangle.Width / 2;
            if (position.X > AeroGame.Graphics.GraphicsDevice.Viewport.Width - boundingRectangle.Width / 2)
                position.X = AeroGame.Graphics.GraphicsDevice.Viewport.Width - boundingRectangle.Width / 2;
        }

        static public void moveUp()
        {
            //Position = new Vector2(position.X,
            //    position.Y - velocity.Y * (float)elapsedTime.TotalSeconds);
            position.Y = position.Y - velocity.Y * (float)dt.TotalSeconds;
            if (position.Y < AeroGame.Graphics.GraphicsDevice.Viewport.Height * 0.65f)
                position.Y = AeroGame.Graphics.GraphicsDevice.Viewport.Height * 0.65f;
        }

        static public void moveDown()
        {
            //Position = new Vector2(position.X,
            //    position.Y + velocity.Y * (float)elapsedTime.TotalSeconds);
            position.Y = position.Y + velocity.Y * (float)dt.TotalSeconds;
            if (position.Y > AeroGame.Graphics.GraphicsDevice.Viewport.Height - boundingRectangle.Height / 2)
                position.Y = AeroGame.Graphics.GraphicsDevice.Viewport.Height - boundingRectangle.Height / 2;
        }

        static public void moveLeft()
        {
            //Position = new Vector2(position.X - velocity.X * (float)elapsedTime.TotalSeconds,
            //    position.Y);
            position.X = position.X - velocity.X * (float)dt.TotalSeconds;
            if (boundingRectangle.Left < 0)
                position.X = boundingRectangle.Left + boundingRectangle.Width / 2;
        }

        static public void moveRight()
        {
            //Position = new Vector2(position.X + velocity.X * (float)elapsedTime.TotalSeconds,
            //    position.Y);
            position.X = position.X + velocity.X * (float)dt.TotalSeconds;
            if (position.X > AeroGame.Graphics.GraphicsDevice.Viewport.Width - boundingRectangle.Width / 2)
                position.X = AeroGame.Graphics.GraphicsDevice.Viewport.Width - boundingRectangle.Width / 2;
        }

        static public void FirePrimary(float firingAngle)
        {
            primaryWeapon.Fire(firingAngle);
            soundFireBullet.Play();
        }

        static public void FireSecondary(float firingAngle)
        {
            secondaryWeapon.Fire(firingAngle);
            soundFireBomb.Play();
        }

        static public bool Alive
        {
            get
            {
                return alive;
            }
            set
            {
                alive = value;
            }
        }

        static public bool Reviving
        {
            get
            {
                return reviving;
            }
        }

        static public bool Exploding
        {
            get
            {
                return exploding;
            }
        }

        static public bool Invincible
        {
            get
            {
                return invincible;
            }
        }

        static public int Score
        {
            get
            {
                return score;
            }
            set
            {
                score = value;
                if (score < 0)
                    score = 0;
            }
        }

        static public int Lives
        {
            get
            {
                return lives;
            }
            set
            {
                lives = value;
                stringLives = "x" + lives.ToString();
            }
        }

        static public string StringLives
        {
            get
            {
                return stringLives;
            }
        }

        static public float MaxShield
        {
            get
            {
                return maxShield;
            }
            set
            {
                maxShield = value;
                CurrentShield = maxShield;
            }
        }

        static private float CurrentShield
        {
            get { return currentShield; }
            set
            {
                currentShield = value;
                percentShield = (int)((currentShield / maxShield) * 100);
                stringShield = percentShield.ToString() + "%";
            }
        }

        static public float PercentShield
        {
            get
            {
                return percentShield;
            }
        }

        static public string StringShield
        {
            get
            {
                return stringShield;
            }
        }

        static public int MainWeaponPower
        {
            get
            {
                return mainWeaponPower;
            }
            set
            {
                mainWeaponPower = value;
                stringMainWeaponPower = mainWeaponPower.ToString();
            }
        }

        static public string StringMainWeaponPower
        {
            get
            {
                return stringMainWeaponPower;
            }
        }

        static public float Speed
        {
            get
            {
                return speed;
            }
            set
            {
                speed = value;
                velocity = new Vector2(Speed);
            }
        }

        static public int ExpansionValue
        {
            get
            {
                return revivalExpansionValue;
            }
        }

        static public Vector2 Velocity
        {
            get
            {
                return velocity;
            }
        }

        static public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
                //Set center based on new position.
                Center = position;
            }
        }

        static public Vector2 Center
        {
            get
            {
                return center;
            }
            private set
            {
                center.X = value.X;// +texture.Width / 2.0f;
                center.Y = value.Y;// +texture.Height / 2.0f;
            }
        }

        static public Vector2 StartingPosition
        {
            get
            {
                return startingPosition;
            }
        }

        static public PrimaryWeapon PrimaryWeapon
        {
            get
            {
                return primaryWeapon;
            }
        }

        static public SecondaryWeapon SecondaryWeapon
        {
            get
            {
                return secondaryWeapon;
            }
        }

        static public PowerUpType PowerUpType
        {
            get
            {
                return powerUpType;
            }
            set
            {
                powerUpActive = true;
                if (value == PowerUpType.RapidFire)
                {
                    primaryWeapon.SetCoolDown(0.1f);
                    rapidFireCooldown = 10;
                }
                else if (value == PowerUpType.TriCannon)
                {
                    powerUpType = value;
                    primaryWeapon = triCannon;
                    if(rapidFireCooldown > 0)
                        primaryWeapon.SetCoolDown(0.1f);
                    else
                        primaryWeapon.SetCoolDown(0.2f);
                    powerUpCooldown = 10.0f;
                }
                else if (value == PowerUpType.QuintuCannon)
                {
                    powerUpType = value;
                    primaryWeapon = quintuCannon;
                    if (rapidFireCooldown > 0)
                        primaryWeapon.SetCoolDown(0.1f);
                    else
                        primaryWeapon.SetCoolDown(0.2f);
                    powerUpCooldown = 10.0f;
                }
                else if (value == PowerUpType.Invincibility)
                {
                    invincible = true;
                    invincibilityCooldown = 10;
                }
            }
        }

        static public Texture2D Texture
        {
            get
            {
                return texture;
            }
        }

        static public Color[] TextureData
        {
            get
            {
                return textureData;
            }
        }

        static public Matrix Rotation
        {
            get
            {
                return rotation;
            }
        }

        static public Rectangle BoundingRectangle
        {
            get
            {
                return boundingRectangle;
            }
        }

        static public ScoreSystem ScoreSystem
        {
            get
            {
                return scoreSystem;
            }
        }
        static public SaveGameData SaveGameData
        {
            get
            {
                return saveData;
            }
            set
            {
                saveData = value;
            }
        }
    }
}
