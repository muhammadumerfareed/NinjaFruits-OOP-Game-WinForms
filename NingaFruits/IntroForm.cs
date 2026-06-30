// ============================================================
// File: IntroForm.cs
// Description: Startup intro screen - title, instructions, start button
// Demonstrates: Windows Forms UI, Encapsulation
// ============================================================
using System;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

namespace NingaFruits
{
    public class IntroForm : Form
    {
        // ---- Private fields (Encapsulation) ----
        private bool startClicked;
        private Label lblTitle;
        private Label lblSubtitle;
        private Label lblInstructions;
        private Button btnStart;
        private System.ComponentModel.IContainer components;

        // ---- Public Properties with explicit get/set ----
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool StartClicked
        {
            get { return startClicked; }
            set { startClicked = value; }
        }

        // Constructor
        public IntroForm()
        {
            startClicked = false;
            components = new System.ComponentModel.Container();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            ComponentResourceManager resources = new ComponentResourceManager(typeof(IntroForm));
            lblTitle = new Label();
            lblSubtitle = new Label();
            lblInstructions = new Label();
            btnStart = new Button();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.BackColor = Color.Transparent;
            lblTitle.Font = new Font("Impact", 44F, FontStyle.Bold);
            lblTitle.ForeColor = Color.Red;
            lblTitle.Location = new Point(73, 48);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(789, 127);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "🗡️ NINJA FRUITS 🗡️";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSubtitle
            // 
            lblSubtitle.BackColor = Color.Transparent;
            lblSubtitle.Font = new Font("Arial", 18F, FontStyle.Italic);
            lblSubtitle.ForeColor = Color.FromArgb(255, 220, 150);
            lblSubtitle.Location = new Point(12, 184);
            lblSubtitle.Name = "lblSubtitle";
            lblSubtitle.Size = new Size(900, 40);
            lblSubtitle.TabIndex = 1;
            lblSubtitle.Text = "Slice them all!";
            lblSubtitle.TextAlign = ContentAlignment.MiddleCenter;
            lblSubtitle.Click += lblSubtitle_Click;
            // 
            // lblInstructions
            // 
            lblInstructions.BackColor = Color.Transparent;
            lblInstructions.Font = new Font("Segoe UI", 13F);
            lblInstructions.ForeColor = Color.FromArgb(230, 230, 230);
            lblInstructions.Location = new Point(202, 239);
            lblInstructions.Name = "lblInstructions";
            lblInstructions.Size = new Size(534, 200);
            lblInstructions.TabIndex = 2;
            lblInstructions.Text = "   Arrow Keys / A, D  to  Move Ninja\r\n\r\n🗡️  Spacebar  ,  Throw Sword\r\n\r\n  Cut fruits to score points and gain health  Don't let fruits fall past you!";
            lblInstructions.TextAlign = ContentAlignment.MiddleCenter;
            lblInstructions.Click += lblInstructions_Click;
            // 
            // btnStart
            // 
            btnStart.BackColor = Color.FromArgb(200, 40, 40);
            btnStart.Cursor = Cursors.Hand;
            btnStart.FlatAppearance.BorderColor = Color.FromArgb(255, 120, 50);
            btnStart.FlatAppearance.BorderSize = 3;
            btnStart.FlatAppearance.MouseOverBackColor = Color.FromArgb(240, 70, 30);
            btnStart.FlatStyle = FlatStyle.Flat;
            btnStart.Font = new Font("Impact", 24F, FontStyle.Bold);
            btnStart.ForeColor = Color.White;
            btnStart.Location = new Point(289, 454);
            btnStart.Name = "btnStart";
            btnStart.Size = new Size(320, 75);
            btnStart.TabIndex = 3;
            btnStart.TabStop = false;
            btnStart.Text = "⚔️  START GAME  ⚔️";
            btnStart.UseVisualStyleBackColor = false;
            btnStart.Click += btnStart_Click;
            // 
            // IntroForm
            // 
            BackColor = Color.FromArgb(30, 30, 40);
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            ClientSize = new Size(900, 550);
            Controls.Add(lblTitle);
            Controls.Add(lblSubtitle);
            Controls.Add(lblInstructions);
            Controls.Add(btnStart);
            DoubleBuffered = true;
            Name = "IntroForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Ninja Fruits - OOP Game";
            ResumeLayout(false);
        }

        // Start Button click handler
        private void btnStart_Click(object sender, EventArgs e)
        {
            startClicked = true;
            this.Close();
        }

        // Center controls dynamically on resize
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (lblTitle == null || lblSubtitle == null || lblInstructions == null || btnStart == null)
            {
                return;
            }

            int w = this.ClientSize.Width;
            int h = this.ClientSize.Height;

            lblTitle.Size = new Size(w, 80);
            lblTitle.Location = new Point(0, (int)(h * 0.08));

            lblSubtitle.Size = new Size(w, 40);
            lblSubtitle.Location = new Point(0, lblTitle.Bottom + 5);

            lblInstructions.Location = new Point((w - lblInstructions.Width) / 2, lblSubtitle.Bottom + 20);

            btnStart.Location = new Point((w - btnStart.Width) / 2, lblInstructions.Bottom + 25);
        }

        // Dispose resources
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void lblInstructions_Click(object sender, EventArgs e)
        {

        }

        private void lblSubtitle_Click(object sender, EventArgs e)
        {

        }
    }
}
