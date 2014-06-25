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
    class TitleScreen : MenuScreen
    {
        private Game game;

        //private IGraphicsDeviceService graphicsService;
        //private GraphicsDevice device;
        //private ContentManager content;
        //private Texture2D backgroundTexture;
        Texture2D texture;
        Color[] textureData;
        HintSystem hintSystem;

        public TitleScreen(Game game)
            : base("")
        {
            this.game = game;
            //if (content == null)
            //    content = new ContentManager(ScreenManager.Game.Services, "Content");
            if (AeroGame.lagTest)
                texture = AeroGame.LoadTextureStream("AeroTitleCross");
            else
                texture = AeroGame.ContentManager.Load<Texture2D>("Textures\\AeroTitleCross");
            textureData = new Color[texture.Width * texture.Height];
            texture.GetData(textureData);

            MenuEntry startGameMenuEntry = new MenuEntry("Start Game");
            MenuEntry exitGameMenuEntry = new MenuEntry("Exit Game");
            MenuEntry controlsMenuEntry = new MenuEntry("Controls");

            startGameMenuEntry.Selected += StartGameMenuEntrySelected;
            controlsMenuEntry.Selected += ControlsMenuEntrySelected;
            exitGameMenuEntry.Selected += OnCancel;

            MenuEntries.Add(startGameMenuEntry);
            MenuEntries.Add(controlsMenuEntry);
            MenuEntries.Add(exitGameMenuEntry);
            
            //this.game = game;

            //graphicsService = (IGraphicsDeviceService)game.Services.GetService(typeof(IGraphicsDeviceService));
            //device = graphicsService.GraphicsDevice;
            //spriteBatch = new SpriteBatch(graphicsService.GraphicsDevice);
            //ScreenManager.Initialize();
            //SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

            //viewportRect = new Rectangle(0, 0, AeroGame.Graphics.GraphicsDevice.Viewport.Width,
            //                                             AeroGame.Graphics.GraphicsDevice.Viewport.Height);
            //backgroundTexture = game.Content.Load<Texture2D>("Textures\\AeroTitleBackground");

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);

            hintSystem = new HintSystem();

        }

        //public override void activate(GraphicsDeviceManager graphics)
        //{
        //    graphicsService = (IGraphicsDeviceService)game.Services.GetService(typeof(IGraphicsDeviceService));
        //    device = graphicsService.GraphicsDevice;
        //    spriteBatch = new SpriteBatch(graphicsService.GraphicsDevice);
        //    //ScreenManager.Initialize();
        //    //SpriteBatch spriteBatch = ScreenManager.SpriteBatch;

        //    viewportRect = new Rectangle(0, 0, AeroGame.Graphics.GraphicsDevice.Viewport.Width,
        //                                                 AeroGame.Graphics.GraphicsDevice.Viewport.Height);
        //    backgroundTexture = game.Content.Load<Texture2D>("Textures\\AeroTitleBackground");
        //}

        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            hintSystem.Update(gameTime.ElapsedGameTime);
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
#if !XBOX
            KeyboardState keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Z))
            {
                //return GameState.GameScreen;
            }
            else if (keyboardState.IsKeyDown(Keys.X))
            {
                //return GameState.Exit;
            }

#endif
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
        }

        #region Handle Input


        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void StartGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            AeroGame.Troll = false;
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new GamePlayScreen(game));
            //ScreenManager.AddScreen(new GamePlayScreen(), e.PlayerIndex);
            //ScreenManager.RemoveScreen(this);
        }

        void ControlsMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.AddScreen(new ControlsScreen(game), ControllingPlayer);
            //ScreenManager.AddScreen(new GamePlayScreen(), e.PlayerIndex);
            //ScreenManager.RemoveScreen(this);
        }

        /// <summary>
        /// When the user cancels the main menu, ask if they want to exit the sample.
        /// </summary>
        protected override void OnCancel(PlayerIndex playerIndex)
        {
            const string message = "Are you sure you want to exit?";

            MessageBoxScreen confirmExitMessageBox = new MessageBoxScreen(message);

            confirmExitMessageBox.Accepted += ConfirmExitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmExitMessageBox, playerIndex);
        }


        /// <summary>
        /// Event handler for when the user selects ok on the "are you sure
        /// you want to exit" message box.
        /// </summary>
        void ConfirmExitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            ScreenManager.Game.Exit();
        }


        #endregion

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            Vector2 position = new Vector2(350, 500);

            // Make the menu slide into place during transitions, using a
            // power curve to make things look more interesting (this makes
            // the movement slow down as it nears the end).
            float transitionOffset = (float)Math.Pow(TransitionPosition, 2);

            if (ScreenState == ScreenState.TransitionOn)
                position.X -= transitionOffset * 256;
            else
                position.X += transitionOffset * 512;

            spriteBatch.Begin();

            // Draw each menu entry in turn.
            for (int i = 0; i < menuEntries.Count; i++)
            {
                MenuEntry menuEntry = menuEntries[i];

                bool isSelected = IsActive && (i == selectedEntry);

                menuEntry.Draw(this, position, isSelected, gameTime);

                position.Y += menuEntry.GetHeight(this);
            }

            // Draw the menu title.
            Vector2 titlePosition = new Vector2(426, 80);
            Vector2 titleOrigin = font.MeasureString(menuTitle) / 2;
            Color titleColor = new Color(192, 192, 192, TransitionAlpha);
            float titleScale = 1.25f;
            titlePosition.Y -= transitionOffset * 100;

            spriteBatch.Draw(texture, new Rectangle(0, 0, ScreenManager.Game.GraphicsDevice.Viewport.Width, ScreenManager.Game.GraphicsDevice.Viewport.Height / 2),
                null, titleColor, 0, titleOrigin, SpriteEffects.None, 0);

            spriteBatch.End();

            hintSystem.Draw(font);
        }
    }
}
