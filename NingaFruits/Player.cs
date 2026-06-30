
using System;
using System.Drawing;

namespace NingaFruits
{
    // Player class - the ninja character controlled by the player
    public class Player : GameObject
    {
        // ---- Private fields (Encapsulation) ----
        private int speed;
        private int formWidth;

        // ---- Public Properties with explicit get/set ----
        public int Speed
        {
            get { return speed; }
            set { speed = value; }
        }

        public int FormWidth
        {
            get { return formWidth; }
            set { formWidth = value; }
        }

        // Constructor
        public Player(int startX, int startY, int objWidth, int objHeight, Image objSprite, int screenWidth)
            : base(startX, startY, objWidth, objHeight, objSprite)
        {
            speed = 8;
            formWidth = screenWidth;
        }

        // Move the player left
        public void MoveLeft()
        {
            if (x - speed >= 0)
            {
                x = x - speed;
            }
        }

        // Move the player right
        public void MoveRight()
        {
            if (x + width + speed <= formWidth)
            {
                x = x + speed;
            }
        }

        // Update method - player updates are handled by input, so this is minimal
        public override void Update()
        {
            // Player movement is handled by keyboard input in Form1
            // This keeps the player within bounds
            if (x < 0)
            {
                x = 0;
            }
            if (x + width > formWidth)
            {
                x = formWidth - width;
            }
        }

        // Collision handler for player
        public override void OnCollision(GameObject other)
        {
            // Player collision logic can be extended later
        }
    }
}
