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
namespace ScpControl
{
    public struct ledColor
    {
        public byte red;
        public byte green;
        public byte blue;
    }

    public class DS4Device
    {
        private short charge = 0;
        private bool isUSB = true;
        private int deviceNum = 0;
        private bool m_isTouchEnabled = false;
        private bool lastIsActive = false;
        private Point lastPoint = new Point(0, 0);
        private byte lastL1Value = 0;
        private byte lastL2Value = 0;
        private byte rumbleBoost = 100;
        private byte smallRumble = 0;
        private byte bigRumble = 0;
        private byte ledFlash = 0;
        private HidDevice hid_device;
        private ledColor m_LedColor;
        private byte[] inputData = new byte[64];
        private byte[] outputData;
        private bool isDirty = true;
        private byte[] Report = new byte[64];
        private byte[] btInputData;
        private byte[] green = { 0, 255, 0 };
        private byte[] red = { 255, 0, 0 };

        public HidDevice Device
        {
            get { return hid_device; }
        }

        public byte SmallRumble
        {
            get {
                uint boosted = ((uint)smallRumble * (uint)rumbleBoost)/100;
                if (boosted > 255)
                    boosted = 255;
                return (byte)boosted; 
            }
            set { if (value == smallRumble) return;
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
            set { if (value == bigRumble) return;
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
            set {
                if (m_LedColor.red != value.red || m_LedColor.green != value.green || m_LedColor.blue != value.blue)
                {
                    m_LedColor = value;
                    isDirty = true;
                }
            }
        }

        public byte FlashLed
        {
            get { return ledFlash; }
            set {
                if (ledFlash != value)
                {
                    ledFlash = value;
                    isDirty = true;
                }
            }
        }

        public DS4Device(HidDevice device, int controllerID)
        {
            hid_device = device;
            deviceNum = controllerID;
            isUSB = Device.Capabilities.InputReportByteLength == 64;
            if (isUSB)
                outputData = new byte[Device.Capabilities.OutputReportByteLength];
            else
                btInputData = new byte[Device.Capabilities.InputReportByteLength];
                outputData = new byte[78];
            isTouchEnabled = Global.getTouchEnabled(deviceNum);
        }

        public byte[] retrieveData()
        {
            if (!isUSB)
            {
                if (Device.ReadWithFileStream(btInputData,16) == HidDevice.ReadStatus.Success)
                {
                    Array.Copy(btInputData, 2, inputData, 0, 64);
                    bool touchPressed = (inputData[7] & (1 << 2 - 1)) != 0 ? true : false;
                    toggleTouchpad(inputData[8], inputData[9], touchPressed);
                    updateBatteryStatus(inputData[30], isUSB);
                    if (isTouchEnabled)
                    {
                        HandleTouchpad(inputData,35);
                        permormMouseClick(inputData[6]);
                    }
                }
                else
                {
                    return null;
                }
                Device.flush_Queue();
                return mapButtons(inputData);
               

            }
            else
            {
                if (Device.ReadWithFileStream(inputData, 8) == HidDevice.ReadStatus.Success)
                {
                    bool touchPressed = (inputData[7] & (1 << 2 - 1)) != 0 ? true : false;
                    toggleTouchpad(inputData[8], inputData[9], touchPressed);
                    updateBatteryStatus(inputData[30], isUSB);
                    if (isTouchEnabled)
                    {
                        HandleTouchpad(inputData, 35);
                        permormMouseClick(inputData[6]);
                    }
                }
                else
                {
                    return null;
                }
                Device.flush_Queue();
                return mapButtons(inputData);
            }
        }

        private byte[] mapButtons(byte[] data)
        {
            if (data == null)
            {
                return null;
            }

            Report[1] = 0x02;
            Report[2] = 0x05;
            Report[3] = 0x12;

            Report[14] = data[1]; //Left Stick X


            Report[15] = data[2]; //Left Stick Y


            Report[16] = data[3]; //Right Stick X


            Report[17] = data[4]; //Right Stick Y


            Report[26] = data[8]; //Left Trrigger


            Report[27] = data[9]; //Right Trigger

            var bitY = ((byte)data[5] & (1 << 8 - 1)) != 0;
            var bitB = ((byte)data[5] & (1 << 7 - 1)) != 0;
            var bitA = ((byte)data[5] & (1 << 6 - 1)) != 0;
            var bitX = ((byte)data[5] & (1 << 5 - 1)) != 0;
            var dpadUpBit = ((byte)data[5] & (1 << 4 - 1)) != 0;
            var dpadDownBit = ((byte)data[5] & (1 << 3 - 1)) != 0;
            var dpadLeftBit = ((byte)data[5] & (1 << 2 - 1)) != 0;
            var dpadRightBit = ((byte)data[5] & (1 << 1 - 1)) != 0;



            bool[] dpad = { dpadRightBit, dpadLeftBit, dpadDownBit, dpadUpBit };
            byte c = 0;
            for (int i = 0; i < 4; ++i)
            {
                if (dpad[i])
                {
                    c |= (byte)(1 << i);
                }
            }

            bool dpadUp = false;
            bool dpadLeft = false;
            bool dpadDown = false;
            bool dpadRight = false;

            int dpad_state = c;
            switch (dpad_state)
            {
                case 0:
                    dpadUp = true;
                    dpadDown = false;
                    dpadLeft = false;
                    dpadRight = false;
                    break;
                case 1:
                    dpadUp = true;
                    dpadDown = false;
                    dpadLeft = false;
                    dpadRight = true;
                    break;
                case 2:
                    dpadUp = false;
                    dpadDown = false;
                    dpadLeft = false;
                    dpadRight = true;
                    break;
                case 3:
                    dpadUp = false;
                    dpadDown = true;
                    dpadLeft = false;
                    dpadRight = true;
                    break;
                case 4:
                    dpadUp = false;
                    dpadDown = true;
                    dpadLeft = false;
                    dpadRight = false;
                    break;
                case 5:
                    dpadUp = false;
                    dpadDown = true;
                    dpadLeft = true;
                    dpadRight = false;
                    break;
                case 6:
                    dpadUp = false;
                    dpadDown = false;
                    dpadLeft = true;
                    dpadRight = false;
                    break;
                case 7:
                    dpadUp = true;
                    dpadDown = false;
                    dpadLeft = true;
                    dpadRight = false;
                    break;
                case 8:
                    dpadUp = false;
                    dpadDown = false;
                    dpadLeft = false;
                    dpadRight = false;
                    break;

            }

            var thumbRight = ((byte)data[6] & (1 << 8 - 1)) != 0;
            var thumbLeft = ((byte)data[6] & (1 << 7 - 1)) != 0;
            var start = ((byte)data[6] & (1 << 6 - 1)) != 0;
            var options = ((byte)data[6] & (1 << 5 - 1)) != 0;
            var abit5 = ((byte)data[6] & (1 << 4 - 1)) != 0;
            var abit6 = ((byte)data[6] & (1 << 3 - 1)) != 0;
            var rb = ((byte)data[6] & (1 << 2 - 1)) != 0;
            var lb = ((byte)data[6] & (1 << 1 - 1)) != 0;

            bool[] r11 = { false, false, lb, rb, bitY, bitB, bitA, bitX };

            byte b11 = 0;
            for (int i = 0; i < 8; ++i)
            {
                if (r11[i])
                {
                    b11 |= (byte)(1 << i);
                }
            }
            Report[11] = b11;

            bool[] r10 = { options, thumbLeft, thumbRight, start, dpadUp, dpadRight, dpadDown, dpadLeft };
            byte b10 = 0;
            for (int i = 0; i < 8; ++i)
            {
                if (r10[i])
                {
                    b10 |= (byte)(1 << i);
                }
            }

            Report[10] = b10;

            //Guite
            var Guide = data[7] & (1 << 1 - 1);
            Report[12] = (byte)(Guide != 0 ? 0xFF : 0x00);

            return Report;

        }

        private void toggleTouchpad(byte lt, byte rt, bool touchPressed)
        {
            if (lt == 255 && rt == 255 && touchPressed)
            {
                isTouchEnabled = true;
            }
            else if (lt == 255 && touchPressed && rt == 0)
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

            this.charge = (short) battery;
            if (Global.getLedAsBatteryIndicator(deviceNum))
            {
                uint ratio = (uint)battery;
                LedColor =  Global.getTransitionedColor(red, green, ratio);


            }
            else
            {
                ledColor color = Global.loadColor(deviceNum);
                LedColor = color;
            }

            if (Global.getFlashWhenLowBattery(deviceNum))
            {
                if (battery < 20)
                {
                    FlashLed = 0x80;
                }
                else
                {
                    FlashLed = 0;
                }
            }
            else
            {
                FlashLed = 0;
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
                    outputData[6] = BigRumble; //motor
                    outputData[7] = SmallRumble; //motor
                    outputData[8] = LedColor.red; //red
                    outputData[9] = LedColor.green; //green
                    outputData[10] = LedColor.blue; //blue
                    outputData[11] = FlashLed; //flash;
                    outputData[12] = FlashLed; //flash;

                    if (Device.WriteOutputReportViaControl(outputData))
                    {
                        isDirty = false;
                    }
                }
                else
                {
                    outputData[0] = 0x5;
                    outputData[1] = 0xFF;
                    outputData[4] = BigRumble; //motor
                    outputData[5] = SmallRumble; //motor
                    outputData[6] = LedColor.red; //red
                    outputData[7] = LedColor.green; //green
                    outputData[8] = LedColor.blue; //blue
                    outputData[9] = FlashLed; //flash;
                    outputData[10] = FlashLed; //flash;
                    if (Device.WriteOutputReportViaInterrupt(outputData, 8))
                    {
                        isDirty = false;
                    }
                }
            }
        }

        private void HandleTouchpad(byte[] data, int offset)
        {
            Cursor c = new Cursor(Cursor.Current.Handle);
            int touchID = data[0 + offset] & 0xF0;

            bool _isActive = (data[0 + offset] >> 7) != 0 ? false : true;
            if (_isActive)
            {
                int currentX = data[1 + offset] + ((data[2 + offset] & 0xF) * 255);
                int currentY = ((data[2 + offset] & 0xF0) >> 4) + (data[3 + offset] * 16);
                if (lastIsActive != _isActive)
                {
                    lastPoint = new Point(currentX, currentY);
                }

                int deltax = currentX - lastPoint.X;
                int deltay = currentY - lastPoint.Y;
                float sensitivity = Global.getTouchSensitivity(deviceNum)/(float)100;
                deltax =(int)( (float)deltax * sensitivity);
                deltay = (int)((float)deltay * sensitivity);
                Cursor.Position = new Point(Cursor.Position.X + deltax, Cursor.Position.Y + deltay);
                lastPoint = new Point(currentX, currentY);

            }
            lastIsActive = _isActive;

        }

        private void permormMouseClick(byte data)
        {

            if (data == 1 && lastL1Value != data)
            {
                mouse_event(0x8000 | 0x0002 | 0x0004, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);

            }
            if (data == 2 && lastL2Value != data)
            {
                mouse_event(0x8000 | 0x0008 | 0x0010, (uint)Cursor.Position.X, (uint)Cursor.Position.Y, 0, 0);

            }
            lastL1Value = data;
            lastL2Value = data;
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        public String toString()
        {
            return "Controller " + (deviceNum + 1) + ": " + "Battery = "+charge+"%," +" Touchpad Enabled = " + isTouchEnabled + (isUSB ? " (USB)" : " (BT)");
        }

    }
    

 

}
