﻿
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
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.ButtonToggleConsole = new System.Windows.Forms.Button();
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
            this.TextBoxConsole.Cursor = System.Windows.Forms.Cursors.Default;
            this.TextBoxConsole.Location = new System.Drawing.Point(534, 62);
            this.TextBoxConsole.Multiline = true;
            this.TextBoxConsole.Name = "TextBoxConsole";
            this.TextBoxConsole.ReadOnly = true;
            this.TextBoxConsole.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.TextBoxConsole.Size = new System.Drawing.Size(567, 448);
            this.TextBoxConsole.TabIndex = 1;
            this.TextBoxConsole.TabStop = false;
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
            this.ListBoxFeedback.HorizontalScrollbar = true;
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
            this.comboBox1.Location = new System.Drawing.Point(104, 256);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(260, 21);
            this.comboBox1.TabIndex = 5;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(370, 255);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(119, 21);
            this.button2.TabIndex = 6;
            this.button2.Text = "Ingredient Info";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(370, 480);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(119, 30);
            this.button3.TabIndex = 7;
            this.button3.Text = "Random Testing";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
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
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(167, 282);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(119, 28);
            this.button4.TabIndex = 10;
            this.button4.Text = "Generate A Kitchen";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(292, 282);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(119, 28);
            this.button5.TabIndex = 11;
            this.button5.Text = "Running Games";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(42, 318);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(119, 28);
            this.button6.TabIndex = 12;
            this.button6.Text = "Remove Elementals";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(167, 318);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(119, 28);
            this.button7.TabIndex = 13;
            this.button7.Text = "Toggle Playing";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // ButtonToggleConsole
            // 
            this.ButtonToggleConsole.Location = new System.Drawing.Point(1016, 21);
            this.ButtonToggleConsole.Name = "ButtonToggleConsole";
            this.ButtonToggleConsole.Size = new System.Drawing.Size(85, 35);
            this.ButtonToggleConsole.TabIndex = 14;
            this.ButtonToggleConsole.Text = "Toggle Console";
            this.ButtonToggleConsole.UseVisualStyleBackColor = true;
            this.ButtonToggleConsole.Click += new System.EventHandler(this.ButtonToggleConsole_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1142, 570);
            this.Controls.Add(this.ButtonToggleConsole);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
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
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
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
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button ButtonToggleConsole;
    }
}

