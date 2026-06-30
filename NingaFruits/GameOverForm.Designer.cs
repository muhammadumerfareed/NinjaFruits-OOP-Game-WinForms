// ============================================================
// File: GameOverForm.Designer.cs
// Description: Designer file for GameOverForm - clean InitializeComponent
// No conditional logic here (VS Designer requirement)
// ============================================================
namespace NingaFruits
{
    partial class GameOverForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        // ---- UI Controls ----
        private System.Windows.Forms.Label lblGameOver;
        private System.Windows.Forms.Label lblFinalScore;
        private System.Windows.Forms.Button btnRestart;
        private System.Windows.Forms.Button btnExit;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lblGameOver = new Label();
            lblFinalScore = new Label();
            btnRestart = new Button();
            btnExit = new Button();
            SuspendLayout();
            // 
            // lblGameOver
            // 
            lblGameOver.BackColor = Color.Transparent;
            lblGameOver.Font = new Font("Impact", 50F, FontStyle.Bold);
            lblGameOver.ForeColor = Color.FromArgb(255, 50, 50);
            lblGameOver.Location = new Point(0, -10);
            lblGameOver.Name = "lblGameOver";
            lblGameOver.Size = new Size(900, 180);
            lblGameOver.TabIndex = 0;
            lblGameOver.Text = "GAME OVER";
            lblGameOver.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblFinalScore
            // 
            lblFinalScore.BackColor = Color.Transparent;
            lblFinalScore.Font = new Font("Arial", 28F, FontStyle.Bold);
            lblFinalScore.ForeColor = Color.FromArgb(255, 220, 0);
            lblFinalScore.Location = new Point(0, 170);
            lblFinalScore.Name = "lblFinalScore";
            lblFinalScore.Size = new Size(900, 60);
            lblFinalScore.TabIndex = 1;
            lblFinalScore.Text = "Final Score: 0";
            lblFinalScore.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnRestart
            // 
            btnRestart.BackColor = Color.FromArgb(0, 150, 50);
            btnRestart.Cursor = Cursors.Hand;
            btnRestart.FlatAppearance.BorderColor = Color.FromArgb(0, 200, 80);
            btnRestart.FlatAppearance.BorderSize = 3;
            btnRestart.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 190, 70);
            btnRestart.FlatStyle = FlatStyle.Flat;
            btnRestart.Font = new Font("Impact", 22F, FontStyle.Bold);
            btnRestart.ForeColor = Color.White;
            btnRestart.Location = new Point(300, 290);
            btnRestart.Name = "btnRestart";
            btnRestart.Size = new Size(300, 65);
            btnRestart.TabIndex = 2;
            btnRestart.TabStop = false;
            btnRestart.Text = "🔄  PLAY AGAIN";
            btnRestart.UseVisualStyleBackColor = false;
            btnRestart.Click += btnRestart_Click;
            // 
            // btnExit
            // 
            btnExit.BackColor = Color.FromArgb(150, 30, 30);
            btnExit.Cursor = Cursors.Hand;
            btnExit.FlatAppearance.BorderColor = Color.FromArgb(200, 60, 60);
            btnExit.FlatAppearance.BorderSize = 3;
            btnExit.FlatAppearance.MouseOverBackColor = Color.FromArgb(200, 50, 30);
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.Font = new Font("Impact", 22F, FontStyle.Bold);
            btnExit.ForeColor = Color.White;
            btnExit.Location = new Point(300, 380);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(300, 65);
            btnExit.TabIndex = 3;
            btnExit.TabStop = false;
            btnExit.Text = "❌  EXIT";
            btnExit.UseVisualStyleBackColor = false;
            btnExit.Click += btnExit_Click;
            // 
            // GameOverForm
            // 
            BackColor = Color.FromArgb(30, 30, 40);
            ClientSize = new Size(900, 550);
            Controls.Add(lblGameOver);
            Controls.Add(lblFinalScore);
            Controls.Add(btnRestart);
            Controls.Add(btnExit);
            DoubleBuffered = true;
            Name = "GameOverForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Ninja Fruits - Game Over";
            ResumeLayout(false);
        }

        #endregion
    }
}
