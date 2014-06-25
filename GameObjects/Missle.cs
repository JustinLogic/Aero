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
    class Missle : AeroObject
    {
        SmallExplosionAnimation explosionAnimation;
        ExplosionSmokeParticleSystem smoke;
        AeroObject target;
        private float goalTheta;
        private const float maxTurnPositive = (float)Math.PI / 4;
        private const float maxTurnNegative = (float)-Math.PI / 4;
        private const float maxTurnSpeed = (float)Math.PI / 2;

        public Missle(bool friendly)
            : base()
        {
            if (AeroGame.lagTest)
                texture = AeroGame.LoadTextureStream("Particle16");
            else
                texture = AeroGame.ContentManager.Load<Texture2D>("Textures\\Particle16");
            textureData = new Color[texture.Width * texture.Height];
            texture.GetData(textureData);
            position = new Vector2();
            velocity = new Vector2(0, 300.0f);///////////
            health = 1;
            alive = false;
            exploding = false;
            this.friendly = friendly;
            explosionAnimation = new SmallExplosionAnimation();
            smoke = new ExplosionSmokeParticleSystem(1);
            smoke.Initialize();
            smoke.LoadContent();
            speed = 300;
        }

        public void Spawn(Vector2 center, float firingAngle, AeroObject target)
        {
            position.X = center.X + texture.Width / 2;
            position.Y = center.Y + texture.Height / 2;
            theta = firingAngle;
            this.target = target;
            alive = true;
            SetRotation();
            Level.activeObjects.Add(this);
        }

        public override void Kill()
        {
            alive = false;
            exploding = true;
            explosionAnimation.Start(center);
            base.Kill();
        }

        public override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);
            if(!target.Alive)//target not alive
            {
                GetNewTarget();
                //Update(elapsedTime);
            }
            if (alive)
            {
                //Calculate Angle Towards Target
                float maxTurn = maxTurnSpeed * (float)elapsedTime.TotalSeconds;
                goalTheta = (target.BoundingRectangle.Center.X - center.X) / (target.BoundingRectangle.Center.Y - center.Y);
                if (theta < goalTheta)
                    theta = (goalTheta - theta) > maxTurn ? theta + maxTurn : goalTheta;
                else
                    theta = -(goalTheta - theta) > maxTurn ? theta - maxTurn : goalTheta;
                theta = theta > maxTurnPositive ? maxTurnPositive : theta;
                theta = theta < maxTurnNegative ? maxTurnNegative : theta;
                MathHelper.WrapAngle(theta);
                velocity.X = theta * velocity.Y;
                velocity.X = velocity.X > 300 ? 300 : velocity.X;
                velocity.X = velocity.X < -300 ? -300 : velocity.X;
                //Move Position
                AeroObject aeroObject = null;
                bool containsMissile = false;
                List<AeroObject> list = Level.activeObjects;
                for (int i = 0; i < list.Count; i++)
                {
                    aeroObject = list[i];
                    if (aeroObject is Missle && this.boundingRectangle.Contains(aeroObject.BoundingRectangle) && aeroObject != this)
                    {
                        MoveFromMissile(aeroObject, elapsedTime);
                        containsMissile = true;
                    }
                }
                //if  (containsMissile == false)
                    MoveToTarget(elapsedTime);
                //Update Smoke
                smoke.AddParticles(position);
                smoke.Update(elapsedTime);
            }
            else if (exploding)
            {
                if (explosionAnimation.Active)
                    explosionAnimation.Update(elapsedTime);
                else
                    exploding = false;
            }
        }

        public void MoveToTarget(TimeSpan elapsedTime)
        {
            if (MathHelper.Distance(center.Y, target.BoundingRectangle.Center.Y) < speed * (float)elapsedTime.TotalSeconds)
                position.Y = target.BoundingRectangle.Center.Y;
            else
            {
                if (target.BoundingRectangle.Center.Y < center.Y)
                    position.Y -= speed * (float)elapsedTime.TotalSeconds;
                else
                    position.Y += speed * (float)elapsedTime.TotalSeconds;
            }
            if (MathHelper.Distance(position.X, target.BoundingRectangle.Center.X) < speed * (float)elapsedTime.TotalSeconds)
                center.X = target.BoundingRectangle.Center.X;
            else
            {
                if (target.BoundingRectangle.Center.X < center.X)
                    position.X -= speed * (float)elapsedTime.TotalSeconds;
                else
                    position.X += speed * (float)elapsedTime.TotalSeconds;
            }
        }

        public void MoveFromMissile(AeroObject missile, TimeSpan elapsedTime)
        {
            if (MathHelper.Distance(center.Y, missile.BoundingRectangle.Center.Y) < speed * (float)elapsedTime.TotalSeconds)
                //position.Y = missile.BoundingRectangle.Center.Y;
            //else
            {
                if (missile.BoundingRectangle.Center.Y < center.Y)
                    position.Y += speed * (float)elapsedTime.TotalSeconds;
                else
                    position.Y -= speed * (float)elapsedTime.TotalSeconds;
            }
            if (MathHelper.Distance(position.X, missile.BoundingRectangle.Center.X) < speed * (float)elapsedTime.TotalSeconds)
                //center.X = missile.BoundingRectangle.Center.X;
            //else
            {
                if (missile.BoundingRectangle.Center.X < center.X)
                    position.X += speed * (float)elapsedTime.TotalSeconds;
                else
                    position.X -= speed * (float)elapsedTime.TotalSeconds;
            }
        }

        public override void Draw()
        {
            if (alive)
            {
                AeroGame.SpriteBatch.Draw(texture, position, null, Color.MediumVioletRed, theta, origin, scale, SpriteEffects.None, 0);
                //smoke.Draw();
            }
            else if (exploding)
                explosionAnimation.Draw();
        }

        private void GetNewTarget(){
            float bestDistance = 2000;
            float newDistance;
            for (int i = 0; i < Level.activeObjects.Count; i++)
            {
                if (Level.activeObjects[i].Friendly == !friendly && Level.activeObjects[i].Alive == true
                    && (Level.activeObjects[i] is Enemy || Level.activeObjects[i] is Boss))
                {
                    newDistance = Vector2.Distance(position, Level.activeObjects[i].Position);
                    if (newDistance < bestDistance)
                    {
                        bestDistance = newDistance;
                        target = Level.activeObjects[i];
                    }
                }
            }
        }

    }
}
