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
            string currentPlayerSymbol;
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
                // currentPlayerSymbol = Enum.GetName(typeof(CellValues), board.CurrentPlayer);
                // DisplayConsoleHelper.WriteAtPosition(PlayerLabelPosition, currentPlayerSymbol + " plays");

                // // Player select cell
                // var playIndex = SelectTile();
                // var cellValue = board.SetValue(playIndex);

                // if (cellValue == 0) continue;

                // // Write to cell
                // MarkTile(currentPlayerSymbol);

                // // Check victory conditions
                // if (board.CheckWinConditions(playIndex))
                //     winner = currentPlayerSymbol;
                // else
                //     board.NextPlayer();
                foreach (var player in players)
                {
                    currentPlayerSymbol = Enum.GetName(typeof(CellValues), player.Symbol);
                    DisplayConsoleHelper.WriteAtPosition(PlayerLabelPosition, currentPlayerSymbol + " plays");

                    int index = -1;
                    int boardResult = -1;
                    // Let player pick a tile until one is valid
                    do
                    {
                        index = player.PlayTurn(board);
                        boardResult = board.SetValue(index, player.Symbol);
                        //DisplayConsoleHelper.WriteAtPosition(PlayerLabelPosition, currentPlayerSymbol + " plays :" + index + " ; result :" + boardResult);

                    } while (boardResult == 0);

                    MarkTile(currentPlayerSymbol);
                    // MarkTile(index.ToString());

                    // Check victory conditions
                    if (board.CheckWinConditions(index)) {
                        winner = currentPlayerSymbol;
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
                DisplayConsoleHelper.Flush();
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
                    DisplayConsoleHelper.Clamp(Console.CursorLeft + navOffset.X * 2, 0, BoardEnd.X - 1),
                    DisplayConsoleHelper.Clamp(Console.CursorTop + navOffset.Y, BoardOrigin.Y, BoardEnd.Y));
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
