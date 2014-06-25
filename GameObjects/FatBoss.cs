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
    class FatBoss : Enemy
    {
        SoundEffect soundFireBullet;
        SoundEffect soundFireBomb;
        float firingAngle;
        protected float activateCooldown;
        protected float shatterCooldown = -1;
        protected float mouthCooldown;
        protected PrimaryWeapon[] mainWeapon;
        protected SecondaryWeapon[] secondaryWeapon;
        protected LargeExplosionAnimation explodingAnimation;
        protected Texture2D texture1;
        protected Color[] textureData1;
        protected StandardCannon standardCannon;
        protected TriCannon triCannon;
        protected QuintuCannon quintuCannon;
        protected StandardCannon standardCannon2;
        protected TriCannon triCannon2;
        protected QuintuCannon quintuCannon2;

        public FatBoss()
            : base()
        {
            if (AeroGame.lagTest)
                texture = AeroGame.LoadTextureStream("FatBoss");
            else
                texture = AeroGame.ContentManager.Load<Texture2D>("Textures\\FatBoss");
            textureData = new Color[texture.Width * texture.Height];
            texture.GetData(textureData);
            if (AeroGame.lagTest)
                texture1 = AeroGame.LoadTextureStream("FatBoss2");
            else
                texture1 = AeroGame.ContentManager.Load<Texture2D>("Textures\\FatBoss2");
            textureData1 = new Color[texture.Width * texture.Height];
            texture1.GetData(textureData);
            soundFireBullet = AeroGame.ContentManager.Load<SoundEffect>("Sounds\\EnemyLaser");
            soundFireBomb = AeroGame.ContentManager.Load<SoundEffect>("Sounds\\Bomb");
            position = new Vector2(0, texture.Height * -1.0f);
            center = new Vector2(position.X + texture.Width / 2.0f, position.Y);
            standardCannon = new StandardCannon(false);
            triCannon = new TriCannon(false);
            quintuCannon = new QuintuCannon(false);
            standardCannon2 = new StandardCannon(false);
            triCannon2 = new TriCannon(false);
            quintuCannon2 = new QuintuCannon(false);
            mainWeapon = new PrimaryWeapon[2];
            mainWeapon[0] = new StandardCannon(false);
            mainWeapon[0].SetCoolDown(1.0f);
            mainWeapon[1] = new StandardCannon(false);
            mainWeapon[1].SetCoolDown(1.0f);
            secondaryWeapon = new SecondaryWeapon[1];
            secondaryWeapon[0] = new FragmentationBombCannon(false, 1.0f);
            secondaryWeapon[0].CoolDownLimit = 9.0f;
            explodingAnimation = new LargeExplosionAnimation(texture);
            health = 100 * Player.MainWeaponPower; ;
            firingAngle = 0;
            speed = 200;
            velocity = new Vector2(0.0f, 0.0f);
            theta = 0;
            mouthCooldown = 0;
        }

        public override void Update(TimeSpan elapsedTime)
        {
            if (activateCooldown > 0)
            {
                activateCooldown -= (float)elapsedTime.TotalSeconds;
                if (activateCooldown <= 0)
                {
                    
                }
            }
            else if (alive)
            {
                Evade(speed * (float)elapsedTime.TotalSeconds);
                mainWeapon[0].Update(elapsedTime);
                mainWeapon[0].Center = new Vector2(center.X - 30, center.Y - 15);
                mainWeapon[1].Update(elapsedTime);
                mainWeapon[1].Center = new Vector2(center.X - 5, center.Y - 15);
                secondaryWeapon[0].Update(elapsedTime);
                secondaryWeapon[0].Center = center;
                if (mouthCooldown > 0)
                    mouthCooldown -= (float)elapsedTime.TotalSeconds;
                if (mainWeapon[0].CoolDown <= 0)
                {
                    firingAngle = CalculateFiringAngle(mainWeapon[0].Center, Player.BoundingRectangle.Center);
                    mainWeapon[0].Fire(firingAngle);
                    firingAngle = CalculateFiringAngle(mainWeapon[1].Center, Player.BoundingRectangle.Center);
                    mainWeapon[1].Fire(firingAngle);
                    soundFireBullet.Play();
                }
                if (secondaryWeapon[0].CoolDown >= secondaryWeapon[0].CoolDownLimit)
                {
                    firingAngle = CalculateFiringAngle(center, Player.BoundingRectangle.Center);
                    secondaryWeapon[0].Fire(firingAngle);
                    soundFireBomb.Play();
                    mouthCooldown = 0.5f;
                }
            }
            if (exploding)
            {
                if (explodingAnimation.Active)
                    explodingAnimation.Update(elapsedTime);
                else
                    exploding = false;
            }
            if (alive || exploding)
                SetRotation();
            if (isHit)
            {
                hitTimeout -= (float)elapsedTime.TotalSeconds;
                if (hitTimeout < 0)
                {
                    isHit = false;
                    hitTimeout = 0.1f;
                    color = Color.White;
                }
            }
        }

        public float CalculateFiringAngle(Vector2 a, Point b)
        {
            double dx = a.X - b.X;
            double dy = a.Y - b.Y;
            float firingAngle = (float)Math.Atan(dy / dx);
            if (b.X > a.X)
                firingAngle = firingAngle + (float)Math.PI;
            return firingAngle;
        }

        private void Evade(float velX)
        {
            float x = position.X;
            x += velX;
            if (x <= 0 || x >= AeroGame.Graphics.GraphicsDevice.Viewport.Width - texture.Width)
                speed *= -1;
            Position = new Vector2(x, position.Y);
        }

        public override void Kill()
        {
            alive = false;
            exploding = true;
            explodingAnimation.Start(center);
            Player.ScoreSystem.Add(this);
        }

        public override void Draw()
        {
            //AeroGame.SpriteBatch.Begin();
            if (alive)
            {
                //Draw enemies normally.
                if(mouthCooldown > 0)
                    AeroGame.SpriteBatch.Draw(texture1, position, null, color, -theta, origin, scale, SpriteEffects.None, 0);
                else
                    AeroGame.SpriteBatch.Draw(texture, position, null, color, -theta, origin, scale, SpriteEffects.None, 0);
            }
            else if (exploding)
            {
                //Draw enemies that are exploding and expand by ExpansionValue.
                AeroGame.SpriteBatch.Draw(texture, position, null, Color.White, -theta, origin, scale, SpriteEffects.None, 0);
                explodingAnimation.Draw();
            }
            //AeroGame.SpriteBatch.End();
        }

        public void Difficulty(int level)
        {
            switch (level)
            {
                case 0:
                    mainWeapon[0] = standardCannon;
                    mainWeapon[1] = standardCannon2;
                    mainWeapon[0].SetCoolDown(1.0f);
                    mainWeapon[1].SetCoolDown(1.0f);
                    secondaryWeapon[0].CoolDownLimit = 9.0f;
                    speed = 200;
                    break;
                case 1:
                    mainWeapon[0] = triCannon;
                    mainWeapon[1] = triCannon2;
                    mainWeapon[0].SetCoolDown(1.0f);
                    mainWeapon[1].SetCoolDown(1.0f);
                    secondaryWeapon[0].CoolDownLimit = 6.0f;
                    speed = 350;
                    break;
                case 2:
                    mainWeapon[0] = quintuCannon;
                    mainWeapon[1] = quintuCannon2;
                    mainWeapon[0].SetCoolDown(1.0f);
                    mainWeapon[1].SetCoolDown(1.0f);
                    secondaryWeapon[0].CoolDownLimit = 3.0f;
                    speed = 500;
                    break;
            }
        }

        public override void spawn(Vector2 position, float playerPosX)
        {
            base.spawn(position, playerPosX);
            health = 100 * Player.MainWeaponPower;
        }
    }
}
