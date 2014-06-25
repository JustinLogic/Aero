using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Aero
{
    class StandardCannon : PrimaryWeapon
    {
        public StandardCannon(bool roundsFriendly)
            : base()
        {
            if (roundsFriendly)
                roundColor = Color.Yellow;
            else
                roundColor = Color.DarkViolet;
            coolDown = originalCoolDown = 0.5f; ;
            firingSpeed = 300.0f;
            for (int i = 0; i < maxRounds; i++)
                rounds[i] = new Round(roundColor, roundsFriendly);
        }

        public override void Fire(double firingAngle)
        {
            foreach(Round r in rounds)
            {
                if (!r.Active)
                {
                    //if (firingAngle < 0 && firingSpeed.Y > 0)
                    //    firingSpeed.Y *= -1.0f;
                    //if(firingAngle < 0 || firingAngle > 0)
                    //    firingSpeed.X = firingSpeed.Y / (float)Math.Tan(firingAngle);
                    //if (firingAngle < 0 && firingSpeed.X < 0)
                    //    firingSpeed.X *= -1.0f;
                    Vector2 direction = new Vector2((float)Math.Cos(firingAngle), (float)Math.Sin(firingAngle));
                    direction.Normalize();
                    r.Spawn(center, firingSpeed * direction, (float)firingAngle);
                    coolDown = originalCoolDown;
                    return;
                }
            }
        }
    }
}
