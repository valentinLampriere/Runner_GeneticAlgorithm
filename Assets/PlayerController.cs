using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float xMovement;
    public float yMovement;
    private float distanceWall;
    private float distanceHole;
    private float distanceHoleX;
    private float distanceHoleY;
    private float velocityX;
    private float velocityY;

    public float speed = 1.5f;

    public GameObject Wall;
    public Wall nextWall;

    private void Update() {
        Vector3 playerPos = transform.position + Vector3.forward * transform.localScale.z;
        Vector3 wallPos = new Vector3(transform.position.x, transform.position.y, nextWall.transform.position.z) + Vector3.back * nextWall.transform.GetChild(0).transform.localScale.z / 2;

        Vector3 freeCellPos = nextWall.cellsPosition[nextWall.indexHole];

        // If the player reach the wall
        if (playerPos.z > wallPos.z) {
            // If the player is in the free cell
            if ((freeCellPos - transform.position).sqrMagnitude < (nextWall.transform.GetChild(0).localScale - transform.localScale).sqrMagnitude) {
                Destroy(nextWall.gameObject);
                nextWall = InstanciateWall();
            } else {
                Debug.Break();
            }
        }

        transform.position = transform.position + transform.right * Time.deltaTime * xMovement;
        transform.position = transform.position + transform.up * Time.deltaTime * yMovement;

        if (playerPos.x < -nextWall.sizeScene / 2)
            transform.position = new Vector3(-nextWall.sizeScene / 2, playerPos.y);
        else if (playerPos.x > nextWall.sizeScene / 2)
            transform.position = new Vector3(nextWall.sizeScene / 2, playerPos.y);
        if (playerPos.y < -nextWall.sizeScene / 2)
            transform.position = new Vector3(playerPos.x, -nextWall.sizeScene / 2);
        else if (playerPos.y > nextWall.sizeScene / 2)
            transform.position = new Vector3(playerPos.x, nextWall.sizeScene / 2);

        distanceHoleX = Mathf.Abs(playerPos.x - freeCellPos.x);
        distanceHoleY = Mathf.Abs(playerPos.y - freeCellPos.y);
        distanceHole = (playerPos - freeCellPos).magnitude;
        distanceWall = (playerPos - wallPos).magnitude;
    }

    private Wall InstanciateWall() {
        GameObject g = Instantiate(Wall, new Vector3(0, 0, 10), Quaternion.identity);
        Wall w = g.GetComponent<Wall>();
        w.player = this;
        return w;
    }
}
