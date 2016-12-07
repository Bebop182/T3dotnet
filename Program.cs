using System;
using System.Threading;

namespace T3dotnet
{
    public class Program
    {
        #region Cursor Positions
        private static Point TitlePosition { get; set; }
        private static Point PlayerLabelPosition { get; set; }
        private static Point MessageBoxPosition { get; set; }
        private static Point FooterPosition { get; set; }
        private static Point BoardOrigin { get; set; }
        private static Point BoardEnd { get; set; }
        #endregion

        public Program()
        {
            // Console settings
            Console.CursorSize = 90;
        }

        public static void Main(string[] args)
        {
            // Initialize layout
            InitializeLayout(3);

            // Display header
            WriteAtPosition(TitlePosition, "-= TicTacToe by Bebop182 =-");
            SoundManager.Welcome();

            // Draw board
            DrawBoard(3);

            // Initialize Game
            var board = new T3Board(3);
            var winner = string.Empty;
            string currentPlayerSymbol;

            // Game loop
            do
            {
                currentPlayerSymbol = Enum.GetName(typeof(CellValues), board.CurrentPlayer);
                WriteAtPosition(PlayerLabelPosition, currentPlayerSymbol + " plays");

                // Player select cell
                var playIndex = SelectTile();
                var cellValue = board.SetValue(playIndex);

                if (cellValue == 0) continue;

                // Write to cell
                MarkTile(currentPlayerSymbol);

                // Check victory conditions
                if (board.CheckWinConditions(playIndex))
                    winner = currentPlayerSymbol;
                else
                    board.NextPlayer();
            } while (winner.Equals(string.Empty) && board.FreeCellCount > 0);

            // Game resolution
            if (winner.Equals(string.Empty))
                WriteAtPosition(MessageBoxPosition, "Draw");
            else
            {
                WriteAtPosition(MessageBoxPosition, "And the winner is... : " + winner + " !");
                SoundManager.Win();
            }

            Thread.Sleep(500);
            Console.SetCursorPosition(FooterPosition.X, FooterPosition.Y);

            WriteAtPosition(FooterPosition, "-= Good Bye ! =-");
            SoundManager.GoodBye();
        }

        private static void InitializeLayout(int resolution)
        {
            // Title
            TitlePosition = GetCursorPosition();
            Console.WriteLine();

            // Player Label
            PlayerLabelPosition = GetCursorPosition();
            Console.WriteLine();

            // Board
            Console.WriteLine();
            BoardOrigin = GetCursorPosition();
            for (int i = 0; i < resolution; i++)
            {
                Console.WriteLine();
            }
            Console.WriteLine();

            // MessageBox
            MessageBoxPosition = GetCursorPosition();
            Console.WriteLine();

            // Footer
            FooterPosition = GetCursorPosition();
            Console.WriteLine();
        }

        private static int SelectTile()
        {
            // Reset cusor position for navigation (middle)
            Console.SetCursorPosition(BoardOrigin.X + 2, BoardOrigin.Y + 1);

            // Navigation using arrow keys
            ConsoleKey key;
            bool endTurn;
            do
            {
                endTurn = false;
                Flush();
                key = Console.ReadKey(true).Key;
                var navOffset = new Point();

                switch (key)
                {
                    case ConsoleKey.LeftArrow:
                        navOffset.X--;
                        break;
                    case ConsoleKey.RightArrow:
                        navOffset.X++;
                        break;
                    case ConsoleKey.UpArrow:
                        navOffset.Y--;
                        break;
                    case ConsoleKey.DownArrow:
                        navOffset.Y++;
                        break;
                    case ConsoleKey.Enter:
                        endTurn = true;
                        break;
                }
                Console.SetCursorPosition(
                    Clamp(Console.CursorLeft + navOffset.X * 2, 0, BoardEnd.X - 1),
                    Clamp(Console.CursorTop + navOffset.Y, BoardOrigin.Y, BoardEnd.Y));
            }
            while (!endTurn);
            var index = GetIndexFromPosition(Console.CursorLeft, Console.CursorTop - BoardOrigin.Y);
            return index;
        }

        private static void MarkTile(string symbol)
        {
            //Flush();
            Console.Write(symbol);
            Console.CursorLeft--;
            Console.Beep(300, 150);
        }

        public static bool RunApp(params object[] args)
        {
            var count = (int)args[0];
            if (count < 1) return true;
            return false;
        }

        private static void DrawBoard(int resolution)
        {
            Console.SetCursorPosition(BoardOrigin.X, BoardOrigin.Y);

            for (int i = 0; i < resolution - 1; i++)
            {
                Console.WriteLine("_|_|_");
                Thread.Sleep(200);
            }
            Console.Write(" | | ");

            BoardEnd = new Point(Console.CursorLeft, Console.CursorTop);

            Console.WriteLine();
        }

        private static void WriteAtPosition(Point position, string content)
        {
            var cursor = GetCursorPosition();
            Console.SetCursorPosition(position.X, position.Y);
            Flush();
            Console.WriteLine(content);
            Console.SetCursorPosition(cursor.X, cursor.Y);
        }

        #region Helpers
        private static void Flush()
        {
            while (Console.KeyAvailable)
                Console.ReadKey(true);
        }

        private static Point GetCursorPosition()
        {
            return new Point(Console.CursorLeft, Console.CursorTop);
        }

        private static int GetIndexFromPosition(Point position)
        {
            return GetIndexFromPosition(position.X, position.Y);
        }

        private static int GetIndexFromPosition(int x, int y)
        {
            var resolution = (BoardEnd.Y - BoardOrigin.Y + 1);
            return x / 2 + y * resolution;
        }

        private static int Clamp(int value, int min, int max)
        {
            if (value <= min) return min;
            if (value >= max) return max;
            return value;
        }
        #endregion
    }
}
