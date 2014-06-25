using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
#if XBOX
using Microsoft.Xna.Framework.GamerServices;
#endif

namespace Aero
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class AeroGame : Microsoft.Xna.Framework.Game
    {
        public static bool lagTest;
        private static GraphicsDeviceManager graphics;
        private IGraphicsDeviceService graphicsService;
        private static GraphicsDevice device;
        ScreenManager screenManager; //Test
        static SpriteBatch spriteBatch;
        SoundEffect gameSound;
        static SoundEffectInstance instance;
        private static bool troll;
        
        //GamePlayScreen gamePlayScreen;

        private Rectangle viewportRect;

        //private static Screen currentScreen;
        private static ContentManager contentManager;
#if XBOX
        Gamer gamer;
        GamerProfile gamerProfile;
#endif
        AvailableNetworkSessionCollection activeSessions;
        NetworkSession session;

        private static Random random = new Random();
        public static Random Random
        {
            get { return random; }
        }

        public Rectangle ViewPortRect
        {
            get
            {
                return viewportRect;
            }
        }

        public static bool Troll
        {
            get
            {
                return troll;
            }
            set
            {
                troll = value;
            }
        }

        public static GraphicsDeviceManager Graphics
        {
            get
            {
                return graphics;
            }
        }

        public static GraphicsDevice Device
        {
            get
            {
                return device;
            }
        }

        public static ContentManager ContentManager
        {
            get
            {
                return contentManager;
            }
        }

        public static SpriteBatch SpriteBatch
        {
            get
            {
                return spriteBatch;
            }
        }

        public static SoundEffectInstance GameSong
        {
            get
            {
                return instance;
            }
        }

        public AeroGame()
        {
            //graphicsService = (IGraphicsDeviceService)this.Services.GetService(typeof(IGraphicsDeviceService));
            //device = graphicsService.GraphicsDevice;
            lagTest = false;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            contentManager = Content;
            //device = graphicsService.GraphicsDevice;

            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 720;
            graphics.IsFullScreen = false;

            // Create the screen manager component.   //Test
            screenManager = new ScreenManager(this);

            Components.Add(screenManager);
            //Components.Add(new GamerServicesComponent(this));
            troll = false;

            // Activate the first screens.
            //gamePlayScreen = new GamePlayScreen(this);
            //screenManager.AddScreen(new BackgroundScreen(), null);
            //screenManager.AddScreen(new TitleScreen(this), null);

            //TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 33);
            //Player.LoadContent();
            //this.IsFixedTimeStep = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
       /* protected override void Initialize()
        {
            GamerServicesDispatcher.WindowHandle = Window.Handle;
   
            GamerServicesDispatcher.Initialize(Services);

            base.Initialize();
        }*/

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            graphicsService = (IGraphicsDeviceService)this.Services.GetService(typeof(IGraphicsDeviceService));
            device = graphicsService.GraphicsDevice;
            //contentManager = new ContentManager(Services);
            spriteBatch = new SpriteBatch(graphics.GraphicsDevice);
            gameSound = contentManager.Load<SoundEffect>("Sounds\\AeroTheme1");
            instance = gameSound.CreateInstance();
            instance.IsLooped = true;
            instance.Volume = 0.25f;
            instance.Play();
            //gameSound.Play();
            

            /*SignedInGamer.SignedIn += new EventHandler<SignedInEventArgs>((o,e) =>
            {
                gamer = e.Gamer;

                gamer.BeginGetProfile(new AsyncCallback((result) =>
                {
                    gamerProfile = gamer.EndGetProfile(result);
                }), null);
            });*/
            // Create a new SpriteBatch, which can be used to draw textures.
            //spriteBatch = new SpriteBatch(GraphicsDevice);

            //viewportRect = new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width,
            //    graphics.GraphicsDevice.Viewport.Height);
            //backgroundTexture = Content.Load<Texture2D>("Textures\\AeroTitleBackground");

            // TODO: use this.Content to load your game content here
            screenManager.AddScreen(new BackgroundScreen(), null);
            screenManager.AddScreen(new TitleScreen(this), null);
            Player.LoadContent();

            //session.LocalGamers[0];
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //GamerServicesDispatcher.Update();
            base.Update(gameTime);
        }
        //protected override void Update(GameTime gameTime)
        //{
        //    // Allows the game to exit
        //    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
        //        this.Exit();

        //    // TODO: Add your update logic here

        //    TimeSpan elapsedTime = gameTime.ElapsedGameTime;
        //    TimeSpan time = gameTime.TotalGameTime;
        //    GameState newGameState;
        //    newGameState = currentScreen.update(time, elapsedTime);
            
        //    if (newGameState == GameState.Exit)
        //    {
        //        this.Exit();
        //    }
        //    else if (newGameState != GameState.None)
        //    {
        //        ChangeState(newGameState);
        //    }
        //    base.Update(gameTime);
        //}

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.Black);
            //spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
            //currentScreen.render();
            //spriteBatch.Draw(backgroundTexture, viewportRect, Color.White);
            //spriteBatch.End();

            // TODO: Add your drawing code here
            base.Draw(gameTime);
        }

        public static float RandomBetween(float min, float max)
        {
            return min + (float)random.NextDouble() * (max - min);
        }

        public static Texture2D LoadTextureStream(string loc)
        {

            Texture2D file = null;
            RenderTarget2D result = null;
            using (Stream titleStream = TitleContainer.OpenStream("Content\\Textures\\" + loc + ".png"))
            {
                file = Texture2D.FromStream(device, titleStream);
            }

            //Setup a render target to hold our final texture which will have premulitplied alpha values
            result = new RenderTarget2D(device, file.Width, file.Height);

            device.SetRenderTarget(result);
            device.Clear(Color.Black);

            //Multiply each color by the source alpha, and write in just the color values into the final texture
            BlendState blendColor = new BlendState();
            blendColor.ColorWriteChannels = ColorWriteChannels.Red | ColorWriteChannels.Green | ColorWriteChannels.Blue;

            blendColor.AlphaDestinationBlend = Blend.Zero;
            blendColor.ColorDestinationBlend = Blend.Zero;
            blendColor.AlphaSourceBlend = Blend.SourceAlpha;
            blendColor.ColorSourceBlend = Blend.SourceAlpha;

            SpriteBatch spriteBatch = new SpriteBatch(device);
            spriteBatch.Begin(SpriteSortMode.Immediate, blendColor);
            spriteBatch.Draw(file, file.Bounds, Color.White);
            spriteBatch.End();

            //Now copy over the alpha values from the PNG source texture to the final one, without multiplying them
            BlendState blendAlpha = new BlendState();
            blendAlpha.ColorWriteChannels = ColorWriteChannels.Alpha;

            blendAlpha.AlphaDestinationBlend = Blend.Zero;
            blendAlpha.ColorDestinationBlend = Blend.Zero;

            blendAlpha.AlphaSourceBlend = Blend.One;
            blendAlpha.ColorSourceBlend = Blend.One;

            spriteBatch.Begin(SpriteSortMode.Immediate, blendAlpha);
            spriteBatch.Draw(file, file.Bounds, Color.White);
            spriteBatch.End();

            //Release the GPU back to drawing to the screen
            device.SetRenderTarget(null);

            return result as Texture2D;
        }


        //internal void ChangeState(GameState newGameState)
        //{
        //    //Logo spash can come from ANY state since its the place you go when you restart
        //    if (newGameState == GameState.TitleScreen)
        //    {
        //        currentScreen = new TitleScreen(this);
        //        //backgroundTexture = Content.Load<Texture2D>("Textures\\AeroTitleBackground");
        //        currentScreen.activate(graphics);
        //    }

        //    else if (newGameState == GameState.GameScreen)
        //    {
        //        currentScreen = new GamePlayScreen(this);
        //        //backgroundTexture = Content.Load<Texture2D>("Textures\\AeroGameBackground");
        //        currentScreen.activate(graphics);
        //    }
        //}
    }
}
