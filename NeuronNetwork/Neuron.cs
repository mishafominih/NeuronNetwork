using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NeuronNetwork
{
    /// <summary>
    /// Базовый, простейший нейрон
    /// </summary>
    public class Neuron
    {
        private static Random rand = new Random();
        public List<double> weights = new List<double>();
        public Neuron(int count)
        {
            for (int i = 0; i < count; i++)
                weights.Add(
                    rand.NextDouble()*2-1
                    );
        }

        public Neuron(List<double> w)
        {
            weights = w;
        }

        public Neuron GetClone()//для обучения
        {
            var newWeight = new List<double>();
            foreach (var e in weights)
                if (rand.Next(0, 2) == 0)
                {
                    double weight = e + 0.1 * rand.Next(1, 3) * Math.Pow(-1, rand.Next(1, 3));
                    newWeight.Add(Control(weight));
                }
                else
                {
                    newWeight.Add(e);
                }
            return new Neuron(newWeight);
        }


        public virtual double Activate(List<double> values)
        {
            return Sum(values) / values.Count;
        }

        protected double Control(double weight)
        {
            if (weight > 1) return 1;
            if (weight < 0) return 0;
            return weight;
        }

        protected double Sum(List<double> values)
        {
            var z = 0.0;
            for (int i = 0; i < weights.Count; i++)
                z += values[i] * weights[i];
            return z;
        }
    }
}
