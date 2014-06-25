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
    class ControlsScreen : MenuScreen
    {
        Game game;
        Texture2D texture;
        Color[] textureData;

        public ControlsScreen(Game game)
            : base("Controls")
        {
            this.game = game;
            IsPopup = true;
            texture = AeroGame.ContentManager.Load<Texture2D>("Textures\\controls");
            textureData = new Color[texture.Width * texture.Height];
            texture.GetData(textureData);
            // Create our menu entries.
            //MenuEntry resumeGameMenuEntry = new MenuEntry("Resume Game");
            //MenuEntry quitGameMenuEntry = new MenuEntry("Quit Game");

            //resumeGameMenuEntry.Selected += OnCancel;
            //quitGameMenuEntry.Selected += QuitGameMenuEntrySelected;

            // Add entries to the menu.
            //MenuEntries.Add(resumeGameMenuEntry);
            //MenuEntries.Add(quitGameMenuEntry);
        }

        protected override void OnSelectEntry(int entryIndex, PlayerIndex playerIndex)
        {
            //Do Nothing
            //menuEntries[selectedEntry].OnSelectEntry(playerIndex);
        }

        /*void QuitGameMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Are you sure you want to quit this game?";

            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }

        void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, false, null, new BackgroundScreen(), new TitleScreen(game));
            Player.Reset();
        }*/

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);

            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;
            Vector2 position = new Vector2();
            spriteBatch.Begin();

            spriteBatch.Draw(texture, new Rectangle(0, 0, texture.Width, texture.Height), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
