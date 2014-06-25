using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aero
{
    class MissleCannon : SecondaryWeapon
    {
        Missle[] missiles;

        public MissleCannon(bool friendly)
        {
            missiles = new Missle[10];
            for (int i = 0; i < 10; i++)
            {
                missiles[i] = new Missle(friendly);
            }
            coolDown = 0;
            coolDownLimit = 10.0f;
        }

        public override void Fire(double firingAngle)
        {
            int targetId = 0;
            AeroObject target = null;
            List<AeroObject> list = Level.activeObjects;
            for (int i = 0; i < 10; i++)
            {
                while (targetId < list.Count)
                {
                    if (Level.activeObjects[targetId].Friendly == false)
                    {
                        target = list[targetId];
                        targetId++;
                        break;
                    }
                    targetId++;
                }
                missiles[i].Spawn(center, (float)firingAngle, target);
                coolDown = 0;
            }
            //bool found = false;
            //for (int i = 0; i < 10; i++)
            //{
            //    while (!found)
            //    {
            //        if (Level.activeObjects[targetId].Friendly == false)
            //        {
            //            target = list[targetId];
            //            found = true;
            //        }
            //    }
            //    missiles[0].Spawn(center, (float)firingAngle, target);
            //    coolDown = 0;
            //}
        }
    }
}
