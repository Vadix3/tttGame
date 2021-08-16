
namespace tttGame
{
    partial class Win_Dialog
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
            this.Win_Message = new System.Windows.Forms.Label();
            this.btnAgain = new System.Windows.Forms.Button();
            this.btnQuit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Win_Message
            // 
            this.Win_Message.AutoSize = true;
            this.Win_Message.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.Win_Message.Location = new System.Drawing.Point(21, 74);
            this.Win_Message.Name = "Win_Message";
            this.Win_Message.Size = new System.Drawing.Size(490, 48);
            this.Win_Message.TabIndex = 0;
            this.Win_Message.Text = "Player X has won in 8 turns!";
            this.Win_Message.Click += new System.EventHandler(this.Win_Message_Click);
            // 
            // btnAgain
            // 
            this.btnAgain.Location = new System.Drawing.Point(39, 189);
            this.btnAgain.Name = "btnAgain";
            this.btnAgain.Size = new System.Drawing.Size(170, 80);
            this.btnAgain.TabIndex = 1;
            this.btnAgain.Text = "Play again";
            this.btnAgain.UseVisualStyleBackColor = true;
            this.btnAgain.Click += new System.EventHandler(this.btnAgain_Click);
            // 
            // btnQuit
            // 
            this.btnQuit.Location = new System.Drawing.Point(322, 189);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.Size = new System.Drawing.Size(170, 80);
            this.btnQuit.TabIndex = 2;
            this.btnQuit.Text = "Quit";
            this.btnQuit.UseVisualStyleBackColor = true;
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // Win_Dialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 317);
            this.Controls.Add(this.btnQuit);
            this.Controls.Add(this.btnAgain);
            this.Controls.Add(this.Win_Message);
            this.Name = "Win_Dialog";
            this.Text = "Game Over";
            this.Load += new System.EventHandler(this.Win_Dialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label Win_Message;
        private System.Windows.Forms.Button btnAgain;
        private System.Windows.Forms.Button btnQuit;
    }
}