using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneticAlgorithm : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       // float[] inputs = new float[] { 0.3f, 2.4f, 2f, -1f, 1 };
        float[] inputs = new float[] { 2f, 10f, 100f, -1f, 1 };
        NeuralNetwork net = new NeuralNetwork(inputs, 2, inputs.Length);

        net.Feed();
        net.DisplayWeights();
    }


}
