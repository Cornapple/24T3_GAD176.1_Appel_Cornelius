//this movement script was greatly devised using the following youtube tutorial
// the last function is made by me
//in. (2022, February 7). FIRST PERSON MOVEMENT in 10 MINUTES - Unity Tutorial. YouTube. https://www.youtube.com/watch?v=f473C43s8nE



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    public Transform orientation;

    [Header("Ground Check")]
    public float playerHeight;
    public float groundDrag;

    public LayerMask whatIsGround;
    bool grounded;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        MyInput();
        PlayerQuickTurn();

        if (grounded)
            rb.drag = groundDrag;
        else
            rb.drag = 0;
    }

    private void FixedUpdate()
    {
        MovePlayer();
        SpeedControl();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }


    // the quick turn part of the first person movement script was made by me to dmeonstrate use of angles

    public void PlayerQuickTurn()
    {
        Debug.Log("player quick turn is called");
        if (Input.GetKey(KeyCode.Q))
        {
            transform.rotation = Quaternion.Euler (0f, 180f * Time.deltaTime, 0f); //demonstrates use of angles
        }
    }
}
