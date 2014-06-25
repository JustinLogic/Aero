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
    class BulletExplosionAnimation
    {
        private bool active;
        private float coolDown;
        BulletExplosionParticleSystem explosion;
        SoundEffect soundHit;

        public BulletExplosionAnimation(Color particleColor)
        {
            active = false;
            coolDown = 2.5f;
            explosion = new BulletExplosionParticleSystem(1, particleColor);
            explosion.Initialize();
            explosion.LoadContent();
            soundHit = AeroGame.ContentManager.Load<SoundEffect>("Sounds\\BulletHit");
        }

        public void Update(TimeSpan elapsedTime)
        {
            coolDown -= (float) elapsedTime.TotalSeconds;
            if (coolDown < 0)
            {
                End();
            }
            explosion.Update(elapsedTime);
        }

        public void Draw()
        {
            explosion.Draw();
        }

        public void Start(Vector2 position, float theta)
        {
            active = true;
            explosion.BulletAngle = theta;
            explosion.AddParticles(position);
            soundHit.Play();
        }

        public void End()
        {
            active = false;
            coolDown = 2.5f;
            //soundHit.Dispose();
        }

        public bool Active
        {
            get
            {
                return active;
            }
        }
    }
}
