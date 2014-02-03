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

        Texture2D midLine;
        SpriteFont Font1;
        SpriteFont Font2;

        Color[] menuColor = {Color.White, Color.White, Color.White};

        enum MenuState
        {
            SinglePlayer,
            Multiplayer,
            Exit,
        }

        MenuState menuSelect = MenuState.SinglePlayer;

        int menuDelay = 0;

        enum GameState
        {
            MainMenu,
            SinglePlayer,
            Multiplayer,
        }

        GameState currentGamesState = GameState.MainMenu;

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

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //graphics.PreferredBackBufferWidth = 800;
            //graphics.PreferredBackBufferHeight = 600;
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
            Vector2 ballPosition = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + GraphicsDevice.Viewport.TitleSafeArea.Width / 2, GraphicsDevice.Viewport.TitleSafeArea.Y + GraphicsDevice.Viewport.TitleSafeArea.Height / 2);
            Vector2 ballDirection = new Vector2(1.0f, 1.00f);
            ball.Initialize(Content.Load<Texture2D>("ball"), ballPosition, ballDirection);

            // load midline
            midLine = Content.Load<Texture2D>("midLine");

            Font1 = Content.Load<SpriteFont>("Test");
            Font2 = Content.Load<SpriteFont>("SpriteFont2");
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
            

            switch (currentGamesState)
            {

                case GameState.MainMenu:
                    // Allows the game to exit
                    if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                        this.Exit();
                    MenuState NewMenuSelect = menuSelect;

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
                            else if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
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
                            else if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
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
                            else if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed)
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

                case GameState.SinglePlayer:
                    previousGamePadState1 = currentGamePadState1;
                    previousGamePadState2 = currentGamePadState2;

                    currentGamePadState1 = GamePad.GetState(PlayerIndex.One);
                    currentGamePadState2 = GamePad.GetState(PlayerIndex.One);

                    paddle1.Update(gameTime, currentGamePadState1.ThumbSticks.Left.Y);
                    paddle1.Position.Y = MathHelper.Clamp(paddle1.Position.Y, 0, GraphicsDevice.Viewport.Height - paddle1.Height);
                    paddle2.Update(gameTime, currentGamePadState2.ThumbSticks.Left.Y);
                    paddle2.Position.Y = MathHelper.Clamp(paddle2.Position.Y, 0, GraphicsDevice.Viewport.Height - paddle2.Height);

                    ball.Update(gameTime, GraphicsDevice.Viewport.Height);

                    UpdateCollision();
               break;

               case GameState.Multiplayer:
                   previousGamePadState1 = currentGamePadState1;
                   previousGamePadState2 = currentGamePadState2;

                   currentGamePadState1 = GamePad.GetState(PlayerIndex.One);
                   currentGamePadState2 = GamePad.GetState(PlayerIndex.Two);

                   paddle1.Update(gameTime, currentGamePadState1.ThumbSticks.Left.Y);
                   paddle1.Position.Y = MathHelper.Clamp(paddle1.Position.Y, 0, GraphicsDevice.Viewport.Height - paddle1.Height);
                   paddle2.Update(gameTime, currentGamePadState2.ThumbSticks.Left.Y);
                   paddle2.Position.Y = MathHelper.Clamp(paddle2.Position.Y, 0, GraphicsDevice.Viewport.Height - paddle2.Height);

                   ball.Update(gameTime, GraphicsDevice.Viewport.Height);

                   UpdateCollision();
               break;
            }

            base.Update(gameTime);
        }

        private Boolean UpdateCollision()
        {
            Rectangle ballBox;
            Rectangle paddle1Box;
            Rectangle paddle2Box;

            ballBox = new Rectangle((int)ball.Position.X,(int)ball.Position.Y,ball.Width,ball.Height);
            paddle1Box = new Rectangle((int)paddle1.Position.X + paddle1.Width * 3 / 4, (int)paddle1.Position.Y, paddle1.Width / 4, paddle1.Height);
            paddle2Box = new Rectangle((int)paddle2.Position.X, (int)paddle2.Position.Y, paddle2.Width / 4, paddle2.Height);

            if (ballBox.Intersects(paddle1Box))
            {
                ball.Collide(true, currentGamePadState1.ThumbSticks.Left.Y);
                return true;
            }

            if (ballBox.Intersects(paddle2Box))
            {
                ball.Collide(false, currentGamePadState2.ThumbSticks.Left.Y);
                return true;
            }
            return false;
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
                    spriteBatch.DrawString(Font2, "Spin", new Vector2(320,100),Color.White, 0f, new Vector2(0,0), 1f, SpriteEffects.None, 0.5f);
                    spriteBatch.DrawString(Font1, "Singleplayer", new Vector2(320, 160), menuColor[0]);
                    spriteBatch.DrawString(Font1, "Multiplayer", new Vector2(320, 180), menuColor[1]);
                    spriteBatch.DrawString(Font1, "Exit", new Vector2(320, 200), menuColor[2]);
                    break;

                case GameState.Multiplayer:
                case GameState.SinglePlayer:

                    paddle1.Draw(spriteBatch);
                    paddle2.Draw(spriteBatch);
                    ball.Draw(spriteBatch);

                    Vector2 midLinePos = new Vector2(0, -36);

                    while (midLinePos.Y < GraphicsDevice.Viewport.Width)
                    {

                        midLinePos = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + GraphicsDevice.Viewport.TitleSafeArea.Width / 2 - Content.Load<Texture2D>("midLine").Width / 2, midLinePos.Y + 48);
                        spriteBatch.Draw(midLine, midLinePos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    }
                    break;
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
