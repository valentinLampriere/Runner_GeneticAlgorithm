using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Wall : MonoBehaviour {

    public GameObject wallTile;

    public float sizeScene = 10;
    public int nbCellsWall = 16;
    public PlayerController player;

    public int indexHole;
    public Dictionary<int, Vector2> cellsPosition;

    Color color;
    void Start() {
        int freeX = Random.Range(0, nbCellsWall);
        int freeY = Random.Range(0, nbCellsWall);


        cellsPosition = new Dictionary<int, Vector2>();

        indexHole = freeX + freeY * nbCellsWall;

        for (int i = 0; i < nbCellsWall; i++) {
            for (int j = 0; j< nbCellsWall; j++) {
                int index = i + j * nbCellsWall;

                float x = (sizeScene / nbCellsWall) * i;
                float y = (sizeScene / nbCellsWall) * j;

                Vector3 cellPos = new Vector3(x - sizeScene / 2, y - sizeScene / 2, transform.position.z);

                cellsPosition.Add(index, cellPos);

                if (i == freeX && j == freeY)
                    continue;

                GameObject cell = Instantiate(wallTile, cellPos, Quaternion.identity, transform);
                cell.name = (i + j * nbCellsWall).ToString();
                cell.transform.localScale = new Vector3(sizeScene / nbCellsWall, sizeScene / nbCellsWall, cell.transform.localScale.z);

            }
        }
    }

    void Update() {
        transform.position = transform.position + Vector3.back * Time.deltaTime * player.speed;
    }
}
