using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CW1_Crawler
{
    class Program
    {
        public async static Task Main(string[] args)
        {
            string pattern = @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            HashSet<String> emails = new HashSet<string>();
            if (args.Length < 1)
            {
                throw new ArgumentNullException();
            }
            string websiteUrl = args[0];
            if (!Uri.IsWellFormedUriString(websiteUrl, UriKind.Absolute))
            {
                throw new ArgumentException();
            }
            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(websiteUrl);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    foreach (Match match in Regex.Matches(content, pattern, RegexOptions.IgnoreCase))
                    {
                        emails.Add(match.Value);
                    }
                    response.Dispose();
                    if (emails.Count == 0)
                    {
                        Console.WriteLine("Nie znaleziono adresów email");
                    }
                    else
                    {
                        Console.WriteLine(string.Join("\n", emails));
                    }

                }
                else
                {
                    throw new Exception("Błąd w czasie pobierania strony");
                }
            }
        }
    }
}
