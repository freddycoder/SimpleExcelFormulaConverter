using SimpleExcelFormulaConverter;
using System;
using System.Diagnostics;

namespace PerfTest
{
    class Program
    {
        static string[] formula = new string[]
        {
            "TEXT(TODAY())",
            "5-6",
            "TEXT(TODAY()-3,\"aaaammjj\")&\"120000+0000",
            "TEXT(TODAY(),\"aaaammjj\")&\"120000+0000",
            "TEXT(TODAY()-3,\"aaaammjj\")",
            "TEXT(TODAY()-3,\"aaaammjj\")&\"120000+0000",
            "CONCATENATE(\"/\", TEXT(TODAY() - 5, \"AAAAMMJJ\"))",
            "TEXT(TODAY()-30,\"aaaammjj\")&\"/\"&TEXT(TODAY(),\"aaaammjj\")",
            "TEXT(TODAY()-30,\"aaaammjj\")&\"/\"&TEXT(TODAY()+30,\"aaaammjj\")",
            "\"/\"&\"/\"&\"/\"&\"/\"&\"/\"",
            /*"UnparsableThing+)-))_0-_)",*/
            "TEXT(EDATE(TODAY(),12),\"aaaammjj\")&\"050000+0000\"",
            "TEXT(EDATE(TODAY(),+12),\"aaaammjj\")&\"050000+0000\"",
            "TEXT(EDATE(TODAY()+3,-36),\"aaaammjj\")&\"050000+0000\"&TEXT(EDATE(TODAY()+38,-346),\"aaaammjj\")",
            "TEXT(TODAY()-3+5,\"aaaammjj\")&\"120000+0000",
            "TEXT(TODAY()-30+1, \"aaaammjj\")",
            "TEXT(TODAY()+365+365, \"aaaammjj\")"
        };

        static readonly Random RandEngine = new Random();

        static void Main(string[] args)
        {
            Console.WriteLine("Press enter to start...");
            Console.ReadLine();
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var converter = new FormulaConverterTIPS();

            var nbFormule = 200000;

            for (int i = 0; i < nbFormule; i++)
            {
                int indice = RandEngine.Next(formula.Length);
                var stringChosen = formula[indice];

                try
                {
                    var convertedString = converter.Convert(stringChosen);

                    if (i % 1000 == 0)
                    {
                        Console.WriteLine(convertedString);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("---------------------------------------------------------");
                    Console.WriteLine($"Exception at test {i} with random indices {indice} and string to convert {stringChosen}.");
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.StackTrace);
                }
            }

            Console.WriteLine($"Total elapseMilisencond : {stopWatch.ElapsedMilliseconds}");
            Console.WriteLine($"Moyenne : {(double)stopWatch.ElapsedMilliseconds / nbFormule} ms");
            Console.WriteLine("Test done. Press enter to exit");
            Console.ReadLine();
        }
    }
}
