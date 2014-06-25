using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Aero
{
    class Kamacazie : Enemy
    {
        private float goalTheta;
        private const float maxTurnPositive = (float) Math.PI / 4;
        private const float maxTurnNegative = (float) -Math.PI / 4;
        private const float maxTurnSpeed = (float)Math.PI/2;

        public Kamacazie()
            : base()
        {
            if (AeroGame.lagTest)
                texture = AeroGame.LoadTextureStream("Kamikaze");
            else
                texture = AeroGame.ContentManager.Load<Texture2D>("Textures\\Kamikaze");
            textureData = new Color[texture.Width * texture.Height];
            texture.GetData(textureData);
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            velocity = new Vector2(0, 300);
            goalTheta = 0;
            health = maxHealth = 40;
        }

        public override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);
            float maxTurn = maxTurnSpeed * (float)elapsedTime.TotalSeconds;
            goalTheta = (Player.BoundingRectangle.Center.X - center.X) / (Player.BoundingRectangle.Center.Y - center.Y);
            if(theta < goalTheta)
                theta = (goalTheta - theta) > maxTurn ? theta + maxTurn : goalTheta;
            else
                theta = -(goalTheta - theta) > maxTurn ? theta - maxTurn : goalTheta;
            theta = theta > maxTurnPositive ? maxTurnPositive : theta;
            theta = theta < maxTurnNegative ? maxTurnNegative : theta;
            MathHelper.WrapAngle(theta);
            velocity.X = theta * velocity.Y;
            velocity.X = velocity.X > 300 ? 300 : velocity.X;
            velocity.X = velocity.X < -300 ? -300 : velocity.X;
        }

        
    }
}
