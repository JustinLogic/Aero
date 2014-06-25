using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Aero
{
    class PowerUpTriCannon : PowerUp
    {
        public PowerUpTriCannon()
            : base()
        {
            if (AeroGame.lagTest)
                texture = AeroGame.LoadTextureStream("AeroPowerUp_TriCannon");
            else
                texture = AeroGame.ContentManager.Load<Texture2D>("Textures\\AeroPowerUp_TriCannon");
            type = PowerUpType.TriCannon;
            textureData = new Color[texture.Width * texture.Height];
            texture.GetData(textureData);
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }
    }

}
