using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Individual : MonoBehaviour
{
    public float Fitness { get; set; }
    public NeuralNetwork NeuralNet { get; set; }

    public void Initialize(NeuralNetwork nt)
    {
        NeuralNet = nt;
        Fitness = 0;
    }

    void FixedUpdate()
    {
       NeuralNet.Feed(new float[5]);
    }
}
