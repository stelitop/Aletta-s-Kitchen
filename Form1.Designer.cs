
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
            this.TextBoxConsole = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ButtonBotStart
            // 
            this.ButtonBotStart.Location = new System.Drawing.Point(465, 21);
            this.ButtonBotStart.Name = "ButtonBotStart";
            this.ButtonBotStart.Size = new System.Drawing.Size(115, 35);
            this.ButtonBotStart.TabIndex = 0;
            this.ButtonBotStart.Text = "Start The Bot";
            this.ButtonBotStart.UseVisualStyleBackColor = true;
            this.ButtonBotStart.Click += new System.EventHandler(this.ButtonBotStart_Click);
            // 
            // TextBoxConsole
            // 
            this.TextBoxConsole.Location = new System.Drawing.Point(465, 62);
            this.TextBoxConsole.Multiline = true;
            this.TextBoxConsole.Name = "TextBoxConsole";
            this.TextBoxConsole.ReadOnly = true;
            this.TextBoxConsole.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TextBoxConsole.Size = new System.Drawing.Size(636, 441);
            this.TextBoxConsole.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(774, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Console";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1142, 570);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TextBoxConsole);
            this.Controls.Add(this.ButtonBotStart);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonBotStart;
        private System.Windows.Forms.TextBox TextBoxConsole;
        private System.Windows.Forms.Label label1;
    }
}

