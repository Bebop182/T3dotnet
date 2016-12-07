namespace T3dotnet
{
    public static class PointHelper
    {
        public static Point Clamp(this Point point, int min, int max)
        {
            return new Point(Clamp(point.X, min, max), Clamp(point.Y, min, max));
        }

        public static int Clamp(int value, int min, int max)
        {
            if (value <= min) return min;
            if (value >= max) return max;
            return value;
        }
    }

    public struct Point
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
