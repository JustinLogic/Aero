using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Aero
{
    class SecondaryWeapon
    {
        protected float coolDown;
        protected float coolDownLimit;
        protected Vector2 center;
        protected float firingSpeed;
        protected AeroObject projectiles;

        public SecondaryWeapon()
        {
            center = Vector2.Zero;
        }

        public virtual void Update(TimeSpan elapsedTime)
        {
            if (coolDown <= coolDownLimit)
                coolDown += (float)elapsedTime.TotalSeconds;
        }

        public virtual void Fire(double firingAngle)
        {
        }

        public virtual void SetCoolDown(float value)
        {
            coolDownLimit = value;
        }

        public float CoolDown
        {
            get
            {
                return coolDown;
            }
        }

        public float CoolDownLimit
        {
            get
            {
                return coolDownLimit;
            }
            set
            {
                coolDownLimit = value;
            }
        }

        public Vector2 Center
        {
            get
            {
                return center;
            }
            set
            {
                center = value;
            }
        }

        public AeroObject Projectile
        {
            get
            {
                return projectiles;
            }
        }
    }
}
