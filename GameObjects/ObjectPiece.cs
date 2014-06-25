using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Aero
{
    class ObjectPiece
    {
        Vector2 position;
        Texture2D texture;
        float speed = 400;
        //Vector2 direction;
        Vector2 velocity;

        public ObjectPiece(Texture2D texture, Vector2 direction)
        {
            this.texture = texture;
            velocity = direction * speed;
        }

        public void Init(Vector2 position)
        {
            this.position = position;
        }

        public void Update(TimeSpan elapsedTime)
        {
            position += velocity * (float)elapsedTime.TotalSeconds;
        }

        public void Draw()
        {
            AeroGame.SpriteBatch.Draw(texture, position, Color.White);
        }

        public Texture2D Texture
        {
            get
            {
                return texture;
            }
        }
    }
}
