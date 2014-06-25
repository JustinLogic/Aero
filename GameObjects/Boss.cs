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
    class Boss : AeroObject
    {
        //private int difficultyLevel;
        protected float activateCooldown;
        protected float shatterCooldown = -1;
        protected PrimaryWeapon[] mainWeapon;
        protected SecondaryWeapon[] secondaryWeapon;
        protected LargeExplosionAnimation explodingAnimation;
        protected ObjectPiece[] pieces;
        protected SoundEffect soundFireBullet;
        protected SoundEffect soundFireBomb;

        public Boss()
            : base()
        {
            velocity = new Vector2(0, 70.0f);
            alive = false;
            exploding = false;
            activateCooldown = 4.0f;
            theta = 0;
            
        }

        public override void Draw()
        {
            //AeroGame.SpriteBatch.Begin();
            if (alive)
            {
                //AeroGame.SpriteBatch.Draw(texture, position, color);
                AeroGame.SpriteBatch.Draw(texture, position, null, color, -theta, origin, scale, SpriteEffects.None, 0);
                //AeroGame.SpriteBatch.Draw(Texture, Position, null, Color.White, theta, new Vector2(texture.Width / 2, texture.Height / 2), 1, SpriteEffects.None, 0);
            }
            else if (!explodingAnimation.Done)
            {
                //AeroGame.SpriteBatch.Draw(Texture, position, null, Color.White, 0, Vector2.Zero, shrink, SpriteEffects.None, 1);
                AeroGame.SpriteBatch.Draw(texture, new Vector2(position.X + (((1-scale) * texture.Width) / 2), position.Y + (((1-scale) * texture.Height) / 2)), 
                    null, color, -theta, origin, scale, SpriteEffects.None, 0);
                explodingAnimation.Draw();
            }
            //Iterate through bossrounds to be drawn.
            //foreach (Round r in mainWeapon.Rounds)
            //{
            //    if (r.Alive || r.Exploding)
            //    {
            //        //Draw fired rounds.
            //        r.Draw();
            //    }
            //}
            //AeroGame.SpriteBatch.End();
        }

        public override void Update(TimeSpan elapsedTime)
        {
            if (!exploding && alive)
                if (position.Y < 0)
                    position.Y += velocity.Y * (float)elapsedTime.TotalSeconds;
            if (exploding)
            {
                //position.X += 70.0f * (float)elapsedTime.TotalSeconds;
                //position.Y += 40.0f * (float)elapsedTime.TotalSeconds;
                //theta -= 0.5f * (float)elapsedTime.TotalSeconds;
                scale -= 0.05f * (float)elapsedTime.TotalSeconds;
                if (explodingAnimation.Active)
                    explodingAnimation.Update(elapsedTime);
                else if (shatterCooldown > 0)
                {
                    shatterCooldown -= (float)elapsedTime.TotalSeconds;
                    if (shatterCooldown <= 0)
                        exploding = false;
                }
                if(explodingAnimation.CoolDown <= 1 && shatterCooldown == -1)
                {
                    shatterCooldown = 2.0f;
                    //exploding = false;
                }
            }
            base.Update(elapsedTime);
        }

        public void Spawn(Vector2 position)
        {
            alive = true;
            activateCooldown = 4.0f;
            this.position = position;
        }

        public override void Kill()
        {
            alive = false;
            exploding = true;
            explodingAnimation.Start(position);
            Level.InitLevelEnd();
            base.Kill();
        }

        public float CalculateFiringAngle(Vector2 a, Vector2 b)
        {
            double dx = a.X - b.X;
            double dy = a.Y - b.Y;
            float firingAngle = (float)Math.Atan(dy / dx);
            if (b.X > a.X)
                firingAngle = firingAngle + (float)Math.PI;
            return firingAngle;
        }

        public void explode(Texture2D loadedTexture)
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

        //public Texture2D Texture
        //{
        //    get
        //    {
        //        return texture;
        //    }
        //    set
        //    {
        //        texture = value;
        //    }
        //}

        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position.X = value.X;// -16;
                position.Y = value.Y;
                //Set center based on new position.
                Center = position;
            }
        }

        public Vector2 Center
        {
            get
            {
                return center;
            }
            private set
            {
                center.X = value.X + (texture.Width / 2.0f);
                center.Y = value.Y + (texture.Height / 2.0f);
            }
        }

        public int Health
        {
            get
            {
                return health;
            }
        }

        /*public PrimaryWeapon MainWeapon
        {
            get
            {
                return mainWeapon;
            }
        }*/
    }
}
