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
    public int wallPassed { get; set; }

    private float boundXmin;
    private float boundXmax;
    private float boundYmin;
    private float boundYmax;
    private Individual individual;

    public GeneticAlgorithm GA { get; set; }
    public GameManager manager { get; set; }

    void Start()
    {
        individual = GetComponent<Individual>();


        boundXmin = (-manager.wall.sizeScene / 2) - manager.wall.sizeCell / 2;
        boundXmax = (manager.wall.sizeScene / 2) + manager.wall.sizeCell / 2;
        boundYmin = (-manager.wall.sizeScene / 2) - manager.wall.sizeCell / 2;
        boundYmax = (manager.wall.sizeScene / 2) + manager.wall.sizeCell / 2;
    }

    public bool IsInHole() {
        float cellSize = manager.wall.sizeCell;
        Vector3 holePos = manager.wall.freeCellPosition;
        Vector3 pPos = transform.position;
        if (pPos.x > holePos.x - cellSize / 2 && pPos.x < holePos.x + cellSize / 2 &&
            pPos.y > holePos.y - cellSize / 2 && pPos.y < holePos.y + cellSize / 2) {
            return true;
        }
        return false;
        //return distanceHole < manager.wall.sizeCell;
    }

    private void Update() {
        Vector3 playerPos = transform.position + Vector3.forward * transform.localScale.z;
        Vector3 wallPos = new Vector3(transform.position.x, transform.position.y, manager.wall.transform.position.z) + Vector3.back * manager.wall.sizeCell / 2;


        transform.position = transform.position + transform.right * Time.deltaTime * xMovement * manager.wall.speed * 1.5f;
        transform.position = transform.position + transform.up * Time.deltaTime * yMovement * manager.wall.speed * 1.5f;

        if (playerPos.x < boundXmin) {
            transform.position = new Vector3(boundXmax, playerPos.y);
        } else if (playerPos.x > boundXmax) {
            transform.position = new Vector3(boundXmin, playerPos.y);
        }
        if (playerPos.y < boundYmin) {
            transform.position = new Vector3(playerPos.x, (manager.wall.sizeScene / 2) + manager.wall.sizeCell / 2);
        } else if (playerPos.y > boundYmax) {
            transform.position = new Vector3(playerPos.x, (-manager.wall.sizeScene / 2) - manager.wall.sizeCell / 2);
        }

        distanceHoleX = Mathf.Abs(playerPos.x - manager.wall.freeCellPosition.x);
        distanceHoleY = Mathf.Abs(playerPos.y - manager.wall.freeCellPosition.y);
        distanceHole = Mathf.Sqrt((distanceHoleX * distanceHoleX) + (distanceHoleY * distanceHoleY));
        distanceWall = Mathf.Abs(playerPos.z - wallPos.z);

        if (!individual.isDisable) {
            individual.Fitness = (1f / distanceHole) + wallPassed * wallPassed;
        }
    }
}
