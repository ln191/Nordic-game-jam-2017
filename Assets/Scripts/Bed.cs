using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour
{
    public bool isOccupied;

    [SerializeField]
    private Vector3 currentPlacement;

    private bool isMoving = false;

    public Vector3 CurrentPlacement
    {
        get
        {
            return currentPlacement;
        }

        set
        {
            currentPlacement = value;
        }
    }

    // Use this for initialization
    private void Start()
    {
        isOccupied = false;
    }

    // Update is called once per frame
    private void Update()
    {
    }
}