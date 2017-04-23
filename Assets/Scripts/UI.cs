﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{


    // Use this for initialization
    void Start()
    {
        for (int i = 1; i < GameObject.Find("MenuScript").GetComponent<MenuScript>().Players + 1; i++)
        {
            GetComponent<Text>().text += (GameObject.FindGameObjectWithTag("Player" + i).GetComponent<PlayerMovement>().color) + " health: " +
 (GameObject.FindGameObjectWithTag("Player" + i).GetComponent<PlayerMovement>().Health) + "\n";
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
