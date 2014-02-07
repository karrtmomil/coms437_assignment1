using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace karl_assign1_pong
{
    class Ball
    {
        // Texture representing the ball
        public Texture2D BallTexture;

        // Postion
        public Vector2 Position;

        public bool Visible;

        private const float StartSpeed = 3.0f;

        public float CurrentSpeed;

        private const float MaxSpeed = 15.0f;

        public Vector2 Direction;

        public float Spin;

        private const float SpinFactor = 0.01f;

        private int Delay;

        private bool Strobe;

        private int StrobeTimer;

        private const int StrobePeriod = 700 * (int)StartSpeed;

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

        public void Initialize(Texture2D texture, Vector2 position, Vector2 direction, int delay)
        {
            BallTexture = texture;

            Position = new Vector2(position.X - this.Width / 2, position.Y - this.Height / 2);

            CurrentSpeed = StartSpeed;

            Direction = direction;

            Delay = delay;

            Spin = 0;

            Visible = true;
        }

        public void Reset(Vector2 position, Vector2 direction, int delay)
        {
            Position = new Vector2(position.X - this.Width / 2, position.Y - this.Height / 2);

            CurrentSpeed = StartSpeed;

            Direction = direction;

            Delay = delay;

            Spin = 0;

            Strobe = false;

            Visible = true;
        }

        public void Boost(Vector2 direction, float speed)
        {
            Direction = direction;
            CurrentSpeed += speed;
            CurrentSpeed = MathHelper.Clamp(CurrentSpeed, 0, MaxSpeed);
        }

        public void StrobeStart()
        {
            Strobe = true;
            StrobeTimer = StrobePeriod / (int)CurrentSpeed;
            Visible = false;
        }

        public void Update(GameTime gameTime, float maxHeight, SoundEffect wallSound)
        {
            if (Delay < 0)
            {
                float timestep = gameTime.ElapsedGameTime.Milliseconds / 16;
                Direction.Y -= Spin * SpinFactor;
                Direction.Normalize();
                Position.Y -= CurrentSpeed * timestep * Direction.Y;
                Position.X += CurrentSpeed * timestep * Direction.X;

                if (Position.Y < 0 && Direction.Y > 0)
                {
                    Direction.Y = -Direction.Y * (float)Math.Pow(0.8f, timestep);
                    Spin = Spin / (2 * timestep);
                    wallSound.Play();
                }

                if (Position.Y > maxHeight - Height && Direction.Y < 0)
                {
                    Direction.Y = -Direction.Y * (float)Math.Pow(0.8f, timestep) ;
                    Spin = Spin / (2 * timestep);
                    wallSound.Play();
                }
                if (Strobe)
                {
                    StrobeTimer -= gameTime.ElapsedGameTime.Milliseconds;
                    if (StrobeTimer <= 0)
                    {
                        StrobeTimer += StrobePeriod / (int) CurrentSpeed;
                        Visible = !Visible;
                    }
                }
            }
            else
            {
                Delay -= gameTime.ElapsedGameTime.Milliseconds;
            }
        }

        public void Collide(Boolean left, float spin)
        {
            Spin = spin;

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
            CurrentSpeed += 0.20f;
            CurrentSpeed = MathHelper.Clamp(CurrentSpeed, 0, MaxSpeed);
            Strobe = false;
            Visible = true;

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Visible)
            {
                spriteBatch.Draw(BallTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }
    }
}
