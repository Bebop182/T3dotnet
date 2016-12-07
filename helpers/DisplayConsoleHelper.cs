using System;

namespace T3dotnet
{
    public static class DisplayConsoleHelper
    {
        public static void Flush()
        {
            while (Console.KeyAvailable)
                Console.ReadKey(true);
        }

        public static Point GetCursorPosition()
        {
            return new Point(Console.CursorLeft, Console.CursorTop);
        }

        public static void WriteAtPosition(Point position, string content)
        {
            var cursor = GetCursorPosition();
            Console.SetCursorPosition(position.X, position.Y);
            Flush();
            Console.WriteLine(content);
            Console.SetCursorPosition(cursor.X, cursor.Y);
        }

        public static int Clamp(int value, int min, int max)
        {
            if (value <= min) return min;
            if (value >= max) return max;
            return value;
        }
    }
}