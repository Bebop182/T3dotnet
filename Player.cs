// public playTurn()
// private SelectTile()
// 
using System;

namespace T3dotnet
{
    public class Player
    {
        public class PlayerNavigationEventArgs : EventArgs {
            public PlayerNavigationEventArgs(Point gridCoordinates) {
                GridCoordinates = gridCoordinates;
            }
            public Point GridCoordinates {get; set;}
        }

        public event EventHandler<PlayerNavigationEventArgs> OnNavigated;

        private void TriggerNavigationEvent(Point navigationOffset)
        {
            if (OnNavigated != null)
                OnNavigated.Invoke(this, new PlayerNavigationEventArgs(navigationOffset));
        }

        public string Label {get; set;}
        public CellValues Symbol { get; set; }
        public Player(CellValues symbol)
        {
            Symbol = symbol;
            Label = Enum.GetName(typeof(CellValues), symbol);
        }

        public int PlayTurn(T3Board board)
        {
            if (board == null) return -1;
            var tileCoordinate = ChooseTile(board.Resolution);

            return board.GetIndexFromCoordinates(tileCoordinate);
        }

        private Point ChooseTile(int boardResolution)
        {
            // Navigation using arrow keys
            bool endTurn = false;
            Point tileCoordinate = new Point(boardResolution/2, boardResolution/2);
            TriggerNavigationEvent(tileCoordinate);
            do
            {
                //Flush();
                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.LeftArrow:
                        tileCoordinate.X--;
                        break;
                    case ConsoleKey.RightArrow:
                        tileCoordinate.X++;
                        break;
                    case ConsoleKey.UpArrow:
                        tileCoordinate.Y--;
                        break;
                    case ConsoleKey.DownArrow:
                        tileCoordinate.Y++;
                        break;
                    case ConsoleKey.Enter:
                        endTurn = true;
                        break;
                }
                tileCoordinate = tileCoordinate.Clamp(0, boardResolution-1);
                TriggerNavigationEvent(tileCoordinate);
            }
            while (!endTurn);

            return tileCoordinate;
        }
    }
}