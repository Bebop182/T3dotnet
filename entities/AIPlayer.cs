using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static T3dotnet.T3Board;

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
            // var index = ChooseTile_Easy(board);
            //var index = ChooseTile_Medium(board);
            var index = ChooseTile_Hard(board);

            Thread.Sleep(50);
            return index;
        }

        private IEnumerable<WeightedMove> GetAvailableMoves(T3Board board)
        {
            var moves = new List<WeightedMove>();
            var grid = board.Grid;
            int lineRisk = 0;

            for (int i = 0; i < board.Grid.Length; i++)
            {
                if (i % board.Resolution == 0)
                    lineRisk = EvaluateSequenceRisk(board.GetRow(i).Select(t => t.Value).ToArray());

                if (grid[i].Value != TileValues.Empty) continue;

                var diags = board.GetDiagonals(i);
                var diagRisk = EvaluateDiagRisk(diags);
                var columnRisk = EvaluateSequenceRisk(board.GetColumn(i).Select(t => t.Value).ToArray());
                var maxRisk = lineRisk > columnRisk ? lineRisk : columnRisk;
                maxRisk = maxRisk >= diagRisk ? maxRisk : diagRisk;
                var connexions = diags.Count();

                var move = new WeightedMove()
                {
                    Index = i,
                    Cost = maxRisk + connexions
                };
                moves.Add(move);
                //DisplayConsoleHelper.WriteAtPosition(new Point(0, Console.CursorTop+10+i), "Index: " + i + " / lineRisk: " + lineRisk + " / columnRisk: " + columnRisk + " / diagRisk: " + diagRisk);
            }
            moves.Sort((m1, m2) => { return m2.Cost.CompareTo(m1.Cost); });
            return moves;
        }

        private int ChooseTile_Easy(T3Board board)
        {
            return _rnd.Next(0, board.Grid.Length);
        }

        private int ChooseTile_Medium(T3Board board)
        {
            return GetAvailableMoves(board).First().Index;
        }

        private int ChooseTile_Hard(T3Board board)
        {
            var moves = GetAvailableMoves(board);
            int i = 1;
            var move = moves.First();
            if (move.Cost <= 2) return move.Index;

            while (i < moves.Count() - 1)
            {
                var next = moves.ElementAt(i++);
                if (move.Cost == next.Cost)
                {
                    move = moves.ElementAt(i);
                }
                else
                    break;
            }
            return move.Index;
        }

        private int EvaluateSequenceRisk(TileValues[] sequence)
        {
            if (sequence.Any(t => t == Symbol)) return 0;
            return sequence.Length - sequence.Count(t => (t == TileValues.Empty));
            //return sequence.Length;
        }

        private int EvaluateDiagRisk(IEnumerable<Tile[]> diags)
        {
            int risk = 0;
            foreach (var diag in diags)
            {
                risk += EvaluateSequenceRisk(diag.Select(t => t.Value).ToArray());
            }
            return risk;
        }
    }
}