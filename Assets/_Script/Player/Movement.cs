using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    //[Header("Components")]
    CharacterController characterController;
    Forces forces;

    [SerializeField] PlayerStats profile;

    Vector3 horizontalVelocity = Vector3.zero;
    Vector3 verticalVelocity = Vector3.zero;
    Vector3 wallNormal = Vector3.zero;
    bool isGrounded;
    bool wasGrounded;
    bool isJumpOnCooldown;
    bool isWallJumpOnCooldown;
    public bool isOnWall;
    bool canWallJump; //probably redundant but may have future use
    float airTime;
    RaycastHit hit;

    public float GetAirTime() => airTime;
    public bool GetIsGrounded() => isGrounded;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        forces = GetComponent<Forces>();
    }

    private void OnEnable()
    {
        InputManager.inputJump += Jump;
    }

    private void OnDisable()
    {
        InputManager.inputJump -= Jump;
    }

    private void OnDrawGizmos()
    {
        if(profile == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position + profile.offset, profile.radius);
    }

    private void Update()
    {
        //check Groudning. Do this first before airtime gets cutdown
        CheckGrounded();
        CheckOnWall();

        if (isGrounded)
        {
            horizontalVelocity = Vector3.zero;
            horizontalVelocity += transform.right * InputManager.inputMove.x;
            horizontalVelocity += transform.forward * InputManager.inputMove.y;
            horizontalVelocity *= profile.speed;
        }
        else
        {
            Vector3 airVelocity = horizontalVelocity - (horizontalVelocity * profile.airDrag * Time.deltaTime);
            airVelocity += transform.right * InputManager.inputMove.x * profile.airSpeed;
            airVelocity += transform.forward * InputManager.inputMove.y * profile.airSpeed;
            horizontalVelocity = Vector3.ClampMagnitude(airVelocity, profile.speed);
        }

        verticalVelocity = characterController.velocity.y > -0.001f ? Physics.gravity : Vector3.ClampMagnitude(verticalVelocity + Physics.gravity * Time.deltaTime, profile.terminalGravity);

        //move
        //characterController.Move(horizontalVelocity * Time.deltaTime);
        forces.AddForceConstant(horizontalVelocity);
        forces.AddForceConstant(verticalVelocity);
    }

    void Jump()
    {
        if (!isWallJumpOnCooldown && canWallJump)
        {
            WallJump();
        }

        if (airTime > profile.coyoteTime || isJumpOnCooldown)
            return;
        
        GroundedJump();
    }

    IEnumerator JumpCooldown()
    {
        isJumpOnCooldown = true;
        yield return new WaitForSeconds(profile.jumpCooldown);
        isJumpOnCooldown = false;
    }

    IEnumerator WallJumpCooldown()
    {
        isWallJumpOnCooldown = true;
        yield return new WaitForSeconds(profile.wallJumpCooldown);
        isWallJumpOnCooldown = false;
    }

    private void GroundedJump()
    {
        StartCoroutine(JumpCooldown());

        isGrounded = false;
        forces.AddForce(profile.jumpForce);
    }

    private void WallJump()
    {
        StartCoroutine(WallJumpCooldown());

        canWallJump = false;
        isOnWall = false;

        //Add horizontal force to vertical jump force
        Vector3 bounceVec = wallNormal * profile.wallJumpForce + profile.jumpForce.force;
        Forces.Force wallJumpForce = new Forces.Force(bounceVec, profile.jumpForce.drag, profile.jumpForce.time);

        forces.AddForce(wallJumpForce);
        Debug.Log("Wall Jump");
    }

    void AirbornTrigger()
    {
        //AIRBORN STATE IN HERE
    }

    void LandingTrigger()
    {
        //LANDING STATE IN HERE
        airTime = 0;
    }

    void AirbornUpdate()
    {
        //AIRBORN TICK STUFF HERE
        airTime += Time.deltaTime;

    }

    void GroundedUpdate()
    {
        //GROUNDED TICK STUFF HERE

    }

    void CheckGrounded()
    {
        //SET GROUNDED STATE HERE
        if(airTime > profile.jumpCooldown)
            isGrounded = Physics.CheckSphere(transform.position + profile.offset, profile.radius, profile.groundingLayers);

        if(!isGrounded && wasGrounded)
        {
            AirbornTrigger();
            wasGrounded = false;
            return;
        }

        if (isGrounded && !wasGrounded)
        {
            LandingTrigger();
            wasGrounded = true;
            return;
        }

        if (isGrounded)
            GroundedUpdate();
        else
            AirbornUpdate();
    }

    private void CheckOnWall()
    {
        if (isGrounded)
        {
            isOnWall = false;
            canWallJump = false;
            return;
        }

        // check four directions rather than sphere cast to get wall normal
        Vector3[] wallCheckDirections = 
        {
            transform.right,
            -transform.right,
            transform.forward,
            -transform.forward
        };

        foreach(Vector3 direction in wallCheckDirections)
        {
            if (Physics.Raycast(transform.position, direction, out hit, profile.radius + 0.25f, profile.wallLayer))
            {
                wallNormal = hit.normal;
                isOnWall = true;
                break;
            }
        }

        if(isOnWall)
        {
            canWallJump = true;
        }
        else
        {
            canWallJump = false;
        }
    }
}