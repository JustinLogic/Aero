using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Aero
{
    class BeamCannon : SecondaryWeapon
    {
        EnergyBeam beam;
        float firingCoolDown = 5.0f;

        public BeamCannon(bool friendly)
        {
            beam = new EnergyBeam(friendly);
            projectiles = beam;
            coolDown = 0;
            coolDownLimit = 10.0f;
        }

        public override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);
            if (beam.Alive)
            {
                firingCoolDown -= (float)elapsedTime.TotalSeconds;
                if (firingCoolDown > 0)
                    firingCoolDown = 5.0f;
            }
        }

        public override void Fire(double firingAngle)
        {
            beam.Spawn(center, firingAngle);
        }
    }
}
