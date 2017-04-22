using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Direction { Up, Right, Down, Left }

public class MonsterMovement : MonoBehaviour
{
    //Get this from world (beds)
    private int offsetX, offsetZ;
    private GameObject[,] testBeds;
    public float speed;
    private GameObject targetBed;
    private int destinationX = 2, destinationZ = 2;
    public int amountOfWaypoints;
    private int amountOfBedsX = 3, amountOfBedsZ = 4;
    private bool isBedFound = false;
    int axis;

    private Vector3 destination;
    private int index = 0;
    private Direction entryDir;
    Vector3[,] wayPoints;
    private Vector3 lastPos;


    // Use this for initialization
    void Start()
    {
        testBeds = GameObject.Find("GameStuff").GetComponent<Shuffle>().Bedplacements;


        if (amountOfBedsX > 1)
        {
            offsetX = (int)((testBeds[1, 0].transform.position - testBeds[0, 0].transform.position).magnitude / 2);
        }
        if (amountOfBedsZ > 1)
        {
            offsetZ = (int)((testBeds[0, 1].transform.position - testBeds[0, 0].transform.position).magnitude / 2);
        }


        //array of waypoints AROUND/DIAGONAL from the beds
        wayPoints = new Vector3[amountOfBedsX + 1, amountOfBedsZ + 1];
        for (int z = 0; z < amountOfBedsZ; z++)
        {
            for (int x = 0; x < amountOfBedsX; x++)
            {

                if (z == 0)
                {
                    if (x == 0)
                    {
                        //top left
                        wayPoints[x, z] = new Vector3(testBeds[x, z].transform.position.x - offsetX, 0, testBeds[x, z].transform.position.z + offsetZ);

                    }

                    //top right
                    wayPoints[x + 1, z] = new Vector3(testBeds[x, z].transform.position.x + offsetX, 0, testBeds[x, z].transform.position.z + offsetZ);

                }
                if (x == 0)
                {
                    //lower left
                    wayPoints[x, z + 1] = new Vector3(testBeds[x, z].transform.position.x - offsetX, 0, testBeds[x, z].transform.position.z - offsetZ);
                }

                wayPoints[x + 1, z + 1] = new Vector3(testBeds[x, z].transform.position.x + offsetX, 0, testBeds[x, z].transform.position.z - offsetZ);

            }
        }
        ChooseTargetBed();

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (!isBedFound)
            iTween.MoveUpdate(gameObject, wayPoints[destinationX, destinationZ], speed);

        if (isBedFound)
        {
            iTween.MoveUpdate(gameObject, targetBed.transform.position, speed);
        }

        if (transform.position.x <= wayPoints[destinationX, destinationZ].x + 0.2 && transform.position.x >= wayPoints[destinationX, destinationZ].x - 0.2 && transform.position.z <= wayPoints[destinationX, destinationZ].z + 0.2 && transform.position.z >= wayPoints[destinationX, destinationZ].z - 0.2 && amountOfWaypoints > 0)
        {
            ChooseNextWaypoint();
        }

        if (amountOfWaypoints == 0 && !isBedFound)
        {
            List<GameObject> bedsInRange = new List<GameObject>();
            foreach (GameObject bed in testBeds)
            {
                if ((bed.transform.position - transform.position).magnitude < 15 && !bed.GetComponent<Bed>().isMonsterOccupied)
                {
                    bedsInRange.Add(bed);
                    
                }
            }
            if (bedsInRange.Count != 0)
            {
                targetBed = bedsInRange[Random.Range(0, bedsInRange.Count)];
                targetBed.GetComponent<Bed>().isMonsterOccupied = true;
                GameObject.Find("GameStuff").GetComponent<GameManager>().monsterPlaced++;
                isBedFound = true;
            }
            else
                amountOfWaypoints += 3;
        }


    }

    public void ChooseNextWaypoint()
    {

        int destXTemp, destZTemp;
        do
        {
            //chose axis, x = 1, z = -1
            axis = 0;
            int distance = 0;
            while (axis == 0)
            {
                axis = Random.Range(-1, 2);
            }

            while (distance == 0)
            {
                distance = Random.Range(-1, 2);
            }

            destXTemp = destinationX;
            destZTemp = destinationZ;
            if (axis == 1)
            {
                destXTemp += distance;
            }
            else
            {
                destZTemp += distance;
            }

        } while (destXTemp < 0 || destZTemp < 0 || destXTemp >= wayPoints.GetLength(0) || destZTemp >= wayPoints.GetLength(1));

        destinationX = destXTemp;
        destinationZ = destZTemp;
        amountOfWaypoints--;



    }

    public void ChooseTargetBed()
    {
        //targetBed = testBeds[Random.Range(0, amountOfBedsX - 1), Random.Range(0, amountOfBedsZ)];
        //entryDir = (Direction)Random.Range(0, 3);
    }
}
