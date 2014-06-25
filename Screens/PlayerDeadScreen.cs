using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
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
    class PlayerDeadScreen : MenuScreen
    {
        Game game;
        TimeSpan time;
        int score;
        string name;
        SaveGameData saveData;
        SaveGameData oldData;
        SpriteFont font;
        Rectangle viewportRect;
        ContentManager content;
        StorageDevice device;
        Color textColor;
        List<SaveGameData> listScores;

        public PlayerDeadScreen(Game game, TimeSpan timer)
            : base("No more lives remaining!")
        {
            this.game = game;
            IsPopup = true;
            // Create our menu entries.
            MenuEntry trollMenuEntry = new MenuEntry("TrollMode");
            MenuEntry okayMenuEntry = new MenuEntry("Okay");

            trollMenuEntry.Selected += trollMenuEntrySelected;
            okayMenuEntry.Selected += OkayMenuEntrySelected;

            // Add entries to the menu.
            MenuEntries.Add(trollMenuEntry);
            MenuEntries.Add(okayMenuEntry);

            //draw variable
            
            //if (content == null)
            //    content = new ContentManager(ScreenManager.Game.Services, "Content");
            font = game.Content.Load<SpriteFont>("Fonts\\GameFont");
            //font = ScreenManager.Game.Content.Load<SpriteFont>("Fonts\\GameFont");
            viewportRect = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width,
                                                         game.GraphicsDevice.Viewport.Height);

            //HighScore Values
            time = timer;
            score = Player.ScoreSystem.TotalScore;
            name = Player.SaveGameData.playerName;
            saveData = new SaveGameData(time, score, name);
            oldData = new SaveGameData(new TimeSpan(), 0, "Aero");
//#if XBOX
            StorageDevice.BeginShowSelector(PlayerIndex.One, this.GetDevice, (object)"GetDevice for Player One");
//#endif
        }

        void GetDevice(IAsyncResult result)
        {
            device = StorageDevice.EndShowSelector(result);
            if (device != null && device.IsConnected)
            {
                SaveData();
            }
        }

        void SaveData()
        {
            Stream stream;
            XmlSerializer serializer;
            //Open Storage
            IAsyncResult result = device.BeginOpenContainer("Aero", null, null);
            result.AsyncWaitHandle.WaitOne();
            StorageContainer container = device.EndOpenContainer(result);
            result.AsyncWaitHandle.Close();
            //Check for old save
            string filename = "aerosave.sav";
            if (container.FileExists(filename))
            {
                stream = container.OpenFile(filename, FileMode.Open);
                serializer = new XmlSerializer(typeof(SaveGameData));
                oldData = (SaveGameData)serializer.Deserialize(stream);
                stream.Close();
                //container.Dispose();
                if (oldData.score < saveData.score)
                {
                    //container = device.EndOpenContainer(result);
                    //If new Highscore is better, then replace old one
                    container.DeleteFile(filename);
                    //Create new file
                    stream = container.CreateFile(filename);
                    //Convert to XML data
                    serializer = new XmlSerializer(typeof(SaveGameData));
                    serializer.Serialize(stream, saveData);
                    //Close file
                    stream.Close();
                }
                //Dispose Container, commit changes.
                container.Dispose();
            }
            //If no old file
            else
            {
                //Create new file
                stream = container.CreateFile(filename);
                //Convert to XML data
                serializer = new XmlSerializer(typeof(SaveGameData));
                serializer.Serialize(stream, saveData);
                //Close file
                stream.Close();
                //Dispose Container, commit changes.
                container.Dispose();
            }
        }

        void OkayMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            const string message = "Try to beat your High Score!";

            MessageBoxScreen confirmQuitMessageBox = new MessageBoxScreen(message);

            confirmQuitMessageBox.Accepted += ConfirmQuitMessageBoxAccepted;

            ScreenManager.AddScreen(confirmQuitMessageBox, ControllingPlayer);
        }

        void ConfirmQuitMessageBoxAccepted(object sender, PlayerIndexEventArgs e)
        {
            Player.Reset();
            //ScreenManager.AddScreen(new BackgroundScreen(), null);
            ScreenManager.AddScreen(new TitleScreen(game), null);
            //LoadingScreen.Load(ScreenManager, false, null, new TitleScreen(game));
        }

        void trollMenuEntrySelected(object sender, PlayerIndexEventArgs e)
        {
            AeroGame.Troll = true;
            Player.Reset();
            //ScreenManager.AddScreen(new BackgroundScreen(), null);
            ScreenManager.AddScreen(new GamePlayScreen(game), null);
            //LoadingScreen.Load(ScreenManager, false, null, new TitleScreen(game));
        }

        protected override void OnCancel(PlayerIndex playerIndex)
        {
            //ScreenManager.AddScreen(new BackgroundScreen(), null);
            ScreenManager.AddScreen(new TitleScreen(game), null);
            ExitScreen();
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.FadeBackBufferToBlack(TransitionAlpha * 2 / 3);
            AeroGame.SpriteBatch.Begin();
            //if(oldData != null)
            if (oldData.score > score)
            {
                AeroGame.SpriteBatch.DrawString(font, "High Score: " + oldData.score + ", " + oldData.playerName,
                                        new Vector2(viewportRect.Width * 0.5f, viewportRect.Height * 0.4f), Color.Green);
                AeroGame.SpriteBatch.DrawString(font, "Score: " + score +
                                        "\n Time: " + time.Minutes + ":" + time.Seconds + ":" + time.Milliseconds + "\n" + name,
                                        new Vector2(viewportRect.Width * 0.5f, viewportRect.Height * 0.5f), Color.Red);
            }
            else
            {
                AeroGame.SpriteBatch.DrawString(font, "High Score: " + oldData.score + ", " + oldData.playerName,
                                    new Vector2(viewportRect.Width * 0.5f, viewportRect.Height * 0.4f), Color.Red);
                AeroGame.SpriteBatch.DrawString(font, "Score: " + score +
                                        "\n Time: " + time.Minutes + ":" + time.Seconds + ":" + time.Milliseconds + "\n" + name,
                                        new Vector2(viewportRect.Width * 0.5f, viewportRect.Height * 0.5f), Color.Green);
            }
            AeroGame.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
