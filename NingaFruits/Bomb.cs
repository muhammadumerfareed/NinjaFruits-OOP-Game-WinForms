// ============================================================
// File: Bomb.cs
// Description: Bomb object that falls down - instant game over if hit
// Demonstrates: Inheritance from GameObject
// ============================================================
using System;
using System.Drawing;

namespace NingaFruits
{
    // Bomb - a dangerous object that causes instant game over when hit by sword
    public class Bomb : GameObject
    {
        // ---- Private fields (Encapsulation) ----
        private int speed;
        private Image explosionSprite;

        // ---- Public Properties with explicit get/set ----
        public int BombSpeed
        {
            get { return speed; }
            set { speed = value; }
        }

        public Image ExplosionSprite
        {
            get { return explosionSprite; }
            set { explosionSprite = value; }
        }

        // Constructor
        public Bomb(int startX, int startY, int objWidth, int objHeight, Image objSprite, int fallSpeed)
            : base(startX, startY, objWidth, objHeight, objSprite)
        {
            speed = fallSpeed;
            explosionSprite = null;
        }

        // Bomb falls straight down
        public override void Update()
        {
            y = y + speed;
        }

        // When bomb is hit by sword, it deactivates
        public override void OnCollision(GameObject other)
        {
            isActive = false;
        }
    }
}
