namespace MarioMore
{
    class Program
    {
        static void Main()
        {
            Console.Write("Pyramid Height: ");
            string? heightInput = Console.ReadLine();

            int height;
            try
            {
                height = Convert.ToInt32(heightInput);

                if (height > 10) 
                {
                    throw new FormatException();
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Height invalid or bigger than 10, try again\n");
                Main();
                return;
            }

            PrintPyramid(height);
        }

        static void PrintPyramid(int height)
        {
            for (int i = 0; i < height; i++)
            {
                Console.Write( new string(' ', height - i - 1) );
                Console.Write( new string('#', i + 1) );
                Console.Write("  ");
                Console.WriteLine(new string('#', i + 1));

            }
        }
    }
}