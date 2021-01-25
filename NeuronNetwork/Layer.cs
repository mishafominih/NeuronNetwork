using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NeuronNetwork
{
    public class Layer<T> where T : MathNeuron
    {
        public List<T> neurons = new List<T>();
        public Layer<T> next;
        public Layer<T> previous;

        public Layer(int count, int countInput)
        {
            for (int i = 0; i < count; i++)
                if (typeof(T) == typeof(MathNeuron))
                    neurons.Add((T)new MathNeuron(countInput));
                else
                    neurons.Add((T)new Neuron(countInput));
        }

        public Layer(List<T> ns) => neurons = ns;

        public void SetNext(Layer<T> l)
        {
            next = l;
            l.previous = this;
        }

        public int GetCountElem() => neurons == null ? 0 : neurons.Count;

        public List<double> ActivateNeurons(List<double> values)
        {
            var res = neurons
                .Select(x => x.Activate(values))
                .ToList();
            return next == null ? res : next.ActivateNeurons(res);
        }

        public Layer<T> GetClone()
        {
            var newNeurons = neurons
                .Select(x => x.GetClone())
                .ToList();
            var res = new Layer<T>(newNeurons.Select(x => (T)x).ToList());
            if (next != null)
                res.SetNext(next.GetClone());
            return res;
        }

        public Layer<T> End()
        {
            var end = this;
            while(end.next != null)
                end = end.next;
            return end;
        }
    }
}
