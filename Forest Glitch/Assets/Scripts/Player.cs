using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Attributes")]
    public float runSpeed = 15f; 
    private float moveSpeed;
    public float jumpForce = 5f;
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
    private const float CROUCH_SPEED_MULTIPLIER   = 0.38f;
    private const float SNEAK_SPEED_MULTIPLIER    = 0.65f;
    private const float DIAGONAL_SPEED_MULTIPLIER = 0.70f;

    private const float MOMENTUM_BUILDSPEED       = 0.05f;

    private void Update()
    {
        // Lock the cursor.
        Cursor.lockState = CursorLockMode.Locked;

        // Retrieve and store directional input. 
        int horizontal = 0; 
        int vertical   = 0;
        if (Input.GetKey("w")) { vertical++;   }
        if (Input.GetKey("a")) { horizontal--; }
        if (Input.GetKey("s")) { vertical--;   }
        if (Input.GetKey("d")) { horizontal++; }
        realtimeDir[0] = realtimeDir[0] + MOMENTUM_BUILDSPEED * (horizontal - realtimeDir[0]);
        realtimeDir[1] = realtimeDir[1] + MOMENTUM_BUILDSPEED * (vertical   - realtimeDir[1]);

        // Update playerState.
        playerState = 0;
        if (Input.GetKey(KeyCode.LeftControl)) { playerState = State.Crouching; }
        if (horizontal != 0 || vertical != 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))     { playerState = State.Sneaking;    }
            else if (playerState == State.Crouching) { playerState = State.CrabWalking; }
            else                                     { playerState = State.Running;     }
        }
        
        // Player Camera.
        playerCam.transform.position = transform.position + camOffset;
        camDir.x -= Input.GetAxis("Mouse Y") * sensitivity;
        camDir.y += Input.GetAxis("Mouse X") * sensitivity;
        playerCam.transform.eulerAngles = camDir;

        // Movement.
        if (playerState >= State.CrabWalking)
        {
            moveSpeed = runSpeed;
            if      (playerState == State.CrabWalking) { moveSpeed *= CROUCH_SPEED_MULTIPLIER; }
            else if (playerState == State.Sneaking)    { moveSpeed *= SNEAK_SPEED_MULTIPLIER;  }
            bool movingDiagonal = (horizontal != 0 && vertical != 0);
            if (movingDiagonal) { moveSpeed *= DIAGONAL_SPEED_MULTIPLIER; }

            Vector3 newPos = transform.position + playerCam.transform.TransformDirection(new Vector3(
                moveSpeed * horizontal * Time.deltaTime,
                0f,
                moveSpeed * vertical   * Time.deltaTime));
            rb.MovePosition(newPos);
        }

        // Jumping.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(new Vector3(0f, jumpForce, 0f), ForceMode.Impulse);
        }
    }
}
