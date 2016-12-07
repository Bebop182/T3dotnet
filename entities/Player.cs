using System;

namespace T3dotnet
{
    public class Player
    {
        public string Label {get; set;}
        public TileValues Symbol { get; set; }
        public event EventHandler<PlayerNavigationEventArgs> OnNavigated;

        public Player(TileValues symbol)
        {
            Symbol = symbol;
            Label = Enum.GetName(typeof(TileValues), symbol);
        }

        public virtual int PlayTurn(T3Board board)
        {
            if (board == null) return -1;
            
            // Navigation using arrow keys
            bool endTurn = false;
            Point tileCoordinate = new Point(board.Resolution/2, board.Resolution/2);
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
                tileCoordinate = tileCoordinate.Clamp(0, board.Resolution-1);
                TriggerNavigationEvent(tileCoordinate);
            }
            while (!endTurn);

            return board.GetIndexFromCoordinates(tileCoordinate);
        }

        protected void TriggerNavigationEvent(Point navigationOffset)
        {
            if (OnNavigated != null)
                OnNavigated.Invoke(this, new PlayerNavigationEventArgs(navigationOffset));
        }

        public class PlayerNavigationEventArgs : EventArgs {
            public PlayerNavigationEventArgs(Point gridCoordinates) {
                GridCoordinates = gridCoordinates;
            }
            public Point GridCoordinates {get; set;}
        }
    }
}