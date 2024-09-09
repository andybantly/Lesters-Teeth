using System;
using System.Threading;

internal class Program
{
    private const int WAIT = 1000;
    private const int WINNING_SCORE = 10000;
    private const int DICE_SIDES = 6;
    private const int DICE_COUNT = 5;

    private static Random SeedRng(ref string[] args)
    {
        int seed = (args.Length > 0) ? int.Parse(args[0]) : DateTime.Now.Millisecond;
        return new Random(seed);
    }

    private static int RollPlayer(ref Random Rnd, int currentScore, int playerNumber)
    {
        int rollScore = RollDice(ref Rnd);
        currentScore += rollScore;
        Console.WriteLine("Player {0} rolls and scores {1} points, total score: {2}", playerNumber, rollScore, currentScore);
        return currentScore;
    }

    private static int RollCPU(ref Random Rnd, int currentScore, int cpuNumber)
    {
        int rollScore = RollDice(ref Rnd);
        currentScore += rollScore;
        Console.WriteLine("CPU {0} rolls and scores {1} points, total score: {2}", cpuNumber, rollScore, currentScore);
        return currentScore;
    }

    private static int RollDice(ref Random Rnd)
    {
        int[] dice = new int[DICE_COUNT];
        int[] counts = new int[DICE_SIDES];
        int score = 0;

        // Roll the dice and count occurrences
        for (int i = 0; i < DICE_COUNT; i++)
        {
            dice[i] = Rnd.Next(1, DICE_SIDES + 1);
            counts[dice[i] - 1]++;
        }

        // Calculate the score based on dice counts
        if (IsStraight(counts))
        {
            score += 1500;
        }
        else
        {
            score += CalculateScore(counts);
        }

        return score;
    }

    private static bool IsStraight(int[] counts)
    {
        for (int i = 0; i < DICE_SIDES; i++)
        {
            if (counts[i] != 1)
            {
                return false;
            }
        }
        return true;
    }

    private static int CalculateScore(int[] counts)
    {
        int score = 0;

        // Scoring rules
        if (counts[0] >= 3)
        {
            score += 1000 + ((counts[0] - 3) * 1000);  // Three or more ones
        }

        if (counts[0] < 3)
        {
            score += counts[0] * 100;  // Single ones
        }

        if (counts[4] >= 3)
        {
            score += 500 + ((counts[4] - 3) * 500);   // Three or more fives
        }

        if (counts[4] < 3)
        {
            score += counts[4] * 50;   // Single fives
        }

        // Scoring for other triples
        for (int i = 1; i < DICE_SIDES; i++)
        {
            if (counts[i] >= 3)
            {
                score += (i + 1) * 100 * (counts[i] - 2);
            }
        }

        return score;
    }

    private static void Scoring()
    {
        Console.WriteLine("Lesters Teeth");
        Console.WriteLine();
        Console.WriteLine("Scoring:\r\n");
        Console.WriteLine("Straight - 1500");
        Console.WriteLine("3 Ones   - 1000 - Additional 1000 each");
        Console.WriteLine("3 Sixes  -  600 - Additional  600 each");
        Console.WriteLine("3 Fives  -  500 - Additional  500 each");
        Console.WriteLine("3 Fours  -  400 - Additional  400 each");
        Console.WriteLine("3 Threes -  300 - Additional  300 each");
        Console.WriteLine("3 Twos   -  200 - Additional  200 each");
        Console.WriteLine("3 Pair   -  500");
        Console.WriteLine("1 One    -  100");
        Console.WriteLine("1 Five   -   50");
        Console.WriteLine();
    }

    private static void SetupPlayers(out int nPlayers, out int nCPUs, out int nPeople)
    {
        nCPUs = 0;
        nPeople = 0;

        bool bConvert;
        do
        {
            Console.Write("Number of players? ");
            string? strPlayers = Console.ReadLine();
            bConvert = int.TryParse(strPlayers, out nPlayers);
            if (bConvert)
            {
                if (nPlayers < 1)
                {
                    Console.WriteLine("Not enough players");
                    bConvert = false;
                }
                else
                {
                    Console.Write("How many are CPU? ");
                    string? strCPUs = Console.ReadLine();
                    bConvert = int.TryParse(strCPUs, out nCPUs);
                    if (!bConvert)
                    {
                        Console.WriteLine("Input error, try again");
                    }
                    else
                    {
                        if (nCPUs < 0)
                        {
                            Console.WriteLine("There must be 0 or more CPU players");
                            bConvert = false;
                        }
                        else
                        {
                            if (nCPUs > nPlayers)
                            {
                                Console.WriteLine("Too many CPUs");
                                bConvert = false;
                            }
                            else
                            {
                                nPeople = nPlayers - nCPUs;
                            }
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("Input error, try again");
            }
        } while (!bConvert);
    }

    private static void Main(string[] args)
    {
        Random Rnd = SeedRng(ref args);

        bool bAgain = true;

        while (bAgain)
        {
            Scoring();
            SetupPlayers(out int nPlayers, out _, out int nPeople);

            Console.WriteLine("\r\nStarting order is Player(s) first, CPU(s) second. Good Luck!\r\n");

            int[] PlayerScore = new int[nPlayers];

            int iTurn = 0;
            bool bGameLoop = true;
            while (bGameLoop)
            {
                Console.WriteLine("Lesters Teeth\r\nTurn {0}\r\n", ++iTurn);
                for (int iPlayer = 0; iPlayer < nPlayers; iPlayer++)
                {
                    Console.WriteLine(string.Format("{0} {1}",
                    iPlayer < nPeople ? "Player" : "CPU",
                    iPlayer < nPeople ? iPlayer + 1 : iPlayer - nPeople + 1
                    ));

                    PlayerScore[iPlayer] = iPlayer < nPeople
                        ? RollPlayer(ref Rnd, PlayerScore[iPlayer], iPlayer + 1)
                        : RollCPU(ref Rnd, PlayerScore[iPlayer], iPlayer + 1);

                    Console.WriteLine("Score");
                    for (int jPlayer = 0; jPlayer < nPlayers; jPlayer++)
                    {
                        Console.WriteLine(string.Format("{0} {1} - Score {2}",
                        jPlayer < nPeople ? "Player" : "   CPU",
                        jPlayer < nPeople ? jPlayer + 1 : jPlayer - nPeople + 1,
                        PlayerScore[jPlayer]));
                    }
                    Console.WriteLine();
                    Thread.Sleep(WAIT);

                    if (PlayerScore[iPlayer] > WINNING_SCORE)
                    {
                        bGameLoop = false;
                        break;
                    }
                }
            }

            Console.WriteLine();
            for (int iPlayer = 0; iPlayer < nPlayers; iPlayer++)
            {
                if (PlayerScore[iPlayer] > WINNING_SCORE)
                {
                    if (iPlayer < nPeople)
                    {
                        Console.WriteLine("Player {0} scores {1} and wins!", iPlayer + 1, PlayerScore[iPlayer]);
                    }
                    else
                    {
                        Console.WriteLine("CPU {0} scores {1} and wins!", iPlayer - nPeople + 1, PlayerScore[iPlayer]);
                    }
                }
                else
                {
                    if (iPlayer < nPeople)
                    {
                        Console.WriteLine("Player {0} scores {1} and loses", iPlayer + 1, PlayerScore[iPlayer]);
                    }
                    else
                    {
                        Console.WriteLine("CPU {0} scores {1} and loses", iPlayer - nPeople + 1, PlayerScore[iPlayer]);
                    }
                }
            }

            Console.WriteLine();
            Console.WriteLine("Press A to play again.\r\n");
            ConsoleKeyInfo cki = Console.ReadKey(true);
            string strKey = cki.KeyChar.ToString();
            if (strKey is not "A" and not "a")
            {
                bAgain = false;
            }
        }
    }
}
