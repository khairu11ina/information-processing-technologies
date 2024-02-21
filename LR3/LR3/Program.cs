using System;

class AntColonyOptimization
{
    private int numberOfCities;
    private int numberOfAnts;
    private int numberOfIterations;
    private double alpha;
    private double beta;
    private double evaporationRate;
    private double[][] distances;
    private double[][] pheromoneLevels;
    private int[] bestTour;
    private double bestLength;

    public AntColonyOptimization(int numberOfCities, int numberOfAnts, int numberOfIterations, double alpha, double beta, double evaporationRate, double[][] distances)
    {
        this.numberOfCities = numberOfCities;
        this.numberOfAnts = numberOfAnts;
        this.numberOfIterations = numberOfIterations;
        this.alpha = alpha;
        this.beta = beta;
        this.evaporationRate = evaporationRate;
        this.distances = distances;
        pheromoneLevels = new double[numberOfCities][];
        for (int i = 0; i < numberOfCities; i++)
        {
            pheromoneLevels[i] = new double[numberOfCities];
            for (int j = 0; j < numberOfCities; j++)
            {
                pheromoneLevels[i][j] = 1.0 / (numberOfCities * distances[i][j]);
            }
        }
        bestTour = new int[numberOfCities];
        Random random = new Random();
        for (int i = 0; i < bestTour.Length; i++)
        {
            bestTour[i] = i;
        }
        Shuffle(bestTour, random);
        bestLength = GetTourLength(bestTour);
    }

    public void Solve()
    {
        for (int iter = 0; iter < numberOfIterations; iter++)
        {
            int[][] antTours = new int[numberOfAnts][];
            for (int k = 0; k < numberOfAnts; k++)
            {
                antTours[k] = BuildAntTour(k);
            }
            UpdatePheromoneLevels(antTours);
            int[] newBestTour = GetBestTour(antTours);
            double newBestLength = GetTourLength(newBestTour);
            if (newBestLength < bestLength)
            {
                bestTour = newBestTour;
                bestLength = newBestLength;
            }
        }
    }

    private int[] BuildAntTour(int k)
    {
        int startingCity = (new Random()).Next(numberOfCities);
        int[] tour = new int[numberOfCities];
        bool[] visited = new bool[numberOfCities];
        tour[0] = startingCity;
        visited[startingCity] = true;
        for (int i = 1; i < numberOfCities; i++)
        {
            int cityFrom = tour[i - 1];
            int nextCity = ChooseNextCity(visited, cityFrom, k);
            tour[i] = nextCity;
            visited[nextCity] = true;
        }
        return tour;
    }

    private int ChooseNextCity(bool[] visited, int cityFrom, int k)
    {
        double[] probabilities = CalculateProbabilities(visited, cityFrom, k);
        double[] cumulativeProbabilities = new double[probabilities.Length];
        cumulativeProbabilities[0] = probabilities[0];
        for (int i = 1; i < probabilities.Length; i++)
        {
            cumulativeProbabilities[i] = cumulativeProbabilities[i - 1] + probabilities[i];
        }
        double randomValue = (new Random()).NextDouble() * cumulativeProbabilities[cumulativeProbabilities.Length - 1];
        for (int i = 0; i < cumulativeProbabilities.Length; i++)
        {
            if (cumulativeProbabilities[i] > randomValue)
            {
                return i;
            }
        }
        return -1;
    }

    private double[] CalculateProbabilities(bool[] visited, int cityFrom, int k)
    {
        double[] probabilities = new double[numberOfCities];
        double totalScore = 0.0;
        for (int i = 0; i < numberOfCities; i++)
        {
            if (!visited[i])
            {
                double score = Math.Pow(pheromoneLevels[cityFrom][i], alpha) * Math.Pow(1.0 / distances[cityFrom][i], beta);
                probabilities[i] = score;
                totalScore += score;
            }
        }
        for (int i = 0; i < numberOfCities; i++)
        {
            probabilities[i] /= totalScore;
        }
        return probabilities;
    }

    private void UpdatePheromoneLevels(int[][] antTours)
    {
        for (int i = 0; i < numberOfCities; i++)
        {
            for (int j = i + 1; j < numberOfCities; j++)
            {
                double pheromone = 0.0;
                for (int k = 0; k < numberOfAnts; k++)
                {
                    int[] tour = antTours[k];
                    int x = -1;
                    for (int l = 0; l < tour.Length - 1; l++)
                    {
                        if (tour[l] == i && tour[l + 1] == j || tour[l] == j && tour[l + 1] == i)
                        {
                            x = l;
                            break;
                        }
                    }
                    if (x != -1)
                    {
                        pheromone += 1.0 / distances[i][j];
                    }
                }
                pheromoneLevels[i][j] = (1 - evaporationRate) * pheromoneLevels[i][j] + evaporationRate * pheromone;
                pheromoneLevels[j][i] = pheromoneLevels[i][j];
            }
        }
    }

    private int[] GetBestTour(int[][] antTours)
    {
        int bestTourIndex = 0;
        double bestTourLength = GetTourLength(antTours[0]);
        for (int i = 1; i < antTours.Length; i++)
        {
            double currentTourLength = GetTourLength(antTours[i]);
            if (currentTourLength < bestTourLength)
            {
                bestTourLength = currentTourLength;
                bestTourIndex = i;
            }
        }
        return antTours[bestTourIndex];
    }

    private double GetTourLength(int[] tour)
    {
        double length = 0.0;
        for (int i = 0; i < tour.Length - 1; i++)
        {
            length += distances[tour[i]][tour[i + 1]];
        }
        length += distances[tour[tour.Length - 1]][tour[0]];
        return length;
    }

    private void Shuffle(int[] array, Random random)
    {
        for (int i = 0; i < array.Length - 1; i++)
        {
            int j = random.Next(i, array.Length);
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
        }
    }
    public int[] GetBestTour()
    {
        return bestTour;
    }

    public double GetBestLength()
    {
        return bestLength;
    }
}

class MainClass
{
    public static void Main(string[] args)
    {
        int numberOfCities = 5;
        int numberOfAnts = 10;
        int numberOfIterations = 100;
        double alpha = 1.0;
        double beta = 5.0;
        double evaporationRate = 0.5;
        double[][] distances = {
            new double[] { 0, 2, 7, 12, 5 },
            new double[] { 2, 0, 5, 4, 2 },
            new double[] { 7, 5, 0, 3, 6 },
            new double[] { 12, 4, 3, 0, 8 },
            new double[] { 5, 2, 6, 8, 0 }
        };
        AntColonyOptimization aco = new AntColonyOptimization(numberOfCities, numberOfAnts, numberOfIterations, alpha, beta, evaporationRate, distances);
        aco.Solve();
        int[] bestTour = aco.GetBestTour();
        double bestLength = aco.GetBestLength();
        Console.WriteLine("Best tour: " + String.Join(",", bestTour));
        Console.WriteLine("Best tour length: " + bestLength);

        Console.ReadLine();
    }
}
