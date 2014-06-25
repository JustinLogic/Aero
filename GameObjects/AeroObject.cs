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
    class AeroObject
    {
        protected float scale;
        protected int health;
        protected int maxHealth;
        protected float theta;
        protected float speed;
        protected bool alive;
        protected bool exploding;
        protected bool friendly;
        protected Vector2 position;
        protected Vector2 velocity;
        protected Vector2 center;
        protected Vector2 origin;
        protected Texture2D texture;
        protected Color[] textureData;
        protected Matrix rotation;
        protected Rectangle boundingRectangle;
        protected static SpriteBatch spriteBatch;
        protected ContentManager content;
        protected PowerUpType type;
        //Data for bounding rectangle.
        static Vector2 leftTop;
        static Vector2 rightTop;
        static Vector2 leftBottom;
        static Vector2 rightBottom;
        static Vector2 min;
        static Vector2 max;
        protected bool isHit;
        protected Color color;
        protected float hitTimeout;

        public bool wasUpdated = false;

        public AeroObject()
        {
            content = AeroGame.ContentManager;
            //spriteBatch = new SpriteBatch(AeroGame.Graphics.GraphicsDevice);
            rotation = new Matrix();
            boundingRectangle = new Rectangle();
            scale = 1;
            //Bounding Rectangle Data
            leftTop = Vector2.Zero;
            rightTop = Vector2.Zero;
            leftBottom = Vector2.Zero;
            rightBottom = Vector2.Zero;
            min = Vector2.Zero;
            max = Vector2.Zero;
            alive = false;
            exploding = false;
            friendly = false;
            type = PowerUpType.None;
            isHit = false;
            hitTimeout = 0.1f;
            color = Color.White;
        }

        public virtual void Update(TimeSpan timeSpan)
        {
            wasUpdated = true;
            if (alive || exploding)
                SetRotation();
            if (isHit)
            {
                hitTimeout -= (float)timeSpan.TotalSeconds;
                if (hitTimeout < 0)
                {
                    isHit = false;
                    hitTimeout = 0.1f;
                    color = Color.White;
                }
            }
            //else
                //Level.activeObjects.Remove(this);
        }

        public virtual void Hit(int damage)
        {
            health -= damage;
            if (health <= 0)
                Kill();
            isHit = true;
            color = Color.Tomato;
        }

        public virtual void Kill()
        {
            Player.ScoreSystem.Add(this);
            //Level.activeObjects.Remove(this);
        }

        public virtual void Spawn()
        {
        }

        public virtual void Draw()
        {

        }

        protected void SetRotation()
        {
            rotation =
                Matrix.CreateTranslation(new Vector3(-origin, 0.0f)) *
                Matrix.CreateRotationZ(theta) *
                Matrix.CreateTranslation(new Vector3(position, 0.0f));
            boundingRectangle = CalculateBoundingRectangle(new Rectangle(0, 0, texture.Width, texture.Height), rotation);
            center.X = boundingRectangle.Center.X;
            center.Y = boundingRectangle.Center.Y;
        }

        protected static Rectangle CalculateBoundingRectangle(Rectangle rectangle,
                                        Matrix transform)
        {
            //// Get all four corners in local space
            //leftTop = new Vector2(rectangle.Left, rectangle.Top);
            //rightTop = new Vector2(rectangle.Right, rectangle.Top);
            //leftBottom = new Vector2(rectangle.Left, rectangle.Bottom);
            //rightBottom = new Vector2(rectangle.Right, rectangle.Bottom);

            leftTop.X = rectangle.Left;
            leftTop.Y = rectangle.Top;
            rightTop.X = rectangle.Right;
            rightTop.Y = rectangle.Top;
            leftBottom.X = rectangle.Left;
            leftBottom.Y = rectangle.Bottom;
            rightBottom.X = rectangle.Right;
            rightBottom.Y = rectangle.Bottom;

            // Transform all four corners into work space
            Vector2.Transform(ref leftTop, ref transform, out leftTop);
            Vector2.Transform(ref rightTop, ref transform, out rightTop);
            Vector2.Transform(ref leftBottom, ref transform, out leftBottom);
            Vector2.Transform(ref rightBottom, ref transform, out rightBottom);

            // Find the minimum and maximum extents of the rectangle in world space
            min = Vector2.Min(Vector2.Min(leftTop, rightTop),
                                      Vector2.Min(leftBottom, rightBottom));
            max = Vector2.Max(Vector2.Max(leftTop, rightTop),
                                      Vector2.Max(leftBottom, rightBottom));

            // Return as a rectangle
            return new Rectangle((int)min.X, (int)min.Y,
                                 (int)(max.X - min.X), (int)(max.Y - min.Y));
        }

        public virtual void killOffScreen()
        {
            alive = false;
            exploding = false;
            Level.activeObjects.Remove(this);
            //Kill();
        }

        public bool Alive
        {
            get
            {
                return alive;
            }
        }

        public bool Active
        {
            get
            {
                return alive || exploding;
            }
        }

        public bool Friendly
        {
            get
            {
                return friendly;
            }
        }

        public Rectangle BoundingRectangle
        {
            get
            {
                return boundingRectangle;
            }
        }

        public Vector2 Position
        {
            get
            {
                return position;
            }
            set
            {
                position = value;
            }
        }

        public Vector2 Center
        {
            get
            {
                return center;
            }
        }

        public Vector2 Origin
        {
            get
            {
                return origin;
            }
        }

        public float RotationZ
        {
            get
            {
                return theta;
            }
        }

        public Texture2D Texture
        {
            get
            {
                return texture;
            }
        }

        public Color[] TextureData
        {
            get
            {
                return textureData;
            }
        }

        public Matrix Rotation
        {
            get
            {
                return rotation;
            }
        }
        public PowerUpType PowerUpType
        {
            get
            {
                return type;
            }
        }
    }
}
