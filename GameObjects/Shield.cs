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
    class Shield : AeroObject
    {
        Texture2D texture2;
        Color[] textureData2;
        float textureFlipCooldown;
        bool textureFlip;

        public Shield()
        {
            if (AeroGame.lagTest)
                texture = AeroGame.LoadTextureStream("Sheild");
            else
                texture = AeroGame.ContentManager.Load<Texture2D>("Textures\\Sheild");
            textureData = new Color[texture.Width * texture.Height];
            texture.GetData(textureData);
            if (AeroGame.lagTest)
                texture2 = AeroGame.LoadTextureStream("Sheild2");
            else
                texture2 = AeroGame.ContentManager.Load<Texture2D>("Textures\\Sheild2");
            textureData2 = new Color[texture.Width * texture.Height];
            texture.GetData(textureData2);
            this.position = new Vector2();
            alive = false;
            exploding = false;
            this.friendly = friendly;
            textureFlipCooldown = 0.05f;
        }

        public void Spawn(Vector2 position, bool friendly)
        {
            alive = true;
            this.position = position;
            this.friendly = friendly;
            SetRotation();
            Level.activeObjects.Add(this);
        }

        public override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);
            textureFlipCooldown -= (float)elapsedTime.TotalSeconds;
            if (textureFlipCooldown < 0)
            {
                textureFlip = !textureFlip;
                textureFlipCooldown = 0.05f;
            }
        }

        public override void Kill()
        {
            alive = false;
        }

        public override void Hit(int damage)
        {
            
        }

        public override void Draw()
        {
            if (alive)
            {
                if(textureFlip)
                    AeroGame.SpriteBatch.Draw(texture, position, null, Color.White, theta, origin, scale, SpriteEffects.None, 0);
                else
                    AeroGame.SpriteBatch.Draw(texture2, position, null, Color.White, theta, origin, scale, SpriteEffects.None, 0);
            }
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
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
                center.X = value.X + texture.Width / 2.0f;
                center.Y = value.Y + texture.Height;
            }
        }
    }
}
