using System;
using System.IO;
using System.ComponentModel;
using System.Text;
using System.Windows.Forms;
using System.Threading;

using System.Resources;
using System.Management;
using System.Text.RegularExpressions;

using System.ServiceProcess;
using System.Configuration.Install;
using System.Collections;
using System.Collections.Specialized;

namespace ScpDriver 
{
    public enum OSType { INVALID, XP, VISTA, WIN7, WIN8, DEFAULT };

    public partial class ScpForm : Form 
    {
        protected String DS3_BUS_CLASS_GUID = "{F679F562-3164-42CE-A4DB-E7DDBE723909}";

        protected Cursor  Saved;
        protected Difx    Installer;

        protected Boolean Bus_Device_Configured = false;
        protected Boolean Bus_Driver_Configured = false;


        protected Boolean Reboot  = false;
        protected OSType  Valid   = OSType.INVALID;
        protected String  InfPath = @".\System\";


        protected String[] Desc = new String[] { "SUCCESS", "INFO   ", "WARNING", "ERROR  " };

        protected void Logger(DifxLog Event, Int32 Error, String Description) 
        {
            if (tbOutput.InvokeRequired)
            {
                Difx.LogEventHandler d = new Difx.LogEventHandler(Logger);
                Invoke(d, new object[] { Event, Error, Description });
            }
            else
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendFormat("{0} - {1}", Desc[(Int32) Event], Description);
                sb.AppendLine();

                tbOutput.AppendText(sb.ToString());
            }
        }


        protected String OSInfo() 
        {
            String Info = String.Empty;

            try
            {
                using (ManagementObjectSearcher mos = new ManagementObjectSearcher("SELECT * FROM  Win32_OperatingSystem"))
                {
                    foreach (ManagementObject mo in mos.Get())
                    {
                        try
                        {
                            Info = Regex.Replace(mo.GetPropertyValue("Caption").ToString(), "[^A-Za-z0-9 ]", "").Trim();

                            try
                            {
                                Object spv = mo.GetPropertyValue("ServicePackMajorVersion");

                                if (spv != null && spv.ToString() != "0")
                                {
                                    Info += " Service Pack " + spv.ToString();
                                }
                            }
                            catch { }

                            Info = String.Format("{0} ({1} {2})", Info, System.Environment.OSVersion.Version.ToString(), System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE"));

                        }
                        catch { }

                        mo.Dispose();
                    }
                }
            }
            catch { }

            return Info;
        }

        protected OSType OSParse(String Info) 
        {
            OSType Valid = OSType.INVALID;

            try
            {
                String Architecture = System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE").ToUpper().Trim();

                if (Environment.Is64BitOperatingSystem == Environment.Is64BitProcess && (Architecture == "X86" || Architecture == "AMD64"))
                {
                    Valid = OSType.DEFAULT;

                    if (!String.IsNullOrEmpty(Info))
                    {
                        String[] Token = Info.Split(new char[] { ' ' });

                        if (Token[0].ToUpper().Trim() == "MICROSOFT" && Token[1].ToUpper().Trim() == "WINDOWS")
                        {
                            switch (Token[2].ToUpper().Trim())
                            {
                                case "XP":

                                    if (!System.Environment.Is64BitOperatingSystem) Valid = OSType.XP;
                                    break;

                                case "VISTA":

                                    Valid = OSType.VISTA;
                                    break;

                                case "7":

                                    Valid = OSType.WIN7;
                                    break;

                                case "8":

                                    Valid = OSType.WIN8;
                                    break;

                                case "SERVER":

                                    switch (Token[3].ToUpper().Trim())
                                    {
                                        case "2008":

                                            if (Token[4].ToUpper().Trim() == "R2")
                                            {
                                                Valid = OSType.WIN7;
                                            }
                                            else
                                            {
                                                Valid = OSType.VISTA;
                                            }
                                            break;

                                        case "2012":

                                            Valid = OSType.WIN8;
                                            break;
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            catch { }

            return Valid;
        }






        public ScpForm() 
        {
            InitializeComponent();

            StringBuilder sb = new StringBuilder();

            sb.AppendFormat("SCP Virtual Bus Driver Installer {0} [{1}]", Application.ProductVersion, DateTime.Now);
            sb.AppendLine();
            sb.AppendLine();

            Installer = Difx.Factory();
            Installer.onLogEvent += Logger;

            String Info = OSInfo();
            Valid = OSParse(Info);

            sb.Append("Detected - ");
            sb.Append(Info);
            sb.AppendLine();

            tbOutput.AppendText(sb.ToString());
            sb.Clear();

            if (Valid == OSType.INVALID)
            {
                btnInstall.Enabled    = false;
                btnUninstall.Enabled  = false;

                sb.AppendLine("Could not find a valid configuration.");
            }
            else
            {
                btnInstall.Enabled    = true;
                btnUninstall.Enabled  = true;

                sb.AppendFormat("Selected {0} configuration.", Valid);
            }

            sb.AppendLine();
            sb.AppendLine();

            tbOutput.AppendText(sb.ToString());
        }

        protected void ScpForm_FormClosing(object sender, FormClosingEventArgs e) 
        {
            try { File.AppendAllLines("ScpDriver.log", tbOutput.Lines); }
            catch { }
        }


        protected void btnInstall_Click(object sender, EventArgs e) 
        {
            Saved  = Cursor;
            Cursor = Cursors.WaitCursor;

            btnInstall.Enabled    = false;
            btnUninstall.Enabled  = false;
            btnExit.Enabled       = false;

            Bus_Device_Configured = false;
            Bus_Driver_Configured = false;


            pbRunning.Style = ProgressBarStyle.Marquee;

            InstallWorker.RunWorkerAsync(InfPath);
        }

        protected void btnUninstall_Click(object sender, EventArgs e) 
        {
            Saved  = Cursor;
            Cursor = Cursors.WaitCursor;

            btnInstall.Enabled    = false;
            btnUninstall.Enabled  = false;
            btnExit.Enabled       = false;

            Bus_Device_Configured = false;
            Bus_Driver_Configured = false;

            pbRunning.Style = ProgressBarStyle.Marquee;

            UninstallWorker.RunWorkerAsync(InfPath);
        }

        protected void btnExit_Click(object sender, EventArgs e) 
        {
            Close();
        }


        protected void InstallWorker_DoWork(object sender, DoWorkEventArgs e) 
        {
            String InfPath = (String) e.Argument;
            String DevPath = String.Empty, InstanceId = String.Empty;

            try
            {
                UInt32  Result = 0;
                Boolean RebootRequired = false;

                DifxFlags Flags = DifxFlags.DRIVER_PACKAGE_ONLY_IF_DEVICE_PRESENT;

                if (cbForce.Checked) Flags |= DifxFlags.DRIVER_PACKAGE_FORCE;

                if (!Devcon.Find(new Guid(DS3_BUS_CLASS_GUID), ref DevPath, ref InstanceId))
                {
                    if (Devcon.Create("System", new Guid("{4D36E97D-E325-11CE-BFC1-08002BE10318}"), "root\\ScpVBus\0\0"))
                    {
                        Logger(DifxLog.DIFXAPI_SUCCESS, 0, "Virtual Bus Created");
                        Bus_Device_Configured = true;
                    }
                }

                Result = Installer.Install(InfPath + @"ScpVBus.inf", Flags, out RebootRequired); Reboot |= RebootRequired;
                if (Result == 0) Bus_Driver_Configured = true;


            }
            catch { }
        }

        protected void InstallWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) 
        {
            pbRunning.Style = ProgressBarStyle.Continuous;

            btnInstall.Enabled    = true;
            btnUninstall.Enabled  = true;
            btnExit.Enabled       = true;

            Cursor = Saved;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine();
            sb.AppendFormat("Install Succeeded.");
            if (Reboot) sb.Append(" [Reboot Required]");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("-- Install Summary --");
            if (Bus_Device_Configured) sb.AppendLine("Bus Device");
            if (Bus_Driver_Configured) sb.AppendLine("Bus Driver");
            sb.AppendLine();

            sb.AppendLine();
            tbOutput.AppendText(sb.ToString());
        }


        protected void UninstallWorker_DoWork(object sender, DoWorkEventArgs e) 
        {
            String InfPath = (String) e.Argument;
            String DevPath = String.Empty, InstanceId = String.Empty;

            try
            {
   
                if ( Devcon.Find(new Guid(DS3_BUS_CLASS_GUID), ref DevPath, ref InstanceId))
                {
                    if (Devcon.Remove(new Guid(DS3_BUS_CLASS_GUID), DevPath, InstanceId))
                    {
                        Logger(DifxLog.DIFXAPI_SUCCESS, 0, "Virtual Bus Removed");
                        Bus_Device_Configured = true;
                    }
                    else
                    {
                        Logger(DifxLog.DIFXAPI_ERROR, 0, "Virtual Bus Removal Failure");
                    }
                }

               
            }
            catch { }
        }

        protected void UninstallWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) 
        {
            pbRunning.Style = ProgressBarStyle.Continuous;

            btnInstall.Enabled    = true;
            btnUninstall.Enabled  = true;
            btnExit.Enabled       = true;

            Cursor = Saved;

            StringBuilder sb = new StringBuilder();

            sb.AppendLine();
            sb.AppendFormat("Uninstall Succeeded.");
            if (Reboot) sb.Append(" [Reboot Required]");
            sb.AppendLine();
            sb.AppendLine();
            sb.AppendLine("-- Uninstall Summary --");
            if (Bus_Device_Configured) sb.AppendLine("Bus Device");
            if (Bus_Driver_Configured) sb.AppendLine("Bus Driver");

            sb.AppendLine();
            tbOutput.AppendText(sb.ToString());
        }
    }
}
