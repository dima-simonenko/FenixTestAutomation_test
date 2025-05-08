// Utils/KeyParser.cs
using System;
using FlaUI.Core.WindowsAPI;

namespace FenixTestAutomation.Utils
{
    public static class KeyParser
    {
        public static VirtualKeyShort? Parse(string key)
        {
            switch (key)
            {
                case "Alt": return (VirtualKeyShort)0x12;
                case "Ctrl": return (VirtualKeyShort)0x11;
                case "A": return VirtualKeyShort.KEY_A;
                case "B": return VirtualKeyShort.KEY_B;
                case "C": return VirtualKeyShort.KEY_C;
                case "D": return VirtualKeyShort.KEY_D;
                case "E": return VirtualKeyShort.KEY_E;
                case "F": return VirtualKeyShort.KEY_F;
                case "G": return VirtualKeyShort.KEY_G;
                case "H": return VirtualKeyShort.KEY_H;
                case "I": return VirtualKeyShort.KEY_I;
                case "J": return VirtualKeyShort.KEY_J;
                case "L": return VirtualKeyShort.KEY_L;
                case "O": return VirtualKeyShort.KEY_O;
                case "P": return VirtualKeyShort.KEY_P;
                case "Q": return VirtualKeyShort.KEY_Q;
                case "R": return VirtualKeyShort.KEY_R;
                case "S": return VirtualKeyShort.KEY_S;
                case "U": return VirtualKeyShort.KEY_U;
                case "W": return VirtualKeyShort.KEY_W;
                case "Z": return VirtualKeyShort.KEY_Z;
                default: return null;
            }
        }
    }
}