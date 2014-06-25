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
    class GamePlayScreen : GameScreen
    {
        long loadTime;
        ContentManager content;
        //GraphicsDeviceManager graphics;
        static public int level;
        static protected int objectsSpawned;
        static protected bool paused;
        static protected bool shopActive;
        TimeSpan elapsedTime;
        public Level currentLevel;
        static public List<Level> levels;
       //static public Player Player;/////
        //static protected Cruiser[] cruisers;///
        //protected const int maxCruisers = 50;//
        //static protected Fighter fighter;////
        //static protected Kamacazie kama;//////
        protected float spawnCruiserCooldown;//////
        protected float startShopCooldown;
        protected Texture2D backgroundBackTexture1;///
        protected Texture2D backgroundBackTexture2;///
        protected Texture2D backgroundFrontTexture1;///
        protected Texture2D backgroundFrontTexture2;///
        static protected IGraphicsDeviceService graphicsService;
        static protected GraphicsDevice device;
        protected SpriteFont font;////
        //static protected PowerUp powerUp;///
        //private Color[] playerTextureData;
        //private Color[] cruiserTextureData;
        //private Color[] fighterTextureData;
        //private Color[] roundTextureData;
        //private Color[] bossTextureData;
        protected Rectangle viewportRect;
        protected Game game;
        protected SpriteBatch spriteBatch;/////
        Color guageColor;
        float guageProgress;
        int collisionCounter;
        int bgBackY1;
        int bgBackY2;
        int bgFrontY1;
        int bgFrontY2;
        //protected ShopScreen shopScreen;
        protected LevelOne levelOne;
        protected LevelFour levelTwo;
        protected LevelThree levelThree;
        protected LevelTwo levelFour;
        protected LevelFive levelFive;
        protected LevelSurvival levelSurvival;
        NetworkSession session;
        protected string stringLevel;
        protected TimeSpan timer;
        protected string minutes;
        protected string seconds;
        protected Texture2D specialGuageFill;
        protected Texture2D specialGuage;
        protected Texture2D sword32;
        protected Texture2D shield32;


        public GamePlayScreen(Game game)
        {
            this.game = game;
            graphicsService = (IGraphicsDeviceService)game.Services.GetService(typeof(IGraphicsDeviceService));
            device = graphicsService.GraphicsDevice;
            spriteBatch = new SpriteBatch(game.GraphicsDevice);

            TransitionOnTime = TimeSpan.FromSeconds(0.5);
            TransitionOffTime = TimeSpan.FromSeconds(0.5);
        }

        public override void LoadContent()
        {
            long start = DateTime.Now.Ticks;

            if (content == null)
                content = new ContentManager(ScreenManager.Game.Services, "Content");
            level = 1;
            stringLevel = "Level " + level.ToString();
            timer = new TimeSpan();
            Level.LoadContent();
            //player = new Player();
            levels = new List<Level>();
            levels.Add(new LevelOne());
            levels.Add(new LevelTwo());
            levels.Add(new LevelThree());
            levels.Add(new LevelFour());
            levels.Add(new LevelFive());
            levels.Add(new LevelSurvival());
            /*levelOne = new LevelOne();
            levelTwo = new LevelTwo();
            levelThree = new LevelThree();
            levelFour = new LevelFour();
            levelFive = new LevelFive();
            levelSurvival = new LevelSurvival();
            currentLevel = levelOne;*/
            objectsSpawned = 0;
            paused = false;
            shopActive = false;
            if (AeroGame.lagTest)
                backgroundBackTexture1 = AeroGame.LoadTextureStream("backgroundBack");
            else
                backgroundBackTexture1 = AeroGame.ContentManager.Load<Texture2D>("Textures\\backgroundBack");
            if (AeroGame.lagTest)
                backgroundBackTexture2 = AeroGame.LoadTextureStream("backgroundBack");
            else
                backgroundBackTexture2 = AeroGame.ContentManager.Load<Texture2D>("Textures\\backgroundBack");
            if (AeroGame.lagTest)
                backgroundFrontTexture1 = AeroGame.LoadTextureStream("backgroundFront");
            else
                backgroundFrontTexture1 = AeroGame.ContentManager.Load<Texture2D>("Textures\\backgroundFront");
            if (AeroGame.lagTest)
                backgroundFrontTexture2 = AeroGame.LoadTextureStream("backgroundFront");
            else
                backgroundFrontTexture2 = AeroGame.ContentManager.Load<Texture2D>("Textures\\backgroundFront");

            //backgroundBackTexture1 = ScreenManager.Game.Content.Load<Texture2D>("Textures\\backgroundBack");
            //backgroundBackTexture2 = ScreenManager.Game.Content.Load<Texture2D>("Textures\\backgroundBack");
            //backgroundFrontTexture1 = ScreenManager.Game.Content.Load<Texture2D>("Textures\\backgroundFront");
            //backgroundFrontTexture2 = ScreenManager.Game.Content.Load<Texture2D>("Textures\\backgroundFront");

            bgBackY1 = bgFrontY1 = 0;
            bgBackY2 = bgFrontY2 = -900;
            
            font = ScreenManager.Game.Content.Load<SpriteFont>("Fonts\\GameFont");
            specialGuageFill = ScreenManager.Game.Content.Load<Texture2D>("Textures\\SpecialGuageFill");
            specialGuage = ScreenManager.Game.Content.Load<Texture2D>("Textures\\SpecialGuage");
            sword32 = ScreenManager.Game.Content.Load<Texture2D>("Textures\\Sword32");
            shield32 = ScreenManager.Game.Content.Load<Texture2D>("Textures\\shield32");
            
            spawnCruiserCooldown = 1.0f;
            startShopCooldown = 0;
            viewportRect = new Rectangle(0, 0, game.GraphicsDevice.Viewport.Width,
                                                         game.GraphicsDevice.Viewport.Height);
            collisionCounter = 0;
            guageColor = Color.White;
            guageProgress = 0;

            loadTime = (DateTime.Now.Ticks - start) / 10000;
            AeroGame.GameSong.Play();
#if XBOX
            session = NetworkSession.Create( 
                NetworkSessionType.SystemLink, 1, 2, 1, new NetworkSessionProperties());
            Player.SaveGameData = new SaveGameData(new TimeSpan(), 0, session.LocalGamers[0].Gamertag); 
#endif
#if !XBOX
            Player.SaveGameData = new SaveGameData(new TimeSpan(), 0, "Player");
#endif
            //shopScreen = new ShopScreen(game);
        }
        
        public override void Update(GameTime gameTime, bool otherScreenHasFocus,
                                                       bool coveredByOtherScreen)
        {
            base.Update(gameTime, otherScreenHasFocus, coveredByOtherScreen);
            elapsedTime = gameTime.ElapsedGameTime;
            if (!paused)
            {
                timer += elapsedTime;
                //Player.SaveGameData.AddTime(elapsedTime);
                //currentLevel.Update(elapsedTime);
                //currentLevel.OffScreenDetection();
                //currentLevel.CollisionDetection();
                levels[level - 1].Update(elapsedTime);
                levels[level - 1].OffScreenDetection();
                levels[level - 1].CollisionDetection();
                //if (currentLevel.Done)
                if (levels[level - 1].Done)
                {
                    //if (currentLevel is LevelSurvival)
                    if (levels[level - 1] is LevelSurvival)
                    {
                        shopActive = true;
                        levels[level - 1].Done = false;
                    }
                    else
                    {
                        level++;
                        stringLevel = "Level: " + level.ToString();
                        shopActive = true;
                        /*if (level == 2)
                            //currentLevel = new LevelTwo();
                            currentLevel = levelTwo;
                        else if (level == 3)
                            //currentLevel = new LevelThree();
                            currentLevel = levelThree;
                        else if (level == 4)
                            //currentLevel = new LevelFour();
                            currentLevel = levelFour;
                        else if (level == 5)
                            //currentLevel = new LevelFive();
                            currentLevel = levelFive;
                        else if (level == 6)
                            //currentLevel = new LevelSurvival();
                            currentLevel = levelSurvival;
                        */
                    }
                }
                if (Player.Lives < 0 && !Player.Exploding && !this.IsExiting)
                {
                    ScreenManager.AddScreen(new BackgroundScreen(), null);
                    ScreenManager.AddScreen(new PlayerDeadScreen(game, timer), ControllingPlayer);
                    ExitScreen();
                    //LoadingScreen.Load(ScreenManager, true, e.PlayerIndex, new GamePlayScreen(game));
                }
                
            }
            bgBackY1 += (int)(100.0f * elapsedTime.TotalSeconds);
            bgBackY2 += (int)(100.0f * elapsedTime.TotalSeconds);
            if (bgBackY1 > 900)
                bgBackY1 = bgBackY2 - 900;
            if (bgBackY2 > 900)
                bgBackY2 = bgBackY1 - 900;
            bgFrontY1 += (int)(150.0f * elapsedTime.TotalSeconds);
            bgFrontY2 += (int)(150.0f * elapsedTime.TotalSeconds);
            if (bgFrontY1 > 900)
                bgFrontY1 = bgFrontY2 - 900;
            if (bgFrontY2 > 900)
                bgFrontY2 = bgFrontY1 - 900;

            if (Player.SecondaryWeapon.CoolDown >= Player.SecondaryWeapon.CoolDownLimit)
            {
                guageColor = Color.Green;
                guageProgress = 1;
            }
            else
            {
                guageColor = Color.Red;
                guageProgress = Player.SecondaryWeapon.CoolDown / Player.SecondaryWeapon.CoolDownLimit;
            }
            
        }
        
        public override void HandleInput(InputState input)
        {
            //currentLevel.HandleInput(input);
            levels[level - 1].HandleInput(input);
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
#if !XBOX
            KeyboardState keyboardState = Keyboard.GetState();
            if (input.IsPauseGame(ControllingPlayer))
            {
                ScreenManager.AddScreen(new PauseScreen(game), ControllingPlayer);
                paused = true;
            }
            else if (shopActive)
            {
                //ScreenManager.AddScreen(new LevelTwo(game), ControllingPlayer);
                ShopScreen shopScreen = new ShopScreen(game);
                //LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
                //               new TitleScreen(game));
                ScreenManager.AddScreen(shopScreen, ControllingPlayer);
                paused = true;
                shopActive = false;
            }
            else
            {
                paused = false;
                if ((gamePadState.ThumbSticks.Left.X != 0 || gamePadState.ThumbSticks.Left.Y != 0) && !Player.Reviving && !paused)
                    Player.move(gamePadState.ThumbSticks.Left.X, gamePadState.ThumbSticks.Left.Y);
                if ((keyboardState.IsKeyDown(Keys.Up) || gamePadState.IsButtonDown(Buttons.DPadUp)) && !Player.Reviving && !paused)
                {
                    Player.moveUp();
                }
                if ((keyboardState.IsKeyDown(Keys.Down) || gamePadState.IsButtonDown(Buttons.DPadDown)) && !Player.Reviving && !paused)
                {
                    Player.moveDown();
                }
                if ((keyboardState.IsKeyDown(Keys.Left) || gamePadState.IsButtonDown(Buttons.DPadLeft)) && !Player.Reviving && !paused)
                {
                    Player.moveLeft();
                }
                if ((keyboardState.IsKeyDown(Keys.Right) || gamePadState.IsButtonDown(Buttons.DPadRight)) && !Player.Reviving && !paused)
                {
                    Player.moveRight();
                }
                if ((keyboardState.IsKeyDown(Keys.Z) || gamePadState.IsButtonDown(Buttons.RightTrigger)) && !paused)
                {
                    if (Player.PrimaryWeapon.CoolDown <= 0 && Player.Alive)
                    {
                        Player.FirePrimary((float)Math.PI/2);
                    }
                }
                if ((keyboardState.IsKeyDown(Keys.X) || gamePadState.IsButtonDown(Buttons.LeftTrigger)) && !paused)
                {
                    if (Player.SecondaryWeapon.CoolDown >= Player.SecondaryWeapon.CoolDownLimit && Player.Alive)
                    {
                        Player.FireSecondary((float)Math.PI / 2);
                    }
                }
            }
#endif
#if XBOX
            if (input.IsPauseGame(ControllingPlayer))
            {
                ScreenManager.AddScreen(new PauseScreen(game), ControllingPlayer);
                paused = true;
            }
            else if (shopActive)
            {
                //ScreenManager.AddScreen(new LevelTwo(game), ControllingPlayer);
                ShopScreen shopScreen = new ShopScreen(game);
                ScreenManager.AddScreen(shopScreen, ControllingPlayer);
                paused = true;
                shopActive = false;
            }
            else
            {
                paused = false;
                if ((gamePadState.ThumbSticks.Left.X != 0 || gamePadState.ThumbSticks.Left.Y != 0) && !Player.Reviving && !paused)
                    Player.move(gamePadState.ThumbSticks.Left.X, gamePadState.ThumbSticks.Left.Y);
                if (gamePadState.IsButtonDown(Buttons.RightTrigger) && !paused)
                    if (Player.PrimaryWeapon.CoolDown <= 0 && Player.Alive)
                        Player.FirePrimary((float)Math.PI / 2);
                if (gamePadState.IsButtonDown(Buttons.LeftTrigger) && !paused)
                    if (Player.SecondaryWeapon.CoolDown >= Player.SecondaryWeapon.CoolDownLimit && Player.Alive)
                        Player.FireSecondary((float)Math.PI / 2);
            }
#endif
        }

        public override void Draw(GameTime gameTime)
        {
            ScreenManager.GraphicsDevice.Clear(ClearOptions.Target,
                                               Color.Black, 0, 0);
            //Draw Background.
            spriteBatch.Begin();
            //device.RenderState.DepthBufferWriteEnable = false;
            spriteBatch.Draw(backgroundBackTexture1, new Rectangle(0, bgBackY1, backgroundBackTexture1.Width, backgroundBackTexture1.Height), Color.White);
            spriteBatch.Draw(backgroundBackTexture2, new Rectangle(0, bgBackY2, backgroundBackTexture2.Width, backgroundBackTexture2.Height), Color.White);
            spriteBatch.Draw(backgroundFrontTexture1, new Rectangle(0, bgFrontY1, backgroundFrontTexture1.Width, backgroundFrontTexture1.Height), Color.White);
            spriteBatch.Draw(backgroundFrontTexture2, new Rectangle(0, bgFrontY2, backgroundFrontTexture2.Width, backgroundFrontTexture2.Height), Color.White);
            spriteBatch.End();

            //Draw all other objects.
            AeroGame.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            ////Draw Enemies.
            //foreach (Cruiser e in cruisers)
            //{
            //    e.Draw();
            //}
            ////Draw fighter
            //fighter.Draw();
            ////Draw Kamacazie
            //kama.Draw();
            ////Draw Boss.
            //boss.Draw();

            
            //Draw level in the upper right corner of the screen.
            AeroGame.SpriteBatch.DrawString(font, stringLevel,
                                    new Vector2(viewportRect.Width * 0.85f, 0), Color.White);
            //Draw player's score in the lower left corner of the screen.
            AeroGame.SpriteBatch.DrawString(font, Player.ScoreSystem.ScoreString, 
                new Vector2(0, viewportRect.Height * 0.95f), Color.White);
            //Draw Player name and time
            AeroGame.SpriteBatch.DrawString(font, Player.SaveGameData.playerName,
                //timer.Minutes + ":" + timer.Seconds,
                new Vector2(0, 0), Color.White);
            //Draw player's special guage.
            AeroGame.SpriteBatch.Draw(specialGuageFill, new Vector2(viewportRect.Width * 0.2f, viewportRect.Height * 0.96f),
                new Rectangle(0, 0, (int)(specialGuageFill.Width * guageProgress), specialGuageFill.Height), guageColor);
            AeroGame.SpriteBatch.Draw(specialGuage, new Vector2(viewportRect.Width * 0.2f, viewportRect.Height * 0.96f), guageColor);
            //Draw counter for player's Attack.
            AeroGame.SpriteBatch.Draw(sword32, new Vector2(viewportRect.Width * 0.65f, viewportRect.Height * 0.95f), Color.White);
            AeroGame.SpriteBatch.DrawString(font, Player.StringMainWeaponPower,
                                    new Vector2(viewportRect.Width * 0.7f, viewportRect.Height * 0.95f), Color.White);
            //Draw counter for player's Shield.
            AeroGame.SpriteBatch.Draw(shield32, new Vector2(viewportRect.Width * 0.75f, viewportRect.Height * 0.95f), Color.White);
            AeroGame.SpriteBatch.DrawString(font, Player.StringShield,
                                    new Vector2(viewportRect.Width * 0.8f, viewportRect.Height * 0.95f), Color.White);
            //Draw counter for player's lives.
            AeroGame.SpriteBatch.Draw(Player.Texture, new Rectangle((int)(viewportRect.Width * 0.9f), (int)(viewportRect.Height * 0.95f), 32, 32),
                new Rectangle(0, 0, 42, 64), Color.White);
            AeroGame.SpriteBatch.DrawString(font, Player.StringLives,
                                    new Vector2(viewportRect.Width * 0.95f, viewportRect.Height * 0.95f), Color.White);
            //currentLevel.Draw();

            
            levels[level - 1].Draw();
            Player.Draw();
            AeroGame.SpriteBatch.End();

            if (paused)
            {
                ScreenManager.FadeBackBufferToBlack(128);
                //pauseScreen.draw(gametime);
                //Screen screen = new TitleScreen(game);
                //screen.activate(graphics);
                //screen.render();
            }
        }

        public void GameOver()
        {
            //LoadingScreen.Load(ScreenManager, true, e.PlayerIndex,
            //                   new GamePlayScreen(game));
        }

        
    }
}