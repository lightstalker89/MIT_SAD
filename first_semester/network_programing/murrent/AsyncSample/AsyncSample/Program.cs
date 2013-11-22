// *******************************************************
// * <copyright file="Program.cs" company="MDMCoWorks">
// * Copyright (c) 2013 Mario Murrent. All rights reserved.
// * </copyright>
// * <summary>
// *
// * </summary>
// * <author>Mario Murrent</author>
// *******************************************************/
namespace AsyncSample
{
    using System;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// </summary>
    public class Program : Form
    {
        /// <summary>
        /// </summary>
        /// <summary>
        /// </summary>
        private Label label1;

        /// <summary>
        /// </summary>
        private TextBox textBox1;

        /// <summary>
        /// </summary>
        private GroupBox groupBox1;

        /// <summary>
        /// </summary>
        private ListBox listBox1;

        /// <summary>
        /// </summary>
        private Button button1;

        /// <summary>
        /// </summary>
        private Button button3;

        /// <summary>
        /// </summary>
        private Button button2;

        /// <summary>
        /// </summary>
        private readonly Button button;

        ///// <summary>
        ///// </summary>
        ///// <param name="args">
        ///// </param>
        // private static void Main(string[] args)
        // {
        // Application.Run(new Program());
        // }

        /// <summary>
        /// </summary>
        public Program()
        {
            InitializeComponent();

            this.Text = "My first window program";
            this.Size = new Size(400, 400);
        }

        /// <summary>
        /// </summary>
        /// <param name="sender">
        /// </param>
        /// <param name="e">
        /// </param>
        private void ButtonClick(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();

            // label1
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter text:";

            // textBox1
            this.textBox1.Location = new System.Drawing.Point(15, 25);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(268, 20);
            this.textBox1.TabIndex = 1;

            // groupBox1
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Location = new System.Drawing.Point(289, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(177, 191);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Server Control";

            // listBox1
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(15, 77);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(268, 121);
            this.listBox1.TabIndex = 3;

            // button1
            this.button1.Location = new System.Drawing.Point(207, 49);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Send";
            this.button1.UseVisualStyleBackColor = true;

            // button2
            this.button2.Location = new System.Drawing.Point(6, 19);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(165, 26);
            this.button2.TabIndex = 0;
            this.button2.Text = "Connect to Server";
            this.button2.UseVisualStyleBackColor = true;

            // button3
            this.button3.Location = new System.Drawing.Point(6, 51);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(165, 26);
            this.button3.TabIndex = 1;
            this.button3.Text = "Disconnect from Server";
            this.button3.UseVisualStyleBackColor = true;

            // Program
            this.ClientSize = new System.Drawing.Size(478, 215);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label1);
            this.Name = "Program";
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}