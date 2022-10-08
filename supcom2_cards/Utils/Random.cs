using System;

namespace Supcom2Cards
{
    /// <summary>
    /// <para>Use Next...() to generate "random" numbers synchronized on
    /// all clients as long as they're called on every client</para>
    /// <para>If you don't call Next...() on all clients
    /// (for example from code running only on one PC)
    /// you will desync the pseudorandom generator and
    /// the pseudorandom sequence will no longer be
    /// synchronized across all clients</para>
    /// <para>Setting the seed to the same number on all clients will re-synchronize</para>
    /// </summary>
    public static class RNG
    {
        // System.Random would give different numbers on each PC even with the same hard coded seed for some reason,
        // so I made my own pseudorandom generator class (that gives an identical sequence of numbers on every PC)

        public static byte Seed = 137;

        private static readonly decimal mDecimal = 1m / ulong.MaxValue / ulong.MaxValue;
        private static readonly double mDouble = 1d / ulong.MaxValue;
        private static readonly float mFloat = 1f / uint.MaxValue;

        public static bool NextBool()
        {
            return (NextByte() & 1) == 1;
        }

        // where the magic happens
        public static byte NextByte()
        {
            // pseudorandom xor
            int bit = ((Seed >> 6) ^ (Seed >> 5)) & 1;

            // shift everything left and replace last bit
            Seed = BitConverter.GetBytes((Seed << 1) | bit)[0];

            return Seed;
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

        /*
        public static decimal NextDecimal()
        {
            // decimal is 16 bytes of memory
            return mDecimal * new decimal(new int[] { NextInt(), NextInt(), NextInt(), NextInt() });
        }
        public static decimal NextDecimal(decimal min, decimal max)
        {
            return min + (NextDecimal() * (max - min));
        }
        */

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
