using System;

namespace Simulations.Common
{
    public static class Helpers
    {
        private static readonly Random rand = new Random();

        public static int GetRandomColor()
        {
            return (int)(0xFF010101 | (uint)rand.Next(0xFFFFFF));
        }
    }
}
