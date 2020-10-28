using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Individual : MonoBehaviour {
    public float Fitness;// { get; set; }
    public NeuralNetwork NeuralNet { get; set; }

    public PlayerController pc { get; private set; }

    public bool isDisable {get; set; }

    public void Initialize(NeuralNetwork nt)
    {
        NeuralNet = nt;
        Fitness = 0;

        pc = GetComponent<PlayerController>();
    }

    void Update()
    {
        if (isDisable)
            return;

        float[] inputs = new float[] { 
            pc.xMovement, pc.yMovement, 
            pc.distanceWall, pc.distanceHoleX, pc.distanceHoleY,
            1f};

        float[] movements = NeuralNet.Feed(inputs);
        pc.xMovement = movements[0];
        pc.yMovement = movements[1];
    }
}
