using generic_rock_paper_scissors;
using System.Security.Cryptography;
using System.Text;

partial class Game
{
    public class PC
    {

        private readonly Move move;


        private readonly byte[] key;

        private readonly byte[] hash;


        public PC(IRuleset ruleset)
        {
            key = RandomNumberGenerator.GetBytes(16);

            var hmac = new HMACSHA256(key);

            var moveIdx = RandomNumberGenerator.GetInt32(0, ruleset.Moves.Count);

            move = ruleset.Moves.ElementAt(moveIdx);

            hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(move.ToString().ToCharArray()));

        }

        public void PreMove() => Console.WriteLine($"HMAC: {Convert.ToBase64String(hash)}");



        public Move Move()
        {
            Console.WriteLine($"PC move: {move} ");

            Console.WriteLine($"HMAC key: {Convert.ToBase64String(key)}");

            return move;

        }
    }


}
