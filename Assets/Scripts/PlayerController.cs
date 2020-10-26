using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float xMovement;
    public float yMovement;
    public float distanceWall { get; private set; }
    public float distanceHole { get; private set; }
    public float distanceHoleX { get; private set; }
    public float distanceHoleY { get; private set; }

    private float boundXmin;
    private float boundXmax;
    private float boundYmin;
    private float boundYmax;
    private Individual individual;

    public GameObject Wall;
    public Wall NextWall { get; set; }
    public GeneticAlgorithm GA { get; set; }

    void Start()
    {
        individual = GetComponent<Individual>();

        boundXmin = (-NextWall.sizeScene / 2) - NextWall.sizeCell / 2;
        boundXmax = (NextWall.sizeScene / 2) + NextWall.sizeCell / 2;
        boundYmin = (-NextWall.sizeScene / 2) - NextWall.sizeCell / 2;
        boundYmax = (NextWall.sizeScene / 2) + NextWall.sizeCell / 2;
    }

    private void Update() {
        Vector3 playerPos = transform.position + Vector3.forward * transform.localScale.z;
        Vector3 wallPos = new Vector3(transform.position.x, transform.position.y, NextWall.transform.position.z) + Vector3.back * NextWall.transform.GetChild(0).transform.localScale.z / 2;

        Vector3 freeCellPos = NextWall.cellsPosition[NextWall.indexHole];

        transform.position = transform.position + transform.right * Time.deltaTime * xMovement * NextWall.speed;
        transform.position = transform.position + transform.up * Time.deltaTime * yMovement * NextWall.speed;

        if (playerPos.x < boundXmin) {
            transform.position = new Vector3(boundXmax, playerPos.y);
        } else if (playerPos.x > boundXmax) {
            transform.position = new Vector3(boundXmin, playerPos.y);
        }
        if (playerPos.y < boundYmin) {
            transform.position = new Vector3(playerPos.x, (NextWall.sizeScene / 2) + NextWall.sizeCell / 2);
        } else if (playerPos.y > boundYmax) {
            transform.position = new Vector3(playerPos.x, (-NextWall.sizeScene / 2) - NextWall.sizeCell / 2);
        }

        distanceHoleX = Mathf.Abs(playerPos.x - freeCellPos.x);
        distanceHoleY = Mathf.Abs(playerPos.y - freeCellPos.y);
        distanceHole = Mathf.Sqrt((distanceHoleX * distanceHoleX) + (distanceHoleY * distanceHoleY));
        distanceWall = Mathf.Abs(playerPos.z - wallPos.z);

        individual.Fitness = 1f / distanceHole;
    }
}
