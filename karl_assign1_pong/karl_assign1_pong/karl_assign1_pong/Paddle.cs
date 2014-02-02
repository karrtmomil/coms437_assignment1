using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace karl_assign1_pong
{
    class Paddle
    {
        // Animation representing the paddle
        public Texture2D PaddleTexture;

        // Postion
        public Vector2 Position;

        // constant Paddle move speed
        public float paddleMoveSpeed = 8.0f;

        public Boolean OneGamepad;

        // Get the width of the paddle
        public int Width
        {
            get { return PaddleTexture.Width; }
        }

        // Get the height of the paddle
        public int Height
        {
            get { return PaddleTexture.Height; }
        }

        public void Initialize(Texture2D texture, Vector2 position, Boolean oneGamepad)
        {
            PaddleTexture = texture;

            Position = new Vector2(position.X - this.Width / 2, position.Y - this.Height / 2);

            OneGamepad = oneGamepad;
        }

        public void Update(GameTime gameTime, GamePadState currentGamePadState)
        {
            if (!OneGamepad)
            {
                this.Position.Y -= currentGamePadState.ThumbSticks.Left.Y * paddleMoveSpeed;

                if (currentGamePadState.DPad.Up == ButtonState.Pressed)
                {
                    Position.Y -= paddleMoveSpeed;
                }
                if (currentGamePadState.DPad.Down == ButtonState.Pressed)
                {
                    Position.Y += paddleMoveSpeed;
                }
            }
            else
            {
                Position.Y -= currentGamePadState.ThumbSticks.Right.Y * paddleMoveSpeed;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(PaddleTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
