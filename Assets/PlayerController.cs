using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [Range(-1f, 1f)]
    public float xMovement;
    [Range(-1f, 1f)]
    public float yMovement;
    private float distanceWall;
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

        // If the player reach the wall
        if (playerPos.z > wallPos.z) {
            Vector3 freeCellPos = nextWall.cellsPosition[nextWall.indexHole];
            // If the player is in the free cell
            if ((freeCellPos - transform.position).sqrMagnitude < (nextWall.transform.GetChild(0).localScale -transform.localScale).sqrMagnitude) {
                Destroy(nextWall.gameObject);
                GameObject g = Instantiate(Wall, new Vector3(0, 0, 10), Quaternion.identity);
                nextWall = g.GetComponent<Wall>();
                nextWall.player = this;
            } else
                Debug.Break();
        }

        transform.position = transform.position + transform.right * Time.deltaTime * xMovement;
        transform.position = transform.position + transform.up * Time.deltaTime * yMovement;

        distanceWall = (playerPos - wallPos).magnitude;
    }
}
