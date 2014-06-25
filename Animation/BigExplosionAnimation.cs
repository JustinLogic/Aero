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
    class BigExplosionAnimation
    {
        private bool active;
        private float coolDown;
        private LargeExplosionParticleSystem explosion;
        private LargeExplosionSmokeParticleSystem explosionSmoke;
        SoundEffect soundEffect;

        public BigExplosionAnimation()
        {
            active = false;
            coolDown = 2.0f;
            explosion = new LargeExplosionParticleSystem(1);
            explosionSmoke = new LargeExplosionSmokeParticleSystem(1);
            explosion.Initialize();
            explosion.LoadContent();
            explosionSmoke.Initialize();
            explosionSmoke.LoadContent();
            soundEffect = AeroGame.ContentManager.Load<SoundEffect>("Sounds\\explosion");
        }

        public void Update(TimeSpan elapsedTime)
        {
            coolDown -= (float)elapsedTime.TotalSeconds;
            if (coolDown < 0)
            {
                End();
            }
            explosion.Update(elapsedTime);
            explosionSmoke.Update(elapsedTime);
        }

        public void Draw()
        {
            explosion.Draw();
            explosionSmoke.Draw();
        }

        public void Start(Vector2 position)
        {
            active = true;
            explosion.AddParticles(position);
            explosionSmoke.AddParticles(position);
            soundEffect.Play();
        }

        public void End()
        {
            active = false;
            coolDown = 2.5f;
            //soundEffect.Dispose();
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
