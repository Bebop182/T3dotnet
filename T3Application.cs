using System;
using System.Threading;

namespace T3dotnet
{
    public class T3Application
    {
        #region Cursor Positions
        private static Point TitlePosition { get; set; }
        private static Point PlayerLabelPosition { get; set; }
        private static Point MessageBoxPosition { get; set; }
        private static Point FooterPosition { get; set; }
        private static Point BoardOrigin { get; set; }
        private static Point BoardEnd { get; set; }
        #endregion



        public T3Application()
        {
            // Console settings
            Console.CursorSize = 90;

        }

        public static void Main(string[] args)
        {
            // Initialize layout
            InitializeLayout(3);

            // Display header
            DisplayConsoleHelper.WriteAtPosition(TitlePosition, "-= TicTacToe by Bebop182 =-");
            SoundManager.Welcome();

            // Draw board
            DrawBoard(3);

            // Initialize Game
            var board = new T3Board(3);
            var winner = string.Empty;
            var players = new Player[2] {
                new Player(CellValues.X),
                new Player(CellValues.O),
            };
            foreach (var player in players)
            {
                player.OnNavigated += (object sender, Player.PlayerNavigationEventArgs e) =>
                {
                    Console.SetCursorPosition(BoardOrigin.X + e.GridCoordinates.X * 2, BoardOrigin.Y + e.GridCoordinates.Y);
                };
            }

            // Game loop
            do
            {
                foreach (var player in players)
                {
                    DisplayConsoleHelper.WriteAtPosition(PlayerLabelPosition, player.Label + " plays");

                    int index = -1;
                    // Let player pick a tile until one is valid
                    do
                    {
                        index = player.PlayTurn(board);
                    } while (board.SetValue(index, player.Symbol) == 0);

                    MarkTile(player.Label);

                    // Check victory conditions
                    if (board.CheckWinConditions(index, player.Symbol)) {
                        winner = player.Label;
                        break;
                    }
                }
            } while (winner.Equals(string.Empty) && board.FreeCellCount > 0);

            // Game resolution
            if (winner.Equals(string.Empty))
                DisplayConsoleHelper.WriteAtPosition(MessageBoxPosition, "Draw");
            else
            {
                DisplayConsoleHelper.WriteAtPosition(MessageBoxPosition, "And the winner is... : " + winner + " !");
                SoundManager.Win();
            }

            Thread.Sleep(500);
            Console.SetCursorPosition(FooterPosition.X, FooterPosition.Y);

            DisplayConsoleHelper.WriteAtPosition(FooterPosition, "-= Good Bye ! =-");
            SoundManager.GoodBye();
        }

        private static void InitializeLayout(int resolution)
        {
            // Title
            TitlePosition = DisplayConsoleHelper.GetCursorPosition();
            Console.WriteLine();

            // Player Label
            PlayerLabelPosition = DisplayConsoleHelper.GetCursorPosition();
            Console.WriteLine();

            // Board
            Console.WriteLine();
            BoardOrigin = DisplayConsoleHelper.GetCursorPosition();
            for (int i = 0; i < resolution; i++)
            {
                Console.WriteLine();
            }
            Console.WriteLine();

            // MessageBox
            MessageBoxPosition = DisplayConsoleHelper.GetCursorPosition();
            Console.WriteLine();

            // Footer
            FooterPosition = DisplayConsoleHelper.GetCursorPosition();
            Console.WriteLine();
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
        #region Helpers


        private static int GetIndexFromPosition(int x, int y)
        {
            var resolution = (BoardEnd.Y - BoardOrigin.Y + 1);
            return x / 2 + y * resolution;
        }

        private static int GetIndexFromPosition(Point position)
        {
            return GetIndexFromPosition(position.X, position.Y);
        }


        #endregion
    }
}
