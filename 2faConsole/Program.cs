using _2faConsole;

while (true)
{
    Console.Clear();
    byte? option = OtpManager.SelectOption();

    if (option == 0)
    {
        Environment.Exit(0);
    }
    else if (option == 1)
    {

    }
    else
    {
        Console.WriteLine("No option is avaibale!");
        Console.WriteLine("Please retry...");
        Thread.Sleep(500);
    }
}
