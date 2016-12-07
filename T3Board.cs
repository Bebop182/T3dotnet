namespace T3dotnet
{
    public enum CellValues
    {
        Empty = 0,
        X = 1,
        O = -1
    }

    public class T3Board
    {
        #region Public Properties
        public int Resolution { get; set; }
        public CellValues[] Grid { get; set; }
        public CellValues CurrentPlayer { get; set; }
        public int FreeCellCount {get; set;}
        #endregion

        public T3Board(int resolution)
        {
            Resolution = resolution;
            Grid = new CellValues[resolution * resolution];
            FreeCellCount = Grid.Length;
            CurrentPlayer = CellValues.X;
        }

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

        public int GetLeftDiagonal()
        {
            int result = 0;
            for (int i = 0; i < Grid.Length; i += Resolution + 1)
            {
                result += (int)Grid[i];
            }
            return result;
        }

        public int GetRightDiagonal()
        {
            int result = 0;
            for (int i = Resolution - 1; i < Grid.Length; i += Resolution - 1)
            {
                result += (int)Grid[i];
            }
            return result;
        }

        public int SetValue(int index)
        {
            if (Grid[index] != CellValues.Empty) return 0;
            Grid[index] = CurrentPlayer;
            FreeCellCount--;
            return (int)Grid[index];
        }

        public bool CheckWinConditions(int lastPlayIndex)
        {
            var winValue = (int)CurrentPlayer * Resolution;
            if (GetRowValue(lastPlayIndex) == winValue) return true;
            if (GetColumnValue(lastPlayIndex) == winValue) return true;
            if(GetLeftDiagonal() == winValue) return true;
            if(GetRightDiagonal() == winValue) return true;
            return false;
        }

        public CellValues NextPlayer()
        {
            return CurrentPlayer = CurrentPlayer == CellValues.X ? CellValues.O : CellValues.X;
        }
    }
}
