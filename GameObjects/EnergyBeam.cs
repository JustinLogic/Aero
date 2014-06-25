using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Aero
{
    class EnergyBeam : AeroObject
    {
        Color color;

        public EnergyBeam(bool friendly)
        {
            if (AeroGame.lagTest)
                texture = AeroGame.LoadTextureStream("LaserBeam");
            else
                texture = AeroGame.ContentManager.Load<Texture2D>("Textures\\LaserBeam");
            textureData = new Color[texture.Width * texture.Height];
            texture.GetData(textureData);
            scale = 2;
            origin = new Vector2(0, texture.Height / 2);
            color = Color.White;
            this.friendly = friendly;
        }

        public override void Update(TimeSpan timeSpan)
        {
            base.Update(timeSpan);
        }

        public override void Draw()
        {
            AeroGame.SpriteBatch.Draw(texture, position, null, color, theta, origin, scale, SpriteEffects.None, 0);
        }

        public void Spawn(Vector2 position, double firingAngle)
        {
            theta = (float)firingAngle;
            //this.position = position;
            this.position.X = position.X + (texture.Height / 2);
            this.position.Y = position.Y;
            alive = true;
            SetRotation();
            Level.activeObjects.Add(this);
        }
    }
}
