using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOI7_2
{
    class Program
    {
        static void Main(string[] args)
        {
            // Исходные данные
            double[,] input = new double[,] { { 1, 1 }, { -1, -1 }, { 1, -1 }, { -1, 1 } };
            int[] output = new int[] { 0, 1, 1, 1 };
            double[,] weights = new double[,] { { 0.2, 0.8 }, { 0.6, 0.4 } };

            // Количество эпох обучения
            int epochs = 10;

            // Коэффициент обучения
            double learningRate = 0.1;

            for (int epoch = 0; epoch < epochs; epoch++)
            {
                for (int i = 0; i < input.GetLength(0); i++)
                {
                    // Вычисление ближайшего центра кластера
                    double minDistance = double.MaxValue;
                    int winningNeuron = 0;
                    for (int j = 0; j < weights.GetLength(0); j++)
                    {
                        double distance = 0;
                        for (int k = 0; k < weights.GetLength(1); k++)
                        {
                            distance += Math.Pow(input[i, k] - weights[j, k], 2);
                        }
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            winningNeuron = j;
                        }
                    }

                    // Обновление весов
                    for (int j = 0; j < weights.GetLength(0); j++)
                    {
                        for (int k = 0; k < weights.GetLength(1); k++)
                        {
                            weights[j, k] += learningRate * (input[i, k] - weights[j, k]);
                        }
                    }

                    // Обновление меток классов
                    output[winningNeuron] = output[winningNeuron] == 0 ? 1 : output[winningNeuron];
                }
            }

            // Тестирование обученной модели
            Console.WriteLine("Testing Kohonen Network");
            Console.WriteLine("Input\t\tPredicted Output");
            Console.WriteLine($"{input[0, 0]}, {input[0, 1]}\t\t{output[0]}");
            Console.WriteLine($"{input[1, 0]}, {input[1, 1]}\t\t{output[1]}");
            Console.WriteLine($"{input[2, 0]}, {input[2, 1]}\t\t{output[2]}");
            Console.WriteLine($"{input[3, 0]}, {input[3, 1]}\t\t{output[3]}");
            Console.ReadLine();
        }
    }
}
