// ============================================================
// File: Form1.Designer.cs
// Description: UI controls setup - Game timer only (no panels)
// Start/GameOver screens are now in IntroForm and GameOverForm
// ============================================================
namespace NingaFruits
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // ---- UI Controls ----
        private System.Windows.Forms.Timer gameTimer;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            // ===================================================
            // Game Timer - 25ms interval (~40 FPS)
            // ===================================================
            gameTimer = new System.Windows.Forms.Timer(components);
            gameTimer.Interval = 25;
            gameTimer.Tick += new System.EventHandler(gameTimer_Tick);

            // ===================================================
            // MAIN FORM SETUP (game only - no panels)
            // ===================================================
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 550);
            this.Text = "Ninja Fruits - OOP Game";
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.MaximizeBox = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.BackColor = System.Drawing.Color.FromArgb(30, 30, 40);
            this.KeyPreview = true; // Form gets key events before any control
        }

        #endregion
    }
}
