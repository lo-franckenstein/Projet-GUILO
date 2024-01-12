using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovment : MonoBehaviour
{
    [Header("Movment")]
    public float walkSpeed;
    public float runspeed;
    public float moveSpeed;

    public float groundDrag;

    public float jumpForce;
    public float jumpCouldown;
    public float airMultiplier;
    bool readyToJump;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode runkey = KeyCode.LeftShift;

    [Header("GroundCheck")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.freezeRotation = true;

        readyToJump = true;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        isrunning();
        myINput();
        speedControl();

        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
        MovePlayer();
    }

    private void myINput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //when to jump
        if (Input.GetKeyDown(jumpKey))
        {
            readyToJump = false;

            jump();

            Invoke(nameof(ResetJump), jumpCouldown);
        }
    }
    private void MovePlayer()
    {
        // Obtenez la direction avant de la caméra sans composante verticale
        Vector3 cameraForward = Vector3.Scale(orientation.forward, new Vector3(1, 0, 1)).normalized;

        // Calculez le vecteur de déplacement en utilisant la direction de la caméra
        Vector3 moveDirection = cameraForward * verticalInput + orientation.right * horizontalInput;

        // Appliquer le mouvement uniquement si le joueur est au sol
        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }
    private void speedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //limit velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }

    }
    private void jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    private void ResetJump()
    {
        readyToJump = true;
    }

    private void isrunning()
    {
        if (Input.GetKey(runkey))
        {
            moveSpeed = runspeed;
        }
        else 
        {
            moveSpeed = walkSpeed;
        }
    }
}





