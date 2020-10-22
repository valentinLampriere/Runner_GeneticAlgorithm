using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork
{
    private float[] inputs;
    private float[] hidden;
    private float[] outputs;
    private List<float[,]> weightsList;

    public NeuralNetwork(float[] inputs, int outSize, int hiddenSize)
    {
        this.inputs = inputs;
        hidden = new float[hiddenSize];
        outputs = new float[outSize];
        weightsList = new List<float[,]>();

        InitWeights();
    }

    float[,] InitLayerWeight(int sizeI, int sizeJ)
    {
        float[,] weights = new float[sizeI,sizeJ];

        for (int j = 0; j < sizeJ; j++)
        {
            for (int i = 0; i < sizeI; i++)
            {
                weights[i, j] = UnityEngine.Random.Range(0f, 1f);
            }
        }

        return weights;
    }

    public void InitWeights()
    {
        weightsList.Add(InitLayerWeight(inputs.Length, hidden.Length));
        weightsList.Add(InitLayerWeight(hidden.Length, outputs.Length));
    }

    public void Feed()
    {

    }

    public void DisplayWeights()
    {
        foreach (float[,] weights in weightsList)
        {
            string s = "";
            foreach (float w in weights)
            {
                s += (w + " ");
            }
            Debug.Log(s);
        }
    }
}
