using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace karl_assign1_pong
{
    class StrobePower
    {
        // Texture representing the Strobe PowerUp
        public Texture2D StrobeTexture;

        public bool Active;

        public int Timer;

        private const int TimerMax = 2 * 1000;

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

        public void Initialize(Texture2D texture, Rectangle spawn)
        {
            StrobeTexture = texture;
            Active = false;
            Timer = TimerMax;
            Spawn = spawn;
        }

        public void Update(GameTime gameTime, Random rand)
        {
            Timer -= gameTime.ElapsedGameTime.Milliseconds;
            if (Timer <= 0)
            {
                if (Active)
                {
                    Active = false;
                    Timer += TimerMax;
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
            Timer += TimerMax;
            Position = new Vector2((float)rand.NextDouble() * Spawn.Width + Spawn.Left, (float)rand.NextDouble() * Spawn.Height + Spawn.Top);
        }

        public void Collide(Ball ball)
        {

        }
    }
}
