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
    class Enemy : AeroObject
    {
        protected SmallExplosionAnimation explosionAnimation;


        public Enemy()
            : base()
        {
            position = Vector2.Zero;
            alive = false;
            exploding = false;
            velocity = new Vector2(50.0f, 200.0f);
            theta = (float)Math.Tan((position.X - velocity.X) / (position.Y - velocity.Y));
            theta = MathHelper.WrapAngle(theta);
            explosionAnimation = new SmallExplosionAnimation();
        }

        public override void Update(TimeSpan elapsedTime)
        {
            if (!exploding)
            {
                position.Y += velocity.Y * (float)elapsedTime.TotalSeconds;
                position.X += velocity.X * (float)elapsedTime.TotalSeconds;
            }
            if (exploding)
            {
                if (explosionAnimation.Active)
                    explosionAnimation.Update(elapsedTime);
                else
                    exploding = false;
            }
            base.Update(elapsedTime);
        }

        public override void Draw()
        {
            //AeroGame.SpriteBatch.Begin();
            if (alive)
            {
                //Draw enemies normally.
                AeroGame.SpriteBatch.Draw(texture, position, null, color, -theta, origin, scale, SpriteEffects.None, 0);
            }
            else if (exploding)
            {
                //Draw enemies that are exploding and expand by ExpansionValue.
                explosionAnimation.Draw();
            }
            //AeroGame.SpriteBatch.End();
        }

        public virtual void spawn(Vector2 position, float playerPosX)
        {
            alive = true;
            health = maxHealth;
            this.position = position;
            //arc = maxTurnSpeed * theta;
            if (playerPosX < position.X && velocity.X >= 0)
            {
                theta *= -1.0f;
                velocity.X *= -1.0f;
            }
            else// if (playerPosX > position.X && velocity.X <= 0)
            {
                theta *= -1.0f;
                velocity.X *= -1.0f;
            }
            SetRotation();
        }

        public override void Kill()
        {
            alive = false;
            exploding = true;
            explosionAnimation.Start(center);
            base.Kill();
        }

        public void Explode(Texture2D loadedTexture)
        {
            texture = loadedTexture;
            exploding = true;
        }

        public bool Exploding
        {
            get
            {
                return exploding;
            }
        }
    }
}
