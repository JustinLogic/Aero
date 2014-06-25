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
    class GameOverScreen : MenuScreen
    {
        Game game;

        public GameOverScreen(Game game)
            :base("GAME OVER")
        {
            this.game = game;
            MenuEntry trollModeMenuEntry = new MenuEntry("Troll Mode");
            MenuEntry titleReturnMenuEntry = new MenuEntry("Return to Title Screen");
            MenuEntry exitGameMenuEntry = new MenuEntry("Exit Game");

            trollModeMenuEntry.Selected += trollModeEntrySelected;
            titleReturnMenuEntry.Selected += titleReturnMenuEntrySelected;
            exitGameMenuEntry.Selected += OnCancel;

            MenuEntries.Add(titleReturnMenuEntry);
            MenuEntries.Add(exitGameMenuEntry);

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }


        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
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

        void trollModeEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            //LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
            //                   new TitleScreen(game));
            //ScreenManager.AddScreen(new GamePlayScreen(), e.PlayerIndex);
            //ScreenManager.RemoveScreen(this);
        }

        /// <summary>
        /// Event handler for when the Play Game menu entry is selected.
        /// </summary>
        void titleReturnMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                               new TitleScreen(game));
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
    }
}
