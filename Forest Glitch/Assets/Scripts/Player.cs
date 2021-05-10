using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Attributes")]
    public float runSpeed = 25f; 
    public float moveSpeed;
    private float targetMoveSpeed; 
    public float jumpForce = 5f;
    public bool isGrounded = false;
    public enum State
    {
        Idle        = 0,
        Crouching   = 1,
        CrabWalking = 2,
        Sneaking    = 3,
        Running     = 4
    }
    public State playerState;
    /// <summary>
    /// varLayout 'realtimeDir'
    /// [0] - horizontalAxis (-1f to 1f)
    /// [1] - verticalAxis   (-1f to 1f)
    /// </summary>
    public float[] realtimeDir = new float[2];

    [Header("GO References")]
    public Vector3 camOffset;
    public Vector3 camDir;
    public float sensitivity = 0.5f; 
    public GameObject playerCam; 

    [Header("Component References")]
    public Rigidbody rb;

    /* Constants. */
    private const float CROUCH_SPEED_MULTIPLIER   = 0.2800f;
    private const float SNEAK_SPEED_MULTIPLIER    = 0.4500f;
    private const float DIAGONAL_SPEED_MULTIPLIER = 0.7000f;

    private const float MOMENTUM_BUILDSPEED       = 0.0850f; // realtimeDir.
    private const float MOVESPEED_INFLUENCE       = 0.0035f; // realtimeDir.
    private const float APPROACH_VELOCITY_SPEED   = 0.1000f; // movespeed.

    private void Update()
    {
        // Player Camera.
        playerCam.transform.position = transform.position + camOffset;
        camDir.x -= Input.GetAxis("Mouse Y") * sensitivity;
        camDir.y += Input.GetAxis("Mouse X") * sensitivity;
        playerCam.transform.eulerAngles = camDir;

        // Jumping.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector3(0f, jumpForce, 0f), ForceMode.Impulse);
        }

        // Update isGrounded.
        int groundsDetected = DetectGround();
    }

    private void FixedUpdate()
    {
        // Lock the cursor.
        Cursor.lockState = CursorLockMode.Locked;

        // Retrieve and store directional input. 
        int horizontal = 0;
        int vertical = 0;
        if (Input.GetKey("w")) { vertical++; }
        if (Input.GetKey("a")) { horizontal--; }
        if (Input.GetKey("s")) { vertical--; }
        if (Input.GetKey("d")) { horizontal++; }
        realtimeDir[0] = realtimeDir[0] + (MOMENTUM_BUILDSPEED - moveSpeed * MOVESPEED_INFLUENCE) * (horizontal - realtimeDir[0]);
        realtimeDir[1] = realtimeDir[1] + (MOMENTUM_BUILDSPEED - moveSpeed * MOVESPEED_INFLUENCE) * (vertical - realtimeDir[1]);

        // Update playerState.
        playerState = 0;
        if (Input.GetKey(KeyCode.LeftControl)) { playerState = State.Crouching; }
        if (horizontal != 0 || vertical != 0)
        {
            if (Input.GetKey(KeyCode.LeftShift)) { playerState = State.Sneaking; }
            else if (playerState == State.Crouching) { playerState = State.CrabWalking; }
            else { playerState = State.Running; }
        }

        // Movement.
        if (playerState >= State.CrabWalking)
        {
            targetMoveSpeed = runSpeed;
            if      (playerState == State.CrabWalking) { targetMoveSpeed *= CROUCH_SPEED_MULTIPLIER; }
            else if (playerState == State.Sneaking)    { targetMoveSpeed *= SNEAK_SPEED_MULTIPLIER;  }
            bool movingDiagonal = (horizontal != 0 && vertical != 0);
            if (movingDiagonal) { targetMoveSpeed *= DIAGONAL_SPEED_MULTIPLIER; }
        }
        else
        {
            targetMoveSpeed = 0f;
        }
        moveSpeed = moveSpeed + APPROACH_VELOCITY_SPEED * (targetMoveSpeed - moveSpeed);
        Vector3 newPos = transform.position + playerCam.transform.TransformDirection(new Vector3(
                moveSpeed * realtimeDir[0] * Time.deltaTime,
                0f,
                moveSpeed * realtimeDir[1] * Time.deltaTime));
        rb.MovePosition(newPos);
    }

    /// <summary>
    /// Shoots raycasts below the player feet 
    /// and returns a ground collision count.
    /// </summary>
    private int DetectGround()
    {
        int rays = 3;
        float spacing = 0.25f;
        Vector3 shootPos = new Vector3(transform.position.x - spacing, transform.position.y, transform.position.z - spacing);

        for (int i = 0; i < rays; i++)
        {
            for (int j = 0; j < rays; j++)
            {
                Debug.DrawRay(shootPos, Vector3.down * 1f, Color.red);
                shootPos.x += spacing;
            }
            shootPos.x -= spacing * rays;
            shootPos.z += spacing;
        }

        return -1;
    }
}

/* REFERENCE LIST
1. Dragon Bot
https://assetstore.unity.com/packages/3d/characters/creatures/dragon-bot-181926

2. ...

3. ...
*/
