using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Reflection;
using System.Xml;
namespace ScpControl 
{
    public enum Ds3PadId :   byte { None = 0xFF, One = 0x00, Two = 0x01, Three = 0x02, Four = 0x03, All = 0x04 };

    public enum DS4Controls : byte { LXNeg, LXPos, LYNeg, LYPos, RXNeg, RXPos, RYNeg, RYPos, L1, L2, L3, R1, R2, R3, Square, Triangle, Circle, Cross, DpadUp, DpadRight, DpadDown, DpadLeft, PS, TouchButton, Share, Options };
    public enum X360Controls : byte { LXNeg, LXPos, LYNeg, LYPos, RXNeg, RXPos, RYNeg, RYPos, LB, LT, LS, RB, RT, RS, X, Y, B, A, DpadUp, DpadRight, DpadDown, DpadLeft, Guide, Back, Start, LeftMouse, RightMouse, MiddleMouse};
   
    public class DebugEventArgs   : EventArgs 
    {
        protected DateTime m_Time = DateTime.Now;
        protected String m_Data = String.Empty;

        public DebugEventArgs(String Data) 
        {
            m_Data = Data;
        }

        public DateTime Time 
        {
            get { return m_Time; }
        }

        public String Data 
        {
            get { return m_Data; }
        }
    }

    public class MappingDoneEventArgs : EventArgs
    {
        protected int deviceNum = -1;

        public MappingDoneEventArgs(int DeviceID)
        {
            deviceNum = DeviceID;
        }

        public int DeviceID
        {
            get { return deviceNum; }
        }
    }

    public class ReportEventArgs  : EventArgs 
    {
        protected Ds3PadId m_Pad = Ds3PadId.None;
        protected Byte[] m_Report = new Byte[64];

        public ReportEventArgs() 
        {
        }

        public ReportEventArgs(Ds3PadId Pad) 
        {
            m_Pad = Pad;
        }

        public Ds3PadId Pad 
        {
            get { return m_Pad; }
            set { m_Pad = value; }
        }

        public Byte[] Report 
        {
            get { return m_Report; }
        }
    }

    public class Global 
    {
        protected static BackingStore m_Config = new BackingStore();
        protected static Int32 m_IdleTimeout = 600000;

        public static ledColor loadColor(int device)
        {
           ledColor color = new ledColor();
           color.red = m_Config.m_Leds[device][0];
           color.green = m_Config.m_Leds[device][1];
           color.blue = m_Config.m_Leds[device][2];
           return color;
        }
        public static void saveColor(int device, byte red, byte green, byte blue)
        {
            m_Config.m_Leds[device][0] = red;
            m_Config.m_Leds[device][1] = green;
            m_Config.m_Leds[device][2] = blue;
        }

        public static byte loadRumbleBoost(int device)
        {
            return m_Config.m_Rumble[device];
        }
        public static void saveRumbleBoost(int device, byte boost)
        {
            m_Config.m_Rumble[device] = boost;

        }

        public static byte getTouchSensitivity(int device)
        {
            return m_Config.touchSensitivity[device];
        }
        public static void setTouchSensitivity(int device, byte sen)
        {
             m_Config.touchSensitivity[device] = sen;
        }

        public static void setFlashWhenLowBattery(int device, bool flash)
        {
            m_Config.flashLedLowBattery[device] = flash;

        }
        public static bool getFlashWhenLowBattery(int device)
        {
            return m_Config.flashLedLowBattery[device];

        }

        public static void setLedAsBatteryIndicator(int device, bool ledAsBattery)
        {
            m_Config.ledAsBattery[device] = ledAsBattery;

        }
        public static bool getLedAsBatteryIndicator(int device)
        {
            return m_Config.ledAsBattery[device];     
        }

        public static void setTouchEnabled(int device, bool touchEnabled)
        {
            m_Config.touchEnabled[device] = touchEnabled;

        }
        public static bool getTouchEnabled(int device)
        {
            return m_Config.touchEnabled[device];

        }

        public static void setUseExclusiveMode(bool exclusive)
        {
            m_Config.useExclusiveMode = exclusive;
        }
        public static bool getUseExclusiveMode()
        {
            return m_Config.useExclusiveMode;
        }
        
        // New settings
        public static void saveLowColor(int device, byte red, byte green, byte blue)
        {
            m_Config.m_LowLeds[device][0] = red;
            m_Config.m_LowLeds[device][1] = green;
            m_Config.m_LowLeds[device][2] = blue;
        }
        public static ledColor loadLowColor(int device)
        {
            ledColor color = new ledColor();
            color.red = m_Config.m_LowLeds[device][0];
            color.green = m_Config.m_LowLeds[device][1];
            color.blue = m_Config.m_LowLeds[device][2];
            return color;
        }
        public static void setTapSensitivity(int device, byte sen)
        {
            m_Config.tapSensitivity[device] = sen;
        }
        public static byte getTapSensitivity(int device)
        {
            return m_Config.tapSensitivity[device];
        }
        public static void setScrollSensitivity(int device, byte sen)
        {
            m_Config.scrollSensitivity[device] = sen;
        }
        public static byte getScrollSensitivity(int device)
        {
            return m_Config.scrollSensitivity[device];
        }
        public static void setTwoFingerRC(int device, bool twoFingerRC)
        {
            m_Config.twoFingerRC[device] = twoFingerRC;
        }
        public static bool getTwoFingerRC(int device)
        {
            return m_Config.twoFingerRC[device];
        }
        public static void setStartMinimized(bool startMinimized)
        {
            m_Config.startMinimized = startMinimized;
        }
        public static bool getStartMinimized()
        {
            return m_Config.startMinimized;
        }
        public static void setFormWidth(int size)
        {
            m_Config.formWidth = size;
        }
        public static int getFormWidth()
        {
            return m_Config.formWidth;
        }
        public static void setFormHeight(int size)
        {
            m_Config.formHeight = size;
        }
        public static int getFormHeight()
        {
            return m_Config.formHeight;
        }
        public static void setCustomMap(int device, string customMap)
        {
            m_Config.customMapPath[device] = customMap;
        }
        public static string getCustomMap(int device)
        {
            return m_Config.customMapPath[device];
        }
        public static bool saveCustomMapping(string customMapPath, System.Windows.Forms.Control[] buttons)
        {
            return m_Config.SaveCustomMapping(customMapPath, buttons);
        }
        public static bool loadCustomMapping(string customMapPath, System.Windows.Forms.Control[] buttons)
        {
            return m_Config.LoadCustomMapping(customMapPath, buttons);
        }
        public static bool loadCustomMapping(int device)
        {
            return m_Config.LoadCustomMapping(getCustomMap(device));
        }
        public static ushort getCustomKey(DS4Controls controlName)
        {
            return m_Config.GetCustomKey(controlName);
        }
        public static X360Controls getCustomButton(DS4Controls controlName)
        {
            return m_Config.GetCustomButton(controlName);
        }
        public static bool getHasCustomKeysorButtons(int device)
        {
            return m_Config.customMapButtons.Count > 0 || m_Config.customMapKeys.Count > 0;
        }
        public static Dictionary<DS4Controls, X360Controls> getCustomButtons()
        {
            return new Dictionary<DS4Controls, X360Controls>(m_Config.customMapButtons);
        }
        public static Dictionary<DS4Controls, ushort> getCustomKeys()
        {
            return new Dictionary<DS4Controls, ushort>(m_Config.customMapKeys);
        }
        #region watch sixaxis data
        static List<SixaxisObserver> observers = new List<SixaxisObserver>();
        static List<DisposableSixaxisObserver> unregisteringObservers = new List<DisposableSixaxisObserver>();
        public interface SixaxisObserver
        {
            void Update(byte[] data);
        }
        public interface DisposableSixaxisObserver : SixaxisObserver
        {
            void InvokeClose();
        }
        public static void registerForSixaxisData(SixaxisObserver observer)
        {
            observers.Add(observer);
        }
        public static bool unregisterForSixaxisData(DisposableSixaxisObserver observer)
        {
            if (!observers.Contains(observer))
                return false;
            unregisteringObservers.Add(observer);
            return true;
        }
        public static bool unregisterForSixaxisData(SixaxisObserver observer)
        {
            return observers.Remove(observer);
        }
        public static bool setSixaxisData(byte[] data)
        {
            for (int i = observers.Count -1; i >= 0; i--)
                observers[i].Update(data);

            if (unregisteringObservers.Count > 0)
            {
                DisposableSixaxisObserver observer = unregisteringObservers[0];
                unregisteringObservers.Remove(observer);
                observers.Remove(observer);
                observer.InvokeClose();
                return false;
            }
            return true;
        }
        #endregion

        public static void Load() 
        {
            m_Config.Load();
        }
        public static void Save() 
        {
            m_Config.Save();
        }

        private static byte applyRatio(byte b1, byte b2, uint r)
        {
            uint ratio = r;
            if (b1 > b2)
            {
                ratio = 100 - r;
            }
            byte bmax = Math.Max(b1, b2);
            byte bmin = Math.Min(b1, b2);
            byte bdif = (byte)(bmax - bmin);
            return (byte)(bmin + (bdif * ratio / 100));
        }
        public static ledColor getTransitionedColor(byte[] c1, byte[] c2, uint ratio)
        {
            ledColor color = new ledColor();
            color.red = 255;
            color.green = 255;
            color.blue = 255;
            uint r = ratio % 101;
            if (c1.Length != 3 || c2.Length != 3 || ratio < 0)
            {
                return color;
            }
            color.red = applyRatio(c1[0], c2[0], ratio);
            color.green = applyRatio(c1[1], c2[1], ratio);
            color.blue = applyRatio(c1[2], c2[2], ratio);

            return color;
        }
    }

   

    public class BackingStore 
    {
        protected String m_File = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName + @"\ScpControl.xml";
        protected XmlDocument m_Xdoc = new XmlDocument();

        public Byte[][] m_Leds = new Byte[][]
        {
            new Byte[] {0,0,255},
            new Byte[] {255,0,0},
            new Byte[] {0,255,0},
            new Byte[] {255,0,255},
        };

        public Byte[] m_Rumble = { 100, 100, 100, 100 };
        public bool[] ledAsBattery = { false, false, false, false };
        public bool[] flashLedLowBattery = { false, false, false, false };
        public Byte[] touchSensitivity = { 100, 100, 100, 100 };
        public bool[] touchEnabled = { false, false, false, false };

        public bool useExclusiveMode = false;

        // Add new settings
        public String[] customMapPath = { String.Empty, String.Empty, String.Empty, String.Empty };
        public Dictionary<DS4Controls, UInt16> customMapKeys = new Dictionary<DS4Controls, UInt16>();
        public Dictionary<DS4Controls, X360Controls> customMapButtons = new Dictionary<DS4Controls, X360Controls>();
        public Int32 formWidth = 782;
        public Int32 formHeight = 550;
        public Boolean startMinimized = false;
        public Boolean[] twoFingerRC = { false, false, false, false };
        public Byte[] tapSensitivity = { 0, 0, 0, 0 };
        public Byte[] scrollSensitivity = { 0, 0, 0, 0 };
        public Byte[][] m_LowLeds = new Byte[][]
        {
            // Default low light for low battery
            new Byte[] {0,0,0},
            new Byte[] {0,0,0},
            new Byte[] {0,0,0},
            new Byte[] {0,0,0}
        };
        public X360Controls GetCustomButton(DS4Controls controlName)
        {
            if (customMapButtons.ContainsKey(controlName))
                return customMapButtons[controlName];
            else return 0;
        }
        public UInt16 GetCustomKey(DS4Controls controlName)
        {
            if (customMapKeys.ContainsKey(controlName))
                return customMapKeys[controlName];
            else return 0;
        }
        public Boolean LoadCustomMapping(String customMapPath)
        {
            Boolean Loaded = true;
            customMapButtons.Clear();
            customMapKeys.Clear();
            try
            {
                if (customMapPath != string.Empty && File.Exists(customMapPath))
                {
                    m_Xdoc.Load(customMapPath);
                    UInt16 wvk;
                    foreach (XmlNode Item in m_Xdoc.SelectSingleNode("/Control").ChildNodes)
                        try
                        {
                            if (UInt16.TryParse(Item.InnerText, out wvk))
                                customMapKeys.Add(getDS4ControlsByName(Item.Name), wvk);
                            else customMapButtons.Add(getDS4ControlsByName(Item.Name), getX360ControlsByName(Item.InnerText));
                        }
                        catch
                        {

                        } 
                }
            }
            catch
            {
                Loaded = false;
            }
            return Loaded;
        }
        public Boolean LoadCustomMapping(String customMapPath, System.Windows.Forms.Control[] buttons)
        {
            Boolean Loaded = true;
            customMapButtons.Clear();
            customMapKeys.Clear();
            try
            {
                if (customMapPath != string.Empty && File.Exists(customMapPath))
                {
                    XmlNode Item;
                    m_Xdoc.Load(customMapPath);
                    UInt16 wvk;
                    foreach (var button in buttons)
                        try
                        {
                            Item = m_Xdoc.SelectSingleNode(String.Format("/Control/{0}", button.Name));
                            if (Item == null)
                                Item = m_Xdoc.SelectSingleNode(String.Format("/Control/{0}", button.Name + "Repeat"));
                            if (Item != null)
                                if (UInt16.TryParse(Item.InnerText, out wvk))
                                {
                                    button.Tag = wvk;
                                    button.Text = ((System.Windows.Forms.Keys)wvk).ToString();
                                    customMapKeys.Add(getDS4ControlsByName(button.Name), wvk);

                                    if (Item.Name.Contains("Repeat"))
                                        button.ForeColor = System.Drawing.Color.Red;
                                }
                                else
                                {
                                    button.Tag = Item.InnerText;
                                    button.Text = Item.InnerText;
                                    customMapButtons.Add(getDS4ControlsByName(button.Name), getX360ControlsByName( Item.InnerText));
                                }
                        }
                        catch
                        {

                        }
                }
            }
            catch 
            { 
                Loaded = false; 
            }
            return Loaded;
        }
        public Boolean SaveCustomMapping(String customMapPath, System.Windows.Forms.Control[] buttons)
        {
            Boolean Saved = true;

            try
            {
                XmlNode Node;
                m_Xdoc.RemoveAll();
                Node = m_Xdoc.CreateXmlDeclaration("1.0", "utf-8", String.Empty);
                m_Xdoc.AppendChild(Node);
                Node = m_Xdoc.CreateComment(String.Format(" Custom Control Mapping Data. {0} ", DateTime.Now));
                m_Xdoc.AppendChild(Node);
                Node = m_Xdoc.CreateWhitespace("\r\n");
                m_Xdoc.AppendChild(Node);
                Node = m_Xdoc.CreateNode(XmlNodeType.Element, "Control", null);
                
                foreach (var button in buttons)
                    try
                    {
                        // Save even if string (for xbox controller buttons)
                        if (button.Tag != null)
                        {
                            string name = button.Name;
                            if (button.ForeColor == System.Drawing.Color.Red)
                                name += "Repeat";
                            XmlNode buttonNode = m_Xdoc.CreateNode(XmlNodeType.Element, name, null);
                            buttonNode.InnerText = button.Tag.ToString(); 
                            Node.AppendChild(buttonNode);
                        }
                    }
                    catch
                    {
                        Saved = false;
                    }
                m_Xdoc.AppendChild(Node);
                m_Xdoc.Save(customMapPath);
            }
            catch 
            { 
                Saved = false; 
            }

            return Saved;
        }

        private DS4Controls getDS4ControlsByName(string key)
        {
            switch (key)
            {
                case "cbShare": return DS4Controls.Share;
                case "cbL3": return DS4Controls.L3;
                case "cbR3": return DS4Controls.R3;
                case "cbOptions": return DS4Controls.Options;
                case "cbUp": return DS4Controls.DpadUp;
                case "cbRight": return DS4Controls.DpadRight;
                case "cbDown": return DS4Controls.DpadDown;
                case "cbLeft": return DS4Controls.DpadLeft;

                case "cbL1": return DS4Controls.L1;
                case "cbR1": return DS4Controls.R1;
                case "cbTriangle": return DS4Controls.Triangle;
                case "cbCircle": return DS4Controls.Circle;
                case "cbCross": return DS4Controls.Cross;
                case "cbSquare": return DS4Controls.Square;

                case "cbPS": return DS4Controls.PS;
                case "cbLX": return DS4Controls.LXNeg;
                case "cbLY": return DS4Controls.LYNeg;
                case "cbRX": return DS4Controls.RXNeg;
                case "cbRY": return DS4Controls.RYNeg;
                case "cbLX2": return DS4Controls.LXPos;
                case "cbLY2": return DS4Controls.LYPos;
                case "cbRX2": return DS4Controls.RXPos;
                case "cbRY2": return DS4Controls.RYPos;
                case "cbL2": return DS4Controls.L2;
                case "cbR2": return DS4Controls.R2;
            }
            return 0;
        }
        private X360Controls getX360ControlsByName(string key)
        {
            switch (key)
            {
                case "Back": return X360Controls.Back;
                case "Left Stick": return X360Controls.LS;
                case "Right Stick": return X360Controls.RS;
                case "Start": return X360Controls.Start;
                case "Up Button": return X360Controls.DpadUp;
                case "Right Button": return X360Controls.DpadRight;
                case "Down Button": return X360Controls.DpadDown;
                case "Left Button": return X360Controls.DpadLeft;

                case "Left Bumper": return X360Controls.LB;
                case "Right Bumper": return X360Controls.RB;
                case "Y Button": return X360Controls.Y;
                case "B Button": return X360Controls.B;
                case "A Button": return X360Controls.A;
                case "X Button": return X360Controls.X;

                case "Guide": return X360Controls.Guide;
                case "Left X-Axis-": return X360Controls.LXNeg;
                case "Left Y-Axis-": return X360Controls.LYNeg;
                case "Right X-Axis-": return X360Controls.RXNeg;
                case "Right Y-Axis-": return X360Controls.RYNeg;

                case "Left X-Axis+": return X360Controls.LXPos;
                case "Left Y-Axis+": return X360Controls.LYPos;
                case "Right X-Axis+": return X360Controls.RXPos;
                case "Right Y-Axis+": return X360Controls.RYPos;
                case "Left Trigger": return X360Controls.LT;
                case "Right Trigger": return X360Controls.RT; 
                case "Click": return X360Controls.LeftMouse;
                case "Right Click": return X360Controls.RightMouse;
                case "Middle Click": return X360Controls.MiddleMouse;
                    
            }
            return 0;
        }
        public Boolean Load() 
        {
            Boolean Loaded = true;
            Boolean missingSetting = false;

            try
            {
                if (File.Exists(m_File))
                {
                    XmlNode Item;

                    m_Xdoc.Load(m_File);


                    for (int i = 0; i < 4; i++)
                    {
                        try { Item = m_Xdoc.SelectSingleNode("/ScpControl/Controller" + (i + 1) + "/Red"); Byte.TryParse(Item.InnerText, out m_Leds[i][0]); }
                        catch { missingSetting = true; }

                        try { Item = m_Xdoc.SelectSingleNode("/ScpControl/Controller" + (i + 1) + "/Green"); Byte.TryParse(Item.InnerText, out m_Leds[i][1]); }
                        catch { missingSetting = true; }

                        try { Item = m_Xdoc.SelectSingleNode("/ScpControl/Controller" + (i + 1) + "/Blue"); Byte.TryParse(Item.InnerText, out m_Leds[i][2]); }
                        catch { missingSetting = true; }

                        try { Item = m_Xdoc.SelectSingleNode("/ScpControl/Controller" + (i + 1) + "/RumbleBoost"); Byte.TryParse(Item.InnerText, out m_Rumble[i]); }
                        catch { missingSetting = true; }

                        try { Item = m_Xdoc.SelectSingleNode("/ScpControl/Controller" + (i + 1) + "/ledAsBatteryIndicator"); Boolean.TryParse(Item.InnerText, out ledAsBattery[i]); }
                        catch { missingSetting = true; }

                        try { Item = m_Xdoc.SelectSingleNode("/ScpControl/Controller" + (i + 1) + "/lowBatteryFlash"); Boolean.TryParse(Item.InnerText, out flashLedLowBattery[i]); }
                        catch { missingSetting = true; }

                        try { Item = m_Xdoc.SelectSingleNode("/ScpControl/Controller" + (i + 1) + "/touchSensitivity"); Byte.TryParse(Item.InnerText, out touchSensitivity[i]); }
                        catch { missingSetting = true; }

                        try { Item = m_Xdoc.SelectSingleNode("/ScpControl/Controller" + (i + 1) + "/touchEnabled"); Boolean.TryParse(Item.InnerText, out touchEnabled[i]); }
                        catch { missingSetting = true; }

                        // Add new settings
                        try { Item = m_Xdoc.SelectSingleNode("/ScpControl/Controller" + (i + 1) + "/LowRed"); Byte.TryParse(Item.InnerText, out m_LowLeds[i][0]); }
                        catch { missingSetting = true; }
                        try { Item = m_Xdoc.SelectSingleNode("/ScpControl/Controller" + (i + 1) + "/LowGreen"); Byte.TryParse(Item.InnerText, out m_LowLeds[i][1]); }
                        catch { missingSetting = true; }
                        try { Item = m_Xdoc.SelectSingleNode("/ScpControl/Controller" + (i + 1) + "/LowBlue"); Byte.TryParse(Item.InnerText, out m_LowLeds[i][2]); }
                        catch { missingSetting = true; }
                        try { Item = m_Xdoc.SelectSingleNode("/ScpControl/Controller" + (i + 1) + "/twoFingerRC"); Boolean.TryParse(Item.InnerText, out twoFingerRC[i]); }
                        catch { missingSetting = true; }
                        try { Item = m_Xdoc.SelectSingleNode("/ScpControl/Controller" + (i + 1) + "/tapSensitivity"); Byte.TryParse(Item.InnerText, out tapSensitivity[i]); }
                        catch { missingSetting = true; }
                        try { Item = m_Xdoc.SelectSingleNode("/ScpControl/Controller" + (i + 1) + "/scrollSensitivity"); Byte.TryParse(Item.InnerText, out scrollSensitivity[i]); }
                        catch { missingSetting = true; }
                        try { Item = m_Xdoc.SelectSingleNode("/ScpControl/Controller" + (i + 1) + "/customMapPath"); customMapPath[i] = Item.InnerText; }
                        catch { missingSetting = true; }

                    }

                    try { Item = m_Xdoc.SelectSingleNode("/ScpControl/useExclusiveMode"); Boolean.TryParse(Item.InnerText, out useExclusiveMode); }
                    catch { missingSetting = true; }
                    try { Item = m_Xdoc.SelectSingleNode("/ScpControl/startMinimized"); Boolean.TryParse(Item.InnerText, out startMinimized); }
                    catch { missingSetting = true; }
                    try { Item = m_Xdoc.SelectSingleNode("/ScpControl/formWidth"); Int32.TryParse(Item.InnerText, out formWidth); }
                    catch { missingSetting = true; }
                    try { Item = m_Xdoc.SelectSingleNode("/ScpControl/formHeight"); Int32.TryParse(Item.InnerText, out formHeight); }
                    catch { missingSetting = true; }
                }
            }
            catch { Loaded = false; }

            // Only add missing settings if the actual load was graceful
            if (missingSetting && Loaded)
                Save();

            return Loaded;
        }
        public Boolean Save() 
        {
            Boolean Saved = true;

            try
            {
                XmlNode Node, Entry;

                m_Xdoc.RemoveAll();

                Node = m_Xdoc.CreateXmlDeclaration("1.0", "utf-8", String.Empty);
                m_Xdoc.AppendChild(Node);

                Node = m_Xdoc.CreateComment(String.Format(" ScpControl Configuration Data. {0} ", DateTime.Now));
                m_Xdoc.AppendChild(Node);

                Node = m_Xdoc.CreateWhitespace("\r\n");
                m_Xdoc.AppendChild(Node);

                Node = m_Xdoc.CreateNode(XmlNodeType.Element, "ScpControl", null);

                XmlNode xmlUseExclNode = m_Xdoc.CreateNode(XmlNodeType.Element, "useExclusiveMode", null); xmlUseExclNode.InnerText = useExclusiveMode.ToString(); Node.AppendChild(xmlUseExclNode);
                XmlNode xmlStartMinimized = m_Xdoc.CreateNode(XmlNodeType.Element, "startMinimized", null); xmlStartMinimized.InnerText = startMinimized.ToString(); Node.AppendChild(xmlStartMinimized);
                XmlNode xmlFormWidth = m_Xdoc.CreateNode(XmlNodeType.Element, "formWidth", null); xmlFormWidth.InnerText = formWidth.ToString(); Node.AppendChild(xmlFormWidth);
                XmlNode xmlFormHeight = m_Xdoc.CreateNode(XmlNodeType.Element, "formHeight", null); xmlFormHeight.InnerText = formHeight.ToString(); Node.AppendChild(xmlFormHeight);
                    
                XmlNode cNode1 = m_Xdoc.CreateNode(XmlNodeType.Element, "Controller1", null); Node.AppendChild(cNode1);
                XmlNode cNode2 = m_Xdoc.CreateNode(XmlNodeType.Element, "Controller2", null); Node.AppendChild(cNode2);
                XmlNode cNode3 = m_Xdoc.CreateNode(XmlNodeType.Element, "Controller3", null); Node.AppendChild(cNode3);
                XmlNode cNode4 = m_Xdoc.CreateNode(XmlNodeType.Element, "Controller4", null); Node.AppendChild(cNode4);

                XmlNode[] cNodes = {cNode1,cNode2,cNode3,cNode4};

                for (int i = 0; i < 4; i++)
                {
                    Entry = m_Xdoc.CreateNode(XmlNodeType.Element, "Red", null); Entry.InnerText = m_Leds[i][0].ToString(); cNodes[i].AppendChild(Entry);
                    Entry = m_Xdoc.CreateNode(XmlNodeType.Element, "Green", null); Entry.InnerText = m_Leds[i][1].ToString(); cNodes[i].AppendChild(Entry);
                    Entry = m_Xdoc.CreateNode(XmlNodeType.Element, "Blue", null); Entry.InnerText = m_Leds[i][2].ToString(); cNodes[i].AppendChild(Entry);
                    Entry = m_Xdoc.CreateNode(XmlNodeType.Element, "RumbleBoost", null); Entry.InnerText = m_Rumble[i].ToString(); cNodes[i].AppendChild(Entry);
                    Entry = m_Xdoc.CreateNode(XmlNodeType.Element, "ledAsBatteryIndicator", null); Entry.InnerText = ledAsBattery[i].ToString(); cNodes[i].AppendChild(Entry);
                    Entry = m_Xdoc.CreateNode(XmlNodeType.Element, "lowBatteryFlash", null); Entry.InnerText = flashLedLowBattery[i].ToString(); cNodes[i].AppendChild(Entry);
                    Entry = m_Xdoc.CreateNode(XmlNodeType.Element, "touchSensitivity", null); Entry.InnerText = touchSensitivity[i].ToString(); cNodes[i].AppendChild(Entry);
                    Entry = m_Xdoc.CreateNode(XmlNodeType.Element, "touchEnabled", null); Entry.InnerText = touchEnabled[i].ToString(); cNodes[i].AppendChild(Entry);

                    // Add new settings
                    Entry = m_Xdoc.CreateNode(XmlNodeType.Element, "LowRed", null); Entry.InnerText = m_LowLeds[i][0].ToString(); cNodes[i].AppendChild(Entry);
                    Entry = m_Xdoc.CreateNode(XmlNodeType.Element, "LowGreen", null); Entry.InnerText = m_LowLeds[i][1].ToString(); cNodes[i].AppendChild(Entry);
                    Entry = m_Xdoc.CreateNode(XmlNodeType.Element, "LowBlue", null); Entry.InnerText = m_LowLeds[i][2].ToString(); cNodes[i].AppendChild(Entry);
                    Entry = m_Xdoc.CreateNode(XmlNodeType.Element, "twoFingerRC", null); Entry.InnerText = twoFingerRC[i].ToString(); cNodes[i].AppendChild(Entry);
                    Entry = m_Xdoc.CreateNode(XmlNodeType.Element, "tapSensitivity", null); Entry.InnerText = tapSensitivity[i].ToString(); cNodes[i].AppendChild(Entry);
                    Entry = m_Xdoc.CreateNode(XmlNodeType.Element, "scrollSensitivity", null); Entry.InnerText = scrollSensitivity[i].ToString(); cNodes[i].AppendChild(Entry);
                    Entry = m_Xdoc.CreateNode(XmlNodeType.Element, "customMapPath", null); Entry.InnerText = customMapPath[i]; cNodes[i].AppendChild(Entry);
                }               
                m_Xdoc.AppendChild(Node);

                m_Xdoc.Save(m_File);
            }
            catch { Saved = false; }

            return Saved;
        }
    }
}
