namespace Simulations.Common
{
    public static class ExtensionMethods
    {
        public static double Constrain(this double distance, double l, double h)
        {
            if (distance < l) return l;
            if (distance > h) return h;
            return distance;
        }

        public static double Map(this double value, double min1, double max1, double min2, double max2)
        {
            return min2 + (
                    (value - min1) * (
                        (max2 - min2) / (max1 - min1)));
        }
    }
}
