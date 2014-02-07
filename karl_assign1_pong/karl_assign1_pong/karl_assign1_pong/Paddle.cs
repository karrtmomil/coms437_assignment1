using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace karl_assign1_pong
{
    class Paddle
    {
        // Texture representing the paddle
        public Texture2D PaddleTexture;

        // Postion
        public Vector2 Position;

        // constant Paddle move speed
        public float paddleMoveSpeed = 10.0f;

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

        public void Initialize(Texture2D texture, Vector2 position)
        {
            PaddleTexture = texture;

            Position = new Vector2(position.X - this.Width / 2, position.Y - this.Height / 2);
        }

        public void Update(GameTime gameTime, float leftStickY)
        {
            
            this.Position.Y -= leftStickY * paddleMoveSpeed;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(PaddleTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
