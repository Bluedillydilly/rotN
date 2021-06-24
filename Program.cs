using System;
using System.IO;

namespace rot13
{
    class Program
    {
        // minimum character in non-escape character ascii
        private const int minAlpha = '!';
        // maximum character in non-escape character ascii
        private const int maxAlpha = '~';
        // how much to 'rotate' each character
        private const int shiftAmount = 29;
        private static bool encode;
        
        static void Main(string[] args)
        {

            while (true)
            {
                Console.WriteLine("(D)ecode or (E)ncode: ");
                encode = Console.ReadLine() == "E" ? true : false ;
                Console.WriteLine("(F)ile or (C)li input:");
                switch(Console.ReadLine())
                {
                    case "F":
                        FileInput();
                        return;
                    case "C":
                        UserInput();
                        return;
                    default:
                        Console.WriteLine($"Enter 'F' to rot{shiftAmount} cypher a file, 'C' for user input.");
                        break;
                }
            }
        }

        static void FileInput()
        {
            try 
            {
                Console.WriteLine("Enter Filename to rot13:");
                var fileName = Console.ReadLine();
                using (var fs = new StreamReader( fileName ))
                {
                    Console.WriteLine($"Output to file (leave empty for 'rot{fileName}'): ");
                    var outputName = Console.ReadLine();
                    outputName = outputName == "" ? fileName + ".rot" : outputName ; 
                    var destFile = new StreamWriter( File.Create( outputName ) );

                    string line;
                    while ( (line = fs.ReadLine()) != null )
                    {
                        destFile.WriteLine( rotN(line) );
                    }
                    destFile.Flush();
                    destFile.Close();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Invalid filename provided");
            }

            return;
        }

        static void UserInput()
        {
            Console.Write($"Message to rot{shiftAmount}: ");
            Console.WriteLine(rotN(Console.ReadLine()));
        }


        static string rotN(string word)
        {
            var sb = new char[word.Length];
            for (var i = 0; i < word.Length; i++)
            {
                if (minAlpha <= word[i] && word[i] <= maxAlpha)
                {

                    // [10-20], shift 6
                    // 10 + 6 = 16
                    // 16 - 6 = 10
                    // (19 + 6) % 21 = 4 = 20, 10, 11, 12, 13, 14
                    // 14 - 6 = 8 = 20, 19

                    int newVal;
                    if (encode)
                    {
                        newVal = (word[i] + shiftAmount) % (maxAlpha + 1);
                    }
                    else{
                        newVal = (word[i] - shiftAmount) % (maxAlpha + 1);
                    }

                    if (newVal < minAlpha)
                    {
                        newVal = encode ? newVal + minAlpha: (maxAlpha + 1) - (minAlpha - newVal);
                    }
                    sb[i] = (char) newVal;

                }
                else {
                    sb[i] = word[i];
                }
            }

            return new string(sb);
        }
    }
}
