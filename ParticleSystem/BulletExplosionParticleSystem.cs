#region Using Statements
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Aero
{
    class BulletExplosionParticleSystem : ParticleSystem
    {
        private float bulletAngle;

        public float BulletAngle
        {
            get
            {
                return bulletAngle;
            }
            set
            {
                bulletAngle = value;
            }
        }

        public BulletExplosionParticleSystem(int howManyEffects, Color particleColor)
            : base(howManyEffects)
        {
            this.particleColor = particleColor;
            bulletAngle = 0;
        }

        /// <summary>
        /// Set up the constants that will give this particle system its behavior and
        /// properties.
        /// </summary>
        protected override void InitializeConstants()
        {
            textureFilename = "Particle16";

            // less initial speed than the explosion itself
            minInitialSpeed = 100;
            maxInitialSpeed = 200;

            // acceleration is negative, so particles will accelerate away from the
            // initial velocity.  this will make them slow down, as if from wind
            // resistance. we want the smoke to linger a bit and feel wispy, though,
            // so we don't stop them completely like we do ExplosionParticleSystem
            // particles.
            minAcceleration = 0;
            maxAcceleration = 0;

            // explosion smoke lasts for longer than the explosion itself, but not
            // as long as the plumes do.
            minLifetime = 0.5f;
            maxLifetime = 1.0f;

            minScale = 0.25f;
            maxScale = 0.5f;

            minNumParticles = 10;
            maxNumParticles = 20;

            minRotationSpeed = -MathHelper.PiOver4;
            maxRotationSpeed = MathHelper.PiOver4;


            blendState = BlendState.AlphaBlend;

            //DrawOrder = AlphaBlendDrawOrder;
        }

        protected override Vector2 PickRandomDirection()
        {
            float angle = AeroGame.RandomBetween(-MathHelper.Pi / 6, (MathHelper.Pi) / 6) + bulletAngle -MathHelper.Pi;
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }
    }
}
