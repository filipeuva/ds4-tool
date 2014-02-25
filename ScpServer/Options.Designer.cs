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
            this.CustomMappingButton = new System.Windows.Forms.Button();
            this.saveButton = new System.Windows.Forms.Button();
            this.batteryLed = new System.Windows.Forms.CheckBox();
            this.flashLed = new System.Windows.Forms.CheckBox();
            this.touchCheckBox = new System.Windows.Forms.CheckBox();
            this.touchSensitivityBar = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.sensitivityValLabel = new System.Windows.Forms.Label();
            this.lowerRCOffCheckBox = new System.Windows.Forms.CheckBox();
            this.lowRedValLabel = new System.Windows.Forms.Label();
            this.lowGreenValLabel = new System.Windows.Forms.Label();
            this.lowBlueValLabel = new System.Windows.Forms.Label();
            this.fullColorLabel = new System.Windows.Forms.Label();
            this.lowColorLabel = new System.Windows.Forms.Label();
            this.lowLedCheckBox = new System.Windows.Forms.CheckBox();
            this.lowLedPanel = new System.Windows.Forms.Panel();
            this.lowColorChooserButton = new System.Windows.Forms.Button();
            this.fullLedPanel = new System.Windows.Forms.Panel();
            this.colorChooserButton = new System.Windows.Forms.Button();
            this.tapSensitivityValLabel = new System.Windows.Forms.Label();
            this.tapSensitivityBar = new System.Windows.Forms.TrackBar();
            this.scrollSensitivityValLabel = new System.Windows.Forms.Label();
            this.scrollSensitivityBar = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.realTimeChangesCheckBox = new System.Windows.Forms.CheckBox();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.rightTriggerMiddlePoint = new System.Windows.Forms.TextBox();
            this.leftTriggerMiddlePoint = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.flushHIDQueue = new System.Windows.Forms.CheckBox();
            this.touchpadJitterCompensation = new System.Windows.Forms.CheckBox();
            this.advColorDialog = new ScpServer.AdvancedColorDialog();
            ((System.ComponentModel.ISupportInitialize)(this.blueBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.greenBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.redBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rightMotorBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.leftMotorBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumbleBoostBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.touchSensitivityBar)).BeginInit();
            this.lowLedPanel.SuspendLayout();
            this.fullLedPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tapSensitivityBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scrollSensitivityBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // BlueLabel
            // 
            this.BlueLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.BlueLabel.AutoSize = true;
            this.BlueLabel.Location = new System.Drawing.Point(44, 116);
            this.BlueLabel.Name = "BlueLabel";
            this.BlueLabel.Size = new System.Drawing.Size(28, 13);
            this.BlueLabel.TabIndex = 15;
            this.BlueLabel.Text = "Blue";
            // 
            // GreenLabel
            // 
            this.GreenLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.GreenLabel.AutoSize = true;
            this.GreenLabel.Location = new System.Drawing.Point(40, 84);
            this.GreenLabel.Name = "GreenLabel";
            this.GreenLabel.Size = new System.Drawing.Size(36, 13);
            this.GreenLabel.TabIndex = 14;
            this.GreenLabel.Text = "Green";
            // 
            // RedLabel
            // 
            this.RedLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.RedLabel.AutoSize = true;
            this.RedLabel.Location = new System.Drawing.Point(44, 56);
            this.RedLabel.Name = "RedLabel";
            this.RedLabel.Size = new System.Drawing.Size(27, 13);
            this.RedLabel.TabIndex = 13;
            this.RedLabel.Text = "Red";
            // 
            // blueBar
            // 
            this.blueBar.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.blueBar.Location = new System.Drawing.Point(77, 114);
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
            this.greenBar.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.greenBar.Location = new System.Drawing.Point(77, 82);
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
            this.redBar.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.redBar.Location = new System.Drawing.Point(77, 50);
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
            this.blueValLabel.Location = new System.Drawing.Point(2, 66);
            this.blueValLabel.Name = "blueValLabel";
            this.blueValLabel.Size = new System.Drawing.Size(30, 13);
            this.blueValLabel.TabIndex = 16;
            this.blueValLabel.Text = "0";
            this.blueValLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // greenValLabel
            // 
            this.greenValLabel.Location = new System.Drawing.Point(2, 34);
            this.greenValLabel.Name = "greenValLabel";
            this.greenValLabel.Size = new System.Drawing.Size(30, 13);
            this.greenValLabel.TabIndex = 17;
            this.greenValLabel.Text = "0";
            this.greenValLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // redValLabel
            // 
            this.redValLabel.Location = new System.Drawing.Point(2, 2);
            this.redValLabel.Name = "redValLabel";
            this.redValLabel.Size = new System.Drawing.Size(30, 13);
            this.redValLabel.TabIndex = 18;
            this.redValLabel.Text = "0";
            this.redValLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rumbleBoostMotorValLabel
            // 
            this.rumbleBoostMotorValLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.rumbleBoostMotorValLabel.Location = new System.Drawing.Point(629, 53);
            this.rumbleBoostMotorValLabel.Name = "rumbleBoostMotorValLabel";
            this.rumbleBoostMotorValLabel.Size = new System.Drawing.Size(30, 13);
            this.rumbleBoostMotorValLabel.TabIndex = 27;
            this.rumbleBoostMotorValLabel.Text = "100";
            this.rumbleBoostMotorValLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // leftMotorValLabel
            // 
            this.leftMotorValLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.leftMotorValLabel.Location = new System.Drawing.Point(629, 85);
            this.leftMotorValLabel.Name = "leftMotorValLabel";
            this.leftMotorValLabel.Size = new System.Drawing.Size(30, 13);
            this.leftMotorValLabel.TabIndex = 26;
            this.leftMotorValLabel.Text = "0";
            this.leftMotorValLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rightMotorValLabel
            // 
            this.rightMotorValLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.rightMotorValLabel.Location = new System.Drawing.Point(629, 119);
            this.rightMotorValLabel.Name = "rightMotorValLabel";
            this.rightMotorValLabel.Size = new System.Drawing.Size(30, 13);
            this.rightMotorValLabel.TabIndex = 25;
            this.rightMotorValLabel.Text = "0";
            this.rightMotorValLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // rightMotorLabel
            // 
            this.rightMotorLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.rightMotorLabel.AutoSize = true;
            this.rightMotorLabel.Location = new System.Drawing.Point(393, 119);
            this.rightMotorLabel.Name = "rightMotorLabel";
            this.rightMotorLabel.Size = new System.Drawing.Size(30, 13);
            this.rightMotorLabel.TabIndex = 24;
            this.rightMotorLabel.Text = "Light";
            // 
            // leftMotorLabel
            // 
            this.leftMotorLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.leftMotorLabel.AutoSize = true;
            this.leftMotorLabel.Location = new System.Drawing.Point(385, 85);
            this.leftMotorLabel.Name = "leftMotorLabel";
            this.leftMotorLabel.Size = new System.Drawing.Size(38, 13);
            this.leftMotorLabel.TabIndex = 23;
            this.leftMotorLabel.Text = "Heavy";
            // 
            // rumbleBoostLabel
            // 
            this.rumbleBoostLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.rumbleBoostLabel.AutoSize = true;
            this.rumbleBoostLabel.Location = new System.Drawing.Point(393, 53);
            this.rumbleBoostLabel.Name = "rumbleBoostLabel";
            this.rumbleBoostLabel.Size = new System.Drawing.Size(34, 13);
            this.rumbleBoostLabel.TabIndex = 22;
            this.rumbleBoostLabel.Text = "Boost";
            // 
            // rightMotorBar
            // 
            this.rightMotorBar.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.rightMotorBar.Location = new System.Drawing.Point(426, 114);
            this.rightMotorBar.Maximum = 255;
            this.rightMotorBar.Name = "rightMotorBar";
            this.rightMotorBar.Size = new System.Drawing.Size(197, 45);
            this.rightMotorBar.TabIndex = 21;
            this.rightMotorBar.TickFrequency = 25;
            this.rightMotorBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.rightMotorBar.ValueChanged += new System.EventHandler(this.rightMotorBar_ValueChanged);
            // 
            // leftMotorBar
            // 
            this.leftMotorBar.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.leftMotorBar.Location = new System.Drawing.Point(426, 82);
            this.leftMotorBar.Maximum = 255;
            this.leftMotorBar.Name = "leftMotorBar";
            this.leftMotorBar.Size = new System.Drawing.Size(197, 45);
            this.leftMotorBar.TabIndex = 20;
            this.leftMotorBar.TickFrequency = 25;
            this.leftMotorBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.leftMotorBar.ValueChanged += new System.EventHandler(this.leftMotorBar_ValueChanged);
            // 
            // rumbleBoostBar
            // 
            this.rumbleBoostBar.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.rumbleBoostBar.Location = new System.Drawing.Point(426, 50);
            this.rumbleBoostBar.Maximum = 200;
            this.rumbleBoostBar.Name = "rumbleBoostBar";
            this.rumbleBoostBar.Size = new System.Drawing.Size(197, 45);
            this.rumbleBoostBar.TabIndex = 19;
            this.rumbleBoostBar.TickFrequency = 25;
            this.rumbleBoostBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.rumbleBoostBar.Value = 100;
            this.rumbleBoostBar.ValueChanged += new System.EventHandler(this.rumbleBoostBar_ValueChanged);
            // 
            // rumbleLabel
            // 
            this.rumbleLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.rumbleLabel.AutoSize = true;
            this.rumbleLabel.Location = new System.Drawing.Point(485, 9);
            this.rumbleLabel.Name = "rumbleLabel";
            this.rumbleLabel.Size = new System.Drawing.Size(77, 13);
            this.rumbleLabel.TabIndex = 28;
            this.rumbleLabel.Text = "Rumble motors";
            // 
            // colorLabel
            // 
            this.colorLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.colorLabel.AutoSize = true;
            this.colorLabel.Location = new System.Drawing.Point(150, 9);
            this.colorLabel.Name = "colorLabel";
            this.colorLabel.Size = new System.Drawing.Size(76, 13);
            this.colorLabel.TabIndex = 29;
            this.colorLabel.Text = "Light Bar Color";
            // 
            // setButton
            // 
            this.setButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.setButton.Location = new System.Drawing.Point(659, 269);
            this.setButton.Name = "setButton";
            this.setButton.Size = new System.Drawing.Size(75, 23);
            this.setButton.TabIndex = 30;
            this.setButton.Text = "Set";
            this.setButton.UseVisualStyleBackColor = true;
            this.setButton.Click += new System.EventHandler(this.setButton_Click);
            // 
            // CustomMappingButton
            // 
            this.CustomMappingButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CustomMappingButton.Location = new System.Drawing.Point(54, 246);
            this.CustomMappingButton.Name = "CustomMappingButton";
            this.CustomMappingButton.Size = new System.Drawing.Size(172, 23);
            this.CustomMappingButton.TabIndex = 31;
            this.CustomMappingButton.Text = "Custom Control Mapping";
            this.CustomMappingButton.UseVisualStyleBackColor = true;
            this.CustomMappingButton.Click += new System.EventHandler(this.CustomMappingButton_Click);
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Location = new System.Drawing.Point(740, 269);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 32;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // batteryLed
            // 
            this.batteryLed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.batteryLed.AutoSize = true;
            this.batteryLed.Location = new System.Drawing.Point(54, 199);
            this.batteryLed.Name = "batteryLed";
            this.batteryLed.Size = new System.Drawing.Size(200, 17);
            this.batteryLed.TabIndex = 33;
            this.batteryLed.Text = "Light Bar Color indicates battery level";
            this.batteryLed.UseVisualStyleBackColor = true;
            this.batteryLed.CheckedChanged += new System.EventHandler(this.ledAsBatteryIndicator_CheckedChanged);
            // 
            // flashLed
            // 
            this.flashLed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.flashLed.AutoSize = true;
            this.flashLed.Location = new System.Drawing.Point(54, 222);
            this.flashLed.Name = "flashLed";
            this.flashLed.Size = new System.Drawing.Size(192, 17);
            this.flashLed.TabIndex = 34;
            this.flashLed.Text = "Flash pattern indicates battery level";
            this.flashLed.UseVisualStyleBackColor = true;
            this.flashLed.CheckedChanged += new System.EventHandler(this.flashWhenLowBattery_CheckedChanged);
            // 
            // touchCheckBox
            // 
            this.touchCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.touchCheckBox.AutoSize = true;
            this.touchCheckBox.Location = new System.Drawing.Point(667, 177);
            this.touchCheckBox.Name = "touchCheckBox";
            this.touchCheckBox.Size = new System.Drawing.Size(148, 17);
            this.touchCheckBox.TabIndex = 35;
            this.touchCheckBox.Text = "Enable Touchpad at Start";
            this.touchCheckBox.UseVisualStyleBackColor = true;
            this.touchCheckBox.CheckedChanged += new System.EventHandler(this.touchAtStartCheckBox_CheckedChanged);
            // 
            // touchSensitivityBar
            // 
            this.touchSensitivityBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.touchSensitivityBar.Location = new System.Drawing.Point(706, 50);
            this.touchSensitivityBar.Maximum = 150;
            this.touchSensitivityBar.Minimum = 10;
            this.touchSensitivityBar.Name = "touchSensitivityBar";
            this.touchSensitivityBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.touchSensitivityBar.Size = new System.Drawing.Size(45, 104);
            this.touchSensitivityBar.TabIndex = 36;
            this.touchSensitivityBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.touchSensitivityBar.Value = 100;
            this.touchSensitivityBar.ValueChanged += new System.EventHandler(this.touchSensitivityBar_ValueChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(709, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 13);
            this.label1.TabIndex = 37;
            this.label1.Text = "Touchpad Sensitivity";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // sensitivityValLabel
            // 
            this.sensitivityValLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.sensitivityValLabel.Location = new System.Drawing.Point(706, 161);
            this.sensitivityValLabel.Name = "sensitivityValLabel";
            this.sensitivityValLabel.Size = new System.Drawing.Size(25, 13);
            this.sensitivityValLabel.TabIndex = 38;
            this.sensitivityValLabel.Text = "100";
            this.sensitivityValLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lowerRCOffCheckBox
            // 
            this.lowerRCOffCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lowerRCOffCheckBox.AutoSize = true;
            this.lowerRCOffCheckBox.Location = new System.Drawing.Point(667, 200);
            this.lowerRCOffCheckBox.Name = "lowerRCOffCheckBox";
            this.lowerRCOffCheckBox.Size = new System.Drawing.Size(151, 17);
            this.lowerRCOffCheckBox.TabIndex = 39;
            this.lowerRCOffCheckBox.Text = "Turn Off Lower Right Click";
            this.lowerRCOffCheckBox.UseVisualStyleBackColor = true;
            this.lowerRCOffCheckBox.CheckedChanged += new System.EventHandler(this.lowerRCOffCheckBox_CheckedChanged);
            // 
            // lowRedValLabel
            // 
            this.lowRedValLabel.Location = new System.Drawing.Point(36, 44);
            this.lowRedValLabel.Name = "lowRedValLabel";
            this.lowRedValLabel.Size = new System.Drawing.Size(30, 13);
            this.lowRedValLabel.TabIndex = 40;
            this.lowRedValLabel.Text = "0";
            this.lowRedValLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lowGreenValLabel
            // 
            this.lowGreenValLabel.Location = new System.Drawing.Point(36, 76);
            this.lowGreenValLabel.Name = "lowGreenValLabel";
            this.lowGreenValLabel.Size = new System.Drawing.Size(30, 13);
            this.lowGreenValLabel.TabIndex = 41;
            this.lowGreenValLabel.Text = "0";
            this.lowGreenValLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lowBlueValLabel
            // 
            this.lowBlueValLabel.Location = new System.Drawing.Point(36, 108);
            this.lowBlueValLabel.Name = "lowBlueValLabel";
            this.lowBlueValLabel.Size = new System.Drawing.Size(30, 13);
            this.lowBlueValLabel.TabIndex = 42;
            this.lowBlueValLabel.Text = "0";
            this.lowBlueValLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // fullColorLabel
            // 
            this.fullColorLabel.AutoSize = true;
            this.fullColorLabel.Location = new System.Drawing.Point(3, 22);
            this.fullColorLabel.Name = "fullColorLabel";
            this.fullColorLabel.Size = new System.Drawing.Size(23, 13);
            this.fullColorLabel.TabIndex = 43;
            this.fullColorLabel.Text = "Full";
            // 
            // lowColorLabel
            // 
            this.lowColorLabel.AutoSize = true;
            this.lowColorLabel.Location = new System.Drawing.Point(36, 22);
            this.lowColorLabel.Name = "lowColorLabel";
            this.lowColorLabel.Size = new System.Drawing.Size(27, 13);
            this.lowColorLabel.TabIndex = 44;
            this.lowColorLabel.Text = "Low";
            // 
            // lowLedCheckBox
            // 
            this.lowLedCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lowLedCheckBox.AutoSize = true;
            this.lowLedCheckBox.Location = new System.Drawing.Point(193, 144);
            this.lowLedCheckBox.Name = "lowLedCheckBox";
            this.lowLedCheckBox.Size = new System.Drawing.Size(173, 17);
            this.lowLedCheckBox.TabIndex = 45;
            this.lowLedCheckBox.Text = "Set Low-Battery Light Bar Color";
            this.lowLedCheckBox.UseVisualStyleBackColor = true;
            this.lowLedCheckBox.Visible = false;
            this.lowLedCheckBox.CheckedChanged += new System.EventHandler(this.lowBatteryLed_CheckedChanged);
            // 
            // lowLedPanel
            // 
            this.lowLedPanel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.lowLedPanel.Controls.Add(this.lowColorChooserButton);
            this.lowLedPanel.Controls.Add(this.fullColorLabel);
            this.lowLedPanel.Controls.Add(this.lowRedValLabel);
            this.lowLedPanel.Controls.Add(this.lowColorLabel);
            this.lowLedPanel.Controls.Add(this.lowGreenValLabel);
            this.lowLedPanel.Controls.Add(this.lowBlueValLabel);
            this.lowLedPanel.Location = new System.Drawing.Point(303, 9);
            this.lowLedPanel.Name = "lowLedPanel";
            this.lowLedPanel.Size = new System.Drawing.Size(63, 129);
            this.lowLedPanel.TabIndex = 46;
            this.lowLedPanel.Visible = false;
            // 
            // lowColorChooserButton
            // 
            this.lowColorChooserButton.BackColor = System.Drawing.Color.White;
            this.lowColorChooserButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lowColorChooserButton.Location = new System.Drawing.Point(42, 0);
            this.lowColorChooserButton.Name = "lowColorChooserButton";
            this.lowColorChooserButton.Size = new System.Drawing.Size(13, 13);
            this.lowColorChooserButton.TabIndex = 49;
            this.lowColorChooserButton.UseVisualStyleBackColor = false;
            this.lowColorChooserButton.Click += new System.EventHandler(this.lowColorChooserButton_Click);
            // 
            // fullLedPanel
            // 
            this.fullLedPanel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.fullLedPanel.Controls.Add(this.redValLabel);
            this.fullLedPanel.Controls.Add(this.blueValLabel);
            this.fullLedPanel.Controls.Add(this.greenValLabel);
            this.fullLedPanel.Location = new System.Drawing.Point(302, 51);
            this.fullLedPanel.Name = "fullLedPanel";
            this.fullLedPanel.Size = new System.Drawing.Size(28, 83);
            this.fullLedPanel.TabIndex = 47;
            // 
            // colorChooserButton
            // 
            this.colorChooserButton.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.colorChooserButton.BackColor = System.Drawing.Color.White;
            this.colorChooserButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.colorChooserButton.Location = new System.Drawing.Point(311, 9);
            this.colorChooserButton.Name = "colorChooserButton";
            this.colorChooserButton.Size = new System.Drawing.Size(13, 13);
            this.colorChooserButton.TabIndex = 48;
            this.colorChooserButton.UseVisualStyleBackColor = false;
            this.colorChooserButton.Click += new System.EventHandler(this.colorChooserButton_Click);
            // 
            // tapSensitivityValLabel
            // 
            this.tapSensitivityValLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tapSensitivityValLabel.Location = new System.Drawing.Point(749, 161);
            this.tapSensitivityValLabel.Name = "tapSensitivityValLabel";
            this.tapSensitivityValLabel.Size = new System.Drawing.Size(25, 13);
            this.tapSensitivityValLabel.TabIndex = 50;
            this.tapSensitivityValLabel.Text = "Off";
            this.tapSensitivityValLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tapSensitivityBar
            // 
            this.tapSensitivityBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tapSensitivityBar.Location = new System.Drawing.Point(749, 50);
            this.tapSensitivityBar.Maximum = 150;
            this.tapSensitivityBar.Name = "tapSensitivityBar";
            this.tapSensitivityBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tapSensitivityBar.Size = new System.Drawing.Size(45, 104);
            this.tapSensitivityBar.TabIndex = 49;
            this.tapSensitivityBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tapSensitivityBar.ValueChanged += new System.EventHandler(this.tapSensitivityBar_ValueChanged);
            // 
            // scrollSensitivityValLabel
            // 
            this.scrollSensitivityValLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.scrollSensitivityValLabel.Location = new System.Drawing.Point(792, 161);
            this.scrollSensitivityValLabel.Name = "scrollSensitivityValLabel";
            this.scrollSensitivityValLabel.Size = new System.Drawing.Size(25, 13);
            this.scrollSensitivityValLabel.TabIndex = 52;
            this.scrollSensitivityValLabel.Text = "Off";
            this.scrollSensitivityValLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // scrollSensitivityBar
            // 
            this.scrollSensitivityBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.scrollSensitivityBar.Location = new System.Drawing.Point(792, 50);
            this.scrollSensitivityBar.Name = "scrollSensitivityBar";
            this.scrollSensitivityBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.scrollSensitivityBar.Size = new System.Drawing.Size(45, 104);
            this.scrollSensitivityBar.TabIndex = 51;
            this.scrollSensitivityBar.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.scrollSensitivityBar.ValueChanged += new System.EventHandler(this.scrollSensitivityBar_ValueChanged);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.Location = new System.Drawing.Point(737, 34);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 53;
            this.label4.Text = "Tap";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Location = new System.Drawing.Point(694, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 13);
            this.label5.TabIndex = 54;
            this.label5.Text = "Touch Sensitivity";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.Location = new System.Drawing.Point(780, 34);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 13);
            this.label6.TabIndex = 55;
            this.label6.Text = "Scroll";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // realTimeChangesCheckBox
            // 
            this.realTimeChangesCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.realTimeChangesCheckBox.AutoSize = true;
            this.realTimeChangesCheckBox.Location = new System.Drawing.Point(700, 246);
            this.realTimeChangesCheckBox.Name = "realTimeChangesCheckBox";
            this.realTimeChangesCheckBox.Size = new System.Drawing.Size(115, 17);
            this.realTimeChangesCheckBox.TabIndex = 56;
            this.realTimeChangesCheckBox.Text = "Real-time Changes";
            this.realTimeChangesCheckBox.UseVisualStyleBackColor = true;
            this.realTimeChangesCheckBox.CheckedChanged += new System.EventHandler(this.realTimeChangesCheckBox_CheckedChanged);
            // 
            // pictureBox
            // 
            this.pictureBox.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.pictureBox.BackColor = System.Drawing.Color.Aqua;
            this.pictureBox.Image = global::ScpServer.Properties.Resources._1;
            this.pictureBox.Location = new System.Drawing.Point(270, 222);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(140, 70);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox.TabIndex = 73;
            this.pictureBox.TabStop = false;
            this.pictureBox.Click += new System.EventHandler(this.pictureBox_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(559, 177);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(21, 13);
            this.label11.TabIndex = 78;
            this.label11.Text = "R2";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(485, 177);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(19, 13);
            this.label10.TabIndex = 77;
            this.label10.Text = "L2";
            // 
            // rightTriggerMiddlePoint
            // 
            this.rightTriggerMiddlePoint.Location = new System.Drawing.Point(555, 192);
            this.rightTriggerMiddlePoint.Margin = new System.Windows.Forms.Padding(2);
            this.rightTriggerMiddlePoint.MaxLength = 20;
            this.rightTriggerMiddlePoint.Name = "rightTriggerMiddlePoint";
            this.rightTriggerMiddlePoint.Size = new System.Drawing.Size(52, 20);
            this.rightTriggerMiddlePoint.TabIndex = 76;
            this.rightTriggerMiddlePoint.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // leftTriggerMiddlePoint
            // 
            this.leftTriggerMiddlePoint.Location = new System.Drawing.Point(479, 192);
            this.leftTriggerMiddlePoint.Margin = new System.Windows.Forms.Padding(2);
            this.leftTriggerMiddlePoint.MaxLength = 20;
            this.leftTriggerMiddlePoint.Name = "leftTriggerMiddlePoint";
            this.leftTriggerMiddlePoint.Size = new System.Drawing.Size(52, 20);
            this.leftTriggerMiddlePoint.TabIndex = 75;
            this.leftTriggerMiddlePoint.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(470, 148);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(180, 26);
            this.label9.TabIndex = 74;
            this.label9.Text = "Trigger middle-point adjustment:\r\n(Arthritis accessibility; standard = 0.5)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(470, 226);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 13);
            this.label2.TabIndex = 79;
            this.label2.Text = "Sixaxis gyro/accel data:";
            // 
            // flushHIDQueue
            // 
            this.flushHIDQueue.AccessibleName = "flushHIDQueue";
            this.flushHIDQueue.AutoSize = true;
            this.flushHIDQueue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.flushHIDQueue.Location = new System.Drawing.Point(43, 279);
            this.flushHIDQueue.Name = "flushHIDQueue";
            this.flushHIDQueue.Size = new System.Drawing.Size(220, 19);
            this.flushHIDQueue.TabIndex = 80;
            this.flushHIDQueue.Text = "Flush HID queue after each reading";
            this.flushHIDQueue.UseVisualStyleBackColor = true;
            this.flushHIDQueue.CheckedChanged += new System.EventHandler(this.flushHIDQueue_CheckedChanged);
            // 
            // touchpadJitterCompensation
            // 
            this.touchpadJitterCompensation.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.touchpadJitterCompensation.AutoSize = true;
            this.touchpadJitterCompensation.Location = new System.Drawing.Point(667, 223);
            this.touchpadJitterCompensation.Name = "touchpadJitterCompensation";
            this.touchpadJitterCompensation.Size = new System.Drawing.Size(153, 17);
            this.touchpadJitterCompensation.TabIndex = 81;
            this.touchpadJitterCompensation.Text = "Perform jitter compensation";
            this.touchpadJitterCompensation.UseVisualStyleBackColor = true;
            this.touchpadJitterCompensation.CheckedChanged += new System.EventHandler(this.touchpadJitterCompensation_CheckedChanged);
            // 
            // advColorDialog
            // 
            this.advColorDialog.AnyColor = true;
            this.advColorDialog.Color = System.Drawing.Color.Blue;
            this.advColorDialog.FullOpen = true;
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.ClientSize = new System.Drawing.Size(827, 310);
            this.Controls.Add(this.touchpadJitterCompensation);
            this.Controls.Add(this.flushHIDQueue);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.rightTriggerMiddlePoint);
            this.Controls.Add(this.leftTriggerMiddlePoint);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.pictureBox);
            this.Controls.Add(this.realTimeChangesCheckBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.scrollSensitivityValLabel);
            this.Controls.Add(this.scrollSensitivityBar);
            this.Controls.Add(this.tapSensitivityValLabel);
            this.Controls.Add(this.tapSensitivityBar);
            this.Controls.Add(this.colorChooserButton);
            this.Controls.Add(this.fullLedPanel);
            this.Controls.Add(this.lowLedCheckBox);
            this.Controls.Add(this.lowerRCOffCheckBox);
            this.Controls.Add(this.sensitivityValLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.touchSensitivityBar);
            this.Controls.Add(this.touchCheckBox);
            this.Controls.Add(this.flashLed);
            this.Controls.Add(this.batteryLed);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.CustomMappingButton);
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
            this.Controls.Add(this.BlueLabel);
            this.Controls.Add(this.GreenLabel);
            this.Controls.Add(this.RedLabel);
            this.Controls.Add(this.blueBar);
            this.Controls.Add(this.greenBar);
            this.Controls.Add(this.redBar);
            this.Controls.Add(this.lowLedPanel);
            this.MaximumSize = new System.Drawing.Size(886, 359);
            this.MinimumSize = new System.Drawing.Size(797, 303);
            this.Name = "Options";
            this.Text = "Options";
            ((System.ComponentModel.ISupportInitialize)(this.blueBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.greenBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.redBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rightMotorBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.leftMotorBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rumbleBoostBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.touchSensitivityBar)).EndInit();
            this.lowLedPanel.ResumeLayout(false);
            this.lowLedPanel.PerformLayout();
            this.fullLedPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.tapSensitivityBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.scrollSensitivityBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
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
        private System.Windows.Forms.Button CustomMappingButton;
        private System.Windows.Forms.TrackBar redBar;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.CheckBox batteryLed;
        private System.Windows.Forms.CheckBox flashLed;
        private AdvancedColorDialog advColorDialog;
        private System.Windows.Forms.CheckBox touchCheckBox;
        private System.Windows.Forms.TrackBar touchSensitivityBar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label sensitivityValLabel;
        private System.Windows.Forms.CheckBox lowerRCOffCheckBox;
        private System.Windows.Forms.Label lowRedValLabel;
        private System.Windows.Forms.Label lowGreenValLabel;
        private System.Windows.Forms.Label lowBlueValLabel;
        private System.Windows.Forms.Label fullColorLabel;
        private System.Windows.Forms.Label lowColorLabel;
        private System.Windows.Forms.CheckBox lowLedCheckBox;
        private System.Windows.Forms.Panel lowLedPanel;
        private System.Windows.Forms.Panel fullLedPanel;
        private System.Windows.Forms.Button colorChooserButton;
        private System.Windows.Forms.Button lowColorChooserButton;
        private System.Windows.Forms.Label tapSensitivityValLabel;
        private System.Windows.Forms.TrackBar tapSensitivityBar;
        private System.Windows.Forms.Label scrollSensitivityValLabel;
        private System.Windows.Forms.TrackBar scrollSensitivityBar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox realTimeChangesCheckBox;
        //private System.Windows.Forms.TrackBar tBsixaxisGyroX;
        //private System.Windows.Forms.TrackBar tBsixaxisGyroY;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox rightTriggerMiddlePoint;
        private System.Windows.Forms.TextBox leftTriggerMiddlePoint;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox flushHIDQueue;
        private System.Windows.Forms.CheckBox touchpadJitterCompensation;
    }
}
