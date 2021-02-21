using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 1.0f;
    [SerializeField] private Transform spriteTransform = null;
    [SerializeField] protected Animator animator = null;

    private Rigidbody2D rb2d;        //Store a reference to the Rigidbody2D component required to use 2D Physics.

    // Use this for initialization
    void Start()
    {
        //Get and store a reference to the Rigidbody2D component so that we can access it.
        rb2d = GetComponent<Rigidbody2D> ();
    }

    //FixedUpdate is called at a fixed interval and is independent of frame rate. Put physics code here.
    void FixedUpdate()
    {
        //Store the current horizontal input in the float moveHorizontal.
        float moveHorizontal = Input.GetAxis ("Horizontal");

        // rotate sprite based on direction
        if(moveHorizontal < 0)
        {
            spriteTransform.eulerAngles = new Vector3(spriteTransform.eulerAngles.x, 0, spriteTransform.eulerAngles.z);
        }
        else if(moveHorizontal > 0)
        {
            spriteTransform.eulerAngles = new Vector3(spriteTransform.eulerAngles.x, 180, spriteTransform.eulerAngles.z);
        }

        //Store the current vertical input in the float moveVertical.
        float moveVertical = Input.GetAxis ("Vertical");

        // Set animation Moving & idling
        float x = Mathf.Pow( Mathf.Abs(moveHorizontal), 2 );
        float y = Mathf.Pow( Mathf.Abs(moveVertical), 2 );
        float speed = Mathf.Sqrt( x + y );
        animator.SetFloat("Speed", speed);

        //Use the two store floats to create a new Vector2 variable movement.
        Vector2 movement = new Vector2 (moveHorizontal, moveVertical);

        //Call the MovePosition function of our Rigidbody2D rb2d supplying movement multiplied by speed to 
        //move our player.
        rb2d.MovePosition (rb2d.position + movement * playerSpeed * Time.deltaTime);
    }
}
