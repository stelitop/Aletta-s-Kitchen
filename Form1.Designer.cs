
namespace Aletta_s_Kitchen
{
    partial class Form1
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
            this.ButtonBotStart = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ButtonBotStart
            // 
            this.ButtonBotStart.Location = new System.Drawing.Point(107, 118);
            this.ButtonBotStart.Name = "ButtonBotStart";
            this.ButtonBotStart.Size = new System.Drawing.Size(115, 35);
            this.ButtonBotStart.TabIndex = 0;
            this.ButtonBotStart.Text = "Start The Bot";
            this.ButtonBotStart.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ButtonBotStart);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button ButtonBotStart;
    }
}

