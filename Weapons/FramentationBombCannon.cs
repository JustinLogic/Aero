using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Aero
{
    class FragmentationBombCannon : SecondaryWeapon
    {
        FragmentationBomb bomb;

        public FragmentationBombCannon(bool friendly)
        {
            bomb = new FragmentationBomb(friendly);
            projectiles = bomb;
            coolDown = 0;
            coolDownLimit = 10.0f;
        }

        public FragmentationBombCannon(bool friendly, float timer)
        {
            bomb = new FragmentationBomb(friendly, timer);
            projectiles = bomb;
            coolDown = 0;
            coolDownLimit = 10.0f;
        }

        public override void Fire(double firingAngle)
        {
            bomb.Spawn(center, firingAngle);
            coolDown = 0;
        }
    }
}
