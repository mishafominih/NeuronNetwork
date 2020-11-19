using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuronNetwork
{
    class Program
    {
        const int countNetworks = 5;
        static void Main(string[] args)
        {
            var networks = new List<NeuralNetwork>();
            for(int i = 0; i < countNetworks; i++)
            {
                networks[i] = new NeuralNetwork(4, new int[] { 96, 96, 96, 4 });
            }

            Console.ReadKey();
        }
    }
}
