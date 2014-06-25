using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Aero
{
    class ShadowBoss : Boss
    {
        double firingAngle;
        SoundEffect soundFireBullet;
        int maxHealth;
        int weaponLevel;
        Shield shield;
        TriCannon triCannon;
        QuintuCannon quintuCannon;

        public ShadowBoss()
            : base()
        {
            if (AeroGame.lagTest)
                texture = AeroGame.LoadTextureStream("ShadowBoss");
            else
                texture = AeroGame.ContentManager.Load<Texture2D>("Textures\\ShadowBoss");
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
            triCannon = new TriCannon(false);
            quintuCannon = new QuintuCannon(false);
            mainWeapon = new PrimaryWeapon[1];
            mainWeapon[0] = new StandardCannon(false);
            mainWeapon[0].SetCoolDown(1.0f);
            secondaryWeapon = new SecondaryWeapon[1];
            secondaryWeapon[0] = new FragmentationBombCannon(false, 1.0f);
            secondaryWeapon[0].CoolDownLimit = 5.0f;
            explodingAnimation = new LargeExplosionAnimation(texture);
            health = maxHealth = 4000;
            firingAngle = 0;
            speed = 200;
            weaponLevel = 0;
            shield = new Shield();
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
                    //shield.Spawn(position, friendly);
                }
            }
            else if (alive)
            {
                Stalk(speed * (float)elapsedTime.TotalSeconds);
                //shield.Position = position;
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
                if(weaponLevel == 0)
                    if (health < maxHealth * 0.66){
                        mainWeapon[0] = triCannon;
                        weaponLevel = 1;
                    }
                if(weaponLevel == 1)
                    if (health < maxHealth * 0.33)
                    {
                        mainWeapon[0] = quintuCannon;
                        weaponLevel = 2;
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

        public override void Draw()
        {
            base.Draw();
            /*if (exploding)
            {
                foreach (ObjectPiece p in pieces)
                    p.Draw();
            }*/
        }

        public void Stalk(float velX)
        {
            float playerX = Player.Position.X;
            if (MathHelper.Distance(playerX, position.X) < velX)
            {
                position.X = playerX;// -texture.Width / 2;
            }
            else
            {
                if (playerX < position.X)
                    position.X -= velX;
                else
                    position.X += velX;
            }
            Position = new Vector2(position.X, position.Y);
        }
    }
}
