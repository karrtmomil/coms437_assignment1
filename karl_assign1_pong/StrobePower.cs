using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace karl_assign1_pong
{
    /// <summary>
    /// Class to handle the Strobe PowerUP
    /// </summary>
    class StrobePower
    {
        // Texture representing the Strobe PowerUp
        public Texture2D StrobeTexture;

        public SoundEffect StrobeSound;

        // Whether or not the powerUp is active
        public bool Active;

        // Timer for when to activate / deactivate the powerUp
        public int Timer;

        // Time constant for how long the powerup will remain active
        private const int TimerMaxUp = 11 * 1000;

        // Time constant for the time to add when the powerUp deactivates
        private const int TimerDown = 2 * 1000;

        // Bounding box for where the powerup can spawn
        public Rectangle Spawn;

        public Vector2 Position;

        // Get the width of the PowerUP
        public int Width
        {
            get { return StrobeTexture.Width; }
        }

        // Get the height of the PowerUp
        public int Height
        {
            get { return StrobeTexture.Height; }
        }

        public void Initialize(Texture2D texture, SoundEffect sound, Rectangle spawn)
        {
            StrobeTexture = texture;
            StrobeSound = sound;
            Active = false;
            Timer = TimerDown;
            Spawn = spawn;
        }

        public void Reset()
        {
            Active = false;
            Timer = TimerDown;
        }

        /// <summary>
        /// Updates the powerUp, essentional counts down the timer and then either activates or deactivates
        /// the powerUp based on its previous state
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="rand"></param>
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
                spriteBatch.Draw(StrobeTexture, Position, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }
        }

        /// <summary>
        /// Turns the powerUp active, resets the timer, and sets a postion within the spawn area
        /// </summary>
        /// <param name="rand"></param>
        private void Activate(Random rand)
        {
            Active = true;
            Timer += TimerMaxUp;
            Position = new Vector2((float)rand.NextDouble() * Spawn.Width + Spawn.Left, (float)rand.NextDouble() * Spawn.Height + Spawn.Top);
        }

        /// <summary>
        /// Handles colitions with the ball, causes the ball to start strobing
        /// </summary>
        /// <param name="ball">the game ball</param>
        public void Collide(Ball ball)
        {
            if (Active)
            {
                ball.StrobeStart();
                Active = false;
                Timer += TimerDown;
                StrobeSound.Play();
            }
        }
    }
}
