using System;
using System.IO;
using System.Text;

namespace OTP
{
    static class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1 || args[0] == null)
            {
                Console.WriteLine("Missing action parameter!");
                return;
            }

            switch (args[0])
            {
                case "-p":
                    //PrepareText();
                    break;

                case "-e":
                    //Encryp();
                    break;

                case "-k":
                    //Cryptoanalysis();
                    break;

                default:
                    Console.WriteLine("Wrong action parameter! Try:" + 
                        "\n-p -- prepare text" + 
                        "\n-e -- encrypt text" + 
                        "\n-k -- cryptoanalysis");
                    break;
            }
        }
    }
}
