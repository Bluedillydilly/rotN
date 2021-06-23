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
        private const string defaultOutputName = "rotted.txt";
        
        static void Main(string[] args)
        {

            while (true)
            {
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
                using (var fs = new StreamReader( Console.ReadLine() ))
                {
                    Console.WriteLine($"Output to file (leave empty for '{defaultOutputName}'): ");
                    var outputName = Console.ReadLine();
                    outputName = outputName == "" ? defaultOutputName : outputName;
                    var destFile = new StreamWriter( File.Create( outputName ) );

                    string line;
                    while ( (line = fs.ReadLine()) != null )
                    {
                        var rottedLine = rotN(line);
                        destFile.WriteLine( rottedLine );
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
            return;
        }


        static string rotN(string word)
        {
            var sb = new char[word.Length];
            for (var i = 0; i < word.Length; i++)
            {
                if (minAlpha <= word[i] && word[i] <= maxAlpha)
                {
                    var newVal = (word[i] + shiftAmount) % (maxAlpha + 1);
                    if (newVal < minAlpha)
                    {
                        newVal += minAlpha;
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
