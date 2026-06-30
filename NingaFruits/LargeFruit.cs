
using System;
using System.Drawing;

namespace NingaFruits
{
    public class LargeFruit : Fruit
    {
        // Constructor
        public LargeFruit(int startX, int startY, int objWidth, int objHeight, Image objSprite, int fallSpeed, string type)
            : base(startX, startY, objWidth, objHeight, objSprite, fallSpeed, type)
        {
            // LargeFruit specific setup is done in constructor
        }

        // LargeFruit falls straight down
        public override void Update()
        {
            y = y + speed;
        }

        // When a LargeFruit is hit, it gets destroyed
        public override void OnCollision(GameObject other)
        {
            isActive = false;
        }
    }
}
