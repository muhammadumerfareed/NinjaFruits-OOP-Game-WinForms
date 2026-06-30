
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace NingaFruits
{
    public class GameManager
    {
        // ---- Private fields ----
        private Player player;
        private List<GameObject> gameObjects;
        private List<GameObject> objectsToAdd;
        private int score;
        private int health;
        private int maxHealth;
        private int gameState;          // 0=Start  1=Playing  2=GameOver
        private int formWidth;
        private int formHeight;
        private int fruitSpawnTimer;
        private int fruitSpawnInterval;
        private Random random;
        private int swordCooldown;
        private int swordCooldownMax;
        private bool bombExploded;

        // ---- Sound ----
        private SoundManager soundManager;

        // ---- Sprites ----
        private string assetsPath;
        private Image swordSprite;
        private Image backgroundImage;
        private Image bombSprite;
        private Image explosionSprite;
        private string[] fruitNames;
        private Image[] fruitSprites;
        private Image[] fruitHalf1Sprites;
        private Image[] fruitHalf2Sprites;
        private Image[] splashSprites;

        private Font  scoreFont;
        private Font  healthFont;
        private Brush whiteBrush;
        private Brush blackShadowBrush;
        private Brush healthGreen;
        private Brush healthOrange;
        private Brush healthRed;
        private Brush barBgBrush;
        private Brush barBorderBrush;
        private Pen   barBorderPen;
        private Brush fallbackBgBrush;

 
        private int bombWarningTimer;
        private Pen[] warningPens;
        private Brush[] warningBrushes;
        private Font bombLabelFont;

        private string lastHealthText;
        private SizeF lastHealthTextSize;

        public Player GamePlayer   { get { return player; }    set { player    = value; } }
        public int    Score        { get { return score; }     set { score     = value; } }
        public int    Health       { get { return health; }    set { health    = value; } }
        public int    MaxHealth    { get { return maxHealth; } set { maxHealth = value; } }
        public int    GameState    { get { return gameState; } set { gameState = value; } }
        public int    FormWidth    { get { return formWidth; } set { formWidth  = value; } }
        public int    FormHeight   { get { return formHeight;}  set { formHeight = value; } }
        public bool   BombExploded { get { return bombExploded;} set { bombExploded = value; } }

        public GameManager(int screenWidth, int screenHeight, string resourcesPath)
        {
            formWidth  = screenWidth;
            formHeight = screenHeight;
            assetsPath = resourcesPath;
            random     = new Random();

            gameObjects   = new List<GameObject>();
            objectsToAdd  = new List<GameObject>();

            score              = 0;
            maxHealth          = 5;
            health             = maxHealth;
            gameState          = 0;
            fruitSpawnTimer    = 0;
            fruitSpawnInterval = 55;   // start: spawn every ~1.4 sec (less frantic)
            swordCooldown      = 0;
            swordCooldownMax   = 8;
            bombExploded       = false;
            bombWarningTimer   = 0;

            soundManager = new SoundManager(resourcesPath);

            LoadAssets();

         
            scoreFont       = new Font("Impact", 24, FontStyle.Bold);
            healthFont      = new Font("Impact", 14, FontStyle.Bold);
            whiteBrush      = new SolidBrush(Color.White);
            blackShadowBrush= new SolidBrush(Color.FromArgb(180, 0, 0, 0));
            healthGreen     = new SolidBrush(Color.FromArgb(220, 50, 205, 50));
            healthOrange    = new SolidBrush(Color.FromArgb(220, 255, 165, 0));
            healthRed       = new SolidBrush(Color.FromArgb(220, 255, 30, 30));
            barBgBrush      = new SolidBrush(Color.FromArgb(180, 80, 10, 10));
            barBorderBrush  = new SolidBrush(Color.FromArgb(140, 0, 0, 0));
            barBorderPen    = new Pen(Color.FromArgb(220, 255, 255, 255), 2);
            fallbackBgBrush = new SolidBrush(Color.FromArgb(40, 35, 50));

            // Pre-create arrays for warning pulses to prevent per-frame allocations
            warningPens = new Pen[20];
            warningBrushes = new Brush[20];
            bombLabelFont = new Font("Impact", 9, FontStyle.Bold);
            for (int i = 0; i < 20; i++)
            {
                int alpha;
                if (i < 10)
                {
                    alpha = 80 + i * 14;
                }
                else
                {
                    alpha = 220 - (i - 10) * 14;
                }
                warningPens[i] = new Pen(Color.FromArgb(alpha, 255, 30, 30), 3);
                warningBrushes[i] = new SolidBrush(Color.FromArgb(alpha, 255, 60, 60));
            }

            lastHealthText = "";
            lastHealthTextSize = SizeF.Empty;

        
            Image ninjaSprite  = LoadImage("ninja.png");
            int   playerWidth  = 100;
            int   playerHeight = 110;
            int   px           = (formWidth  / 2) - (playerWidth  / 2);
            int   py           = formHeight - playerHeight - 10;
            player = new Player(px, py, playerWidth, playerHeight, ninjaSprite, formWidth);
            player.Speed = 14;
        }

  
        private Image LoadImage(string fileName)
        {
            string path = Path.Combine(assetsPath, fileName);
            if (File.Exists(path))
            {
                return Image.FromFile(path);
            }
            return null;
        }

        private void LoadAssets()
        {
            backgroundImage = LoadImage("background.png");
            swordSprite     = LoadImage("sword.png");
            bombSprite      = LoadImage("bomb_small.png");
            explosionSprite = LoadImage("explosion_small.png");

            fruitNames = new string[6];
            fruitNames[0] = "apple";
            fruitNames[1] = "banana";
            fruitNames[2] = "orange";
            fruitNames[3] = "watermelon";
            fruitNames[4] = "coconut";
            fruitNames[5] = "pineapple";

            fruitSprites     = new Image[6];
            fruitHalf1Sprites= new Image[6];
            fruitHalf2Sprites= new Image[6];
            for (int i = 0; i < 6; i++)
            {
                fruitSprites[i]      = LoadImage(fruitNames[i] + "_small.png");
                fruitHalf1Sprites[i] = LoadImage(fruitNames[i] + "_half_1_small.png");
                fruitHalf2Sprites[i] = LoadImage(fruitNames[i] + "_half_2_small.png");
            }

            string[] splashColors = new string[4];
            splashColors[0] = "red";
            splashColors[1] = "orange";
            splashColors[2] = "yellow";
            splashColors[3] = "transparent";

            splashSprites = new Image[4];
            for (int i = 0; i < 4; i++)
            {
                splashSprites[i] = LoadImage("splash_" + splashColors[i] + "_small.png");
            }
        }

        private int GetSplashIndex(string name)
        {
            if (name == "apple"  || name == "watermelon") { return 0; }
            if (name == "orange" || name == "coconut")     { return 1; }
            if (name == "banana" || name == "pineapple")   { return 2; }
            return 3;
        }


        public void StartGame()
        {
            soundManager.StopAll();
            gameObjects.Clear();
            objectsToAdd.Clear();

            score               = 0;
            health              = maxHealth;
            gameState           = 1;
            bombExploded        = false;
            fruitSpawnTimer     = 0;
            fruitSpawnInterval  = 55;
            swordCooldown       = 0;
            bombWarningTimer    = 0;

            player.X       = (formWidth / 2) - (player.Width / 2);
            player.Y       = formHeight - player.Height - 10;
            player.IsActive= true;
        }

        
        public void SpawnFruit()
        {
            // 18% chance to spawn a bomb
            if (random.Next(0, 100) < 18)
            {
                SpawnBomb();
                return;
            }

            int fruitIndex = random.Next(0, 6);
            int fruitSize  = 55;
            int spawnX     = random.Next(30, formWidth - fruitSize - 30);
            int fallSpeed  = random.Next(3, 6);  

            LargeFruit f = new LargeFruit(spawnX, -fruitSize, fruitSize, fruitSize,
                fruitSprites[fruitIndex], fallSpeed, fruitNames[fruitIndex]);

            f.HalfSprite1  = fruitHalf1Sprites[fruitIndex];
            f.HalfSprite2  = fruitHalf2Sprites[fruitIndex];
            f.SplashSprite = splashSprites[GetSplashIndex(fruitNames[fruitIndex])];

            gameObjects.Add(f);
        }

        private void SpawnBomb()
        {
            int bombSize  = 55;
            int spawnX    = random.Next(30, formWidth - bombSize - 30);
            int fallSpeed = random.Next(3, 5);

            Bomb b = new Bomb(spawnX, -bombSize, bombSize, bombSize, bombSprite, fallSpeed);
            b.ExplosionSprite = explosionSprite;
            gameObjects.Add(b);
        }

        public void FireSword()
        {
            if (swordCooldown > 0) { return; }

            int sw = 40;
            int sh = 70;
            int sx = player.X + (player.Width  / 2) - (sw / 2);
            int sy = player.Y - sh;

            Sword s = new Sword(sx, sy, sw, sh, swordSprite);
            gameObjects.Add(s);
            swordCooldown = swordCooldownMax;
        }

        // ---- Main update loop ----
        public void UpdateAll()
        {
            if (gameState != 1) { return; }

            if (swordCooldown > 0) { swordCooldown--; }

            bombWarningTimer++;

            // Spawn timer
            fruitSpawnTimer++;
            if (fruitSpawnTimer >= fruitSpawnInterval)
            {
                SpawnFruit();
                fruitSpawnTimer = 0;
                // Slowly increase difficulty - floor at 28 frames (~0.7 sec)
                if (fruitSpawnInterval > 28)
                {
                    fruitSpawnInterval--;
                }
            }

            player.Update();

            // Polymorphism: Update() on every object regardless of type
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (gameObjects[i].IsActive)
                {
                    gameObjects[i].Update();
                }
            }

            CheckCollisions();
            CheckFruitsAtBottom();
            CleanUpObjects();

            // Add queued objects (SmallFruits, splashes) after cleanup
            for (int i = 0; i < objectsToAdd.Count; i++)
            {
                gameObjects.Add(objectsToAdd[i]);
            }
            objectsToAdd.Clear();

            // Game over check
            if (health <= 0 || bombExploded)
            {
                health    = 0;
                gameState = 2;
                // Game over sound is played by GameOverForm when it loads.
                // Bomb explosion sound was already played at collision moment.
            }
        }

        // ---- Collision detection ----
        private void CheckCollisions()
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                GameObject obj1 = gameObjects[i];
                if (!(obj1 is Sword) || !obj1.IsActive) { continue; }

                Sword sword = (Sword)obj1;

                for (int j = 0; j < gameObjects.Count; j++)
                {
                    GameObject obj2 = gameObjects[j];
                    if (!obj2.IsActive) { continue; }
                    if (!sword.IsActive) { break; } // sword was deactivated already

                    // Manual bounding box check: avoid Rectangle allocations and getter overhead in inner loop
                    bool intersects = (sword.X < obj2.X + obj2.Width) &&
                                     (sword.X + sword.Width > obj2.X) &&
                                     (sword.Y < obj2.Y + obj2.Height) &&
                                     (sword.Y + sword.Height > obj2.Y);

                    if (!intersects) { continue; }

                    // --- Sword hits BOMB ---
                    if (obj2 is Bomb)
                    {
                        Bomb hitBomb = (Bomb)obj2;
                        sword.OnCollision(hitBomb);
                        hitBomb.OnCollision(sword);

                        // Big explosion effect
                        if (hitBomb.ExplosionSprite != null)
                        {
                            int es  = 130;
                            int ex  = hitBomb.X + hitBomb.Width  / 2 - es / 2;
                            int ey  = hitBomb.Y + hitBomb.Height / 2 - es / 2;
                            SplashEffect boom = new SplashEffect(ex, ey, es, es, hitBomb.ExplosionSprite);
                            boom.MaxLifetime = 35;
                            objectsToAdd.Add(boom);
                        }

                        // Play bomb sound immediately at collision
                        soundManager.PlayBombBlast();
                        bombExploded = true;
                        continue;
                    }

                    // --- Sword hits FRUIT ---
                    if (!(obj2 is Fruit)) { continue; }

                    Fruit hitFruit = (Fruit)obj2;
                    sword.OnCollision(hitFruit);
                    hitFruit.OnCollision(sword);

                    // Slice sound - non-blocking seek+play
                    soundManager.PlaySlice();

                    // Splash effect
                    if (hitFruit.SplashSprite != null)
                    {
                        int ss  = 85;
                        int spx = hitFruit.X + hitFruit.Width  / 2 - ss / 2;
                        int spy = hitFruit.Y + hitFruit.Height / 2 - ss / 2;
                        objectsToAdd.Add(new SplashEffect(spx, spy, ss, ss, hitFruit.SplashSprite));
                    }

                    // If LargeFruit, spawn two cut halves
                    if (hitFruit is LargeFruit)
                    {
                        LargeFruit lf = (LargeFruit)hitFruit;
                        int hs = 42;
                        int hspeed = 3;

                        if (lf.HalfSprite1 != null)
                        {
                            SmallFruit left = new SmallFruit(
                                hitFruit.X - 15, hitFruit.Y,
                                hs, hs, lf.HalfSprite1, hspeed, hitFruit.FruitType, -1);
                            left.SplashSprite = hitFruit.SplashSprite;
                            left.HalfSprite1  = null;
                            left.HalfSprite2  = null;
                            objectsToAdd.Add(left);
                        }

                        if (lf.HalfSprite2 != null)
                        {
                            SmallFruit right = new SmallFruit(
                                hitFruit.X + hitFruit.Width - 5, hitFruit.Y,
                                hs, hs, lf.HalfSprite2, hspeed, hitFruit.FruitType, 1);
                            right.SplashSprite = hitFruit.SplashSprite;
                            right.HalfSprite1  = null;
                            right.HalfSprite2  = null;
                            objectsToAdd.Add(right);
                        }

                        score += 10;
                    }
                    else if (hitFruit is SmallFruit)
                    {
                        score += 5;
                    }

                    if (health < maxHealth) { health++; }
                }
            }
        }

        // ---- Fruit/bomb out-of-bounds ----
        private void CheckFruitsAtBottom()
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                GameObject obj = gameObjects[i];
                if (!obj.IsActive) { continue; }

                if (obj.Y > formHeight + 20)
                {
                    obj.IsActive = false;
                    if (obj is LargeFruit) { health--; }  // penalty only for uncut LargeFruit
                }

                if (obj is SmallFruit)
                {
                    if (obj.X < -120 || obj.X > formWidth + 120)
                    {
                        obj.IsActive = false;
                    }
                }
            }
        }

        // ---- Cleanup ----
        private void CleanUpObjects()
        {
            for (int i = gameObjects.Count - 1; i >= 0; i--)
            {
                if (!gameObjects[i].IsActive)
                {
                    gameObjects.RemoveAt(i);
                }
            }
        }

        // ---- Draw ----
        public void DrawAll(Graphics g)
        {
            // Background
            if (backgroundImage != null)
            {
                g.DrawImage(backgroundImage, 0, 0, formWidth, formHeight);
            }
            else
            {
                g.FillRectangle(fallbackBgBrush, 0, 0, formWidth, formHeight);
            }

            if (gameState == 1 || gameState == 2)
            {
                // Splash effects BEHIND everything else
                for (int i = 0; i < gameObjects.Count; i++)
                {
                    GameObject obj = gameObjects[i];
                    if (obj.IsActive && obj is SplashEffect)
                    {
                        obj.Draw(g);
                    }
                }

                // Fruits, bombs, swords
                for (int i = 0; i < gameObjects.Count; i++)
                {
                    GameObject obj = gameObjects[i];
                    if (!obj.IsActive || obj is SplashEffect) { continue; }

                    // Draw a glowing red border around bombs so they are obvious
                    if (obj is Bomb)
                    {
                        DrawBombWarning(g, obj);
                    }

                    obj.Draw(g);
                }

                // Player drawn last (always on top)
                player.Draw(g);

                DrawHUD(g);
            }
        }

        // Draw pulsing red warning ring around bombs
        private void DrawBombWarning(Graphics g, GameObject bomb)
        {
            int pulse = bombWarningTimer % 20;
            if (pulse < 0) { pulse = 0; }
            if (pulse >= 20) { pulse = 19; }

            int pad = 6;
            g.DrawEllipse(warningPens[pulse],
                bomb.X - pad,
                bomb.Y - pad,
                bomb.Width  + pad * 2,
                bomb.Height + pad * 2);

            // Small "BOMB" label above it
            g.DrawString("💣", bombLabelFont, warningBrushes[pulse], bomb.X + bomb.Width / 2 - 8, bomb.Y - 18);
        }

        // Draw HUD using PRE-CREATED resources - no new Font/Brush per frame
        private void DrawHUD(Graphics g)
        {
            // Score
            string scoreText = "Score: " + score.ToString();
            g.DrawString(scoreText, scoreFont, blackShadowBrush, 17f, 12f);
            g.DrawString(scoreText, scoreFont, whiteBrush,       15f, 10f);

            // Health bar
            int barX = formWidth - 260;
            int barY = 15;
            int barW = 240;
            int barH = 30;

            g.FillRectangle(barBorderBrush, barX - 3, barY - 3, barW + 6, barH + 6);
            g.DrawRectangle(barBorderPen,   barX - 1, barY - 1, barW + 2, barH + 2);
            g.FillRectangle(barBgBrush,     barX, barY, barW, barH);

            int fillW = (int)((double)health / maxHealth * barW);
            Brush fillBrush;
            if      (health > 3) { fillBrush = healthGreen;  }
            else if (health > 1) { fillBrush = healthOrange; }
            else                 { fillBrush = healthRed;     }

            if (fillW > 0)
            {
                g.FillRectangle(fillBrush, barX, barY, fillW, barH);
            }

            string healthText = "Health: " + health.ToString() + " / " + maxHealth.ToString();
            if (healthText != lastHealthText)
            {
                lastHealthText = healthText;
                lastHealthTextSize = g.MeasureString(healthText, healthFont);
            }
            SizeF ts = lastHealthTextSize;
            float tx = barX + (barW - ts.Width)  / 2;
            float ty = barY + (barH - ts.Height) / 2;
            g.DrawString(healthText, healthFont, blackShadowBrush, tx + 1, ty + 1);
            g.DrawString(healthText, healthFont, whiteBrush,       tx,     ty);
        }

        // Dynamically resize game boundaries and update player position
        public void ResizeGame(int newW, int newH)
        {
            formWidth = newW;
            formHeight = newH;

            if (player != null)
            {
                player.FormWidth = newW;
                player.Y = newH - player.Height - 10;

                // Ensure player is within bounds
                if (player.X + player.Width > newW)
                {
                    player.X = newW - player.Width;
                }
                if (player.X < 0)
                {
                    player.X = 0;
                }
            }
        }

        // Clean up GDI resources on disposal
        public void DisposeResources()
        {
            if (soundManager != null)
            {
                soundManager.CloseAll();
            }

            if (scoreFont != null) { scoreFont.Dispose(); }
            if (healthFont != null) { healthFont.Dispose(); }
            if (bombLabelFont != null) { bombLabelFont.Dispose(); }
            if (whiteBrush != null) { whiteBrush.Dispose(); }
            if (blackShadowBrush != null) { blackShadowBrush.Dispose(); }
            if (healthGreen != null) { healthGreen.Dispose(); }
            if (healthOrange != null) { healthOrange.Dispose(); }
            if (healthRed != null) { healthRed.Dispose(); }
            if (barBgBrush != null) { barBgBrush.Dispose(); }
            if (barBorderBrush != null) { barBorderBrush.Dispose(); }
            if (barBorderPen != null) { barBorderPen.Dispose(); }
            if (fallbackBgBrush != null) { fallbackBgBrush.Dispose(); }

            if (warningPens != null)
            {
                for (int i = 0; i < warningPens.Length; i++)
                {
                    if (warningPens[i] != null) { warningPens[i].Dispose(); }
                }
            }

            if (warningBrushes != null)
            {
                for (int i = 0; i < warningBrushes.Length; i++)
                {
                    if (warningBrushes[i] != null) { warningBrushes[i].Dispose(); }
                }
            }
        }
    }
}
