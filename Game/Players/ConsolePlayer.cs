using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Game.Objects;
using NLog;

namespace Game.Players
{
    public class ConsolePlayer : IPlayer
    {
        private static Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        public Tuple<int, int> GetMove(Board board)
        {
            var moves = board.GetAvailableMoves();
            var boardStr = board.ToString();
            var player = board.GetCurrentPlayer().ToString();
            return ReadInput(moves, boardStr, player);
        }

        private Tuple<int, int> ReadInput(IList<Tuple<int, int>> moves, string board, string player)
        {
            var alphabets = Enumerable.Range('a', 26).Select((num) => (char)num).ToList();
            _logger.Log(LogLevel.Error, $"Current player: {player}");
            int _a = 0;
            StringBuilder sb = new StringBuilder(board);
            
            while (board.Contains(" "))
            {
                sb[board.IndexOf(" ")] = alphabets[_a];
                board = sb.ToString();
                _a++;
            }
            alphabets = alphabets.Take(_a).ToList();
            _logger.Log(LogLevel.Error, board);
            Console.Write("Give move letter: ");
            var result = Console.ReadLine();
            while (!alphabets.Contains(result[0]))
            {
                Console.WriteLine("Wrong move letter!");
                Console.Write("Give move letter: ");
                result = Console.ReadLine();
            }
            return moves.ElementAt(alphabets.IndexOf(result[0]));
        }
    }
}
