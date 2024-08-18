namespace Lesters_Teeth
{
    internal class LestersTeeth
    {
        const int ZERO = -1;
        const int ONE = 0;
        const int TWO = 1;
        const int THREE = 2;
        const int FOUR = 3;
        const int FIVE = 4;
        const int SIX = 5;

        static int[]? m_Dice = null;
        static int[]? m_Count = null;

        static Random? m_Rnd;

        static void SeedRng(ref string[] args)
        {
            int iSeed = 0;
            bool bSeed = false;
            if (args.Length > 0)
                bSeed = Int32.TryParse(args[0], out iSeed);
            m_Rnd = bSeed ? new Random(iSeed) : new Random();
        }
        static bool IsLargeStraight(int nDice)
        {
            bool bIsLargeStraight = m_Count != null &&
                    nDice == 6 &&
                    m_Count[ONE] == 1 &&
                    m_Count[TWO] == 1 &&
                    m_Count[THREE] == 1 &&
                    m_Count[FOUR] == 1 &&
                    m_Count[FIVE] == 1 &&
                    m_Count[SIX] == 1;
            return bIsLargeStraight;
        }

        static bool Is3Pair(int nDice)
        {
            bool bIs3Pair = false;
            if (nDice == 6)
            {
                int n3Pair = 0;
                if (m_Count != null && m_Count[ONE] == 2)
                    n3Pair++;
                if (m_Count != null && m_Count[TWO] == 2)
                    n3Pair++;
                if (m_Count != null && m_Count[THREE] == 2)
                    n3Pair++;
                if (m_Count != null && m_Count[FOUR] == 2)
                    n3Pair++;
                if (m_Count != null && m_Count[FIVE] == 2)
                    n3Pair++;
                if (m_Count != null && m_Count[SIX] == 2)
                    n3Pair++;
                bIs3Pair = n3Pair == 3;
            }
            return bIs3Pair;
        }

        static int RollPlayer(int iGameScorePlayer, int iPlayerID)
        {
            bool bRollLoop = true;
            int nDice = 6;
            int nRoll = 0;
            int iRollScore = 0;
            bool bBuela = false;
            int nMult = ZERO;
            while (bRollLoop)
            {
                while (true)
                {
                    Console.Write("Press R to roll, H to hold");
                    string strKey = Console.ReadLine();
                    if (strKey == "H" || strKey == "h")
                        return iGameScorePlayer;
                    if (strKey == "R" || strKey == "r")
                        break;
                }

                Console.Write(" Roll {0}: ", nRoll + 1);
                int iTempScore = 0;
                m_Dice = new int[6] { 0, 0, 0, 0, 0, 0 };
                m_Count = new int[6] { 0, 0, 0, 0, 0, 0 };
                for (int iDie = 0; iDie < nDice; iDie++)
                {
                    m_Dice[iDie] = m_Rnd.Next(1, 7);
                    m_Count[m_Dice[iDie] - 1]++;
                    Console.Write("{0} ", m_Dice[iDie]);
                }
                Console.WriteLine();

                bool bWriteCount = true;
                if (bWriteCount)
                {
                    Console.Write("Count {0}: ", nRoll + 1);
                    for (int iDie = 0; iDie < 6; iDie++)
                        Console.Write("{0} ", m_Count[iDie]);
                    Console.WriteLine();
                }
                Console.WriteLine();

                bool bChoose = true;
                do
                {
                    Console.Write("Pick up score dice: ");
                    int[] Count = new int[6] { m_Count[0], m_Count[1], m_Count[2], m_Count[3], m_Count[4], m_Count[5] };
                    int[] Count2 = new int[6] { 0, 0, 0, 0, 0, 0 };
                    string? strDice = Console.ReadLine();
                    if (strDice != null)
                    {
                        bool bError = false;
                        foreach (char cDie in strDice)
                        {
                            if (cDie != ' ')
                            {
                                int iDie = (cDie - '0') - 1;
                                if (iDie >= 0 && iDie <= 5)
                                {
                                    if (Count[iDie] > 0)
                                    {
                                        Count[iDie]--;
                                        Count2[iDie]++;
                                    }
                                    else
                                        bError = true;
                                }
                                else
                                    bError = true;
                            }
                        }
                        if (!bError)
                        {
                            for (int i = 0; i < 6; i++)
                                m_Count[i] = Count2[i];
                            bChoose = false;
                        }
                    }
                } while (bChoose);

                bBuela = true;
                bool bIsLargeStraight = IsLargeStraight(nDice);
                bool bIs3Pair = Is3Pair(nDice);
                if (bIsLargeStraight)
                    bBuela = false;
                else
                {
                    if (bIs3Pair)
                        bBuela = false;
                    else
                    {
                        if (nMult == ONE)
                        {
                            if (m_Count[ONE] > 0)
                                bBuela = false;
                        }
                        else if (m_Count[ONE] >= 3)
                            bBuela = false;
                        else if (nMult == SIX)
                        {
                            if (m_Count[SIX] > 0)
                                bBuela = false;
                        }
                        else if (m_Count[SIX] >= 3)
                            bBuela = false;
                        else if (nMult == FIVE)
                        {
                            if (m_Count[FIVE] > 0)
                                bBuela = false;
                        }
                        else if (m_Count[FIVE] >= 3)
                            bBuela = false;
                        else if (nMult == FOUR)
                        {
                            if (m_Count[FOUR] > 0)
                                bBuela = false;
                        }
                        else if (m_Count[FOUR] >= 3)
                            bBuela = false;
                        else if (nMult == THREE)
                        {
                            if (m_Count[THREE] > 0)
                                bBuela = false;
                        }
                        else if (m_Count[THREE] >= 3)
                            bBuela = false;
                        else if (nMult == TWO)
                        {
                            if (m_Count[TWO] > 0)
                                bBuela = false;
                        }
                        else if (m_Count[TWO] >= 3)
                            bBuela = false;

                        if (nMult == ZERO)
                        {
                            if (m_Count[ONE] > 0 && m_Count[ONE] < 3)
                                bBuela = false;
                            if (m_Count[FIVE] > 0 && m_Count[FIVE] < 3)
                                bBuela = false;
                        }
                    }
                }

                if (!bBuela)
                {
                    // Look for patterns
                    if (bIsLargeStraight)
                    {
                        // Large straight : 1500 points
                        iTempScore += 1500;
                        Console.WriteLine("Large Straight");
                        nDice = 0;
                        nMult = ZERO;
                    }
                    else
                    {
                        if (bIs3Pair)
                        {
                            // 3 Pair : 500 points
                            iTempScore += 500;
                            Console.WriteLine("Three Pair");
                            nDice = 0;
                            nMult = ZERO;
                        }
                        else
                        {
                            if (nMult == ONE)
                            {
                                if (m_Count[ONE] > 0)
                                {
                                    iTempScore += m_Count[ONE] * 1000;
                                    Console.WriteLine("{0} more Ones", m_Count[ONE]);
                                    nDice -= m_Count[ONE];
                                }
                                else
                                    nMult = ZERO;
                            }
                            else if (m_Count[ONE] >= 3)
                            {
                                // Eintausend - 1000
                                iTempScore += (m_Count[ONE] - 2) * 1000;
                                Console.WriteLine("{0} Ones", m_Count[ONE]);
                                nDice -= m_Count[ONE];
                                nMult = ONE;
                            }
                            else if (nMult == SIX)
                            {
                                if (m_Count[SIX] > 0)
                                {
                                    iTempScore += m_Count[SIX] * 600;
                                    Console.WriteLine("{0} more Sixes", m_Count[SIX]);
                                    nDice -= m_Count[SIX];
                                }
                                else
                                    nMult = ZERO;
                            }
                            else if (m_Count[SIX] >= 3)
                            {
                                iTempScore += (m_Count[SIX] - 2) * 600;
                                Console.WriteLine("{0} Sixes", m_Count[SIX]);
                                nDice -= m_Count[SIX];
                                nMult = SIX;
                            }
                            else if (nMult == FIVE)
                            {
                                if (m_Count[FIVE] > 0)
                                {
                                    iTempScore += m_Count[FIVE] * 500;
                                    Console.WriteLine("{0} more Fives", m_Count[FIVE]);
                                    nDice -= m_Count[FIVE];
                                }
                                else
                                    nMult = ZERO;
                            }
                            else if (m_Count[FIVE] >= 3)
                            {
                                iTempScore += (m_Count[FIVE] - 2) * 500;
                                Console.WriteLine("{0} Fives", m_Count[FIVE]);
                                nDice -= m_Count[FIVE];
                                nMult = FIVE;
                            }
                            else if (nMult == FOUR)
                            {
                                if (m_Count[FOUR] > 0)
                                {
                                    iTempScore += m_Count[FOUR] * 400;
                                    Console.WriteLine("{0} more Fours", m_Count[FOUR]);
                                    nDice -= m_Count[FOUR];
                                }
                                else
                                    nMult = ZERO;
                            }
                            else if (m_Count[FOUR] >= 3)
                            {
                                iTempScore += (m_Count[FOUR] - 2) * 400;
                                Console.WriteLine("{0} Fours", m_Count[FOUR]);
                                nDice -= m_Count[FOUR];
                                nMult = FOUR;
                            }
                            else if (nMult == THREE)
                            {
                                if (m_Count[THREE] > 0)
                                {
                                    iTempScore += m_Count[THREE] * 300;
                                    Console.WriteLine("{0} more Threes", m_Count[THREE]);
                                    nDice -= m_Count[THREE];
                                }
                                else
                                    nMult = ZERO;
                            }
                            else if (m_Count[THREE] >= 3)
                            {
                                iTempScore += (m_Count[THREE] - 2) * 300;
                                Console.WriteLine("{0} Threes", m_Count[THREE]);
                                nDice -= m_Count[THREE];
                                nMult = THREE;
                            }
                            else if (nMult == TWO)
                            {
                                if (m_Count[TWO] > 0)
                                {
                                    iTempScore += m_Count[TWO] * 200;
                                    Console.WriteLine("{0} more Twos", m_Count[TWO]);
                                    nDice -= m_Count[TWO];
                                }
                                else
                                    nMult = ZERO;
                            }
                            else if (m_Count[TWO] >= 3)
                            {
                                iTempScore += (m_Count[TWO] - 2) * 200;
                                Console.WriteLine("{0} Twos", m_Count[TWO]);
                                nDice -= m_Count[TWO];
                                nMult = TWO;
                            }

                            if (nMult == ZERO)
                            {
                                if (m_Count[ONE] > 0 && m_Count[ONE] < 3)
                                {
                                    iTempScore += m_Count[ONE] * 100;
                                    Console.WriteLine("Roll {0} - 100 - Score {1} - Total {2}", nRoll + 1, m_Count[0] * 100, iTempScore);
                                    nDice -= m_Count[ONE];

                                    if (m_Count[FIVE] > 0 && m_Count[FIVE] < 3)
                                    {
                                        iTempScore += m_Count[FIVE] * 50;
                                        Console.WriteLine("Roll {0} - 50 - Score {1} - Total {2}", nRoll + 1, m_Count[0] * 50, iTempScore);
                                        nDice -= m_Count[FIVE];
                                    }
                                }
                                else if (m_Count[FIVE] > 0 && m_Count[FIVE] < 3)
                                {
                                    iTempScore += m_Count[FIVE] * 50;
                                    Console.WriteLine("Roll {0} - 50 - Score {1} - Total {2}", nRoll + 1, m_Count[0] * 50, iTempScore);
                                    nDice -= m_Count[FIVE];
                                }
                            }
                        }
                    }
                }

                if (iTempScore == 0)
                {
                    bRollLoop = false;
                    Console.WriteLine("Buela!");
                    bBuela = true;
                }
                else
                {
                    iRollScore += iTempScore;
                    Console.WriteLine("Total Roll Score {0}", iRollScore);

                    if (nMult > ZERO)
                        Console.WriteLine("{0} is the roll multiplier", nMult + 1);
                }
            }
            if (!bBuela)
                iGameScorePlayer += iRollScore;
            Console.WriteLine(string.Format("Player {0} - Score {1}\r\n", iPlayerID, iGameScorePlayer));
            return iGameScorePlayer;
        }

        static int RollCPU(int iGameScoreCPU, int iCPUID)
        {
            bool bRollLoop = true;
            int nDice = 6;
            int nRoll = 0;
            int iRollScore = 0;
            bool bBuela = false;
            int nMult = ZERO;
            while (bRollLoop)
            {
                Console.Write(" Roll {0}: ", nRoll + 1);
                int iTempScore = 0;
                m_Dice = new int[6] { 0, 0, 0, 0, 0, 0 };
                m_Count = new int[6] { 0, 0, 0, 0, 0, 0 };
                for ( int iDie = 0; iDie < nDice; iDie++)
                {
                    m_Dice[iDie] = m_Rnd.Next(1, 7);
                    m_Count[m_Dice[iDie] - 1]++;
                    Console.Write("{0} ", m_Dice[iDie]);
                }
                Console.WriteLine();

                bool bWriteCount = true;
                if (bWriteCount)
                {
                    Console.Write("Count {0}: ", nRoll + 1);
                    for (int iDie = 0; iDie < 6; iDie++)
                        Console.Write("{0} ", m_Count[iDie]);
                    Console.WriteLine();
                }

                bool bIsLargeStraight = IsLargeStraight(nDice);
                bool bIs3Pair = Is3Pair(nDice);

                // Look for patterns
                if (bIsLargeStraight)
                {
                    // Large straight : 1500 points
                    iTempScore += 1500;
                    Console.WriteLine("Large Straight");
                    nDice = 0;
                    nMult = ZERO;
                }
                else
                {
                    if (bIs3Pair)
                    {
                        // 3 Pair : 500 points
                        iTempScore += 500;
                        Console.WriteLine("Three Pair");

                        nDice = 0;
                        nMult = ZERO;
                    }
                    else
                    {
                        if (nMult == ONE)
                        {
                            if (m_Count[ONE] > 0)
                            {
                                iTempScore += m_Count[ONE] * 1000;
                                Console.WriteLine("{0} more Ones", m_Count[ONE]);
                                nDice -= m_Count[ONE];
                            }
                            else
                                nMult = ZERO;
                        }
                        else if (m_Count[ONE] >= 3)
                        {
                            // Eintausend - 1000
                            iTempScore += (m_Count[ONE] - 2) * 1000;
                            Console.WriteLine("{0} Ones", m_Count[ONE]);
                            nDice -= m_Count[ONE];
                            nMult = ONE;
                        }
                        else if (nMult == SIX)
                        {
                            if (m_Count[SIX] > 0)
                            {
                                iTempScore += m_Count[SIX] * 600;
                                Console.WriteLine("{0} more Sixes", m_Count[SIX]);
                                nDice -= m_Count[SIX];
                            }
                            else
                                nMult = ZERO;
                        }
                        else if (m_Count[SIX] >= 3)
                        {
                            iTempScore += (m_Count[SIX] - 2) * 600;
                            Console.WriteLine("{0} Sixes", m_Count[SIX]);
                            nDice -= m_Count[SIX];
                            nMult = SIX;
                        }
                        else if (nMult == FIVE)
                        {
                            if (m_Count[FIVE] > 0)
                            {
                                iTempScore += m_Count[FIVE] * 500;
                                Console.WriteLine("{0} more Fives", m_Count[FIVE]);
                                nDice -= m_Count[FIVE];
                            }
                            else
                                nMult = ZERO;
                        }
                        else if (m_Count[FIVE] >= 3)
                        {
                            iTempScore += (m_Count[FIVE] - 2) * 500;
                            Console.WriteLine("{0} Fives", m_Count[FIVE]);
                            nDice -= m_Count[FIVE];
                            nMult = FIVE;
                        }
                        else if (nMult == FOUR)
                        {
                            if (m_Count[FOUR] > 0)
                            {
                                iTempScore += m_Count[FOUR] * 400;
                                Console.WriteLine("{0} more Fours", m_Count[FOUR]);
                                nDice -= m_Count[FOUR];
                            }
                            else
                                nMult = ZERO;
                        }
                        else if (m_Count[FOUR] >= 3)
                        {
                            iTempScore += (m_Count[FOUR] - 2) * 400;
                            Console.WriteLine("{0} Fours", m_Count[FOUR]);
                            nDice -= m_Count[FOUR];
                            nMult = FOUR;
                        }
                        else if (nMult == THREE)
                        {
                            if (m_Count[THREE] > 0)
                            {
                                iTempScore += m_Count[THREE] * 300;
                                Console.WriteLine("{0} more Threes", m_Count[THREE]);
                                nDice -= m_Count[THREE];
                            }
                            else
                                nMult = ZERO;
                        }
                        else if (m_Count[THREE] >= 3)
                        {
                            iTempScore += (m_Count[THREE] - 2) * 300;
                            Console.WriteLine("{0} Threes", m_Count[THREE]);
                            nDice -= m_Count[THREE];
                            nMult = THREE;
                        }
                        else if (nMult == TWO)
                        {
                            if (m_Count[TWO] > 0)
                            {
                                iTempScore += m_Count[TWO] * 200;
                                Console.WriteLine("{0} more Twos", m_Count[TWO]);
                                nDice -= m_Count[TWO];
                            }
                            else
                                nMult = ZERO;
                        }
                        else if (m_Count[TWO] >= 3)
                        {
                            iTempScore += (m_Count[TWO] - 2) * 200;
                            Console.WriteLine("{0} Twos", m_Count[TWO]);
                            nDice -= m_Count[TWO];
                            nMult = TWO;
                        }

                        if (nMult == ZERO)
                        {
                            // How to strand points to let roll loop know to pick up the extra on the end

                            // Eventually have logic to choose 1's or 5's
                            if (m_Count[ONE] > 0 && m_Count[ONE] < 3) // 1s
                            {
                                iTempScore += m_Count[ONE] * 100;
                                Console.WriteLine("Roll {0} - 100 - Score {1} - Total {2}", nRoll + 1, m_Count[0] * 100, iTempScore);
                                nDice -= m_Count[ONE];

                                if (m_Count[FIVE] > 0 && m_Count[FIVE] < 3) // 5s
                                {
                                    iTempScore += m_Count[FIVE] * 50;
                                    Console.WriteLine("Roll {0} - 50 - Score {1} - Total {2}", nRoll + 1, m_Count[0] * 50, iTempScore);
                                    nDice -= m_Count[FIVE];
                                }
                            }
                            else if (m_Count[FIVE] > 0 && m_Count[FIVE] < 3) // 5s
                            {
                                iTempScore += m_Count[FIVE] * 50;
                                Console.WriteLine("Roll {0} - 50 - Score {1} - Total {2}", nRoll + 1, m_Count[0] * 50, iTempScore);
                                nDice -= m_Count[FIVE];
                            }
                        }
                    }
                }

                if (iTempScore == 0)
                {
                    bRollLoop = false;
                    Console.WriteLine("Buela!");
                    bBuela = true;
                }
                else
                {
                    iRollScore += iTempScore;
                    Console.WriteLine("Total Roll Score {0}", iRollScore);

                    if (nMult > ZERO)
                        Console.WriteLine("{0} is the roll multiplier", nMult + 1);

                    int iChance = m_Rnd.Next(1, 101);
                    if (iGameScoreCPU < 500)
                    {
                        if (iGameScoreCPU + iRollScore >= 500)
                        {
                            if (nDice > 0)
                            {
                                if (nMult > ZERO)
                                {
                                    // Could have 3, 2, or 1 dice left.  Higher multiplier makes higher score. Determine chance of going for it!
                                    if (nMult == ONE || nMult == FIVE || nMult == SIX)
                                    {
                                        int GT = nMult == ONE ? 80 : (nMult == FIVE ? 90 : 100);
                                        if ((iChance * nDice) < GT)
                                        {
                                            Console.WriteLine("Not going for it! It's Your turn");
                                            bRollLoop = false;
                                        }
                                        else
                                            Console.WriteLine("Going for it!");
                                    }
                                    else
                                    {
                                        if (iChance < 90)
                                        {
                                            Console.WriteLine("Not going for it! It's Your turn");
                                            bRollLoop = false;
                                        }
                                        else
                                            Console.WriteLine("Going for it!");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("On the board! It's Your turn");
                                    bRollLoop = false;
                                }
                            }
                            else
                            {
                                if (iChance > 75)
                                {
                                    Console.WriteLine("Going for it, Fresh Teeth!");
                                    nDice = 6;
                                }
                                else
                                {
                                    Console.WriteLine("Not going for it. On the board! It's Your turn");
                                    bRollLoop = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (nDice == 0)
                        {
                            if (iRollScore > 2000)
                            {
                                Console.WriteLine("Fresh Teeth, but not going for it. Your turn");
                                bRollLoop = false;
                            }
                            else
                            {
                                Console.WriteLine("Fresh Teeth");
                                nDice = 6;
                            }
                        }
                        else
                        {
                            if (iRollScore > 500)
                            {
                                if (nDice <= 2)
                                {
                                    Console.WriteLine("Your turn");
                                    bRollLoop = false;
                                }
                            }

                            if (bRollLoop)
                                Console.WriteLine("Go Teeth!");
                        }
                        nRoll++;
                    }
                    Console.WriteLine();
                }
            }
            if (!bBuela)
                iGameScoreCPU += iRollScore;
            Console.WriteLine(string.Format("CPU {0} - Score {1}\r\n", iCPUID, iGameScoreCPU));
            return iGameScoreCPU;
        }

        static void Main(string[] args)
        {
            SeedRng(ref args);

            Console.WriteLine("Lesters Teeth\r\n");

            bool bConvert = false;
            int nPlayers = 0, nCPUs = 0, nPeople = 0;
            do
            {
                Console.Write("Number of players? ");
                string? strPlayers = Console.ReadLine();
                bConvert = Int32.TryParse(strPlayers, out nPlayers);
                if (bConvert)
                {
                    if (nPlayers < 2)
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

            Console.WriteLine("\r\nStarting order is Player first, CPU second. Good Luck!\r\n");

            int[] PlayerScore = new int[nPlayers];

            bool bGameLoop = true;
            while (bGameLoop)
            {
                Console.WriteLine("Lesters Teeth\r\n");
                for (int iPlayer = 0; iPlayer < nPlayers; iPlayer++)
                {
                    if (iPlayer < nPeople)
                    {
                        PlayerScore[iPlayer] = RollPlayer(PlayerScore[iPlayer], iPlayer + 1);
                    }
                    else
                    {
                        PlayerScore[iPlayer] = RollCPU(PlayerScore[iPlayer], iPlayer + 1);
                    }

                    if (PlayerScore[iPlayer] > 10000)
                        bGameLoop = false;
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
                        Console.WriteLine("CPU {0} scores {1} and wins!", iPlayer + 1 - nPeople, PlayerScore[iPlayer]);
                }
                else
                {
                    if (iPlayer < nPeople)
                        Console.WriteLine("Player {0} scores {1} and loses!", iPlayer + 1, PlayerScore[iPlayer]);
                    else
                        Console.WriteLine("CPU {0} scores {1} and loses!", iPlayer + 1 - nPeople, PlayerScore[iPlayer]);
                }
            }
        }
    }
}
