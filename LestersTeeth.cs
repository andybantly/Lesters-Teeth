﻿/*
Andrew Scott Bantly's Software License Agreement
Version 1.0

Copyright(C) 2024 Andrew Scott Bantly

1. Permission and Restrictions:
   - This software is proprietary and is provided "as is."
   - No individual or organization may copy, modify, distribute, or use this software or its derivatives without prior written consent from Andrew Scott Bantly.
   - Usage in commercial, educational, or personal projects is strictly prohibited without explicit permission from Andrew Scott Bantly

2. Limited Access :
   - Access to the source code is granted for reference purposes only.
   - Redistribution of the source code or its compiled binaries, whether modified or unmodified, is expressly forbidden.

3. Liability Disclaimer :
   - This software is provided without warranty of any kind, either express or implied, including but not limited to the warranties of merchantability, fitness for a particular purpose, or noninfringement.
   - Under no circumstances shall Andrew Scott Bantly be liable for any damages, including incidental, special, or consequential damages arising out of or in connection with the use of this software.

4. Violation of Terms :
   - Any unauthorized use, modification, or distribution of this software will result in immediate revocation of any implied permissions and may lead to legal action.

5. Jurisdiction :
   - This license shall be governed by the laws of The United States of America.
*/

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

        const int WAIT = 2000;

        static int[]? m_Dice = null;
        static int[]? m_Count = null;

        static Random SeedRng(ref string[] args)
        {
            int iSeed = 0;
            bool bSeed = false;
            if (args.Length > 0)
                bSeed = Int32.TryParse(args[0], out iSeed);
            Random Rnd = bSeed ? new Random(iSeed) : new Random();
            return Rnd;
        }

        static bool IsLargeStraight(int nDice)
        {
            bool bIsLargeStraight = false;
            if (m_Count != null && nDice == 6)
            {
                int nLS = 0;
                for (int iDie = ONE; iDie <= SIX; ++iDie)
                    nLS += (m_Count[iDie] == 1 ? 1 : 0);
                bIsLargeStraight = nLS == 6;
            }
            return bIsLargeStraight;
        }

        static bool Is3Pair(int nDice)
        {
            bool bIs3Pair = false;
            if (m_Count != null && nDice == 6)
            {
                int n3Pair = 0;
                for (int iDie = ONE; n3Pair < 3 && iDie <= SIX; ++iDie)
                    n3Pair += (m_Count[iDie] == 2 ? 1 : 0);
                bIs3Pair = n3Pair == 3;
            }
            return bIs3Pair;
        }

        static bool Is23Kind(int nDice)
        {
            bool bIs23Kind = false;
            if (m_Count != null && nDice == 6)
            {
                int n23Kind = 0;
                for (int iDie = ONE; n23Kind < 2 && iDie <= SIX; ++iDie)
                    n23Kind += (m_Count[iDie] == 3 ? 1 : 0);
                bIs23Kind = n23Kind == 2;
            }
            return bIs23Kind;
        }

        static bool IsRunClean(int nDice, int nMult)
        {
            bool bIsRunClean = false;
            if (m_Count != null)
            {
                int nRem = nDice - (nMult != ZERO ? m_Count[nMult] : 0);
                if (nMult != ONE && m_Count[ONE] > 0)
                    nRem -= m_Count[ONE];
                if (nMult != FIVE && m_Count[FIVE] > 0)
                    nRem -= m_Count[FIVE];
                if (nRem == 0)
                    bIsRunClean = true;
            }
            return bIsRunClean;
        }

        // ASB - 10/17/24 - This modifies and needs to be renamed to reflect that
        static int CheckStranded(int nRoll, ref int nDice, ref int nMult)
        {
            int iTempScore = 0;
            if (m_Count != null)
            {
                if (nMult != ZERO)
                {
                    if (m_Count[nMult] > 0)
                    {
                        string[]? Die = ["One(s)", "Two{s}", "Three(s)", "Four(s)", "Five(s)", "Six(es)"];
                        int[]? Score = [1000, 200, 300, 400, 500, 600];

                        iTempScore += m_Count[nMult] * Score[nMult];
                        Console.WriteLine("Roll {0} - {1} {2} - Total {3}", nRoll + 1, m_Count[ONE], Die[nMult], iTempScore);
                        nDice -= m_Count[nMult];
                        m_Count[nMult] = 0;
                    }
                }

                if (nMult != ONE && m_Count[ONE] > 0)
                {
                    iTempScore += m_Count[ONE] * 100;
                    Console.WriteLine("Roll {0} - {1} Ones(s) - Total {2}", nRoll + 1, m_Count[ONE], iTempScore);
                    nDice -= m_Count[ONE];
                    m_Count[ONE] = 0;
                }

                if (nMult != FIVE && m_Count[FIVE] > 0)
                {
                    iTempScore += m_Count[FIVE] * 50;
                    Console.WriteLine("Roll {0} - {1} Fives(s) - Total {2}", nRoll + 1, m_Count[FIVE], iTempScore);
                    nDice -= m_Count[FIVE];
                    m_Count[FIVE] = 0;
                }
            }

            return iTempScore;
        }

        static bool IsBuela(int nDice, int nMult)
        {
            bool bBuela = true;
            if (m_Count != null)
            {
                if (IsLargeStraight(nDice) || Is3Pair(nDice) || Is23Kind(nDice) || IsRunClean(nDice, nMult))
                    bBuela = false;
                if (nMult != ZERO && m_Count[nMult] > 0)
                    bBuela = false;
                int[]? Limit = [1, 3, 3, 3, 1, 3];
                for (int iDie = ONE; bBuela && iDie <= SIX; ++iDie)
                {
                    if (m_Count[iDie] >= Limit[iDie])
                        bBuela = false;
                }
            }
            return bBuela;
        }

        static bool GoForIt(ref Random Rnd, int nPlayers, int[] PlayerScore, int iGameScoreCPU, int iRollScore)
        {
            bool bResult = false;
            int nMax = 0;
            for (int iPlayer = 0; iPlayer < nPlayers; iPlayer++)
            {
                if (PlayerScore[iPlayer] > nMax)
                    nMax = PlayerScore[iPlayer];
            }
            if ((iGameScoreCPU + iRollScore) < nMax)
            {
                int nDiff = nMax - (iGameScoreCPU + iRollScore);
                if (nDiff > 2500)
                {
                    if (Rnd.Next(1, 101) > 75)
                        bResult = true;
                }
                else if (nDiff > 1000)
                {
                    if (Rnd.Next(1, 101) > 90)
                        bResult = true;
                }
            }
            return bResult;
        }

        static int RollPlayer(ref Random Rnd, int iGameScorePlayer, int iPlayerID)
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
                    if (iRollScore >= 500 || (iGameScorePlayer >= 500 && iRollScore > 0))
                        Console.WriteLine("Press R to roll, S to score");
                    else
                        Console.WriteLine("Press R to roll");
                    Console.WriteLine();
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
                m_Dice = [ 0, 0, 0, 0, 0, 0 ];
                m_Count = [ 0, 0, 0, 0, 0, 0 ];
                for (int iDie = 0; iDie < nDice; iDie++)
                    m_Dice[iDie] = Rnd.Next(1, 7);
                //m_Dice = [1, 1, 1, 2, 2, 2]; // For debugging specific rolls
                for (int iDie = 0; iDie < nDice; iDie++)
                {
                    m_Count[m_Dice[iDie] - 1]++;
                    Console.Write("{0} ", m_Dice[iDie]);
                }
                Console.WriteLine();

#if DEBUG
                bool bWriteCount = false;
                if (bWriteCount)
                {
                    Console.Write("Count {0}: ", nRoll + 1);
                    for (int iDie = 0; iDie < 6; iDie++)
                        Console.Write("{0} ", m_Count[iDie]);
                    Console.WriteLine();
                }
                Console.WriteLine();
#endif

                bBuela = IsBuela(nDice, nMult);
                if (bBuela)
                {
                    bRollLoop = false;
                    Console.WriteLine("Buela!\r\n");
                    bBuela = true;
                    Thread.Sleep(WAIT);
                }
                else
                {
                    do
                    {
                        bool bChoose = true;
                        int[] CountBackup = [m_Count[ONE], m_Count[TWO], m_Count[THREE], m_Count[FOUR], m_Count[FIVE], m_Count[SIX]];
                        do
                        {
                            m_Count = [CountBackup[ONE], CountBackup[TWO], CountBackup[THREE], CountBackup[FOUR], CountBackup[FIVE], CountBackup[SIX]];
                            int[] Count = [m_Count[ONE], m_Count[TWO], m_Count[THREE], m_Count[FOUR], m_Count[FIVE], m_Count[SIX]];
                            int[] Count2 = [0, 0, 0, 0, 0, 0];

                            Console.Write("Pick up score dice: ");
                            string? strDice = Console.ReadLine();
                            if (!string.IsNullOrEmpty(strDice))
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
                                    for (int iDie = ONE; iDie <= SIX; iDie++)
                                        m_Count[iDie] = Count2[iDie];
                                    if (!IsBuela(6, nMult))
                                        bChoose = false;
                                }
                            }
                        } while (bChoose);

                        // Look for patterns
                        bool bIsLargeStraight = IsLargeStraight(nDice);
                        bool bIs3Pair = Is3Pair(nDice);
                        bool bIs23Kind = Is23Kind(nDice);
                        if (bIsLargeStraight)
                        {
                            // Large straight : 1500 points
                            iTempScore += 1500;
                            Console.WriteLine("Roll {0} - Large Straight - Total {1}", nRoll + 1, iTempScore);
                            m_Count = [0, 0, 0, 0, 0, 0];
                            nDice = 0;
                            nMult = ZERO;
                        }
                        else if (bIs3Pair)
                        {
                            // 3 Pair : 500 points
                            iTempScore += 500;
                            Console.WriteLine("Roll {0} - Three Pair - Total {1}", nRoll + 1, iTempScore);
                            m_Count = [0, 0, 0, 0, 0, 0];
                            nDice = 0;
                            nMult = ZERO;
                        }
                        else if (bIs23Kind)
                        {
                            if (m_Count[ONE] == 3)
                            {
                                iTempScore += 1000;
                                Console.WriteLine("Roll {0} - {1} Ones - Total {2}", nRoll + 1, m_Count[ONE], iTempScore);
                                nDice -= m_Count[ONE];
                                m_Count[ONE] = 0;
                            }
                            if (m_Count[SIX] == 3)
                            {
                                iTempScore += 600;
                                Console.WriteLine("Roll {0} - {1} Sixes - Total {2}", nRoll + 1, m_Count[SIX], iTempScore);
                                nDice -= m_Count[SIX];
                                m_Count[SIX] = 0;
                            }
                            if (m_Count[FIVE] == 3)
                            {
                                iTempScore += 500;
                                Console.WriteLine("Roll {0} - {1} Five(s) - Total {2}", nRoll + 1, m_Count[FIVE], iTempScore);
                                nDice -= m_Count[FIVE];
                                m_Count[FIVE] = 0;
                            }
                            if (m_Count[FOUR] == 3)
                            {
                                iTempScore += 400;
                                Console.WriteLine("Roll {0} - {1} Four(s) - Total {2}", nRoll + 1, m_Count[FOUR], iTempScore);
                                nDice -= m_Count[FOUR];
                                m_Count[FOUR] = 0;
                            }
                            if (m_Count[THREE] == 3)
                            {
                                iTempScore += 300;
                                Console.WriteLine("Roll {0} - {1} Three(s) - Total {2}", nRoll + 1, m_Count[THREE], iTempScore);
                                nDice -= m_Count[THREE];
                                m_Count[THREE] = 0;
                            }
                            if (m_Count[TWO] == 3)
                            {
                                iTempScore += 200;
                                Console.WriteLine("Roll {0} - {1} Two(s) - Total {2}", nRoll + 1, m_Count[TWO], iTempScore);
                                nDice -= m_Count[TWO];
                                m_Count[TWO] = 0;
                            }
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
                                Console.WriteLine("Roll {0} - 100 - Score {1} - Total {2}", nRoll + 1, m_Count[ONE] * 100, iTempScore);
                                nDice -= m_Count[ONE];
                                m_Count[ONE] = 0;
                                nMult = ZERO;
                            }
                                
                            if (m_Count[FIVE] > 0 && m_Count[FIVE] < 3)
                            {
                                iTempScore += m_Count[FIVE] * 50;
                                Console.WriteLine("Roll {0} - 50 - Score {1} - Total {2}", nRoll + 1, m_Count[FIVE] * 50, iTempScore);
                                nDice -= m_Count[FIVE];
                                m_Count[FIVE] = 0;
                                nMult = ZERO;
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
            Console.WriteLine(string.Format("Player {0} - Round Score {1}\r\n", iPlayerID, iGameScorePlayer));
            return iGameScorePlayer;
        }

        static int RollCPU(ref Random Rnd, int nPlayers, int[] PlayerScore, int iGameScoreCPU, int iCPUID)
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
                m_Dice = [0, 0, 0, 0, 0, 0];
                m_Count = [0, 0, 0, 0, 0, 0];
                for ( int iDie = 0; iDie < nDice; iDie++)
                    m_Dice[iDie] = Rnd.Next(1, 7);
                for (int iDie = 0; iDie < nDice; iDie++)
                {
                    m_Count[m_Dice[iDie] - 1]++;
                    Console.Write("{0} ", m_Dice[iDie]);
                }
                Console.WriteLine();

#if DEBUG
                bool bWriteCount = false;
                if (bWriteCount)
                {
                    Console.Write("Count {0}: ", nRoll + 1);
                    for (int iDie = 0; iDie < 6; iDie++)
                        Console.Write("{0} ", m_Count[iDie]);
                    Console.WriteLine();
                }
                Console.WriteLine();
#endif
                Thread.Sleep(WAIT);

                bool bIsLargeStraight = IsLargeStraight(nDice);
                bool bIs3Pair = Is3Pair(nDice);
                bool bIs23Kind = Is23Kind(nDice);
                bool bIsRunClean = IsRunClean(nDice, nMult);

                // Look for patterns
                if (bIsLargeStraight)
                {
                    // Large straight : 1500 points
                    iTempScore += 1500;
                    Console.WriteLine("Roll {0} - Large Straight - Total {1}", nRoll + 1, iTempScore);
                    m_Count = [0, 0, 0, 0, 0, 0];
                    nDice = 0;
                    nMult = ZERO;

                    if (iGameScoreCPU < 500)
                    {
                        Console.WriteLine("Counting my blessings");
                        if (!GoForIt(ref Rnd, nPlayers, PlayerScore, iGameScoreCPU, iRollScore))
                            bRollLoop = false;
                        Console.WriteLine(bRollLoop ? "And going for it" : " And passing the teeth");
                    }

                    if ((iRollScore > 1000) && Rnd.Next(1, 101) > 80)
                    {
                        Console.WriteLine("Not going to chance it");
                        if (!GoForIt(ref Rnd, nPlayers, PlayerScore, iGameScoreCPU, iRollScore))
                            bRollLoop = false;
                        Console.WriteLine(bRollLoop ? "And going for it" : " And passing the teeth");
                    }
                }
                else if (bIs3Pair)
                {
                    // 3 Pair : 500 points
                    iTempScore += 500;
                    Console.WriteLine("Roll {0} - Three Pair - Total {1}", nRoll + 1, iTempScore);
                    m_Count = [0, 0, 0, 0, 0, 0];
                    nDice = 0;
                    nMult = ZERO;
                }
                else if (bIs23Kind)
                {
                    if (m_Count[ONE] == 3)
                    {
                        iTempScore += 1000;
                        Console.WriteLine("Roll {0} - {1} Ones - Total {2}", nRoll + 1, m_Count[ONE], iTempScore);
                        nDice -= m_Count[ONE];
                        m_Count[ONE] = 0;
                    }
                    if (m_Count[SIX] == 3)
                    {
                        iTempScore += 600;
                        Console.WriteLine("Roll {0} - {1} Sixes - Total {2}", nRoll + 1, m_Count[SIX], iTempScore);
                        nDice -= m_Count[SIX];
                        m_Count[SIX] = 0;
                    }
                    if (m_Count[FIVE] == 3)
                    {
                        iTempScore += 500;
                        Console.WriteLine("Roll {0} - {1} Five(s) - Total {2}", nRoll + 1, m_Count[FIVE], iTempScore);
                        nDice -= m_Count[FIVE];
                        m_Count[FIVE] = 0;
                    }
                    if (m_Count[FOUR] == 3)
                    {
                        iTempScore += 400;
                        Console.WriteLine("Roll {0} - {1} Four(s) - Total {2}", nRoll + 1, m_Count[FOUR], iTempScore);
                        nDice -= m_Count[FOUR];
                        m_Count[FOUR] = 0;
                    }
                    if (m_Count[THREE] == 3)
                    {
                        iTempScore += 300;
                        Console.WriteLine("Roll {0} - {1} Three(s) - Total {2}", nRoll + 1, m_Count[THREE], iTempScore);
                        nDice -= m_Count[THREE];
                        m_Count[THREE] = 0;
                    }
                    if (m_Count[TWO] == 3)
                    {
                        iTempScore += 200;
                        Console.WriteLine("Roll {0} - {1} Two(s) - Total {2}", nRoll + 1, m_Count[TWO], iTempScore);
                        nDice -= m_Count[TWO];
                        m_Count[TWO] = 0;
                    }
                }
                else if (bIsRunClean)
                {
                    iTempScore += CheckStranded(nRoll, ref nDice, ref nMult);
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

                        if (iGameScoreCPU < 500)
                        {
                            iTempScore += CheckStranded(nRoll, ref nDice, ref nMult);
                            Console.WriteLine("Dice getting hot but on the board!");
                            if (!GoForIt(ref Rnd, nPlayers, PlayerScore, iGameScoreCPU, iRollScore))
                                bRollLoop = false;
                            Console.WriteLine(bRollLoop ? "Going for it" : "Passing the teeth");
                        }
                        else if (nDice == 0)
                        {
                            if (Rnd.Next(1, 101) > 50)
                            {
                                Console.WriteLine("Dice too hot to handle!");
                                if (!GoForIt(ref Rnd, nPlayers, PlayerScore, iGameScoreCPU, iRollScore))
                                    bRollLoop = false;
                                Console.WriteLine(bRollLoop ? "Going for it" : "Passing the teeth");
                            }
                        }
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

                        if (iGameScoreCPU < 500)
                        {
                            iTempScore += CheckStranded(nRoll, ref nDice, ref nMult);
                            if (!GoForIt(ref Rnd, nPlayers, PlayerScore, iGameScoreCPU, iRollScore))
                                bRollLoop = false;
                        }
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

                        if (iGameScoreCPU < 500)
                        {
                            iTempScore += CheckStranded(nRoll, ref nDice, ref nMult);
                            if (!GoForIt(ref Rnd, nPlayers, PlayerScore, iGameScoreCPU, iRollScore))
                                bRollLoop = false;
                        }
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
                        bool bExtra = m_Count[FOUR] >= 4;
                        iTempScore += (m_Count[FOUR] - 2) * 400;
                        Console.WriteLine("Roll {0} - {1} Four(s) - Total {2}", nRoll + 1, m_Count[FOUR], iTempScore);
                        nDice -= m_Count[FOUR];
                        m_Count[FOUR] = 0;
                        nMult = FOUR;

                        if (iGameScoreCPU < 500 && bExtra)
                        {
                            iTempScore += CheckStranded(nRoll, ref nDice, ref nMult);
                            if (!GoForIt(ref Rnd, nPlayers, PlayerScore, iGameScoreCPU, iRollScore))
                                bRollLoop = false;
                        }
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

                        bool bExtra = m_Count[THREE] >= 4;
                        if (iGameScoreCPU < 500 && bExtra)
                        {
                            iTempScore += CheckStranded(nRoll, ref nDice, ref nMult);
                            if (!GoForIt(ref Rnd, nPlayers, PlayerScore, iGameScoreCPU, iRollScore))
                                bRollLoop = false;
                        }
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

                        bool bExtra = m_Count[TWO] >= 5;
                        if (iGameScoreCPU < 500 && bExtra)
                        {
                            iTempScore += CheckStranded(nRoll, ref nDice, ref nMult);
                            if (!GoForIt(ref Rnd, nPlayers, PlayerScore, iGameScoreCPU, iRollScore))
                                bRollLoop = false;
                        }
                    }
                    else
                    {
                        if (m_Count[ONE] > 0 && iTempScore < 500)
                        {
                            iTempScore += 100;
                            Console.WriteLine("Roll {0} - {1} Ones(s) - Total {2}", nRoll + 1, 1, iTempScore);
                            nDice -= 1;
                            m_Count[ONE]--;
                            nMult = ZERO;
                        }
                        else if (m_Count[FIVE] > 0 && iTempScore < 500)
                        {
                            iTempScore += 50;
                            Console.WriteLine("Roll {0} - {1} Five(s) - Total {2}", nRoll + 1, 1, iTempScore);
                            nDice -= 1;
                            m_Count[FIVE]--;
                            nMult = ZERO;
                        }
                    }
                }

                if (iTempScore == 0)
                {
                    iTempScore += CheckStranded(nRoll, ref nDice, ref nMult);
                    if (iTempScore == 0)
                    {
                        bRollLoop = false;
                        Console.WriteLine("Buela!\r\n");
                        bBuela = true;
                        Thread.Sleep(WAIT);
                    }
                }

                if (bRollLoop)
                {
                    int iChance = Rnd.Next(1, 101);
                    if (iGameScoreCPU < 500)
                    {
                        if (iGameScoreCPU + iRollScore >= 500)
                        {
                            if (nDice > 0)
                            {
                                if (nMult > ZERO)
                                {
                                    if (nMult == ONE || nMult == FIVE || nMult == SIX)
                                    {
                                        int GT = nMult == ONE ? 100 : (nMult == FIVE ? 110 : 120);
                                        if ((iChance * nDice) < GT)
                                        {
                                            if (!GoForIt(ref Rnd, nPlayers, PlayerScore, iGameScoreCPU, iRollScore))
                                            {
                                                Console.WriteLine("Not going for it!");
                                                bRollLoop = false;
                                            }
                                        }
                                        else
                                            Console.WriteLine("Going for it!");
                                    }
                                    else
                                    {
                                        if (iChance < 90)
                                        {
                                            if (!GoForIt(ref Rnd, nPlayers, PlayerScore, iGameScoreCPU, iRollScore))
                                            {
                                                Console.WriteLine("Not going for it!");
                                                bRollLoop = false;
                                            }
                                            else
                                                Console.WriteLine("Going for it!");
                                        }
                                        else
                                            Console.WriteLine("Going for it!");
                                    }
                                }
                                else
                                {
                                    if (!GoForIt(ref Rnd, nPlayers, PlayerScore, iGameScoreCPU, iRollScore))
                                    {
                                        Console.WriteLine("On the board!");
                                        bRollLoop = false;
                                    }
                                }
                            }
                            else
                            {
                                if (iChance > 75)
                                {
                                    Console.WriteLine("Going for it, Fresh Teeth!");
                                    nDice = 6;
                                    nMult = ZERO;
                                }
                                else
                                {
                                    Console.WriteLine("Not going for it. On the board!");
                                    bRollLoop = false;
                                }
                            }
                        }
                        else
                        {
                            if (nDice == 0)
                            {
                                Console.WriteLine("Go Teeth!");
                                nDice = 6;
                            }
                        }
                    }
                    else // todo - build metrics that show how many times these events happen
                    {
                        if (nDice == 0)
                        {
                            if (iRollScore > 2500)
                            {
                                Console.WriteLine("Fresh Teeth, rolled enough and not going for it.");
                                if (!GoForIt(ref Rnd, nPlayers, PlayerScore, iGameScoreCPU, iRollScore))
                                {
                                    bRollLoop = false;
                                    nMult = ZERO;
                                }
                            }
                            else
                            {
                                Console.WriteLine("Fresh Teeth!");
                                nMult = ZERO;
                                nDice = 6;
                            }
                        }
                        else
                        {
                            if (iRollScore > 1500)
                            {
                                if (nDice < 4)
                                    if (!GoForIt(ref Rnd, nPlayers, PlayerScore, iGameScoreCPU, iRollScore))
                                        bRollLoop = false;
                            }
                            else if (iRollScore > 1000)
                            {
                                if (nDice < 3)
                                    if (!GoForIt(ref Rnd, nPlayers, PlayerScore, iGameScoreCPU, iRollScore))
                                        bRollLoop = false;
                            }
                            else
                            {
                                int nDie = Rnd.Next(1, 101);
                                if (nDice == 5)
                                {
                                    if (nDie < 10)
                                        if (!GoForIt(ref Rnd, nPlayers, PlayerScore, iGameScoreCPU, iRollScore))
                                            bRollLoop = false;
                                }
                                else if (nDice == 4)
                                {
                                    if (nDie < 20)
                                        if (!GoForIt(ref Rnd, nPlayers, PlayerScore, iGameScoreCPU, iRollScore))
                                            bRollLoop = false;
                                }
                                else if (nDice == 3)
                                {
                                    if (nDie > 40)
                                        if (!GoForIt(ref Rnd, nPlayers, PlayerScore, iGameScoreCPU, iRollScore))
                                            bRollLoop = false;
                                }
                                else if (nDice == 2)
                                {
                                    if (nDie > 20)
                                        if (!GoForIt(ref Rnd, nPlayers, PlayerScore, iGameScoreCPU, iRollScore))
                                            bRollLoop = false;
                                }
                                else
                                {
                                    if (!GoForIt(ref Rnd, nPlayers, PlayerScore, iGameScoreCPU, iRollScore))
                                        bRollLoop = false;
                                }
                            }

                            if (!bRollLoop)
                                iTempScore += CheckStranded(nRoll, ref nDice, ref nMult);
                        }
                        nRoll++;
                    }
                    Console.WriteLine();
                }

                iRollScore += iTempScore;
                if ((iGameScoreCPU + iRollScore) >= 10000)
                {
                    Console.WriteLine("Good game but this ones mine!\r\n");
                    bRollLoop = false;
                    break;
                }

                if (bRollLoop && nMult != ZERO)
                    Console.WriteLine("{0} is the roll multiplier\r\n", nMult + 1);
            }

            if (!bBuela)
                iGameScoreCPU += iRollScore;

            Console.WriteLine(string.Format("CPU {0} - Round Score {1} - Total Score {2}\r\n", iCPUID, bBuela ? 0 : iRollScore, iGameScoreCPU));
            Thread.Sleep(WAIT);
            return iGameScoreCPU;
        }

        static void Scoring()
        {
            Console.WriteLine("Lesters Teeth v1.0.0");
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
            nPlayers = 0;
            nCPUs = 0;
            nPeople = 0;

            bool bConvert = false;
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
                    int iPlayerID = 1, iCPUID = 1;
                    for (int iPlayer = 0; iPlayer < nPlayers; iPlayer++)
                    {
                        Console.WriteLine(string.Format("{0} {1}",
                        iPlayer < nPeople ? "Player" : "CPU",
                        iPlayer < nPeople ? iPlayer + 1 : iPlayer - nPeople + 1
                        ));

                        if (iPlayer < nPeople)
                            PlayerScore[iPlayer] = RollPlayer(ref Rnd, PlayerScore[iPlayer], iPlayerID++);
                        else
                            PlayerScore[iPlayer] = RollCPU(ref Rnd, nPlayers, PlayerScore, PlayerScore[iPlayer], iCPUID++);

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

                        if (PlayerScore[iPlayer] >= 10000)
                        {
                            bGameLoop = false;
                            break;
                        }

                        Console.WriteLine("It's your roll");
                        Console.WriteLine();
                    }
                }

                Console.WriteLine();
                for (int iPlayer = 0; iPlayer < nPlayers; iPlayer++)
                {
                    if (PlayerScore[iPlayer] >= 10000)
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
                ConsoleKeyInfo cki = Console.ReadKey(true);
                string strKey = cki.KeyChar.ToString();
                if (strKey != "A" && strKey != "a")
                    bAgain = false;
            }
        }
    }
}
