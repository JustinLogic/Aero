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
    class Cruiser : Enemy
    {
        public Cruiser()
            : base()
        {
            if (AeroGame.lagTest)
                texture = AeroGame.LoadTextureStream("Cruiser");
            else
                texture = AeroGame.ContentManager.Load<Texture2D>("Textures\\Cruiser");
            textureData = new Color[texture.Width * texture.Height];
            texture.GetData(textureData);
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            health = maxHealth = 30;
        }

        public override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);
        }
    }
}
