using System;

class GeneticAlgorithm
{
    static Random rng = new Random();

    static double GetFitness(double x)
    {
        // Задаём функцию расчёта приспособленности, которую мы будем оптимизировать генетическим алгоритмом
        return x * Math.Sin(x);
    }

    static double[] InitializePopulation(int size, double min, double max)
    {
        // Инициализируем популяцию случайными значениями в заданном диапазоне
        double[] population = new double[size];
        for (int i = 0; i < population.Length; i++)
        {
            population[i] = rng.NextDouble() * (max - min) + min;
        }
        return population;
    }

    static double[] SelectParents(double[] population)
    {
        // Выбираем двух родителей случайным образом
        double[] parents = new double[2];
        parents[0] = population[rng.Next(population.Length)];
        parents[1] = population[rng.Next(population.Length)];
        return parents;
    }

    static double[] Crossover(double[] parents)
    {
        // Осуществляем скрещивание путём взятия среднего значения генов родителей
        return new double[] { (parents[0] + parents[1]) / 2 };
    }

    static double Mutation(double x, double mutationRate, double min, double max)
    {
        // Выполняем мутацию путём замены одного гена случайным числом с заданной вероятностью
        if (rng.NextDouble() < mutationRate)
        {
            return rng.NextDouble() * (max - min) + min;
        }
        return x;
    }

    static double[] NextGeneration(double[] population, double mutationRate, double min, double max)
    {
        // Задаём новую популяцию, осуществляя отбор, скрещивание и мутацию
        double[] newPopulation = new double[population.Length];
        for (int i = 0; i < population.Length; i++)
        {
            double[] parents = SelectParents(population);
            double[] offspring = Crossover(parents);
            double mutatedOffspring = Mutation(offspring[0], mutationRate, min, max);
            newPopulation[i] = mutatedOffspring;
        }
        return newPopulation;
    }

    static double GeneticAlgorithmMinimize(double min, double max, int populationSize, double mutationRate, int numGenerations)
    {
        double[] population = InitializePopulation(populationSize, min, max);
        double bestFitness = double.MinValue;
        double bestIndividual = double.MinValue;
        for (int i = 0; i < numGenerations; i++)
        {
            population = NextGeneration(population, mutationRate, min, max);
            for (int j = 0; j < population.Length; j++)
            {
                double fitness = GetFitness(population[j]);
                if (fitness > bestFitness)
                {
                    bestFitness = fitness;
                    bestIndividual = population[j];
                }
            }
        }
        return bestIndividual;
    }

    static void Main()
    {
        // Задаём параметры для алгоритма
        double min = -10.0;
        double max = 10.0;
        int populationSize = 100;
        double mutationRate = 0.01;
        int numGenerations = 500;

        // Запускаем алгоритм и выводим результат
        double result = GeneticAlgorithmMinimize(min, max, populationSize, mutationRate, numGenerations);
        Console.WriteLine("Exact solution: 4.49341");
        Console.WriteLine("Found solution: " + result);
        Console.WriteLine("Difference: " + Math.Abs(4.49341 - result));
        Console.ReadLine();
    }
}
