using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Individual : MonoBehaviour
{
    public float Fitness { get; set; }
    public NeuralNetwork NeuralNet { get; set; }

    private PlayerController pc;

    public void Initialize(NeuralNetwork nt)
    {
        NeuralNet = nt;
        Fitness = 0;

        pc = GetComponent<PlayerController>();
    }

    void FixedUpdate()
    {
        float[] inputs = new float[] {pc.xMovement, pc.yMovement, pc.distanceWall, pc.distanceHole, 1f };

        float[] movements = NeuralNet.Feed(inputs);
        pc.xMovement = movements[0];
        pc.yMovement = movements[1];
    }
}
