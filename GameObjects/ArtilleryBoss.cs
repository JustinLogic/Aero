using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Aero
{
    class ArtilleryBoss : Boss
    {
        double firingAngle;
        

        public ArtilleryBoss()
            : base()
        {
            if (AeroGame.lagTest)
                texture = AeroGame.LoadTextureStream("ArtilleryBoss");
            else
                texture = AeroGame.ContentManager.Load<Texture2D>("Textures\\ArtilleryBoss");
            textureData = new Color[texture.Width * texture.Height];
            texture.GetData(textureData);
            soundFireBullet = AeroGame.ContentManager.Load<SoundEffect>("Sounds\\EnemyLaser");
            soundFireBomb = AeroGame.ContentManager.Load<SoundEffect>("Sounds\\Bomb");
            /*pieces = new ObjectPiece[8];
            pieces[0] = new ObjectPiece(AeroGame.ContentManager.Load<Texture2D>("Textures\\Boss3Piece1"), new Vector2((float)Math.Cos(Math.PI), (float)Math.Sin(Math.PI)));
            pieces[1] = new ObjectPiece(AeroGame.ContentManager.Load<Texture2D>("Textures\\Boss3Piece2"), new Vector2((float)Math.Cos(-2 * Math.PI / 3), (float)Math.Sin(-2 * Math.PI / 3)));//
            pieces[2] = new ObjectPiece(AeroGame.ContentManager.Load<Texture2D>("Textures\\Boss3Piece3"), new Vector2((float)Math.Cos(-Math.PI / 3), (float)Math.Sin(-Math.PI / 3)));
            pieces[3] = new ObjectPiece(AeroGame.ContentManager.Load<Texture2D>("Textures\\Boss3Piece4"), new Vector2((float)Math.Cos(0), (float)Math.Sin(0)));
            pieces[4] = new ObjectPiece(AeroGame.ContentManager.Load<Texture2D>("Textures\\Boss3Piece5"), new Vector2((float)Math.Cos(-5 * Math.PI / 6), (float)Math.Sin(-5 * Math.PI / 6)));//
            pieces[5] = new ObjectPiece(AeroGame.ContentManager.Load<Texture2D>("Textures\\Boss3Piece6"), new Vector2((float)Math.Cos(3 * Math.PI / 4), (float)Math.Sin(3 * Math.PI / 4)));
            pieces[6] = new ObjectPiece(AeroGame.ContentManager.Load<Texture2D>("Textures\\Boss3Piece7"), new Vector2((float)Math.Cos(Math.PI / 4), (float)Math.Sin(Math.PI / 4)));
            pieces[7] = new ObjectPiece(AeroGame.ContentManager.Load<Texture2D>("Textures\\Boss3Piece8"), new Vector2((float)Math.Cos(-Math.PI / 6), (float)Math.Sin(-Math.PI / 6)));
            */
            //origin = new Vector2(texture.Width / 2, texture.Height / 2);
            position = new Vector2(0, texture.Height * -1.0f);
            center = new Vector2(position.X + texture.Width / 2.0f, position.Y);
            mainWeapon = new PrimaryWeapon[1];
            mainWeapon[0] = new TriCannon(false);
            mainWeapon[0].SetCoolDown(1.0f);
            secondaryWeapon = new SecondaryWeapon[1];
            secondaryWeapon[0] = new FragmentationBombCannon(false, 1.0f);
            secondaryWeapon[0].CoolDownLimit = 5.0f;
            explodingAnimation = new LargeExplosionAnimation(texture);
            health = 2000;
            firingAngle = 0;
            speed = 300;
        }

        public override void Update(TimeSpan elapsedTime)
        {
            base.Update(elapsedTime);
            if (activateCooldown > 0)
            {
                activateCooldown -= (float)elapsedTime.TotalSeconds;
                if (activateCooldown <= 0)
                {
                    /*for (int i = 0, n = 0; i < pieces.Length; i++)
                    {
                        pieces[i].Init(position);
                    }*/
                }
            }
            else if (alive)
            {
                Evade(speed * (float)elapsedTime.TotalSeconds);
                mainWeapon[0].Update(elapsedTime);
                mainWeapon[0].Center = center;
                secondaryWeapon[0].Update(elapsedTime);
                secondaryWeapon[0].Center = center;
                if (mainWeapon[0].CoolDown <= 0)
                {
                    firingAngle = CalculateFiringAngle(center, Player.Center);
                    mainWeapon[0].Fire(firingAngle);
                    soundFireBullet.Play();
                }
                if (secondaryWeapon[0].CoolDown >= secondaryWeapon[0].CoolDownLimit)
                {
                    firingAngle = CalculateFiringAngle(center, Player.Center);
                    secondaryWeapon[0].Fire(firingAngle);
                    soundFireBomb.Play();
                }
            }
            else
            {
                mainWeapon[0].Update(elapsedTime);
                if (shatterCooldown > 0)
                {
                    //foreach (ObjectPiece p in pieces)
                    //    p.Update(elapsedTime);
                }
            }
        }

        private void Evade(float velX)
        {
            float x = position.X;
            x += velX;
            if (x <= 0 || x >= AeroGame.Graphics.GraphicsDevice.Viewport.Width - texture.Width)
                speed *= -1;
            Position = new Vector2(x, position.Y);
            
        }

        public override void Draw()
        {
            base.Draw();
            /*if (exploding)
            {
                foreach (ObjectPiece p in pieces)
                    p.Draw();
            }*/
        }
    }
}
