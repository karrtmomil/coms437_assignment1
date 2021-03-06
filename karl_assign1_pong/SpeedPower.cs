﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace karl_assign1_pong
{
    /// <summary>
    /// The class to defing the powerup that speeds the ball up and changes the direction.
    /// </summary>
    class SpeedPower
    {
        // Texture representing the Speed PowerUp
        public Texture2D SpeedTexture;

        public SoundEffect SpeedSound;

        public bool Active;

        public int Timer;

        private const int TimerMaxUp = 8 * 1000;

        private const int TimerDown = 3 * 1000;

        public Rectangle Spawn;

        public Vector2 Position;

        // Speed to to increase the ball by when collided with
        private const float boostSpeed = 1.5f;

        // Get the width of the PowerUP
        public int Width
        {
            get { return SpeedTexture.Width; }
        }

        // Get the height of the PowerUp
        public int Height
        {
            get { return SpeedTexture.Height; }
        }

        public void Initialize(Texture2D texture,SoundEffect sound, Rectangle spawn)
        {
            SpeedTexture = texture;
            SpeedSound = sound;
            Active = false;
            Timer = TimerDown;
            Spawn = spawn;
        }

        public void Reset()
        {
            Active = false;
            Timer = TimerDown;
        }

        public void Update(GameTime gameTime, Random rand)
        {
            Timer -= gameTime.ElapsedGameTime.Milliseconds;
            if (Timer <= 0)
            {
                if (Active)
                {
                    Active = false;
                    Timer += TimerDown;
                }
                else
                {
                    Activate(rand);
                }
            }
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (Active)
            {
                spriteBatch.Draw(SpeedTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }

        private void Activate(Random rand)
        {
            Active = true;
            Timer += TimerMaxUp;
            Position = new Vector2((float)rand.NextDouble() * Spawn.Width + Spawn.Left, (float)rand.NextDouble() * Spawn.Height + Spawn.Top);
        }

        /// <summary>
        /// Handles the collision with the ball.  it chooses a random angle in the same direction the ball was previously going
        /// and increase th eballs speed
        /// </summary>
        /// <param name="ball"></param>
        /// <param name="rand"></param>
        public void Collide(Ball ball, Random rand)
        {
            if (Active)
            {
                float direction = rand.Next(2) * 2 - 1; ;
                if (ball.Direction.X > 0)
                {
                    direction = 1.0f;
                }
                else
                {
                    direction = -1.0f;
                }
                Vector2 ballDirection = new Vector2(direction, (float)(rand.NextDouble() * 1.5 - 1));
                ball.Boost(ballDirection, boostSpeed);
                Active = false;
                Timer += TimerDown;
                SpeedSound.Play();
            }
        }
    }
}
