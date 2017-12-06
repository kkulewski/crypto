using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vigenere
{
    class Program
    {
        static void Main(string[] args)
        {
            const string parameterHelp = " Try:" +
                                         "\n-p -- prepare text" +
                                         "\n-e -- encrypt text" +
                                         "\n-d -- decrypt text" +
                                         "\n-k -- cryptoanalysis";

            if (args.Length != 1 || args[0] == null)
            {
                Console.WriteLine("Missing action parameter!" + parameterHelp);
                return;
            }

            try
            {
                switch (args[0])
                {
                    case "-p":
                        // prepare text
                        break;

                    case "-e":
                        // encrypt
                        break;

                    case "-d":
                        // decrypt
                        break;

                    case "-k":
                        // cryptoanalysis
                        break;

                    default:
                        Console.WriteLine("Wrong action parameter!" + parameterHelp);
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return;
            }
        }
    }
}
