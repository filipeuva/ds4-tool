namespace ScpServer
{
    partial class Options
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
            this.BlueLabel = new System.Windows.Forms.Label();
            this.GreenLabel = new System.Windows.Forms.Label();
            this.RedLabel = new System.Windows.Forms.Label();
            this.blueBar = new System.Windows.Forms.TrackBar();
            this.greenBar = new System.Windows.Forms.TrackBar();
            this.redBar = new System.Windows.Forms.TrackBar();
            this.blueValLabel = new System.Windows.Forms.Label();
            this.greenValLabel = new System.Windows.Forms.Label();
            this.redValLabel = new System.Windows.Forms.Label();
            this.rumbleBoostMotorValLabel = new System.Windows.Forms.Label();
            this.leftMotorValLabel = new System.Windows.Forms.Label();
            this.rightMotorValLabel = new System.Windows.Forms.Label();
            this.rightMotorLabel = new System.Windows.Forms.Label();
            this.leftMotorLabel = new System.Windows.Forms.Label();
            this.rumbleBoostLabel = new System.Windows.Forms.Label();
            this.rightMotorBar = new System.Windows.Forms.TrackBar();
            this.leftMotorBar = new System.Windows.Forms.TrackBar();
            this.rumbleBoostBar = new System.Windows.Forms.TrackBar();
            this.rumbleLabel = new System.Windows.Forms.Label();
            this.colorLabel = new System.Windows.Forms.Label();
            this.setButton = new System.Windows.Forms.Button();
            this.CloselButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.batteryLed = new System.Windows.Forms.CheckBox();
            this.flashLed = new System.Windows.Forms.CheckBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.touchCheckBox = new System.Windows.Forms.CheckBox();
            this.sensitivityBar = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.sensitivityValLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.blueBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.greenBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.redBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rightMotorBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.leftMotorBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumbleBoostBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sensitivityBar)).BeginInit();
            this.SuspendLayout();
            // 
            // BlueLabel
            // 
            this.BlueLabel.AutoSize = true;
            this.BlueLabel.Location = new System.Drawing.Point(21, 116);
            this.BlueLabel.Name = "BlueLabel";
            this.BlueLabel.Size = new System.Drawing.Size(28, 13);
            this.BlueLabel.TabIndex = 15;
            this.BlueLabel.Text = "Blue";
            // 
            // GreenLabel
            // 
            this.GreenLabel.AutoSize = true;
            this.GreenLabel.Location = new System.Drawing.Point(17, 84);
            this.GreenLabel.Name = "GreenLabel";
            this.GreenLabel.Size = new System.Drawing.Size(36, 13);
            this.GreenLabel.TabIndex = 14;
            this.GreenLabel.Text = "Green";
            // 
            // RedLabel
            // 
            this.RedLabel.AutoSize = true;
            this.RedLabel.Location = new System.Drawing.Point(21, 56);
            this.RedLabel.Name = "RedLabel";
            this.RedLabel.Size = new System.Drawing.Size(27, 13);
            this.RedLabel.TabIndex = 13;
            this.RedLabel.Text = "Red";
            // 
            // blueBar
            // 
            this.blueBar.Location = new System.Drawing.Point(54, 114);
            this.blueBar.Maximum = 255;
            this.blueBar.Name = "blueBar";
            this.blueBar.Size = new System.Drawing.Size(223, 45);
            this.blueBar.TabIndex = 12;
            this.blueBar.TickFrequency = 25;
            this.blueBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.blueBar.ValueChanged += new System.EventHandler(this.blueBar_ValueChanged);
            // 
            // greenBar
            // 
            this.greenBar.Location = new System.Drawing.Point(54, 82);
            this.greenBar.Maximum = 255;
            this.greenBar.Name = "greenBar";
            this.greenBar.Size = new System.Drawing.Size(223, 45);
            this.greenBar.TabIndex = 11;
            this.greenBar.TickFrequency = 25;
            this.greenBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.greenBar.ValueChanged += new System.EventHandler(this.greenBar_ValueChanged);
            // 
            // redBar
            // 
            this.redBar.Location = new System.Drawing.Point(54, 50);
            this.redBar.Maximum = 255;
            this.redBar.Name = "redBar";
            this.redBar.Size = new System.Drawing.Size(223, 45);
            this.redBar.TabIndex = 10;
            this.redBar.TickFrequency = 25;
            this.redBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.redBar.ValueChanged += new System.EventHandler(this.redBar_ValueChanged);
            // 
            // blueValLabel
            // 
            this.blueValLabel.AutoSize = true;
            this.blueValLabel.Location = new System.Drawing.Point(283, 116);
            this.blueValLabel.Name = "blueValLabel";
            this.blueValLabel.Size = new System.Drawing.Size(13, 13);
            this.blueValLabel.TabIndex = 16;
            this.blueValLabel.Text = "0";
            // 
            // greenValLabel
            // 
            this.greenValLabel.AutoSize = true;
            this.greenValLabel.Location = new System.Drawing.Point(283, 84);
            this.greenValLabel.Name = "greenValLabel";
            this.greenValLabel.Size = new System.Drawing.Size(13, 13);
            this.greenValLabel.TabIndex = 17;
            this.greenValLabel.Text = "0";
            // 
            // redValLabel
            // 
            this.redValLabel.AutoSize = true;
            this.redValLabel.Location = new System.Drawing.Point(283, 56);
            this.redValLabel.Name = "redValLabel";
            this.redValLabel.Size = new System.Drawing.Size(13, 13);
            this.redValLabel.TabIndex = 18;
            this.redValLabel.Text = "0";
            // 
            // rumbleBoostMotorValLabel
            // 
            this.rumbleBoostMotorValLabel.AutoSize = true;
            this.rumbleBoostMotorValLabel.Location = new System.Drawing.Point(632, 56);
            this.rumbleBoostMotorValLabel.Name = "rumbleBoostMotorValLabel";
            this.rumbleBoostMotorValLabel.Size = new System.Drawing.Size(25, 13);
            this.rumbleBoostMotorValLabel.TabIndex = 27;
            this.rumbleBoostMotorValLabel.Text = "100";
            // 
            // leftMotorValLabel
            // 
            this.leftMotorValLabel.AutoSize = true;
            this.leftMotorValLabel.Location = new System.Drawing.Point(632, 84);
            this.leftMotorValLabel.Name = "leftMotorValLabel";
            this.leftMotorValLabel.Size = new System.Drawing.Size(13, 13);
            this.leftMotorValLabel.TabIndex = 26;
            this.leftMotorValLabel.Text = "0";
            // 
            // rightMotorValLabel
            // 
            this.rightMotorValLabel.AutoSize = true;
            this.rightMotorValLabel.Location = new System.Drawing.Point(632, 116);
            this.rightMotorValLabel.Name = "rightMotorValLabel";
            this.rightMotorValLabel.Size = new System.Drawing.Size(13, 13);
            this.rightMotorValLabel.TabIndex = 25;
            this.rightMotorValLabel.Text = "0";
            // 
            // rightMotorLabel
            // 
            this.rightMotorLabel.AutoSize = true;
            this.rightMotorLabel.Location = new System.Drawing.Point(370, 116);
            this.rightMotorLabel.Name = "rightMotorLabel";
            this.rightMotorLabel.Size = new System.Drawing.Size(32, 13);
            this.rightMotorLabel.TabIndex = 24;
            this.rightMotorLabel.Text = "Right";
            // 
            // leftMotorLabel
            // 
            this.leftMotorLabel.AutoSize = true;
            this.leftMotorLabel.Location = new System.Drawing.Point(372, 84);
            this.leftMotorLabel.Name = "leftMotorLabel";
            this.leftMotorLabel.Size = new System.Drawing.Size(25, 13);
            this.leftMotorLabel.TabIndex = 23;
            this.leftMotorLabel.Text = "Left";
            // 
            // rumbleBoostLabel
            // 
            this.rumbleBoostLabel.AutoSize = true;
            this.rumbleBoostLabel.Location = new System.Drawing.Point(370, 56);
            this.rumbleBoostLabel.Name = "rumbleBoostLabel";
            this.rumbleBoostLabel.Size = new System.Drawing.Size(34, 13);
            this.rumbleBoostLabel.TabIndex = 22;
            this.rumbleBoostLabel.Text = "Boost";
            // 
            // rightMotorBar
            // 
            this.rightMotorBar.Location = new System.Drawing.Point(403, 114);
            this.rightMotorBar.Maximum = 255;
            this.rightMotorBar.Name = "rightMotorBar";
            this.rightMotorBar.Size = new System.Drawing.Size(223, 45);
            this.rightMotorBar.TabIndex = 21;
            this.rightMotorBar.TickFrequency = 25;
            this.rightMotorBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.rightMotorBar.ValueChanged += new System.EventHandler(this.rightMotorBar_ValueChanged);
            // 
            // leftMotorBar
            // 
            this.leftMotorBar.Location = new System.Drawing.Point(403, 82);
            this.leftMotorBar.Maximum = 255;
            this.leftMotorBar.Name = "leftMotorBar";
            this.leftMotorBar.Size = new System.Drawing.Size(223, 45);
            this.leftMotorBar.TabIndex = 20;
            this.leftMotorBar.TickFrequency = 25;
            this.leftMotorBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.leftMotorBar.ValueChanged += new System.EventHandler(this.leftMotorBar_ValueChanged);
            // 
            // rumbleBoostBar
            // 
            this.rumbleBoostBar.Location = new System.Drawing.Point(403, 50);
            this.rumbleBoostBar.Maximum = 200;
            this.rumbleBoostBar.Name = "rumbleBoostBar";
            this.rumbleBoostBar.Size = new System.Drawing.Size(223, 45);
            this.rumbleBoostBar.TabIndex = 19;
            this.rumbleBoostBar.TickFrequency = 25;
            this.rumbleBoostBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.rumbleBoostBar.Value = 100;
            this.rumbleBoostBar.ValueChanged += new System.EventHandler(this.rumbleBoostBar_ValueChanged);
            // 
            // rumbleLabel
            // 
            this.rumbleLabel.AutoSize = true;
            this.rumbleLabel.Location = new System.Drawing.Point(505, 9);
            this.rumbleLabel.Name = "rumbleLabel";
            this.rumbleLabel.Size = new System.Drawing.Size(43, 13);
            this.rumbleLabel.TabIndex = 28;
            this.rumbleLabel.Text = "Rumble";
            // 
            // colorLabel
            // 
            this.colorLabel.AutoSize = true;
            this.colorLabel.Location = new System.Drawing.Point(148, 9);
            this.colorLabel.Name = "colorLabel";
            this.colorLabel.Size = new System.Drawing.Size(31, 13);
            this.colorLabel.TabIndex = 29;
            this.colorLabel.Text = "Color";
            // 
            // setButton
            // 
            this.setButton.Location = new System.Drawing.Point(599, 224);
            this.setButton.Name = "setButton";
            this.setButton.Size = new System.Drawing.Size(75, 23);
            this.setButton.TabIndex = 30;
            this.setButton.Text = "Set";
            this.setButton.UseVisualStyleBackColor = true;
            this.setButton.Click += new System.EventHandler(this.setButton_Click);
            // 
            // CloselButton
            // 
            this.CloselButton.Location = new System.Drawing.Point(54, 224);
            this.CloselButton.Name = "CloselButton";
            this.CloselButton.Size = new System.Drawing.Size(75, 23);
            this.CloselButton.TabIndex = 31;
            this.CloselButton.Text = "Close";
            this.CloselButton.UseVisualStyleBackColor = true;
            this.CloselButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(694, 224);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 32;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // batteryLed
            // 
            this.batteryLed.AutoSize = true;
            this.batteryLed.Location = new System.Drawing.Point(54, 154);
            this.batteryLed.Name = "batteryLed";
            this.batteryLed.Size = new System.Drawing.Size(170, 17);
            this.batteryLed.TabIndex = 33;
            this.batteryLed.Text = "Use LED as a battery indicator";
            this.batteryLed.UseVisualStyleBackColor = true;
            this.batteryLed.CheckedChanged += new System.EventHandler(this.batteryLed_CheckedChanged);
            // 
            // flashLed
            // 
            this.flashLed.AutoSize = true;
            this.flashLed.Location = new System.Drawing.Point(54, 177);
            this.flashLed.Name = "flashLed";
            this.flashLed.Size = new System.Drawing.Size(174, 17);
            this.flashLed.TabIndex = 34;
            this.flashLed.Text = "Flash LED when battery at 20%";
            this.flashLed.UseVisualStyleBackColor = true;
            this.flashLed.CheckedChanged += new System.EventHandler(this.flashLed_CheckedChanged);
            // 
            // colorDialog1
            // 
            this.colorDialog1.AnyColor = true;
            this.colorDialog1.Color = System.Drawing.Color.Blue;
            // 
            // touchCheckBox
            // 
            this.touchCheckBox.AutoSize = true;
            this.touchCheckBox.Location = new System.Drawing.Point(373, 154);
            this.touchCheckBox.Name = "touchCheckBox";
            this.touchCheckBox.Size = new System.Drawing.Size(142, 17);
            this.touchCheckBox.TabIndex = 35;
            this.touchCheckBox.Text = "Enable touchpad at start";
            this.touchCheckBox.UseVisualStyleBackColor = true;
            this.touchCheckBox.CheckedChanged += new System.EventHandler(this.touchCheckBox_CheckedChanged);
            // 
            // sensitivityBar
            // 
            this.sensitivityBar.Location = new System.Drawing.Point(699, 50);
            this.sensitivityBar.Maximum = 150;
            this.sensitivityBar.Minimum = 10;
            this.sensitivityBar.Name = "sensitivityBar";
            this.sensitivityBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.sensitivityBar.Size = new System.Drawing.Size(45, 104);
            this.sensitivityBar.TabIndex = 36;
            this.sensitivityBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.sensitivityBar.Value = 100;
            this.sensitivityBar.ValueChanged += new System.EventHandler(this.sensitivityBar_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(663, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 37;
            this.label1.Text = "Touchpad Sensitivity";
            // 
            // sensitivityValLabel
            // 
            this.sensitivityValLabel.AutoSize = true;
            this.sensitivityValLabel.Location = new System.Drawing.Point(699, 161);
            this.sensitivityValLabel.Name = "sensitivityValLabel";
            this.sensitivityValLabel.Size = new System.Drawing.Size(25, 13);
            this.sensitivityValLabel.TabIndex = 38;
            this.sensitivityValLabel.Text = "100";
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(781, 265);
            this.Controls.Add(this.sensitivityValLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.sensitivityBar);
            this.Controls.Add(this.touchCheckBox);
            this.Controls.Add(this.flashLed);
            this.Controls.Add(this.batteryLed);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.CloselButton);
            this.Controls.Add(this.setButton);
            this.Controls.Add(this.colorLabel);
            this.Controls.Add(this.rumbleLabel);
            this.Controls.Add(this.rumbleBoostMotorValLabel);
            this.Controls.Add(this.leftMotorValLabel);
            this.Controls.Add(this.rightMotorValLabel);
            this.Controls.Add(this.rightMotorLabel);
            this.Controls.Add(this.leftMotorLabel);
            this.Controls.Add(this.rumbleBoostLabel);
            this.Controls.Add(this.rightMotorBar);
            this.Controls.Add(this.leftMotorBar);
            this.Controls.Add(this.rumbleBoostBar);
            this.Controls.Add(this.redValLabel);
            this.Controls.Add(this.greenValLabel);
            this.Controls.Add(this.blueValLabel);
            this.Controls.Add(this.BlueLabel);
            this.Controls.Add(this.GreenLabel);
            this.Controls.Add(this.RedLabel);
            this.Controls.Add(this.blueBar);
            this.Controls.Add(this.greenBar);
            this.Controls.Add(this.redBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Options";
            this.Text = "Options";
            ((System.ComponentModel.ISupportInitialize)(this.blueBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.greenBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.redBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rightMotorBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.leftMotorBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumbleBoostBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sensitivityBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label BlueLabel;
        private System.Windows.Forms.Label GreenLabel;
        private System.Windows.Forms.Label RedLabel;
        private System.Windows.Forms.TrackBar blueBar;
        private System.Windows.Forms.TrackBar greenBar;
        private System.Windows.Forms.Label blueValLabel;
        private System.Windows.Forms.Label greenValLabel;
        private System.Windows.Forms.Label redValLabel;
        private System.Windows.Forms.Label rumbleBoostMotorValLabel;
        private System.Windows.Forms.Label leftMotorValLabel;
        private System.Windows.Forms.Label rightMotorValLabel;
        private System.Windows.Forms.Label rightMotorLabel;
        private System.Windows.Forms.Label leftMotorLabel;
        private System.Windows.Forms.Label rumbleBoostLabel;
        private System.Windows.Forms.TrackBar rightMotorBar;
        private System.Windows.Forms.TrackBar leftMotorBar;
        private System.Windows.Forms.TrackBar rumbleBoostBar;
        private System.Windows.Forms.Label rumbleLabel;
        private System.Windows.Forms.Label colorLabel;
        private System.Windows.Forms.Button setButton;
        private System.Windows.Forms.Button CloselButton;
        private System.Windows.Forms.TrackBar redBar;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.CheckBox batteryLed;
        private System.Windows.Forms.CheckBox flashLed;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.CheckBox touchCheckBox;
        private System.Windows.Forms.TrackBar sensitivityBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label sensitivityValLabel;
    }
}