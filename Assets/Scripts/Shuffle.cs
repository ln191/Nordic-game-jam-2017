using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuffle : MonoBehaviour
{
    public GameObject bed;

    [Range(1f, 10f)]
    public float spaceBetweenBed = 1;

    public int bedWidth = 5;
    public int bedHight = 5;

    // Use this for initialization
    private void Start()
    {
        Setupbeds(bedWidth, bedHight);
    }

    // Update is called once per frame
    private void Update()
    {
    }

    public void Shufflebed()
    {
    }

    private void BedMovement()
    {
    }

    public void Setupbeds(int numOfBedWidth, int numOfBedHight)
    {
        for (int z = 0; z < numOfBedWidth; z++)
        {
            for (int x = 0; x < numOfBedHight; x++)
            {
                Instantiate(bed, new Vector3(x * spaceBetweenBed, 0.5f, z * spaceBetweenBed), Quaternion.identity);
            }
        }
    }
}