using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace karl_assign1_pong
{
    class Ball
    {
        // Animation representing the ball
        public Texture2D BallTexture;

        // Postion
        public Vector2 Position;

        public const float StartSpeed = 2.0f;

        public float CurrentSpeed;

        public const float MaxSpeed = 12.0f;

        public Vector2 Direction;

        // Get the width of the ball
        public int Width
        {
            get { return BallTexture.Width; }
        }

        // Get the height of the ball
        public int Height
        {
            get { return BallTexture.Height; }
        }

        public void Initialize(Texture2D texture, Vector2 position, Vector2 direction)
        {
            BallTexture = texture;

            Position = new Vector2(position.X - this.Width / 2, position.Y - this.Height / 2);

            CurrentSpeed = StartSpeed;

            Direction = direction;
        }

        public void Reset(Vector2 position, Vector2 direction)
        {
            Position = new Vector2(position.X - this.Width / 2, position.Y - this.Height / 2);

            CurrentSpeed = StartSpeed;

            Direction = direction;
        }

        public void Update(GameTime gameTime, float maxHeight)
        {
            Direction.Normalize();
            Position.Y -= CurrentSpeed* Direction.Y;
            Position.X += CurrentSpeed * Direction.X;

            if (Position.Y < 0 && Direction.Y > 0)
            {
                Direction.Y = -Direction.Y;
            }

            if (Position.Y > maxHeight - Height && Direction.Y < 0)
            {
                Direction.Y = -Direction.Y;
            }
        }

        public void Collide(Boolean left)
        {
            if (left)
            {
                if (Direction.X < 0)
                {
                    Direction.X = -Direction.X;
                }
            }
            else
            {
                if (Direction.X > 0)
                {
                    Direction.X = -Direction.X;
                }
            }
            CurrentSpeed += 0.1f;
            CurrentSpeed = MathHelper.Clamp(CurrentSpeed, 0, MaxSpeed);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(BallTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
        }
    }
}
