using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuronNetwork
{
    class Program
    {
        static void Main(string[] args)
        {
            var network = new MathNeuronNetwork(new int[] { 7, 14, 1 });
            //network.Load("load.txt");
            network.binaryLearn("BigLearn.txt", 0.1, 100000);

            Console.WriteLine();

            network.Save("load.txt");

            Console.ReadKey();
        }
    }
}
