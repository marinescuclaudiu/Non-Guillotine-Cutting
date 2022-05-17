using System;
using System.Collections.Generic;
using System.Text;

namespace Non_Guillotine_Cutting
{
    class GeneticAlgorithm
    {
        private Individual bestSolution;

        private int nThChild;

        private List<Individual> individuals;

        public GeneticAlgorithm()
        {
            bestSolution = new Individual();
            nThChild = 0;
            individuals = new List<Individual>();
        }

        private void GeneratePopulation()
        {
            System.IO.StreamReader streamReader = System.IO.File.OpenText("..\\..\\..\\input.txt");
            string stringValues = streamReader.ReadToEnd();
            streamReader.Close();

            string[] stringValuesSplitted = stringValues.Split(',');

            List<double> doubleValues = new List<double>();

            foreach (string value in stringValuesSplitted)
            {
                doubleValues.Add(double.Parse(value));
            }

            int stockRectangleLength = (int)doubleValues[0];
            int stockRectangleWidth = (int)doubleValues[1];

            List<Tuple<int, int, int, int, int>> piecesInfo = new List<Tuple<int, int, int, int, int>>();
            int type = 1;

            for (int indexValue = 2; indexValue < doubleValues.Count; indexValue += 4)
            {
                int noOfPieces = (int)doubleValues[indexValue];
                int value = (int)doubleValues[indexValue + 1];
                int length = (int)doubleValues[indexValue + 2];
                int width = (int)doubleValues[indexValue + 3];

                Tuple<int, int, int, int, int> pieceInfo = new Tuple<int, int, int, int, int>(noOfPieces, type, value, length, width);
                piecesInfo.Add(pieceInfo);

                type++;
            }

            Console.Write("Population size: ");
            int populationSize = Convert.ToInt32(Console.ReadLine());

            for (int index = 0; index < populationSize; index++)
            {
                Individual individual = new Individual(piecesInfo, stockRectangleLength, stockRectangleWidth);
                individuals.Add(individual);
            }
        }

        private void EvolvePopulation()
        {
            Random random = new Random();

            int indexParent1 = random.Next(0, individuals.Count);
            int indexParent2 = random.Next(0, individuals.Count);


            while (indexParent2 == indexParent1)
            {
                indexParent2 = random.Next(0, individuals.Count);
            }

            Individual child = new Individual(individuals[indexParent1], individuals[indexParent2]);

            nThChild++;

            if (nThChild % 10 == 0)
            {
                child.Mutation();
            }

            bool canReplace = true;

            child.SortPieces();

            for (int index = 0; index < individuals.Count; index++)
            {
                individuals[index].SortPieces();
                if (child.IsEqualTo(individuals[index]) == true)
                {
                    canReplace = false;
                    break;
                }
            }

            if (canReplace)
            {
                List<int> indexesGroup1 = new List<int>();
                List<int> indexesGroup2 = new List<int>();
                List<int> indexesGroup3 = new List<int>();
                List<int> indexesGroup4 = new List<int>();

                for (int index = 0; index < individuals.Count; index++)
                {
                    //group 1
                    if (child.Fitness > individuals[index].Fitness && child.Unfitness < individuals[index].Unfitness)
                    {
                        indexesGroup1.Add(index);
                        continue;
                    }

                    //group 4
                    if (child.Fitness < individuals[index].Fitness && child.Unfitness > individuals[index].Unfitness)
                    {
                        indexesGroup4.Add(index);
                        continue;
                    }

                    //group 2
                    if (child.Unfitness < individuals[index].Unfitness)
                    {
                        indexesGroup2.Add(index);
                        continue;
                    }

                    //group 3
                    indexesGroup3.Add(index);

                }

                int indexIndividualToDelete;

                if (indexesGroup1.Count > 0)
                {
                    int indexGroup = random.Next(0, indexesGroup1.Count);
                    indexIndividualToDelete = indexesGroup1[indexGroup];
                }
                else if (indexesGroup2.Count > 0)
                {
                    int indexGroup = random.Next(0, indexesGroup2.Count);
                    indexIndividualToDelete = indexesGroup2[indexGroup];
                }
                else if (indexesGroup3.Count > 0)
                {
                    int indexGroup = random.Next(0, indexesGroup3.Count);
                    indexIndividualToDelete = indexesGroup3[indexGroup];
                }
                else
                {
                    int indexGroup = random.Next(0, indexesGroup4.Count);
                    indexIndividualToDelete = indexesGroup4[indexGroup];
                }

                individuals.RemoveAt(indexIndividualToDelete);

                individuals.Add(child);
            }

            SaveBestSolution();
        }

        private void SaveBestSolution()
        {
            foreach (Individual individual in individuals)
            {
                if (individual.Unfitness == 0)
                {
                    if (individual.Fitness > bestSolution.Fitness)
                    {
                        bestSolution.Copy(individual);
                    }

                }
            }
        }

        public void Start()
        {
            GeneratePopulation();

            while (nThChild != 5000)
            {
                EvolvePopulation();
            }

            Console.WriteLine("The best solution found: ");
            bestSolution.PrintInfo();
        }
    }
}
