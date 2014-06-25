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
    class ReviveAnimation
    {
        private bool active;
        private float coolDown;
        RevivalParticleSystem reviveSystem;

        public ReviveAnimation()
        {
            active = false;
            coolDown = 2.1f;
            reviveSystem = new RevivalParticleSystem(1);
            reviveSystem.Initialize();
            reviveSystem.LoadContent();
        }

        public void start(Vector2 startPosition)
        {
            active = true;
            reviveSystem.AddParticles(startPosition);
        }

        public void end()
        {
            active = false;
            coolDown = 2.1f;
        }

        public void Update(TimeSpan elapsedTime)
        {
            coolDown -= (float) elapsedTime.TotalSeconds;
            if (coolDown < 0)
            {
                end();
            }
            reviveSystem.Update(elapsedTime);
        }

        public void draw()
        {
            reviveSystem.Draw();
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
