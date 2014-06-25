#region File Description
//-----------------------------------------------------------------------------
// BackgroundScreen.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
#endregion

namespace Aero
{
    /// <summary>
    /// The background screen sits behind all the other menu screens.
    /// It draws a background image that remains fixed in place regardless
    /// of whatever transitions the screens on top of it may be doing.
    /// </summary>
    class BackgroundScreen : GameScreen
    {
        #region Fields

        ContentManager content;
        Texture2D backgroundTexture;
        protected Texture2D backgroundBackTexture1;///
        protected Texture2D backgroundBackTexture2;///
        protected Texture2D backgroundFrontTexture1;///
        protected Texture2D backgroundFrontTexture2;///
        int bgBackY1;
        int bgBackY2;
        int bgFrontY1;
        int bgFrontY2;

        #endregion

        #region Initialization


        /// <summary>
        /// Constructor.
        /// </summary>
        public BackgroundScreen()
        {
            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }


        /// <summary>
        /// Loads graphics content for this screen. The background texture is quite
        /// big, so we use our own local ContentManager to load it. This allows us
        /// to unload before going from the menus into the game itself, wheras if we
        /// used the shared ContentManager provided by the Game class, the content
        /// would remain loaded forever.
        /// </summary>
        public override void LoadContent()
        {
            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");

           backgroundTexture = content.Load<Texture2D>("Textures\\AeroGameBackground");
           backgroundBackTexture1 = AeroGame.ContentManager.Load<Texture2D>("Textures\\backgroundBack");
           backgroundBackTexture2 = AeroGame.ContentManager.Load<Texture2D>("Textures\\backgroundBack");
           backgroundFrontTexture1 = AeroGame.ContentManager.Load<Texture2D>("Textures\\backgroundFront");
           backgroundFrontTexture2 = AeroGame.ContentManager.Load<Texture2D>("Textures\\backgroundFront");

           bgBackY1 = bgFrontY1 = 0;
           bgBackY2 = bgFrontY2 = -900;
        }


        /// <summary>
        /// Unloads graphics content for this screen.
        /// </summary>
        public override void UnloadContent()
        {
            content.Unload();
        }


        #endregion

        #region Update and Draw


        /// <summary>
        /// Updates the background screen. Unlike most screens, this should not
        /// transition off even if it has been covered by another screen: it is
        /// supposed to be covered, after all! This overload forces the
        /// coveredByOtherScreen parameter to false in order to stop the base
        /// Update method wanting to transition off.
        /// </summary>
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, false);
            bgBackY1 += (int)(100.0f * 0.0166);
            bgBackY2 += (int)(100.0f * 0.0166);
            if (bgBackY1 > 900)
                bgBackY1 = bgBackY2 - 900;
            if (bgBackY2 > 900)
                bgBackY2 = bgBackY1 - 900;
            bgFrontY1 += (int)(150.0f * 0.0166);
            bgFrontY2 += (int)(150.0f * 0.0166);
            if (bgFrontY1 > 900)
                bgFrontY1 = bgFrontY2 - 900;
            if (bgFrontY2 > 900)
                bgFrontY2 = bgFrontY1 - 900;
        }


        /// <summary>
        /// Draws the background screen.
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            Viewport viewport = ScreenManager.GraphicsDevice.Viewport;
            Rectangle fullscreen = new Rectangle(0, 0, viewport.Width, viewport.Height);
            byte fade = TransitionAlpha;
            AeroGame.SpriteBatch.Begin();
            //AeroGame.SpriteBatch.Draw(backgroundTexture, fullscreen,
            //                 new Color(fade, fade, fade));
            AeroGame.SpriteBatch.Draw(backgroundBackTexture1, new Rectangle(0, bgBackY1, backgroundBackTexture1.Width, backgroundBackTexture1.Height), Color.White);
            AeroGame.SpriteBatch.Draw(backgroundBackTexture2, new Rectangle(0, bgBackY2, backgroundBackTexture2.Width, backgroundBackTexture2.Height), Color.White);
            AeroGame.SpriteBatch.Draw(backgroundFrontTexture1, new Rectangle(0, bgFrontY1, backgroundFrontTexture1.Width, backgroundFrontTexture1.Height), Color.White);
            AeroGame.SpriteBatch.Draw(backgroundFrontTexture2, new Rectangle(0, bgFrontY2, backgroundFrontTexture2.Width, backgroundFrontTexture2.Height), Color.White);
            
            AeroGame.SpriteBatch.End();
        }


        #endregion
    }
}
