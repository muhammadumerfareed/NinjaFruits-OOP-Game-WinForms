
using System;
using System.Drawing;

namespace NingaFruits
{
    // Sword class - the projectile that the ninja throws upward
    public class Sword : GameObject
    {
        // ---- Private fields (Encapsulation) ----
        private int speed;

        // ---- Public Properties with explicit get/set ----
        public int SwordSpeed
        {
            get { return speed; }
            set { speed = value; }
        }

        // Constructor
        public Sword(int startX, int startY, int objWidth, int objHeight, Image objSprite)
            : base(startX, startY, objWidth, objHeight, objSprite)
        {
            speed = 12;
        }

        // Update - sword moves upward each frame
        public override void Update()
        {
            y = y - speed;

            // Deactivate if it goes off the top of the screen
            if (y + height < 0)
            {
                isActive = false;
            }
        }

        // Collision handler for sword
        public override void OnCollision(GameObject other)
        {
            // When sword hits something, it gets destroyed
            isActive = false;
        }
    }
}
