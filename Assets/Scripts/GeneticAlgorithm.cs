using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GeneticAlgorithm : MonoBehaviour
{
    [SerializeField] private GameManager manager = null;
    [SerializeField] GameObject playerPrefab = null;
    [SerializeField] private int individualsPerGen = 0;
    [SerializeField] private int individualsToSelect = 0;
    [SerializeField] private float mutateRate = 0f;

    [Header("Neural Net Settings")]
    [SerializeField] private int inputSize = 0;
    [SerializeField] private int hiddenSize = 0;
    [SerializeField] private int outsideSize = 0;

    public int PlayersLeft;// { get; set; }
    private int activeIndividuals;
    private List<Individual> individuals;
    private int generations;

    // Start is called before the first frame update
    void Start()
    {
        generations = 1;
        CreateGeneration();
    }

    void Update()
    {
        if (manager.wall.transform.position.z <= 0) {
            //List<Individual> newIndividuals = new List<Individual>();
            for (int i = 0; i < individuals.Count; i++) {
                if (!individuals[i].isDisable) {
                    if (individuals[i].pc.IsInHole()) {
                        individuals[i].pc.wallPassed++;
                    } else {
                        individuals[i].isDisable = true;
                        individuals[i].gameObject.SetActive(false);
                        PlayersLeft--;
                    }
                }
                /*if (individuals[i].pc.IsInHole()) {
                    //newIndividuals.Add(individuals[i]);
                } else {
                    individuals[i].isDisable = true;
                    individuals[i].gameObject.SetActive(false);
                    PlayersLeft--;
                    //Destroy(individuals[i].gameObject);
                }*/
            }
            //individuals = newIndividuals;

            manager.DestroyWall();
            manager.CreateWall();


            if (PlayersLeft <= 0) {
                Debug.Log("----- GENERATION : " + generations);
                CreateNextGen();
                generations++;
            }
        }
    }

    void CreatePlayer(NeuralNetwork net)
    {
        GameObject player = Instantiate(playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        PlayerController pc = player.GetComponent<PlayerController>();
        Individual individual = player.GetComponent<Individual>();
        pc.GA = this;
        pc.manager = manager;
        individual.Initialize(net);
        individuals.Add(individual);
    }

    void CreateGeneration()
    {
        individuals = new List<Individual>();

        for (int i = 0; i < individualsPerGen; i++)
        {
            NeuralNetwork net = new NeuralNetwork(inputSize, hiddenSize, outsideSize, true);
            CreatePlayer(net);
        }

        PlayersLeft = individuals.Count;
    }

    void CreateNextGen()
    {
        List<Individual> bestIndividuals = GetBestIndividuals();

        DestroyIndividualsObjects();
        individuals = new List<Individual>();

        for (int i = 0; i < bestIndividuals.Count; i++)
        {
            NeuralNetwork bNet = bestIndividuals[i].NeuralNet;
            CreatePlayer(bNet);
        }
        
        for (int i = 0; i < individualsPerGen - bestIndividuals.Count; i++)
        {
            int i1 = Random.Range(0, bestIndividuals.Count);
            int i2;
            do
            {
                i2 = Random.Range(0, bestIndividuals.Count);
            } while (i1 == i2);

            NeuralNetwork net = new NeuralNetwork(inputSize, hiddenSize, outsideSize);
            net.WeightsList = Crossover(bestIndividuals[i1], bestIndividuals[i2]);

            CreatePlayer(net);
        }

        PlayersLeft = individuals.Count;
    }

    List<Individual> GetBestIndividuals()
    {
        Assert.IsTrue(individuals.Count >= individualsToSelect);

        individuals.Sort((x, y) => y.Fitness.CompareTo(x.Fitness));

        Debug.Log(individuals[0].Fitness);
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
        for (int i = 0; i < crossedWeights.GetLength(0); i++)
        {
            for (int j = 0; j < crossedWeights.GetLength(1); j++)
            {
                if(Random.Range(0f, 1f) <= mutateRate)
                {
                    crossedWeights[i, j] = weights1[i, j] + (weights2[i,j] * Random.Range(-0.05f, 0.05f));
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

    void DestroyIndividualsObjects()
    {
        for (int i = 0; i < individuals.Count; i++)
        {
            Destroy(individuals[i].gameObject);
        }
    }
}
