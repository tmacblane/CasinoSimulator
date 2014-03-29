using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NPack;

namespace Craps
{
    public class Simulator
    {
        #region Fields

        public int NumberOfSimulationsToRun;
        public int FirstDieNumber;
        public int SecondDieNumber;
        public int BaseBetAmount;
        public int BetAmount;
        public double InitialBankRoll;
        public double BankRoll;
        public int GamesPlayed;
        public int GamesWon;
        public double TotalWonLoss = 0;

        public List<int> DieNumberList = new List<int>(new int[] { 1, 2, 3, 4, 5, 6 });
        public List<int> WinningFieldNumbers = new List<int>(new int[] { 2, 3, 4, 9, 10, 11, 12 });
        public Dictionary<int, List<int>> RollDictionary = new Dictionary<int, List<int>>();

        MersenneTwister MersenneTwister = new MersenneTwister(new Random().Next(0, 2000000));

        #endregion

        #region Type specific methods

        public void RunSimulator()
        {
            this.InitialBankRoll = this.SetInitialBankRoll();
            this.BaseBetAmount = this.SetBaseBetAmount();
            this.NumberOfSimulationsToRun = this.SetNumberOfSimulationsToRun();

            this.RollDice(this.NumberOfSimulationsToRun);
            this.Fibonacci();
            // Pick Simulation
        }

        #endregion

        #region Helpers

        private double SetInitialBankRoll()
        {
            Console.Write("Enter Starting Bankroll: ");
            string keyboardInput = Console.ReadLine();
            double baseBet;

            while (!double.TryParse(keyboardInput, out baseBet))
            {
                Console.WriteLine("You have entered an invalid value, try again.");
                Console.Write("Enter Starting Bankroll: ");
                keyboardInput = Console.ReadLine();
            }

            return baseBet;
        }

        private int SetBaseBetAmount()
        {
            Console.Write("Enter Base Bet: ");
            string keyboardInput = Console.ReadLine();
            int baseBet;

            while (!int.TryParse(keyboardInput, out baseBet))
            {
                Console.WriteLine("You have entered an invalid value, try again.");
                Console.Write("Enter Base Bet: ");
                keyboardInput = Console.ReadLine();
            }

            return baseBet;
        }

        private int SetNumberOfSimulationsToRun()
        {
            Console.Write("Number of simulations to run: ");
            string keyboardInput = Console.ReadLine();
            int numberOfSimulationsToRun;

            while (!int.TryParse(keyboardInput, out numberOfSimulationsToRun))
            {
                Console.WriteLine("You have entered an invalid value, try again.");
                Console.Write("Number of simulations to run: ");
                keyboardInput = Console.ReadLine();
            }

            return numberOfSimulationsToRun;
        }

        private void RollDice()
        {
            this.FirstDieNumber = this.DieNumberList[MersenneTwister.Next(this.DieNumberList.Count())];
            this.SecondDieNumber = this.DieNumberList[MersenneTwister.Next(this.DieNumberList.Count())];

            this.RollDictionary.Add(this.RollDictionary.Count + 1, new List<int>(new int[] { this.FirstDieNumber, this.SecondDieNumber }));
        }

        private void RollDice(int numberOfSimulationsToRun)
        {
            for (int i = 1; i <= this.NumberOfSimulationsToRun; i++)
            {
                this.RollDice();
            }
        }

        private void Fibonacci()
        {
            List<int> betAmounts = new List<int>(new int[] { 1, 2, 3, 5, 8, 13, 21, 34, 55, 89 });
            List<string> winLossRecord = new List<string>();
            int betAmountIndex = 0;
            //bool betWon = false;
            int winStreak = 0;
            this.BetAmount = this.BaseBetAmount;
            this.BankRoll = this.InitialBankRoll;
            int maxBetReachedCount = 0;
            int winLossWinReachedCount = 0;
            int startOverCount = 0;
            int maxToLose = 200;
            int maxToWin = 100;

            foreach (List<int> diceRollList in this.RollDictionary.Values)
            {
                // Check if game amount min or max has been reached
                if (this.BankRoll <= this.InitialBankRoll - maxToLose || this.BankRoll >= this.InitialBankRoll + maxToWin)
                {
                    this.TotalWonLoss = this.TotalWonLoss + this.BankRoll;
                    this.GamesPlayed++;

                    if (this.BankRoll >= this.InitialBankRoll + maxToWin)
                    {
                        this.GamesWon++;
                    }

                    // Print Out results 
                    //Console.WriteLine();
                    //Console.WriteLine("Game Finished");
                    //Console.Write("Ending Balance: \t\t");
                    //Console.WriteLine(this.BankRoll);
                    //Console.Write("Max Bet Reached: \t\t");
                    //Console.WriteLine(maxBetReachedCount);
                    //Console.Write("WinLossWin Reached: \t\t");
                    //Console.WriteLine(winLossWinReachedCount);

                    // Reset Betting for another game
                    this.BankRoll = this.InitialBankRoll;
                    betAmountIndex = 0;
                    maxBetReachedCount = 0;
                    winLossRecord.Clear();
                    winLossWinReachedCount = 0;
                    startOverCount = 0;
                    winStreak = 0;
                }
                else
                {
                    // Continue Gambling

                    if (winStreak > 2)
                    {
                        betAmountIndex = 0;
                    }

                    this.BetAmount = betAmounts[betAmountIndex];

                    int diceTotal = diceRollList[0] + diceRollList[1];

                    if (this.WinningFieldNumbers.Contains(diceTotal))
                    {
                        if (diceTotal == 2)
                        {
                            this.BankRoll = this.BankRoll + (this.BetAmount * 2);
                            betAmountIndex = 0;
                        }
                        else if (diceTotal == 12)
                        {
                            this.BankRoll = this.BankRoll + (this.BetAmount * 3);
                            betAmountIndex = 0;
                        }
                        else
                        {
                            this.BankRoll = this.BankRoll + this.BetAmount;
                        }

                        winStreak++;

                        if (betAmountIndex > 0)
                        {
                            betAmountIndex--;
                        }

                        winLossRecord.Add("W");

                        if (winLossRecord.Count >= 3 && winLossRecord[winLossRecord.Count - 3] == "W" && winLossRecord[winLossRecord.Count - 2] == "L" && winLossRecord[winLossRecord.Count - 1] == "W")
                        {
                            betAmountIndex = 0;
                            winLossWinReachedCount++;
                        }
                    }
                    else
                    {
                        this.BankRoll = this.BankRoll - this.BetAmount;
                        winStreak = 0;
                        //betWon = false;

                        if (betAmountIndex < betAmounts.Count - 1)
                        {
                            betAmountIndex++;
                        }
                        else
                        {
                            betAmountIndex = 0;
                            maxBetReachedCount++;
                        }

                        winLossRecord.Add("L");
                    }
                }
            }

            // Print Out results
            Console.WriteLine();
            Console.Write("Games Played: \t\t");
            Console.WriteLine(this.GamesPlayed);
            Console.Write("Games Won: \t\t");
            Console.WriteLine(this.GamesWon);
            Console.Write("Total Balance: \t\t");
            Console.WriteLine(this.TotalWonLoss);
            Console.WriteLine();
            Console.WriteLine("Scenario Over");
            Console.Write("Remaing Balance: \t");
            Console.WriteLine(this.BankRoll);
            //Console.Write("Max Bet Reached: \t\t");
            //Console.WriteLine(maxBetReachedCount);
            //Console.Write("WinLossWin Reached: \t\t");
            //Console.WriteLine(winLossWinReachedCount);
            //Console.Write("Start Over Betting Reached: \t");
            //Console.Write(startOverCount);
            Console.WriteLine();
        }

        #endregion
    }
}