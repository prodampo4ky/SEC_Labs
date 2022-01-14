using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Laba1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            try
            {
                bool isDone = false;
                int Switch;
                string pathIN = "";
                Console.WriteLine("Enter the number of your text:");
                while (!isDone)
                {
                    try
                    {
                        Switch = int.Parse(Console.ReadLine());
                        switch (Switch)
                        {
                            case 1:
                                isDone = true;
                                pathIN = @"C:\Users\Dima\Desktop\Lab4\Gimn.txt";
                                break;
                            case 2:
                                isDone = true;
                                pathIN = @"C:\Users\Dima\Desktop\Lab4\Epoxa.txt";
                                break;
                            case 3:
                                isDone = true;
                                pathIN = @"C:\Users\Dima\Desktop\Lab4\1.txt";
                                break;

                        }

                        Console.WriteLine();
                        Console.WriteLine(pathIN.Split("\\")[5]);
                        Console.WriteLine();
                        Amount_File(pathIN);

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("The file could not be open:");
                        Console.WriteLine(e.Message);
                    }
                }
            }
            catch
            {
                Console.WriteLine("The number must be in the range from 1 to 3. Try again:");
            }



        }

        static void Amount_File(string path)
        {

            double biggest = 0;
            int amount2 = 0;
            int amount3 = 0;


            int tLength = 0;
            char[] arr;

            Dictionary<string, double> frequency1 = new Dictionary<string, double>();
            Dictionary<string, double> frequency2 = new Dictionary<string, double>();
            Dictionary<string, double> frequency3 = new Dictionary<string, double>();

            string[] alphabet = new string[33] {"а", "б", "в", "г", "ґ", "д", "е", "є", "ж", "з", "и",
                                            "і", "ї", "й", "к", "л", "м", "н", "о", "п", "р", "с",
                                            "т", "у", "ф", "х", "ц", "ч", "ш", "щ", "ь", "ю", "я" };


            for (int i = 0; i < alphabet.Length; i++)
            {
                frequency1[alphabet[i]] = 0;
                for (int j = 0; j < alphabet.Length; j++)
                {
                    frequency2[alphabet[i].ToString() + alphabet[j].ToString()] = 0;
                    for (int k = 0; k < alphabet.Length; k++)
                    {
                        frequency3[alphabet[i].ToString() + alphabet[j].ToString() + alphabet[k].ToString()] = 0;
                    }
                }
            }

            using (StreamReader reader = new StreamReader(path, Encoding.GetEncoding("UTF-8")))
            {

                do
                {
                    string line = reader.ReadLine().ToLower();
                    string[] lineWords = line.Split(" ");
                    foreach (string s in lineWords)
                    {
                        tLength += s.Count(Char.IsLetter);

                        arr = s.ToCharArray(0, s.Length);

                        for (int i = 0; i < arr.Length; i++)
                        {
                            for (int j = 0; j < 33; j++)// співпадіння 1 літери
                            {

                                if (alphabet[j] == arr[i].ToString())
                                {
                                    frequency1[alphabet[j]]++;
                                }
                            }
                            if (arr.Length >= 2)// співпадіння 2x літер
                            {
                                if (i == arr.Length - 1) { continue; }
                                for (int j = 0; j < 33; j++)
                                {
                                    for (int k = 0; k < 33; k++)
                                    {
                                        if (alphabet[j] == arr[i].ToString() & alphabet[k] == arr[i + 1].ToString())
                                        {
                                            frequency2[(alphabet[j] + alphabet[k]).ToString()]++;
                                            amount2++;
                                        }
                                    }

                                }
                            }
                            if (arr.Length >= 3)// співпадіння 3x літер
                            {
                                if (i == arr.Length - 2) { continue; }
                                for (int j = 0; j < 33; j++)
                                {
                                    for (int k = 0; k < 33; k++)
                                    {
                                        for (int t = 0; t < 33; t++)
                                        {
                                            if (alphabet[j] == arr[i].ToString() & alphabet[k] == arr[i + 1].ToString() & alphabet[t] == arr[i + 2].ToString())
                                            {
                                                frequency3[(alphabet[j] + alphabet[k] + alphabet[t]).ToString()]++;
                                                amount3++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                } while (!reader.EndOfStream);
            }

            Console.WriteLine("\n");

            var desc = frequency1.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            var asc = frequency1.OrderBy(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            biggest = desc.ElementAt(1).Value;

            WriteOne(frequency1, tLength, biggest);
            WriteOne(desc, tLength, biggest);
            WriteOne(asc, tLength, biggest);

            WriteALL(frequency2, amount2);
            WriteALL(frequency3, amount3);

        }

        static void WriteOne(Dictionary<string, double> freq, int amount, double biggest)
        {
            foreach (var dict in freq)
            {

                double p = dict.Value / amount;
                Console.WriteLine("{0} = {1}, freq={2:f5}\t   |" + Dot(dict.Value / (biggest / 50)), dict.Key, dict.Value, p);

            }
            Console.WriteLine("\n");

        }

        static void WriteALL(Dictionary<string, double> freq, int amount)
        {


            var desc = freq.Where(x => x.Value / amount > 0.0005).OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
            var top = desc.Take(30);
            double biggest = desc.ElementAt(1).Value;

            int c = 0;
            int k = 0;

            foreach (var dict in desc)
            {
                if (dict.Value != 0)
                {
                    if (c == 10)
                    {
                        Console.Write("\n");
                        c = 0;
                    }
                    if (k == 30)
                    {
                        Console.WriteLine();
                    }
                    double p = dict.Value / amount;
                    Console.Write("{0} = {1:f3}; ", dict.Key, p);
                    c++;
                    k++;

                }
            }

            Console.WriteLine("\n");

            foreach (var dict in top)
            {

                double p = dict.Value / amount;
                Console.WriteLine("{0} = {1}, freq={2:f5}\t   |" + Dot(dict.Value / (biggest / 40)), dict.Key, dict.Value, p);

            }

            Console.WriteLine("\n\n");

        }


        static String ReadAllText(StreamReader stream)
        {
            string text = "";
            while (stream.Peek() != -1)
            {
                text += stream.ReadLine();
            }
            return text;
        }

        static string Dot(double n)
        {
            string s = "";
            for (int i = 0; i < n; i++)
            {
                s += ".";
            }
            return s;
        }


    }
}
