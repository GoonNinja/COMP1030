using System;
using System.Linq;

namespace MMR_Game
{
    class Game
    {
        // Define the Game class

        // Initialize the player and pc HP values and the heal cooldown flag
        private int playerHp = 100;
        private int pcHp = 100;
        private int playerHealCooldown = 0;
        private int pcHealCooldown = 0;

        // Initialize player and pc level and experience points
        private int playerLevel = 1;
        private int pcLevel = 1;
        private int playerExp = 0;
        private int pcExp = 0;
        private int expToLevelUp = 100;

        // Initialize the random number generator and the array of available choices
        private Random random = new Random();
        private string[] choices = { "melee", "magic", "ranged", "block", "heal" };

        // Display the welcome menu with game instructions
        public void DisplayWelcomeMenu()
        {
            Console.WriteLine();
            Console.WriteLine("Welcome to Melee, Magic, Ranged! Fight your way to victory using");
            Console.WriteLine("various attacks, blocks, and healing potions. In this game, you are");
            Console.WriteLine("fighting the pc in an intense 1v1 battle, you are provided with these");
            Console.WriteLine("options: Attack, Block, Heal.");
            Console.WriteLine();
            Console.WriteLine("Attack: Melee, Ranged, Magic. Attacks do different damage based on level.");
            Console.WriteLine("Counters: Magic > Melee, Ranged > Magic, Melee > Ranged.");
            Console.WriteLine("Block: Blocks 50% incoming damage.");
            Console.WriteLine("Heal: Heals 15hp but has 1 turn cooldown. Healing during attack negates all dmg.");
            Console.WriteLine("Level up: Gain experience points and level up to increase attack power and health.");
            Console.WriteLine();
            Console.WriteLine("Press any key to start...");
            Console.ReadKey();
            Console.Clear();
        }

        // Start the game loop
        public void Start()
        {
            // Continue the game loop as long as both players have health
            while (playerHp > 0 && pcHp > 0)
            {
                // Display current health status, level, and experience points
                // and get player and pc choices
                DisplayStatus();
                string playerChoice = GetPlayerChoice();
                string pcChoice = GetPcChoice();
                Console.WriteLine($"PC chose {pcChoice}");
                Console.WriteLine();

                // Process the choices for the current turn
                ProcessTurn(playerChoice, pcChoice);
            }

            // Display the end of the game message
            DisplayEndMessage();
        }

        // Display the current health status of the player and pc, level, and experience points
        private void DisplayStatus()
        {
            Console.WriteLine($"Player HP: {playerHp} |-| PC HP: {pcHp}");
            Console.WriteLine($"Player Level: {playerLevel} |-| PC Level: {pcLevel}");
            Console.WriteLine($"Player Experience: {playerExp}/{expToLevelUp} |-| PC Experience: {pcExp}/{expToLevelUp}");
            Console.WriteLine();
        }

        // Get the player's choice for their action
        private string GetPlayerChoice()
        {
            string playerChoice;
            do
            {
                Console.WriteLine("Choose your action (melee, magic, ranged, block, heal):");
                Console.WriteLine();
                playerChoice = (Console.ReadLine() ?? string.Empty).ToLower();

                if (playerChoice == "heal" && playerHealCooldown > 0)
                {
                    Console.WriteLine("Heal is on cooldown.");
                }
                else if (!choices.Contains(playerChoice))
                {
                    Console.WriteLine("Invalid choice. Please try again.");
                }
                else
                {
                    break;
                }
            } while (true);

            return playerChoice;
        }

        // Get the pc's choice for their action
        private string GetPcChoice()
        {
            int pcChoiceIndex;
            do
            {
                // Generate a random index and use it to select the pc's choice
                pcChoiceIndex = random.Next(0, 5);

                // If heal is on cooldown for the pc, generate a new random choice
                if (choices[pcChoiceIndex] == "heal" && pcHealCooldown > 0)
                {
                    continue;
                }
                break;
            } while (true);

            return choices[pcChoiceIndex];
        }

        // Calculate damage based on attacker and defender choices
        private int CalculateDamage(string attacker, string defender, int attackerLevel)
        {
            int baseDamage = attackerLevel * 5;

            if (attacker == defender) return baseDamage;
            if ((attacker == "melee" && defender == "ranged") || (attacker == "magic" && defender == "melee") || (attacker == "ranged" && defender == "magic"))
            {
                return baseDamage * 2;
            }
            return baseDamage;
        }

        // Process a single turn based on the player and pc choices
        private void ProcessTurn(string playerChoice, string pcChoice)
        {
            int playerDamage = 0;
            int pcDamage = 0;

            switch (playerChoice)
            {
                case "melee":
                case "magic":
                case "ranged":
                    pcDamage = pcChoice != "block" && pcChoice != "heal" ? CalculateDamage(playerChoice, pcChoice, playerLevel) : 0;
                    break;
                case "block":
                    break;
                case "heal":
                    playerHp = Math.Min(playerHp + 15, 100);
                    playerHealCooldown = 2;
                    break;
            }

            switch (pcChoice)
            {
                case "melee":
                case "magic":
                case "ranged":
                    playerDamage = playerChoice != "block" && playerChoice != "heal" ? CalculateDamage(pcChoice, playerChoice, pcLevel) : 0;
                    break;
                case "block":
                    break;
                case "heal":
                    pcHp = Math.Min(pcHp + 15, 100);
                    pcHealCooldown = 2;
                    break;
            }

            // Apply the damage to both the player and pc
            playerHp -= playerDamage;
            pcHp -= pcDamage;

            if (playerChoice == "block")
            {
                playerHp += Math.Min(5, playerDamage);
            }

            if (pcChoice == "block")
            {
                pcHp += Math.Min(5, pcDamage);
            }

            // Player cooldown reset
            if (playerHealCooldown > 0)
            {
                playerHealCooldown--;
            }

            // PC cooldown reset
            if (pcHealCooldown > 0)
            {
                pcHealCooldown--;
            }

            // Gain experience points after each turn
            playerExp += 10;
            pcExp += 10;

            // Level up if enough experience points are gained
            if (playerExp >= expToLevelUp)
            {
                playerLevel++;
                playerHp += 20; // Increase max HP by 20
                playerExp = 0;
                Console.WriteLine("You leveled up!");
            }

            if (pcExp >= expToLevelUp)
            {
                pcLevel++;
                pcHp += 20; // Increase max HP by 20
                pcExp = 0;
            }
        }

        // Display the end message based on the result of the game
        private void DisplayEndMessage()
        {
            // If the player's health is 0 or less, they lost
            if (playerHp <= 0)
            {
                Console.WriteLine("You lost!");
            }
            // If the player's health is above 0, they won
            else
            {
                Console.WriteLine("You won!");
            }
        }
    }
}