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
                    if (iRollScore >= 500 || iGameScorePlayer >= 500)
                        Console.WriteLine("Press R to roll, S to score");
                    else
                        Console.WriteLine("Press R to roll");
                    ConsoleKeyInfo cki = Console.ReadKey(true);
                    string strKey = cki.KeyChar.ToString();
                    if (iRollScore >= 500 || iGameScorePlayer >= 500)
                    {
                        if (strKey == "S" || strKey == "s")
                        {
                            bRollLoop = false;
                            break;
                        }
                    }
                    if (strKey == "R" || strKey == "r")
                        break;
                }

                if (!bRollLoop)
                    break;

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

#if DEBUG
                bool bWriteCount = true;
                if (bWriteCount)
                {
                    Console.Write("Count {0}: ", nRoll + 1);
                    for (int iDie = 0; iDie < 6; iDie++)
                        Console.Write("{0} ", m_Count[iDie]);
                    Console.WriteLine();
                }
                Console.WriteLine();
#endif

                bBuela = true;
                if (IsLargeStraight(nDice) || Is3Pair(nDice))
                    bBuela = false;
                else
                {
                    if (nMult == ONE)
                    {
                        if (m_Count[ONE] > 0)
                            bBuela = false;
                    }
                    else if (m_Count[ONE] > 2)
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

                    if (m_Count[ONE] > 0 && m_Count[ONE] < 3)
                        bBuela = false;
                    if (m_Count[FIVE] > 0 && m_Count[FIVE] < 3)
                        bBuela = false;
                }

                if (bBuela)
                {
                    bRollLoop = false;
                    Console.WriteLine("Buela!");
                    bBuela = true;
                }
                else
                {
                    do
                    {
                        bool bChoose = true;
                        do
                        {
                            int[] Count = new int[6] { m_Count[0], m_Count[1], m_Count[2], m_Count[3], m_Count[4], m_Count[5] };
                            int[] Count2 = new int[6] { 0, 0, 0, 0, 0, 0 };

                            Console.Write("Pick up score dice: ");
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

                        // Look for patterns
                        bool bIsLargeStraight = IsLargeStraight(nDice);
                        if (bIsLargeStraight)
                        {
                            // Large straight : 1500 points
                            iTempScore += 1500;
                            Console.WriteLine("Roll {0} - Large Straight - Total {1}", nRoll + 1, iTempScore);
                            for (int iDie = ONE; iDie <= SIX; iDie++)
                                m_Count[iDie] = 0;
                            nDice = 0;
                            nMult = ZERO;
                        }
                        else
                        {
                            bool bIs3Pair = Is3Pair(nDice);
                            if (bIs3Pair)
                            {
                                // 3 Pair : 500 points
                                iTempScore += 500;
                                Console.WriteLine("Roll {0} - Three Pair - Total {1}", nRoll + 1, iTempScore);
                                for (int iDie = ONE; iDie <= SIX; iDie++)
                                    m_Count[iDie] = 0;
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
                                        Console.WriteLine("Roll {0} - {1} Ones - Total {2}", nRoll + 1, m_Count[ONE], iTempScore);
                                        nDice -= m_Count[ONE];
                                        m_Count[ONE] = 0;
                                    }
                                    else
                                        nMult = ZERO;
                                }
                                else if (m_Count[ONE] >= 3)
                                {
                                    // Eintausend - 1000
                                    iTempScore += (m_Count[ONE] - 2) * 1000;
                                    Console.WriteLine("Roll {0} - {1} Ones - Total {2}", nRoll + 1, m_Count[ONE], iTempScore);
                                    nDice -= m_Count[ONE];
                                    m_Count[ONE] = 0;
                                    nMult = ONE;
                                }
                                else if (nMult == SIX)
                                {
                                    if (m_Count[SIX] > 0)
                                    {
                                        iTempScore += m_Count[SIX] * 600;
                                        Console.WriteLine("Roll {0} - {1} Sixes - Total {2}", nRoll + 1, m_Count[SIX], iTempScore);
                                        nDice -= m_Count[SIX];
                                        m_Count[SIX] = 0;
                                    }
                                    else
                                        nMult = ZERO;
                                }
                                else if (m_Count[SIX] >= 3)
                                {
                                    iTempScore += (m_Count[SIX] - 2) * 600;
                                    Console.WriteLine("Roll {0} - {1} Sixes - Total {2}", nRoll + 1, m_Count[SIX], iTempScore);
                                    nDice -= m_Count[SIX];
                                    m_Count[SIX] = 0;
                                    nMult = SIX;
                                }
                                else if (nMult == FIVE)
                                {
                                    if (m_Count[FIVE] > 0)
                                    {
                                        iTempScore += m_Count[FIVE] * 500;
                                        Console.WriteLine("Roll {0} - {1} Five(s) - Total {2}", nRoll + 1, m_Count[FIVE], iTempScore);
                                        nDice -= m_Count[FIVE];
                                        m_Count[FIVE] = 0;
                                    }
                                    else
                                        nMult = ZERO;
                                }
                                else if (m_Count[FIVE] >= 3)
                                {
                                    iTempScore += (m_Count[FIVE] - 2) * 500;
                                    Console.WriteLine("Roll {0} - {1} Five(s) - Total {2}", nRoll + 1, m_Count[FIVE], iTempScore);
                                    nDice -= m_Count[FIVE];
                                    m_Count[FIVE] = 0;
                                    nMult = FIVE;
                                }
                                else if (nMult == FOUR)
                                {
                                    if (m_Count[FOUR] > 0)
                                    {
                                        iTempScore += m_Count[FOUR] * 400;
                                        Console.WriteLine("Roll {0} - {1} Four(s) - Total {2}", nRoll + 1, m_Count[FOUR], iTempScore);
                                        nDice -= m_Count[FOUR];
                                        m_Count[FOUR] = 0;
                                    }
                                    else
                                        nMult = ZERO;
                                }
                                else if (m_Count[FOUR] >= 3)
                                {
                                    iTempScore += (m_Count[FOUR] - 2) * 400;
                                    Console.WriteLine("Roll {0} - {1} Four(s) - Total {2}", nRoll + 1, m_Count[FOUR], iTempScore);
                                    nDice -= m_Count[FOUR];
                                    m_Count[FOUR] = 0;
                                    nMult = FOUR;
                                }
                                else if (nMult == THREE)
                                {
                                    if (m_Count[THREE] > 0)
                                    {
                                        iTempScore += m_Count[THREE] * 300;
                                        Console.WriteLine("Roll {0} - {1} Three(s) - Total {2}", nRoll + 1, m_Count[THREE], iTempScore);
                                        nDice -= m_Count[THREE];
                                        m_Count[THREE] = 0;
                                    }
                                    else
                                        nMult = ZERO;
                                }
                                else if (m_Count[THREE] >= 3)
                                {
                                    iTempScore += (m_Count[THREE] - 2) * 300;
                                    Console.WriteLine("Roll {0} - {1} Three(s) - Total {2}", nRoll + 1, m_Count[THREE], iTempScore);
                                    nDice -= m_Count[THREE];
                                    m_Count[THREE] = 0;
                                    nMult = THREE;
                                }
                                else if (nMult == TWO)
                                {
                                    if (m_Count[TWO] > 0)
                                    {
                                        iTempScore += m_Count[TWO] * 200;
                                        Console.WriteLine("Roll {0} - {1} Two(s) - Total {2}", nRoll + 1, m_Count[TWO], iTempScore);
                                        nDice -= m_Count[TWO];
                                        m_Count[TWO] = 0;
                                    }
                                    else
                                        nMult = ZERO;
                                }
                                else if (m_Count[TWO] >= 3)
                                {
                                    iTempScore += (m_Count[TWO] - 2) * 200;
                                    Console.WriteLine("Roll {0} - {1} Two(s) - Total {2}", nRoll + 1, m_Count[TWO], iTempScore);
                                    nDice -= m_Count[TWO];
                                    m_Count[TWO] = 0;
                                    nMult = TWO;
                                }

                                if (m_Count[ONE] > 0 && m_Count[ONE] < 3)
                                {
                                    iTempScore += m_Count[ONE] * 100;
                                    Console.WriteLine("Roll {0} - 100 - Score {1} - Total {2}", nRoll + 1, m_Count[0] * 100, iTempScore);
                                    nDice -= m_Count[ONE];
                                    m_Count[ONE] = 0;
                                    nMult = ZERO;
                                }
                                
                                if (m_Count[FIVE] > 0 && m_Count[FIVE] < 3)
                                {
                                    iTempScore += m_Count[FIVE] * 50;
                                    Console.WriteLine("Roll {0} - 50 - Score {1} - Total {2}", nRoll + 1, m_Count[0] * 50, iTempScore);
                                    nDice -= m_Count[FIVE];
                                    m_Count[FIVE] = 0;
                                    nMult = ZERO;
                                }
                            }
                        }

                        if (iTempScore == 0)
                            Console.WriteLine("Please pick the scoring dice");
                        else
                        {
                            if (nDice == 0)
                            {
                                nDice = 6;
                                nMult = ZERO;
                            }
                        }
                    } while (iTempScore == 0);

                    iRollScore += iTempScore;
                    Console.WriteLine("\r\nTotal Roll Score {0}\r\n", iRollScore);

                    if (nMult > ZERO)
                        Console.WriteLine("{0} is the roll multiplier\r\n", nMult + 1);
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

#if DEBUG
                bool bWriteCount = true;
                if (bWriteCount)
                {
                    Console.Write("Count {0}: ", nRoll + 1);
                    for (int iDie = 0; iDie < 6; iDie++)
                        Console.Write("{0} ", m_Count[iDie]);
                    Console.WriteLine();
                }
                Console.WriteLine();
#endif

                bool bIsLargeStraight = IsLargeStraight(nDice);
                bool bIs3Pair = Is3Pair(nDice);

                // Look for patterns
                if (bIsLargeStraight)
                {
                    // Large straight : 1500 points
                    iTempScore += 1500;
                    Console.WriteLine("Roll {0} - Large Straight - Total {1}", nRoll + 1, iTempScore);
                    for (int iDie = ONE; iDie <= SIX; iDie++)
                        m_Count[iDie] = 0;
                    nDice = 0;
                    nMult = ZERO;
                }
                else
                {
                    if (bIs3Pair)
                    {
                        // 3 Pair : 500 points
                        iTempScore += 500;
                        Console.WriteLine("Roll {0} - Three Pair - Total {1}", nRoll + 1, iTempScore);
                        for (int iDie = ONE; iDie <= SIX; iDie++)
                            m_Count[iDie] = 0;
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
                                Console.WriteLine("Roll {0} - {1} Ones - Total {2}", nRoll + 1, m_Count[ONE], iTempScore);
                                nDice -= m_Count[ONE];
                                m_Count[ONE] = 0;
                            }
                            else
                                nMult = ZERO;
                        }
                        else if (m_Count[ONE] >= 3)
                        {
                            // Eintausend - 1000
                            iTempScore += (m_Count[ONE] - 2) * 1000;
                            Console.WriteLine("Roll {0} - {1} Ones - Total {2}", nRoll + 1, m_Count[ONE], iTempScore);
                            nDice -= m_Count[ONE];
                            m_Count[ONE] = 0;
                            nMult = ONE;
                        }
                        else if (nMult == SIX)
                        {
                            if (m_Count[SIX] > 0)
                            {
                                iTempScore += m_Count[SIX] * 600;
                                Console.WriteLine("Roll {0} - {1} Sixes - Total {2}", nRoll + 1, m_Count[SIX], iTempScore);
                                nDice -= m_Count[SIX];
                                m_Count[SIX] = 0;
                            }
                            else
                                nMult = ZERO;
                        }
                        else if (m_Count[SIX] >= 3)
                        {
                            iTempScore += (m_Count[SIX] - 2) * 600;
                            Console.WriteLine("Roll {0} - {1} Six(es) - Total {2}", nRoll + 1, m_Count[SIX], iTempScore);
                            nDice -= m_Count[SIX];
                            m_Count[SIX] = 0;
                            nMult = SIX;
                        }
                        else if (nMult == FIVE)
                        {
                            if (m_Count[FIVE] > 0)
                            {
                                iTempScore += m_Count[FIVE] * 500;
                                Console.WriteLine("Roll {0} - {1} Five(s) - Total {2}", nRoll + 1, m_Count[FIVE], iTempScore);
                                nDice -= m_Count[FIVE];
                                m_Count[FIVE] = 0;
                            }
                            else
                                nMult = ZERO;
                        }
                        else if (m_Count[FIVE] >= 3)
                        {
                            iTempScore += (m_Count[FIVE] - 2) * 500;
                            Console.WriteLine("Roll {0} - {1} Five(s) - Total {2}", nRoll + 1, m_Count[FIVE], iTempScore);
                            nDice -= m_Count[FIVE];
                            m_Count[FIVE] = 0;
                            nMult = FIVE;
                        }
                        else if (nMult == FOUR)
                        {
                            if (m_Count[FOUR] > 0)
                            {
                                iTempScore += m_Count[FOUR] * 400;
                                Console.WriteLine("Roll {0} - {1} Four(s) - Total {2}", nRoll + 1, m_Count[FOUR], iTempScore);
                                nDice -= m_Count[FOUR];
                                m_Count[FOUR] = 0;
                            }
                            else
                                nMult = ZERO;
                        }
                        else if (m_Count[FOUR] >= 3)
                        {
                            iTempScore += (m_Count[FOUR] - 2) * 400;
                            Console.WriteLine("Roll {0} - {1} Four(s) - Total {2}", nRoll + 1, m_Count[FOUR], iTempScore);
                            nDice -= m_Count[FOUR];
                            m_Count[FOUR] = 0;
                            nMult = FOUR;
                        }
                        else if (nMult == THREE)
                        {
                            if (m_Count[THREE] > 0)
                            {
                                iTempScore += m_Count[THREE] * 300;
                                Console.WriteLine("Roll {0} - {1} Three(s) - Total {2}", nRoll + 1, m_Count[THREE], iTempScore);
                                nDice -= m_Count[THREE];
                                m_Count[THREE] = 0;
                            }
                            else
                                nMult = ZERO;
                        }
                        else if (m_Count[THREE] >= 3)
                        {
                            iTempScore += (m_Count[THREE] - 2) * 300;
                            Console.WriteLine("Roll {0} - {1} Three(s) - Total {2}", nRoll + 1, m_Count[THREE], iTempScore);
                            nDice -= m_Count[THREE];
                            m_Count[THREE] = 0;
                            nMult = THREE;
                        }
                        else if (nMult == TWO)
                        {
                            if (m_Count[TWO] > 0)
                            {
                                iTempScore += m_Count[TWO] * 200;
                                Console.WriteLine("Roll {0} - {1} Two(s) - Total {2}", nRoll + 1, m_Count[TWO], iTempScore);
                                nDice -= m_Count[TWO];
                                m_Count[TWO] = 0;
                            }
                            else
                                nMult = ZERO;
                        }
                        else if (m_Count[TWO] >= 3)
                        {
                            iTempScore += (m_Count[TWO] - 2) * 200;
                            Console.WriteLine("Roll {0} - {1} Two(s) - Total {2}", nRoll + 1, m_Count[TWO], iTempScore);
                            nDice -= m_Count[TWO];
                            m_Count[TWO] = 0;
                            nMult = TWO;
                        }

                        if (m_Count[ONE] > 0 && m_Count[ONE] < 3)
                        {
                            iTempScore += m_Count[ONE] * 100;
                            Console.WriteLine("Roll {0} - {1} Ones(s) - Total {2}", nRoll + 1, m_Count[ONE], iTempScore);
                            nDice -= m_Count[ONE];
                            m_Count[ONE] = 0;
                            nMult = ZERO;
                        }

                        if (m_Count[FIVE] > 0 && m_Count[FIVE] < 3)
                        {
                            iTempScore += m_Count[FIVE] * 50;
                            Console.WriteLine("Roll {0} - {1} Five(s) - Total {2}", nRoll + 1, m_Count[FIVE], iTempScore);
                            nDice -= m_Count[FIVE];
                            m_Count[FIVE] = 0;
                            nMult = ZERO;
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
                Thread.Sleep(1000);
            }
            if (!bBuela)
                iGameScoreCPU += iRollScore;
            Console.WriteLine(string.Format("CPU {0} - Score {1}\r\n", iCPUID, iGameScoreCPU));
            return iGameScoreCPU;
        }

        static void Scoring()
        {
            Console.WriteLine("Lesters Teeth");
            Console.WriteLine();
            Console.WriteLine("Scoring:");
            Console.WriteLine("Straight - 1500");
            Console.WriteLine("3 Ones   - 1000 - Additional 1000 each");
            Console.WriteLine("3 Sixes  - 600  - Additional 600 each");
            Console.WriteLine("3 Fives  - 500  - Additional 500 each");
            Console.WriteLine("3 Fours  - 400  - Additional 400 each");
            Console.WriteLine("3 Threes - 300  - Additional 300 each");
            Console.WriteLine("3 Twos   - 200  - Additional 200 each");
            Console.WriteLine("3 Pair   - 500");
            Console.WriteLine("One      - 100");
            Console.WriteLine("Five     - 50");
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            SeedRng(ref args);

            bool bAgain = true;

            while (bAgain)
            {
                Scoring();

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

                Console.WriteLine("\r\nStarting order is Player(s) first, CPU(s) second. Good Luck!\r\n");

                int[] PlayerScore = new int[nPlayers];

                bool bGameLoop = true;
                while (bGameLoop)
                {
                    Console.WriteLine("Lesters Teeth\r\n");
                    for (int iPlayer = 0; iPlayer < nPlayers; iPlayer++)
                    {
                        if (iPlayer < nPeople)
                            PlayerScore[iPlayer] = RollPlayer(PlayerScore[iPlayer], iPlayer + 1);
                        else
                            PlayerScore[iPlayer] = RollCPU(PlayerScore[iPlayer], iPlayer + 1);

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

                Console.WriteLine();
                Console.WriteLine("Press A to play again.\r\n");
                ConsoleKeyInfo cki = Console.ReadKey(true);
                string strKey = cki.KeyChar.ToString();
                if (strKey != "A" && strKey != "a")
                    bAgain = false;
            }
        }
    }
}
