using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
//using Gradient = System.Collections.Generic.List<double>;//содержит список всех градиентов для одного нейрона

namespace NeuronNetwork
{
    /// <summary>
    /// Нейронная сеть, использующая математический нейрон и обучение методом градиентного спуска
    /// на выходе 1 или 0
    /// </summary>
    public class MathNeuronNetwork : NeuralNetwork<MathNeuron>
    {
        //Исправить ChangeWeight!!!!
        public MathNeuronNetwork(int[] lenghts) : base(lenghts) { }

        /// <summary>
        /// обучение на основе градиентного спуска
        /// </summary>
        public void binaryLearn(string path, double alpha, int max_iters)
        {
            var input = new List<List<double>>();// Training data set,
            var output = new List<List<double>>();// Training data set,
            GetData(ref input, ref output, path);//заполняем data set'ы


            for (int i = 0; i < max_iters; i++)
            {//каждая итерация
                var errors = new List<List<List<double>>>();
                GetAllNeurons().ForEach(n => n.Null());//обновляем кэш нейронов
                var realOutput = input.Select(x => Run(x)).ToList();//запуск сети
                errors.Add(CalculateErrorOnLastLayer(realOutput, output));//считаем ошибку на последнем уровне
                if (i % 1000 == 0) Console.WriteLine($"Ошибка на {i} итерации: " + Mean(realOutput, output));
                var layer = start.End();
                while (layer.previous != null)
                {
                    errors.Add(CalculateErrorOnHideLayer(errors.Last(), layer));//считаем новую ошибку
                    layer = layer.previous;
                }
                layer = start.End();
                while (layer.previous != null)
                {
                    ChangeWeight(layer, alpha, errors.First());//меняем веса на каждом уровне отталкиваясь от ошибки на предыдущем
                    errors.RemoveAt(0);
                    layer = layer.previous;
                }
                ChangeWeight(layer, alpha, errors.Last(), input);//меняем веса на самом первом уровне
            }
        }

        private string GetTime(TimeSpan time)
        {
            return String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
                time.Hours,
                time.Minutes,
                time.Seconds,
                time.Milliseconds);
        }

        private List<List<double>> CalculateErrorOnLastLayer(List<List<double>> output, List<List<double>> y)
        {
            var res = new List<List<double>>();
            for(int i = 0; i < output.Count; i++)
            {
                var localRes = new List<double>();
                for(int j = 0; j < output[i].Count; j++)
                {
                    localRes.Add((y[i][j] - output[i][j]) * output[i][j] * (1 - output[i][j]));
                }
                res.Add(localRes);
            }
            return res;
        }

        private void ChangeWeight(Layer<MathNeuron> layer, double alpha, List<List<double>> layerError, List<List<double>> input = null)
        {
                                                                    //?
            var previousValue = input == null ? layer.previous.neurons.Select(x => x.Cash).ToList() : Transport(input);
            //previousValue = Transport(previousValue);
            var resMatrix = MyltiMatrix(previousValue, layerError);
            for(int i = 0; i < resMatrix.Count; i++)
            {
                for (int j = 0; j < resMatrix[i].Count; j++)//по строке
                {
                    layer.neurons[j].weights[i] += resMatrix[i][j];
                }
            }
        }

        private List<List<double>> CalculateErrorOnHideLayer(List<List<double>> previousLayerError, Layer<MathNeuron> previousLayer)
        {
            var res = new List<List<double>>();
            var weights = previousLayer.neurons.Select(x => x.weights).ToList();
            var resMatrix = MyltiMatrix(previousLayerError, weights);
            var layer = previousLayer.previous;
            for(int i = 0; i < resMatrix.Count; i++)
            {
                var localRes = new List<double>();
                for(int j = 0; j < resMatrix[i].Count; j++)
                {
                    double w = layer.neurons[j].Cash[i];
                    localRes.Add(resMatrix[i][j] * w * (1 - w));
                }
                res.Add(localRes);
            }
            return res;
        }

        private List<List<double>> Transport(List<List<double>> start)
        {
            var res = new List<List<double>>();
            for (int j = 0; j < start[0].Count; j++)
                res.Add(new List<double> { start[0][j] });
            for (int i = 1; i < start.Count; i++)
                for (int j = 0; j < start[i].Count; j++)
                    res[j].Add(start[i][j]);
            return res;
        }

        private List<List<double>> MyltiMatrix(List<List<double>> first, List<List<double>> second)
        {
            var res = new List<List<double>>();
            for(int i = 0; i < first.Count; i++)
            {
                res.Add(new List<double>());
                for(int j = 0; j < second.First().Count; j++)
                {
                    var sum = 0.0;
                    for(int x = 0; x < first[i].Count; x++)
                    {
                        sum += first[i][x] * second[x][j];
                    }
                    res[i].Add(sum);
                }
            }
            return res;
        }

        private double Mean(List<List<double>> first, List<List<double>> second)
        {
            var sum = 0.0;
            for(int i = 0; i < first.Count; i++)
            {
                for(int j = 0; j < first[i].Count; j++)
                {
                    sum += Math.Abs(first[i][j] - second[i][j]);
                }
            }
            return sum / (first.Count * first[0].Count);
        }
    }
}
