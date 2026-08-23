using _2faConsole;

while (true)
{
    byte? option = OtpManager.SelectOption();

    if (option == 0)
    {
        Console.Clear();
        Console.WriteLine("Thank you for using 2FA Console. Goodbye!");
        Environment.Exit(0);
    }
    else if (option == 1)
    {
        OtpManager.AddOtp();
    }
    else if (option == 2)
    {
        OtpManager.EditOtp();
    }
    else if (option == 3)
    {
        OtpManager.RemoveOtp();
    }
    else if (option == 4)
    {
        OtpManager.ViewOtp();
    }
    else if (option == 5)
    {
        OtpManager.ListOtp();
    }
    else
    {
        Console.Clear();
        Console.WriteLine("That option is not available.");
        Console.WriteLine("Please press any key to try again.");
        Console.ReadKey();
    }
}
