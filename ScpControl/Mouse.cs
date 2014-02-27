using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
namespace ScpControl
{
    class Mouse
    {
        private static INPUT[] sendInputs = new INPUT[2]; // will allow for keyboard + mouse/tablet input within one SendInput call, or two mouse events
        private DateTime pastTime;
        private Touch firstTouch;
        private int deviceNum;
        private bool rightClick = false;
        public Mouse(int deviceID)
        {
            deviceNum = deviceID;
        }
        void  MoveCursorBy(int x, int y)
        {
            if (x != 0 || y != 0)
            {
                sendInputs[0].Type = INPUT_MOUSE;
                sendInputs[0].Data.Mouse.ExtraInfo = IntPtr.Zero;
                sendInputs[0].Data.Mouse.Flags = MOUSEEVENTF_MOVE;
                sendInputs[0].Data.Mouse.MouseData = 0;
                sendInputs[0].Data.Mouse.Time = 0;
                sendInputs[0].Data.Mouse.X = x;
                sendInputs[0].Data.Mouse.Y = y;
                uint result = SendInput(1, sendInputs, Marshal.SizeOf(sendInputs[0]));
            }
        }

        public void MouseWheel(int vertical, int horizontal)
        {
            uint inputs = 0;
            if (vertical != 0)
            {
                sendInputs[inputs].Type = INPUT_MOUSE;
                sendInputs[inputs].Data.Mouse.ExtraInfo = IntPtr.Zero;
                sendInputs[inputs].Data.Mouse.Flags = MOUSEEVENTF_WHEEL;
                sendInputs[inputs].Data.Mouse.MouseData = (uint)vertical;
                sendInputs[inputs].Data.Mouse.Time = 0;
                sendInputs[inputs].Data.Mouse.X = 0;
                sendInputs[inputs].Data.Mouse.Y = 0;
                inputs++;
            }
            if (horizontal != 0)
            {
                sendInputs[inputs].Type = INPUT_MOUSE;
                sendInputs[inputs].Data.Mouse.ExtraInfo = IntPtr.Zero;
                sendInputs[inputs].Data.Mouse.Flags = MOUSEEVENTF_HWHEEL;
                sendInputs[inputs].Data.Mouse.MouseData = (uint)horizontal;
                sendInputs[inputs].Data.Mouse.Time = 0;
                sendInputs[inputs].Data.Mouse.X = 0;
                sendInputs[inputs].Data.Mouse.Y = 0;
                inputs++;
            }
            SendInput(inputs, sendInputs, (int)inputs * Marshal.SizeOf(sendInputs[0]));
        }

        public void MouseEvent(uint mouseButton)
        {
            sendInputs[0].Type = INPUT_MOUSE;
            sendInputs[0].Data.Mouse.ExtraInfo = IntPtr.Zero;
            sendInputs[0].Data.Mouse.Flags = mouseButton;
            sendInputs[0].Data.Mouse.MouseData = 0;
            sendInputs[0].Data.Mouse.Time = 0;
            sendInputs[0].Data.Mouse.X = 0;
            sendInputs[0].Data.Mouse.Y = 0;
            uint result = SendInput(1, sendInputs, Marshal.SizeOf(sendInputs[0]));
        }

        public void performLeftClick()
        {
            sendInputs[0].Type = INPUT_MOUSE;
            sendInputs[0].Data.Mouse.ExtraInfo = IntPtr.Zero;
            sendInputs[0].Data.Mouse.Flags = 0;
            sendInputs[0].Data.Mouse.Flags |= MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP;
            sendInputs[0].Data.Mouse.MouseData = 0;
            sendInputs[0].Data.Mouse.Time = 0;
            sendInputs[0].Data.Mouse.X = 0;
            sendInputs[0].Data.Mouse.Y = 0;
            uint result = SendInput(1, sendInputs, Marshal.SizeOf(sendInputs[0]));
        }

        public void performRightClick()
        {
            rightClick = true;
            sendInputs[0].Type = INPUT_MOUSE;
            sendInputs[0].Data.Mouse.ExtraInfo = IntPtr.Zero;
            sendInputs[0].Data.Mouse.Flags = 0;
            sendInputs[0].Data.Mouse.Flags |= MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP;
            sendInputs[0].Data.Mouse.MouseData = 0;
            sendInputs[0].Data.Mouse.Time = 0;
            sendInputs[0].Data.Mouse.X = 0;
            sendInputs[0].Data.Mouse.Y = 0;
            uint result = SendInput(1, sendInputs, Marshal.SizeOf(sendInputs[0]));
        }

        public void performMiddleClick()
        {
            sendInputs[0].Type = INPUT_MOUSE;
            sendInputs[0].Data.Mouse.ExtraInfo = IntPtr.Zero;
            sendInputs[0].Data.Mouse.Flags = 0;
            sendInputs[0].Data.Mouse.Flags |= MOUSEEVENTF_MIDDLEDOWN | MOUSEEVENTF_MIDDLEUP;
            sendInputs[0].Data.Mouse.MouseData = 0;
            sendInputs[0].Data.Mouse.Time = 0;
            sendInputs[0].Data.Mouse.X = 0;
            sendInputs[0].Data.Mouse.Y = 0;
            uint result = SendInput(1, sendInputs, Marshal.SizeOf(sendInputs[0]));
        }

        public void touchesMoved(object sender, TouchpadEventArgs arg)
        {
            if (arg.touches.Length == 1)
            {
                double sensitivity = Global.getTouchSensitivity(deviceNum) / 100.0;
                int mouseDeltaX = (int)(sensitivity * (arg.touches[0].deltaX));
                int mouseDeltaY = (int)(sensitivity * (arg.touches[0].deltaY));
                MoveCursorBy(mouseDeltaX, mouseDeltaY);
            }
            else if (arg.touches.Length == 2)
            {
                Touch lastT0 = arg.touches[0].previousTouch;
                Touch lastT1 = arg.touches[1].previousTouch;
                Touch T0 = arg.touches[0];
                Touch T1 = arg.touches[1];

                //mouse wheel 120 == 1 wheel click according to Windows API
                int lastMidX = (lastT0.hwX + lastT1.hwX) / 2, lastMidY = (lastT0.hwY + lastT1.hwY) / 2,
                    currentMidX = (T0.hwX + T1.hwX) / 2, currentMidY = (T0.hwY + T1.hwY) / 2; // XXX Will controller swap touch IDs?
                double coefficient = Global.getScrollSensitivity(deviceNum);
                // Adjust for touch distance: "standard" distance is 960 pixels, i.e. half the width.  Scroll farther if fingers are farther apart, and vice versa, in linear proportion.
                double touchXDistance = T1.hwX - T0.hwX, touchYDistance = T1.hwY - T0.hwY, touchDistance = Math.Sqrt(touchXDistance * touchXDistance + touchYDistance * touchYDistance);
                coefficient *= touchDistance / 960.0;
                MouseWheel((int)(coefficient * (lastMidY - currentMidY)), (int)(coefficient * (currentMidX - lastMidX)));
            }
        }

        public void touchesBegan(object sender, TouchpadEventArgs arg)
        {
            pastTime = DateTime.Now;
            firstTouch = arg.touches[0];
        }

        public void touchesEnded(object sender, TouchpadEventArgs arg)
        {
            if (Global.getTapSensitivity(deviceNum) != 0)
            {
                DateTime test = DateTime.Now;
                if (test <= (pastTime + TimeSpan.FromMilliseconds((double)Global.getTapSensitivity(deviceNum) * 2)) && !arg.touchButtonPressed)
                {
                    if (Math.Abs(firstTouch.hwX - arg.touches[0].hwX) < 10 &&
                        Math.Abs(firstTouch.hwY - arg.touches[0].hwY) < 10)
                        performLeftClick();
                }
            }
        }

        public void touchButtonUp(object sender, TouchpadEventArgs arg)
        {
            if (arg.touches == null)
            {
                //No touches, finger on upper portion of touchpad
                mapTouchPad(DS4Controls.TouchUpper,true);
            }
            else if (arg.touches.Length > 1)
                mapTouchPad(DS4Controls.TouchMulti, true);
            else if (!rightClick && arg.touches.Length == 1 && !mapTouchPad(DS4Controls.TouchButton, true))
            {
                MouseEvent(MOUSEEVENTF_LEFTUP);
            }
        }

        public void touchButtonDown(object sender, TouchpadEventArgs arg)
        {
            if (arg.touches == null)
            {
                //No touches, finger on upper portion of touchpad
                if(!mapTouchPad(DS4Controls.TouchUpper))
                    performMiddleClick();
            }
            else if (!Global.getLowerRCOff(deviceNum) && arg.touches[0].hwX > (1920 * 3)/4
                && arg.touches[0].hwY > (960 * 3)/4)
            {
                performRightClick();
            }
            else if (arg.touches.Length>1 && !mapTouchPad(DS4Controls.TouchMulti))
            {
                performRightClick();
            }
            else if (arg.touches.Length==1 && !mapTouchPad(DS4Controls.TouchButton))
            {
                rightClick = false;
                MouseEvent(MOUSEEVENTF_LEFTDOWN);
            }
        }


        public void performSCKeyPress(ushort key)
        {
            sendInputs[0].Type = INPUT_KEYBOARD;
            sendInputs[0].Data.Keyboard.ExtraInfo = IntPtr.Zero;
            sendInputs[0].Data.Keyboard.Flags = KEYEVENTF_SCANCODE;
            sendInputs[0].Data.Keyboard.Scan = MapVirtualKey(key, MAPVK_VK_TO_VSC); ;
            sendInputs[0].Data.Keyboard.Time = 0;
            sendInputs[0].Data.Keyboard.Vk = key;
            uint result = SendInput(1, sendInputs, Marshal.SizeOf(sendInputs[0]));
        }

        public void performKeyPress(ushort key)
        {
            sendInputs[0].Type = INPUT_KEYBOARD;
            sendInputs[0].Data.Keyboard.ExtraInfo = IntPtr.Zero;
            sendInputs[0].Data.Keyboard.Flags = 0;
            sendInputs[0].Data.Keyboard.Scan = 0;
            sendInputs[0].Data.Keyboard.Time = 0;
            sendInputs[0].Data.Keyboard.Vk = key;
            uint result = SendInput(1, sendInputs, Marshal.SizeOf(sendInputs[0]));
        }

        public void performSCKeyRelease(ushort key)
        {
            sendInputs[0].Type = INPUT_KEYBOARD;
            sendInputs[0].Data.Keyboard.ExtraInfo = IntPtr.Zero;
            sendInputs[0].Data.Keyboard.Flags = KEYEVENTF_SCANCODE | KEYEVENTF_KEYUP;
            sendInputs[0].Data.Keyboard.Scan = MapVirtualKey(key, MAPVK_VK_TO_VSC);
            sendInputs[0].Data.Keyboard.Time = 0;
            sendInputs[0].Data.Keyboard.Vk = key;
            uint result = SendInput(1, sendInputs, Marshal.SizeOf(sendInputs[0]));
        }

        public void performKeyRelease(ushort key)
        {
            sendInputs[0].Type = INPUT_KEYBOARD;
            sendInputs[0].Data.Keyboard.ExtraInfo = IntPtr.Zero;
            sendInputs[0].Data.Keyboard.Flags = KEYEVENTF_KEYUP;
            sendInputs[0].Data.Keyboard.Scan = 0;
            sendInputs[0].Data.Keyboard.Time = 0;
            sendInputs[0].Data.Keyboard.Vk = key;
            uint result = SendInput(1, sendInputs, Marshal.SizeOf(sendInputs[0]));
        }

        bool mapTouchPad(DS4Controls padControl, bool release = false)
        {
            ushort key = Global.getCustomKey(padControl);
            if (key == 0)
                return false;
            else
            {
                DS4KeyType keyType = Global.getCustomKeyType(padControl);
                if (!release)
                    if (keyType.HasFlag(DS4KeyType.ScanCode))
                        performSCKeyPress(key);
                    else performKeyPress(key);
                else
                    if (!keyType.HasFlag(DS4KeyType.Repeat))
                        if (keyType.HasFlag(DS4KeyType.ScanCode))
                            performSCKeyRelease(key);
                        else performKeyRelease(key);
                return true;
            }
        }


        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/ms646270(v=vs.85).aspx
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct INPUT
        {
            public uint Type;
            public MOUSEKEYBDHARDWAREINPUT Data;
        }
        internal const uint INPUT_MOUSE = 0, INPUT_KEYBOARD = 1, INPUT_HARDWARE = 2;

        /// <summary>
        /// http://social.msdn.microsoft.com/Forums/en/csharplanguage/thread/f0e82d6e-4999-4d22-b3d3-32b25f61fb2a
        /// </summary>
        [StructLayout(LayoutKind.Explicit)]
        internal struct MOUSEKEYBDHARDWAREINPUT
        {
            [FieldOffset(0)]
            public HARDWAREINPUT Hardware;
            [FieldOffset(0)]
            public KEYBDINPUT Keyboard;
            [FieldOffset(0)]
            public MOUSEINPUT Mouse;
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/ms646310(v=vs.85).aspx
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct HARDWAREINPUT
        {
            public uint Msg;
            public ushort ParamL;
            public ushort ParamH;
        }

        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/ms646310(v=vs.85).aspx
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct KEYBDINPUT
        {
            public ushort Vk;
            public ushort Scan;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }

        /// <summary>
        /// http://social.msdn.microsoft.com/forums/en-US/netfxbcl/thread/2abc6be8-c593-4686-93d2-89785232dacd
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        internal struct MOUSEINPUT
        {
            public int X;
            public int Y;
            public uint MouseData;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }

        internal const uint MOUSEEVENTF_MOVE = 1, // just apply X/Y (delta due to not setting absolute flag)
            MOUSEEVENTF_LEFTDOWN = 2, MOUSEEVENTF_LEFTUP = 4,
            MOUSEEVENTF_RIGHTDOWN = 8, MOUSEEVENTF_RIGHTUP = 16,
            MOUSEEVENTF_MIDDLEDOWN = 32, MOUSEEVENTF_MIDDLEUP = 64,
            KEYEVENTF_KEYUP = 2, MOUSEEVENTF_WHEEL = 0x0800, MOUSEEVENTF_HWHEEL = 0x1000,
            KEYEVENTF_SCANCODE = 0x0008, MAPVK_VK_TO_VSC = 0;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint numberOfInputs, INPUT[] inputs, int sizeOfInputs);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern ushort MapVirtualKey(uint uCode, uint uMapType);
    }
}
