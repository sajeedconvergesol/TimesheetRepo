﻿using System.Globalization;
using System.Net.Sockets;
using System.Net;
using System.Text.RegularExpressions;

namespace TMS.API.Helpers
{
    public static class UtilityHelper
    {
        public static int GenerateRandomNumbers()
        {
            int NoDigits = 4;
            Random rnd = new Random();
            int result = Convert.ToInt32(rnd.Next((int)Math.Pow(10, (NoDigits - 1)), (int)Math.Pow(10, NoDigits) - 1).ToString());
            return result;
        }
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    var domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                    @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch
            {
                return false;
            }
        }

        public static string GetClientIPAddress(IPAddress iPAddress)
        {
            string ipAddress = string.Empty;
            IPAddress ip = iPAddress;
            if (ip != null)
            {
                if (ip.AddressFamily == AddressFamily.InterNetworkV6)
                {
                    ip = Dns.GetHostEntry(ip).AddressList
                        .FirstOrDefault(x => x.AddressFamily == AddressFamily.InterNetwork);
                }
                ipAddress = ip.ToString();
            }
            return ipAddress;
        }
        public static string GetUserStatusEnumValue(int statusId)
        {
            switch (statusId)
            {
                case 1:
                    return "Pending";
                case 2:
                    return "Absent Or Cancelled";
                case 3:
                    return "Internet Issue";
                case 4:
                    return "Completed";
            }
            return string.Empty;
        }
        public static string GetExamStatusEnumValue(int statusId)
        {
            switch (statusId)
            {
                case 1:
                    return "Pending";
                case 2:
                    return "In progress";
                case 3:
                    return "Incomplete";
                case 4:
                    return "Completed";
            }
            return string.Empty;
        }
    }
}
