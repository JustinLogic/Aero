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
    class PowerUp : AeroObject
    {
        

        public PowerUp()
            : base()
        {
            alive = false;
            position = Vector2.Zero;
            velocity = new Vector2( 0, 200.0f);
            theta = 0;
        }

        public override void Update(TimeSpan elapsedTime)
        {
            position.Y += velocity.Y * (float)elapsedTime.TotalSeconds;
            base.Update(elapsedTime);
        }

        public override void Draw()
        {
            if (alive)
            {
                AeroGame.SpriteBatch.Draw(texture, position, null, Color.White, -theta, origin, scale, SpriteEffects.None, 0);
            }
        }

        public void spawn(Vector2 position)
        {
            alive = true;
            this.position = position;
        }

        public override void Kill()
        {
            alive = false;
        }
    }
}
