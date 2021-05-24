
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
            this.ListBoxFeedback = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.ButtonBotStop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ButtonBotStart
            // 
            this.ButtonBotStart.Location = new System.Drawing.Point(534, 21);
            this.ButtonBotStart.Name = "ButtonBotStart";
            this.ButtonBotStart.Size = new System.Drawing.Size(115, 35);
            this.ButtonBotStart.TabIndex = 0;
            this.ButtonBotStart.Text = "Start The Bot";
            this.ButtonBotStart.UseVisualStyleBackColor = true;
            this.ButtonBotStart.Click += new System.EventHandler(this.ButtonBotStart_Click);
            // 
            // TextBoxConsole
            // 
            this.TextBoxConsole.Location = new System.Drawing.Point(534, 62);
            this.TextBoxConsole.Multiline = true;
            this.TextBoxConsole.Name = "TextBoxConsole";
            this.TextBoxConsole.ReadOnly = true;
            this.TextBoxConsole.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TextBoxConsole.Size = new System.Drawing.Size(567, 448);
            this.TextBoxConsole.TabIndex = 1;
            this.TextBoxConsole.UseWaitCursor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(833, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(45, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Console";
            // 
            // ListBoxFeedback
            // 
            this.ListBoxFeedback.FormattingEnabled = true;
            this.ListBoxFeedback.Location = new System.Drawing.Point(42, 62);
            this.ListBoxFeedback.Name = "ListBoxFeedback";
            this.ListBoxFeedback.Size = new System.Drawing.Size(447, 186);
            this.ListBoxFeedback.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(42, 282);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(119, 30);
            this.button1.TabIndex = 4;
            this.button1.Text = "General Stats";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.comboBox1.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(104, 255);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(264, 21);
            this.comboBox1.TabIndex = 5;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(374, 255);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(115, 21);
            this.button2.TabIndex = 6;
            this.button2.Text = "Ingredient Info";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(167, 282);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(119, 30);
            this.button3.TabIndex = 7;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(39, 259);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Ingredients";
            // 
            // ButtonBotStop
            // 
            this.ButtonBotStop.Enabled = false;
            this.ButtonBotStop.Location = new System.Drawing.Point(655, 21);
            this.ButtonBotStop.Name = "ButtonBotStop";
            this.ButtonBotStop.Size = new System.Drawing.Size(115, 35);
            this.ButtonBotStop.TabIndex = 9;
            this.ButtonBotStop.Text = "Stop The Bot";
            this.ButtonBotStop.UseVisualStyleBackColor = true;
            this.ButtonBotStop.Click += new System.EventHandler(this.ButtonBotStop_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1142, 570);
            this.Controls.Add(this.ButtonBotStop);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ListBoxFeedback);
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
        private System.Windows.Forms.ListBox ListBoxFeedback;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ButtonBotStop;
    }
}

