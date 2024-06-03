// https://cs50.harvard.edu/x/2024/psets/2/scrabble/

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Scrabble
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine(
                "====================\n" +
                "      SCRABBLE      \n" +
                "====================\n"
            );


            Player[] players = GetPlayers();
            int round = 0;
            
            // Handle ctrl + c 
            Console.CancelKeyPress += delegate
            {
                Console.WriteLine(
                    "\n\n====================" +
                    "\n     GAME OVER      " +
                    "\n====================\n" 
                );

                // Remove last round inputs if game exited mid-round
                int[] playersRounds = new int[players.Length];
                for ( int i = 0; i < players.Length; i++ )
                {
                    playersRounds[i] = players[i].WordsPlayed.Count;
                }

                round = playersRounds.Min();
                foreach ( Player player in players )
                {
                    if (player.WordsPlayed.Count > round )
                    {
                        player.WordsPlayed.RemoveAt( player.WordsPlayed.Count - 1);
                    }
                }

                // Find and declare the Winner(s)
                int[] playerWins = new int[players.Length]; 
                for( int i = 0;i < players.Length; i++ )
                {
                    playerWins[i] = players[i].Wins;
                }

                int winnerScore = playerWins.Max();
                int winnersCount = 0;
                foreach ( Player player in players )
                {
                    Console.WriteLine($"{player.Name}: {player.Wins} Wins");
                    if (player.Wins ==  winnerScore)
                    {
                        winnersCount++;
                    }
                }

                if ( winnersCount > 1 )
                {
                    Console.WriteLine();
                    foreach ( Player player in players )
                    {
                        if (player.Wins == winnerScore)
                        {
                            Console.Write($"{player.Name}, ");
                        }
                    }
                    Console.Write("Tied The Game! Good Job!\n");
                }
                else
                {
                    foreach( Player player in players )
                    {
                        if (player.Wins == winnerScore)
                        {
                            Console.WriteLine($"\n{player.Name} is the WINNER!!! Good Job!\n");
                            break;
                        }
                    }
                }
                return;
            };
            
            while (true)
            {

                PlayRound(players, round + 1);
                round++;
            }
        }

        static void PlayRound(Player[] players, int round)
        {
            Console.WriteLine(
                "\n====================\n" +
               $"      ROUND {round}     \n" +
                "===================="
                );

            foreach ( Player player in players )
            {
                player.GetNewWord();
            }

            int[] scores = new int[players.Length];
            for (int i = 0; i < players.Length; i++)
            {
                scores[i] = players[i].CurrentWord.Score;
            }

            int maxScore = scores.Max();
            int maxScoreCount = 0;
            foreach ( Player player in players )
            {
                if (player.CurrentWord.Score == maxScore)
                {
                    maxScoreCount++;  
                }
            }

            if ( maxScoreCount > 1 )
            {
                foreach ( Player player in players )
                {
                    if (player.CurrentWord.Score == maxScore)
                    {
                        Console.Write($"{player.Name}, ");
                        player.Wins++;
                    }
                }
                Console.Write("are Tied! Each get a point\n");
            }
            else
            {
                foreach( Player player in players )
                {
                    if (player.CurrentWord.Score == maxScore)
                    {
                        Console.WriteLine($"{player.Name} Wins!");
                        player.Wins++;
                    }
                }
            }
        }

        static Player[] GetPlayers()
        {
            Console.Write("How Many Players? ");
            int playerCount; 

            try
            {
                playerCount = Convert.ToInt32(Console.ReadLine().Trim());
                if (playerCount < 2 || playerCount > 4)
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid number of players. Players must be at least 2, and at most 4 people\n");
                return GetPlayers();
            }

            bool wantsDefaultNames = true;
            Console.Write("Should I use default names for the Players (Y/N)? ");
            string answer = Console.ReadLine().Trim();
            if (answer.ToLower().Equals("n") || answer.ToLower().Equals("no"))
            {
                wantsDefaultNames = false;
            }

            Player[] players = new Player[playerCount];
            for (int i = 0; i < playerCount; i++)
            {
                if (!wantsDefaultNames)
                { 
                    Console.Write($"Player {i + 1}'s Name: ");
                    string currPlayerName = Console.ReadLine().Trim();

                    if (string.IsNullOrEmpty(currPlayerName))
                    {
                        Console.WriteLine("Name cannot be empty, try again");
                        i--;
                        continue;
                    }

                    players[i] = new Player(currPlayerName);
                }
                else
                {
                    players[i] = new Player();
                }
            }

            return players;
        }

        class Player
        {
            public Player(string? name = null)
            {
                TotalPlayerCount++;
                if (string.IsNullOrEmpty(name))
                {
                    Name = $"Player {TotalPlayerCount}";
                }
                else
                {
                    Name = name;
                }
            }
            public readonly Dictionary<char, int> ALPHABET_SCORES = new()
            {
                {'a', 1},
                {'b', 3},
                {'c', 3},
                {'d', 2},
                {'e', 1},
                {'f', 4},
                {'g', 2},
                {'h', 4},
                {'i', 1},
                {'j', 8},
                {'k', 5},
                {'l', 1},
                {'m', 3},
                {'n', 1},
                {'o', 1},
                {'p', 3},
                {'q', 10},
                {'r', 1},
                {'s', 1},
                {'t', 1},
                {'u', 1},
                {'v', 4},
                {'w', 4},
                {'x', 8},
                {'y', 4},
                {'z', 10},
            };
            private static int TotalPlayerCount = 0;

            public readonly string Name;
            public Word? CurrentWord = null;
            public List<Word> WordsPlayed = [];
            public int Wins = 0;
            
            public class Word(string word, int score)
            {
                public readonly string Content = word;
                public readonly int Score = score;
            }

            public bool GetNewWord()
            {
                Console.Write($"{Name}: ");
                string? input = Console.ReadLine()
                                       .Trim();

                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Can't proceed with empy input");
                    return GetNewWord();
                }

                int wordScore = 0;
                foreach (char letter in input.ToLower())
                {
                    if (char.IsLetter(letter)) 
                    { 
                        wordScore += ALPHABET_SCORES[letter];
                    }
                }

                CurrentWord = new Word(input, wordScore);
                WordsPlayed.Add(CurrentWord);
                return true;
            }
        }
    }
}