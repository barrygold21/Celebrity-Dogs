using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dog_Top_Trumps
{
    public class DogStats
    {
        public static string[] DogCard = new string[30];
        public static int[] exercise = new int[30]; // highest wins 1-5
        public static int[] intelligence = new int[30]; // highest wins 1-100
        public static int[] friendliness = new int[30]; // highest wins 1-10
        public static int[] drool = new int[30]; // lowest wins 1-10
        public static List<int> pCards = new List<int>();
        public static List<int> cpuCards = new List<int>();
    }

    class Program
    {
        static void Main()
        {
            Console.Clear();
            Console.Title = "dog card";
            Console.WriteLine("dog card");
            Console.WriteLine("press key");
            Console.ReadKey(true);
            MainMenu();
        }

        public static void MainMenu()
        {
            bool mnuChoValid = false;
            bool cheatMode = false;
            int mCho = 0;
            do
            {
                do
                {
                    Console.Clear();
                    Console.WriteLine("menu");
                    Console.WriteLine("1. play 2. quit");
                    Console.WriteLine("pls enter choice as number");
                    Console.WriteLine();
                    string userChoice = Console.ReadLine();
                    bool isNumeric = int.TryParse(userChoice, out mCho);
                    if (isNumeric)
                    {
                        mnuChoValid = true;
                    }
                    else
                    {
                        if (userChoice == "ACTIVATE_CHEATS")
                        {
                            cheatMode = true;
                            Console.Clear();
                            Console.Write("Cheats: ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("ACTIVE");
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.White;
                            System.Threading.Thread.Sleep(2000);
                        }
                        else if (userChoice == "DEACTIVATE_CHEATS")
                        {
                            cheatMode = false;
                            Console.Clear();
                            Console.Write("Cheats: ");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write("DISABLED");
                            Console.WriteLine();
                            Console.ForegroundColor = ConsoleColor.White;
                            System.Threading.Thread.Sleep(2000);
                        }
                        else
                        {
                            Console.Clear();
                            Console.WriteLine("bad input");
                            System.Threading.Thread.Sleep(2000);
                            mnuChoValid = false;
                        }
                    }
                } while (mnuChoValid == false);
                mnuChoValid = false;
                if (mCho == 1)
                {
                    mnuChoValid = true;
                    PlayGame(cheatMode);
                }
                else if (mCho == 2)
                {
                    mnuChoValid = true;
                    Console.WriteLine("program will close");
                    System.Threading.Thread.Sleep(1000);
                    Environment.Exit(0);
                }
                else
                {
                    mnuChoValid = false;
                    Console.Clear();
                    Console.Write("bad input");
                    System.Threading.Thread.Sleep(2000);
                }
            } while (mnuChoValid == false);
        }

        public static void PlayGame(bool cheatMode)
        {
            string lastRoundWinner = "p";
            bool winnerDecided = false;
            string winner;
            int cardsInPlay = EnterCardNumber();
            ReadFile(cardsInPlay);
            RandomiseStats(cardsInPlay);
            do
            {
                DisplayTopCard();
                if (cheatMode == true)
                {
                    DisplayCPUsCard();
                }
                int nextCategory = ChooseNextCategory(lastRoundWinner, cheatMode);
                ComparativeTable(nextCategory);
                Console.WriteLine("Press any key to continue");
                Console.ReadKey(true);
                Console.Clear();
                lastRoundWinner = CompareStats(nextCategory);
                if (lastRoundWinner == "p")
                {
                    Console.WriteLine("You won the last round!");
                    Console.WriteLine("As such, you will receive the card CPU used last round");
                    Console.WriteLine("Both this and your top card will go to the bottom of your deck.");
                    Console.WriteLine("You will also be allowed to choose the next category to be played from.");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey(true);
                }
                else if (lastRoundWinner == "c")
                {
                    Console.WriteLine("Unfortunately, the CPU won this round.");
                    Console.WriteLine("As such, it will receive the card you were using last round.");
                    Console.WriteLine("That card and the card it was using will go to the bottom of its deck.");
                    Console.WriteLine("The CPU will also choose the next category to be played.");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey(true);
                }
                else if (lastRoundWinner == "DRAW")
                {
                    Console.WriteLine("The last round was a draw, and so you will win the round.");
                    Console.WriteLine("As such, you will receive the card CPU used last round");
                    Console.WriteLine("Both this and your top card will go to the bottom of your deck.");
                    Console.WriteLine("You will also be allowed to choose the next category to be played from.");
                    Console.WriteLine("Press any key to continue");
                    Console.ReadKey(true);
                }
                MoveCardsToCorrectPlaces(lastRoundWinner);
                winner = WinDetection();
                if (winner == "")
                {
                    winnerDecided = false;
                }
                else if (winner != "")
                {
                    winnerDecided = true;
                }
            } while (winnerDecided == false);
            if (winner == "p")
            {
                YouWin();
            }
            else if (winner == "c")
            {
                YouLose();
            }
        }

        public static int EnterCardNumber()
        {
            bool inputValid = false;
            int cardsInPlay = 0;
            do
            {
                do
                {
                    Console.Clear();
                    Console.WriteLine("enter card number pls");
                    string userInput = Console.ReadLine();
                    bool IsNumeric = int.TryParse(userInput, out cardsInPlay);
                    if (IsNumeric)
                    {
                        Console.Clear();
                        inputValid = true;
                    }
                    else
                    {
                        Console.Clear();
                        inputValid = false;
                        Console.WriteLine("bad input");
                        System.Threading.Thread.Sleep(500);
                    }
                } while (inputValid == false);
                inputValid = false;
                if (cardsInPlay >= 4 & cardsInPlay % 2 == 0 & cardsInPlay <= 30)
                {
                    Console.Clear();
                    inputValid = true;
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("bad input");
                    System.Threading.Thread.Sleep(2500);
                }
            } while (inputValid == false);
            return cardsInPlay;
        }

        public static void ReadFile(int cardsInPlay)
        {
            using (StreamReader fileReader = new StreamReader(@"C:\Users\Conor\Documents\TestFile.txt"))
            {
                string line;
                int i = 0;
                while ((line = fileReader.ReadLine()) != null)
                {
                    if (i == cardsInPlay)
                    {
                        break;
                    }
                    if (i % 2 == 0)
                    {
                        DogStats.pCards.Add(i);
                    }
                    else if (i % 2 == 1)
                    {
                        DogStats.cpuCards.Add(i);
                    }
                    DogStats.DogCard[i] = line;
                    i = i + 1;
                }
            }
        }

        public static void RandomiseStats(int cardsInPlay)
        {
            Random randomStats = new Random();
            for (int i = 0; i < cardsInPlay; i++)
            {
                DogStats.exercise[i] = randomStats.Next(1, 5);
                DogStats.intelligence[i] = randomStats.Next(1, 100);
                DogStats.friendliness[i] = randomStats.Next(1, 10);
                DogStats.drool[i] = randomStats.Next(1, 10);
            }
        }

        public static void DisplayTopCard()
        {
            Console.WriteLine();
            Console.WriteLine("Dog: {0}", DogStats.DogCard[DogStats.pCards[0]]);
            Console.WriteLine("--------------------");
            Console.WriteLine("Exercise:        {0}", DogStats.exercise[DogStats.pCards[0]]);
            Console.WriteLine("--------------------");
            Console.WriteLine("Intelligence:    {0}", DogStats.intelligence[DogStats.pCards[0]]);
            Console.WriteLine("--------------------");
            Console.WriteLine("Friendliness:    {0}", DogStats.friendliness[DogStats.pCards[0]]);
            Console.WriteLine("--------------------");
            Console.WriteLine("Drool:           {0}", DogStats.drool[DogStats.pCards[0]]);
            Console.WriteLine("--------------------");
        }

        public static void DisplayCPUsCard()
        {
            Console.WriteLine();
            Console.WriteLine("Dog: {0}", DogStats.DogCard[DogStats.cpuCards[0]]);
            Console.WriteLine("--------------------");
            Console.WriteLine("Exercise:        {0}", DogStats.exercise[DogStats.cpuCards[0]]);
            Console.WriteLine("--------------------");
            Console.WriteLine("Intelligence:    {0}", DogStats.intelligence[DogStats.cpuCards[0]]);
            Console.WriteLine("--------------------");
            Console.WriteLine("Friendliness:    {0}", DogStats.friendliness[DogStats.cpuCards[0]]);
            Console.WriteLine("--------------------");
            Console.WriteLine("Drool:           {0}", DogStats.drool[DogStats.cpuCards[0]]);
            Console.WriteLine("--------------------");
        }

        public static int ChooseNextCategory(string lastRoundWinner, bool cheatMode)
        {
            int nextCategory = 0;
            bool inputValid = false;
            if (lastRoundWinner == "p" || lastRoundWinner == "DRAW")
            {
                do
                {
                    Console.WriteLine();
                    Console.WriteLine("Please enter which category you wish to play with as a number:");
                    Console.WriteLine();
                    Console.WriteLine("1. Exercise");
                    Console.WriteLine("2. Intelligence");
                    Console.WriteLine("3. Friendliness");
                    Console.WriteLine("4. Drool");
                    Console.WriteLine();
                    string usersCategory = Console.ReadLine();
                    bool isNumeric = int.TryParse(usersCategory, out nextCategory);
                    if (isNumeric)
                    {
                        if (nextCategory == 1 || nextCategory == 2 || nextCategory == 3 || nextCategory == 4)
                        {
                            inputValid = true;
                        }
                        else
                        {
                            inputValid = false;
                            Console.Clear();
                            Console.WriteLine();
                            Console.WriteLine("bad input");
                            Console.WriteLine("try again silly");
                            System.Threading.Thread.Sleep(1000);
                            Console.Clear();
                            DisplayTopCard();
                            if (cheatMode == true)
                            {
                                DisplayCPUsCard();
                            }
                        }
                    }
                    else
                    {
                        inputValid = false;
                        Console.Clear();
                        Console.WriteLine();
                        Console.WriteLine("bad input");
                        Console.WriteLine("try again silly");
                        System.Threading.Thread.Sleep(1000);
                        Console.Clear();
                        DisplayTopCard();
                        if (cheatMode == true)
                        {
                            DisplayCPUsCard();
                        }
                    }
                } while (inputValid == false);
            }
            else if (lastRoundWinner == "c")
            {
                Random randomInt = new Random();
                nextCategory = randomInt.Next(1, 4);
            }
            Console.Clear();
            return nextCategory;
        }

        public static void ComparativeTable(int nextCategory)
        {
            Console.Write("Category:        ");
            Console.Write("{0}    ", DogStats.DogCard[DogStats.pCards[0]]);
            Console.Write("{0}", DogStats.DogCard[DogStats.cpuCards[0]]);
            Console.WriteLine();
            Console.WriteLine("-------------------------------");
            if (nextCategory == 1)
            {
                Console.Write("Exercise:         ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("{0}        ", DogStats.exercise[DogStats.pCards[0]]);
                Console.Write("{0}   ", DogStats.exercise[DogStats.cpuCards[0]]);
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("-------------------------------");
                Console.Write("Intelligence:     ");
                Console.Write("{0}       ", DogStats.intelligence[DogStats.pCards[0]]);
                Console.Write("{0}   ", DogStats.intelligence[DogStats.cpuCards[0]]);
                Console.WriteLine();
                Console.WriteLine("-------------------------------");
                Console.Write("Friendliness:     ");
                Console.Write("{0}        ", DogStats.friendliness[DogStats.pCards[0]]);
                Console.Write("{0}   ", DogStats.friendliness[DogStats.cpuCards[0]]);
                Console.WriteLine();
                Console.WriteLine("-------------------------------");
                Console.Write("Drool:            ");
                Console.Write("{0}        ", DogStats.drool[DogStats.pCards[0]]);
                Console.Write("{0}   ", DogStats.drool[DogStats.cpuCards[0]]);
                Console.WriteLine();
                Console.WriteLine("-------------------------------");
            }
            else if (nextCategory == 2)
            {
                Console.Write("Exercise:         ");
                Console.Write("{0}        ", DogStats.exercise[DogStats.pCards[0]]);
                Console.Write("{0}   ", DogStats.exercise[DogStats.cpuCards[0]]);
                Console.WriteLine();
                Console.WriteLine("-------------------------------");
                Console.Write("Intelligence:     ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("{0}       ", DogStats.intelligence[DogStats.pCards[0]]);
                Console.Write("{0}   ", DogStats.intelligence[DogStats.cpuCards[0]]);
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("-------------------------------");
                Console.Write("Friendliness:     ");
                Console.Write("{0}        ", DogStats.friendliness[DogStats.pCards[0]]);
                Console.Write("{0}   ", DogStats.friendliness[DogStats.cpuCards[0]]);
                Console.WriteLine();
                Console.WriteLine("-------------------------------");
                Console.Write("Drool:            ");
                Console.Write("{0}        ", DogStats.drool[DogStats.pCards[0]]);
                Console.Write("{0}   ", DogStats.drool[DogStats.cpuCards[0]]);
                Console.WriteLine();
                Console.WriteLine("-------------------------------");
            }
            else if (nextCategory == 3)
            {
                Console.Write("Exercise:         ");
                Console.Write("{0}        ", DogStats.exercise[DogStats.pCards[0]]);
                Console.Write("{0}   ", DogStats.exercise[DogStats.cpuCards[0]]);
                Console.WriteLine();
                Console.WriteLine("-------------------------------");
                Console.Write("Intelligence:     ");
                Console.Write("{0}       ", DogStats.intelligence[DogStats.pCards[0]]);
                Console.Write("{0}   ", DogStats.intelligence[DogStats.cpuCards[0]]);
                Console.WriteLine();
                Console.WriteLine("-------------------------------");
                Console.Write("Friendliness:     ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("{0}       ", DogStats.friendliness[DogStats.pCards[0]]);
                Console.Write("{0}   ", DogStats.friendliness[DogStats.cpuCards[0]]);
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("-------------------------------");
                Console.Write("Drool:            ");
                Console.Write("{0}        ", DogStats.drool[DogStats.pCards[0]]);
                Console.Write("{0}   ", DogStats.drool[DogStats.cpuCards[0]]);
                Console.WriteLine();
                Console.WriteLine("-------------------------------");
            }
            else if (nextCategory == 4)
            {
                Console.Write("Exercise:         ");
                Console.Write("{0}        ", DogStats.exercise[DogStats.pCards[0]]);
                Console.Write("{0}   ", DogStats.exercise[DogStats.cpuCards[0]]);
                Console.WriteLine();
                Console.WriteLine("-------------------------------");
                Console.Write("Intelligence:     ");
                Console.Write("{0}       ", DogStats.intelligence[DogStats.pCards[0]]);
                Console.Write("{0}   ", DogStats.intelligence[DogStats.cpuCards[0]]);
                Console.WriteLine();
                Console.WriteLine("-------------------------------");
                Console.Write("Friendliness:     ");
                Console.Write("{0}        ", DogStats.friendliness[DogStats.pCards[0]]);
                Console.Write("{0}   ", DogStats.friendliness[DogStats.cpuCards[0]]);
                Console.WriteLine();
                Console.WriteLine("-------------------------------");
                Console.Write("Drool:            ");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write("{0}        ", DogStats.drool[DogStats.pCards[0]]);
                Console.Write("{0}   ", DogStats.drool[DogStats.cpuCards[0]]);
                Console.WriteLine();
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("-------------------------------");
            }

        }

        public static string CompareStats(int nextCategory)
        {
            string lastRoundWinner = "";
            if (nextCategory == 1)
            {
                if (DogStats.exercise[DogStats.pCards[0]] > DogStats.exercise[DogStats.cpuCards[0]])
                {
                    lastRoundWinner = "p";
                }
                else if (DogStats.exercise[DogStats.pCards[0]] < DogStats.exercise[DogStats.cpuCards[0]])
                {
                    lastRoundWinner = "c";
                }
                else
                {
                    lastRoundWinner = "DRAW";
                }
            }
            else if (nextCategory == 2)
            {
                if (DogStats.intelligence[DogStats.pCards[0]] > DogStats.intelligence[DogStats.cpuCards[0]])
                {
                    lastRoundWinner = "p";
                }
                else if (DogStats.intelligence[DogStats.pCards[0]] < DogStats.intelligence[DogStats.cpuCards[0]])
                {
                    lastRoundWinner = "c";
                }
                else
                {
                    lastRoundWinner = "DRAW";
                }
            }
            else if (nextCategory == 3)
            {
                if (DogStats.friendliness[DogStats.pCards[0]] > DogStats.friendliness[DogStats.cpuCards[0]])
                {
                    lastRoundWinner = "p";
                }
                else if (DogStats.friendliness[DogStats.pCards[0]] < DogStats.friendliness[DogStats.cpuCards[0]])
                {
                    lastRoundWinner = "c";
                }
                else
                {
                    lastRoundWinner = "DRAW";
                }
            }
            else if (nextCategory == 4)
            {
                if (DogStats.drool[DogStats.pCards[0]] < DogStats.drool[DogStats.cpuCards[0]])
                {
                    lastRoundWinner = "p";
                }
                else if (DogStats.drool[DogStats.pCards[0]] > DogStats.drool[DogStats.cpuCards[0]])
                {
                    lastRoundWinner = "c";
                }
                else
                {
                    lastRoundWinner = "DRAW";
                }
            }
            return lastRoundWinner;
        }

        public static void MoveCardsToCorrectPlaces(string lastRoundWinner)
        {
            List<int> temp = new List<int>();
            if (lastRoundWinner == "p" || lastRoundWinner == "DRAW")
            {
                temp.Add(DogStats.pCards[0]);
                DogStats.pCards = DogStats.pCards.Except(temp).ToList();
                DogStats.pCards.Add(temp[0]);
                temp.RemoveAt(0);
                temp.Add(DogStats.cpuCards[0]);
                DogStats.cpuCards = DogStats.cpuCards.Except(temp).ToList();
                DogStats.pCards.Add(temp[0]);
                temp.RemoveAt(0);
            }
            else if (lastRoundWinner == "c")
            {
                temp.Add(DogStats.cpuCards[0]);
                DogStats.cpuCards = DogStats.cpuCards.Except(temp).ToList();
                DogStats.cpuCards.Add(temp[0]);
                temp.RemoveAt(0);
                temp.Add(DogStats.pCards[0]);
                DogStats.pCards = DogStats.pCards.Except(temp).ToList();
                DogStats.cpuCards.Add(temp[0]);
                temp.RemoveAt(0);
            }
        }

        public static string WinDetection()
        {
            string winner = "";
            if (DogStats.pCards.Count == 0)
            {
                winner = "c";
            }
            else if (DogStats.cpuCards.Count == 0)
            {
                winner = "p";
            }
            return winner;
        }

        public static void YouWin()
        {
            bool userPlayAgain = false;
            do
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Congratulations! You win.");
                Console.WriteLine();
                Console.WriteLine("Would you like to play again? (Y/N)");
                string playAgain = Console.ReadLine();
                playAgain = playAgain.ToUpper();
                Console.Clear();
                if (playAgain == "Y")
                {
                    userPlayAgain = true;
                    Main();
                }
                else if (playAgain == "N")
                {
                    userPlayAgain = false;
                    Console.WriteLine("Thank you for playing!");
                    System.Threading.Thread.Sleep(1000);
                    Environment.Exit(0);
                }
                else
                {
                    userPlayAgain = false;
                    Console.WriteLine("Please enter a valid character!");
                    System.Threading.Thread.Sleep(5000);
                }
            } while (userPlayAgain == false);
        }

        public static void YouLose()
        {
            bool userPlayAgain = false;
            do
            {
                Console.Clear();
                Console.WriteLine();
                Console.WriteLine("Ouch! Seems like you didn't win this time around.");
                Console.WriteLine("Better luck next time!");
                Console.WriteLine();
                Console.WriteLine("Would you like to play again? (Y/N)");
                string playAgain = Console.ReadLine();
                playAgain = playAgain.ToUpper();
                Console.Clear();
                if (playAgain == "Y")
                {
                    userPlayAgain = true;
                    Main();
                }
                else if (playAgain == "N")
                {
                    userPlayAgain = false;
                    Console.WriteLine("Thank you for playing!");
                    System.Threading.Thread.Sleep(1000);
                    Environment.Exit(0);
                }
                else
                {
                    userPlayAgain = false;
                    Console.WriteLine("Please enter a valid character!");
                    System.Threading.Thread.Sleep(1000);
                }
            } while (userPlayAgain == false);
        }
    }
}