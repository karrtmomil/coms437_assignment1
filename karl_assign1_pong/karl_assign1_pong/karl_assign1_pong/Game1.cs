using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace karl_assign1_pong
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Paddle paddle1;
        Paddle paddle2;
        Ball ball;
        Computer computer;
        SpeedPower speedPower;
        StrobePower strobePower;

        int score1 = 0;
        int score2 = 0;

        Texture2D midLine;
        SpriteFont Font1;
        SpriteFont Font2;
        Random Rand;

        Color[] menuColor = {Color.White, Color.White, Color.White, Color.White, Color.White};

        enum MenuState
        {
            SinglePlayer,
            Multiplayer,
            Exit,
        }
        
        MenuState menuSelect = MenuState.SinglePlayer;

        enum EndState
        {
            MainMenu,
            Exit,
        }

        EndState endSelect = EndState.MainMenu;

        int menuDelay = 0;

        enum GameState
        {
            MainMenu,
            SinglePlayer,
            Multiplayer,
            ScoreSreen,
        }

        GameState currentGamesState = GameState.MainMenu;
        //GameState currentGamesState = GameState.SinglePlayer;
        

        // Keyboard states used to determine key presses
        //KeyboardState currentKeyboardState;
        //KeyboardState previousKeyboardState;

        // Gamepad states used to determine button presses
        GamePadState currentGamePadState1;
        GamePadState previousGamePadState1;
        GamePadState currentGamePadState2;
        GamePadState previousGamePadState2;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Initialize paddles
            paddle1 = new Paddle();
            paddle2 = new Paddle();

            // Initialize ball
            ball = new Ball();

            // Initialize the computer
            computer = new Computer();

            // Initialize the powerUps
            speedPower = new SpeedPower();
            strobePower = new StrobePower();

            Rand = new Random();

            base.Initialize();
        }

        private void resetGame()
        {
            score1 = 0;
            score2 = 0;
            
            float direction = Rand.Next(2) * 2 - 1;
            Vector2 ballPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + GraphicsDevice.Viewport.TitleSafeArea.Width / 2, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            Vector2 ballDirection = new Vector2(direction, (float)(Rand.NextDouble() * 1.5 - 1));
            ball.Reset(ballPosition, ballDirection, 500);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //graphics.PreferredBackBufferWidth = 400;
            //graphics.PreferredBackBufferHeight = 220;
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();

            // Load the paddles
            float buffer = 18;

            Vector2 paddlePosition1 = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + buffer, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            paddle1.Initialize(Content.Load<Texture2D>("paddle"), paddlePosition1);

            Vector2 paddlePosition2 = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + GraphicsDevice.Viewport.TitleSafeArea.Width - buffer,
                                                    GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            paddle2.Initialize(Content.Load<Texture2D>("paddle"), paddlePosition2);

            // Load the ball
            float direction = Rand.Next(2) * 2 - 1;
            Vector2 ballPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + GraphicsDevice.Viewport.TitleSafeArea.Width / 2, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            Vector2 ballDirection = new Vector2(direction, (float)(Rand.NextDouble() * 1.5 - 1));
            ball.Initialize(Content.Load<Texture2D>("ball"), ballPosition, ballDirection, 1000);

            // load midline
            midLine = Content.Load<Texture2D>("midLine");

            // load the powerUps
            
            Rectangle spawnArea = new Rectangle((int)paddlePosition1.X + paddle1.Width,
                GraphicsDevice.Viewport.Y + (int)buffer,
                GraphicsDevice.Viewport.Width - (int)buffer * 4 - paddle1.Width * 2 - Content.Load<Texture2D>("speed").Width,
                GraphicsDevice.Viewport.Height - (int)buffer * 2 - Content.Load<Texture2D>("speed").Height);
            speedPower.Initialize(Content.Load<Texture2D>("speed3"), spawnArea);
            strobePower.Initialize(Content.Load<Texture2D>("strobe2"), spawnArea);

            Font1 = Content.Load<SpriteFont>("Font1");
            Font2 = Content.Load<SpriteFont>("Font2");
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
            previousGamePadState1 = currentGamePadState1;
            previousGamePadState2 = currentGamePadState2;

            currentGamePadState1 = GamePad.GetState(PlayerIndex.One);
            currentGamePadState2 = GamePad.GetState(PlayerIndex.Two);

            switch (currentGamesState)
            {
                #region MainMenu

                case GameState.MainMenu:
                    // Allows the game to exit
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                        this.Exit();
                    MenuState NewMenuSelect = menuSelect;

                    resetGame();

                    switch (menuSelect)
                    {
                        case MenuState.SinglePlayer:
                            menuColor[0] = Color.Red;
                            menuColor[1] = Color.White;
                            menuColor[2] = Color.White;

                            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < -0.9)
                            {
                                NewMenuSelect = MenuState.Multiplayer;                       
                            }
                            else if (currentGamePadState1.Buttons.A == ButtonState.Released && previousGamePadState1.Buttons.A == ButtonState.Pressed)
                            {
                                currentGamesState = GameState.SinglePlayer;
                            }

                        break;
                        
                        case MenuState.Multiplayer:
                            menuColor[0] = Color.White;
                            menuColor[1] = Color.Red;
                            menuColor[2] = Color.White;

                            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0.9)
                            {
                                NewMenuSelect = MenuState.SinglePlayer;
                            }
                            else if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < -0.9)
                            {
                                NewMenuSelect = MenuState.Exit;
                            }
                            else if (currentGamePadState1.Buttons.A == ButtonState.Released && previousGamePadState1.Buttons.A == ButtonState.Pressed)
                            {
                                currentGamesState = GameState.Multiplayer;
                            }
                        break;

                        case MenuState.Exit:
                            menuColor[0] = Color.White;
                            menuColor[1] = Color.White;
                            menuColor[2] = Color.Red;

                            if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0.9)
                            {
                                NewMenuSelect = MenuState.Multiplayer;
                            }
                            else if (currentGamePadState1.Buttons.A == ButtonState.Released && previousGamePadState1.Buttons.A == ButtonState.Pressed)
                            {
                                this.Exit();
                            }
                        break;
                    }

                    if (gameTime.ElapsedGameTime.Milliseconds > menuDelay)
                    {
                        menuSelect = NewMenuSelect;
                        menuDelay = 100;
                    }
                    else
                    {
                        menuDelay -= gameTime.ElapsedGameTime.Milliseconds;
                    }
                break;

                #endregion

                #region SinglePlayer

                case GameState.SinglePlayer:
                    
                    paddle2.Update(gameTime, computer.Move(ball, paddle2));
                    paddle2.Position.Y = MathHelper.Clamp(paddle2.Position.Y, 0, GraphicsDevice.Viewport.Height - paddle2.Height);

                    UpdateGame(gameTime);
                break;

                #endregion

                #region Multiplayer

                case GameState.Multiplayer:
                    
                    paddle2.Update(gameTime, currentGamePadState2.ThumbSticks.Left.Y);
                    paddle2.Position.Y = MathHelper.Clamp(paddle2.Position.Y, 0, GraphicsDevice.Viewport.Height - paddle2.Height);

                    UpdateGame(gameTime);                    
                break;

                #endregion

                #region ScoreScreen

                case GameState.ScoreSreen:

                    EndState newEndSelect = endSelect;
                    switch (endSelect)
                    {
                        case EndState.MainMenu:
                           menuColor[3] = Color.Red;
                           menuColor[4] = Color.White;

                           if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y < -0.9)
                           {
                               newEndSelect = EndState.Exit;
                           }
                           else if (currentGamePadState1.Buttons.A == ButtonState.Released && previousGamePadState1.Buttons.A == ButtonState.Pressed)
                           {
                               currentGamesState = GameState.MainMenu;
                           }

                        break;

                        case EndState.Exit:
                           menuColor[3] = Color.White;
                           menuColor[4] = Color.Red;

                           if (GamePad.GetState(PlayerIndex.One).ThumbSticks.Left.Y > 0.9)
                           {
                               newEndSelect = EndState.MainMenu;
                           }
                           else if (currentGamePadState1.Buttons.A == ButtonState.Released && previousGamePadState1.Buttons.A == ButtonState.Pressed)
                           {
                               this.Exit();
                           }
                        break;
                
                    }
                   if (gameTime.ElapsedGameTime.Milliseconds > menuDelay)
                   {
                       endSelect = newEndSelect;
                       menuDelay = 100;
                   }
                   else
                   {
                       menuDelay -= gameTime.ElapsedGameTime.Milliseconds;
                   }
                break;
                #endregion
            }            

            base.Update(gameTime);
        }

        private void UpdateGame(GameTime gameTime)
        {
            paddle1.Update(gameTime, currentGamePadState1.ThumbSticks.Left.Y);
            paddle1.Position.Y = MathHelper.Clamp(paddle1.Position.Y, 0, GraphicsDevice.Viewport.Height - paddle1.Height);

            ball.Update(gameTime, GraphicsDevice.Viewport.Height);

            UpdateCollision();
            CheckForScore();

            speedPower.Update(gameTime, Rand);
            strobePower.Update(gameTime, Rand);
        }

        private void UpdateCollision()
        {
            Rectangle ballBox;
            Rectangle paddle1Box;
            Rectangle paddle2Box;
            Rectangle speedBox;
            Rectangle strobeBox;

            ballBox = new Rectangle((int)ball.Position.X,(int)ball.Position.Y,ball.Width,ball.Height);
            paddle1Box = new Rectangle((int)paddle1.Position.X + paddle1.Width * 3 / 4, (int)paddle1.Position.Y, paddle1.Width / 4, paddle1.Height);
            paddle2Box = new Rectangle((int)paddle2.Position.X, (int)paddle2.Position.Y, paddle2.Width / 4, paddle2.Height);
            speedBox = new Rectangle((int)speedPower.Position.X, (int)speedPower.Position.Y, speedPower.Width, speedPower.Height);
            strobeBox = new Rectangle((int)strobePower.Position.X, (int)strobePower.Position.Y, strobePower.Width, strobePower.Height);

            if (ballBox.Intersects(paddle1Box))
            {
                ball.Collide(true, currentGamePadState1.ThumbSticks.Left.Y);
            }

            if (ballBox.Intersects(paddle2Box))
            {
                ball.Collide(false, currentGamePadState2.ThumbSticks.Left.Y);
            }

            if (ballBox.Intersects(speedBox))
            {
                speedPower.Collide(ball, Rand);
            }

            if (ballBox.Intersects(strobeBox))
            {
                strobePower.Collide(ball);
            }
        }

        private void CheckForScore()
        {
            Rectangle ballBox;
            Rectangle screenBox;

            ballBox = new Rectangle((int)ball.Position.X, (int)ball.Position.Y, ball.Width, ball.Height);
            screenBox = new Rectangle(GraphicsDevice.Viewport.X, GraphicsDevice.Viewport.Y, GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);

            if (!ballBox.Intersects(screenBox))
            {
                float direction = Rand.Next(2) * 2 - 1;
                Vector2 ballPosition = new Vector2(GraphicsDevice.Viewport.X + GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Y + GraphicsDevice.Viewport.Height / 2);
                if (ball.Position.X < 0)
                {
                    score2++;
                    direction = 1.0f;
                    if (score2 >= 7)
                    {
                        currentGamesState = GameState.ScoreSreen;
                    }
                }

                if (ball.Position.X > GraphicsDevice.Viewport.X + GraphicsDevice.Viewport.Width)
                {
                    score1++;
                    direction = -1.0f;
                    if (score1 >= 7)
                    {
                        currentGamesState = GameState.ScoreSreen;
                    }
                }

                Vector2 ballDirection = new Vector2(direction, (float)(Rand.NextDouble() * 1.5 - 1));
                ball.Reset(ballPosition, ballDirection, 500);
               
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();
            switch (currentGamesState)
            {
                case GameState.MainMenu:
                    Vector2 origin = Font2.MeasureString("Spin") / 2;
                    spriteBatch.DrawString(Font2, "Spin", new Vector2(GraphicsDevice.Viewport.Width / 2, 100), Color.White, 0f, origin, 1f, SpriteEffects.None, 0.5f);
                    spriteBatch.DrawString(Font1, "Singleplayer", new Vector2(GraphicsDevice.Viewport.Width / 2, 160), menuColor[0], 0f, origin, 1f, SpriteEffects.None, 0.5f);
                    spriteBatch.DrawString(Font1, "Multiplayer", new Vector2(GraphicsDevice.Viewport.Width / 2, 180), menuColor[1], 0f, origin, 1f, SpriteEffects.None, 0.5f);
                    spriteBatch.DrawString(Font1, "Exit", new Vector2(GraphicsDevice.Viewport.Width / 2, 200), menuColor[2], 0f, origin, 1f, SpriteEffects.None, 0.5f);
                    break;
                case GameState.ScoreSreen:
                    if (score1 > score2)
                    {
                        Vector2 origin1 = Font2.MeasureString("Player One Wins!") / 2;
                        spriteBatch.DrawString(Font2, "Player One Wins!", new Vector2(GraphicsDevice.Viewport.Width / 2, 100), Color.White, 0f, origin1, 1f, SpriteEffects.None, 0.5f);
                    }
                    else
                    {
                        Vector2 origin2 = Font2.MeasureString("Player Two Wins!") / 2;
                        spriteBatch.DrawString(Font2, "Player Two Wins!", new Vector2(GraphicsDevice.Viewport.Width / 2, 100), Color.White, 0f, origin2, 1f, SpriteEffects.None, 0.5f);
                    }

                    spriteBatch.DrawString(Font1, "MainMenu", new Vector2(GraphicsDevice.Viewport.Width / 2 + 50, 200), menuColor[3], 0f, new Vector2(0,0), 1f, SpriteEffects.None, 0.5f);
                    spriteBatch.DrawString(Font1, "Exit", new Vector2(GraphicsDevice.Viewport.Width / 2 + 50, 220), menuColor[4], 0f, new Vector2(0,0), 1f, SpriteEffects.None, 0.5f);

                    DrawGame();

                    break;
                case GameState.Multiplayer:
                case GameState.SinglePlayer:

                    DrawGame();
                    
                break;
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawGame()
        {

            Vector2 midLinePos = new Vector2(0, -36);

            while (midLinePos.Y < GraphicsDevice.Viewport.Width)
            {

                midLinePos = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + GraphicsDevice.Viewport.TitleSafeArea.Width / 2 - Content.Load<Texture2D>("midLine").Width / 2, midLinePos.Y + 48);
                spriteBatch.Draw(midLine, midLinePos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }

            paddle1.Draw(spriteBatch);
            paddle2.Draw(spriteBatch);
            ball.Draw(spriteBatch);
            speedPower.Draw(spriteBatch);
            strobePower.Draw(spriteBatch);

            Vector2 origin1 = Font2.MeasureString(score1.ToString()) / 2;
            Vector2 origin2 = Font2.MeasureString(score2.ToString()) / 2;

            spriteBatch.DrawString(Font2, score1.ToString(), new Vector2(GraphicsDevice.Viewport.Width * 7 / 16, GraphicsDevice.Viewport.Height / 10), Color.White, 0f, origin1, 1f, SpriteEffects.None, 0.5f);
            spriteBatch.DrawString(Font2, score2.ToString(), new Vector2(GraphicsDevice.Viewport.Width * 9 / 16, GraphicsDevice.Viewport.Height / 10), Color.White, 0f, origin1, 1f, SpriteEffects.None, 0.5f);
            
        }
    }
}
