using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Aero
{
    class FragmentationBomb : AeroObject
    {
        Round[] bullets;
        const int maxBullets = 12;
        Color color;
        float timer;
        float timerMax = -1;
        bool timerActive = false;

        public FragmentationBomb(bool friendly)
        {
            if (AeroGame.lagTest)
                texture = AeroGame.LoadTextureStream("Particle16");
            else
                texture = AeroGame.ContentManager.Load<Texture2D>("Textures\\Particle16");
            textureData = new Color[texture.Width * texture.Height];
            texture.GetData(textureData);
            if (friendly)
                color = Color.Yellow;
            else
                color = Color.DarkViolet;
            bullets = new Round[maxBullets];
            for(int i = 0; i < maxBullets; i++)
                bullets[i] = new Round(color, friendly);
            speed = 400;
            scale = 2;
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            this.friendly = friendly;
        }

        public FragmentationBomb(bool friendly, float timer)
        {
            if (AeroGame.lagTest)
                texture = AeroGame.LoadTextureStream("Particle16");
            else
                texture = AeroGame.ContentManager.Load<Texture2D>("Textures\\Particle16");
            textureData = new Color[texture.Width * texture.Height];
            texture.GetData(textureData);
            if (friendly)
                color = Color.Yellow;
            else
                color = Color.DarkViolet;
            bullets = new Round[maxBullets];
            for (int i = 0; i < maxBullets; i++)
                bullets[i] = new Round(color, friendly);
            speed = 400;
            scale = 2;
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            this.friendly = friendly;
            timerMax = timer;
            if (timerMax > -1)
                timerActive = true;
        }

        public override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);
            position.Y -= velocity.Y * (float)elapsedTime.TotalSeconds;
            position.X -= velocity.X * (float)elapsedTime.TotalSeconds;
            if (timerActive)
            {
                timer -= (float)elapsedTime.TotalSeconds;
                if (timer <= 0)
                    Kill();
            }
        }

        public void Spawn(Vector2 center, double firingAngle)
        {
            position.X = center.X + texture.Width / 2;
            position.Y = center.Y + texture.Height / 2;
            velocity = new Vector2((float)Math.Cos(firingAngle), (float)Math.Sin(firingAngle)) * speed;
            alive = true;
            theta = (float)firingAngle;
            SetRotation();
            timer = timerMax;
            Level.activeObjects.Add(this);
        }

        public override void Kill()
        {
            alive = false;
            float firingAngle = 0;
            for (int i = 0; i < bullets.Length; firingAngle += ((float)Math.PI / 6), i++)
            {
                bullets[i].Spawn(center, new Vector2((float)Math.Cos(firingAngle),(float)Math.Sin(firingAngle)) * 300, firingAngle);
            }
        }

        public override void Draw()
        {
            AeroGame.SpriteBatch.Draw(texture, position, null, color, theta, origin, scale, SpriteEffects.None, 0);
        }
    }
}
