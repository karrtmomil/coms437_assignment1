using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace karl_assign1_pong
{
    class StrobePower
    {
        // Texture representing the Strobe PowerUp
        public Texture2D StrobeTexture;

        public SoundEffect StrobeSound;

        public bool Active;

        public int Timer;

        private const int TimerMaxUp = 11 * 1000;

        private const int TimerDown = 2 * 1000;

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

        private void Activate(Random rand)
        {
            Active = true;
            Timer += TimerMaxUp;
            Position = new Vector2((float)rand.NextDouble() * Spawn.Width + Spawn.Left, (float)rand.NextDouble() * Spawn.Height + Spawn.Top);
        }

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
