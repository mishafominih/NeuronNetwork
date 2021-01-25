using System.Collections;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NeuronNetwork
{
    /// <summary>
    /// Базовая нейронная сеть, использующая любой нейрон и обучение методом потомков
    /// </summary>
    public class NeuralNetwork<T> where T : MathNeuron
    {
        protected Layer<T> start;
        protected int count;
        protected int[] lenghts;

        public NeuralNetwork(int[] lenghts)
        {
            count = lenghts.Length;
            this.lenghts = lenghts;
            start = newLayer();
        }

        public List<double> Run(List<double> input) => start.ActivateNeurons(input);

        /// <summary>
        /// обучение методом потомков
        /// </summary>
        public void Study(string path, double d) 
        {
            var input = new List<List<double>>();
            var output = new List<List<double>>();
            GetData(ref input, ref output, path);

            var start = new List<double>();
            var end = new List<double>();
            while (true)
            {
                var succes = true;
                for (int i = 0; i < input.Count; i++)
                {
                    start = input[i];
                    end = output[i];
                    var res = Run(start);
                    if (Compare(res, end) > d)
                    {
                        succes = false;
                        break;
                    }
                }
                if(succes) 
                    return;
                Learn(d, start, end);
            }
        }

        public void Save(string path)
        {
            var result = new List<string> { count.ToString() };
            var layer = start;
            while (layer != null)
            {
                var str = new StringBuilder();
                foreach (var n in layer.neurons)
                {
                    n.weights.ForEach(weight => str.Append(weight.ToString() + ' '));
                    str = str.Remove(str.Length - 1, 1);
                    str.Append('|');
                }
                str = str.Remove(str.Length - 1, 1);
                result.Add(str.ToString());

                layer = layer.next;
            }
            File.WriteAllLines(path, result);
        }

        public void Load(string path)
        {
            var lines = File.ReadAllLines(path);
            count = int.Parse(lines[0]);
            var lenghts = new List<int>();
            var res = new Layer<T>(0, 0);
            var prev = res;
            for (int i = 1; i < lines.Length; i++)
            {
                var neurons = lines[i]
                    .Split('|')
                    .Select(x => x.Split(' ')
                        .Select(y => double.Parse(y)).ToList())
                    .Select(x => typeof(T) == typeof(MathNeuron) ? (T)new MathNeuron(x) : (T)new Neuron(x))
                    .ToList();
                lenghts.Add(neurons.Count());
                var layer = new Layer<T>(neurons);
                prev.SetNext(layer);
                prev = layer;
            }
            this.lenghts = lenghts.ToArray();
            start = res.next;
        }

        private void Learn(double d, List<double> start, List<double> end)
        {
            var layers = Enumerable.Range(0, 5).Select(y => this.start.GetClone()).ToList();
            double v = Compare(layers.First().ActivateNeurons(start), end);
            while (v >= d)
            {
                //0layers.Add(newLayer());
                layers = layers
                    .SelectMany(x => GetClons(x))
                    .OrderBy(x => Compare(x.ActivateNeurons(start), end))
                    .Take(5)
                    .ToList();
                v = Compare(layers.First().ActivateNeurons(start), end);
            }
            this.start = layers.First();
        }

        protected Layer<T> newLayer()
        {
            var res = new Layer<T>(lenghts[1], lenghts[0]);
            var prevLayer = res;
            for (int i = 2; i < count; i++)
            {
                var t = new Layer<T>(lenghts[i], lenghts[i - 1]);
                prevLayer.SetNext(t);
                prevLayer = t;
            }
            return res;
        }

        protected static List<Layer<T>> GetClons(Layer<T> layer)
        {
            var list = Enumerable.Range(0, 5).Select(y => layer.GetClone()).ToList();
            list.Add(layer);
            return list;
        }

        //загружает данные для обучения из файла
        protected void GetData(ref List<List<double>> start, ref List<List<double>> end, string path)
        {
            var strs = File.ReadAllLines(path);
            start = GetData(strs, 0);
            end = GetData(strs, 1);
        }

        protected static List<List<double>> GetData(string[] strs, int n)
        {//1 1:0 0
            return strs
                .Select(x => x.Split(':')[n])
                .Select(x => x.Split(' '))
                .Select(x => x.Select(y => double.Parse(y)).ToList())
                .ToList();
        }

        protected double Compare(List<double> first, List<double> second)
        {
            return Math.Abs(first.Sum() - second.Sum());
        }

        protected List<T> GetAllNeurons()
        {
            var res = new List<T>();
            var layer = start;
            while(layer != null)
            {
                res.AddRange(layer.neurons);
                layer = layer.next;
            }
            return res;
        }
    }
}
