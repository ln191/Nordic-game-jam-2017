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
    private int playerNumber;
    public int health;
    public float speed;
    private float timer;
    public bool automaticSpacing;
    private List<GameObject> players = new List<GameObject>();
    private bool allowMovement = false;
    private Vector3 startPos;
    private GameObject bed;
    private AudioSource audio;
    private Animator animator;
    public string color;

    [SerializeField]
    public AudioClip Scream;
    public AudioClip Sleep1;
    public AudioClip Sleep2;
    //public AudioClip Sleep3;

    private bool inBed, keyDown, keyTest;

    public bool InBed
    {
        get
        {
            return inBed;
        }

        set
        {
            inBed = value;
        }
    }

    public int Health
    {
        get
        {
            return health;
        }

        set
        {
            health = value;
        }
    }

    public GameObject Bed
    {
        get
        {
            return bed;
        }

        set
        {
            bed = value;
        }
    }

    public Vector3 StartPos
    {
        get
        {
            return startPos;
        }

        set
        {
            startPos = value;
        }
    }

    public int PlayerNumber
    {
        get
        {
            return playerNumber;
        }

        set
        {
            playerNumber = value;
        }
    }

    public bool AllowMovement
    {
        get
        {
            return allowMovement;
        }

        set
        {
            allowMovement = value;
        }
    }

    // Use this for initialization
    void Start()
    {

        audio = GetComponent<AudioSource>();



        animator = GetComponentInChildren<Animator>();
        animator.SetBool("walking", false);


        rb = GetComponent<Rigidbody>();

        startPos = transform.position;
        //rb.position = new Vector3(startPosX * playerNumber, 1, startPosY);



        //for (int i = 1; i < GameObject.Find("MenuScript").GetComponent<MenuScript>().Players + 1; i++)
        //{
        //    players.Add(GameObject.FindGameObjectWithTag("Player" + i));
        //}

        //UI = GameObject.Find("UI Text").GetComponent<Text>();




    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetAxisRaw("Fire" + playerNumber) == 0)
        {
            keyDown = false;
        }


        //if (UI.text == "")
        //{
        //    for (int i = 0; i < players.Count; i++)
        //    {
        //        players[i].GetComponent<PlayerMovement>().UpdateText();
        //        //
        //    }
        //}

        //timer += Time.deltaTime;


        //as long as player has health, he can move
        if (health > 0 && !inBed && allowMovement)
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
            if (velocity.magnitude > 0.01f)
            {
                animator.SetBool("walking",true);
            }
            else
            {
                animator.SetBool("walking", false);
            }
            if (rb.velocity.magnitude < 10)
            {
                rb.AddForce(velocity * speed);
            }

            if (velocity.x != 0 || velocity.z != 0)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity), 0.25f);
            }
        }
        else { animator.SetBool("walking", true); }

        if (health <= 0 || inBed)
        {
            rb.velocity = new Vector3(0, 0, 0);
        }

        if (inBed)
        {
            animator.SetBool("walking", false);
        }

        if (!allowMovement)
        {
            animator.SetBool("walking", false);
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
            if (!InBed && !keyDown && !other.GetComponent<Bed>().isOccupied && Input.GetAxis("Fire" + playerNumber) == 1)
            {
                if (allowMovement)
                {
                    if (Random.Range(1, 10f) < 5)
                    {
                        audio.clip = Sleep1;
                    }
                    else
                    {
                        audio.clip = Sleep2;
                    }

                    audio.volume = (0.5f);
                    audio.Play();

                    inBed = true;

                    animator.SetBool("walking", false);
                    transform.position = (new Vector3(other.gameObject.transform.position.x, 1, other.gameObject.transform.position.z));
                    transform.rotation = Quaternion.Euler(new Vector3(90, other.transform.rotation.y + 90, 90));

                }
                other.GetComponent<Bed>().isOccupied = true;
                keyDown = true;
                bed = other.gameObject;


            }

            //Debug.Log("Bed test");   

            else if (!keyDown && inBed && Input.GetAxis("Fire" + playerNumber) == 1 && allowMovement)
            {
                other.GetComponent<Bed>().isOccupied = false;
                inBed = false;
                transform.position = (new Vector3(other.gameObject.transform.position.x, 1f, other.gameObject.transform.position.z - 2));
                transform.rotation = Quaternion.Euler(new Vector3(0, other.transform.rotation.y, 0));
                keyDown = true;
            }
        }

    }



    public void Reset()
    {
        transform.position = startPos;
        gameObject.SetActive(true);
        inBed = false;
        allowMovement = false;
        animator.SetBool("walking", false);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
    }
}