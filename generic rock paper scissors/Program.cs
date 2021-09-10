public static class Program
{

    public static void Main(string[] args)
    {

        try
        {
            new Game(args).Start();
        }

        catch (Exception e)
        {

            Console.WriteLine(e.Message);

        }

    }
}