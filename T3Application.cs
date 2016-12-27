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
        }

        public static void Main(string[] args)
        {
            Console.CursorSize = 90;
            
            // Initialize layout
            InitializeLayout(3);

            // Display header
            DisplayConsoleHelper.WriteAtPosition(TitlePosition, "-= TicTacToe by Bebop182 =-");
            SoundManager.Welcome();

            // Draw board
            DrawBoard(3);

            // Initialize Game
            var board = new T3Board(3);
            var players = new Player[2] {
                new Player(TileValues.X),
                new AIPlayer(TileValues.O),
            };
            var currentPlayer = players[0];
            var winner = string.Empty;

            foreach(var player in players) {
                if(player is AIPlayer) continue;
                player.OnNavigated += (object sender, Player.PlayerNavigationEventArgs e) => {
                    Console.SetCursorPosition(BoardOrigin.X + e.GridCoordinates.X * 2, BoardOrigin.Y + e.GridCoordinates.Y);
                };
            }
            board.OnTileMarked += (object sender, T3Board.WriteTileEvent e) =>
            {
                var t3_board = sender as T3Board;
                var x = e.TileIndex % t3_board.Resolution;
                var y = e.TileIndex / t3_board.Resolution;
                Console.SetCursorPosition(BoardOrigin.X + x * 2, BoardOrigin.Y + y);
            };

            // Game loop
            do
            {
                DisplayConsoleHelper.WriteAtPosition(PlayerLabelPosition, currentPlayer.Label + " plays");

                int index = -1;
                // Let player pick a tile until one is valid
                do
                {
                    index = currentPlayer.PlayTurn(board);
                } while (board.SetValue(index, currentPlayer.Symbol) == 0);

                MarkTile(currentPlayer.Label);

                // Check victory conditions
                if (board.CheckWinConditions(index, currentPlayer.Symbol))
                    winner = currentPlayer.Label;
                else
                    currentPlayer = currentPlayer == players[0] ? players[1] : players[0];
            } while (winner.Equals(string.Empty) && !board.IsBoardFull);

            // Game resolution
            if (winner.Equals(string.Empty))
                DisplayConsoleHelper.WriteAtPosition(MessageBoxPosition, "Draw");
            else
            {
                DisplayConsoleHelper.WriteAtPosition(MessageBoxPosition, "And the winner is... " + winner + " !");
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
    }
}
