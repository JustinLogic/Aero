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
    class LargeExplosionAnimation
    {
        private Texture2D textureToExplode;
        private Color[] textureToExplodeData;
        private Vector2 startPosition;
        private SmallExplosionAnimation[] explosions;
        private const int maxExplosions = 15;
        private float spawnExplosionCoolDown;
        private float coolDown;
        private bool active;
        BigExplosionAnimation bigExplosion;

        public LargeExplosionAnimation(Texture2D textureToExplode)
        {
            this.textureToExplode = textureToExplode;
            textureToExplodeData = new Color[textureToExplode.Width * textureToExplode.Height];
            this.textureToExplode.GetData(textureToExplodeData);
            startPosition = Vector2.Zero;
            spawnExplosionCoolDown = 0;
            coolDown = 5.0f;
            active = false;
            explosions = new SmallExplosionAnimation[maxExplosions];
            for(int i = 0; i < maxExplosions; i++)
            {
                explosions[i] = new SmallExplosionAnimation();
            }
            bigExplosion = new BigExplosionAnimation();
        }

        public void Update(TimeSpan elapsedTime)
        {
            spawnExplosionCoolDown -= (float) elapsedTime.TotalSeconds;
            coolDown -= (float) elapsedTime.TotalSeconds;

            if (coolDown > 1)
            {
                if (spawnExplosionCoolDown <= 0)
                {
                    spawnExplosionCoolDown = 0.2f;
                    foreach (SmallExplosionAnimation ex in explosions)
                    {
                        if (!ex.Active)
                        {
                            int x = AeroGame.Random.Next((int)startPosition.X, (int)startPosition.X + textureToExplode.Width);
                            int y = AeroGame.Random.Next((int)startPosition.Y, (int)startPosition.Y + textureToExplode.Height);
                            ex.Start(new Vector2(x, y));
                            break;
                        }

                    }
                }
            }
            else
            {
                if (!bigExplosion.Active)
                    bigExplosion.Start(new Vector2(startPosition.X + textureToExplode.Width / 2, startPosition.Y + textureToExplode.Height / 2));
                bigExplosion.Update(elapsedTime);
            }
            foreach (SmallExplosionAnimation ex in explosions)
            {
                if (ex.Active)
                    ex.Update(elapsedTime);
            }
            if (coolDown < 0)
                End();
        }

        public void Draw()
        {
            foreach (SmallExplosionAnimation ex in explosions)
            {
                if (ex.Active)
                    ex.Draw();
            }
            if (coolDown <= 1)
                bigExplosion.Draw();
        }

        public void Start(Vector2 startPosition)
        {
            this.startPosition = startPosition;
            active = true;
            coolDown = 5.0f;
        }

        public void End()
        {
            active = false;
            spawnExplosionCoolDown = 0;
        }

        public bool Active
        {
            get
            {
                return active;
            }
        }

        public bool Done
        {
            get
            {
                return coolDown <= 0;
            }
        }

        public float CoolDown
        {
            get
            {
                return coolDown;
            }
        }

    }
}
