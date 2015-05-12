namespace HotSwapping
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txbError = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnExecute = new System.Windows.Forms.Button();
            this.btnInject = new System.Windows.Forms.Button();
            this.txbCode = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txbMonoError = new System.Windows.Forms.TextBox();
            this.txbMonoCode = new System.Windows.Forms.TextBox();
            this.btnExecuteCode = new System.Windows.Forms.Button();
            this.btnInjectCode = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txbError);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnExecute);
            this.groupBox1.Controls.Add(this.btnInject);
            this.groupBox1.Controls.Add(this.txbCode);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(661, 308);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Pointer method";
            // 
            // txbError
            // 
            this.txbError.ForeColor = System.Drawing.Color.Red;
            this.txbError.Location = new System.Drawing.Point(6, 218);
            this.txbError.Multiline = true;
            this.txbError.Name = "txbError";
            this.txbError.Size = new System.Drawing.Size(519, 84);
            this.txbError.TabIndex = 4;
            this.txbError.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Fill in your code:";
            // 
            // btnExecute
            // 
            this.btnExecute.Location = new System.Drawing.Point(532, 189);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(123, 23);
            this.btnExecute.TabIndex = 1;
            this.btnExecute.Text = "Execute code";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // btnInject
            // 
            this.btnInject.Location = new System.Drawing.Point(532, 49);
            this.btnInject.Name = "btnInject";
            this.btnInject.Size = new System.Drawing.Size(123, 23);
            this.btnInject.TabIndex = 1;
            this.btnInject.Text = "Inject code";
            this.btnInject.UseVisualStyleBackColor = true;
            this.btnInject.Click += new System.EventHandler(this.btnInject_Click);
            // 
            // txbCode
            // 
            this.txbCode.Location = new System.Drawing.Point(6, 49);
            this.txbCode.Multiline = true;
            this.txbCode.Name = "txbCode";
            this.txbCode.Size = new System.Drawing.Size(519, 163);
            this.txbCode.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txbMonoError);
            this.groupBox2.Controls.Add(this.txbMonoCode);
            this.groupBox2.Controls.Add(this.btnExecuteCode);
            this.groupBox2.Controls.Add(this.btnInjectCode);
            this.groupBox2.Location = new System.Drawing.Point(12, 335);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(655, 288);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Mono cecil method";
            // 
            // txbMonoError
            // 
            this.txbMonoError.ForeColor = System.Drawing.Color.Red;
            this.txbMonoError.Location = new System.Drawing.Point(10, 190);
            this.txbMonoError.Multiline = true;
            this.txbMonoError.Name = "txbMonoError";
            this.txbMonoError.Size = new System.Drawing.Size(515, 84);
            this.txbMonoError.TabIndex = 2;
            this.txbMonoError.Visible = false;
            // 
            // txbMonoCode
            // 
            this.txbMonoCode.Location = new System.Drawing.Point(10, 21);
            this.txbMonoCode.Multiline = true;
            this.txbMonoCode.Name = "txbMonoCode";
            this.txbMonoCode.Size = new System.Drawing.Size(515, 163);
            this.txbMonoCode.TabIndex = 1;
            // 
            // btnExecuteCode
            // 
            this.btnExecuteCode.Location = new System.Drawing.Point(532, 161);
            this.btnExecuteCode.Name = "btnExecuteCode";
            this.btnExecuteCode.Size = new System.Drawing.Size(117, 23);
            this.btnExecuteCode.TabIndex = 0;
            this.btnExecuteCode.Text = "Execute method";
            this.btnExecuteCode.UseVisualStyleBackColor = true;
            this.btnExecuteCode.Click += new System.EventHandler(this.btnExecuteCode_Click);
            // 
            // btnInjectCode
            // 
            this.btnInjectCode.Location = new System.Drawing.Point(532, 21);
            this.btnInjectCode.Name = "btnInjectCode";
            this.btnInjectCode.Size = new System.Drawing.Size(117, 23);
            this.btnInjectCode.TabIndex = 0;
            this.btnInjectCode.Text = "Inject method";
            this.btnInjectCode.UseVisualStyleBackColor = true;
            this.btnInjectCode.Click += new System.EventHandler(this.btnInjectCode_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(685, 634);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Swapping-Code Application";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.Button btnInject;
        private System.Windows.Forms.TextBox txbCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txbError;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnExecuteCode;
        private System.Windows.Forms.Button btnInjectCode;
        private System.Windows.Forms.TextBox txbMonoError;
        private System.Windows.Forms.TextBox txbMonoCode;
    }
}

