using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Aero
{
    class KamacazieBoss : Boss
    {
        double firingAngle;
        SoundEffect soundFireBullet;
        float speed;
        bool crashing = false;
        float crashTimer = 0;
        float crashTimerMax = 6.0f;

        public KamacazieBoss()
            : base()
        {
            if (AeroGame.lagTest)
                texture = AeroGame.LoadTextureStream("KamikazeBoss");
            else
                texture = AeroGame.ContentManager.Load<Texture2D>("Textures\\KamikazeBoss");
            textureData = new Color[texture.Width * texture.Height];
            texture.GetData(textureData);
            soundFireBullet = AeroGame.ContentManager.Load<SoundEffect>("Sounds\\EnemyLaser");
            /*pieces = new ObjectPiece[8];
            pieces[0] = new ObjectPiece(AeroGame.ContentManager.Load<Texture2D>("Textures\\Boss2Piece1"), new Vector2((float)Math.Cos(Math.PI), (float)Math.Sin(Math.PI)));
            pieces[1] = new ObjectPiece(AeroGame.ContentManager.Load<Texture2D>("Textures\\Boss2Piece2"), new Vector2((float)Math.Cos(-2 * Math.PI / 3), (float)Math.Sin(-2 * Math.PI / 3)));//
            pieces[2] = new ObjectPiece(AeroGame.ContentManager.Load<Texture2D>("Textures\\Boss2Piece3"), new Vector2((float)Math.Cos(-Math.PI / 3), (float)Math.Sin(-Math.PI / 3)));
            pieces[3] = new ObjectPiece(AeroGame.ContentManager.Load<Texture2D>("Textures\\Boss2Piece4"), new Vector2((float)Math.Cos(0), (float)Math.Sin(0)));
            pieces[4] = new ObjectPiece(AeroGame.ContentManager.Load<Texture2D>("Textures\\Boss2Piece5"), new Vector2((float)Math.Cos(-5 * Math.PI / 6), (float)Math.Sin(-5 * Math.PI / 6)));//
            pieces[5] = new ObjectPiece(AeroGame.ContentManager.Load<Texture2D>("Textures\\Boss2Piece6"), new Vector2((float)Math.Cos(3 * Math.PI / 4), (float)Math.Sin(3 * Math.PI / 4)));
            pieces[6] = new ObjectPiece(AeroGame.ContentManager.Load<Texture2D>("Textures\\Boss2Piece7"), new Vector2((float)Math.Cos(Math.PI / 4), (float)Math.Sin(Math.PI / 4)));
            pieces[7] = new ObjectPiece(AeroGame.ContentManager.Load<Texture2D>("Textures\\Boss2Piece8"), new Vector2((float)Math.Cos(-Math.PI / 6), (float)Math.Sin(-Math.PI / 6)));
            */
            //origin = new Vector2(texture.Width / 2, texture.Height / 2);
            position = new Vector2(0, texture.Height * -1.0f);
            center = new Vector2(position.X + texture.Width / 2.0f, position.Y);
            mainWeapon = new PrimaryWeapon[1];
            mainWeapon[0] = new QuintuCannon(false);
            mainWeapon[0].SetCoolDown(1.0f);
            explodingAnimation = new LargeExplosionAnimation(texture);
            health = 4000;
            firingAngle = 0;
            speed = 200;
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
                if (!crashing)
                {
                    Evade(speed * (float)elapsedTime.TotalSeconds);
                    crashTimer += (float)elapsedTime.TotalSeconds;
                    if(crashTimer >= crashTimerMax)
                        if (speed < 0)
                            speed *= -1;
                }
                if (crashTimer >= crashTimerMax)
                {
                    crashing = true;
                    Crash(speed * 4 * (float)elapsedTime.TotalSeconds);
                }
                mainWeapon[0].Update(elapsedTime);
                mainWeapon[0].Center = center;
                if (mainWeapon[0].CoolDown <= 0)
                {
                    if (mainWeapon[0].CoolDown <= 0)
                    {
                        firingAngle = CalculateFiringAngle(mainWeapon[0].Center, Player.Center);
                        mainWeapon[0].Fire(firingAngle);
                        soundFireBullet.Play();
                    }/*
                    double dx = center.X - Player.BoundingRectangle.Center.X;
                    double dy = center.Y - Player.BoundingRectangle.Center.Y;
                    firingAngle = (float)Math.Atan(dy / dx);
                    if (Player.BoundingRectangle.Center.X > center.X)
                        firingAngle = firingAngle + Math.PI;
                    //mainWeapon[0].Fire(firingAngle);
                    //soundFireBullet.Play();*/
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

        private void Evade(float velX)
        {
            float x = position.X;
            x += velX;
            if (x <= 0 || x >= AeroGame.Graphics.GraphicsDevice.Viewport.Width - texture.Width)
                speed *= -1;
            Position = new Vector2(x, position.Y);
        }

        private void Crash(float velY)
        {
            float y = position.Y;
            y += velY;
            if (y >= AeroGame.Graphics.GraphicsDevice.Viewport.Height - texture.Height)
                speed *= -1;
            else if (y <= 0)
            {
                crashTimer = 0;
                crashing = false;
                y = 0;
                if (position.X < Player.Position.X)
                    speed *= -1;
            }
            Position = new Vector2(position.X, y);
        }
    }
}
