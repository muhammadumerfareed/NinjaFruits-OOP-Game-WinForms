// ============================================================
// File: Form1.cs
// Description: Game form - handles input, game loop, rendering
// Demonstrates: Windows Forms UI, Timer-based game loop, Paint events
// This form ONLY contains the gameplay. IntroForm and GameOverForm
// handle the start and end screens respectively.
// ============================================================
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.ComponentModel;
using System.Windows.Forms;

namespace NingaFruits
{
    public partial class Form1 : Form
    {
        // ---- Private fields ----
        private GameManager gameManager;
        private bool moveLeft;
        private bool moveRight;
        private bool firePressed;
        private string resourcesPath;
        private int finalScore;
        private bool wasBombExploded;
        private bool gameEndedNaturally;
        private int gameOverDelay;       // countdown before closing form (lets sounds play)

        // ---- Public Properties with explicit get/set ----
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int FinalScore
        {
            get { return finalScore; }
            set { finalScore = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool WasBombExploded
        {
            get { return wasBombExploded; }
            set { wasBombExploded = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool GameEndedNaturally
        {
            get { return gameEndedNaturally; }
            set { gameEndedNaturally = value; }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string ResourcesPath
        {
            get { return resourcesPath; }
        }

        // Constructor
        public Form1()
        {
            InitializeComponent();

            // Set form properties for smooth rendering
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.UserPaint, true);

            // Initialize input flags
            moveLeft = false;
            moveRight = false;
            firePressed = false;
            finalScore = 0;
            wasBombExploded = false;
            gameEndedNaturally = false;
            gameOverDelay = -1;  // -1 means game is still active

            // Set the path to the Resources folder
            resourcesPath = Path.Combine(Application.StartupPath, "Resources");

            // If Resources folder doesn't exist at startup path, try the project directory
            if (!Directory.Exists(resourcesPath))
            {
                string projectDir = Path.GetDirectoryName(Application.StartupPath);
                if (projectDir != null)
                {
                    resourcesPath = Path.Combine(projectDir, "Resources");
                }
            }
            if (!Directory.Exists(resourcesPath))
            {
                // Fallback: try the project source directory
                resourcesPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Resources");
                resourcesPath = Path.GetFullPath(resourcesPath);
            }

            // Create the GameManager
            gameManager = new GameManager(this.ClientSize.Width, this.ClientSize.Height, resourcesPath);

            // Start the game immediately (IntroForm already handled the start screen)
            gameManager.StartGame();
            gameTimer.Enabled = true;
            this.Focus();
        }

        // ============================================================
        // CRITICAL FIX: Override ProcessCmdKey to intercept Space and
        // Arrow keys BEFORE they reach any button or control.
        // ============================================================
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // When the game is playing, intercept game keys
            if (gameManager != null && gameManager.GameState == 1)
            {
                if (keyData == Keys.Space)
                {
                    firePressed = true;
                    return true; // Handled - don't pass to controls
                }
                if (keyData == Keys.Left || keyData == Keys.A)
                {
                    moveLeft = true;
                    return true;
                }
                if (keyData == Keys.Right || keyData == Keys.D)
                {
                    moveRight = true;
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        // Game Timer Tick - the main game loop (runs every 25ms)
        private void gameTimer_Tick(object sender, EventArgs e)
        {
            // Handle input - move player
            if (moveLeft)
            {
                gameManager.GamePlayer.MoveLeft();
            }
            if (moveRight)
            {
                gameManager.GamePlayer.MoveRight();
            }
            if (firePressed)
            {
                gameManager.FireSword();
            }

            // If game over delay is running, count down then close
            if (gameOverDelay > 0)
            {
                gameOverDelay--;
                if (gameOverDelay <= 0)
                {
                    gameTimer.Enabled = false;
                    gameEndedNaturally = true;
                    this.Close();
                    return;
                }
                // Still draw so player sees explosion effect
                this.Invalidate();
                return;
            }

            // Update all game objects
            gameManager.UpdateAll();

            // Check for game over - start a short delay for bomb explosion effect
            if (gameManager.GameState == 2)
            {
                finalScore = gameManager.Score;
                wasBombExploded = gameManager.BombExploded;
                // Bomb: short delay to see explosion + hear blast sound
                // Health death: almost instant transition
                if (wasBombExploded)
                {
                    gameOverDelay = 25;  // ~625ms for explosion
                }
                else
                {
                    gameOverDelay = 1;   // immediate
                }
                // Stop accepting input
                moveLeft = false;
                moveRight = false;
                firePressed = false;
                // Draw the final frame (with explosion)
                this.Invalidate();
                return;
            }

            // Refresh the screen (triggers Paint event)
            this.Invalidate();
        }

        // Paint event - draw all game objects
        // Use NearestNeighbor (fast) - bicubic is expensive and unnecessary for sprites
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode    = System.Drawing.Drawing2D.SmoothingMode.None;
            g.InterpolationMode= System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.PixelOffsetMode  = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;

            // Draw the game through the GameManager
            gameManager.DrawAll(g);
        }

        // Clean up MCI sound handles when form closes
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            gameTimer.Enabled = false;
            if (gameManager != null)
            {
                gameManager.DisposeResources();
            }
        }

        // Handle resizing of the form
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (gameManager != null)
            {
                gameManager.ResizeGame(this.ClientSize.Width, this.ClientSize.Height);
            }

            this.Invalidate();
        }

        // KeyDown event - set input flags
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A)
            {
                moveLeft = true;
            }
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D)
            {
                moveRight = true;
            }
            if (e.KeyCode == Keys.Space)
            {
                firePressed = true;
            }

            // Prevent key events from reaching controls (buttons)
            e.Handled = true;
            e.SuppressKeyPress = true;
        }

        // KeyUp event - clear input flags
        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);

            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.A)
            {
                moveLeft = false;
            }
            if (e.KeyCode == Keys.Right || e.KeyCode == Keys.D)
            {
                moveRight = false;
            }
            if (e.KeyCode == Keys.Space)
            {
                firePressed = false;
            }

            e.Handled = true;
            e.SuppressKeyPress = true;
        }
    }
}
