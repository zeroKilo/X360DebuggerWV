using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace X360DebuggerWV
{
    public static class PPC
    {
        public static uint getOPCD(uint u)
        {
            return (u >> 26) & 0x3f; 
        }

        public static uint getXOfromXLform(uint u)
        {
            return (u >> 1) & 0x3ff; 
        }

        public static byte getBHfromXLform(uint u)
        {
            return (byte)((u >> 11) & 0x3); 
        }

        public static uint getLIfromIform(uint u)
        {
            return u & 0x3FFFFFC;
        }

        public static uint getBDfromBform(uint u)
        {
            return u & 0xFFFC;
        }

        public static bool isAbsoluteAddress(uint u)
        {
            return (u & 2) != 0;
        }

        public static bool getLK(uint u)
        {
            return (u & 1) != 0;
        }

        public static bool isBranchOpc(uint u)
        {
            uint opcd = getOPCD(u);
            switch (opcd)
            {
                case 16:
                case 17:
                case 18:
                    return true;
                case 19:
                    uint xo = getXOfromXLform(u);
                    switch (xo)
                    {
                        case 16:
                        case 528:
                            return true;
                        default:
                            return false;
                    }
                default:
                    return false;
            }
        }

        public static bool calcBranchTarget(uint opc, uint address, out uint target)
        {
            target = 0;
            long temp = address;
            uint opcd = getOPCD(opc);
            bool isAA = isAbsoluteAddress(opc);
            switch (opcd)
            {
                case 18:
                    temp = getLIfromIform(opc);
                    if ((opc & 0x02000000) != 0) temp += 0xFC000000;
                    if (!isAA)
                        temp = address + (int)temp;
                    target = (uint)temp;
                    return true;
                case 16:
                    temp = getBDfromBform(opc);
                    if ((opc & 0x00008000) != 0)
                        temp |= 0xFFFF0000;
                    if (!isAA)
                        temp = address + (int)temp;
                    target = (uint)temp;
                    return true;
            }
            return false;
        }

        public static bool hintSubReturn(uint u)
        {
            uint opcd = getOPCD(u);
            uint xo = getXOfromXLform(u);
            byte bh = getBHfromXLform(u);
            return (opcd == 19 && xo == 16 && bh == 0) ||
                   (u == 0) ||
                   (u == 0x4e800020);
        }

        public static uint SwapEndian(uint u)
        {
            byte[] buf = BitConverter.GetBytes(u);
            uint result = 0;
            for (int i = 0; i < 4; i++)
                result = (result << 8) | buf[i];
            return result;
        }
    }
}
