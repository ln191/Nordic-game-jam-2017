using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour {
    private Vector3 velocity;
    public int playerNumber;
    public float speed;
    private string horizontalInput, verticalInput;
    private Rigidbody rb;

	// Use this for initialization
	void Start ()
    {
        rb = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {

        velocity.x = Input.GetAxis("Horizontal" + playerNumber);
        velocity.y = Input.GetAxis("Vertical" + playerNumber);

        if (velocity.magnitude > 1)
        {
            velocity.Normalize();
        }
        

        rb.velocity = velocity * Time.deltaTime * speed;

        //rb.AddForce(velocity * Time.deltaTime * speed);

        Debug.Log(velocity);
    }





}