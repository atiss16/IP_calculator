using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IP_calculator
{
    public class Transformations
    {
        public class Address
        {
            public int Number;
            public byte[] Decimal;
            public string Binary;
        }

        public static BitArray Get_BitArray(byte[] decimalAddress)
        {
            bool[] bits = new bool[32];
            for (int i = 0; i < 4; i++)
                BinaryFromByte(decimalAddress[i]).CopyTo(bits, i*8);
            BitArray ba = new BitArray(bits);
            return ba;
        }

        public static BitArray BinaryFromByte(byte number)
        {
            int temp1 = 0;
            List<int> l = new List<int>();
            BitArray ba = new BitArray(8);
            while (number > 0)
            {
                temp1 = number % 2;
                number /= 2;
                l.Add(temp1);
            }
            l.Reverse();
            for (int i = 0; i < l.Count; i++)
                ba[i + (8 - l.Count)] = (l[i] == 1);
            return ba;
        }
        public static string Get_BinaryFromDecimal(byte[] decimalAddress)
        {
            string s = "";
            for (int i = 0; i < 4; i++)
            {
                BitArray ba = BinaryFromByte(decimalAddress[i]);
                for (int j = 0; j < 8; j++)
                    s += ba[j] ? '1' : '0';
            }
            return s;
        }
        
        public static Address Get_Wildcard(byte[] netmask)
        {
            Address a = new Address();
            byte[] res = new byte[4];
            for (int i = 0; i < 4; i++)
                res[i] = (byte)(255 - netmask[i]);
            a.Decimal = res;
            a.Binary = Get_BinaryFromDecimal(res);
            return a;
        }
        public static Address Get_Network(byte[] ip_address, int slashMask)
        {
            Address a = new Address();
            BitArray ba = Get_BitArray(ip_address);
            for (int i = slashMask; i < 32; i++)
                ba[i] = false;
            a.Decimal = Get_DecimalArray(ba);
            a.Binary = Get_BinaryFromDecimal(a.Decimal);
            return a;
        }
        public static Address Get_Broadcast(byte[] ip_address, int slashMask)
        {
            Address a = new Address();
            BitArray ba = Get_BitArray(ip_address);
            for (int i = slashMask; i < 32; i++)
                ba[i] = true;
            a.Decimal = Get_DecimalArray(ba);
            a.Binary = Get_BinaryFromDecimal(a.Decimal);
            return a;
        }

        public static Address Get_Hostmin(byte[] network, int slashMask)
        {
            Address a = new Address();
            BitArray ba = Get_BitArray(network);
            
            if (slashMask < 31)
                ba[31] = true;
            else if (slashMask == 31)
                ba[31] = false;

            a.Decimal = Get_DecimalArray(ba);
            a.Binary = Get_BinaryFromDecimal(a.Decimal);
            return a;
        }
        public static Address Get_Hostmax(byte[] network, int slashMask)
        {
            Address a = new Address();
            BitArray ba = Get_BitArray(network);

            if (slashMask < 32)
            {
                for (int i = slashMask; i < 31; i++)
                    ba[i] = true;
                ba[31] = (slashMask == 31);
            }

            a.Decimal = Get_DecimalArray(ba);
            a.Binary = Get_BinaryFromDecimal(a.Decimal);
            return a;
        }

        public static int Get_PossibleHosts(int slashMask)
        {
            if (slashMask == 32) return 0;
            if (slashMask == 31) return 2;
            return (int)Math.Pow(2, (32 - slashMask)) - 2;
        }

        private static byte[] BitsToBytes(BitArray src)
        {
            byte[] res = new byte[4];
            for (int j = 0; j < res.Count(); j++)
            {
                byte num = 0;
                {
                    for (byte index = 0, m = 1; index < 8; index++, m *= 2)
                        num += src[index + j * 8] ? m : (byte)0;
                    res[j] = num;
                }
            }
            return res;
        }
        private static byte[] Get_DecimalArray(BitArray ba)
        {
            byte[] res = new byte[ba.Length/8];
            for (int i = 0; i < res.Length; i++)
            {
                int temp = 0;
                for (int j = i*8; j < i*8 + 8; j++)
                {
                    int t = ba[j] ? 1 : 0;
                    temp += (int)(t * Math.Pow(2, 7 - (j % 8)));
                }
                res[i] = (byte)temp;
            }
            return res;
        }
        public static byte[] Get_DecimalMask(int slashMask)
        {
            byte[] decimalMask = new byte[4];

            BitArray mask = new BitArray(32);
            for (int i = 0; i < 32; i++)
            {
                mask[i] = (i < slashMask);
                bool b = mask[i];
            }

            return Get_DecimalArray(mask);
        }
    }
}
