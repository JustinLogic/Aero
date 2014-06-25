using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Aero
{
    class QuintuCannon : PrimaryWeapon
    {
        //Vector2 Degree90 = new Vector2((float)Math.Cos(Math.PI / 2), (float)Math.Sin(Math.PI / 2));
        //Vector2 Degree30 = new Vector2((float)Math.Cos(Math.PI / 6), (float)Math.Sin(Math.PI / 6));
        //Vector2 Degree150 = new Vector2((float)Math.Cos(5 * Math.PI / 6), (float)Math.Sin(5 * Math.PI / 6));
        //Vector2 Degree60 = new Vector2((float)Math.Cos(Math.PI / 3), (float)Math.Sin(Math.PI / 3));
        //Vector2 Degree120 = new Vector2((float)Math.Cos(2 * Math.PI / 3), (float)Math.Sin(2 * Math.PI / 3));
        float degree30 = MathHelper.Pi / 6;
        float degree60 = MathHelper.Pi / 3;

        public QuintuCannon(bool roundsFriendly)
            : base()
        {
            if (roundsFriendly)
                roundColor = Color.DarkRed;
            else
                roundColor = Color.DarkViolet;
            coolDown = originalCoolDown = 0.5f;
            firingSpeed = 300.0f;
            for (int i = 0; i < maxRounds; i++)
                rounds[i] = new Round(roundColor, roundsFriendly);
        }

        public override void Fire(double firingAngle)
        {
            int n = 0;
            Vector2 direction;// = new Vector2((float)Math.Cos(firingAngle), (float)Math.Sin(firingAngle));
            //firingSpeed.X = firingSpeed.Y * (float)(1 / Math.Tan(firingAngle));
            foreach (Round r in rounds)
            {
                if (!r.Active && n == 0)
                {
                    direction = new Vector2((float)Math.Cos(firingAngle), (float)Math.Sin(firingAngle));
                    r.Spawn(center, firingSpeed * direction, (float)firingAngle);
                    n++;
                }
                else if (!r.Active && n == 1)
                {
                    direction = new Vector2((float)Math.Cos(firingAngle + degree30), (float)Math.Sin(firingAngle + degree30));
                    r.Spawn(center, firingSpeed * direction, (float)firingAngle + (MathHelper.Pi / 6));
                    n++;
                }
                else if (!r.Active && n == 2)
                {
                    direction = new Vector2((float)Math.Cos(firingAngle - degree30), (float)Math.Sin(firingAngle - degree30));
                    r.Spawn(center, firingSpeed * direction, (float)firingAngle - (MathHelper.Pi / 6));
                    n++;
                }
                else if (!r.Active && n == 3)
                {
                    direction = new Vector2((float)Math.Cos(firingAngle + degree60), (float)Math.Sin(firingAngle + degree60));
                    r.Spawn(center, firingSpeed * direction, (float)firingAngle + (MathHelper.Pi / 3));
                    n++;
                }
                else if (!r.Active && n == 4)
                {
                    direction = new Vector2((float)Math.Cos(firingAngle - degree60), (float)Math.Sin(firingAngle - degree60));
                    r.Spawn(center, firingSpeed * direction, (float)firingAngle - (MathHelper.Pi / 3));
                    coolDown = originalCoolDown;
                    return;
                }
            }
        }
    }
}
