using System;
using System.Collections.Generic;
using System.Text;

using System.IO;
using System.Reflection;
using System.Xml;
namespace ScpControl 
{
    public enum Ds3PadId :   byte { None = 0xFF, One = 0x00, Two = 0x01, Three = 0x02, Four = 0x03, All = 0x04 };

   
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

        public static byte loadRumbleBoost(int device)
        {
            return m_Config.m_Rumble[device];
        }

        public static byte getTouchSensitivity(int device)
        {
            return m_Config.touchSensitivity[device];
        }
        public static void setTouchSensitivity(int device, byte sen)
        {
             m_Config.touchSensitivity[device] = sen;
        }
        public static void saveColor(int device, byte red, byte green, byte blue)
        {
            m_Config.m_Leds[device][0] = red;
            m_Config.m_Leds[device][1] = green;
            m_Config.m_Leds[device][2] = blue;
        }


        public static void saveRumbleBoost(int device, byte boost)
        {
            m_Config.m_Rumble[device] = boost;

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

        public static bool getUseExclusiveMode()
        {
            return m_Config.useExclusiveMode;

        }

        public static void setUseExclusiveMode(bool exlusive)
        {
            m_Config.useExclusiveMode = exlusive;

        }

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

        public Boolean Load() 
        {
            Boolean Loaded = true;

            try
            {
                XmlNode Item;

                m_Xdoc.Load(m_File);


                for (int i=0; i<4; i++)
                {
                    try { Item = m_Xdoc.SelectSingleNode("/ScpControl/Controller" + (i + 1) + "/Red"); Byte.TryParse(Item.InnerText, out m_Leds[i][0]); }
                     catch { }

                    try { Item = m_Xdoc.SelectSingleNode("/ScpControl/Controller" + (i + 1) + "/Green"); Byte.TryParse(Item.InnerText, out m_Leds[i][1]); }
                     catch { }

                     try { Item = m_Xdoc.SelectSingleNode("/ScpControl/Controller" + (i + 1) + "/Blue"); Byte.TryParse(Item.InnerText, out m_Leds[i][2]); }
                     catch { }

                     try { Item = m_Xdoc.SelectSingleNode("/ScpControl/Controller" + (i + 1) + "/RumbleBoost"); Byte.TryParse(Item.InnerText, out m_Rumble[i]); }
                     catch { }

                     try { Item = m_Xdoc.SelectSingleNode("/ScpControl/Controller" + (i + 1) + "/ledAsBatteryIndicator"); Boolean.TryParse(Item.InnerText, out ledAsBattery[i]); }
                     catch { }

                     try { Item = m_Xdoc.SelectSingleNode("/ScpControl/Controller" + (i + 1) + "/lowBatteryFlash"); Boolean.TryParse(Item.InnerText, out flashLedLowBattery[i]); }
                     catch { }

                     try { Item = m_Xdoc.SelectSingleNode("/ScpControl/Controller" + (i + 1) + "/touchSensitivity"); Byte.TryParse(Item.InnerText, out touchSensitivity[i]); }
                     catch { }

                     try { Item = m_Xdoc.SelectSingleNode("/ScpControl/Controller" + (i + 1) + "/touchEnabled"); Boolean.TryParse(Item.InnerText, out touchEnabled[i]); }
                     catch { }
                }

                try { Item = m_Xdoc.SelectSingleNode("/ScpControl/useExclusiveMode"); Boolean.TryParse(Item.InnerText, out useExclusiveMode); }
                catch { }
            }
            catch { Loaded = false; }
            
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

                XmlNode cNode1 = m_Xdoc.CreateNode(XmlNodeType.Element, "Controller1", null);  Node.AppendChild(cNode1);
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

                }               
                m_Xdoc.AppendChild(Node);

                m_Xdoc.Save(m_File);
            }
            catch { Saved = false; }

            return Saved;
        }

        
    }
}
