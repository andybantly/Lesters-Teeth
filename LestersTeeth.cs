using System.ComponentModel;

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

                        List<int> Dice = new List<int>();
                        if (m_Count[ONE] == 2)
                            Dice.Add(m_Count[ONE]);
                        if (m_Count[TWO] == 2)
                            Dice.Add(m_Count[TWO]);
                        if (m_Count[THREE] == 2)
                            Dice.Add(m_Count[THREE]);
                        if (m_Count[FOUR] == 2)
                            Dice.Add(m_Count[FOUR]);
                        if (m_Count[FIVE] == 2)
                            Dice.Add(m_Count[FIVE]);
                        if (m_Count[SIX] == 2)
                            Dice.Add(m_Count[SIX]);
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

                    if (iGameScoreCPU < 500)
                    {
                        if (iGameScoreCPU + iRollScore >= 500)
                        {
                            int iChance = m_Rnd.Next(1, 101);
                            if (nDice > 0)
                            {
                                if (nMult > ZERO)
                                {
                                    // Could have 3, 2, or 1 dice left.  Higher multiplier makes higher score. Determine chance of going for it!
                                    if (nMult == ONE)
                                    {

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
                            Console.WriteLine("Fresh Teeth");
                            nDice = 6;
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
            bool bGameLoop = true;
            SeedRng(ref args);

            int iGameScoreCPU1 = 0;
            int iGameScoreCPU2 = 0;
            while (bGameLoop)
            {
                Console.WriteLine("Lesters Teeth\r\n");
                iGameScoreCPU1 = RollCPU(iGameScoreCPU1, 1);
                iGameScoreCPU2 = RollCPU(iGameScoreCPU2, 2);

                Console.WriteLine("CPU {0}: {1} to CPU {2}: {3}\r\n", 1, iGameScoreCPU1, 2, iGameScoreCPU2);

                if (iGameScoreCPU1 >= 10000 || iGameScoreCPU2 >= 10000)
                {
                    if (iGameScoreCPU1 > iGameScoreCPU2)
                        Console.WriteLine(string.Format("CPU 1 Wins! The score is {0} to {1}", iGameScoreCPU1, iGameScoreCPU2));
                    else if (iGameScoreCPU2 > iGameScoreCPU1)
                        Console.WriteLine(string.Format("CPU 2 Wins! The score is {0} to {1}", iGameScoreCPU2, iGameScoreCPU1));
                    else
                        Console.WriteLine(string.Format("Tie! The score is {0} to {1}", iGameScoreCPU1, iGameScoreCPU2));
                    bGameLoop = false;
                }
            }
        }
    }
}
