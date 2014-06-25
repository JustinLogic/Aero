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
    class PrimaryWeapon
    {
        protected Color roundColor;
        protected float originalCoolDown;
        protected float coolDown;
        protected const int maxRounds = 512;//
        protected Vector2 center;//
        protected float firingSpeed;
        protected Round[] rounds;//

        public PrimaryWeapon()
        {
            rounds = new Round[maxRounds];
            center = Vector2.Zero;
        }

        public virtual void Update(TimeSpan elapsedTime)
        {
            if (coolDown >= 0)
                coolDown -= (float)elapsedTime.TotalSeconds;
            //foreach (Round r in rounds)
            //    if (r.Alive || r.Exploding)
            //        r.Update(elapsedTime);
        }

        public virtual void Fire(double firingAngle)
        {
        }

        public virtual void SetCoolDown(float value)
        {
            coolDown = originalCoolDown = value;
        }

        public Round[] Rounds
        {
            get
            {
                return rounds;
            }
        }

        public float CoolDown
        {
            get
            {
                return coolDown;
            }
        }

        public Color RoundColor
        {
            get
            {
                return roundColor;
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
    }
}
