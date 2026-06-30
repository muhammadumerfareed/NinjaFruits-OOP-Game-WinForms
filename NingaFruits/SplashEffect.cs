
using System;
using System.Drawing;

namespace NingaFruits
{
    public class SplashEffect : GameObject
    {
        private int lifetime;
        private int maxLifetime;

        public int Lifetime    { get { return lifetime;    } set { lifetime    = value; } }
        public int MaxLifetime { get { return maxLifetime; } set { maxLifetime = value; } }

        public SplashEffect(int startX, int startY, int objWidth, int objHeight, Image objSprite)
            : base(startX, startY, objWidth, objHeight, objSprite)
        {
            lifetime    = 0;
            maxLifetime = 20;
        }

        public override void Update()
        {
            lifetime++;
            if (lifetime >= maxLifetime)
            {
                isActive = false;
            }
        }

        // Simple draw - no per-call object allocation
        public override void Draw(Graphics g)
        {
            if (!isActive || sprite == null) { return; }
            g.DrawImage(sprite, x, y, width, height);
        }

        public override void OnCollision(GameObject other)
        {
            // Splash effects do not interact with anything
        }
    }
}
