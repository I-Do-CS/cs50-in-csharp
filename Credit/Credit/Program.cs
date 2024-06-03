// https://cs50.harvard.edu/x/2024/psets/1/credit/

using System.ComponentModel.DataAnnotations;

namespace Credit
{
    class Program
    {
        static void Main()
        {
            Console.Write("Credit Card Number (ctrl + c to exit): ");
            string? creditCardNumber = Console.ReadLine().Trim();

            string creditCardCompany;

            try
            {
                if (string.IsNullOrEmpty(creditCardNumber))
                {
                    throw new Exception();
                }

                foreach (char c in creditCardNumber)
                {
                    if (!char.IsDigit(c))
                    {
                        throw new Exception();
                    }
                }

                if ( !GetCreditCardCompany(creditCardNumber, out creditCardCompany) )
                {
                    throw new Exception();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid Card\n");
                Main();
                return;
            }

            Console.WriteLine($"Credit Card Company: {creditCardCompany}\n");
            Main();
        } 

        static bool GetCreditCardCompany(string creditCardNumber, out string company)
        {
            List<string> everyOtherDigit = [];
            int sum = 0;

            // Luhn's Algorith to check cc number's validity.
            int loopStartingIndex = creditCardNumber.Length % 2;
            if ( loopStartingIndex == 1 )
            {
                sum += Convert.ToInt32(creditCardNumber[0].ToString());
            } 
            for (int i = loopStartingIndex; i < creditCardNumber.Length; i++)
            {
                string currentNum = creditCardNumber[i].ToString();

                if (i % 2 == loopStartingIndex)
                {
                    everyOtherDigit.Add( currentNum );
                }
                else
                {
                    sum += Convert.ToInt32( currentNum );
                }
            }

            for (int i = 0; i < everyOtherDigit.Count; i++)
            {
                string currentNumber = everyOtherDigit[i].ToString();

                int doubled = Convert.ToInt32( currentNumber ) * 2;
                everyOtherDigit[i] = doubled.ToString();
            }

            for (int i = 0; i < everyOtherDigit.Count; i++)
            {
                string currentNumber = everyOtherDigit[i].ToString();

                if ( everyOtherDigit[i].Length > 1 )
                {  
                    sum += Convert.ToInt32(currentNumber[0].ToString());
                    sum += Convert.ToInt32(currentNumber[1].ToString());
                }
                else
                {
                    sum += Convert.ToInt32(currentNumber);
                }
            }

            if ( sum % 10 != 0 )
            {
                company = "INVALID";
                return false;
            }

            // Check Credit Card Company
            int len = creditCardNumber.Length;
            string firstDigits = creditCardNumber[..2];
            string[] masterCardIndex = ["51", "52", "53", "54", "55"];

            if (len == 15 && (
                firstDigits.Equals("34") || firstDigits.Equals("37")))
            {
                company = "American Express";
            }
            else if (len == 16 && (masterCardIndex.Contains(firstDigits)))
            {
                company = "Master Card";
            }
            else if ( (len == 16 || len == 13) && firstDigits.StartsWith("4") )
            {
                company = "VISA";
            }
            else
            {
                company = "INVALID";
                return false;
            }

            return true;
        }
    }
}