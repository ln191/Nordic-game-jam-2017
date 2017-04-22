using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    private Vector3 velocity;
    private Rigidbody rb;
    private Text UI;
    public int maxTimer;
    public int playerNumber;
    private int health;
    public float startPosX, startPosY, speed;
    private float timer;
    public bool automaticSpacing;
    private List<GameObject> players = new List<GameObject>();

    // Use this for initialization
    void Start()
    {

        health = 3;

        rb = GetComponent<Rigidbody>();

        if (automaticSpacing)
        {
            rb.position = new Vector3(startPosX * playerNumber, 1, startPosY);
        }

        else
        {
            rb.position = new Vector3(startPosX, 1, startPosY);
        }

        for (int i = 1; i < 5; i++)
        {
            players.Add(GameObject.FindGameObjectWithTag("Player" + i));
        }

        UI = GameObject.Find("UI Text").GetComponent<Text>();




    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (UI.text == "")
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].GetComponent<PlayerMovement>().UpdateText();
            }
        }

        //timer += Time.deltaTime;
        if (timer > maxTimer)
        {
            health--;
            timer = 0;
            UI.text = "";

            for (int i = 0; i < players.Count; i++)
            {
                players[i].GetComponent<PlayerMovement>().UpdateText();
            }

        }

        //as long as player has health, he can move
        if (health > 0)
        {
            velocity.x = Input.GetAxis("Horizontal" + playerNumber);
            velocity.z = Input.GetAxis("Vertical" + playerNumber);

            if (velocity.magnitude > 1)
            {
                velocity.Normalize();
            }

            rb.velocity = velocity * speed;
        }

        if (health <= 0)
        {
            rb.velocity = new Vector3(0, 0, 0);
        }

        Debug.Log(velocity);


    }

    /// <summary>
    /// Writes the players name and health out to the textbox
    /// </summary>
    public void UpdateText()
    {
        UI.text += ("Player " + playerNumber + " health: " + health + "\n");
    }
}