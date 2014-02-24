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
        private Dictionary<string, string> defaults = new Dictionary<string, string>();
        private ComboBox lastSelected;

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

                    // Add defaults
                    defaults.Add(((ComboBox)control).Name, ((ComboBox)control).Text);
                    // Add events here (easier for modification/addition)
                    ((ComboBox)control).SelectedIndexChanged += new System.EventHandler(this.SelectedIndexChangedCommand);
                    ((ComboBox)control).Enter += new System.EventHandler(this.EnterCommand);
                    ((ComboBox)control).KeyDown += new System.Windows.Forms.KeyEventHandler(this.KeyDownCommand);
                    ((ComboBox)control).KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.KeyPressCommand);
                    ((ComboBox)control).PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.PreviewKeyDownCommand);
                }
            availableButtons.Sort();
            foreach (ComboBox comboBox in comboBoxes)
                comboBox.Items.AddRange(availableButtons.ToArray());
            // Do not add XInput to touchpad
            cbTouchButtom.Items.Clear();
            cbTouchButtom.Items.Add(cbTouchButtom.Text);
            cbTouchUpper.Items.Clear();
            cbTouchUpper.Items.Add(cbTouchUpper.Text);
            cbTouchMulti.Items.Clear();
            cbTouchMulti.Items.Add(cbTouchMulti.Text);
            Global.loadCustomMapping(Global.getCustomMap(device), comboBoxes.ToArray());
        }

        private void EnterCommand(object sender, EventArgs e)
        {
            //Change image to represent button
            if (sender is ComboBox)
            {
                lastSelected = (ComboBox)sender;
                switch (lastSelected.Name)
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
                    case "cbLY2": pictureBox.Image = Properties.Resources._11;
                        break;
                    case "cbLX2": pictureBox.Image = Properties.Resources._12;
                        break;
                    case "cbR3": pictureBox.Image = Properties.Resources._13;
                        break;
                    case "cbRY": pictureBox.Image = Properties.Resources._14;
                        break;
                    case "cbRX": pictureBox.Image = Properties.Resources._15;
                        break;
                    case "cbRY2": pictureBox.Image = Properties.Resources._14;
                        break;
                    case "cbRX2": pictureBox.Image = Properties.Resources._15;
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
                    case "cbTouchButton": pictureBox.Image = Properties.Resources._22;
                        break;
                    case "cbTouchUpper": pictureBox.Image = Properties.Resources._22;
                        break;
                    case "cbTouchMulti": pictureBox.Image = Properties.Resources._22;
                        break;
                    case "cbPS": pictureBox.Image = Properties.Resources._23;
                        break;
                    default: pictureBox.Image = Properties.Resources._1;
                        break;
                    #endregion
                }
                if (lastSelected.ForeColor == Color.Red)
                    cbRepeat.Checked = true;
                else cbRepeat.Checked = false;
                if (lastSelected.Font.Bold)
                    cbScanCode.Checked = true;
                else cbScanCode.Checked = false;
            }
        }
        private void PreviewKeyDownCommand(object sender, PreviewKeyDownEventArgs e)
        {
            if (sender is ComboBox)
            {
                if (e.KeyCode == Keys.Tab)
                    if (((ComboBox)sender).Text.Length == 0)
                    {
                        ((ComboBox)sender).Tag = e.KeyValue;
                        ((ComboBox)sender).Text = e.KeyCode.ToString();
                        handleNextKeyPress = true;
                    }
            }
        }
        private void KeyDownCommand(object sender, KeyEventArgs e)
        {
            if (sender is ComboBox)
            {
                if (((ComboBox)sender).Tag is int)
                {
                    if (e.KeyValue == (int)(((ComboBox)sender).Tag) 
                        && !((ComboBox)sender).Name.Contains("Touch"))
                    {
                        if (((ComboBox)sender).ForeColor == SystemColors.WindowText)
                        {
                            ((ComboBox)sender).ForeColor = Color.Red;
                            cbRepeat.Checked = true;
                        }
                        else
                        {
                            ((ComboBox)sender).ForeColor = SystemColors.WindowText;
                            cbRepeat.Checked = false;
                        }
                        return;
                    }
                }
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
                if (((ComboBox)sender).Text != defaults[((ComboBox)sender).Name])
                    ((ComboBox)sender).Tag = ((ComboBox)sender).Text;

                else ((ComboBox)sender).Tag = null;
            }
        }
        private void cbRepeat_CheckedChanged(object sender, EventArgs e)
        {
            if (!lastSelected.Name.Contains("Touch") &&
                (lastSelected.Tag is int || lastSelected.Tag is UInt16))
                if (cbRepeat.Checked)
                    lastSelected.ForeColor = Color.Red;
                else lastSelected.ForeColor = SystemColors.WindowText;
            else
            {
                cbRepeat.Checked = false;
                lastSelected.ForeColor = SystemColors.WindowText;
            }
        }
        private void cbScanCode_CheckedChanged(object sender, EventArgs e)
        {
            if (lastSelected.Tag is int || lastSelected.Tag is UInt16)
                if (cbScanCode.Checked)
                    lastSelected.Font = new Font(lastSelected.Font, FontStyle.Bold);
                else lastSelected.Font = new Font(lastSelected.Font, FontStyle.Regular);
            else
            {
                cbScanCode.Checked = false;
                lastSelected.Font = new Font(lastSelected.Font, FontStyle.Regular);
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
