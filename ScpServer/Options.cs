using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ScpControl;
namespace ScpServer
{
    public partial class Options : Form
    {
        private ScpControl.BusDevice scpDevice;
        private int device;
        public Options(BusDevice bus_device, int deviceNum)
        {
            InitializeComponent();
            device = deviceNum;
            scpDevice = bus_device;
            ledColor color  = Global.loadColor(device);
            redBar.Value = color.red;
            greenBar.Value = color.green;
            blueBar.Value = color.blue;
            rumbleBoostBar.Value = ScpControl.Global.loadRumbleBoost(device);
            batteryLed.Checked = ScpControl.Global.getLedAsBatteryIndicator(device);
            flashLed.Checked = ScpControl.Global.getFlashWhenLowBattery(device);
            touchCheckBox.Checked = Global.getTouchEnabled(device);
            sensitivityBar.Value = Global.getTouchSensitivity(device);
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void setButton_Click(object sender, EventArgs e)
        {
            byte red = (byte)redBar.Value;
            byte green = (byte)greenBar.Value;
            byte blue = (byte)blueBar.Value;
            scpDevice.setRumble(device, (byte)rumbleBoostBar.Value, (byte)leftMotorBar.Value, (byte)rightMotorBar.Value);
            ScpControl.Global.saveColor(device, red, green, blue);
            Global.setTouchSensitivity(device, (byte)sensitivityBar.Value);
        }

        private void redBar_ValueChanged(object sender, EventArgs e)
        {
            redValLabel.Text = redBar.Value.ToString();
            
        }

        private void greenBar_ValueChanged(object sender, EventArgs e)
        {
            greenValLabel.Text = greenBar.Value.ToString();
        }

        private void blueBar_ValueChanged(object sender, EventArgs e)
        {
            blueValLabel.Text = blueBar.Value.ToString();
        }

        private void rumbleBoostBar_ValueChanged(object sender, EventArgs e)
        {
            rumbleBoostMotorValLabel.Text = rumbleBoostBar.Value.ToString();
        }

        private void leftMotorBar_ValueChanged(object sender, EventArgs e)
        {
            leftMotorValLabel.Text = leftMotorBar.Value.ToString();
        }

        private void rightMotorBar_ValueChanged(object sender, EventArgs e)
        {
            rightMotorValLabel.Text = rightMotorBar.Value.ToString();
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            byte red = (byte)redBar.Value;
            byte green = (byte)greenBar.Value;
            byte blue = (byte)blueBar.Value;

            rightMotorValLabel.Text = rightMotorBar.Value.ToString();

            byte rumbleboost = (byte)rumbleBoostBar.Value;
            Global.saveColor(device, red, green, blue);
            Global.saveRumbleBoost(device, rumbleboost);
            Global.setTouchSensitivity(device,(byte) sensitivityBar.Value);
            Global.Save();
            this.Close();
        }

        private void batteryLed_CheckedChanged(object sender, EventArgs e)
        {
            Global.setLedAsBatteryIndicator(device, batteryLed.Checked);
        }

        private void flashLed_CheckedChanged(object sender, EventArgs e)
        {
            Global.setFlashWhenLowBattery(device, flashLed.Checked);
        }

        private void colorPick_Click(object sender, EventArgs e)
        {
           
            colorDialog1.ShowDialog();
            
            Global.saveColor(device, colorDialog1.Color.R, colorDialog1.Color.G, colorDialog1.Color.B);
            ledColor color = Global.loadColor(device);
            redBar.Value = color.red;
            greenBar.Value = color.green;
            blueBar.Value = color.blue;
        }

        private void touchCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Global.setTouchEnabled(device,touchCheckBox.Checked);
        }

        private void sensitivityBar_ValueChanged(object sender, EventArgs e)
        {
            sensitivityValLabel.Text = sensitivityBar.Value.ToString();
        }
    }
}
