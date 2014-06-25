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
    class Round : AeroObject
    {
        private Vector2 originalVelocity;
        Color color;
        BulletExplosionAnimation explosionAnimation;

        public Round(Color roundColor, bool friendly)
            : base()
        {
            color = roundColor;
            if (AeroGame.lagTest)
                texture = AeroGame.LoadTextureStream("Particle16");
            else
                texture = AeroGame.ContentManager.Load<Texture2D>("Textures\\Particle16");
            textureData = new Color[texture.Width * texture.Height];
            texture.GetData(textureData);
            position = new Vector2();
            originalVelocity = new Vector2(0, 300.0f);
            velocity = new Vector2(0, 300.0f);
            health = 1;
            alive = false;
            exploding = false;
            this.friendly = friendly;
            explosionAnimation = new BulletExplosionAnimation(roundColor);
        }

        public void Spawn(Vector2 center, Vector2 firingVelocity, float firingAngle)
        {
            position.X = center.X + texture.Width / 2;
            position.Y = center.Y + texture.Height / 2;
            velocity = firingVelocity;
            theta = firingAngle;
            alive = true;
            SetRotation();
            Level.activeObjects.Add(this);
        }

        public override void Kill()
        {
            alive = false;
            exploding = true;
            velocity = originalVelocity;
            explosionAnimation.Start(center, theta);
            base.Kill();
        }

        public override void killOffScreen()
        {
            exploding = false;
            velocity = originalVelocity;
            base.killOffScreen();
        }

        public Vector2 Velocity
        {
            get
            {
                return velocity;
            }
            set
            {
                velocity = value;
            }
        }

        public Vector2 OriginalVelocity
        {
            get
            {
                return originalVelocity;
            }
        }

        public bool Exploding
        {
            get
            {
                return exploding;
            }
        }

        public override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);
            if (alive)
            {
                position.Y -= velocity.Y * (float)elapsedTime.TotalSeconds;
                //if (roundCurve == RoundCurve.None)
                //{
                    position.X -= velocity.X * (float)elapsedTime.TotalSeconds;
                //}
                //else if (roundCurve == RoundCurve.Degree45)
                //{
                //    velocity.X = velocity.Y;
                //    position.X -= velocity.X * (float)elapsedTime.TotalSeconds;
                //}
                //else if (roundCurve == RoundCurve.DegreeNegative45)
                //{
                //    velocity.X = -velocity.Y;
                //    position.X -= velocity.X * (float)elapsedTime.TotalSeconds;
                //}
                //else if (roundCurve == RoundCurve.QuintuOutside)
                //{
                //    velocity.X = (float)Math.Sqrt((double)velocity.Y * QuintuOutsideMultiple);
                //    position.X -= velocity.X * (float)elapsedTime.TotalSeconds;
                //}
                //else if (roundCurve == RoundCurve.QuintuNegativeOutside)
                //{
                //    velocity.X = (float)-Math.Sqrt((double)velocity.Y * QuintuOutsideMultiple);
                //    position.X -= velocity.X * (float)elapsedTime.TotalSeconds;
                //}
                //else if (roundCurve == RoundCurve.QuintuInside)
                //{
                //    velocity.X = (float)Math.Sqrt((double)velocity.Y * QuintuInsideMultiple);
                //    position.X -= velocity.X * (float)elapsedTime.TotalSeconds;
                //}
                //else if (roundCurve == RoundCurve.QuintuNegativeInside)
                //{
                //    velocity.X = (float)-Math.Sqrt((double)velocity.Y * QuintuInsideMultiple);
                //    position.X -= velocity.X * (float)elapsedTime.TotalSeconds;
                //}
            }
            else if (exploding)
            {
                if (explosionAnimation.Active)
                    explosionAnimation.Update(elapsedTime);
                else
                {
                    exploding = false;
                    Level.activeObjects.Remove(this);
                }
            }
            else
            {
                Level.activeObjects.Remove(this);
            }
            base.Update(elapsedTime);
        }

        public override void Draw()
        {
            if (alive)
                AeroGame.SpriteBatch.Draw(texture, position, null, color, theta, origin, scale, SpriteEffects.None, 0);
            else if (exploding)
                explosionAnimation.Draw();
        }
    }
}
