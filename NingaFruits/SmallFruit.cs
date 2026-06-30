
using System;
using System.Drawing;

namespace NingaFruits
{
    // SmallFruit - a cut piece that flies diagonally after a LargeFruit is split
    public class SmallFruit : Fruit
    {
        // ---- Private fields (Encapsulation) ----
        private int horizontalSpeed;

        // ---- Public Properties with explicit get/set ----
        public int HorizontalSpeed
        {
            get { return horizontalSpeed; }
            set { horizontalSpeed = value; }
        }

        // Constructor - horizontalDir: -1 for left, +1 for right
        public SmallFruit(int startX, int startY, int objWidth, int objHeight, Image objSprite, int fallSpeed, string type, int horizontalDir)
            : base(startX, startY, objWidth, objHeight, objSprite, fallSpeed, type)
        {
            horizontalSpeed = horizontalDir * 3;
        }

        // SmallFruit moves diagonally (down-left or down-right)
        public override void Update()
        {
            x = x + horizontalSpeed;
            y = y + speed;
        }

        // When a SmallFruit is hit, it gets destroyed
        public override void OnCollision(GameObject other)
        {
            isActive = false;
        }
    }
}
