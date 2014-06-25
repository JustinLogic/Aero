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
    class Fighter : Enemy
    {
        private PrimaryWeapon mainWeapon;
        SoundEffect soundFireBullet;
        float firingAngle;

        public Fighter()
            : base()
        {
            if (AeroGame.lagTest)
                texture = AeroGame.LoadTextureStream("Fighter");
            else
                texture = AeroGame.ContentManager.Load<Texture2D>("Textures\\Fighter");
            textureData = new Color[texture.Width * texture.Height];
            texture.GetData(textureData);
            soundFireBullet = AeroGame.ContentManager.Load<SoundEffect>("Sounds\\EnemyLaser");
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            health = maxHealth = 30;
            mainWeapon = new StandardCannon(false);
            mainWeapon.SetCoolDown(1.0f);
            firingAngle = -MathHelper.Pi / 2;
        }

        public override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);

            if (alive)
            {
                mainWeapon.Update(elapsedTime);
                mainWeapon.Center = center;
                if (mainWeapon.CoolDown <= 0)
                {
                    mainWeapon.Fire(firingAngle);
                    soundFireBullet.Play();
                }
            }
            else
                mainWeapon.Update(elapsedTime);
        }

        public override void Draw()
        {
            base.Draw();
            //Iterate through fighter rounds to be drawn.
            //foreach (Round r in mainWeapon.Rounds)
            //{
            //    if (r.Alive || r.Exploding)
            //    {
            //        //Draw fired rounds.
            //        r.Draw();
            //    }
            //}
        }

        public PrimaryWeapon MainWeapon
        {
            get
            {
                return mainWeapon;
            }
        }
    }
}
