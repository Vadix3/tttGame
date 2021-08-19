
namespace tttGame
{
    partial class Start_Dialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.startGame = new System.Windows.Forms.Button();
            this.replayGame = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // startGame
            // 
            this.startGame.BackColor = System.Drawing.Color.White;
            this.startGame.ForeColor = System.Drawing.Color.Blue;
            this.startGame.Location = new System.Drawing.Point(65, 87);
            this.startGame.Name = "startGame";
            this.startGame.Size = new System.Drawing.Size(93, 76);
            this.startGame.TabIndex = 6;
            this.startGame.Text = "Start Game";
            this.startGame.UseVisualStyleBackColor = false;
            this.startGame.Click += new System.EventHandler(this.startGame_Click);
            // 
            // replayGame
            // 
            this.replayGame.BackColor = System.Drawing.Color.White;
            this.replayGame.ForeColor = System.Drawing.Color.Blue;
            this.replayGame.Location = new System.Drawing.Point(284, 87);
            this.replayGame.Name = "replayGame";
            this.replayGame.Size = new System.Drawing.Size(94, 76);
            this.replayGame.TabIndex = 7;
            this.replayGame.Text = "Replay Game";
            this.replayGame.UseVisualStyleBackColor = false;
            this.replayGame.Click += new System.EventHandler(this.replayGame_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.label1.Location = new System.Drawing.Point(142, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 27);
            this.label1.TabIndex = 8;
            this.label1.Text = "Tic tac toe";
            // 
            // Start_Dialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(437, 208);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.replayGame);
            this.Controls.Add(this.startGame);
            this.Name = "Start_Dialog";
            this.Text = "Start_Dialog";
            this.Load += new System.EventHandler(this.Start_Dialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startGame;
        private System.Windows.Forms.Button replayGame;
        private System.Windows.Forms.Label label1;
    }
}