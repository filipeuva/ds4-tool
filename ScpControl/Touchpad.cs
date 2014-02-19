using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
namespace ScpControl
{
    class Touchpad
    {
        internal static int TOUCHPAD_DATA_OFFSET = 35;
        internal static int lastTouchPadX, lastTouchPadY,
            lastTouchPadX2, lastTouchPadY2;
        internal static bool lastTouchPadIsDown;
        internal static bool lastIsActive;
        internal static byte lastTouchID;
        internal static INPUT[] sendInputs = new INPUT[1]; // will allow for keyboard + mouse/tablet input within one SendInput call
        internal static int ticks = -1; //incremented every input report, in the future use something more reliable
        public static void handleTouchpad(byte[] data, bool touchPadIsDown)
        {
            int mouseDeltaX = 0, mouseDeltaY = 0,
                mouseDeltaX2 = 0, mouseDeltaY2 = 0;

            bool _isActive = (data[0 + TOUCHPAD_DATA_OFFSET] >> 7) != 0 ? false : true; // >= 1 touch detected
            // increased scope of _isActive2 
            bool _isActive2 = (data[4 + TOUCHPAD_DATA_OFFSET] >> 7) != 0 ? false : true; // > 1 touch detected
            byte touchID = (byte)(data[0 + TOUCHPAD_DATA_OFFSET] & 0x7F);
     
            if (_isActive)
            {
                ticks++;
                int currentX = data[1 + TOUCHPAD_DATA_OFFSET] + ((data[2 + TOUCHPAD_DATA_OFFSET] & 0xF) * 255);
                int currentY = ((data[2 + TOUCHPAD_DATA_OFFSET] & 0xF0) >> 4) + (data[3 + TOUCHPAD_DATA_OFFSET] * 16);
                //add secondary touch data
                int currentX2 = data[5 + TOUCHPAD_DATA_OFFSET] + ((data[6 + TOUCHPAD_DATA_OFFSET] & 0xF) * 255);
                int currentY2 = ((data[6 + TOUCHPAD_DATA_OFFSET] & 0xF0) >> 4) + (data[7 + TOUCHPAD_DATA_OFFSET] * 16);

                if (lastIsActive)
                {

                    double sensitivity = Global.getTouchSensitivity(0) / 100.0;
                    mouseDeltaX = (int)(sensitivity * (currentX - lastTouchPadX));
                    mouseDeltaY = (int)(sensitivity * (currentY - lastTouchPadY));
                    //add mouseDelta for secondary touch data
                    mouseDeltaX2 = (int)(sensitivity * (currentX2 - lastTouchPadX2));
                    mouseDeltaY2 = (int)(sensitivity * (currentY2 - lastTouchPadY2));

                    //prevent jitter of  the cursor
                    if (Math.Abs(mouseDeltaX) < 5 && Math.Abs(mouseDeltaY) < 5 && ticks<100)
                    {
                        mouseDeltaX = 0;
                        mouseDeltaY = 0;
                    }

                    if (touchPadIsDown)
                        {
                            if (!lastTouchPadIsDown)
                            {
                                mouseDeltaX = 0;
                                mouseDeltaY = 0;
                                //own right click (more than 1 touch detected) - if enabled.
                                //if its right corner do a right click (resolution of touchpad values is ~ 1920x1080)
                                if ((Global.getTwoFingerRC(0) && _isActive2)
                                            || (!Global.getTwoFingerRC(0)
                                            && currentX > 1500 && currentY > 600))
                                    performRightClick();
                                //perform a mouse down event when touchpad pressed
                                else LeftMouseDown();
                            }
                        }
                    //perform a mouse up event when touchpad released
                    else if (lastTouchPadIsDown)
                    {
                        mouseDeltaX = 0;
                        mouseDeltaY = 0;
                        LeftMouseUp();
                    }

                    //either zoom or scroll, scroll has X/2/Y/2 within 400 points 
                    if (_isActive2 &&
                        Math.Abs(currentX - currentX2) < 900
                        && Math.Abs(currentY - currentY2) < 300)
                    {
                        //mouse wheel 120 == 1 wheel click
                        int rotation = (int)(-0.6 * Global.getScrollSensitivity(0));
                        if (mouseDeltaY >= 5)
                        {
                            MouseWheel(rotation);
                        }
                        else if (mouseDeltaY <= -5)
                        {
                            MouseWheel(-rotation);
                        }
                        mouseDeltaY = 0;
                        mouseDeltaX = 0;

                    }
                }

                lastTouchPadX = currentX;
                lastTouchPadY = currentY;
                //secondary touch data
                lastTouchPadX2 = currentX2;
                lastTouchPadY2 = currentY2;
                
                lastTouchPadIsDown = touchPadIsDown;
            }
            else // finger(s) lifted from touchpad while virtual mouse button(s) clicked
            {
                // Click trackpad's top edge (no touch movement) - configurable
                if (touchPadIsDown)
                    if (!lastTouchPadIsDown)
                    {
                        ushort key = Global.getCustomKey("cbPad");
                        //Whatever key the config asks for.
                        if (key != 0)
                            performKeyPress(key);
                    }
                // Set new last touchpad down value
                lastTouchPadIsDown = touchPadIsDown;
                if (!lastIsActive) // neither active before or now
                    return;
                if (lastIsActive)
                {
                    //should be configurable (was 100)
                    //was a tap perform mouse left click
                    if (ticks < Global.getTapSensitivity(0))
                    {
                        performLeftClick();
                    }
                }
            }

            if (lastTouchID != touchID)
            {
                lastTouchID = touchID;
                ticks = 0;
            }

            lastIsActive = _isActive;

            MoveCursorBy(mouseDeltaX, mouseDeltaY);

        }

        static void MoveCursorBy(int x, int y)
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

        static void MouseWheel(int amount)
        {
            sendInputs[0].Type = INPUT_MOUSE;
            sendInputs[0].Data.Mouse.ExtraInfo = IntPtr.Zero;
            sendInputs[0].Data.Mouse.Flags = MOUSEEVENTF_WHEEL;
            sendInputs[0].Data.Mouse.MouseData = (uint)amount;
            sendInputs[0].Data.Mouse.Time = 0;
            sendInputs[0].Data.Mouse.X = 0;
            sendInputs[0].Data.Mouse.Y = 0;
            uint result = SendInput(1, sendInputs, Marshal.SizeOf(sendInputs[0]));
        }

        static void LeftMouseDown()
        {
            sendInputs[0].Type = INPUT_MOUSE;
            sendInputs[0].Data.Mouse.ExtraInfo = IntPtr.Zero;
            sendInputs[0].Data.Mouse.Flags = MOUSEEVENTF_LEFTDOWN ;
            sendInputs[0].Data.Mouse.MouseData = 0;
            sendInputs[0].Data.Mouse.Time = 0;
            sendInputs[0].Data.Mouse.X = 0;
            sendInputs[0].Data.Mouse.Y = 0;
            uint result = SendInput(1, sendInputs, Marshal.SizeOf(sendInputs[0]));
        }

        static void LeftMouseUp()
        {
            sendInputs[0].Type = INPUT_MOUSE;
            sendInputs[0].Data.Mouse.ExtraInfo = IntPtr.Zero;
            sendInputs[0].Data.Mouse.Flags = MOUSEEVENTF_LEFTUP;
            sendInputs[0].Data.Mouse.MouseData = 0;
            sendInputs[0].Data.Mouse.Time = 0;
            sendInputs[0].Data.Mouse.X = 0;
            sendInputs[0].Data.Mouse.Y = 0;
            uint result = SendInput(1, sendInputs, Marshal.SizeOf(sendInputs[0]));
        }

        public static void performLeftClick()
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

        static void performRightClick()
        {
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

        public static void performKeyPress(ushort key)
        {
            sendInputs[0].Type = INPUT_KEYBOARD;
            sendInputs[0].Data.Keyboard.ExtraInfo = IntPtr.Zero;
            sendInputs[0].Data.Keyboard.Flags = 0;
            sendInputs[0].Data.Keyboard.Scan = 0;
            sendInputs[0].Data.Keyboard.Time = 0;
            sendInputs[0].Data.Keyboard.Vk = key;
            uint result = SendInput(1, sendInputs, Marshal.SizeOf(sendInputs[0]));
        }

        public static void performKeyRelease(ushort key)
        {
            sendInputs[0].Type = INPUT_KEYBOARD;
            sendInputs[0].Data.Keyboard.ExtraInfo = IntPtr.Zero;
            sendInputs[0].Data.Keyboard.Flags = KEYEVENTF_KEYUP;
            sendInputs[0].Data.Keyboard.Scan = 0;
            sendInputs[0].Data.Keyboard.Time = 0;
            sendInputs[0].Data.Keyboard.Vk = key;
            uint result = SendInput(1, sendInputs, Marshal.SizeOf(sendInputs[0]));
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
            KEYEVENTF_KEYUP = 2, MOUSEEVENTF_WHEEL = 0x0800;

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint numberOfInputs, INPUT[] inputs, int sizeOfInputs);
    }
}
