using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject wallPrefab;
    public float speed = 10f;

    public Wall wall { get; set; }
    private GameObject gw;

    void Awake()
    {
        CreateWall();
    }

    public void CreateWall()
    {
        // Instantiate a wall 
        gw = Instantiate(wallPrefab, new Vector3(0, 0, 10), Quaternion.identity);
        wall = gw.GetComponent<Wall>();
        wall.speed = speed;
    }

    public void DestroyWall()
    {
        Destroy(gw);
    }
}
