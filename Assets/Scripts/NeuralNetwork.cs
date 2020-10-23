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

        for (int i = 0; i < sizeI; i++)
        {
            for (int j = 0; j < sizeJ; j++)
            {
                weights[i, j] = UnityEngine.Random.Range(0f, 1f);
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
                Debug.Log(weights.GetLength(1) + " " + cells.Length);
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
        weightsList.Add(InitLayerWeight(hidden.Length - 1, inputs.Length));
        weightsList.Add(InitLayerWeight(outputs.Length, hidden.Length));
    }

    public void Feed()
    {
        hidden = ApplyActivationFunction(MultiplyListMatrix(inputs, weightsList[0]));
        for (int i = 0; i < hidden.Length; i++)
        {
            Debug.Log(hidden[i]);
        }
        outputs = ApplyActivationFunction(MultiplyListMatrix(hidden, weightsList[1]));
    }

    public void DisplayWeights()
    {
        foreach (float[,] weights in weightsList)
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
