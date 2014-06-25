using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Aero
{
    class TriCannon : PrimaryWeapon
    {
        //Vector2 Degree90 = new Vector2((float)Math.Cos(Math.PI / 2), (float)Math.Sin(Math.PI / 2));
        //Vector2 Degree45 = new Vector2((float)Math.Cos(Math.PI / 4), (float)Math.Sin(Math.PI / 4));
        //Vector2 Degree135 = new Vector2((float)Math.Cos(3 * Math.PI / 4), (float)Math.Sin(3 * Math.PI / 4));
        float degree45 = MathHelper.PiOver4;

        public TriCannon(bool roundsFriendly)
            : base()
        {
            if (roundsFriendly)
                roundColor = Color.Orange;
            else
                roundColor = Color.DarkViolet;
            coolDown = originalCoolDown = 0.5f; ;
            firingSpeed = 300.0f;
            for (int i = 0; i < maxRounds; i++)
                rounds[i] = new Round(roundColor, roundsFriendly);
        }

        public override void Fire(double firingAngle)
        {
            int n = 0;
            //firingSpeed.X = firingSpeed.Y * (float)(1 / Math.Tan(firingAngle));
            Vector2 direction;// = new Vector2((float)Math.Cos(firingAngle), (float)Math.Sin(firingAngle));
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
                    direction = new Vector2((float)Math.Cos(firingAngle + degree45), (float)Math.Sin(firingAngle + degree45));
                    r.Spawn(center, firingSpeed * direction, (float)firingAngle + MathHelper.PiOver4);
                    n++;
                }
                else if (!r.Active && n == 2)
                {
                    direction = new Vector2((float)Math.Cos(firingAngle - degree45), (float)Math.Sin(firingAngle - degree45));
                    r.Spawn(center, firingSpeed * direction, (float)firingAngle - MathHelper.PiOver4);
                    coolDown = originalCoolDown;
                    return;
                }
            }
        }
    }
}