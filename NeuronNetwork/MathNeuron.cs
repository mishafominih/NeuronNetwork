using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuronNetwork
{
    /// <summary>
    /// Математический нейрон
    /// </summary>
    public class MathNeuron : Neuron
    {
        public List<double> Cash { get; private set; }
        public MathNeuron(int count) : base(count) { }
        public MathNeuron(List<double> w) : base(w) { }

        public override double Activate(List<double> values)
        {
            Check();
            var res = 1 / (1 + Math.Exp(-Sum(values)));
            Cash.Add(res);
            return res;
        }

        public void Null()
        {
            Check();
            Cash.Clear();
        }

        private void Check()
        {
            if (Cash == null)
                Cash = new List<double>();
        }
    }
}
