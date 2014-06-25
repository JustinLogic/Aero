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
    class ShopScreen : MenuScreen
    {
        Rectangle viewportRect;
        bool weaponUpgraded;
        bool shieldUpgraded;
        bool engineUpgraded;

        public ShopScreen(Game game)
            : base("Shop")
        {
            viewportRect = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width,
                                                         game.GraphicsDevice.Viewport.Height);

            IsPopup = true;
            weaponUpgraded = false;
            shieldUpgraded = false;
            engineUpgraded = false;
            // Create our menu entries.
            MenuEntry upgradeWeaponMenuEntry = new MenuEntry("Upgrade Weapon, Cost: Score -500");
            MenuEntry upgradeShieldMenuEntry = new MenuEntry("Upgrade Shield, Cost: Score -1000");
            MenuEntry upgradeEngineMenuEntry = new MenuEntry("Upgrade Engine, Cost: Score -700");
            MenuEntry resumeGameMenuEntry = new MenuEntry("Resume Game");

            upgradeWeaponMenuEntry.Selected += UpgradeWeaponMenuEntrySelected;
            upgradeShieldMenuEntry.Selected += UpgradeShieldMenuEntrySelected;
            upgradeEngineMenuEntry.Selected += UpgradeEngineMenuEntrySelected;
            resumeGameMenuEntry.Selected += OnCancel;
            
            // Add entries to the menu.
            MenuEntries.Add(upgradeWeaponMenuEntry);
            MenuEntries.Add(upgradeShieldMenuEntry);
            MenuEntries.Add(upgradeEngineMenuEntry);
            MenuEntries.Add(resumeGameMenuEntry);
        }

        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            if (Player.ScoreSystem.RunningScore < 500 || weaponUpgraded)
                MenuEntries[0].Selectable = false;
            if (Player.ScoreSystem.RunningScore < 1000 || shieldUpgraded)
                MenuEntries[1].Selectable = false;
            if (Player.ScoreSystem.RunningScore < 700 || engineUpgraded)
                MenuEntries[2].Selectable = false;
            base.Update(gameTime,otherScreenHasFocus, coveredByOtherScreen);
        }

        void UpgradeWeaponMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            string message;
            if (MenuEntries[0].Selectable)
            {
                message = "Upgrade Weapon?";
                MessageBoxScreen confirmMessageBox = new MessageBoxScreen(message);
                confirmMessageBox.Accepted += ConfirmUpgradeWeaponMessageBoxAccepted;
                ScreenManager.AddScreen(confirmMessageBox, ControllingPlayer);
            }
            else
            {
                if(Player.ScoreSystem.RunningScore < 500)
                    message = "You don't have enough points to upgrade that item!";
                else
                    message = "Can only upgrade this part once per Shop Screen!";
                MessageBoxScreen messageBox = new MessageBoxScreen(message);
                ScreenManager.AddScreen(messageBox, ControllingPlayer);
            }
        }

        void UpgradeShieldMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            string message;
            if (MenuEntries[1].Selectable)
            {
                message = "Upgrade Shield?";

                MessageBoxScreen confirmMessageBox = new MessageBoxScreen(message);

                confirmMessageBox.Accepted += ConfirmUpgradeShieldMessageBoxAccepted;

                ScreenManager.AddScreen(confirmMessageBox, ControllingPlayer);
            }
            else
            {
                if (Player.ScoreSystem.RunningScore < 1000)
                    message = "You don't have enough points to upgrade that item!";
                else
                    message = "Can only upgrade this part once per Shop Screen!";

                MessageBoxScreen messageBox = new MessageBoxScreen(message);

                ScreenManager.AddScreen(messageBox, ControllingPlayer);
            }
        }

        void UpgradeEngineMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            string message;
            if (MenuEntries[2].Selectable)
            {
                message = "Upgrade engine?";

                MessageBoxScreen confirmMessageBox = new MessageBoxScreen(message);

                confirmMessageBox.Accepted += ConfirmUpgradeEngineMessageBoxAccepted;

                ScreenManager.AddScreen(confirmMessageBox, ControllingPlayer);
            }
            else
            {
                if (Player.ScoreSystem.RunningScore < 700)
                    message = "You don't have enough points to upgrade that item!";
                else
                    message = "Can only upgrade this part once per Shop Screen!";

                MessageBoxScreen messageBox = new MessageBoxScreen(message);

                ScreenManager.AddScreen(messageBox, ControllingPlayer);
            }
        }

        void ConfirmUpgradeWeaponMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            Player.MainWeaponPower += 10;
            Player.ScoreSystem.Subtract(500);
            weaponUpgraded = true;
        }

        void ConfirmUpgradeShieldMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            Player.MaxShield += 10;
            Player.ScoreSystem.Subtract(1000);
            shieldUpgraded = true;
        }

        void ConfirmUpgradeEngineMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            Player.Speed += 50.0f;
            Player.ScoreSystem.Subtract(700);
            engineUpgraded = true;
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            base.OnCancel(playerIndex);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            SpriteBatch spriteBatch = ScreenManager.SpriteBatch;
            SpriteFont font = ScreenManager.Font;

            spriteBatch.Begin();

            //Draw player's score in the lower left corner of the screen.
            spriteBatch.DrawString(font, "Score: " + Player.ScoreSystem.RunningScore.ToString(),
                                    new Vector2(0, viewportRect.Height * 0.95f), Color.White);
            spriteBatch.End();
        }
    }
}
