using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace T3dotnet
{
    public class AIPlayer : Player
    {
        public class WeightedMove
        {
            public int Index { get; set; }
            public int Cost { get; set; }
        }

        private Random _rnd;

        public AIPlayer(TileValues symbol) : base(symbol)
        {
            _rnd = new Random();
        }

        public override int PlayTurn(T3Board board)
        {
            
            var index = ChooseTile_Easy(board);
            Thread.Sleep(50);
            return index;
        }

        private int ChooseTile_Easy(T3Board board) {
            return _rnd.Next(0, board.Grid.Length);
        }

        private int ChooseTile_Medium(T3Board board) {
            var moves = new List<WeightedMove>();
            var grid = board.Grid;
            for(int i=0; i<board.Grid.Length; i++) {
                if(grid[i].Value != TileValues.Empty) continue;
                var move = new WeightedMove() {
                    Index = i,
                    Cost = 0
                };
            }

            foreach(var move in moves) {

            }
            return 0;
        }

        private int EvaluateSequenceRisk(TileValues[] sequence)
        {
            var freeCellCount = sequence.Count(c => c.HasFlag(TileValues.Empty));
            if (freeCellCount == 0 || freeCellCount == sequence.Length) return 0;
            var friendlyCellCount = sequence.Count(c => c.HasFlag(Symbol));
            if (friendlyCellCount > 0) return 0;
            return sequence.Length - friendlyCellCount - freeCellCount;
        }
    }
}