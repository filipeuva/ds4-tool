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

    public class DS4Device
    {
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
                    bool touchPressed = (inputData[7] & (1 << 2 - 1)) != 0 ? true : false;
                    toggleTouchpad(inputData[8], inputData[9], touchPressed);
                    updateBatteryStatus(inputData[30], isUSB);
                    if (isTouchEnabled)
                        Touchpad.handleTouchpad(inputData, touchPressed);
                }
                else return null;
            else if (Device.ReadFile(inputData) == HidDevice.ReadStatus.Success)
            {
                bool touchPressed = (inputData[7] & (1 << 2 - 1)) != 0 ? true : false;
                toggleTouchpad(inputData[8], inputData[9], touchPressed);
                updateBatteryStatus(inputData[30], isUSB);
                if (isTouchEnabled)
                    Touchpad.handleTouchpad(inputData, touchPressed);
            }
            else return null;
            Device.flush_Queue();
            if (Global.getHasCustomKeysorButtons(deviceNum))
                return remapButtons(inputData);
            else return mapButtons(inputData);
        }

        private byte[] remapButtons(byte[] data)
        {
            if (data == null)
                return null;
            Report[1] = 0x02;
            Report[2] = 0x05;
            Report[3] = 0x12;
            // ignore 4 to 9

            bool up =      ((byte)data[5] & (1 << 4 - 1)) != 0,
                down =     ((byte)data[5] & (1 << 3 - 1)) != 0,
                left =     ((byte)data[5] & (1 << 2 - 1)) != 0,
                right =    ((byte)data[5] & (1 << 1 - 1)) != 0,
                triangle = ((byte)data[5] & (1 << 8 - 1)) != 0,
                circle =   ((byte)data[5] & (1 << 7 - 1)) != 0,
                cross =    ((byte)data[5] & (1 << 6 - 1)) != 0,
                square =   ((byte)data[5] & (1 << 5 - 1)) != 0,
                r1 =       ((byte)data[6] & (1 << 2 - 1)) != 0,
                l1 =       ((byte)data[6] & (1 << 1 - 1)) != 0,
                r2 =       ((byte)data[6] & (1 << 4 - 1)) != 0,
                l2 =       ((byte)data[6] & (1 << 3 - 1)) != 0,
                r3 =       ((byte)data[6] & (1 << 8 - 1)) != 0,
                l3 =       ((byte)data[6] & (1 << 7 - 1)) != 0,
                options =  ((byte)data[6] & (1 << 6 - 1)) != 0,
                share =    ((byte)data[6] & (1 << 5 - 1)) != 0,
                ps =       ((byte)data[7] & (1 << 1 - 1)) != 0;

            bool[] dpad = { right, left, down, up };
            byte dpad_state = 0;
            for (int i = 0; i < 4; ++i)
                if (dpad[i])
                    dpad_state |= (byte)(1 << i);

            switch (dpad_state)
            {
                case 0: up = true;  down = false; left = false; right = false; break;
                case 1: up = true;  down = false; left = false; right = true;  break;
                case 2: up = false; down = false; left = false; right = true;  break;
                case 3: up = false; down = true;  left = false; right = true;  break;
                case 4: up = false; down = true;  left = false; right = false; break;
                case 5: up = false; down = true;  left = true;  right = false; break;
                case 6: up = false; down = false; left = true;  right = false; break;
                case 7: up = true;  down = false; left = true;  right = false; break;
                case 8: up = false; down = false; left = false; right = false; break;
            }

            bool[] r10 = { share, l3, r3, options, up, right, down, left };
            bool[] r11 = { false, false, l1, r1, triangle, circle, cross, square };
            byte[] rx = { (byte)(ps?255:0), data[1], data[2], data[3], data[4], data[8], data[9] };

            RemapButtons(r10, r11, rx);

            byte b10 = 0;
            for (int i = 0; i < 8; ++i)
                if (r10[i])
                    b10 |= (byte)(1 << i);
            Report[10] = b10;
            byte b11 = 0;
            for (int i = 0; i < 8; ++i)
                if (r11[i])
                    b11 |= (byte)(1 << i);
            Report[11] = b11;

            Report[12] = rx[0]; //Guide
            Report[14] = rx[1]; //Left Stick X
            Report[15] = rx[2]; //Left Stick Y
            Report[16] = rx[3]; //Right Stick X
            Report[17] = rx[4]; //Right Stick Y
            Report[26] = rx[5]; //Left Trigger
            Report[27] = rx[6]; //Right Trigger

            return Report;
        }
        private void RemapButtons(bool[] r10, bool[] r11, byte[] rx)
        {
            // Original array values
            bool[] or10 = new bool[r10.Length], or11 = new bool[r11.Length];
            byte[] orx = new byte[rx.Length];
            Array.Copy(r10, or10, r10.Length);
            Array.Copy(r11, or11, r11.Length);
            Array.Copy(rx, orx, rx.Length);

            bool anyPressed = false;
                foreach (KeyValuePair<string, ushort> customKey in Global.getCustomKeys())
                    if (RemapBool(or10, or11, orx, customKey.Key))
                    {
                        anyPressed = true;
                        if (lastPressed == null)
                        {
                            ResetMapping(r10, r11, rx, customKey.Key);
                            Touchpad.performKeyPress(customKey.Value);
                            lastPressed = customKey.Value;
                        }
                    }
                    else if (lastPressed is ushort && (ushort)lastPressed == customKey.Value)
                    {
                        Touchpad.performKeyRelease(customKey.Value);
                        // Reset last pressed to avoid keyup repeat, 
                        // but not to null or the keydown will repeat
                        lastPressed = 0;
                    }

                foreach (KeyValuePair<string, string> customButton in Global.getCustomButtons())
                    switch (customButton.Value)
                    {
                        case "Back":         r10[0] = RemapBool(or10, or11, orx, customButton.Key); break;
                        case "Left Stick":   r10[1] = RemapBool(or10, or11, orx, customButton.Key); break;
                        case "Right Stick":  r10[2] = RemapBool(or10, or11, orx, customButton.Key); break;
                        case "Start":        r10[3] = RemapBool(or10, or11, orx, customButton.Key); break;
                        case "Up Button":    r10[4] = RemapBool(or10, or11, orx, customButton.Key); break;
                        case "Right Button": r10[5] = RemapBool(or10, or11, orx, customButton.Key); break;
                        case "Down Button":  r10[6] = RemapBool(or10, or11, orx, customButton.Key); break;
                        case "Left Button":  r10[7] = RemapBool(or10, or11, orx, customButton.Key); break;

                        case "Left Bumper":  r11[2] = RemapBool(or10, or11, orx, customButton.Key); break;
                        case "Right Bumper": r11[3] = RemapBool(or10, or11, orx, customButton.Key); break;
                        case "Y Button":     r11[4] = RemapBool(or10, or11, orx, customButton.Key); break;
                        case "B Button":     r11[5] = RemapBool(or10, or11, orx, customButton.Key); break;
                        case "A Button":     r11[6] = RemapBool(or10, or11, orx, customButton.Key); break;
                        case "X Button":     r11[7] = RemapBool(or10, or11, orx, customButton.Key); break;

                        case "Guide":         rx[0] = RemapByte(or10, or11, orx, customButton.Key); break;
                        case "Left X-Axis":   rx[1] = RemapByte(or10, or11, orx, customButton.Key); break;
                        case "Left Y-Axis":   rx[2] = RemapByte(or10, or11, orx, customButton.Key); break;
                        case "Right X-Axis":  rx[3] = RemapByte(or10, or11, orx, customButton.Key); break;
                        case "Right Y-Axis":  rx[4] = RemapByte(or10, or11, orx, customButton.Key); break;
                        case "Left Trigger":  rx[5] = RemapByte(or10, or11, orx, customButton.Key); break;
                        case "Right Trigger": rx[6] = RemapByte(or10, or11, orx, customButton.Key); break;

                        case "Click": if (RemapBool(or10, or11, orx, customButton.Key))
                            {
                                anyPressed = true;
                                if (lastPressed == null)
                                {
                                    ResetMapping(r10, r11, rx, customButton.Key);
                                    Touchpad.performLeftClick();
                                    lastPressed = customButton.Value;
                                }
                            }
                            break;
                    }
                if (!anyPressed && lastPressed != null)
                    lastPressed = null;
        }
        private bool RemapBool(bool[] r10, bool[] r11, byte[] rx, string key)
        {
            switch (key)
            {
                case "cbShare":    return r10[0];
                case "cbL3":       return r10[1];
                case "cbR3":       return r10[2];
                case "cbOptions":  return r10[3];
                case "cbUp":       return r10[4];
                case "cbRight":    return r10[5];
                case "cbDown":     return r10[6];
                case "cbLeft":     return r10[7];

                case "cbL1":       return r11[2];
                case "cbR1":       return r11[3];
                case "cbTriangle": return r11[4];
                case "cbCircle":   return r11[5];
                case "cbCross":    return r11[6];
                case "cbSquare":   return r11[7];

                case "cbPS":       return rx[0] > 100;
                case "cbLX":       return rx[1] > 127;
                case "cbLY":       return rx[2] > 123;
                case "cbRX":       return rx[3] > 125;
                case "cbRY":       return rx[4] > 127;
                case "cbL2":       return rx[5] > 100;
                case "cbR2":       return rx[6] > 100;
            }
            return false;
        }
        private byte RemapByte(bool[] r10, bool[] r11, byte[] rx, string key)
        {
            switch (key)
            {
                case "cbShare":    return (byte)(r10[0]?255:0);
                case "cbL3":       return (byte)(r10[1]?255:0);
                case "cbR3":       return (byte)(r10[2]?255:0);
                case "cbOptions":  return (byte)(r10[3]?255:0);
                case "cbUp":       return (byte)(r10[4]?255:0);
                case "cbRight":    return (byte)(r10[5]?255:0);
                case "cbDown":     return (byte)(r10[6]?255:0);
                case "cbLeft":     return (byte)(r10[7]?255:0);

                case "cbL1":       return (byte)(r11[2]?255:0);
                case "cbR1":       return (byte)(r11[3]?255:0);
                case "cbTriangle": return (byte)(r11[4]?255:0);
                case "cbCircle":   return (byte)(r11[5]?255:0);
                case "cbCross":    return (byte)(r11[6]?255:0);
                case "cbSquare":   return (byte)(r11[7]?255:0);

                case "cbPS":       return rx[0];
                case "cbLX":       return rx[1];
                case "cbLY":       return rx[2];
                case "cbRX":       return rx[3];
                case "cbRY":       return rx[4];
                case "cbL2":       return rx[5];
                case "cbR2":       return rx[6];
            }
            return 0;
        }
        private void ResetMapping(bool[] r10, bool[] r11, byte[] rx, string key)
        {
            switch (key)
            {
                case "cbShare":    r10[0] = false; break;
                case "cbL3":       r10[1] = false; break;
                case "cbR3":       r10[2] = false; break;
                case "cbOptions":  r10[3] = false; break;
                case "cbUp":       r10[4] = false; break;
                case "cbRight":    r10[5] = false; break;
                case "cbDown":     r10[6] = false; break;
                case "cbLeft":     r10[7] = false; break;

                case "cbL1":       r11[2] = false; break;
                case "cbR1":       r11[3] = false; break;
                case "cbTriangle": r11[4] = false; break;
                case "cbCircle":   r11[5] = false; break;
                case "cbCross":    r11[6] = false; break;
                case "cbSquare":   r11[7] = false; break;

                case "cbPS":  rx[0] = 0;   break;
                case "cbLX":  rx[1] = 127; break;
                case "cbLY":  rx[2] = 123; break;
                case "cbRX":  rx[3] = 125; break;
                case "cbRY":  rx[4] = 127; break;
                case "cbL2":  rx[5] = 0;   break;
                case "cbR2":  rx[6] = 0;   break;
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


            Report[26] = data[8]; //Left Trigger


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

            //Guide
            var Guide = data[7] & (1 << 1 - 1);
            Report[12] = (byte)(Guide != 0 ? 0xFF : 0x00);

            #region watch sixaxis data
            Global.setSixaxisData(new byte[] { 
                data[20], data[24]
            });
            #endregion

            return Report;

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
            return "Controller " + (deviceNum + 1) + ": " + "Battery = "+charge+"%," +" Touchpad Enabled = " + isTouchEnabled + (isUSB ? " (USB)" : " (BT)");
        }

    }
    

 

}
