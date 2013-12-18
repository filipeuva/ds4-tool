namespace ScpDriver
{
    partial class ScpForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ScpForm));
            this.btnUninstall = new System.Windows.Forms.Button();
            this.btnInstall = new System.Windows.Forms.Button();
            this.InstallWorker = new System.ComponentModel.BackgroundWorker();
            this.UninstallWorker = new System.ComponentModel.BackgroundWorker();
            this.tbOutput = new System.Windows.Forms.TextBox();
            this.pbRunning = new System.Windows.Forms.ProgressBar();
            this.btnExit = new System.Windows.Forms.Button();
            this.cbForce = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btnUninstall
            // 
            this.btnUninstall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUninstall.Location = new System.Drawing.Point(416, 377);
            this.btnUninstall.Name = "btnUninstall";
            this.btnUninstall.Size = new System.Drawing.Size(75, 23);
            this.btnUninstall.TabIndex = 3;
            this.btnUninstall.Text = "&Uninstall";
            this.btnUninstall.UseVisualStyleBackColor = true;
            this.btnUninstall.Click += new System.EventHandler(this.btnUninstall_Click);
            // 
            // btnInstall
            // 
            this.btnInstall.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInstall.Location = new System.Drawing.Point(335, 377);
            this.btnInstall.Name = "btnInstall";
            this.btnInstall.Size = new System.Drawing.Size(75, 23);
            this.btnInstall.TabIndex = 2;
            this.btnInstall.Text = "&Install";
            this.btnInstall.UseVisualStyleBackColor = true;
            this.btnInstall.Click += new System.EventHandler(this.btnInstall_Click);
            // 
            // InstallWorker
            // 
            this.InstallWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.InstallWorker_DoWork);
            this.InstallWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.InstallWorker_RunWorkerCompleted);
            // 
            // UninstallWorker
            // 
            this.UninstallWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.UninstallWorker_DoWork);
            this.UninstallWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.UninstallWorker_RunWorkerCompleted);
            // 
            // tbOutput
            // 
            this.tbOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOutput.BackColor = System.Drawing.SystemColors.Window;
            this.tbOutput.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbOutput.Location = new System.Drawing.Point(13, 13);
            this.tbOutput.Multiline = true;
            this.tbOutput.Name = "tbOutput";
            this.tbOutput.ReadOnly = true;
            this.tbOutput.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbOutput.Size = new System.Drawing.Size(559, 335);
            this.tbOutput.TabIndex = 4;
            this.tbOutput.TabStop = false;
            this.tbOutput.WordWrap = false;
            // 
            // pbRunning
            // 
            this.pbRunning.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbRunning.Location = new System.Drawing.Point(13, 354);
            this.pbRunning.Name = "pbRunning";
            this.pbRunning.Size = new System.Drawing.Size(559, 17);
            this.pbRunning.TabIndex = 5;
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Location = new System.Drawing.Point(497, 377);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 0;
            this.btnExit.Text = "E&xit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // cbForce
            // 
            this.cbForce.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cbForce.AutoSize = true;
            this.cbForce.Location = new System.Drawing.Point(237, 381);
            this.cbForce.Name = "cbForce";
            this.cbForce.Size = new System.Drawing.Size(83, 17);
            this.cbForce.TabIndex = 8;
            this.cbForce.Text = "Force Install";
            this.cbForce.UseVisualStyleBackColor = true;
            // 
            // ScpForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 412);
            this.Controls.Add(this.cbForce);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.pbRunning);
            this.Controls.Add(this.tbOutput);
            this.Controls.Add(this.btnInstall);
            this.Controls.Add(this.btnUninstall);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(400, 300);
            this.Name = "ScpForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "SCP Virtual Bus Driver Installer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ScpForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnUninstall;
        private System.Windows.Forms.Button btnInstall;
        private System.ComponentModel.BackgroundWorker InstallWorker;
        private System.ComponentModel.BackgroundWorker UninstallWorker;
        private System.Windows.Forms.TextBox tbOutput;
        private System.Windows.Forms.ProgressBar pbRunning;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.CheckBox cbForce;
    }
}

