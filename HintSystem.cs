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
    class HintSystem
    {
        List<String> hints;
        Vector2 position;
        float speed;
        float newHintCooldown;
        Random rand;
        int index;

        public HintSystem()
        {
            rand = new Random();
            position = new Vector2(AeroGame.Graphics.GraphicsDevice.Viewport.Width, (int)(AeroGame.Graphics.GraphicsDevice.Viewport.Height * 0.93));
            speed = 300.0f;
            newHintCooldown = 5.0f;
            hints = new List<string>();
            hints.Add("When the bar at the bottom of the screen turns green, you can fire your special weapon.");
            hints.Add("You can upgrade each part once per Shop Screen.");
            hints.Add("Try to upgrade your weapon once per level.");
            hints.Add("A special Shop Screen Powerup has a chance of spawning when the final boss respawns.");
            hints.Add("Don't shoot the falling Powerups! You'll destroy them!");
            hints.Add("Upgrading your weapon increases firepower!");
            hints.Add("Upgrading your shield increases shield strength and heals it to %100!");
            hints.Add("Upgrading your engine increases movement speed!");
            index = rand.Next(hints.Count);
        }

        public void Update(TimeSpan elapsedTime)
        {
            position.X -= speed * (float)elapsedTime.TotalSeconds;
            if (position.X < 0)
            {
                newHintCooldown -= (float)elapsedTime.TotalSeconds;
                if (newHintCooldown < 0)
                {
                    newHintCooldown = 5.0f;
                    position.X = AeroGame.Graphics.GraphicsDevice.Viewport.Width;
                    index = rand.Next(hints.Count);
                }
            }
        }

        public void Draw(SpriteFont font)
        {
            AeroGame.SpriteBatch.Begin();

            AeroGame.SpriteBatch.DrawString(font, hints[index], position, Color.White);

            AeroGame.SpriteBatch.End();
        }
    }
}
