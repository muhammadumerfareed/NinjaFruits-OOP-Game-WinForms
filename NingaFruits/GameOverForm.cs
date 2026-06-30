// ============================================================
// File: GameOverForm.cs
// Description: Game Over screen - shows score, restart/exit buttons
// Plays the game over sound when this form loads.
// Demonstrates: Windows Forms UI, Encapsulation, Constructor params
// ============================================================
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace NingaFruits
{
    public partial class GameOverForm : Form
    {
        // ---- MCI sound API (same as SoundManager) ----
        [DllImport("winmm.dll", CharSet = CharSet.Auto)]
        private static extern int mciSendString(string command, string returnString, int returnLength, IntPtr callback);

        // ---- Private fields (Encapsulation) ----
        private bool restartClicked;
        private int displayScore;
        private bool displayBombExploded;
        private string gameOverResourcesPath;
        private bool gameOverSoundLoaded;

        // ---- Public Properties with explicit get/set ----
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool RestartClicked
        {
            get { return restartClicked; }
            set { restartClicked = value; }
        }

        // Parameterless constructor - required by VS Designer
        public GameOverForm()
        {
            restartClicked = false;
            displayScore = 0;
            displayBombExploded = false;
            gameOverResourcesPath = "";
            gameOverSoundLoaded = false;

            InitializeComponent();
        }

        // Constructor - receives score, bomb status, and resources path from game form
        public GameOverForm(int score, bool bombExploded, string resourcesPath)
        {
            restartClicked = false;
            displayScore = score;
            displayBombExploded = bombExploded;
            gameOverResourcesPath = resourcesPath;
            gameOverSoundLoaded = false;

            InitializeComponent();

            // Set label text AFTER InitializeComponent (not inside it)
            // so the VS Designer can parse InitializeComponent cleanly
            if (displayBombExploded)
            {
                lblGameOver.Text = "\U0001f4a3 BOMB EXPLODED! \U0001f4a5";
                lblGameOver.ForeColor = Color.FromArgb(255, 150, 0);
            }
            else
            {
                lblGameOver.Text = "\U0001f480 GAME OVER \U0001f480";
                lblGameOver.ForeColor = Color.FromArgb(255, 50, 50);
            }

            lblFinalScore.Text = "\U0001f3c6 Final Score: " + displayScore.ToString();

            // Play the game over sound when this form loads
            LoadAndPlayGameOverSound();
        }

        // Load and play the game over MP3 using MCI
        private void LoadAndPlayGameOverSound()
        {
            if (gameOverResourcesPath == null || gameOverResourcesPath.Length == 0)
            {
                return;
            }

            string path = Path.Combine(gameOverResourcesPath, "gameover.mp3");
            if (!File.Exists(path))
            {
                return;
            }

            // Close any leftover handle from a previous session
            mciSendString("close gameOverFormAlias", null, 0, IntPtr.Zero);

            int err = mciSendString("open \"" + path + "\" type mpegvideo alias gameOverFormAlias", null, 0, IntPtr.Zero);
            if (err == 0)
            {
                gameOverSoundLoaded = true;
                mciSendString("play gameOverFormAlias from 0", null, 0, IntPtr.Zero);
            }
        }

        // Restart Button click handler
        private void btnRestart_Click(object sender, EventArgs e)
        {
            restartClicked = true;
            this.Close();
        }

        // Exit Button click handler
        private void btnExit_Click(object sender, EventArgs e)
        {
            restartClicked = false;
            this.Close();
        }

        // Center controls dynamically on resize
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (lblGameOver == null || lblFinalScore == null || btnRestart == null || btnExit == null)
            {
                return;
            }

            int w = this.ClientSize.Width;
            int h = this.ClientSize.Height;

            lblGameOver.Size = new Size(w, 80);
            lblGameOver.Location = new Point(0, (int)(h * 0.12));

            lblFinalScore.Size = new Size(w, 60);
            lblFinalScore.Location = new Point(0, lblGameOver.Bottom + 15);

            btnRestart.Location = new Point((w - btnRestart.Width) / 2, lblFinalScore.Bottom + 35);

            btnExit.Location = new Point((w - btnExit.Width) / 2, btnRestart.Bottom + 20);
        }

        // Clean up MCI sound handle when form closes
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (gameOverSoundLoaded)
            {
                mciSendString("stop gameOverFormAlias", null, 0, IntPtr.Zero);
                mciSendString("close gameOverFormAlias", null, 0, IntPtr.Zero);
            }
        }
    }
}
