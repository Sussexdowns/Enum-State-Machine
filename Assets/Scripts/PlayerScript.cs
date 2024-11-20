using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum States // used by all logic
{
    None,
    Idle,
    Walk,
    Jump,
};

public class PlayerScript : MonoBehaviour
{
    States state;


    Rigidbody rb;
    bool grounded;

    float moveSpeed = 5f;


    // Start is called before the first frame update
    void Start()
    {
        state = States.Idle;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        DoLogic();

        
    }

    void FixedUpdate()
    {
        grounded=false;
    }


    void DoLogic()
    {
        if( state == States.Idle )
        {
            PlayerIdle();
        }

        if( state == States.Jump )
        {
            PlayerJumping();
        }

        if( state == States.Walk )
        {
            PlayerWalk();
        }
    }


    void PlayerIdle()
    {
        if( Input.GetKeyDown(KeyCode.Space))
        {
            // simulate jump
            print("Jump!");
            state = States.Jump;
            rb.linearVelocity = new Vector3( 0,10,0);
        }

        if( Input.GetKey("left"))
        {
            print("turn left!");
            transform.Rotate( 0, 0.5f, 0, Space.Self );

        }
        if( Input.GetKey("right"))
        {
            print("turn right!");
            transform.Rotate( 0,-0.5f, 0, Space.Self );
        }

        if( Input.GetKey("up"))
        {
            print("walk!");
            state = States.Walk;
        }

    }

    void PlayerJumping()
    {
        // player is jumping, check for hitting the ground
        if( grounded == true )
        {
            //player has landed on floor
            state = States.Idle;
        }
    }

    void PlayerWalk()
    {

            // Set a constant forward velocity
            Vector3 forwardVelocity = transform.forward * moveSpeed;

            // Update the Rigidbody's velocity, preserving the Y component for gravity
            rb.linearVelocity = new Vector3(forwardVelocity.x, rb.linearVelocity.y, forwardVelocity.z);

            // Clamp the velocity magnitude (optional, depending on desired behavior)
            rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, moveSpeed);
    }
   


    void OnCollisionEnter( Collision col )
    {
        if( col.gameObject.tag == "Floor")
        {
            grounded=true;
            print("landed!");
        }
    }


    private void OnGUI()
    {
        //debug text
        string text = "Left/Right arrows = Rotate\nSpace = Jump\nUp Arrow = Forward\nCurrent state=" + state;

        // Create a GUIStyle for the text
        GUIStyle textStyle = new GUIStyle();
        textStyle.fontSize = 48; // Set font size
        textStyle.normal.textColor = Color.black; // Set text color to black

        // Define debug text area
        GUILayout.BeginArea(new Rect(10f, 10f, 1600f, 1600f));
        GUILayout.Label(text, textStyle);
        GUILayout.EndArea();
    }


}
