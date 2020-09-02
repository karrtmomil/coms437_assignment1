using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace karl_assign1_pong
{
    /// <summary>
    /// Class to define how a computer should move
    /// </summary>
    class Computer
    {
        // Max movespeed, on a range form 0 to 1.0f
        public float maxSpeed = 0.25f;

        // Max spin on range form 0 to 1.0f
        public float maxSpin = 0.8f;

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

            if (ball.Position.Y + ball.Height / 2   < paddle.Position.Y + paddle.Height / 2 - ball.Height / 2)
            {
                return maxSpin;
            }
            else if (ball.Position.Y + ball.Height / 2 > paddle.Position.Y + paddle.Height / 2 + ball.Height / 2)
            {
                return - maxSpin;
            }

            return 0f;

        }
    }
}
