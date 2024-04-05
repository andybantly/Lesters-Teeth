namespace Lesters_Teeth
{
    internal class Program
    {
        const int ZERO = -1;
        const int ONE = 0;
        const int TWO = 1;
        const int THREE = 2;
        const int FOUR = 3;
        const int FIVE = 4;
        const int SIX = 5;
        static void Main(string[] args)
        {
            int iGameScore = 0;

            Random Rnd = new Random();
            bool bRollLoop = true;
            int nDice = 6;
            int nRoll = 0;
            int iRollScore = 0;
            bool bBuela = false;
            while (bRollLoop)
            {
                Console.WriteLine("Lesters Teeth");
                Console.Write(" Roll {0}: ", nRoll + 1);
                int nMult = ZERO;
                int iTempScore = 0;
                int[] Dice = new int[6] { 0, 0, 0, 0, 0, 0 };
                int[] Count = new int[6] { 0, 0, 0, 0, 0, 0 };
                for (int iDie = 0; iDie < nDice; iDie++)
                {
                    Dice[iDie] = Rnd.Next(1, 7);
                    Count[Dice[iDie] - 1]++;
                    Console.Write("{0} ", Dice[iDie]);
                }
                Console.WriteLine();
                Console.Write("Count {0}: ", nRoll + 1);
                for (int iDie = 0; iDie < 6; iDie++)
                    Console.Write("{0} ", Count[iDie]);
                Console.WriteLine();

                // Look for patterns
                if (Count[ONE] == 1 &&
                    Count[TWO] == 1 &&
                    Count[THREE] == 1 &&
                    Count[FOUR] == 1 &&
                    Count[FIVE] == 1 &&
                    Count[SIX] == 1)
                {
                    // Large straight : 1500 points
                    iTempScore += 1500;
                    Console.WriteLine("Large Straight");
                    nDice = 0;
                    nMult = ZERO;

                    // Needs to be logic to control going on next
                    // What if I am under 500?
                    // What if I am over 10000?
                    // Accepting this as scoring ups the risk rolling forward
                }
                else
                {
                    // Write out all patterns that score from most to least (no else-ifs)
                    int n3Pair = 0;
                    if (Count[ONE] == 2)
                        n3Pair++;
                    if (Count[TWO] == 2)
                        n3Pair++;
                    if (Count[THREE] == 2)
                        n3Pair++;
                    if (Count[FOUR] == 2)
                        n3Pair++;
                    if (Count[FIVE] == 2)
                        n3Pair++;
                    if (Count[SIX] == 2)
                        n3Pair++;
                    if (n3Pair == 3)
                    {
                        // 3 Pair : 500 points
                        iTempScore += 500;
                        Console.WriteLine("Three Pair");
                        nDice = 0;
                        nMult = ZERO;

                        // Needs to be logic to control going on next
                        // What if I am under 500?
                        // What if I am over 10000?
                        // Accepting this as scoring ups the risk rolling forward
                    }
                    else
                    {
                        if (nMult == ONE)
                        {
                            if (Count[ONE] > 0)
                            {
                                iTempScore += Count[ONE] * 1000;
                                Console.WriteLine("{0} more Ones", Count[ONE]);
                                nDice -= Count[ONE];
                            }
                            else
                                nMult = ZERO;
                        }
                        else if (Count[ONE] >= 3)
                        {
                            // Eintausend - 1000
                            iTempScore += (Count[ONE] - 2) * 1000;
                            Console.WriteLine("{0} Ones", Count[ONE]);
                            nDice -= Count[ONE];
                            nMult = ONE;
                        }
                        else if (nMult == SIX)
                        {
                            if (Count[SIX] > 0)
                            {
                                iTempScore += Count[SIX] * 600;
                                Console.WriteLine("{0} more Sixes", Count[SIX]);
                                nDice -= Count[SIX];
                            }
                            else
                                nMult = ZERO;
                        }
                        else if (Count[SIX] >= 3)
                        {
                            iTempScore += (Count[SIX] - 2) * 600;
                            Console.WriteLine("{0} Sixes", Count[SIX]);
                            nDice -= Count[SIX];
                            nMult = SIX;
                        }
                        else if (nMult == FIVE)
                        {
                            if (Count[FIVE] > 0)
                            {
                                iTempScore += Count[FIVE] * 500;
                                Console.WriteLine("{0} more Fives", Count[FIVE]);
                                nDice -= Count[FIVE];
                            }
                            else
                                nMult = ZERO;
                        }
                        else if (Count[FIVE] >= 3)
                        {
                            iTempScore += (Count[FIVE] - 2) * 500;
                            Console.WriteLine("{0} Fives", Count[FIVE]);
                            nDice -= Count[FIVE];
                            nMult = FIVE;
                        }
                        else if (nMult == FOUR)
                        {
                            if (Count[FOUR] > 0)
                            {
                                iTempScore += Count[FOUR] * 400;
                                Console.WriteLine("{0} more Fours", Count[FOUR]);
                                nDice -= Count[FOUR];
                            }
                            else
                                nMult = ZERO;
                        }
                        else if (Count[FOUR] >= 3)
                        {
                            iTempScore += (Count[FOUR] - 2) * 400;
                            Console.WriteLine("{0} Fours", Count[FOUR]);
                            nDice -= Count[FOUR];
                            nMult = FOUR;
                        }
                        else if (nMult == THREE)
                        {
                            if (Count[THREE] > 0)
                            {
                                iTempScore += Count[THREE] * 300;
                                Console.WriteLine("{0} more Threes", Count[THREE]);
                                nDice -= Count[THREE];
                            }
                            else
                                nMult = ZERO;
                        }
                        else if (Count[THREE] >= 3)
                        {
                            iTempScore += (Count[THREE] - 2) * 300;
                            Console.WriteLine("{0} Threes", Count[THREE]);
                            nDice -= Count[THREE];
                            nMult = THREE;
                        }
                        else if (nMult == TWO)
                        {
                            if (Count[TWO] > 0)
                            {
                                iTempScore += Count[TWO] * 200;
                                Console.WriteLine("{0} more Twos", Count[TWO]);
                                nDice -= Count[TWO];
                            }
                            else
                                nMult = ZERO;
                        }
                        else if (Count[TWO] >= 3)
                        {
                            iTempScore += (Count[TWO] - 2) * 200;
                            Console.WriteLine("{0} Twos", Count[TWO]);
                            nDice -= Count[TWO];
                            nMult = TWO;
                        }

                        if (nMult == ZERO)
                        {
                            // Eventually have logic to choose 1's or 5's
                            if (Count[ONE] > 0 && Count[ONE] < 3) // 1s
                            {
                                iTempScore += Count[ONE] * 100;
                                Console.WriteLine("Roll {0} - 100 - Score {1} - Total {2}", nRoll + 1, Count[0] * 100, iTempScore);
                                nDice -= Count[ONE];
                            }
                            else if (Count[FIVE] > 0 && Count[FIVE] < 3) // 5s
                            {
                                iTempScore += Count[FIVE] * 50;
                                Console.WriteLine("Roll {0} - 50 - Score {1} - Total {2}", nRoll + 1, Count[0] * 50, iTempScore);
                                nDice -= Count[FIVE];
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

                    // Needs to be logic to control going on next
                    // What if I am under 500?
                    // What if I am over 10000?
                    // Accepting this as scoring ups the risk rolling forward

                    if (iGameScore < 500)
                    {
                        if (iGameScore + iRollScore >= 500)
                        {
                            Console.WriteLine("On the board! It's Your turn");
                            bRollLoop = false;
                        }
                    }
                    else
                    {
                        if (nMult > ZERO)
                            Console.WriteLine("{0} is the roll multiplier", nMult);

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
                iGameScore += iRollScore;
            Console.WriteLine(string.Format("Game Score {0}", iGameScore));
        }
    }
}
