using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NeuralNetwork
{
    private float[] inputs;
    private float[] hidden;
    private float[] outputs;
    public List<float[,]> WeightsList { get; set; }

    public NeuralNetwork(int inSize, int hiddenSize, int outSize, bool shouldInit = false)
    {
        inputs = new float[inSize];
        hidden = new float[hiddenSize];
        outputs = new float[outSize];
        WeightsList = new List<float[,]>();

        if(shouldInit)
        {
            InitWeights();
        }
    }

    float[,] InitLayerWeight(int sizeI, int sizeJ)
    {
        float[,] weights = new float[sizeI,sizeJ];

        for (int i = 0; i < sizeI; i++)
        {
            for (int j = 0; j < sizeJ; j++)
            {
                weights[i, j] = UnityEngine.Random.Range(-100f, 100f);
            }
        }

        return weights;
    }

    float[] MultiplyListMatrix(float[] cells, float[,] weights)
    {
        float[,] multWeights = new float[weights.GetLength(0), weights.GetLength(1)];
        float[] resCells = new float[weights.GetLength(0)];

        for (int j = 0; j < weights.GetLength(1); j++)
        {
            for (int i = 0; i < weights.GetLength(0); i++)
            {
                multWeights[i, j] = weights[i, j] * cells[j];
            }
        }

        for (int i = 0; i < multWeights.GetLength(0); i++)
        {
            resCells[i] = 0;
            for (int j = 0; j < multWeights.GetLength(1); j++)
            {
                resCells[i] += multWeights[i, j];
            }
        }

        return resCells;
    }

    float[] ApplyActivationFunction(float[] fedCells)
    {
        float[] cellsValues = fedCells;

        for (int i = 0; i < cellsValues.Length; i++)
        {
            cellsValues[i] = (float)Math.Tanh(fedCells[i]);
        }

        return cellsValues;
    }

    public void InitWeights()
    {
        WeightsList.Add(InitLayerWeight(hidden.Length, inputs.Length));
        WeightsList.Add(InitLayerWeight(outputs.Length, hidden.Length));
    }

    public float[] Feed(float[] _inputs)
    {
        inputs = _inputs;

        hidden = ApplyActivationFunction(MultiplyListMatrix(inputs, WeightsList[0]));
        hidden[hidden.Length - 1] = 1;

        outputs = ApplyActivationFunction(MultiplyListMatrix(hidden, WeightsList[1]));

        return outputs;
    }

    public void DisplayWeights()
    {
        foreach (float[,] weights in WeightsList)
        {
            for (int i = 0; i < weights.GetLength(0); i++)
            {
                string s = "";
                for (int j = 0; j < weights.GetLength(1); j++)
                {
                    s += (weights[i, j] + " ");
                }
                Debug.Log(s);
                Debug.Log("----");
            }
        }
    }

}
