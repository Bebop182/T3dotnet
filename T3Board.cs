using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace T3dotnet
{
    [Flags]
    public enum CellValues
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
        #region Public Properties
        public int Resolution { get; set; }
        public CellValues[] Grid { get; set; }
        public CellValues CurrentPlayer { get; set; }
        public int FreeCellCount { get; set; }
        #endregion

        public T3Board(int resolution)
        {
            Resolution = resolution;
            Grid = new CellValues[resolution * resolution];
            FreeCellCount = Grid.Length;
            CurrentPlayer = CellValues.X;
        }

        public int GetIndexFromCoordinates(Point coord)
        {
            if (coord.X < 0 || coord.X >= Resolution) return -1;
            if (coord.Y < 0 || coord.Y >= Resolution) return -1;
            return coord.X + coord.Y * Resolution;
        }

        public bool AreValuesEquals(params int[] values)
        {
            var equals = true;
            for (int i = 0; i < values.Length - 1; i++)
            {
                if (values[i] != values[i + 1])
                    equals = false;
            }
            return equals;
        }
        //todo: better victory condition check
        public int GetRowValue(int itemIndex)
        {
            int result = 0;
            var baseIndex = itemIndex - itemIndex % Resolution;
            for (int i = 0; i < Resolution; i++)
            {
                result += (int)Grid[baseIndex + i];
            }
            return result;
        }

        public int GetColumnValue(int itemIndex)
        {
            int result = 0;
            for (int i = itemIndex % Resolution; i < Grid.Length; i += Resolution)
            {
                result += (int)Grid[i];
            }
            return result;
        }

        public int GetLeftDiagonalValue()
        {
            int result = 0;
            for (int i = 0; i < Grid.Length; i += Resolution + 1)
            {
                result += (int)Grid[i];
            }
            return result;
        }

        public int GetRightDiagonalValue()
        {
            int result = 0;
            for (int i = Resolution - 1; i < Grid.Length; i += Resolution - 1)
            {
                result += (int)Grid[i];
            }
            return result;
        }

        public int SetValue(int index, CellValues value)
        {
            // if cell marked leave
            if (Grid[index] != CellValues.Empty) return 0;
            Grid[index] = value;
            FreeCellCount--;
            return (int)Grid[index];
        }

        public CellValues[] GetRow(int itemIndex)
        {
            var baseIndex = itemIndex - itemIndex % Resolution;
            return Grid.GetSequence(baseIndex, Resolution);
        }

        public CellValues[] GetColumn(int itemIndex)
        {
            var baseIndex = itemIndex % Resolution;
            return Grid.GetSequence(baseIndex, Resolution, (i) => i += Resolution);
        }

        public IEnumerable<CellValues[]> GetDiagonals(int itemIndex)
        {
            var diagonals = new List<CellValues[]>();
            // Check if index on diagonal
            if (itemIndex % (Resolution + 1) == 0)
                diagonals.Add(Grid.GetSequence(0, Resolution, (i) => i += Resolution + 1));
            if (itemIndex % (Resolution - 1) == 0)
                diagonals.Add(Grid.GetSequence(Resolution - 1, Resolution, (i) => i += Resolution - 1));

            return diagonals;
        }

        public bool IsSequenceValid(CellValues[] sequence, CellValues expectedValue)
        {
            return sequence != null && !sequence.Any(i => i != expectedValue);
        }

        public bool CheckWinConditions(int lastPlayIndex, CellValues value)
        {
            if (lastPlayIndex < 0 || lastPlayIndex >= Grid.Length) return false;

            if (IsSequenceValid(GetRow(lastPlayIndex), value)) return true;
            if (IsSequenceValid(GetColumn(lastPlayIndex), value)) return true;
            foreach (var sequence in GetDiagonals(lastPlayIndex))
            {
                if (IsSequenceValid(sequence, value)) return true;
            }

            return false;
        }

        public CellValues NextPlayer()
        {
            return CurrentPlayer = CurrentPlayer == CellValues.X ? CellValues.O : CellValues.X;
        }
    }
}
