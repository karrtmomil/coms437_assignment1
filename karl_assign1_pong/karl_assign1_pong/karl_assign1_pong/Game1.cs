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
            // TODO: Add your initialization logic here

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

            // TODO: use this.Content to load your game content here

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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            previousGamePadState1 = currentGamePadState1;
            previousGamePadState2 = currentGamePadState2;

            currentGamePadState1 = GamePad.GetState(PlayerIndex.One);
            currentGamePadState2 = GamePad.GetState(PlayerIndex.One);

            paddle1.Update(gameTime, currentGamePadState1);
            paddle1.Position.Y = MathHelper.Clamp(paddle1.Position.Y, 0, GraphicsDevice.Viewport.Height - paddle1.Height);
            paddle2.Update(gameTime, currentGamePadState2);
            paddle2.Position.Y = MathHelper.Clamp(paddle2.Position.Y, 0, GraphicsDevice.Viewport.Height - paddle2.Height);

            ball.Update(gameTime, GraphicsDevice.Viewport.Height);

            UpdateCollision();


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

            // TODO: Add your drawing code here

            spriteBatch.Begin();

            paddle1.Draw(spriteBatch);
            paddle2.Draw(spriteBatch);
            ball.Draw(spriteBatch);

            Vector2 midLinePos = new Vector2(0,-36);

            while (midLinePos.Y < GraphicsDevice.Viewport.Width)
            {

                midLinePos = new Vector2(GraphicsDevice.Viewport.TitleSafeArea.X + GraphicsDevice.Viewport.TitleSafeArea.Width / 2 - Content.Load<Texture2D>("midLine").Width / 2, midLinePos.Y + 48);
                spriteBatch.Draw(midLine, midLinePos, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
