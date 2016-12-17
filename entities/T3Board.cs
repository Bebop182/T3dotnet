using System;
using System.Collections.Generic;
using System.Linq;

namespace T3dotnet
{
    [Flags]
    public enum TileValues
    {
        Empty = 0x0,
        X = 0x1,
        O = 0x2
    }

    public static class T3BoardExtensions
    {
        public static T[] GetSequence<T>(this T[] list, int startIndex, int count, Func<int, int> travelLogic = null)
        {
            var sequence = new T[count];
            if (travelLogic == null)
                travelLogic = (int cursor) => { return ++cursor; };

            for (int i = startIndex, j = 0; j < count; i = travelLogic(i), j++)
            {
                sequence[j] = list[i];
            }
            return sequence;
        }
    }

    public class T3Board
    {
        public class Tile
        {
            public TileValues Value { get; set; }

            public Tile()
            {
                Value = TileValues.Empty;
            }
        }

        public class WriteTileEvent : EventArgs
        {
            public int TileIndex { get; set; }
        }

        public event EventHandler<WriteTileEvent> OnTileMarked;

        #region Public Properties
        public int Resolution { get; set; }
        public Tile[] Grid { get; set; }
        public Queue<Tile> Moves { get; set; }
        public bool IsBoardFull
        {
            get
            {
                return Moves.Count == Grid.Length;
            }
        }

        #endregion

        public T3Board(int resolution)
        {
            Resolution = resolution;
            Grid = new Tile[resolution * resolution];
            for (int i = 0; i < Grid.Length; i++)
            {
                Grid[i] = new Tile();
            }
            Moves = new Queue<Tile>();
        }

        public int SetValue(int index, TileValues value)
        {
            if(index > Grid.Length) return 0;
            if (Grid[index].Value != TileValues.Empty) return 0;
            Moves.Enqueue(Grid[index]);
            TriggerTileMarked(index);
            return (int)(Grid[index].Value = value);
        }

        public int GetIndexFromCoordinates(Point coord)
        {
            if (coord.X < 0 || coord.X >= Resolution) return -1;
            if (coord.Y < 0 || coord.Y >= Resolution) return -1;
            return coord.X + coord.Y * Resolution;
        }

        public Tile[] GetRow(int itemIndex)
        {
            var baseIndex = itemIndex - itemIndex % Resolution;
            return Grid.GetSequence(baseIndex, Resolution);
        }

        public Tile[] GetColumn(int itemIndex)
        {
            var baseIndex = itemIndex % Resolution;
            return Grid.GetSequence(baseIndex, Resolution, (i) => i += Resolution);
        }

        public IEnumerable<Tile[]> GetDiagonals(int itemIndex)
        {
            var diagonals = new List<Tile[]>();

            if(itemIndex > 0 
            && itemIndex < Grid.Length-1 
            && itemIndex % (Resolution-1) == 0) // Right To Left diagonal
                diagonals.Add(Grid.GetSequence(Resolution-1, Resolution, (i)=>i += Resolution-1));

            if(itemIndex % (Resolution+1) == 0)
                diagonals.Add(Grid.GetSequence(0, Resolution, (i)=>i += Resolution+1));

            return diagonals;
        }

        public bool IsSequenceValid(Tile[] sequence, TileValues expectedValue)
        {
            return sequence != null && !sequence.Any(t => t.Value != expectedValue);
        }

        public bool CheckWinConditions(int lastPlayedIndex, TileValues value)
        {
            if (lastPlayedIndex < 0 || lastPlayedIndex >= Grid.Length) return false;

            if (IsSequenceValid(GetRow(lastPlayedIndex), value)) return true;
            if (IsSequenceValid(GetColumn(lastPlayedIndex), value)) return true;
            foreach (var sequence in GetDiagonals(lastPlayedIndex))
            {
                if (IsSequenceValid(sequence, value)) return true;
            }

            return false;
        }

        public void TriggerTileMarked(int tileIndex)
        {
            if (OnTileMarked == null) return;
            OnTileMarked.Invoke(this, new WriteTileEvent() { TileIndex = tileIndex });
        }
    }
}
