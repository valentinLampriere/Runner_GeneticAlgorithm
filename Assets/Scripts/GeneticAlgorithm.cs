using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GeneticAlgorithm : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab = null;
    [SerializeField] private int individualsPerGen = 0;
    [SerializeField] private int individualsToSelect = 0;
    [SerializeField] private float mutateRate = 0f;

    private int playersLeft;
    private List<Individual> individuals;

    // Start is called before the first frame update
    void Start()
    {
        //float[] inputs = new float[] { 1.4f, 7.8f, -4.2f, -1f, 1};
        //NeuralNetwork net = new NeuralNetwork(5, 5, 2, true);

        //net.Feed(inputs);
        CreateGeneration();
    }

    void Update()
    {
        if(playersLeft <= 0)
        {
            CreateNextGen();
        }
    }

    void CreatePlayer(NeuralNetwork net)
    {
        GameObject player = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        Individual individual = player.GetComponent<Individual>();
        individual.Initialize(net);
        individuals.Add(individual);
    }

    void CreateGeneration()
    {
        individuals = new List<Individual>();

        for (int i = 0; i < individualsPerGen; i++)
        {
            NeuralNetwork net = new NeuralNetwork(5, 5, 2, true);
            CreatePlayer(net);
        }

        playersLeft = individuals.Count;
    }

    void CreateNextGen()
    {
        List<Individual> bestIndividuals = GetBestIndividuals();

        individuals = new List<Individual>();
        individuals.AddRange(bestIndividuals);

        for(int i = 0; i < individualsPerGen - individuals.Count; i++)
        {
            int i1 = Random.Range(0, bestIndividuals.Count);
            int i2;
            do { 
                i2 = Random.Range(0, bestIndividuals.Count);
            } while (i1 == i2);

            NeuralNetwork net = new NeuralNetwork(5, 5, 2);
            net.WeightsList = Crossover(bestIndividuals[i1], bestIndividuals[i2]);

            CreatePlayer(net);
        }

        playersLeft = individuals.Count;
    }

    List<Individual> GetBestIndividuals()
    {
        Assert.IsTrue(individuals.Count >= individualsToSelect);

        individuals.Sort((x, y) => y.Fitness.CompareTo(x.Fitness));

        return individuals.GetRange(0, individualsToSelect);
    }

    List<float[,]> Crossover(Individual parent1, Individual parent2)
    {
        List<float[,]> crossedWeightsList = new List<float[,]>();
        crossedWeightsList.Add(CrossoverWeights(parent1.NeuralNet.WeightsList[0], parent2.NeuralNet.WeightsList[0]));
        crossedWeightsList.Add(CrossoverWeights(parent1.NeuralNet.WeightsList[1], parent2.NeuralNet.WeightsList[1]));

        return crossedWeightsList;
    }

    float[,] CrossoverWeights(float[,] weights1, float[,] weights2)
    {
        float[,] crossedWeights = new float[weights1.GetLength(0), weights1.GetLength(1)];

        bool f = true;
        for (int i = 0; i < crossedWeights.Length; i++)
        {
            for (int j = 0; j < crossedWeights.Length; j++)
            {
                if(Random.Range(0f, 1f) <= mutateRate)
                {
                    crossedWeights[i, j] = weights1[i, j] * Random.Range(-2, 2);
                }
                else
                {
                    crossedWeights[i, j] = f ? weights1[i, j] : weights2[i, j];
                }
            }
            f = !f;
        }

        return crossedWeights;
    }
}
