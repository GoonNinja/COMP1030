using System;

namespace MMR_Game;
class Program
{
   // Define the main entry point of the program
        public static void Main(string[] args)
        {
            // Create a new game instance and start the game
            Game game = new Game();
            game.DisplayWelcomeMenu();
            game.Start();
        }
}
