using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Aero
{
    class TwinOne : Boss
    {
        double firingAngle;
        SoundEffect soundFireBullet;
        int damageTaken;//count damage taken before seperation
        bool seperated;

        public TwinOne()
            : base()
        {
            if (AeroGame.lagTest)
                texture = AeroGame.LoadTextureStream("TwinBossLeft");
            else
                texture = AeroGame.ContentManager.Load<Texture2D>("Textures\\TwinBossLeft");
            textureData = new Color[texture.Width * texture.Height];
            texture.GetData(textureData);
            soundFireBullet = AeroGame.ContentManager.Load<SoundEffect>("Sounds\\EnemyLaser");
            //origin = new Vector2(texture.Width / 2, texture.Height / 2);
            position = new Vector2(0, texture.Height * -1.0f);
            center = new Vector2(position.X + texture.Width / 2.0f, position.Y);
            mainWeapon = new PrimaryWeapon[1];
            mainWeapon[0] = new TriCannon(false);
            mainWeapon[0].SetCoolDown(1.0f);
            explodingAnimation = new LargeExplosionAnimation(texture);
            health = 1000;
            firingAngle = 0;
            seperated = false;
        }

        public override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);
            if (alive)
            {
                if (!exploding)
                {
                    Evade(speed * (float)elapsedTime.TotalSeconds);
                }
                mainWeapon[0].Update(elapsedTime);
                center.X -= 70;
                mainWeapon[0].Center = center;
                if (mainWeapon[0].CoolDown <= 0)
                {
                    double dx = center.X - Player.Center.X;
                    double dy = center.Y - Player.Center.Y;
                    firingAngle = (float)Math.Atan(dy / dx);
                    if (Player.Center.X > center.X)
                        firingAngle = firingAngle + Math.PI;
                    mainWeapon[0].Fire(firingAngle);
                    soundFireBullet.Play();
                }
            }
            else
            {
                mainWeapon[0].Update(elapsedTime);
            }
        }

        private void Evade(float velX)
        {
            float x = position.X;
            x += velX;
            if (x <= 0 || x >= AeroGame.Graphics.GraphicsDevice.Viewport.Width - texture.Width)
                speed *= -1;
            Position = new Vector2(x, position.Y);
        }

        public int Damage
        {
            get
            {
                return damageTaken;
            }
        }

        public void Seperate()
        {
            seperated = true;
        }

        public override void Kill()
        {
            alive = false;
            exploding = true;
            explodingAnimation.Start(position);
            Player.ScoreSystem.Add(this);
        }
    }
}
