using ScpControl;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ScpServer
{
    public partial class CustomMapping : Form
    {
        private int device;
        private bool handleNextKeyPress = false;
        private List<ComboBox> comboBoxes = new List<ComboBox>();

        public CustomMapping(int deviceNum)
        {
            InitializeComponent();
            device = deviceNum;
            ledColor color = Global.loadColor(device);
            pictureBox.BackColor = Color.FromArgb(color.red, color.green, color.blue);
            List<object> availableButtons = new List<object>();
            foreach (Control control in this.Controls)
                if (control is ComboBox)
                {
                    comboBoxes.Add((ComboBox)control);
                    availableButtons.Add(control.Text);
                }
            foreach (ComboBox comboBox in comboBoxes)
                comboBox.Items.AddRange(availableButtons.ToArray());
            // Do not add XInput to touchpad
            cbPad.Items.Clear();
            Global.loadCustomMapping(Global.getCustomMap(device), comboBoxes.ToArray());
        }

        private void EnterCommand(object sender, EventArgs e)
        {
            //Change image to represent button
            if (sender is ComboBox)
                switch (((ComboBox)sender).Name)
                {
                    #region Set pictureBox.Image to relevant Properties.Resources image
                    case "cbL2": pictureBox.Image = Properties.Resources._2;
                        break;
                    case "cbL1": pictureBox.Image = Properties.Resources._3;
                        break;
                    case "cbR2": pictureBox.Image = Properties.Resources._4;
                        break;
                    case "cbR1": pictureBox.Image = Properties.Resources._5;
                        break;
                    case "cbUp": pictureBox.Image = Properties.Resources._6;
                        break;
                    case "cbLeft": pictureBox.Image = Properties.Resources._7;
                        break;
                    case "cbDown": pictureBox.Image = Properties.Resources._8;
                        break;
                    case "cbRight": pictureBox.Image = Properties.Resources._9;
                        break;
                    case "cbL3": pictureBox.Image = Properties.Resources._10;
                        break;
                    case "cbLY": pictureBox.Image = Properties.Resources._11;
                        break;
                    case "cbLX": pictureBox.Image = Properties.Resources._12;
                        break;
                    case "cbR3": pictureBox.Image = Properties.Resources._13;
                        break;
                    case "cbRY": pictureBox.Image = Properties.Resources._14;
                        break;
                    case "cbRX": pictureBox.Image = Properties.Resources._15;
                        break;
                    case "cbSquare": pictureBox.Image = Properties.Resources._16;
                        break;
                    case "cbCross": pictureBox.Image = Properties.Resources._17;
                        break;
                    case "cbCircle": pictureBox.Image = Properties.Resources._18;
                        break;
                    case "cbTriangle": pictureBox.Image = Properties.Resources._19;
                        break;
                    case "cbOptions": pictureBox.Image = Properties.Resources._20;
                        break;
                    case "cbShare": pictureBox.Image = Properties.Resources._21;
                        break;
                    case "cbPad": pictureBox.Image = Properties.Resources._22;
                        break;
                    case "cbPS": pictureBox.Image = Properties.Resources._23;
                        break;
                    default: pictureBox.Image = Properties.Resources._1;
                        break;
                    #endregion
                }
        }
        private void PreviewKeyDownCommand(object sender, PreviewKeyDownEventArgs e)
        {
            if (e.KeyCode == Keys.Tab)
                if (sender is ComboBox)
                    if (((ComboBox)sender).Text.Length == 0)
                    {
                        ((ComboBox)sender).Tag = e.KeyValue;
                        ((ComboBox)sender).Text = e.KeyCode.ToString();
                        handleNextKeyPress = true;
                    }
        }
        private void KeyDownCommand(object sender, KeyEventArgs e)
        {
            if (sender is ComboBox)
                if (((ComboBox)sender).Text.Length != 0)
                    ((ComboBox)sender).Text = string.Empty;
                else if (e.KeyCode == Keys.Delete)
                {
                    ((ComboBox)sender).Tag = e.KeyValue;
                    ((ComboBox)sender).Text = e.KeyCode.ToString();
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                }
            if (e.KeyCode != Keys.Delete)
            {
                ((ComboBox)sender).Tag = e.KeyValue;
                ((ComboBox)sender).Text = e.KeyCode.ToString();
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }
        private void KeyPressCommand(object sender, KeyPressEventArgs e)
        {
            if (handleNextKeyPress)
            {
                e.Handled = true;
                handleNextKeyPress = false;
            }
        }
        private void SelectedIndexChangedCommand(object sender, EventArgs e)
        {
            if (sender is ComboBox)
            {
                foreach (ComboBox comboBox in comboBoxes)
                    if (comboBox.Text == ((ComboBox)sender).Text && comboBox.Name != ((ComboBox)sender).Name)
                    {
                        comboBox.Text = "(Unbound)";
                        comboBox.Tag = comboBox.Text;
                    }
                if (((ComboBox)sender).Name != comboBoxes[((ComboBox)sender).SelectedIndex].Name)
                    ((ComboBox)sender).Tag = ((ComboBox)sender).Text;

                else ((ComboBox)sender).Tag = null;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDg = new SaveFileDialog();
            saveFileDg.DefaultExt = "xml";
            saveFileDg.Filter = "SCP Custom Map Files (*.xml)|*.xml";
            saveFileDg.FileName = "SCP Custom Mapping.xml";
            if (saveFileDg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                if (Global.saveCustomMapping(saveFileDg.FileName, comboBoxes.ToArray()))
                {
                    if (MessageBox.Show("Custom mapping saved. Enable now?",
                        "Save Successfull", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                        == System.Windows.Forms.DialogResult.Yes)
                    {
                        Global.setCustomMap(device, saveFileDg.FileName);
                        Global.Save();
                        Global.loadCustomMapping(device);
                    }
                }
                else MessageBox.Show("Custom mapping did not save successfully.", 
                    "Save Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void btnLoad_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDg = new OpenFileDialog();
            openFileDg.CheckFileExists = true;
            openFileDg.CheckPathExists = true;
            openFileDg.DefaultExt = "xml";
            openFileDg.Filter = "SCP Custom Map Files (*.xml)|*.xml";
            openFileDg.Multiselect = false;
            if (openFileDg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Global.loadCustomMapping(openFileDg.FileName, comboBoxes.ToArray());
                Global.setCustomMap(device, openFileDg.FileName);
                Global.Save();
            }
        }
    }
}
