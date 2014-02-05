using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

using ScpControl;

namespace ScpServer 
{
    public partial class ScpForm : Form 
    {

        delegate void LogDebugDelegate(DateTime Time, String Data);

        protected void LogDebug(DateTime Time, String Data) 
        {
            if (lvDebug.InvokeRequired)
            {
                LogDebugDelegate d = new LogDebugDelegate(LogDebug);
                try
                {
                    this.Invoke(d, new Object[] { Time, Data });
                }
                catch { }
            }
            else
            {
                String Posted = Time.ToString() + "." + Time.Millisecond.ToString("000");

                lvDebug.Items.Add(new ListViewItem(new String[] { Posted, Data })).EnsureVisible();
            }
        }

        protected void Form_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                notifyIcon1.Visible = true;
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {       
                notifyIcon1.Visible = false;
            }
        }

        protected RadioButton[] Pad = new RadioButton[4];

        public ScpForm() 
        {
            InitializeComponent();

            ThemeUtil.SetTheme(lvDebug);

            Pad[0] = rbPad_1;
            Pad[1] = rbPad_2;
            Pad[2] = rbPad_3;
            Pad[3] = rbPad_4;
           

            
        }

        protected void Form_Load(object sender, EventArgs e) 
        {
            Icon = Properties.Resources.Scp_All;
            tmrUpdate.Enabled = true;
            Global.Load();
            hideDS4CheckBox.Checked = Global.getUseExclusiveMode();
            if(btnStart.Enabled)
                btnStart_Click(sender, e);
        }

        protected void Form_Close(object sender, FormClosingEventArgs e) 
        {
            rootHub.Close();

        }

        protected void btnStart_Click(object sender, EventArgs e) 
        {
            if (rootHub.Open() && rootHub.Start())
            {
                btnStart.Enabled = false;
                btnStop.Enabled  = true;
            }
        }

        protected void btnStop_Click(object sender, EventArgs e) 
        {
            if (rootHub.Stop())
            {
                btnStart.Enabled = true;
                btnStop.Enabled  = false;
            }
        }

        protected void btnClear_Click(object sender, EventArgs e) 
        {
            lvDebug.Items.Clear();
        }

        protected override void WndProc(ref Message m) 
        {
            try
            {
                if (m.Msg == ScpDevice.WM_DEVICECHANGE)
                {
                    Int32 Type = m.WParam.ToInt32();

                    switch (Type)
                    {
                        case ScpDevice.DBT_DEVICEARRIVAL:
                        case ScpDevice.DBT_DEVICEQUERYREMOVE:
                        case ScpDevice.DBT_DEVICEREMOVECOMPLETE:

                            ScpDevice.DEV_BROADCAST_HDR hdr;

                            hdr = (ScpDevice.DEV_BROADCAST_HDR) Marshal.PtrToStructure(m.LParam, typeof(ScpDevice.DEV_BROADCAST_HDR));

                            if (hdr.dbch_devicetype == ScpDevice.DBT_DEVTYP_DEVICEINTERFACE)
                            {
                                ScpDevice.DEV_BROADCAST_DEVICEINTERFACE_M deviceInterface;

                                deviceInterface = (ScpDevice.DEV_BROADCAST_DEVICEINTERFACE_M) Marshal.PtrToStructure(m.LParam, typeof(ScpDevice.DEV_BROADCAST_DEVICEINTERFACE_M));

                                String Class = "{" + new Guid(deviceInterface.dbcc_classguid).ToString().ToUpper() + "}";

                                String Path = new String(deviceInterface.dbcc_name);
                                Path = Path.Substring(0, Path.IndexOf('\0')).ToUpper();
                            }
                            break;
                    }
                }
            }
            catch { }

            base.WndProc(ref m);
        }

        protected void tmrUpdate_Tick(object sender, EventArgs e) 
        {
            bool optionsEnabled = false;
            int controllers = 0;
            for (Int32 Index = 0; Index < Pad.Length; Index++)
            {
                Pad[Index].Text    = rootHub.getControllerInfo(Index);
                if (Pad[Index].Text != null && Pad[Index].Text != "")
                {
                    Pad[Index].Enabled = true;
                    controllers++;
                    if (Pad[Index].Checked == true)
                    {
                        optionsEnabled = true;
                    }
                    
                }
                else {
                    Pad[Index].Text = "Disconnected";
                    Pad[Index].Enabled = false;
                    Pad[Index].Checked = false;
                }
            }
            if (controllers > 0 && !optionsEnabled)
            {
                // at least one controller present but none checked
                // select the first
                Pad[0].Checked = true;
            }
            optionsButton.Enabled = optionsEnabled;
            btnClear.Enabled = lvDebug.Items.Count > 0;
        }

        protected void On_Debug(object sender, ScpControl.DebugEventArgs e) 
        {
            LogDebug(e.Time, e.Data);
        }

        private void optionsButton_Click(object sender, EventArgs e)
        {
            for (Int32 Index = 0; Index < Pad.Length; Index++)
            {
                if (Pad[Index].Checked)
                {
                    Options opt = new Options(rootHub,Index);
                    opt.Text = "Options for Controller " + (Index + 1);
                    opt.ShowDialog();
                }
            }
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            this.Show();
            WindowState = FormWindowState.Normal;
        }


        private void hotkeysButton_Click(object sender, EventArgs e)
        {
            Hotkeys hotkeysForm = new Hotkeys();
            hotkeysForm.ShowDialog();
        }

        private void hideDS4CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Global.setUseExclusiveMode(hideDS4CheckBox.Checked);
            btnStop_Click(sender, e);
            btnStart_Click(sender, e);
            Global.Save();
        }
    }

    public class ThemeUtil 
    {
        [DllImport("UxTheme", CharSet = CharSet.Unicode, ExactSpelling = true)]
        private static extern int SetWindowTheme(IntPtr hWnd, String appName, String partList);

        public static void SetTheme(ListView lv) 
        {
            try
            {
                SetWindowTheme(lv.Handle, "Explorer", null);
            }
            catch { }
        }

        public static void SetTheme(TreeView tv) 
        {
            try
            {
                SetWindowTheme(tv.Handle, "Explorer", null);
            }
            catch { }
        }
    }
}
