﻿using System;

namespace Supcom2Cards
{
    /// <summary>
    /// <para>
    ///     Use Next...() to generate "random" numbers synchronized on
    ///     all clients as long as they're called on every client
    /// </para>
    /// <para>
    ///     If you accidentally don't call Next...() on all clients
    ///     (for example from code running only on one PC)
    ///     you will desync the RNG and the pseudorandom sequence
    ///     will no longer be synchronized across all clients
    /// </para>
    /// <para>
    ///     If you need a random number for a single client just use
    ///     another generator like System.Random
    /// </para>
    /// <para>
    ///     Calling Reset() on all clients will re-synchronize
    /// </para>
    /// </summary>
    public static class RNG
    {
        // System.Random would give different numbers on each PC even with the same hard coded seed for some reason,
        // so I made my own pseudorandom generator class (that gives an identical sequence of numbers on every PC)

        private static uint seed = 0xe71c3954;

        private const decimal m = 1m / ulong.MaxValue;
        private const double d = 1d / ulong.MaxValue;
        private const float f = 1f / uint.MaxValue;

        public enum Distribution
        {
            Uniform,        // min ▒▒▒▒▒ max
            Exponential,    // min ▓▒░░░ max
            //Normal,         // min ░▒▓▒░ max
            //Triangular      // min ░▒▓▒░ max
        }

        public static bool NextBool()
        {
            return (NextUInt() & 1) == 1;
        }

        public static byte NextByte()
        {
            return BitConverter.GetBytes(NextUInt())[0];
        }
        public static byte[] NextBytes(int size)
        {
            byte[] bytes = new byte[size];
            byte[] rand = { 0, 0, 0, 0 };
            for (int i = 0; i < size; i++)
            {
                if (i % 4 == 0)
                {
                    // generate next 4 bytes
                    rand = BitConverter.GetBytes(NextUInt());
                }
                bytes[i] = rand[i % 4];
            }
            return bytes;
        }

        public static decimal NextDecimal(Distribution distribution = Distribution.Uniform)
        {
            // TODO: Upgrade to 128 bits instead of ulong 64 bits?
            decimal ans = NextULong() * m;

            switch (distribution)
            {
                case Distribution.Uniform:
                    return ans;
                case Distribution.Exponential:
                    return ans * NextDecimal(Distribution.Uniform);
                default:
                    throw new NotImplementedException($"NextDecimal() {distribution} Distribution not implemented");
            }
        }
        public static decimal NextDecimal(decimal min, decimal max, Distribution distribution = Distribution.Uniform)
        {
            return (NextDecimal(distribution) * (max - min)) + min;
        }

        public static double NextDouble(Distribution distribution = Distribution.Uniform)
        {
            double ans = NextULong() * d;

            // I'm too lazy to manually check exponent/mantissa validity
            if (ans == double.NaN || ans == double.NegativeInfinity || ans == double.PositiveInfinity)
            {
                ans = NextDouble(Distribution.Uniform);
            }

            switch (distribution)
            {
                case Distribution.Uniform:
                    return ans;
                case Distribution.Exponential:
                    return ans * NextDouble(Distribution.Uniform);
                default:
                    throw new NotImplementedException($"NextDouble() {distribution} Distribution not implemented");
            }
        }
        public static double NextDouble(double min, double max, Distribution distribution = Distribution.Uniform)
        {
            return (NextDouble(distribution) * (max - min)) + min;
        }

        public static float NextFloat(Distribution distribution = Distribution.Uniform)
        {
            float ans = NextUInt() * f;

            // I'm too lazy to manually check exponent/mantissa validity
            if (ans == float.NaN || ans == float.NegativeInfinity || ans == float.PositiveInfinity)
            {
                ans = NextFloat(Distribution.Uniform);
            }

            switch (distribution)
            {
                case Distribution.Uniform:
                    return ans;
                case Distribution.Exponential:
                    return ans * NextFloat(Distribution.Uniform);
                default:
                    throw new NotImplementedException($"NextFloat() {distribution} Distribution not implemented");
            }
        }
        public static float NextFloat(float min, float max, Distribution distribution = Distribution.Uniform)
        {
            return (NextFloat(distribution) * (max - min)) + min;
        }

        public static int NextInt()
        {
            return unchecked((int)NextUInt());
        }
        public static int NextInt(int min, int max)
        {
            uint range = (uint)(max - min);
            uint rUInt = NextUInt(0, range);

            // somehow this doesn't break for: rUInt > int.MaxValue
            return (int)rUInt + min;
        }

        // where the magic happens
        public static uint NextUInt()
        {
            // pseudorandom xor
            uint bit = (seed >> 27) ^ (seed >> 11) ^ (seed >> 6) ^ (seed >> 5);

            // shift everything left and replace last bit
            seed = (seed << 1) | (bit & 1);

            return seed;
        }
        public static uint NextUInt(uint min, uint max)
        {
            uint range = max - min;

            ulong ans = ((ulong)range + 1) * NextUInt() / uint.MaxValue;

            return ans > range ? (range + min) : (uint)ans + min;
        }

        public static long NextLong()
        {
            return unchecked((long)NextULong());
        }

        public static ulong NextULong()
        {
            // ulong is 8 bytes of memory
            return BitConverter.ToUInt64(NextBytes(8), 0);
        }

        public static short NextShort()
        {
            return unchecked((short)NextUShort());
        }

        public static ushort NextUShort()
        {
            // short is 2 bytes of memory
            return BitConverter.ToUInt16(NextBytes(2), 0);
        }

        /// <summary>
        /// Call on all clients to resynchronize the pseudorandom sequence
        /// </summary>
        public static void Reset()
        {
            seed = 0xe71c3954;
        }

        // required for 50% 0/1 on every bit instead of last bit only + shift
        private static uint NextBinary()
        {
            uint ans = 0;
            for (int i = 0; i < 32; i++)
            {
                ans |= (NextUInt() & 1) << i;
            }
            return ans;
        }
    }
}
