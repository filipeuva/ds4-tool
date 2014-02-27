using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
namespace ScpControl
{
    public class TouchpadEventArgs : EventArgs
    {
        public readonly Touch[] touches = null;
        public readonly bool touchButtonPressed;
        public TouchpadEventArgs(bool tButtonDown, Touch t0, Touch t1 = null)
        {
            if (t1 != null)
            {
                touches = new Touch[2];
                touches[0] = t0;
                touches[1] = t1;
            }
            else if (t0 != null)
            {
                touches = new Touch[1];
                touches[0] = t0;
            }
            touchButtonPressed = tButtonDown;
        }
    }

    public class Touch
    {
        public readonly int hwX, hwY, deltaX, deltaY;
        public readonly byte touchID;
        public readonly Touch previousTouch;
        public Touch(int X, int Y,  byte tID, Touch prevTouch = null)
        {
            hwX = X;
            hwY = Y;
            touchID = tID;
            previousTouch = prevTouch;
            if (previousTouch != null)
            {
                deltaX = X - previousTouch.hwX;
                deltaY = Y - previousTouch.hwY;
            }
        }
    }

    class Touchpad
    {
        public event EventHandler<TouchpadEventArgs> TouchesBegan = null;
        public event EventHandler<TouchpadEventArgs> TouchesMoved = null;
        public event EventHandler<TouchpadEventArgs> TouchesEnded = null;
        public event EventHandler<TouchpadEventArgs> TouchButtonDown = null;
        public event EventHandler<TouchpadEventArgs> TouchButtonUp = null;
        internal static int TOUCHPAD_DATA_OFFSET = 35;
        internal static int lastTouchPadX, lastTouchPadY,
            lastTouchPadX2, lastTouchPadY2;
        internal static bool lastTouchPadIsDown;
        internal static bool lastIsActive;
        internal static bool lastIsActive2;
        internal static byte lastTouchID, lastTouchID2;
        internal static int ticks = -1; //incremented every input report, in the future use something more reliable
        internal readonly int deviceNum;
        public Touchpad(int controllerID)
        {
            deviceNum = controllerID;
        }

        public void handleTouchpad(byte[] data, bool touchPadIsDown)
        {

            bool _isActive = (data[0 + TOUCHPAD_DATA_OFFSET] >> 7) != 0 ? false : true; // >= 1 touch detected
            bool _isActive2 = (data[4 + TOUCHPAD_DATA_OFFSET] >> 7) != 0 ? false : true; // > 1 touch detected
            byte touchID = (byte)(data[0 + TOUCHPAD_DATA_OFFSET] & 0x7F);
            byte touchID2 = (byte)(data[4 + TOUCHPAD_DATA_OFFSET] & 0x7F);
            int currentX = data[1 + TOUCHPAD_DATA_OFFSET] + ((data[2 + TOUCHPAD_DATA_OFFSET] & 0xF) * 255);
            int currentY = ((data[2 + TOUCHPAD_DATA_OFFSET] & 0xF0) >> 4) + (data[3 + TOUCHPAD_DATA_OFFSET] * 16);
            //add secondary touch data
            int currentX2 = data[5 + TOUCHPAD_DATA_OFFSET] + ((data[6 + TOUCHPAD_DATA_OFFSET] & 0xF) * 255);
            int currentY2 = ((data[6 + TOUCHPAD_DATA_OFFSET] & 0xF0) >> 4) + (data[7 + TOUCHPAD_DATA_OFFSET] * 16);

            if (_isActive)
            {

                if (!lastTouchPadIsDown && touchPadIsDown && TouchButtonDown != null)
                {
                    TouchpadEventArgs args = null;
                    Touch t0 = new Touch(currentX, currentY, touchID);
                    if (_isActive2)
                    {
                        Touch t1 = new Touch(currentX2, currentY2, touchID2);
                        args = new TouchpadEventArgs(touchPadIsDown, t0, t1);
                    }
                    else
                        args = new TouchpadEventArgs(touchPadIsDown, t0);
                    TouchButtonDown(this, args);
                }
                else if (lastTouchPadIsDown && !touchPadIsDown && TouchButtonUp != null)
                {
                    TouchpadEventArgs args = null;
                    Touch t0 = new Touch(currentX, currentY, touchID);
                    if (_isActive2)
                    {
                        Touch t1 = new Touch(currentX2, currentY2, touchID2);
                        args = new TouchpadEventArgs(touchPadIsDown, t0, t1);
                    }
                    else
                        args = new TouchpadEventArgs(touchPadIsDown, t0);
                    TouchButtonUp(this, args);
                }

                if (!lastIsActive || (_isActive2 && !lastIsActive2))
                {
                    if (TouchesBegan != null)
                    {
                        TouchpadEventArgs args = null;
                        Touch t0 = new Touch(currentX, currentY, touchID);
                        if (_isActive2 && !lastIsActive2)
                        {
                            Touch t1 = new Touch(currentX2, currentY2, touchID2);
                            args = new TouchpadEventArgs(touchPadIsDown, t0, t1);
                        }
                        else
                            args = new TouchpadEventArgs(touchPadIsDown, t0);
                        TouchesBegan(this, args);
                    }
                }
                else if (lastIsActive)
                {
                    if (TouchesMoved != null)
                    {
                        TouchpadEventArgs args = null;

                        Touch t0Prev = new Touch(lastTouchPadX, lastTouchPadY, lastTouchID);
                        Touch t0 = new Touch(currentX, currentY, touchID, t0Prev);
                        if (_isActive && _isActive2)
                        {
                            Touch t1Prev = new Touch(lastTouchPadX2, lastTouchPadY2, lastTouchID2);
                            Touch t1 = new Touch(currentX2, currentY2, touchID2, t1Prev);
                            args = new TouchpadEventArgs(touchPadIsDown, t0, t1);
                        }
                        else
                            args = new TouchpadEventArgs(touchPadIsDown, t0);
                        TouchesMoved(this, args);
                    }
                }
                else
                { 
                }

                lastTouchPadX = currentX;
                lastTouchPadY = currentY;
                //secondary touch data
                lastTouchPadX2 = currentX2;
                lastTouchPadY2 = currentY2;
                lastTouchPadIsDown = touchPadIsDown;
            }
            else
            {
                if (lastIsActive)
                {
                    if (TouchesEnded != null)
                    {
                        TouchpadEventArgs args = null;
                        Touch t0 = new Touch(currentX, currentY, touchID);
                        if (lastIsActive2)
                        {
                            Touch t1 = new Touch(currentX2, currentY2, touchID2);
                            args = new TouchpadEventArgs(touchPadIsDown, t0, t1);
                        }
                        else
                            args = new TouchpadEventArgs(touchPadIsDown, t0);
                        TouchesEnded(this, args);
                    }
                }

                if (touchPadIsDown && !lastTouchPadIsDown)
                {
                   TouchButtonDown(this, new TouchpadEventArgs(touchPadIsDown, null, null));
                }
                else if (!touchPadIsDown && lastTouchPadIsDown)
                {
                    TouchButtonUp(this, new TouchpadEventArgs(touchPadIsDown, null, null));
                }
            }

            lastIsActive = _isActive;
            lastIsActive2 = _isActive2;
            lastTouchID = touchID;
            lastTouchID2 = touchID2;
            lastTouchPadIsDown = touchPadIsDown;
        }

     
    }
}
