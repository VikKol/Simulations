using System;
using SharpDX;

namespace Simulations.Common
{
    public static class Helpers
    {
        private static readonly Random rand = new Random();

        public static Color GetRandomColor()
        {
            var rcol = (int)(0xFF010101 | (uint)rand.Next(0xFFFFFF));
            return Color.FromAbgr(rcol);
        }
    }
}
