
using System;
using System.Drawing;

namespace NingaFruits
{
   
    public abstract class GameObject : ICollidable
    {
       
        protected int x;
        protected int y;
        protected int width;
        protected int height;
        protected Image sprite;
        protected bool isActive;

        
        public int X
        {
            get { return x; }
            set { x = value; }
        }

        public int Y
        {
            get { return y; }
            set { y = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public Image Sprite
        {
            get { return sprite; }
            set { sprite = value; }
        }

        public bool IsActive
        {
            get { return isActive; }
            set { isActive = value; }
        }

        // Returns a Rectangle for collision detection
        public Rectangle Bounds
        {
            get { return new Rectangle(x, y, width, height); }
        }

        // Constructor
        public GameObject(int startX, int startY, int objWidth, int objHeight, Image objSprite)
        {
            x = startX;
            y = startY;
            width = objWidth;
            height = objHeight;
            sprite = objSprite;
            isActive = true;
        }

        // Abstract method - each child class must define how it updates
        public abstract void Update();

        // Draw the object on screen
        public virtual void Draw(Graphics g)
        {
            if (isActive && sprite != null)
            {
                g.DrawImage(sprite, x, y, width, height);
            }
        }

        // ICollidable implementation - default does nothing, children override
        public virtual void OnCollision(GameObject other)
        {
            // Default collision behavior - children will override this
        }
    }
}
