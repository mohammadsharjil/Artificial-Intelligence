using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using _8PuzzleGame.Solvers;

namespace _8PuzzleGame
{
    public class Startup
    {
        public static void Main()
        {
               
               
            var arr = new int[3, 3] { { 0, 5, 6 }, { 3, 7, 1 }, { 2, 8, 4 } };
               
            var board = new Board(arr);

               Console.WriteLine("solving 8 tile puzzle:\n");

            var startingState = new State(board, null, null, 0);

               var astar = new AStarSolver();

            astar.Solve(startingState);


               Console.ReadLine();

               
        }
          
    }
}
