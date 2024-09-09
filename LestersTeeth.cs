using System;
using System.Threading;

class Program
{
    const int WAIT = 1000;

    static Random SeedRng(ref string[] args)
    {
        int seed = (args.Length > 0) ? int.Parse(args[0]) : DateTime.Now.Millisecond;
        return new Random(seed);
    }

    static int RollPlayer(ref Random Rnd, int currentScore, int playerNumber)
    {
        return currentScore;
    }

    static int RollCPU(ref Random Rnd, int currentScore, int cpuNumber)
    {
        return currentScore;
    }
    static void Scoring()
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

    static void SetupPlayers(out int nPlayers, out int nCPUs, out int nPeople)
    {
        nCPUs = 0;
        nPeople = 0;

        bool bConvert;
        do
        {
            Console.Write("Number of players? ");
            string? strPlayers = Console.ReadLine();
            bConvert = Int32.TryParse(strPlayers, out nPlayers);
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
                    bConvert = Int32.TryParse(strCPUs, out nCPUs);
                    if (!bConvert)
                        Console.WriteLine("Input error, try again");
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
                                nPeople = nPlayers - nCPUs;
                        }
                    }
                }
            }
            else
                Console.WriteLine("Input error, try again");
        } while (!bConvert);
    }

    static void Main(string[] args)
    {
        Random Rnd = SeedRng(ref args);

        bool bAgain = true;

        while (bAgain)
        {
            Scoring();

            SetupPlayers(out int nPlayers, out int nCPUs, out int nPeople);

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

                    if (iPlayer < nPeople)
                        PlayerScore[iPlayer] = RollPlayer(ref Rnd, PlayerScore[iPlayer], iPlayer + 1);
                    else
                        PlayerScore[iPlayer] = RollCPU(ref Rnd, PlayerScore[iPlayer], iPlayer + 1);

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

                    if (PlayerScore[iPlayer] > 10000)
                    {
                        bGameLoop = false;
                        break;
                    }
                }
            }

            Console.WriteLine();
            for (int iPlayer = 0; iPlayer < nPlayers; iPlayer++)
            {
                if (PlayerScore[iPlayer] > 10000)
                {
                    if (iPlayer < nPeople)
                        Console.WriteLine("Player {0} scores {1} and wins!", iPlayer + 1, PlayerScore[iPlayer]);
                    else
                        Console.WriteLine("CPU {0} scores {1} and wins!", iPlayer - nPeople + 1, PlayerScore[iPlayer]);
                }
                else
                {
                    if (iPlayer < nPeople)
                        Console.WriteLine("Player {0} scores {1} and loses", iPlayer + 1, PlayerScore[iPlayer]);
                    else
                        Console.WriteLine("CPU {0} scores {1} and loses", iPlayer - nPeople + 1, PlayerScore[iPlayer]);
                }
            }

            Console.WriteLine();
            Console.WriteLine("Press A to play again.\r\n");
        }
    }
}
