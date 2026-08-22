using Pxoqxo.ConsolePlus;
using System.Text;

namespace _2faConsole
{
    public static class OtpManager
    {
        public static byte? SelectOption()
        {
            StringBuilder options = new StringBuilder();
            options.AppendLine("1. Add OTP");
            options.AppendLine("2. End OTP");
            options.AppendLine("3. Remove OTP");
            options.AppendLine("4. View OTP");
            options.AppendLine("5. List OTP");
            options.AppendLine("0. Exit");
            options.Append("Select option: ");

            return ConsoleX.ReadByte(options.ToString());
        }
    }
}
