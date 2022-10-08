using System;

namespace Supcom2Cards
{
    // System.Random would give different numbers on each PC even with the same hard coded seed for some reason,
    // so I made my own pseudorandom generator class (that gives an identical sequence of numbers on every PC)
    public static class RNG
    {
        private static byte seed = 137;

        private static readonly decimal mDecimal = 1 / (decimal)ulong.MaxValue;
        private static readonly double mDouble = 1 / (double)ulong.MaxValue;
        private static readonly float mFloat = 1 / (float)uint.MaxValue;

        public static bool NextBool()
        {
            return (NextByte() & 1) == 1;
        }

        // where the magic happens
        public static byte NextByte()
        {
            // pseudorandom xor
            int bit = ((seed >> 6) ^ (seed >> 5)) & 1;

            // shift everything left and replace last bit
            seed = BitConverter.GetBytes((seed << 1) | bit)[0];

            return seed;
        }
        public static byte[] NextBytes(int size)
        {
            byte[] data = new byte[size];
            for (byte i = 0; i < size; i++)
            {
                data[i] = NextByte();
            }
            return data;
        }

        public static decimal NextDecimal()
        {
            // decimal is 16 bytes of memory
            return mDecimal * BitConverter.ToUInt64(NextBytes(8), 0);
        }
        public static decimal NextDecimal(decimal min, decimal max)
        {
            return min + (NextDecimal() * (max - min));
        }

        public static double NextDouble()
        {
            // double is 8 bytes of memory
            return mDouble * BitConverter.ToUInt64(NextBytes(8), 0);
        }
        public static double NextDouble(double min, double max)
        {
            return min + (NextDouble() * (max - min));
        }

        public static float NextFloat()
        {
            // double is 4 bytes of memory
            return mFloat * BitConverter.ToUInt32(NextBytes(4), 0);
        }
        public static float NextFloat(float min, float max)
        {
            return min + (NextFloat() * (max - min));
        }

        public static int NextInt()
        {
            return (int)NextUInt();
        }

        public static uint NextUInt()
        {
            return BitConverter.ToUInt32(NextBytes(4), 0);
        }

        public static long NextLong()
        {
            return (long)NextULong();
        }

        public static ulong NextULong()
        {
            return BitConverter.ToUInt64(NextBytes(4), 0);
        }

        public static int NextShort()
        {
            return (int)NextUShort();
        }

        public static uint NextUShort()
        {
            return BitConverter.ToUInt16(NextBytes(2), 0);
        }
    }
}
