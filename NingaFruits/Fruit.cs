
using System;
using System.Drawing;

namespace NingaFruits
{
    // Abstract Fruit class
    
    public abstract class Fruit : GameObject
    {
     
        protected int speed;
        protected string fruitType;
        protected Image halfSprite1;
        protected Image halfSprite2;
        protected Image splashSprite;

        public int FruitSpeed
        {
            get { return speed; }
            set { speed = value; }
        }

        public string FruitType
        {
            get { return fruitType; }
            set { fruitType = value; }
        }

        public Image HalfSprite1
        {
            get { return halfSprite1; }
            set { halfSprite1 = value; }
        }

        public Image HalfSprite2
        {
            get { return halfSprite2; }
            set { halfSprite2 = value; }
        }

        public Image SplashSprite
        {
            get { return splashSprite; }
            set { splashSprite = value; }
        }

        // Constructor
        public Fruit(int startX, int startY, int objWidth, int objHeight, Image objSprite, int fallSpeed, string type)
            : base(startX, startY, objWidth, objHeight, objSprite)
        {
            speed = fallSpeed;
            fruitType = type;
            halfSprite1 = null;
            halfSprite2 = null;
            splashSprite = null;
        }

       
        public override abstract void Update();
    }
}
