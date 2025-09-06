using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    //[Header("Components")]
    CharacterController characterController;
    Forces forces;

    [SerializeField] PlayerStats profile;

    Vector3 horizontalVelocity = Vector3.zero;
    Vector3 verticalVelocity = Vector3.zero;
    public bool isGrounded;
    bool wasGrounded;
    bool isCrouching;
    bool isSliding;
    float airTime;

    public float GetAirTime() => airTime;
    public bool GetIsGrounded() => isGrounded;
    public void SetIsSliding(bool _isSliding) => isSliding = _isSliding;
    public void SetIsCrouching(bool _isCrouching) => isCrouching = _isCrouching;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        forces = GetComponent<Forces>();
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

        if (isSliding && isGrounded)
        {
            horizontalVelocity = Vector3.zero;
        }
        else if (isGrounded)
        {
            horizontalVelocity = Vector3.zero;
            horizontalVelocity += transform.right * InputManager.inputMove.x;
            horizontalVelocity += transform.forward * InputManager.inputMove.y;
            horizontalVelocity *= isCrouching ? profile.crouchSpeed : profile.speed;
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
        //if(airTime > profile.jumpCooldown)
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
}