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
        private DS4State PrevState = new DS4State();
        private DS4State cState = new DS4State();
        private short charge = 0;
        private bool isUSB = true;
        private int deviceNum = 0;
        private bool m_isTouchEnabled = false;
        private object lastPressed;

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
        private string MACAddr;
 
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

        public byte SmallRumble
        {
            get {
                uint boosted = ((uint)smallRumble * (uint)rumbleBoost)/100;
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

        public byte FlashLed
        {
            get { return ledFlash; }
            set 
            {
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
            {
                outputData = new byte[Device.Capabilities.OutputReportByteLength];
                byte[] buffer = new byte[16];
                buffer[0] = 18;
                Device.readFeatureData(buffer);
                MACAddr = String.Format("{0:X02}:{1:X02}:{2:X02}:{3:X02}:{4:X02}:{5:X02}", buffer[6], buffer[5], buffer[4], buffer[3], buffer[2], buffer[1]);
            }
            else
            {
                btInputData = new byte[Device.Capabilities.InputReportByteLength];
                outputData = new byte[78];
            }
            isTouchEnabled = Global.getTouchEnabled(deviceNum);
            Device.MonitorDeviceEvents = true;
        }

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

            Device.flush_Queue();

            readButtons(inputData);
            toggleTouchpad(inputData[8], inputData[9], cState.TouchButton);
            updateBatteryStatus(inputData[30], isUSB);
            if (isTouchEnabled)
                Touchpad.handleTouchpad(inputData, cState.TouchButton);

            if (Global.getHasCustomKeysorButtons(deviceNum))
            {
                Mapping.mapButtons(cState);
                return ConvertTo360();
            }
            else return ConvertTo360();
        }


        private byte[] ConvertTo360()
        {

            Report[1] = 0x02;
            Report[2] = 0x05;
            Report[3] = 0x12;

            Report[14] = cState.LX; //Left Stick X


            Report[15] = cState.LY; //Left Stick Y


            Report[16] = cState.RX; //Right Stick X


            Report[17] = cState.RY; //Right Stick Y


            Report[26] = cState.L2; //Left Trigger


            Report[27] = cState.R2; //Right Trigger


            bool[] r11 = { false, false, cState.L1, cState.R1, cState.Triangle, cState.Circle, cState.Cross, cState.Square };

            byte b11 = 0;
            for (int i = 0; i < 8; ++i)
            {
                if (r11[i])
                {
                    b11 |= (byte)(1 << i);
                }
            }
            Report[11] = b11;

            bool[] r10 = { cState.Options, cState.L3, cState.R3, cState.Share, cState.DpadUp, cState.DpadRight, cState.DpadDown, cState.DpadLeft };
            byte b10 = 0;
            for (int i = 0; i < 8; ++i)
            {
                if (r10[i])
                {
                    b10 |= (byte)(1 << i);
                }
            }

            Report[10] = b10;

            //Guide
            Report[12] = (byte)(cState.PS ? 0xFF : 0x00);

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
            bool[] dpad = { cState.DpadRight, cState.DpadLeft, cState.DpadDown, cState.DpadUp };
            byte c = 0;
            for (int i = 0; i < 4; ++i)
            {
                if (dpad[i])
                {
                    c |= (byte)(1 << i);
                }
            }

            int dpad_state = c;
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
            cState.Share = ((byte)data[6] & (1 << 5)) != 0;
            cState.Options = ((byte)data[6] & (1 << 4)) != 0;
            cState.R1 = ((byte)data[6] & (1 << 1)) != 0;
            cState.L1 = ((byte)data[6] & (1 << 0)) != 0;

            cState.PS = ((byte)data[7] & (1 << 0)) != 0;
            cState.TouchButton = (inputData[7] & (1 << 2 - 1)) != 0 ? true : false;

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
                byte[] fullColor = { 
                                   Global.loadColor(deviceNum).red, 
                                   Global.loadColor(deviceNum).green, 
                                   Global.loadColor(deviceNum).blue 
                               };

                // New Setting
                ledColor color = Global.loadLowColor(deviceNum);
                byte[] lowColor = { color.red, color.green, color.blue };

                uint ratio = (uint)battery;
                color = Global.getTransitionedColor(lowColor, fullColor, ratio);
                LedColor = color;


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
                        isDirty = false;
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

        public String toString()
        {
            return "Controller " + (deviceNum + 1) + ": " + "Battery = " + charge + "%," + " Touchpad Enabled = " + isTouchEnabled + (isUSB ? " (USB)" : " (BT)");
        }

    }
    

 

}
