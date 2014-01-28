using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;

using System.Runtime.InteropServices;
using Microsoft.Win32.SafeHandles;
using System.Diagnostics;
using HidLibrary;
using System.Threading.Tasks;
namespace ScpControl 
{
    public partial class BusDevice : ScpDevice 
    {

        protected DS4Device[] DS4Controllers = new DS4Device[4];
        protected UInt32 m_Packet = 0;
        private class DS4Data
        {
            public byte[] parsedData = new byte[28];
            public byte[] output = new byte[8];
        }
        private DS4Data[] processingData = new DS4Data[4];
        protected ReportEventArgs m_ReportArgs = new ReportEventArgs();
        public event EventHandler<DebugEventArgs> Debug = null;
        public event EventHandler<MappingDoneEventArgs> MappingDone = null;
        public const String BUS_CLASS_GUID = "{F679F562-3164-42CE-A4DB-E7DDBE723909}";
        private Thread[] workers = new Thread[4];
        protected bool isWorkersShouldRun = false;

        protected virtual Int32 Scale(Int32 Value, Boolean Flip) 
        {
            Value -= 0x80;

            if (Value == -128) Value = -127;
            if (Flip) Value *= -1;

            return (Int32)((float) Value * 258.00787401574803149606299212599f);
        }


        public BusDevice() : base(BUS_CLASS_GUID) 
        {
            InitializeComponent();
            Initialize();
        }

        public BusDevice(IContainer container) : base(BUS_CLASS_GUID) 
        {
            container.Add(this);
            Initialize();
        }

        public void Initialize()
        {
            InitializeComponent();
            MappingDone = new EventHandler<MappingDoneEventArgs>(this.On_MappingIsDone);
            for (int i = 0; i < 4; i++)
            {
                int t = i;
                processingData[i] = new DS4Data();
                workers[i] = new Thread(() => { ProcessData(t); });
            }
        }

        public override Boolean Open(int Instance = 0) 
        {
            return base.Open(Instance);
        }

        public override Boolean Open(String DevicePath) 
        {
            m_Path = DevicePath;
            m_WinUsbHandle = (IntPtr) INVALID_HANDLE_VALUE;

            if (GetDeviceHandle(m_Path))
            {
                m_IsActive = true;
               
            }

            return true;
        }

        public override Boolean Start() 
        {    
            if (IsActive)
            {
                int ind = 0;
                LogDebug("Starting....");
                LogDebug("Searching for controllers....");
                int[] pid = { 0x05C4 };
                try
                {
                    IEnumerable<HidDevice> devices = HidDevices.Enumerate(0x054C, pid);
                    foreach (HidDevice device in devices)
                    {
                        LogDebug("Found Controller: VID:" + device.Attributes.VendorHexId + " PID:" + device.Attributes.ProductHexId);
                        device.OpenDevice(Global.getUseExclusiveMode());
                        if (device.IsOpen)
                        {
                            DS4Controllers[ind] = new DS4Device(device, ind);
                            ledColor color = Global.loadColor(ind);
                            DS4Controllers[ind].LedColor = color;
                            DS4Controllers[ind].sendOutputReport();
                            Plugin(ind + 1);
                            int t = ind;
                            if(workers[ind].ThreadState==System.Threading.ThreadState.Aborted || workers[ind].ThreadState==System.Threading.ThreadState.Stopped)
                                workers[ind] = new Thread(() => { ProcessData(t); });
                            workers[ind].Start();
                            LogDebug("Controller " + (ind + 1) + " ready to use");
                        }
                        else
                        {
                            LogDebug("Could not open the controller " + (ind + 1) + " for exclusive access");
                            LogDebug("Try to quit any applications that can be using the controller");
                            LogDebug("Then press Stop and Start to try accessing device again");
                        }


                        ind++;
                    }

                    if (ind == 0 && DS4Controllers[0] == null)
                    {
                        LogDebug("No controllers found");
                    }
                    isWorkersShouldRun = true;
                }
                catch (Exception e)
                {
                    LogDebug(e.ToString());
                }
             
            }
            return true;
        }


        public override Boolean Stop()  
        {
            Monitor.Enter(this);
            if (IsActive)
            {
                isWorkersShouldRun = false;
                for (int i = 1; i <= 4; i++)
                {

                    if (DS4Controllers[i - 1] != null)
                    {
                            LogDebug("Stopping controller " + i);
                            workers[i].Abort();
                            Unplug(i);
                            DS4Controllers[i - 1].Device.CloseDevice();
                            DS4Controllers[i - 1] = null;
                            LogDebug("Controller " + i + " has stopped");
                    }
                }
                Unplug(0);                 
            }
            Monitor.Exit(this);
            return base.Stop();
        }

        public override Boolean Close() 
        {
            Monitor.Enter(this);
            if (IsActive)
            {
                isWorkersShouldRun = false;
                for (int i = 1; i <= 4; i++)
                {
                    if (DS4Controllers[i - 1] != null)
                    {
                            workers[i].Abort();
                            Unplug(i);
                            DS4Controllers[i - 1].Device.CloseDevice();
                            DS4Controllers[i - 1] = null;
                    }
                    Unplug(0);               
               }               
            }
            Monitor.Exit(this);
            return base.Close();
        }


        public virtual void Parse(Byte[] Input, Byte[] Output) 
        {
            Byte Serial = (Byte)(Input[0] + 1);

            for (Int32 Index = 0; Index < 28; Index++) Output[Index] = 0x00;

            Output[0] = 0x1C;
            Output[4] = (Byte)(Input[0] + 1);
            Output[9] = 0x14;

            if (Input[1] == 0x02) // Pad is active
            {
                UInt32 Buttons = (UInt32)((Input[10] << 0) | (Input[11] << 8) | (Input[12] << 16) | (Input[13] << 24));

                if ((Buttons & (0x1 <<  0)) > 0) Output[10] |= (Byte)(1 << 5); // Back
                if ((Buttons & (0x1 <<  1)) > 0) Output[10] |= (Byte)(1 << 6); // Left  Thumb
                if ((Buttons & (0x1 <<  2)) > 0) Output[10] |= (Byte)(1 << 7); // Right Thumb
                if ((Buttons & (0x1 <<  3)) > 0) Output[10] |= (Byte)(1 << 4); // Start

                if ((Buttons & (0x1 <<  4)) > 0) Output[10] |= (Byte)(1 << 0); // Up
                if ((Buttons & (0x1 <<  5)) > 0) Output[10] |= (Byte)(1 << 3); // Down
                if ((Buttons & (0x1 <<  6)) > 0) Output[10] |= (Byte)(1 << 1); // Right
                if ((Buttons & (0x1 <<  7)) > 0) Output[10] |= (Byte)(1 << 2); // Left

                if ((Buttons & (0x1 << 10)) > 0) Output[11] |= (Byte)(1 << 0); // Left  Shoulder
                if ((Buttons & (0x1 << 11)) > 0) Output[11] |= (Byte)(1 << 1); // Right Shoulder

                if ((Buttons & (0x1 << 12)) > 0) Output[11] |= (Byte)(1 << 7); // Y
                if ((Buttons & (0x1 << 13)) > 0) Output[11] |= (Byte)(1 << 5); // B
                if ((Buttons & (0x1 << 14)) > 0) Output[11] |= (Byte)(1 << 4); // A
                if ((Buttons & (0x1 << 15)) > 0) Output[11] |= (Byte)(1 << 6); // X

                if ((Buttons & (0x1 << 16)) > 16) Output[11] |= (Byte)(1 << 2); // Guide     
              
                Output[12] = Input[26]; // Left Trigger
                Output[13] = Input[27]; // Right Trigger
            
                Int32 ThumbLX =  Scale(Input[14], false);
                Int32 ThumbLY = -Scale(Input[15], false);
                Int32 ThumbRX = Scale(Input[16], false);
                Int32 ThumbRY = -Scale(Input[17], false);

                Output[14] = (Byte)((ThumbLX >> 0) & 0xFF); // LX
                Output[15] = (Byte)((ThumbLX >> 8) & 0xFF);

                Output[16] = (Byte)((ThumbLY >> 0) & 0xFF); // LY
                Output[17] = (Byte)((ThumbLY >> 8) & 0xFF);

                Output[18] = (Byte)((ThumbRX >> 0) & 0xFF); // RX
                Output[19] = (Byte)((ThumbRX >> 8) & 0xFF);

                Output[20] = (Byte)((ThumbRY >> 0) & 0xFF); // RY
                Output[21] = (Byte)((ThumbRY >> 8) & 0xFF);
            }
        }


        public virtual Boolean Plugin(Int32 Serial) 
        {
            if (IsActive)
            {
                Int32 Transfered = 0;
                Byte[] Buffer = new Byte[16];

                Buffer[0] = 0x10;
                Buffer[1] = 0x00;
                Buffer[2] = 0x00;
                Buffer[3] = 0x00;

                Buffer[4] = (Byte)((Serial >>  0) & 0xFF);
                Buffer[5] = (Byte)((Serial >>  8) & 0xFF);
                Buffer[6] = (Byte)((Serial >> 16) & 0xFF);
                Buffer[7] = (Byte)((Serial >> 24) & 0xFF);

                return DeviceIoControl(m_FileHandle, 0x2A4000, Buffer, Buffer.Length, null, 0, ref Transfered, IntPtr.Zero);
            }

            return false;
        }

        public virtual Boolean Unplug(Int32 Serial) 
        {
            if (IsActive)
            {
                Int32 Transfered = 0;
                Byte[] Buffer = new Byte[16];

                Buffer[0] = 0x10;
                Buffer[1] = 0x00;
                Buffer[2] = 0x00;
                Buffer[3] = 0x00;

                Buffer[4] = (Byte)((Serial >>  0) & 0xFF);
                Buffer[5] = (Byte)((Serial >>  8) & 0xFF);
                Buffer[6] = (Byte)((Serial >> 16) & 0xFF);
                Buffer[7] = (Byte)((Serial >> 24) & 0xFF);

                return DeviceIoControl(m_FileHandle, 0x2A4004, Buffer, Buffer.Length, null, 0, ref Transfered, IntPtr.Zero);
            }

            return false;
        }

        public void ProcessData(int device)
        {
            if (DS4Controllers[device] != null)
            {
                byte[] data = DS4Controllers[device].retrieveData();
                if (data != null)
                {
                    data[0] = (byte)device;
                    Parse(data, processingData[device].parsedData);
                    Report(processingData[device].parsedData, processingData[device].output);
                }
            }

            MappingDone(this, new MappingDoneEventArgs(device));
            
        }

        protected virtual Boolean LogDebug(byte[] array)
        {
            string dataStr = "";
            for (byte i = 0; i < array.Length; i++)
            {
                dataStr = String.Format("{0}{1:X2} ", dataStr, array[i]);
            }
            DebugEventArgs args = new DebugEventArgs(dataStr);

            On_Debug(this, args);
            return true;
        }


        public virtual Boolean Report(Byte[] Input, Byte[] Output) 
        {
            if (IsActive)
            {
                Int32 Transfered = 0;

                bool result = DeviceIoControl(m_FileHandle, 0x2A400C, Input, Input.Length, Output, Output.Length, ref Transfered, IntPtr.Zero) && Transfered > 0;
                Byte Big = (Byte)(Output[3]);
                Byte Small = (Byte)Output[4];
                int deviceInd = Input[4] - 1;
                if (Output[1] == 0x08)
                {

                    DS4Controllers[deviceInd].SmallRumble = Small;
                    DS4Controllers[deviceInd].BigRumble = Big;
                }
                //Task sendReportTask = new Task(() => DS4Controllers[deviceInd].sendOutputReport());
                //sendReportTask.Start();
                DS4Controllers[deviceInd].sendOutputReport();
                return result;

            }
            return false;
        }

        protected virtual Boolean LogDebug(String Data)
        {
            DebugEventArgs args = new DebugEventArgs(Data);

            On_Debug(this, args);

            return true;
        }

        protected virtual void On_Debug(object sender, DebugEventArgs e)
        {
            if (Debug != null) Debug(sender, e);
        }

        protected virtual void On_MappingIsDone(object sender, MappingDoneEventArgs e)
        {
            Monitor.Enter(this);
            if (isWorkersShouldRun)
            {
                int t = e.DeviceID;
                workers[t] = new Thread(() => { ProcessData(t); });
                workers[t].Start();
                Console.WriteLine(DateTime.Now.Millisecond + " " + t);
            }
            Monitor.Exit(this);
        }

        public void sendUpdateReport(int device)
        {
            DS4Controllers[device].sendOutputReport();
        }

        public void SetLeds(int deviceNum,byte Red, byte Green, byte Blue)
        {
            DS4Controllers[deviceNum].setLedColor(Red, Green, Blue);
        }

        public void setRumble(int deviceNum, byte Boost, byte Left, byte Right)
        {
            DS4Controllers[deviceNum].BigRumble = Left;
            DS4Controllers[deviceNum].SmallRumble = Right;
            DS4Controllers[deviceNum].RumbleBoost = Boost;
        }

        public string getControllerInfo(int device)
        {
            if (DS4Controllers[device] != null)
                return DS4Controllers[device].toString();
            else
                return null;
          
        }

    }

}
