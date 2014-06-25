#region Using Statements
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Aero
{
    class RevivalParticleSystem : ParticleSystem
    {
        public RevivalParticleSystem(int howManyEffects)
            : base(howManyEffects)
        {
        }

        protected override void InitializeConstants()
        {
            textureFilename = "Particle16";

            // less initial speed than the explosion itself
            minInitialSpeed = 1;
            maxInitialSpeed = 100;

            // acceleration is negative, so particles will accelerate away from the
            // initial velocity.  this will make them slow down, as if from wind
            // resistance. we want the smoke to linger a bit and feel wispy, though,
            // so we don't stop them completely like we do ExplosionParticleSystem
            // particles.
            minAcceleration = 0;
            maxAcceleration = 0;

            // explosion smoke lasts for longer than the explosion itself, but not
            // as long as the plumes do.
            minLifetime = 1.0f;
            maxLifetime = 2.0f;

            minScale = 0.5f;
            maxScale = 1.0f;

            minNumParticles = 10;
            maxNumParticles = 20;

            minRotationSpeed = -MathHelper.PiOver4;
            maxRotationSpeed = MathHelper.PiOver4;

            // first, call PickRandomDirection to figure out which way the particle
            // will be moving. velocity and acceleration's values will come from this.
            direction = PickRandomDirection();

            blendState = BlendState.AlphaBlend;
        }

        protected override Vector2 PickRandomDirection()
        {
            float rand = AeroGame.Random.Next(2);
            float angle = rand == 0 ? 0 : MathHelper.Pi;
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }
    }
}
