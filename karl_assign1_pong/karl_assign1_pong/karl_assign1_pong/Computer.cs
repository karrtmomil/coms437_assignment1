using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace karl_assign1_pong
{
    class Computer
    {
        // Max movespeed, on a range form 0 to 1.0f
        public float maxSpeed = 0.25f;

        /// <summary>
        /// This aglorithm determines where the computer player should move the paddle.
        /// It predicts the next position of the ball, but intentionally does not detect when the ball bounces
        /// off of the walls of the level, additionally it doesn't adequately detect large amounts of spin
        /// </summary>
        /// <param name="ball">The ball in the game</param>
        /// <param name="paddle">the paddle to be moved</param>
        /// <returns>the control value for the paddle</returns>
        public float Move(Ball ball, Paddle paddle)
        {
            float ballY = ball.Position.Y + ball.Direction.Y * ball.CurrentSpeed;

            if (ball.Position.Y < paddle.Position.Y)
            {
                return maxSpeed;
            }
            else if (ball.Position.Y + ball.Height > paddle.Position.Y + paddle.Height)
            {
                return - maxSpeed;
            }

            return 0f;

        }
    }
}
