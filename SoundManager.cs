using System;

namespace T3dotnet {
    public class SoundManager {
       public static void Welcome()
        {
            Console.Beep(125, 200);
            Console.Beep(250, 200);
        }

        public static void GoodBye()
        {
            Console.Beep(250, 200);
            Console.Beep(125, 200);
        }

        public static void Win()
        {
            Console.Beep(300, 200);
            Console.Beep(400, 300);
            Console.Beep(450, 200);
            Console.Beep(500, 400);
        } 
    }
}