namespace RibbonDemo
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
            this.btMainForm = new System.Windows.Forms.Button();
            this.btTestForm = new System.Windows.Forms.Button();
            this.btHostForm = new System.Windows.Forms.Button();
            this.btBlackForm = new System.Windows.Forms.Button();
            this.btRightToLeft = new System.Windows.Forms.Button();
            this.btThemeBuilderForm = new System.Windows.Forms.Button();
            this.btnMDIForm = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btMainForm
            // 
            this.btMainForm.Location = new System.Drawing.Point(33, 35);
            this.btMainForm.Name = "btMainForm";
            this.btMainForm.Size = new System.Drawing.Size(213, 23);
            this.btMainForm.TabIndex = 0;
            this.btMainForm.Text = "MainForm";
            this.btMainForm.UseVisualStyleBackColor = true;
            this.btMainForm.Click += new System.EventHandler(this.btMainForm_Click);
            // 
            // btTestForm
            // 
            this.btTestForm.Location = new System.Drawing.Point(33, 93);
            this.btTestForm.Name = "btTestForm";
            this.btTestForm.Size = new System.Drawing.Size(213, 23);
            this.btTestForm.TabIndex = 1;
            this.btTestForm.Text = "TestForm";
            this.btTestForm.UseVisualStyleBackColor = true;
            this.btTestForm.Click += new System.EventHandler(this.btTestForm_Click);
            // 
            // btHostForm
            // 
            this.btHostForm.Location = new System.Drawing.Point(33, 122);
            this.btHostForm.Name = "btHostForm";
            this.btHostForm.Size = new System.Drawing.Size(213, 23);
            this.btHostForm.TabIndex = 2;
            this.btHostForm.Text = "HostForm";
            this.btHostForm.UseVisualStyleBackColor = true;
            this.btHostForm.Click += new System.EventHandler(this.btHostForm_Click);
            // 
            // btBlackForm
            // 
            this.btBlackForm.Location = new System.Drawing.Point(33, 151);
            this.btBlackForm.Name = "btBlackForm";
            this.btBlackForm.Size = new System.Drawing.Size(213, 23);
            this.btBlackForm.TabIndex = 3;
            this.btBlackForm.Text = "BlackForm";
            this.btBlackForm.UseVisualStyleBackColor = true;
            this.btBlackForm.Click += new System.EventHandler(this.btBlackForm_Click);
            // 
            // btRightToLeft
            // 
            this.btRightToLeft.Location = new System.Drawing.Point(33, 180);
            this.btRightToLeft.Name = "btRightToLeft";
            this.btRightToLeft.Size = new System.Drawing.Size(213, 23);
            this.btRightToLeft.TabIndex = 4;
            this.btRightToLeft.Text = "RightToLeftForm";
            this.btRightToLeft.UseVisualStyleBackColor = true;
            this.btRightToLeft.Click += new System.EventHandler(this.btRightToLeft_Click);
            // 
            // btThemeBuilderForm
            // 
            this.btThemeBuilderForm.Location = new System.Drawing.Point(33, 209);
            this.btThemeBuilderForm.Name = "btThemeBuilderForm";
            this.btThemeBuilderForm.Size = new System.Drawing.Size(213, 23);
            this.btThemeBuilderForm.TabIndex = 5;
            this.btThemeBuilderForm.Text = "ThemeBuilderForm";
            this.btThemeBuilderForm.UseVisualStyleBackColor = true;
            this.btThemeBuilderForm.Click += new System.EventHandler(this.btThemeBuilderForm_Click);
            // 
            // btnMDIForm
            // 
            this.btnMDIForm.Location = new System.Drawing.Point(33, 64);
            this.btnMDIForm.Name = "btnMDIForm";
            this.btnMDIForm.Size = new System.Drawing.Size(213, 23);
            this.btnMDIForm.TabIndex = 6;
            this.btnMDIForm.Text = "MDIForm";
            this.btnMDIForm.UseVisualStyleBackColor = true;
            this.btnMDIForm.Click += new System.EventHandler(this.btnMDIForm_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(301, 260);
            this.Controls.Add(this.btnMDIForm);
            this.Controls.Add(this.btThemeBuilderForm);
            this.Controls.Add(this.btRightToLeft);
            this.Controls.Add(this.btBlackForm);
            this.Controls.Add(this.btHostForm);
            this.Controls.Add(this.btTestForm);
            this.Controls.Add(this.btMainForm);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btMainForm;
        private System.Windows.Forms.Button btTestForm;
        private System.Windows.Forms.Button btHostForm;
        private System.Windows.Forms.Button btBlackForm;
        private System.Windows.Forms.Button btRightToLeft;
        private System.Windows.Forms.Button btThemeBuilderForm;
        private System.Windows.Forms.Button btnMDIForm;
    }
}