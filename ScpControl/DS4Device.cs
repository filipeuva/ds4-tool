using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HidLibrary;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Diagnostics;
namespace ScpControl
{
    public struct ledColor
    {
        public byte red;
        public byte green;
        public byte blue;
    }

    public struct DS4State
    {
        public byte LX, LY, RX, RY, L2, R2;
        public bool Square, Triangle, Circle, Cross, Share, Options, TouchButton, L1, R1, L3, R3, PS;
        public bool DpadUp, DpadRight, DpadDown, DpadLeft;
    }

    public class DS4Device
    {
        private DS4State pState, cState, nState; // previous/current/next-state flipping
        private short charge = 0;
        private bool isUSB = true;
        private int deviceNum = 0;
        private bool m_isTouchEnabled = false;

        private byte rumbleBoost = 100;
        private byte smallRumble = 0;
        private byte bigRumble = 0;
        private byte ledFlashOn = 0, ledFlashOff = 0;
        private HidDevice hid_device;
        private ledColor m_LedColor;
        private byte[] inputData = new byte[64];
        private byte[] outputData;
        private bool isDirty = true;
        private byte[] Report = new byte[64];
        private byte[] btInputData;
        private string MACAddr;
        public event EventHandler<DebugEventArgs> Debug = null;

        private readonly static byte[/* Light On duration */, /* Light Off duration */] BatteryIndicatorDurations =
        {
            { 255, 255 }, // 0 doesn't happen
            { 28, 252 },
            { 56, 224 },
            { 84, 196 },
            { 112, 168 },
            { 140, 140 },
            { 168, 112 },
            { 196, 84 },
            { 224, 56}, // on 80% of the time at 80, etc.
            { 252, 28 }, // on 90% of the time at 90
            { 0, 0 } // no flash at 100
        };


        // Publicize Input Data
        public byte[] InputData
        {
            get
            {
                return inputData;
            }
        }

        public HidDevice Device
        {
            get { return hid_device; }
        }

        public string MACAddress
        {
            get { return MACAddr; }
        }
        public byte SmallRumble
        {
            get
            {
                uint boosted = ((uint)smallRumble * (uint)rumbleBoost) / 100;
                if (boosted > 255)
                    boosted = 255;
                return (byte)boosted;
            }
            set
            {
                if (value == smallRumble) return;
                smallRumble = value;
                isDirty = true;
            }
        }

        public byte BigRumble
        {
            get
            {
                uint boosted = ((uint)bigRumble * (uint)rumbleBoost) / 100;
                if (boosted > 255)
                    boosted = 255;
                return (byte)boosted;
            }
            set
            {
                if (value == bigRumble) return;
                bigRumble = value;
                isDirty = true;
            }
        }

        public byte RumbleBoost
        {
            get { return rumbleBoost; }
            set { rumbleBoost = value; }
        }

        public bool isTouchEnabled
        {
            get { return m_isTouchEnabled; }
            set { m_isTouchEnabled = value; }
        }

        public void setLedColor(byte red, byte green, byte blue)
        {
            if (m_LedColor.red != red || m_LedColor.green != green || m_LedColor.blue != blue)
            {
                m_LedColor.red = red;
                m_LedColor.green = green;
                m_LedColor.blue = blue;
                isDirty = true;
            }
        }

        public ledColor LedColor
        {
            get { return m_LedColor; }
            set
            {
                if (m_LedColor.red != value.red || m_LedColor.green != value.green || m_LedColor.blue != value.blue)
                {
                    m_LedColor = value;
                    isDirty = true;
                }
            }
        }

        public byte FlashLedOn
        {
            get { return ledFlashOn; }
            set
            {
                if (ledFlashOn != value)
                {
                    ledFlashOn = value;
                    isDirty = true;
                }
            }
        }
        public byte FlashLedOff
        {
            get { return ledFlashOff; }
            set
            {
                if (ledFlashOff != value)
                {
                    ledFlashOff = value;
                    isDirty = true;
                }
            }
        }

        public DS4Device(HidDevice device, int controllerID)
        {
            hid_device = device;
            deviceNum = controllerID;
            isUSB = Device.Capabilities.InputReportByteLength == 64;
            MACAddr = Device.readSerial();
            if (isUSB)
            {
                outputData = new byte[Device.Capabilities.OutputReportByteLength];
            }
            else
            {
                btInputData = new byte[Device.Capabilities.InputReportByteLength];
                outputData = new byte[78];
            }
            isTouchEnabled = Global.getTouchEnabled(deviceNum);
            touchpad = new Touchpad(deviceNum);
            mouse = new Mouse(deviceNum);
            touchpad.TouchButtonDown += mouse.touchButtonDown;
            touchpad.TouchButtonUp += mouse.touchButtonUp;
            touchpad.TouchesBegan += mouse.touchesBegan;
            touchpad.TouchesMoved += mouse.touchesMoved;
            touchpad.TouchesEnded += mouse.touchesEnded;
            Device.MonitorDeviceEvents = true;

          

        }
        internal Touchpad touchpad;
        internal Mouse mouse;
        public byte[] retrieveData()
        {
            if (!isUSB)
                if (Device.ReadFile(btInputData) == HidDevice.ReadStatus.Success)
                {
                    Array.Copy(btInputData, 2, inputData, 0, 64);
                }
                else return null;
            else if (Device.ReadFile(inputData) == HidDevice.ReadStatus.Success) { }
            else return null;

            if (Global.getFlushHIDQueue(deviceNum))
                Device.flush_Queue();

            readButtons(inputData);
            checkQuickDisconnect(); // XXX race when first connecting, quick disconnect will only half-work in the first moments
            toggleTouchpad(inputData[8], inputData[9], cState.TouchButton);
            updateBatteryStatus(inputData[30], isUSB);
            if (isTouchEnabled)
                touchpad.handleTouchpad(inputData, cState.TouchButton);

            if (Global.getHasCustomKeysorButtons(deviceNum))
            {
                Mapping.mapButtons(ref nState, ref cState, ref pState, mouse);
                DS4State swap = pState;
                pState = cState;
                cState = nState;
                nState = swap;
                return ConvertTo360();
            }
            else
            {
                pState = cState;
                return ConvertTo360();
            }
        }

        // Make quick disconnet send a bus disconnect and in the meantime pretend all input is idle.
        private bool disconnecting = false;
        private readonly static DS4State IdleDS4State = new DS4State {
            DpadDown = false, DpadLeft = false, DpadRight = false, DpadUp = false,
            Circle = false, Cross = false, Square = false, Triangle = false,
            Options = false, PS = false, Share = false, TouchButton = false,
            L1 = false, L2 = 0, L3 = false, LX = 127, LY = 127,
            R1 = false, R2 = 0, R3 = false, RX = 128, RY = 128
        };
        private void checkQuickDisconnect()
        {
            if (disconnecting)
                cState = IdleDS4State;
            else if (!isUSB && cState.Options && cState.PS)
            {
                DisconnectBT();
                disconnecting = true;
                cState = IdleDS4State;
            }
        }

        private byte[] ConvertTo360()
        {

            Report[1] = 0x02;
            Report[2] = 0x05;
            Report[3] = 0x12;

            Report[10] = (byte)(
                ((cState.Share ? 1 : 0) << 0) |
                ((cState.L3 ? 1 : 0) << 1) |
                ((cState.R3 ? 1 : 0) << 2) |
                ((cState.Options ? 1 : 0) << 3) |
                ((cState.DpadUp ? 1 : 0) << 4) |
                ((cState.DpadRight ? 1 : 0) << 5) |
                ((cState.DpadDown ? 1 : 0) << 6) |
                ((cState.DpadLeft ? 1 : 0) << 7));

            Report[11] = (byte)(
                ((cState.L1 ? 1 : 0) << 2) |
                ((cState.R1 ? 1 : 0) << 3) |
                ((cState.Triangle ? 1 : 0) << 4) |
                ((cState.Circle ? 1 : 0) << 5) |
                ((cState.Cross ? 1 : 0) << 6) |
                ((cState.Square ? 1 : 0) << 7));

            //Guide
            Report[12] = (byte)(cState.PS ? 0xFF : 0x00);


            Report[14] = cState.LX; //Left Stick X


            Report[15] = cState.LY; //Left Stick Y


            Report[16] = cState.RX; //Right Stick X


            Report[17] = cState.RY; //Right Stick Y


            Report[26] = Mapping.mapLeftTrigger(cState.L2, deviceNum); //Left Trigger


            Report[27] = Mapping.mapRightTrigger(cState.R2, deviceNum); //Right Trigger

            return Report;

        }

        private void readButtons(byte[] data)
        {
            if (data == null)
            {
                return;
            }

            cState.LX = data[1];
            cState.LY = data[2];
            cState.RX = data[3];
            cState.RY = data[4];
            cState.L2 = data[8];
            cState.R2 = data[9];

            cState.Triangle = ((byte)data[5] & (1 << 7)) != 0;
            cState.Circle = ((byte)data[5] & (1 << 6)) != 0;
            cState.Cross = ((byte)data[5] & (1 << 5)) != 0;
            cState.Square = ((byte)data[5] & (1 << 4)) != 0;
            cState.DpadUp = ((byte)data[5] & (1 << 3)) != 0;
            cState.DpadDown = ((byte)data[5] & (1 << 2)) != 0;
            cState.DpadLeft = ((byte)data[5] & (1 << 1)) != 0;
            cState.DpadRight = ((byte)data[5] & (1 << 0)) != 0;

            //Convert dpad into individual On/Off bits instead of a clock representation
            int dpad_state = ((cState.DpadRight ? 1 : 0) << 0) |
                ((cState.DpadLeft ? 1 : 0) << 1) |
                ((cState.DpadDown ? 1 : 0) << 2) |
                ((cState.DpadUp ? 1 : 0) << 3);
            switch (dpad_state)
            {
                case 0: cState.DpadUp = true; cState.DpadDown = false; cState.DpadLeft = false; cState.DpadRight = false; break;
                case 1: cState.DpadUp = true; cState.DpadDown = false; cState.DpadLeft = false; cState.DpadRight = true; break;
                case 2: cState.DpadUp = false; cState.DpadDown = false; cState.DpadLeft = false; cState.DpadRight = true; break;
                case 3: cState.DpadUp = false; cState.DpadDown = true; cState.DpadLeft = false; cState.DpadRight = true; break;
                case 4: cState.DpadUp = false; cState.DpadDown = true; cState.DpadLeft = false; cState.DpadRight = false; break;
                case 5: cState.DpadUp = false; cState.DpadDown = true; cState.DpadLeft = true; cState.DpadRight = false; break;
                case 6: cState.DpadUp = false; cState.DpadDown = false; cState.DpadLeft = true; cState.DpadRight = false; break;
                case 7: cState.DpadUp = true; cState.DpadDown = false; cState.DpadLeft = true; cState.DpadRight = false; break;
                case 8: cState.DpadUp = false; cState.DpadDown = false; cState.DpadLeft = false; cState.DpadRight = false; break;
            }

            cState.R3 = ((byte)data[6] & (1 << 7)) != 0;
            cState.L3 = ((byte)data[6] & (1 << 6)) != 0;
            cState.Options = ((byte)data[6] & (1 << 5)) != 0;
            cState.Share = ((byte)data[6] & (1 << 4)) != 0;
            cState.R1 = ((byte)data[6] & (1 << 1)) != 0;
            cState.L1 = ((byte)data[6] & (1 << 0)) != 0;

            cState.PS = ((byte)data[7] & (1 << 0)) != 0;
            cState.TouchButton = (inputData[7] & (1 << 2 - 1)) != 0;
        }

        private void toggleTouchpad(bool enable)
        {
            // Turned on and off only with touch clicking
            if (enable)
                if (isTouchEnabled)
                    isTouchEnabled = false;
                else isTouchEnabled = true;
        }

        private void toggleTouchpad(byte lt, byte rt, bool touchPressed)
        {
            if (lt > 127 && rt > 127 && touchPressed)
            {
                isTouchEnabled = true;
            }
            else if (lt > 127 && touchPressed && rt <= 127)
            {
                isTouchEnabled = false;
            }
        }

        private void updateBatteryStatus(int status, bool isUsb)
        {
            int battery = 0;
            if (isUsb)
            {
                battery = (status - 16) * 10;
                if (battery > 100)
                    battery = 100;
            }
            else
            {
                battery = (status + 1) * 10;
                if (battery > 100)
                    battery = 100;
            }

            this.charge = (short)battery;
            if (Global.getLedAsBatteryIndicator(deviceNum))
                LedColor = Global.getTransitionedColor(Global.loadLowColor(deviceNum), Global.loadHighColor(deviceNum), (uint)battery);
            else
                LedColor = Global.loadColor(deviceNum);

            if (Global.getFlashWhenLowBattery(deviceNum))
            {
                FlashLedOn = BatteryIndicatorDurations[battery / 10, 0];
                FlashLedOff = BatteryIndicatorDurations[battery / 10, 1];
            }
            else
            {
                FlashLedOn = FlashLedOff = 0;
            }
        }

        public void sendOutputReport()
        {

            if (isDirty)
            {
                if (!isUSB)
                {
                    outputData[0] = 0x11;
                    outputData[1] = 128;
                    outputData[3] = 0xff;
                    outputData[6] = SmallRumble; //fast motor
                    outputData[7] = BigRumble; //slow motor
                    outputData[8] = LedColor.red; //red
                    outputData[9] = LedColor.green; //green
                    outputData[10] = LedColor.blue; //blue
                    outputData[11] = FlashLedOn; //flash on duration
                    outputData[12] = FlashLedOff; //flash off duration

                    if (Device.WriteOutputReportViaControl(outputData))
                        isDirty = false;
                }
                else
                {
                    outputData[0] = 0x5;
                    outputData[1] = 0xFF;
                    outputData[4] = SmallRumble; //fast motor
                    outputData[5] = BigRumble; //slow  motor
                    outputData[6] = LedColor.red; //red
                    outputData[7] = LedColor.green; //green
                    outputData[8] = LedColor.blue; //blue
                    outputData[9] = FlashLedOn; //flash on duration
                    outputData[10] = FlashLedOff; //flash off duration
                    if (Device.WriteOutputReportViaInterrupt(outputData, 8))
                    {
                        isDirty = false;
                    }
                }
            }
        }

        public String toString()
        {
            return "Controller " + (deviceNum + 1) + ": " + MACAddress + ", Battery = " + charge + "%," + " Touchpad Enabled = " + isTouchEnabled + (isUSB ? " (USB)" : " (BT)");
        }

        public bool DisconnectBT()
        {
            if (MACAddr != null)
            {
                Console.WriteLine("Trying to disonnect BT device");
                IntPtr btHandle = IntPtr.Zero;
                int IOCTL_BTH_DISCONNECT_DEVICE = 0x41000c;

                byte[] btAddr = new byte[8];
                string[] sbytes = MACAddr.Split(':');
                for (int i = 0; i < 6; i++)
                {
                    //parse hex byte in reverse order
                    btAddr[5 - i] = Convert.ToByte(sbytes[i], 16);
                }
                long lbtAddr = BitConverter.ToInt64(btAddr, 0);


                BLUETOOTH_FIND_RADIO_PARAMS p = new BLUETOOTH_FIND_RADIO_PARAMS();
                p.dwSize = Marshal.SizeOf(typeof(BLUETOOTH_FIND_RADIO_PARAMS));
                IntPtr searchHandle = BluetoothFindFirstRadio(ref p, ref btHandle);
                int bytesReturned = 0;
                bool success  = false;
                while (!success && btHandle != IntPtr.Zero)
                {
                    success = DeviceIoControl(btHandle, IOCTL_BTH_DISCONNECT_DEVICE, ref lbtAddr, 8, IntPtr.Zero, 0, ref bytesReturned, IntPtr.Zero);
                    CloseHandle(btHandle);
                    if (!success)
                        if (!BluetoothFindNextRadio(searchHandle, ref btHandle))
                            btHandle = IntPtr.Zero;

                }              
                BluetoothFindRadioClose(searchHandle);
                Console.WriteLine("Disconnect successul: "+success);
                return success;
            }
            return false;
        }

        protected virtual Boolean LogDebug(String Data)
        {
            DebugEventArgs args = new DebugEventArgs(Data);

            if(Debug!=null)
                Debug(this, args);

            return true;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct BLUETOOTH_FIND_RADIO_PARAMS
        {
            [MarshalAs(UnmanagedType.U4)]
            public int dwSize;
        }

        [DllImport("bthprops.cpl", CharSet = CharSet.Auto)]
        private extern static IntPtr BluetoothFindFirstRadio(ref BLUETOOTH_FIND_RADIO_PARAMS pbtfrp, ref IntPtr phRadio);

        [DllImport("bthprops.cpl", CharSet = CharSet.Auto)]
        private extern static bool BluetoothFindNextRadio(IntPtr hFind, ref IntPtr phRadio);

        [DllImport("bthprops.cpl", CharSet = CharSet.Auto)]
        private extern static bool BluetoothFindRadioClose(IntPtr hFind);


        [DllImport("kernel32.dll", SetLastError = true)]
        protected static extern Boolean DeviceIoControl(IntPtr DeviceHandle, Int32 IoControlCode, ref long InBuffer, Int32 InBufferSize, IntPtr OutBuffer, Int32 OutBufferSize, ref Int32 BytesReturned, IntPtr Overlapped);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true, CharSet = CharSet.Auto)]
        static internal extern bool CloseHandle(IntPtr hObject);

    }




}
