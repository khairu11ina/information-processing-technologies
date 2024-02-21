using System;

class SimulatedAnnealing
{
    static Random rng = new Random();

    static double GetEnergy(double x)
    {
        // Задаём функцию, чей минимум мы ищем с помощью алгоритма отжига
        return x * x;
    }

    static double GetTemperature(int iteration)
    {
        // В этой функции задаём скорость, с которой температура будет уменьшаться
        return 1.0 / iteration;
    }

    static double GetNeighbor(double x, double range)
    {
        // Получаем случайного соседа x из окрестности заданного радиуса
        return x + range * (rng.NextDouble() * 2.0 - 1.0);
    }

    static bool ShouldAccept(double energy, double newEnergy, double temp)
    {
        // Функция принимает решение, нужно ли принимать новое решение на основе разницы энергий и температуры
        if (newEnergy < energy)
        {
            return true;
        }
        return (rng.NextDouble() < Math.Exp((energy - newEnergy) / temp));
    }

    static double SimulatedAnnealingMinimize(double start, double range, int iterations)
    {
        double state = start;
        double energy = GetEnergy(state);
        for (int i = 0; i < iterations; i++)
        {
            double temperature = GetTemperature(i);
            double nextState = GetNeighbor(state, range);
            double nextEnergy = GetEnergy(nextState);
            if (ShouldAccept(energy, nextEnergy, temperature))
            {
                state = nextState;
                energy = nextEnergy;
            }
        }
        return state;
    }

    static void Main()
    {
        // Точность в пределах которой определяется успех работы алгоритма
        double epsilon = 0.1;
        // Начальное значение и ширина окрестности
        double start = 10.0;
        double range = 5.0;
        // Максимальное количество итераций
        int iterations = 5000;

        double result = SimulatedAnnealingMinimize(start, range, iterations);
        // Выводим результат
        Console.WriteLine("ECorrect solution: 0.0");
        Console.WriteLine("Found solution: " + result);
        Console.WriteLine("Difference: " + Math.Abs(0.0 - result));
        // Проверяем, насколько близко мы пришли к идеальному решению
        if (Math.Abs(0.0 - result) < epsilon)
        {
            Console.WriteLine("Success! We found the correct result.");
        }
        else
        {
            Console.WriteLine("Failure. We did not find the correct result.");
        }
        Console.ReadLine();
    }
}
