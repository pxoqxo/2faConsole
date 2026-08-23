using Pxoqxo.ConsolePlus;
using Pxoqxo.Ext.Core;
using Pxoqxo.Otp2fa;
using System.Text;

namespace _2faConsole
{
    public static class OtpManager
    {
        public static byte? SelectOption()
        {
            StringBuilder options = new StringBuilder();
            options.AppendLine("1. Add OTP");
            options.AppendLine("2. Edit OTP");
            options.AppendLine("3. Remove OTP");
            options.AppendLine("4. View OTP");
            options.AppendLine("5. List OTP");
            options.AppendLine("0. Exit");
            options.Append("Please select an option: ");

            Console.Clear();
            return ConsoleX.ReadByte(options.ToString());
        }
        public static void AddOtp()
        {
            OtpFileDm otpFileDm = OtpFile.Read();
            Console.Clear();

            try
            {
                OtpDm otpDm = GetOtpDm(otpFileDm, string.Empty);
                otpFileDm.Otps.Add(otpDm);
                OtpFile.Write(otpFileDm);

                Console.WriteLine("Your OTP was added successfully.");
                Console.WriteLine("Press any key to return to the options menu.");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press any key to try again.");
                Console.ReadKey();
            }
        }
        public static void EditOtp()
        {
            OtpFileDm otpFileDm = OtpFile.Read();
            Console.Clear();

            try
            {
                string? id = ConsoleX.ReadString("Enter the ID of the OTP you'd like to edit: ");
                if (id.IsNullOrEmptyOrWhiteSpace())
                {
                    throw new Exception("Please enter a valid OTP ID.");
                }

                OtpDm? otpDm = null;
                foreach (OtpDm item in otpFileDm.Otps)
                {
                    if (item.Id.Equals(id, StringComparison.OrdinalIgnoreCase))
                    {
                        otpDm = item;
                        break;
                    }
                }

                if (otpDm == null)
                {
                    throw new Exception("We couldn't find an OTP with that ID. Please check the ID and try again.");
                }

                OtpDm temp = GetOtpDm(otpFileDm, id);
                otpDm.Id = temp.Id;
                otpDm.Name = temp.Name;
                otpDm.Secret = temp.Secret;
                otpDm.Digits = temp.Digits;
                otpDm.Type = temp.Type;
                otpDm.Counter = temp.Counter;
                otpDm.Incremental = temp.Incremental;
                OtpFile.Write(otpFileDm);

                Console.WriteLine("Your OTP was updated successfully.");
                Console.WriteLine("Press any key to return to the options menu.");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press any key to try again.");
                Console.ReadKey();
            }
        }
        public static void RemoveOtp()
        {
            OtpFileDm otpFileDm = OtpFile.Read();
            Console.Clear();

            try
            {
                string? id = ConsoleX.ReadString("Enter the ID of the OTP you'd like to remove: ");
                if (id.IsNullOrEmptyOrWhiteSpace())
                {
                    throw new Exception("Please enter a valid OTP ID.");
                }

                OtpDm? otpDm = null;
                foreach (OtpDm item in otpFileDm.Otps)
                {
                    if (item.Id.Equals(id, StringComparison.OrdinalIgnoreCase))
                    {
                        otpDm = item;
                        break;
                    }
                }

                if (otpDm == null)
                {
                    throw new Exception("We couldn't find an OTP with that ID. Please check the ID and try again.");
                }

                otpFileDm.Otps.Remove(otpDm);
                OtpFile.Write(otpFileDm);

                Console.WriteLine("Your OTP was removed successfully.");
                Console.WriteLine("Press any key to return to the options menu.");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press any key to try again.");
                Console.ReadKey();
            }
        }
        public static void ViewOtp()
        {
            OtpFileDm otpFileDm = OtpFile.Read();
            Console.Clear();

            try
            {
                string? id = ConsoleX.ReadString("Enter the ID of the OTP you'd like to view: ");
                if (id.IsNullOrEmptyOrWhiteSpace())
                {
                    throw new Exception("Please enter a valid OTP ID.");
                }

                OtpDm? otpDm = null;
                foreach (OtpDm item in otpFileDm.Otps)
                {
                    if (item.Id.Equals(id, StringComparison.OrdinalIgnoreCase))
                    {
                        otpDm = item;
                        break;
                    }
                }

                if (otpDm == null)
                {
                    throw new Exception("We couldn't find an OTP with that ID. Please check the ID and try again.");
                }

                if (otpDm.Type == OtpType.Totp)
                {
                    string otp = Totp.GenerateCode(otpDm.Secret, (int)(otpDm.Counter ?? 30), otpDm.Digits ?? 6, DateTime.Now);
                    Console.WriteLine($"OTP for '{otpDm.Name}': {otp}.");
                    Console.WriteLine("Note: Please make sure your system time is synchronized.");
                    Console.WriteLine("You may need to refresh this page periodically to get the latest TOTP.");
                }
                else if (otpDm.Type == OtpType.Hotp)
                {
                    long counter = otpDm.Counter ?? 0;
                    string otp = Hotp.GenerateCode(otpDm.Secret, counter, otpDm.Digits ?? 6);
                    Console.WriteLine($"OTP for '{otpDm.Name}': {otp} (Counter: {counter})");

                    if (otpDm.Incremental)
                    {
                        otpDm.Counter = counter + 1;
                        OtpFile.Write(otpFileDm);
                        Console.WriteLine("The counter has been increased. A new OTP will be generated the next time you view it.");
                    }
                }

                Console.WriteLine("Press any key to return to the options menu.");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press any key to try again.");
                Console.ReadKey();
            }
        }
        public static void ListOtp()
        {
            OtpFileDm otpFileDm = OtpFile.Read();
            Console.Clear();

            try
            {
                int count = otpFileDm.Otps.Count;
                if (count == 0)
                {
                    Console.WriteLine("No OTP entries were found.");
                    Console.WriteLine("Press any key to return to the options menu.");
                    Console.ReadKey();
                    return;
                }

                foreach (OtpDm otpDm in otpFileDm.Otps)
                {
                    Console.WriteLine($"Name: {otpDm.Name}, ID: {otpDm.Id}");
                }

                Console.WriteLine($"{count} OTP {(count == 1 ? "entry was" : "entries were")} found.");
                Console.WriteLine("Press any key to return to the options menu.");
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Press any key to try again.");
                Console.ReadKey();
            }
        }

        private static OtpDm GetOtpDm(OtpFileDm otpFileDm, string id)
        {
            string name = GetName(otpFileDm, id);
            string secret = GetSecret();
            int? digits = GetDigits();
            OtpType type = GetOtpType();
            long? counter = GetCounter(type);
            bool incremental = GetIncremental(type);

            if (id.IsNullOrEmptyOrWhiteSpace())
            {
                id = Guid.NewGuid().ToString();
            }

            return new OtpDm()
            {
                Id = id,
                Name = name,
                Secret = secret,
                Digits = digits,
                Type = type,
                Counter = counter,
                Incremental = incremental
            };
        }
        private static string GetName(OtpFileDm otpFileDm, string id)
        {
            string? name = ConsoleX.ReadString("Enter the OTP name: ");

            if (name.IsNullOrEmptyOrWhiteSpace())
            {
                throw new Exception("Please enter an OTP name.");
            }
            if (name.Length > 200)
            {
                throw new Exception("The OTP name must be 200 characters or fewer.");
            }
            if (!name.IsEnglish(" ~!@#$%^&*()-_=+[{]}|;:,<.>/?".ToCharArray()))
            {
                throw new Exception("The OTP name contains unsupported characters. Please use English letters and the allowed symbols.");
            }
            foreach (OtpDm item in otpFileDm.Otps)
            {
                if (item.Id.Equals(id, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }
                if (item.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    throw new Exception("An OTP with this name already exists. Please choose a different name.");
                }
            }

            return name;
        }
        private static string GetSecret()
        {
            string? secret = ConsoleX.ReadString("Enter the OTP secret: ");

            if (secret.IsNullOrEmptyOrWhiteSpace())
            {
                throw new Exception("Please enter an OTP secret.");
            }
            if (secret.Length > 40)
            {
                throw new Exception("The OTP secret must be 40 characters or fewer.");
            }
            if (!secret.IsBase32())
            {
                throw new Exception("The OTP secret is not valid Base32. Please check it and try again.");
            }

            return secret;
        }
        private static int? GetDigits()
        {
            int? digits = ConsoleX.ReadInt32("Enter the OTP length: ");

            if (digits <= 0)
            {
                throw new Exception("The OTP length must be greater than zero.");
            }

            return digits;
        }
        private static OtpType GetOtpType()
        {
            string? type = ConsoleX.ReadString("Enter the OTP type (TOTP or HOTP): ");
            if (type.IsNullOrEmptyOrWhiteSpace())
            {
                throw new Exception("Please enter either TOTP or HOTP.");
            }

            if (type.Equals("totp", StringComparison.OrdinalIgnoreCase))
            {
                return OtpType.Totp;
            }
            if (type.Equals("hotp", StringComparison.OrdinalIgnoreCase))
            {
                return OtpType.Hotp;
            }

            throw new Exception("The OTP type is not valid. Please enter either TOTP or HOTP.");
        }
        private static long? GetCounter(OtpType type)
        {
            if (type == OtpType.Totp)
            {
                long? timestep = ConsoleX.ReadInt64("Enter the TOTP time step in seconds: ");
                if (timestep < 0)
                {
                    throw new Exception("The TOTP time step cannot be negative. Please enter a valid value.");
                }
                return timestep;
            }
            if (type == OtpType.Hotp)
            {
                long? counter = ConsoleX.ReadInt64("Enter the initial HOTP counter: ");
                if (counter < 0)
                {
                    throw new Exception("The initial HOTP counter cannot be negative. Please enter a valid value.");
                }
                return counter;
            }

            throw new Exception("The OTP type is not valid. Please select either TOTP or HOTP.");
        }
        private static bool GetIncremental(OtpType type)
        {
            if (type == OtpType.Totp)
            {
                return false;
            }
            if (type == OtpType.Hotp)
            {
                bool? incremental = ConsoleX.ReadBoolean("Increase the counter after viewing the OTP? (true/false): ");
                return incremental ?? false;
            }

            throw new Exception("The OTP type is not valid. Please select either TOTP or HOTP.");
        }
    }
}
