using ScpControl;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ScpServer
{
    public partial class Options : Form
    #region watch sixaxis data
        , ScpControl.Global.DisposableSixaxisObserver
    #endregion
    {
        private ScpControl.BusDevice scpDevice;
        private int device;

        Byte[] oldLedColor, oldLowLedColor;

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

            // New settings
            ledColor lowColor = Global.loadLowColor(device);
            twoFingerRCCheckBox.Checked = Global.getTwoFingerRC(device);
            tapSensitivityBar.Value = Global.getTapSensitivity(device);
            scrollSensitivityBar.Value = Global.getScrollSensitivity(device);
            advColorDialog.OnUpdateColor += advColorDialog_OnUpdateColor;

            // Force update of color choosers
            colorChooserButton.BackColor = Color.FromArgb(color.red, color.green, color.blue);
            lowColorChooserButton.BackColor = Color.FromArgb(lowColor.red, lowColor.green, lowColor.blue);
            pictureBox.BackColor = colorChooserButton.BackColor;
            lowRedValLabel.Text = lowColor.red.ToString();
            lowGreenValLabel.Text = lowColor.green.ToString();
            lowBlueValLabel.Text = lowColor.blue.ToString();

            #region watch sixaxis data
            Global.registerForSixaxisData(this);
            InvokeUpdateDelegate += InvokeUpdate;
            InvokeCloseDelegate += this.Close;
            #endregion
        }
        #region watch sixaxis data
        public delegate void UpdateInvoker(byte[] data);
        public delegate void CloseInvoker();
        public UpdateInvoker InvokeUpdateDelegate;
        public CloseInvoker InvokeCloseDelegate;
        public void Update(byte[] data)
        {
            if (this.IsHandleCreated)
            {
                this.BeginInvoke(InvokeUpdateDelegate, data);
            }
        }
        public void InvokeUpdate(byte[] data)
        {
            int x = data[0];
            if (x > 150)
                x -= 254;
            x += 32;
            if (x < 0)
                x = 0;
            else if (x > 64)
                x = 64;
            tBsixaxisX.Value = x;
            x = data[1];
            if (x > 150)
                x -= 254;
            x += 32;
            if (x < 0)
                x = 0;
            else if (x > 64)
                x = 64;
            tBsixaxisY.Value = x;
        }
        public void InvokeClose()
        {
            this.Invoke(InvokeCloseDelegate);
        }
        private void Options_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Global.unregisterForSixaxisData(this))
                e.Cancel = true;
        }
        #endregion

        private void CustomMappingButton_Click(object sender, EventArgs e)
        {
            // open a custom mapping form
            CustomMapping cmForm = new CustomMapping(device);
            cmForm.Icon = this.Icon;
            cmForm.Show();
        }

        private void setButton_Click(object sender, EventArgs e)
        {
            // New implementation
            Global.saveColor(device, 
                colorChooserButton.BackColor.R, 
                colorChooserButton.BackColor.G, 
                colorChooserButton.BackColor.B);
            // New setting
            Global.saveLowColor(device,
                lowColorChooserButton.BackColor.R, 
                lowColorChooserButton.BackColor.G, 
                lowColorChooserButton.BackColor.B);

            scpDevice.setRumble(device, (byte)rumbleBoostBar.Value, (byte)leftMotorBar.Value, (byte)rightMotorBar.Value);
            Global.setTouchSensitivity(device, (byte)touchSensitivityBar.Value);
            // New settings
            Global.setTwoFingerRC(device, twoFingerRCCheckBox.Checked);
            Global.setTapSensitivity(device, (byte)tapSensitivityBar.Value);
            Global.setScrollSensitivity(device, (byte)scrollSensitivityBar.Value);

        }
        private void saveButton_Click(object sender, EventArgs e)
        {
            // New implementation
            Global.saveColor(device,
                colorChooserButton.BackColor.R,
                colorChooserButton.BackColor.G,
                colorChooserButton.BackColor.B);
            // New setting
            Global.saveLowColor(device,
                lowColorChooserButton.BackColor.R,
                lowColorChooserButton.BackColor.G,
                lowColorChooserButton.BackColor.B);

            scpDevice.setRumble(device, (byte)rumbleBoostBar.Value, (byte)leftMotorBar.Value, (byte)rightMotorBar.Value);
            Global.saveRumbleBoost(device, (byte)rumbleBoostBar.Value);
            Global.setTouchSensitivity(device,(byte) touchSensitivityBar.Value);
            // New settings
            Global.setTwoFingerRC(device, twoFingerRCCheckBox.Checked);
            Global.setTapSensitivity(device, (byte)tapSensitivityBar.Value);
            Global.setScrollSensitivity(device, (byte)scrollSensitivityBar.Value);

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
        private void twoFingerRCCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (realTimeChangesCheckBox.Checked)
            if (twoFingerRCCheckBox.Checked)
            {
                Global.setTwoFingerRC(device, true);
            }
            else
            {
                Global.setTwoFingerRC(device, false);
            }
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

    }

    
}
