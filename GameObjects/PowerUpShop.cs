using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Aero
{
    class PowerUpShop : PowerUp
    {
        public PowerUpShop()
            : base()
        {
            if (AeroGame.lagTest)
                texture = AeroGame.LoadTextureStream("AeroPowerUp_Shop");
            else
                texture = AeroGame.ContentManager.Load<Texture2D>("Textures\\AeroPowerUp_Shop");
            type = PowerUpType.Shop;
            textureData = new Color[texture.Width * texture.Height];
            texture.GetData(textureData);
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }
    }
}
