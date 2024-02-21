using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOI7
{
    class Program
    {
        static void Main(string[] args)
        {
            // Исходные данные
            double[,] input = new double[,] { { 1, 1 }, { -1, -1 }, { 1, -1 }, { -1, 1 } };
            int[] output = new int[] { 1, -1, -1, -1 };
            double[] weights = new double[] { 0, 0 };

            // Количество эпох обучения
            int epochs = 10;

            // Коэффициент обучения
            double learningRate = 0.1;

            for (int epoch = 0; epoch < epochs; epoch++)
            {
                for (int i = 0; i < input.GetLength(0); i++)
                {
                    double activation = weights[0] * input[i, 0] + weights[1] * input[i, 1];
                    int predictedOutput = activation >= 0 ? 1 : -1;
                    double error = output[i] - predictedOutput;
                    weights[0] += learningRate * error * input[i, 0];
                    weights[1] += learningRate * error * input[i, 1];
                }
            }

            // Тестирование обученной модели
            Console.WriteLine("Testing Hebbian Learning Algorithm");
            Console.WriteLine("Input\t\tPredicted Output");
            Console.WriteLine($"{input[0, 0]}, {input[0, 1]}\t\t{Math.Sign(weights[0] * input[0, 0] + weights[1] * input[0, 1])}");
            Console.WriteLine($"{input[1, 0]}, {input[1, 1]}\t\t{Math.Sign(weights[0] * input[1, 0] + weights[1] * input[1, 1])}");
            Console.WriteLine($"{input[2, 0]}, {input[2, 1]}\t\t{Math.Sign(weights[0] * input[2, 0] + weights[1] * input[2, 1])}");
            Console.WriteLine($"{input[3, 0]}, {input[3, 1]}\t\t{Math.Sign(weights[0] * input[3, 0] + weights[1] * input[3, 1])}");
            Console.ReadLine();
        }
    }
}
