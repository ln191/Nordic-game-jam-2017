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

    private bool inBed, keyDown, keyTest;

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
        if (Input.GetAxis("Fire" + playerNumber) == 0)
        {
            keyDown = false;
        }


        if (UI.text == "")
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].GetComponent<PlayerMovement>().UpdateText();
                //
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
        if (health > 0 && !inBed)
        {
            //    velocity = new Vector3(Input.GetAxisRaw("Horizontal" + playerNumber), 0, Input.GetAxisRaw("Vertical" + playerNumber));
            //    //velocity.x = Input.GetAxis("Horizontal" + playerNumber);
            //    //velocity.z = Input.GetAxis("Vertical" + playerNumber);

            //    if (velocity.magnitude > 1)
            //    {
            //        velocity.Normalize();
            //    }

            //    rb.velocity = velocity * speed;

            velocity = new Vector3(Input.GetAxisRaw("Horizontal" + playerNumber), 0, Input.GetAxisRaw("Vertical" + playerNumber));
            
            if (rb.velocity.magnitude < 10)
            {
                rb.AddForce(velocity * speed);
            }

            if (velocity.x != 0 || velocity.z != 0)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity), 0.25f);
            }
        }

        if (health <= 0 || inBed)
        {
            rb.velocity = new Vector3(0, 0, 0);
        }
    }

    /// <summary>
    /// Writes the players name and health out to the textbox
    /// </summary>
    public void UpdateText()
    {
        UI.text += ("Player " + playerNumber + " health: " + health + "\n");
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Bed")
        {
            if (!keyDown && !other.GetComponent<Bed>().isOccupied && Input.GetAxis("Fire" + playerNumber) == 1)
            {
                inBed = true;
                transform.position = (new Vector3(other.gameObject.transform.position.x, 0.7f, other.gameObject.transform.position.z));
                transform.rotation = Quaternion.Euler(new Vector3(0, other.transform.rotation.y + 90, 0));
                other.GetComponent<Bed>().isOccupied = true;
                keyDown = true;
               
            }

            //Debug.Log("Bed test");   

            else if (!keyDown && inBed && Input.GetAxis("Fire" + playerNumber) == 1)
            {
                other.GetComponent<Bed>().isOccupied = false;
                inBed = false;
                transform.position = (new Vector3(other.gameObject.transform.position.x - 2, 0.7f, other.gameObject.transform.position.z));
                transform.rotation = Quaternion.Euler(new Vector3(0, other.transform.rotation.y, 0));
                keyDown = true;
            }
        }


    }
}