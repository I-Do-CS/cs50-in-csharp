using System.Globalization;

namespace Cash
{
    class Program
    {

        static void Main()
        {
            Console.Write("Change Owed: ");
            string? changeAmount = Console.ReadLine();

            if (string.IsNullOrEmpty(changeAmount) ||
                !float.TryParse(changeAmount, out float dollarAmount) ||
                dollarAmount <= 0)
            {
                Console.WriteLine("Wrong input, try again.");
                Main();
                return;
            }

            MoneyChange change = new(float.Round(dollarAmount, 2));

            change.PrintChangeResult();
        }
    }

    class MoneyChange 
    {
        public readonly float DollarAmount;
        public readonly long PenniesAmount;
        public readonly Dictionary<string, int> Coins = new Dictionary<string, int>()
        {
            {"Quarters", 0},
            {"Dimes", 0},
            {"Nickels", 0},
            {"Pennies", 0},
            {"Total", 0},
        };

        public MoneyChange(float dollarAmount)
        {
            // constants for coin's worths in pennies
            const int QUARTER = 25,
                      DIME = 10,
                      NICKEL = 5,
                      PENNY = 1;

            this.DollarAmount = dollarAmount;
            this.PenniesAmount = ((long)(dollarAmount * 100));

            // Count coins from total of the pennies
            long TotalPennies = this.PenniesAmount;

            while (TotalPennies != 0)
            {
                if (TotalPennies >= QUARTER )
                {
                    TotalPennies -= QUARTER;
                    this.Coins["Quarters"] += 1;
                }
                else if (TotalPennies >= DIME )
                {
                    TotalPennies -= DIME;
                    this.Coins["Dimes"] += 1;
                }
                else if (TotalPennies >= NICKEL)
                {
                    TotalPennies -= NICKEL;
                    this.Coins["Nickels"] += 1;
                }
                else
                {
                    TotalPennies -= PENNY;
                    this.Coins["Pennies"] += 1;
                }
            }
            
            this.Coins["Total"] = this.Coins["Quarters"] + this.Coins["Dimes"] +
                                    this.Coins["Nickels"] + this.Coins["Pennies"];
        }

        public void PrintChangeResult(bool isOwed = true)
        {
            string OwedStatus = isOwed ? "Are Owed" : "Owe"; 
            Console.WriteLine($"You {OwedStatus} {this.Coins["Total"]} Coins:");
            foreach ( var coin in this.Coins )
            {
                Console.WriteLine( coin.Key + " : " + coin.Value );
            }
        }
    }
}