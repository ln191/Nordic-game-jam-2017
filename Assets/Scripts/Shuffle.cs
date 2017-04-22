using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuffle : MonoBehaviour
{
    public enum State
    {
        PreMonster, Shuffle, PostMonster, Play, Win
    }

    public State currentState;

    public GameObject bed;

    [Range(1f, 10f)]
    public float spaceBetweenBed = 1;

    [SerializeField]
    private GameObject[,] bedplacements;

    private bool isMoving = false;
    private bool startTimer = false;

    public bool IsMoving
    {
        get
        {
            return isMoving;
        }

        set
        {
            isMoving = value;
        }
    }

    public bool StartTimer
    {
        get
        {
            return startTimer;
        }

        set
        {
            startTimer = value;
        }
    }

    public GameObject[,] Bedplacements
    {
        get
        {
            return bedplacements;
        }

        set
        {
            bedplacements = value;
        }
    }

    [Range(0.5f, 5f)]
    public float timeBetweenShuffles = 0.5f;

    public float timeleft = 0.5f;
    private int level = 3;
    private int ind = 0;
    // private List<GameObject> beds = new List<GameObject>();

    public int bedWidth = 3;

    public int bedHeight = 4;

    #region shuffle-matrix

    private int[,] clockPos = new int[,]
       {
            { 4, 0, 1, 2},
            { 8, 5, 6, 3 },
            { 9, 10, 11, 7}
       };

    private int[,] midclock = new int[,]
       {
            { 0, 1, 2, 3},
            { 5, 4, 7, 6 },
            { 8, 9, 10, 11}
       };

    private int[,] countClock = new int[,]
       {
            { 1, 2, 3, 7},
            { 0, 6, 5, 11 },
            { 4, 8, 9, 10}
       };

    private int[,] crossClock = new int[,]
       {
            { 4, 2, 1, 7},
            { 8, 0, 3, 11 },
            { 9, 6, 5, 10}
       };

    private int[,] aaa = new int[,]
       {
            { 1, 0, 3, 2},
            { 5, 4, 7, 6 },
            { 9, 8, 11, 10}
       };

    private int[,] bbb = new int[,]
       {
            { 1, 0, 2, 6},
            { 5, 4, 3, 7 },
            { 8, 10, 9, 11}
       };

    private int[,] doublecountClock = new int[,]
       {
            { 4, 0, 3, 7},
            { 8, 1, 2, 11 },
            { 9, 5, 6, 10}
       };

    #endregion shuffle-matrix

    // Use this for initialization
    private void Start()
    {
        //bedplacements = new GameObject[3, 4];
        // Setupbeds(bedWidth, bedHeight);
    }

    // Update is called once per frame
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    startTimer = true;
        //    isMoving = true;
        //}
    }

    private void level1()
    {
        if (ind == 0)
        {
            bedplacements = ShuffleBed(clockPos);
        }
        else if (ind == 1)
        {
            bedplacements = ShuffleBed(midclock);
        }
        else if (ind == 2)
        {
            bedplacements = ShuffleBed(countClock);
        }
        else if (ind == 3)
        {
            bedplacements = ShuffleBed(crossClock);
        }
        else if (ind == 4)
        {
            bedplacements = ShuffleBed(countClock);
        }
        else
        {
            isMoving = false;
        }
    }

    private void level2()
    {
        if (ind == 0)
        {
            bedplacements = ShuffleBed(aaa);
        }
        else if (ind == 1)
        {
            bedplacements = ShuffleBed(midclock);
        }
        else if (ind == 2)
        {
            bedplacements = ShuffleBed(doublecountClock);
        }
        else if (ind == 3)
        {
            bedplacements = ShuffleBed(countClock);
        }
        else if (ind == 4)
        {
            bedplacements = ShuffleBed(crossClock);
        }
        else
        {
            isMoving = false;
        }
    }

    private void level3()
    {
        if (ind == 0)
        {
            bedplacements = ShuffleBed(clockPos);
        }
        else if (ind == 1)
        {
            bedplacements = ShuffleBed(bbb);
        }
        else if (ind == 2)
        {
            bedplacements = ShuffleBed(doublecountClock);
        }
        else if (ind == 3)
        {
            bedplacements = ShuffleBed(crossClock);
        }
        else if (ind == 4)
        {
            bedplacements = ShuffleBed(crossClock);
        }
        else
        {
            isMoving = false;
        }
    }

    public void ShuffleState()
    {
        if (startTimer)
        {
            timeleft -= Time.deltaTime;
        }

        if (timeleft < 0 && isMoving)
        {
            switch (level)
            {
                case 1:
                    level1();
                    break;

                case 2:
                    level2();
                    break;

                case 3:
                    level3();
                    break;

                default:
                    level1();
                    break;
            }

            if (!isMoving)
            {
                ind = 0;
            }
            else
            {
                ind++;
            }

            timeleft = timeBetweenShuffles;
        }
        else if (startTimer && timeleft < 0 && !isMoving)
        {
            gameObject.GetComponent<GameManager>().currentState = GameManager.State.PostMonster;
            startTimer = false;
        }
        BedMovement();
    }

    /// <summary>
    /// Shuffle the beds depended on the given shufflematrix
    /// </summary>
    /// <param name="shuffleMatrix"></param>
    /// <returns></returns>
    private GameObject[,] ShuffleBed(int[,] shuffleMatrix)
    {
        GameObject[,] bedsPosTemp = new GameObject[bedHeight, bedWidth];

        //int[,] clockPos = new int[,]
        //{
        //    { 4, 0, 1, 2},
        //    { 8, 6, 5, 3 },
        //    { 9, 10, 11, 7}
        //};

        int index = 0;
        for (int z = 0; z < bedHeight; z++)
        {
            for (int x = 0; x < bedWidth; x++)
            {
                for (int z1 = 0; z1 < bedHeight; z1++)
                {
                    for (int x1 = 0; x1 < bedWidth; x1++)
                    {
                        if (index == shuffleMatrix[z1, x1])
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

    /// <summary>
    /// moves the bed to the shuffle position
    /// </summary>
    private void BedMovement()
    {
        for (int z = 0; z < bedHeight; z++)
        {
            for (int x = 0; x < bedWidth; x++)
            {
                bedplacements[z, x].transform.position = new Vector3(Mathf.Lerp(bedplacements[z, x].transform.position.x, x * spaceBetweenBed-12, 0.05f), 0.5f, Mathf.Lerp(bedplacements[z, x].transform.position.z, z * spaceBetweenBed-8, 0.05f));
            }
        }
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
                GameObject obj = Instantiate(bed, new Vector3(x * spaceBetweenBed-12, 0.5f, z * spaceBetweenBed-8), Quaternion.Euler(new Vector3(0,270,0)));

                bedplacements[z, x] = obj;
            }
        }
    }
}