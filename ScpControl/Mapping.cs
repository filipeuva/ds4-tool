using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ScpControl
{
    class Mapping
    {
        public static DS4State mapButtons(DS4State cState)
        {
            DS4State MappedState = cState;
            foreach (KeyValuePair<DS4Controls, X360Controls> customButton in Global.getCustomButtons())
            {

                switch (customButton.Value)
                {
                    case X360Controls.A:
                        MappedState.Cross = getBoolMapping(customButton.Key, cState);
                        break;
                    case X360Controls.B:
                        MappedState.Circle = getBoolMapping(customButton.Key, cState);
                        break;
                    case X360Controls.X:
                        MappedState.Square = getBoolMapping(customButton.Key, cState);
                        break;
                    case X360Controls.Y:
                        MappedState.Triangle = getBoolMapping(customButton.Key, cState);
                        break;
                    case X360Controls.LB:
                        MappedState.L1 = getBoolMapping(customButton.Key, cState);
                        break;
                    case X360Controls.LS:
                        MappedState.L3 = getBoolMapping(customButton.Key, cState);
                        break;
                    case X360Controls.RB:
                        MappedState.R1 = getBoolMapping(customButton.Key, cState);
                        break;
                    case X360Controls.RS:
                        MappedState.R3 = getBoolMapping(customButton.Key, cState);
                        break;
                    case X360Controls.DpadUp:
                        MappedState.DpadUp = getBoolMapping(customButton.Key, cState);
                        break;
                    case X360Controls.DpadDown:
                        MappedState.DpadDown = getBoolMapping(customButton.Key, cState);
                        break;
                    case X360Controls.DpadLeft:
                        MappedState.DpadLeft = getBoolMapping(customButton.Key, cState);
                        break;
                    case X360Controls.DpadRight:
                        MappedState.DpadRight = getBoolMapping(customButton.Key, cState);
                        break;
                    case X360Controls.Guide:
                        MappedState.PS = getBoolMapping(customButton.Key, cState);
                        break;
                    case X360Controls.Back:
                        MappedState.Share = getBoolMapping(customButton.Key, cState);
                        break;
                    case X360Controls.Start:
                        MappedState.Options = getBoolMapping(customButton.Key, cState);
                        break;
                    case X360Controls.LXNeg:
                        MappedState.LX = getXYAxisMapping(customButton.Key, cState);
                        break;
                    case X360Controls.LYNeg:
                        MappedState.LY = getXYAxisMapping(customButton.Key, cState);
                        break;
                    case X360Controls.LXPos:
                        MappedState.LX = getXYAxisMapping(customButton.Key, cState);
                        break;
                    case X360Controls.RXNeg:
                        MappedState.RX = getXYAxisMapping(customButton.Key, cState);
                        break;
                    case X360Controls.RYNeg:
                        MappedState.RY = getXYAxisMapping(customButton.Key, cState);
                        break;
                    case X360Controls.LYPos:
                        MappedState.LY = getXYAxisMapping(customButton.Key, cState, true);
                        break;
                    case X360Controls.RXPos:
                        MappedState.RX = getXYAxisMapping(customButton.Key, cState, true);
                        break;
                    case X360Controls.RYPos:
                        MappedState.RY = getXYAxisMapping(customButton.Key, cState, true);
                        break;
                    case X360Controls.LT:
                        MappedState.L2 = getXYAxisMapping(customButton.Key, cState,true);
                        break;
                    case X360Controls.RT:
                        MappedState.R2 = getXYAxisMapping(customButton.Key, cState,true);
                        break;
                }
            }
            return MappedState;
        }

        public static byte getByteMapping(DS4Controls control, DS4State cState)
        {
            switch (control)
            {
                case DS4Controls.Share: return (byte)(cState.Share ? 255 : 0);
                case DS4Controls.Options: return (byte)(cState.Options ? 255 : 0);
                case DS4Controls.L1: return (byte)(cState.L1 ? 255 : 0);
                case DS4Controls.R1: return (byte)(cState.R1 ? 255 : 0);
                case DS4Controls.L3: return (byte)(cState.L3 ? 255 : 0);
                case DS4Controls.R3: return (byte)(cState.R3 ? 255 : 0);
                case DS4Controls.DpadUp: return (byte)(cState.DpadUp ? 255 : 0);
                case DS4Controls.DpadDown: return (byte)(cState.DpadDown ? 255 : 0);
                case DS4Controls.DpadLeft: return (byte)(cState.DpadLeft ? 255 : 0);
                case DS4Controls.DpadRight: return (byte)(cState.DpadRight ? 255 : 0);
                case DS4Controls.TouchButton: return (byte)(cState.TouchButton ? 255 : 0);
                case DS4Controls.PS: return (byte)(cState.PS ? 255 : 0);
                case DS4Controls.Cross: return (byte)(cState.Cross ? 255 : 0);
                case DS4Controls.Square: return (byte)(cState.Square ? 255 : 0);
                case DS4Controls.Triangle: return (byte)(cState.Triangle ? 255 : 0);
                case DS4Controls.Circle: return (byte)(cState.Circle ? 255 : 0);
                case DS4Controls.LXNeg: return cState.LX;
                case DS4Controls.LYNeg: return cState.LY;
                case DS4Controls.RXNeg: return cState.RX;
                case DS4Controls.RYNeg: return cState.RY;
                case DS4Controls.LXPos: return (byte)(cState.LX - 127 < 0 ? 0 : (cState.LX - 127));
                case DS4Controls.LYPos: return (byte)(cState.LY - 123 < 0 ? 0 : (cState.LY - 123));
                case DS4Controls.RXPos: return (byte)(cState.RX - 125 < 0 ? 0 : (cState.RX - 125));
                case DS4Controls.RYPos: return (byte)(cState.RY - 127 < 0 ? 0 : (cState.RY - 127));
                case DS4Controls.L2: return cState.L2;
                case DS4Controls.R2: return cState.R2;
            }
            return 0;
        }

        public static bool getBoolMapping(DS4Controls control, DS4State cState)
        {
            switch (control)
            {
                case DS4Controls.Share: return cState.Share;
                case DS4Controls.Options: return cState.Options;
                case DS4Controls.L1: return cState.L1;
                case DS4Controls.R1: return cState.R1;
                case DS4Controls.L3: return cState.L3;
                case DS4Controls.R3: return cState.R3;
                case DS4Controls.DpadUp: return cState.DpadUp;
                case DS4Controls.DpadDown: return cState.DpadDown;
                case DS4Controls.DpadLeft: return cState.DpadLeft;
                case DS4Controls.DpadRight: return cState.DpadRight;
                case DS4Controls.TouchButton: return cState.TouchButton;
                case DS4Controls.PS: return cState.PS;
                case DS4Controls.Cross: return cState.Cross;
                case DS4Controls.Square: return cState.Square;
                case DS4Controls.Triangle: return cState.Triangle;
                case DS4Controls.Circle: return cState.Circle;
                case DS4Controls.LXNeg: return cState.LX < 117;
                case DS4Controls.LYNeg: return cState.LY < 113;
                case DS4Controls.RXNeg: return cState.RX < 115;
                case DS4Controls.RYNeg: return cState.RY < 117;
                case DS4Controls.LXPos: return cState.LX > 137;
                case DS4Controls.LYPos: return cState.LY > 133;
                case DS4Controls.RXPos: return cState.RX > 135;
                case DS4Controls.RYPos: return cState.RY > 137;
                case DS4Controls.L2: return cState.L2 > 100;
                case DS4Controls.R2: return cState.R2 > 100;
            }
            return false;
        }

        public static byte getXYAxisMapping(DS4Controls control, DS4State cState, bool alt = false)
        {
            byte trueVal = 0;
            byte falseVal = 127;
            if (alt)
            {
                trueVal = 255;
            }
            switch (control)
            {
                case DS4Controls.Share: return (byte)(cState.Share ? trueVal : falseVal);
                case DS4Controls.Options: return (byte)(cState.Options ? trueVal : falseVal);
                case DS4Controls.L1: return (byte)(cState.L1 ? trueVal : falseVal);
                case DS4Controls.R1: return (byte)(cState.R1 ? trueVal : falseVal);
                case DS4Controls.L3: return (byte)(cState.L3 ? trueVal : falseVal);
                case DS4Controls.R3: return (byte)(cState.R3 ? trueVal : falseVal);
                case DS4Controls.DpadUp: return (byte)(cState.DpadUp ? trueVal : falseVal);
                case DS4Controls.DpadDown: return (byte)(cState.DpadDown ? trueVal : falseVal);
                case DS4Controls.DpadLeft: return (byte)(cState.DpadLeft ? trueVal : falseVal);
                case DS4Controls.DpadRight: return (byte)(cState.DpadRight ? trueVal : falseVal);
                case DS4Controls.TouchButton: return (byte)(cState.TouchButton ? trueVal : falseVal);
                case DS4Controls.PS: return (byte)(cState.PS ? trueVal : falseVal);
                case DS4Controls.Cross: return (byte)(cState.Cross ? trueVal : falseVal);
                case DS4Controls.Square: return (byte)(cState.Square ? trueVal : falseVal);
                case DS4Controls.Triangle: return (byte)(cState.Triangle ? trueVal : falseVal);
                case DS4Controls.Circle: return (byte)(cState.Circle ? trueVal : falseVal);
                case DS4Controls.L2: return (byte)(cState.L2 == 255 ? trueVal : falseVal);
                case DS4Controls.R2: return (byte)(cState.R2 == 255 ? trueVal : falseVal);
            }

            if (alt)
            {
                switch (control)
                {
                    case DS4Controls.LXNeg: return cState.LX;
                    case DS4Controls.LYNeg: return cState.LY;
                    case DS4Controls.RXNeg: return cState.RX;
                    case DS4Controls.RYNeg: return cState.RY;
                    case DS4Controls.LXPos: return (byte)(255 - cState.LX);
                    case DS4Controls.LYPos: return (byte)(255 - cState.LY);
                    case DS4Controls.RXPos: return (byte)(255 - cState.RX);
                    case DS4Controls.RYPos: return (byte)(255 - cState.RY);
                }
            }
            else
            {
                switch (control)
                {
                    case DS4Controls.LXNeg: return (byte)(255 - cState.LX);
                    case DS4Controls.LYNeg: return (byte)(255 - cState.LY);
                    case DS4Controls.RXNeg: return (byte)(255 - cState.RX);
                    case DS4Controls.RYNeg: return (byte)(255 - cState.RY);
                    case DS4Controls.LXPos: return cState.LX;
                    case DS4Controls.LYPos: return cState.LY;
                    case DS4Controls.RXPos: return cState.RX;
                    case DS4Controls.RYPos: return cState.RY;
                }
            }
            return 0;
        }
    }
}
