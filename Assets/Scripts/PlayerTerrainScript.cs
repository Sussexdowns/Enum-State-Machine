using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatesPlayer // used by all logic
{
    None,
    Idle,
    Walk,
    Jump,
};

public class PlayerTerrainScript : MonoBehaviour
{
    StatesPlayer state;

    Rigidbody rb;
    bool grounded;

    float moveSpeed = 10f;
    float rotationSpeed = 5f; // Rotation speed for smooth turning

    Coroutine slowingCoroutine;
    Coroutine rotationCoroutine; // Coroutine for smooth rotation

    float currentRotationSpeed = 0f; // Current rotational speed

    // Start is called before the first frame update
    void Start()
    {
        state = StatesPlayer.Idle;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        DoLogic();
    }

    void FixedUpdate()
    {
        grounded = false;
    }

    void DoLogic()
    {
        if (state == StatesPlayer.Idle)
        {
            PlayerIdle();
        }

        if (state == StatesPlayer.Jump)
        {
            PlayerJumping();
        }

        if (state == StatesPlayer.Walk)
        {
            PlayerWalk();
        }
    }

    void PlayerIdle()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // simulate jump
            print("Jump!");
            state = StatesPlayer.Jump;
            rb.linearVelocity = new Vector3(0, 10, 0);
        }

        if (Input.GetKey("left"))
        {
            StartTurning(-rotationSpeed); // Turn left
        }
        else if (Input.GetKey("right"))
        {
            StartTurning(rotationSpeed); // Turn right
        }
        else
        {
            StopTurning(); // Gradually stop rotating
        }

        if (Input.GetKey("up"))
        {
            print("walk!");
            state = StatesPlayer.Walk;
        }
    }

    void PlayerJumping()
    {
        // player is jumping, check for hitting the ground
        if (grounded == true)
        {
            //player has landed on floor
            state = StatesPlayer.Idle;
        }
    }

    void PlayerWalk()
    {
        // Set a constant forward velocity
        Vector3 forwardVelocity = transform.forward * moveSpeed;

        // Update the Rigidbody's velocity, preserving the Y component for gravity
        rb.linearVelocity = new Vector3(forwardVelocity.x, rb.linearVelocity.y, forwardVelocity.z);

        // Start slowing down when the key is released
        if (Input.GetKeyUp("up"))
        {
            // Start coroutine to slow down the player
            slowingCoroutine = StartCoroutine(SlowDownOverTime());
        }
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Floor")
        {
            grounded = true;
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

    IEnumerator SlowDownOverTime()
    {
        float initialSpeed = rb.linearVelocity.magnitude;
        float elapsedTime = 0f;

        while (elapsedTime < 1f) // Over 1 second
        {
            float t = elapsedTime / 1f; // Normalize time

            float newSpeed = Mathf.Lerp(initialSpeed, 0f, t); // Linearly interpolate speed to 0
            rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, moveSpeed);

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }

        // Ensure the player fully stops
        rb.linearVelocity = Vector3.zero;

        state = StatesPlayer.Idle;
        print("Idle!");
    }

    void StartTurning(float targetRotationSpeed)
    {
        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine); // Stop any existing rotation coroutine
        }

        currentRotationSpeed = targetRotationSpeed;
        rotationCoroutine = StartCoroutine(RotateGradually());
    }

    void StopTurning()
    {
        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine); // Stop any existing rotation coroutine
        }

        // Start slowing down the rotation speed gradually
        rotationCoroutine = StartCoroutine(SlowDownRotation());
    }

    IEnumerator RotateGradually()
    {
        while (currentRotationSpeed != 0)
        {
            transform.Rotate(0, currentRotationSpeed * Time.deltaTime, 0, Space.Self);
            yield return null;
        }
    }

    IEnumerator SlowDownRotation()
    {
        float initialRotationSpeed = currentRotationSpeed;

        while (Mathf.Abs(currentRotationSpeed) > 0.1f)
        {
            currentRotationSpeed = Mathf.Lerp(initialRotationSpeed, 0f, Time.deltaTime * 2f); // Smooth stop
            transform.Rotate(0, currentRotationSpeed * Time.deltaTime, 0, Space.Self);
            yield return null;
        }

        currentRotationSpeed = 0f;
    }
}
