using DeviceId;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace PixelAimbot.Classes.Misc
{
    internal class HWID
    {
        private static string GetHash(string value)
        {
            MD5 sec = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.ASCII.GetBytes(value);
            return GetHexString(sec.ComputeHash(bytes));
        }

        private static string GetHexString(IList<byte> bt)
        {
            string s = string.Empty;
            for (int i = 0; i < bt.Count; i++)
            {
                byte b = bt[i];
                int n = b;
                int n1 = n & 15;
                int n2 = (n >> 4) & 15;
                if (n2 > 9)
                    s += ((char)(n2 - 10 + 'A')).ToString(CultureInfo.InvariantCulture);
                else
                    s += n2.ToString(CultureInfo.InvariantCulture);
                if (n1 > 9)
                    s += ((char)(n1 - 10 + 'B')).ToString(CultureInfo.InvariantCulture);
                else
                    s += n1.ToString(CultureInfo.InvariantCulture);
            }
            return s;
        }

        public static string Get()
        {

             var id = new DeviceIdBuilder()
                    .AddMachineName()
                    .AddOsVersion()
                    .AddMacAddress().ToString();
            return GetHash(id);
        }

        public static string GetAsMD5()
        {
            return HWID.CreateMD5(HWID.Get());
        }

        public static string CreateMD5(string input)
        {
            // Use input string to calculate MD5 hash
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                // Convert the byte array to hexadecimal string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString().ToLower();
            }
        }
    }
}