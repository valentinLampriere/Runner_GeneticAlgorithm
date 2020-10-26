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
    private float distanceHoleX;
    private float distanceHoleY;
    private float velocityX;
    private float velocityY;
    private Individual individual;

    public GameObject Wall;
    public Wall NextWall { get; set; }
    public GeneticAlgorithm GA { get; set; }

    void Start()
    {
        individual = GetComponent<Individual>();
    }

    private void Update() {
        Vector3 playerPos = transform.position + Vector3.forward * transform.localScale.z;
        Vector3 wallPos = new Vector3(transform.position.x, transform.position.y, NextWall.transform.position.z) + Vector3.back * NextWall.transform.GetChild(0).transform.localScale.z / 2;

        Vector3 freeCellPos = NextWall.cellsPosition[NextWall.indexHole];

        // If the player reach the wall
        //if (playerPos.z > wallPos.z) {
        //    // If the player is in the free cell
        //    if ((freeCellPos - transform.position).sqrMagnitude < (NextWall.transform.GetChild(0).localScale - transform.localScale).sqrMagnitude) {
        //        //Destroy(NextWall.gameObject);
        //        //nextWall = InstanciateWall();
        //    }
        //}

        transform.position = transform.position + transform.right * Time.deltaTime * xMovement * 10f;
        transform.position = transform.position + transform.up * Time.deltaTime * yMovement * 10f;

        if (playerPos.x < (-NextWall.sizeScene / 2) - NextWall.sizeCell / 2)
        {
             transform.position = new Vector3((-NextWall.sizeScene / 2) - NextWall.sizeCell / 2, playerPos.y);
        }
        else if (playerPos.x > (NextWall.sizeScene / 2) - NextWall.sizeCell)
        {
            transform.position = new Vector3((NextWall.sizeScene / 2) - NextWall.sizeCell / 2, playerPos.y);
        }
        if (playerPos.y < (-NextWall.sizeScene / 2) - NextWall.sizeCell / 2)
        {
            transform.position = new Vector3(playerPos.x, (-NextWall.sizeScene / 2) - NextWall.sizeCell / 2);
        }
        else if (playerPos.y > (NextWall.sizeScene / 2) - NextWall.sizeCell / 2)
        {
            transform.position = new Vector3(playerPos.x, (NextWall.sizeScene / 2) - NextWall.sizeCell / 2);
        }

        distanceHoleX = Mathf.Abs(playerPos.x - freeCellPos.x);
        distanceHoleY = Mathf.Abs(playerPos.y - freeCellPos.y);
        distanceHole = Mathf.Sqrt((distanceHoleX * distanceHoleX) + (distanceHoleY * distanceHoleY));
        distanceWall = (playerPos - wallPos).magnitude;

        individual.Fitness = 1f / distanceHole;
    }

    /*public Wall InstanciateWall() {
        GameObject g = Instantiate(Wall, new Vector3(0, 0, 10), Quaternion.identity);
        Wall w = g.GetComponent<Wall>();
        return w;
    }*/
}
