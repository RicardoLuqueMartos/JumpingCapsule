using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private bool isOnGround = false;
    [SerializeField] private bool isJumping = false;
    [SerializeField] private bool isDoubleJump = false;
    [SerializeField] private bool CanDoubleJump = false;
    [SerializeField] private float DoubleJumpDelay = 0.1f;
    [SerializeField] private Transform groundCheck;
    [Range(.3f, .6f)]
    [SerializeField] private float detectionRange = 0;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Rigidbody rigidBody;
    [SerializeField] private bool isSprinting = false;
    [Range(1f, 2f)]
    [SerializeField] private float sprintMulti = 1.5f;
    [Range(5f, 20f)]
    [SerializeField] private float speed = 10;
   
    [Range(100f, 500f)]
    [SerializeField] private float jumpForce = 10;
    [SerializeField] private float movementX;
    [SerializeField] private float movementZ;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private CameraOrbit cameraOrbit;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        cameraTransform = Camera.main.transform;
    }

    private void FixedUpdate()
    {          
        MovePlayer();        
    }
    
    bool IsGrounded()
    {
        return isOnGround = Physics.CheckSphere(groundCheck.position, detectionRange, groundLayer);
    }

    // Visualiser la zone de détection du sol dans l'éditeur
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isOnGround ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, detectionRange);
        }
    }

    public void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementZ = movementVector.y;
    }

    void MovePlayer()
    {
        float sprintValue = 1;
        if (isSprinting) sprintValue = sprintMulti;
        else sprintValue = 1;

        if (movementX != 0 || movementZ != 0)        
            CameraDirectionToPlayer();

        Vector3 movement = new Vector3(movementX, 0.0f, movementZ);

        float moveSpeed = speed;
        if (!isJumping && isSprinting) moveSpeed = speed * sprintValue;
        else if (isJumping) moveSpeed = speed * .5f;

        transform.Translate(movement * (Time.deltaTime * moveSpeed));
    }

    void CameraDirectionToPlayer()
    {
        cameraOrbit.PlayerMoveInCameraDirection();
    }

    void OnJump(InputValue value)
    {
        bool isGrounded = IsGrounded();

        if (isGrounded)
        {
            isJumping = false;
        }

        float val = value.Get<float>();
        if (val == 1 && (isGrounded && isJumping == false)
            || (isJumping && isDoubleJump == false))
        {
            if (!isJumping)
            {
                isJumping = true;
                CanDoubleJump = false;
                Invoke("InvokeCanDoubleJump", DoubleJumpDelay);
                DoJump();
            }
            else if (!isDoubleJump && CanDoubleJump)
            {
                DoJump(); 
                CanDoubleJump = false;
            }
        }
    }

    void InvokeCanDoubleJump()
    {
        CanDoubleJump = true;
    }

    void DoJump()
    {
        rigidBody.AddForce(Vector3.up * jumpForce);
    }

    void OnSprint(InputValue value)
    {
        float val = value.Get<float>();
        if (val == 1)
            isSprinting = true;

        else
            isSprinting = false;
    }
}
