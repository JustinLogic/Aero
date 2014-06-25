using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Aero
{
    class PowerUpQuintuCannon : PowerUp
    {
        public PowerUpQuintuCannon()
            : base()
        {
            if (AeroGame.lagTest)
                texture = AeroGame.LoadTextureStream("AeroPowerUp_QuintuCannon");
            else
                texture = AeroGame.ContentManager.Load<Texture2D>("Textures\\AeroPowerUp_QuintuCannon");
            type = PowerUpType.QuintuCannon;
            textureData = new Color[texture.Width * texture.Height];
            texture.GetData(textureData);
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
        }
    }

}
