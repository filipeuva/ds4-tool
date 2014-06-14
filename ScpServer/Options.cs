﻿using ScpControl;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ScpServer
{
    public partial class Options : Form
    {
        private ScpControl.BusDevice scpDevice;
        private int device;

        Byte[] oldLedColor, oldLowLedColor;
        TrackBar tBsixaxisGyroX, tBsixaxisGyroY, tBsixaxisGyroZ,
            tBsixaxisAccelX, tBsixaxisAccelY, tBsixaxisAccelZ;

        public Options(BusDevice bus_device, int deviceNum)
        {
            InitializeComponent();
            device = deviceNum;
            scpDevice = bus_device;
            ledColor color = Global.loadColor(device);
            redBar.Value = color.red;
            greenBar.Value = color.green;
            blueBar.Value = color.blue;
            rumbleBoostBar.Value = ScpControl.Global.loadRumbleBoost(device);
            batteryLed.Checked = ScpControl.Global.getLedAsBatteryIndicator(device);
            flashLed.Checked = ScpControl.Global.getFlashWhenLowBattery(device);
            touchCheckBox.Checked = Global.getTouchEnabled(device);
            touchSensitivityBar.Value = Global.getTouchSensitivity(device);
            leftTriggerMiddlePoint.Text = Global.getLeftTriggerMiddle(device).ToString();
            rightTriggerMiddlePoint.Text = Global.getRightTriggerMiddle(device).ToString();
            ledColor lowColor = Global.loadLowColor(device);
            touchpadJitterCompensation.Checked = Global.getTouchpadJitterCompensation(device);
            lowerRCOffCheckBox.Checked = Global.getLowerRCOff(device);
            tapSensitivityBar.Value = Global.getTapSensitivity(device);
            scrollSensitivityBar.Value = Global.getScrollSensitivity(device);
            flushHIDQueue.Checked = Global.getFlushHIDQueue(device);
            advColorDialog.OnUpdateColor += advColorDialog_OnUpdateColor;

            // Force update of color choosers
            colorChooserButton.BackColor = Color.FromArgb(color.red, color.green, color.blue);
            lowColorChooserButton.BackColor = Color.FromArgb(lowColor.red, lowColor.green, lowColor.blue);
            pictureBox.BackColor = colorChooserButton.BackColor;
            lowRedValLabel.Text = lowColor.red.ToString();
            lowGreenValLabel.Text = lowColor.green.ToString();
            lowBlueValLabel.Text = lowColor.blue.ToString();

            turnToUserCheckBox.Checked = Global.getTurnToUser(device);
            turnOffMinuteBar.Value = Global.getTurnOffTime(device);
            turnOffMinuteBar_ValueChanged(null, null);

            #region watch sixaxis data
            Timer sixaxisTimer = new Timer();
            sixaxisTimer.Tick +=
            (delegate
                {
                    if (tBsixaxisGyroX == null)
                    {
                        tBsixaxisGyroX = new TrackBar();
                        tBsixaxisGyroY = new TrackBar();
                        tBsixaxisGyroZ = new TrackBar();
                        tBsixaxisAccelX = new TrackBar();
                        tBsixaxisAccelY = new TrackBar();
                        tBsixaxisAccelZ = new TrackBar();
                        TrackBar[] allSixAxes = { tBsixaxisGyroX, tBsixaxisGyroY, tBsixaxisGyroZ,
                                                tBsixaxisAccelX, tBsixaxisAccelY, tBsixaxisAccelZ};
                        foreach (TrackBar t in allSixAxes)
                        {
                            ((System.ComponentModel.ISupportInitialize)(t)).BeginInit();
                            t.Anchor = AnchorStyles.Bottom;
                            t.AutoSize = false;
                            t.Enabled = false;
                            t.Minimum = -0x8000;
                            t.Maximum = 0x7fff;
                            t.Size = new Size(100, 19);
                            t.TickFrequency = 0x2000; // calibrated to ~1G
                        }
                        // tBsixaxisGyroX
                        tBsixaxisGyroX.Location = new Point(450, 248);
                        tBsixaxisGyroX.Name = "tBsixaxisGyroX";
                        // tBsixaxisGyroY
                        tBsixaxisGyroY.Location = new Point(450, 248 + 20);
                        tBsixaxisGyroY.Name = "tBsixaxisGyroY";
                        // tBsixaxisGyroZ
                        tBsixaxisGyroZ.Location = new Point(450, 248 + 20 + 20);
                        tBsixaxisGyroZ.Name = "tBsixaxisGyroZ";
                        // tBsixaxisAccelX
                        tBsixaxisAccelX.Location = new Point(450 + 100 + 10, 248);
                        tBsixaxisAccelX.Name = "tBsixaxisAccelX";
                        // tBsixaxisAccelY
                        tBsixaxisAccelY.Location = new Point(450 + 100 + 10, 248 + 20);
                        tBsixaxisAccelY.Name = "tBsixaxisAccelY";
                        // tBsixaxisAccelZ
                        tBsixaxisAccelZ.Location = new Point(450 + 100 + 10, 248 + 20 + 20);
                        tBsixaxisAccelZ.Name = "tBsixaxisAccelZ";
                        foreach (TrackBar t in allSixAxes)
                        {
                            Controls.Add(t);
                            ((System.ComponentModel.ISupportInitialize)(t)).EndInit();
                        }
                    }
                    byte[] inputData = scpDevice.GetInputData(device);
                    if (inputData != null)
                    {
                        // MEMS gyro data is all calibrated to roughly -1G..1G for values -0x2000..0x1fff
                        // Enough additional acceleration and we are no longer mostly measuring Earth's gravity...
                        // We should try to indicate setpoints of the calibration when exposing this measurement....

                        // R side of controller upward
                        Int16 x = (Int16)((UInt16)(inputData[20] << 8) | inputData[21]);
                        tBsixaxisGyroX.Value = (x + tBsixaxisGyroX.Value * 2) / 3;
                        // touchpad and button face side of controller upward
                        Int16 y = (Int16)((UInt16)(inputData[22] << 8) | inputData[23]);
                        tBsixaxisGyroY.Value = (y + tBsixaxisGyroY.Value * 2) / 3;
                        // audio/expansion ports upward and light bar/shoulders/bumpers/USB port downward
                        Int16 z = (Int16)((UInt16)(inputData[24] << 8) | inputData[25]);
                        tBsixaxisGyroZ.Value = (z + tBsixaxisGyroZ.Value * 2) / 3;
                        // pitch upward/backward
                        Int16 pitch = (Int16)((UInt16)(inputData[14] << 8) | inputData[15]);
                        tBsixaxisAccelX.Value = (pitch + tBsixaxisAccelX.Value * 2) / 3; // smooth out
                        // yaw leftward/counter-clockwise/turn to port or larboard side
                        Int16 yaw = (Int16)((UInt16)(inputData[16] << 8) | inputData[17]);
                        tBsixaxisAccelY.Value = (yaw + tBsixaxisAccelY.Value * 2) / 3;
                        // roll left/L side of controller down/starboard raising up
                        Int16 roll = (Int16)((UInt16)(inputData[18] << 8) | inputData[19]);
                        tBsixaxisAccelZ.Value = (roll + tBsixaxisAccelZ.Value * 2) / 3;

                    }
                });
            sixaxisTimer.Interval = 1000 / 60;
            this.FormClosing += delegate { sixaxisTimer.Stop(); };
            sixaxisTimer.Start();
            #endregion
        }

        private void CustomMappingButton_Click(object sender, EventArgs e)
        {
            // open a custom mapping form
            CustomMapping cmForm = new CustomMapping(device);
            cmForm.Icon = this.Icon;
            cmForm.Show();
        }

        private void setButton_Click(object sender, EventArgs e)
        {
            Global.saveColor(device, 
                colorChooserButton.BackColor.R, 
                colorChooserButton.BackColor.G, 
                colorChooserButton.BackColor.B);
            Global.saveLowColor(device,
                lowColorChooserButton.BackColor.R, 
                lowColorChooserButton.BackColor.G, 
                lowColorChooserButton.BackColor.B);
            double middle;
            if (Double.TryParse(leftTriggerMiddlePoint.Text, out middle))
                Global.setLeftTriggerMiddle(device, middle);
            if (Double.TryParse(rightTriggerMiddlePoint.Text, out middle))
                Global.setRightTriggerMiddle(device, middle);
            scpDevice.setRumble(device, (byte)rumbleBoostBar.Value, (byte)leftMotorBar.Value, (byte)rightMotorBar.Value);
            Global.setTouchSensitivity(device, (byte)touchSensitivityBar.Value);
            Global.setTouchpadJitterCompensation(device, touchpadJitterCompensation.Checked);
            Global.setLowerRCOff(device, lowerRCOffCheckBox.Checked);
            Global.setTapSensitivity(device, (byte)tapSensitivityBar.Value);
            Global.setScrollSensitivity(device, (byte)scrollSensitivityBar.Value);
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            setButton_Click(null, null);
            Global.Save();
            this.Close();
        }

        private void redBar_ValueChanged(object sender, EventArgs e)
        {
            // New settings
            if (lowLedCheckBox.Checked)
            {
                lowRedValLabel.Text = redBar.Value.ToString();
                lowColorChooserButton.BackColor = Color.FromArgb(
                    redBar.Value,
                    lowColorChooserButton.BackColor.G,
                    lowColorChooserButton.BackColor.B);
                pictureBox.BackColor = Color.FromArgb(
                    redBar.Value,
                    lowColorChooserButton.BackColor.G,
                    lowColorChooserButton.BackColor.B);
                if (realTimeChangesCheckBox.Checked)
                    Global.saveLowColor(device, (byte)redBar.Value,
                        lowColorChooserButton.BackColor.G, 
                        lowColorChooserButton.BackColor.B);
            }
            else
            {
                colorChooserButton.BackColor = Color.FromArgb(
                    redBar.Value,
                    colorChooserButton.BackColor.G,
                    colorChooserButton.BackColor.B);
                pictureBox.BackColor = Color.FromArgb(
                    redBar.Value,
                    colorChooserButton.BackColor.G,
                    colorChooserButton.BackColor.B);
                if (realTimeChangesCheckBox.Checked)
                    Global.saveColor(device, (byte)redBar.Value,
                        colorChooserButton.BackColor.G,
                        colorChooserButton.BackColor.B);

                // Previous implementation
                redValLabel.Text = redBar.Value.ToString();
            }
        }
        private void greenBar_ValueChanged(object sender, EventArgs e)
        {
            // New settings
            if (lowLedCheckBox.Checked)
            {
                lowGreenValLabel.Text = greenBar.Value.ToString();
                lowColorChooserButton.BackColor = Color.FromArgb(
                    lowColorChooserButton.BackColor.R,
                    greenBar.Value,
                    lowColorChooserButton.BackColor.B);
                pictureBox.BackColor = Color.FromArgb(
                    lowColorChooserButton.BackColor.R,
                    greenBar.Value,
                    lowColorChooserButton.BackColor.B); 
                if (realTimeChangesCheckBox.Checked)
                    Global.saveLowColor(device,
                        lowColorChooserButton.BackColor.R,
                        (byte)greenBar.Value,
                        lowColorChooserButton.BackColor.B);
            }
            else
            {
                colorChooserButton.BackColor = Color.FromArgb(
                    colorChooserButton.BackColor.R,
                    greenBar.Value,
                    colorChooserButton.BackColor.B);
                pictureBox.BackColor = Color.FromArgb(
                    colorChooserButton.BackColor.R,
                    greenBar.Value,
                    colorChooserButton.BackColor.B);
                if (realTimeChangesCheckBox.Checked)
                    Global.saveColor(device,
                        colorChooserButton.BackColor.R,
                        (byte)greenBar.Value,
                        colorChooserButton.BackColor.B);

                // Previous implementation
                greenValLabel.Text = greenBar.Value.ToString();
            }
        }
        private void blueBar_ValueChanged(object sender, EventArgs e)
        {
            // New settings
            if (lowLedCheckBox.Checked)
            {
                lowBlueValLabel.Text = blueBar.Value.ToString();
                lowColorChooserButton.BackColor = Color.FromArgb(
                    lowColorChooserButton.BackColor.R,
                    lowColorChooserButton.BackColor.G,
                    blueBar.Value);
                pictureBox.BackColor = Color.FromArgb(
                    lowColorChooserButton.BackColor.R,
                    lowColorChooserButton.BackColor.G,
                    blueBar.Value);
                if (realTimeChangesCheckBox.Checked)
                    Global.saveLowColor(device,
                        lowColorChooserButton.BackColor.R,
                        lowColorChooserButton.BackColor.G,
                        (byte)blueBar.Value);
            }
            else
            {
                colorChooserButton.BackColor = Color.FromArgb(
                    colorChooserButton.BackColor.R,
                    colorChooserButton.BackColor.G,
                    blueBar.Value);
                pictureBox.BackColor = Color.FromArgb(
                    colorChooserButton.BackColor.R,
                    colorChooserButton.BackColor.G,
                    blueBar.Value);
                if (realTimeChangesCheckBox.Checked)
                    Global.saveColor(device,
                        colorChooserButton.BackColor.R,
                        colorChooserButton.BackColor.G,
                        (byte)blueBar.Value);

                // Previous implementation
                blueValLabel.Text = blueBar.Value.ToString();
            }
        }

        private void rumbleBoostBar_ValueChanged(object sender, EventArgs e)
        {
            rumbleBoostMotorValLabel.Text = rumbleBoostBar.Value.ToString();

            if (realTimeChangesCheckBox.Checked)
            {
                Global.saveRumbleBoost(device, (byte)rumbleBoostBar.Value);
                scpDevice.setRumble(device, (byte)rumbleBoostBar.Value, (byte)leftMotorBar.Value, (byte)rightMotorBar.Value);
            }
        }
        private void leftMotorBar_ValueChanged(object sender, EventArgs e)
        {
            leftMotorValLabel.Text = leftMotorBar.Value.ToString();

            if (realTimeChangesCheckBox.Checked)
                scpDevice.setRumble(device, (byte)rumbleBoostBar.Value, (byte)leftMotorBar.Value, (byte)rightMotorBar.Value);
        }
        private void rightMotorBar_ValueChanged(object sender, EventArgs e)
        {
            rightMotorValLabel.Text = rightMotorBar.Value.ToString();

            if (realTimeChangesCheckBox.Checked)
                scpDevice.setRumble(device, (byte)rumbleBoostBar.Value, (byte)leftMotorBar.Value, (byte)rightMotorBar.Value);
        }

        private void touchSensitivityBar_ValueChanged(object sender, EventArgs e)
        {
            sensitivityValLabel.Text = touchSensitivityBar.Value.ToString();

            if (realTimeChangesCheckBox.Checked)
                Global.setTouchSensitivity(device, (byte)touchSensitivityBar.Value);
        }
        private void tapSensitivityBar_ValueChanged(object sender, EventArgs e)
        {
            tapSensitivityValLabel.Text = tapSensitivityBar.Value.ToString();
            if (tapSensitivityValLabel.Text == "0")
                tapSensitivityValLabel.Text = "Off";

            if (realTimeChangesCheckBox.Checked)
                Global.setTapSensitivity(device, (byte)tapSensitivityBar.Value);
        }
        private void scrollSensitivityBar_ValueChanged(object sender, EventArgs e)
        {
            scrollSensitivityValLabel.Text = scrollSensitivityBar.Value.ToString();
            if (scrollSensitivityValLabel.Text == "0")
                scrollSensitivityValLabel.Text = "Off";

            if (realTimeChangesCheckBox.Checked)
                Global.setScrollSensitivity(device, (byte)scrollSensitivityBar.Value);
        }

        private void lowBatteryLed_CheckedChanged(object sender, EventArgs e)
        {
            if (lowLedCheckBox.Checked)
            {
                fullLedPanel.Enabled = false;
                redBar.Value = int.Parse(lowRedValLabel.Text);
                greenBar.Value = int.Parse(lowGreenValLabel.Text);
                blueBar.Value = int.Parse(lowBlueValLabel.Text);
                pictureBox.BackColor = lowColorChooserButton.BackColor;
                if (realTimeChangesCheckBox.Checked)
                    Global.saveLowColor(device,
                        lowColorChooserButton.BackColor.R,
                        lowColorChooserButton.BackColor.G,
                        lowColorChooserButton.BackColor.B);
            }
            else
            {
                fullLedPanel.Enabled = true;
                redBar.Value = int.Parse(redValLabel.Text);
                greenBar.Value = int.Parse(greenValLabel.Text);
                blueBar.Value = int.Parse(blueValLabel.Text);
                pictureBox.BackColor = colorChooserButton.BackColor;
                if (realTimeChangesCheckBox.Checked)
                    Global.saveColor(device,
                        colorChooserButton.BackColor.R,
                        colorChooserButton.BackColor.G,
                        colorChooserButton.BackColor.B);
            }
        }
        private void ledAsBatteryIndicator_CheckedChanged(object sender, EventArgs e)
        {
            Global.setLedAsBatteryIndicator(device, batteryLed.Checked);

            // New settings
            if (batteryLed.Checked)
            {
                lowLedPanel.Visible = true;
                lowLedCheckBox.Visible = true;
                if (realTimeChangesCheckBox.Checked)
                    Global.setLedAsBatteryIndicator(device, true);
            }
            else 
            {
                lowLedPanel.Visible = false;
                lowLedCheckBox.Visible = false;
                if (realTimeChangesCheckBox.Checked)
                    Global.setLedAsBatteryIndicator(device, false);
            }
        }
        private void flashWhenLowBattery_CheckedChanged(object sender, EventArgs e)
        {
            Global.setFlashWhenLowBattery(device, flashLed.Checked);
        }
        private void touchAtStartCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Global.setTouchEnabled(device,touchCheckBox.Checked);
        }
        private void lowerRCOffCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (realTimeChangesCheckBox.Checked)
                Global.setLowerRCOff(device, lowerRCOffCheckBox.Checked);
        }

        private void touchpadJitterCompensation_CheckedChanged(object sender, EventArgs e)
        {
            if (realTimeChangesCheckBox.Checked)
                Global.setTouchpadJitterCompensation(device, touchpadJitterCompensation.Checked);
        }
        private void realTimeChangesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (realTimeChangesCheckBox.Checked)
            {
                setButton.Visible = false;
            }
            else
            {
                setButton.Visible = true;
            }
        }
        
        private void pictureBox_Click(object sender, EventArgs e)
        {
            if (lowLedCheckBox.Checked)
                lowColorChooserButton_Click(sender, e);
            else colorChooserButton_Click(sender, e);
        }
        private void colorChooserButton_Click(object sender, EventArgs e)
        {
            advColorDialog.Color = colorChooserButton.BackColor;
            advColorDialog_OnUpdateColor(colorChooserButton.BackColor, e);
            if (advColorDialog.ShowDialog() == DialogResult.OK)
            {
                redValLabel.Text = advColorDialog.Color.R.ToString();
                greenValLabel.Text = advColorDialog.Color.G.ToString();
                blueValLabel.Text = advColorDialog.Color.B.ToString();
                colorChooserButton.BackColor = advColorDialog.Color;
                pictureBox.BackColor = advColorDialog.Color;
                if (!lowLedCheckBox.Checked)
                {
                    redBar.Value = advColorDialog.Color.R;
                    greenBar.Value = advColorDialog.Color.G;
                    blueBar.Value = advColorDialog.Color.B;
                }
            }
            else Global.saveColor(device, oldLedColor[0], oldLedColor[1], oldLedColor[2]);
            Global.saveLowColor(device, oldLowLedColor[0], oldLowLedColor[1], oldLowLedColor[2]);
            oldLedColor = null;
            oldLowLedColor = null;
        }
        private void lowColorChooserButton_Click(object sender, EventArgs e)
        {
            advColorDialog.Color = lowColorChooserButton.BackColor;
            advColorDialog_OnUpdateColor(lowColorChooserButton.BackColor, e);
            if (advColorDialog.ShowDialog() == DialogResult.OK)
            {
                lowRedValLabel.Text = advColorDialog.Color.R.ToString();
                lowGreenValLabel.Text = advColorDialog.Color.G.ToString();
                lowBlueValLabel.Text = advColorDialog.Color.B.ToString();
                lowColorChooserButton.BackColor = advColorDialog.Color;
                pictureBox.BackColor = advColorDialog.Color;
                if (lowLedCheckBox.Checked)
                {
                    redBar.Value = advColorDialog.Color.R;
                    greenBar.Value = advColorDialog.Color.G;
                    blueBar.Value = advColorDialog.Color.B;
                }
            }
            else Global.saveLowColor(device, oldLowLedColor[0], oldLowLedColor[1], oldLowLedColor[2]);
            Global.saveColor(device, oldLedColor[0], oldLedColor[1], oldLedColor[2]);
            oldLedColor = null;
            oldLowLedColor = null;
        }
        private void advColorDialog_OnUpdateColor(object sender, EventArgs e)
        {
            if (oldLedColor == null || oldLowLedColor == null)
            {
                ledColor color = Global.loadColor(device);
                oldLedColor = new Byte[] { color.red, color.green, color.blue };
                color = Global.loadLowColor(device);
                oldLowLedColor = new Byte[] { color.red, color.green, color.blue };
            }
            if (sender is Color)
            {
                Color color = (Color)sender;
                Global.saveColor(device, color.R, color.G, color.B);
                Global.saveLowColor(device, color.R, color.G, color.B);
            }
        }

        private void flushHIDQueue_CheckedChanged(object sender, EventArgs e)
        {
            Global.setFlushHIDQueue(device, flushHIDQueue.Checked);
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void blueBar_Scroll(object sender, EventArgs e)
        {

        }

        private void greenBar_Scroll(object sender, EventArgs e)
        {

        }

        private void GreenLabel_Click(object sender, EventArgs e)
        {

        }

        private void redBar_Scroll(object sender, EventArgs e)
        {

        }


        private void turnOffMinuteBar_ValueChanged(object sender, EventArgs e)
        {

            int value = turnOffMinuteBar.Value;
            String caption = turnOffMinuteBar.Value > 0 ? (value > 60 ? value / 60 + " hours and " + value % 60 + " minutes" : value + " minutes") : "Off";

            turnOffLabel.Text = "Turn inactive Controller off after : " + caption ;

            Global.saveTurnOffTime(device, turnOffMinuteBar.Value);
        }

        private void turnToUserCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Global.saveTurnToUser(device, turnToUserCheckBox.Checked);
        }

    }

    
}
