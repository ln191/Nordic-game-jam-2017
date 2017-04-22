using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuffle : MonoBehaviour
{
    public GameObject bed;

    [Range(1f, 10f)]
    public float spaceBetweenBed = 1;

    [SerializeField]
    private GameObject[,] bedplacements;

    // private List<GameObject> beds = new List<GameObject>();

    public int bedWidth = 3;

    public int bedHeight = 4;

    // Use this for initialization
    private void Start()
    {
        bedplacements = new GameObject[3, 4];
        Setupbeds(bedWidth, bedHeight);
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            bedplacements = clockShuffle();
        }

        for (int z = 0; z < bedHeight; z++)
        {
            for (int x = 0; x < bedWidth; x++)
            {
                bedplacements[z, x].transform.position = new Vector3(Mathf.Lerp(bedplacements[z, x].transform.position.x, x * -spaceBetweenBed, 0.05f), 0.5f, Mathf.Lerp(bedplacements[z, x].transform.position.z, -z * spaceBetweenBed, 0.05f));
            }
        }
    }

    public void Shufflebed()
    {
    }

    private GameObject[,] clockShuffle()
    {
        GameObject[,] bedsPosTemp = new GameObject[bedHeight, bedWidth];

        int[,] clockPos = new int[,]
        {
            { 4, 0, 1, 2},
            { 8, 6, 5, 3 },
            { 9, 10, 11, 7}
        };

        int index = 0;
        for (int z = 0; z < bedHeight; z++)
        {
            for (int x = 0; x < bedWidth; x++)
            {
                for (int z1 = 0; z1 < bedHeight; z1++)
                {
                    for (int x1 = 0; x1 < bedWidth; x1++)
                    {
                        if (index == clockPos[z1, x1])
                        {
                            bedsPosTemp[z, x] = bedplacements[z1, x1];
                        }
                    }
                }

                index++;
            }
        }
        return bedsPosTemp;
    }

    private void BedMovement()
    {
    }

    /// <summary>
    /// Setup the beds in the scene depended on the given values
    /// </summary>
    /// <param name="numOfBedWidth"></param>
    /// <param name="numOfBedHight"></param>
    public void Setupbeds(int numOfBedWidth, int numOfBedHight)
    {
        for (int x = 0; x < numOfBedWidth; x++)
        {
            for (int z = 0; z < numOfBedHight; z++)
            {
                GameObject obj = Instantiate(bed, new Vector3(x * spaceBetweenBed, 0.5f, z * spaceBetweenBed), Quaternion.identity);
                obj.GetComponent<Bed>().CurrentPlacement = new Vector3(x * spaceBetweenBed, 0.5f, z * spaceBetweenBed);

                bedplacements[z, x] = obj;
            }
        }
    }
}